using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NLog;
using SimpleServer.Helpers;

namespace SimpleServer.Persisters
{
    /// <summary>
    /// XML database persister
    /// </summary>
    internal class XmlDataPersister: IPersister
    {
        private readonly string pathToSave;
        private readonly XDocument document;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public XmlDataPersister(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                logger.Error("Provided empty path to XML database file");
                throw new SimpleServerException
                {
                    ExceptionMessage = "Provided empty path to XML database file"
                };
            }

            try
            {
                pathToSave = path;
                if (File.Exists(path))
                {
                    document = XDocument.Load(path);
                }
                else
                {
                    document = new XDocument();
                    var library = new XElement("records");
                    document.Add(library);
                    document.Save(path);
                }
            }
            catch (Exception e)
            {
                logger.Error("Internal error during initializing XML database. Exception message: {0}", e.Message);
                throw new SimpleServerException()
                {
                    ExceptionMessage = "Internal error during initializing XML database"
                };
            }
        }

        public void AddRecord(Tuple<string, string> record)
        {
            try
            {
                var newRecord = new XElement("record",
                    new XAttribute("user", record.Item1),
                    new XAttribute("message", record.Item2));

                document.Root.Add(newRecord);
                document.Save(pathToSave);
            }
            catch(Exception e)
            {
                logger.Error("Can't add new record to XML database. Exception message: {0}", e.Message);
                throw new SimpleServerException()
                      {
                          ExceptionMessage = "Can't add new record to XML database"
                      };
            }
        }

        public IEnumerable<Tuple<string, string>> GetAllRecords()
        {
            var result = new List<Tuple<string, string>>();

            try
            {
                foreach (XElement el in document.Root.Elements())
                {
                    var user = el.Attribute("user").Value;
                    var message = el.Attribute("message").Value;
                    result.Add(new Tuple<string, string>(user, message));
                }
            }
            catch (Exception e)
            {
                logger.Error("Can't get all records from XML database. Exception message: {0}", e.Message);
                throw new SimpleServerException()
                {
                    ExceptionMessage = "Can't get all records from XML database"
                };
            }

            return result;
        }
    }
}
