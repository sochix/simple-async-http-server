using System;
using System.Linq;
using System.Xml.Linq;

namespace SimpleServer
{
    /// <summary>
    /// Static class for reading settings from config file in XML format
    /// </summary>
    internal static class XmlSettingsReader
    {
        private const string PortName = "port";
        private const string PathToXmlFileName = "pathToXmlFile";
        private const string UseSqlDbName = "useSqlDb";
        private const string PathToSqlFileName = "pathToSqlFile";

        /// <summary>
        /// Method to read settings from config file in XML format
        /// </summary>
        /// <param name="path">Path to config file</param>
        /// <returns>Instance of ServerSettings class</returns>
        public static ServerSettings ReadSettings(string path)
        {
            if (String.IsNullOrEmpty(path))
                return null;

            //TODO: add exception handling
            var xmlReader = XDocument.Load(path);

            return new ServerSettings
                    {
                        Port = (int)xmlReader.Descendants(PortName).Single(),
                        UseSqlDb = (bool)xmlReader.Descendants(UseSqlDbName).Single(),
                        PathToSqlFile = (string)xmlReader.Descendants(PathToSqlFileName).Single(),
                        PathToXmlFile = (string)xmlReader.Descendants(PathToXmlFileName).Single()
                    };
        }
    }
}
