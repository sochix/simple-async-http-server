using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SimpleServer.Persisters
{
    internal class XmlDataPersister: IPersister
    {
        private readonly string pathToSave;
        private readonly XDocument document;
        public XmlDataPersister(string path)
        {
            //TODO: check path for null or empty
            //TODO; remove try, use Exist
            try
            {
                document = XDocument.Load(path);
            }
            catch (FileNotFoundException)
            {
                document = new XDocument();
                InitializeFile(path);
            }
            
            pathToSave = path; 
        }

        public void AddRecord(Tuple<string, string> record)
        {
            var newRecord = new XElement("record",
                new XAttribute("user", record.Item1),
                new XAttribute("message", record.Item2));    

            document.Root.Add(newRecord);
            document.Save(pathToSave);
        }

        public IEnumerable<Tuple<string, string>> GetAllRecords()
        {
            var result = new List<Tuple<string, string>>();

            foreach (XElement el in document.Root.Elements())
            {
                var user = el.Attribute("user").Value;
                var message = el.Attribute("message").Value;
	            Console.WriteLine("{0} {1}", user, message);
                result.Add(new Tuple<string, string>(user, message));
            }

            return result;
        }

        private void InitializeFile(string path)
        {
            var library = new XElement("records");
            document.Add(library);
            document.Save(path);
        }
    }
}
