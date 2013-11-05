using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using NLog;
using SimpleServer.Helpers;

namespace SimpleServer.Persisters
{
    /// <summary>
    /// SQL database persister
    /// </summary>
    internal class SqlDataPersister: IPersister
    {
        private readonly string dbConnectionString;
        private const string UserTable = "CREATE TABLE users (id INTEGER PRIMARY KEY ASC, user VARCHAR(256))";
        private const string MessageTable = "CREATE TABLE messages (id INTEGER PRIMARY KEY ASC, message VARCHAR(256))";
        private const string GetAllRecordsQuery = "SELECT user, message FROM users u LEFT JOIN  messages m ON u.id = m.id";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SqlDataPersister(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                logger.Error("Provided empty path to SQL database file");
                throw new SimpleServerException
                      {
                          ExceptionMessage = "Provided empty path to SQL database file"
                      };
            }
            
            dbConnectionString = String.Format("Data Source={0};Version=3;", path);
            if (File.Exists(path))
            {
                try
                {
                    using (var c = new SQLiteConnection(dbConnectionString))
                    {
                        c.Open();                     
                    }
                }
                catch (Exception e)
                {
                    logger.Error("Internal error during opening SQL database at path {0}. Exception message: {1}",path, e.Message);
                    throw new SimpleServerException()
                    {
                        ExceptionMessage = String.Format("Internal error during opening SQL database at path {0}", path)
                    };
                }
                return;
            }

            try
            {
                SQLiteConnection.CreateFile(path);
                using (var c = new SQLiteConnection(dbConnectionString))
                {
                    c.Open();
                    using (var cmd = new SQLiteCommand(String.Format("{0};{1}", UserTable, MessageTable), c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("Internal error during initializing SQL database. Exception message: {0}", e.Message);
                throw new SimpleServerException()
                      {
                          ExceptionMessage = "Internal error during initializing SQL database"
                      };
            }
        }

        //TODO: too slow now, need to create bulk save method
        public void AddRecord(Tuple<string, string> record)
        {
            if (record == null) return;

            if (String.IsNullOrEmpty(record.Item1) || String.IsNullOrEmpty(record.Item2)) return;

            string addUser = String.Format("INSERT INTO users values (NULL, '{0}')", record.Item1);
            string addMessage = String.Format("INSERT INTO messages values (NULL, '{0}')", record.Item2);

            try
            {
                using (var c = new SQLiteConnection(dbConnectionString))
                {
                    c.Open();
                    using (var cmd = new SQLiteCommand(String.Format("{0};{1}", addUser, addMessage), c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("Can't add new record to SQL database. Exception message: {0}", e.Message);
                throw new SimpleServerException()
                {
                    ExceptionMessage = "Can't add new record to SQL database"
                };
            }
        }

        public IEnumerable<Tuple<string, string>> GetAllRecords()
        {
            var result = new List<Tuple<string, string>>();

            try
            {
                using (var c = new SQLiteConnection(dbConnectionString))
                {
                    c.Open();
                    using (var cmd = new SQLiteCommand(GetAllRecordsQuery, c))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                result.Add(new Tuple<string, string>((string) rdr[0], (string) rdr[1]));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("Can't get all records from SQL database. Exception message: {0}", e.Message);
                throw new SimpleServerException()
                {
                    ExceptionMessage = "Can't get all records from SQL database"
                };
            }
            return result;            
        }
    }
}
