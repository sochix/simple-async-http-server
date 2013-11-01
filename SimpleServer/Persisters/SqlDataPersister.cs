using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleServer.Persisters
{
    internal class SqlDataPersister: IPersister
    {
        private readonly string dbConnectionString;

        public SqlDataPersister(string path)
        {
            dbConnectionString = String.Format("Data Source={0};Version=3;", path);
            if (File.Exists(path)) return;

            SQLiteConnection.CreateFile(path);

            const string userTable = "CREATE TABLE users (id INTEGER PRIMARY KEY ASC, user VARCHAR(256))";
            const string messageTable = "CREATE TABLE messages (id INTEGER PRIMARY KEY ASC, message VARCHAR(256))";

            using (var c = new SQLiteConnection(dbConnectionString))
            {
                c.Open();
                using (var cmd = new SQLiteCommand(String.Format("{0};{1}",userTable,messageTable), c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddRecord(Tuple<string, string> record)
        {
            string addUser = String.Format("INSERT INTO users values (NULL, '{0}')", record.Item1);
            string addMessage = String.Format("INSERT INTO messages values (NULL, '{0}')", record.Item2);

            using (var c = new SQLiteConnection(dbConnectionString))
            {
                c.Open();
                using (var cmd = new SQLiteCommand(String.Format("{0};{1}", addUser, addMessage), c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Tuple<string, string>> GetAllRecords()
        {
            string query = "SELECT user, message FROM users u LEFT JOIN  messages m ON u.id = m.id";

            var result = new List<Tuple<string, string>>();

            using (var c = new SQLiteConnection(dbConnectionString))
            {
                c.Open();
                using (var cmd = new SQLiteCommand(query, c))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new Tuple<string, string>((string)rdr[0], (string)rdr[1]));    
                        }
                    }
                }
            }
            return result;            
        }
    }
}
