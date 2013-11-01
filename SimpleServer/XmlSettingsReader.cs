using System;
using System.Linq;
using System.Runtime.Remoting;
using System.Xml.Linq;
using NLog;

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

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Method to read settings from config file in XML format
        /// </summary>
        /// <param name="path">Path to config file</param>
        /// <returns>Instance of ServerSettings class</returns>
        public static ServerSettings ReadSettings(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                logger.Warn("Recieved Null or Empty string as path to settings file");
                return null;
            }
                
            //TODO: think about IDisposable
            var xmlReader = XDocument.Load(path);
            ServerSettings settings;
            try
            {
                settings = new ServerSettings
                {
                    Port = (int)xmlReader.Descendants(PortName).Single(),
                    UseSqlDb = (bool)xmlReader.Descendants(UseSqlDbName).Single(),
                    PathToSqlFile = (string)xmlReader.Descendants(PathToSqlFileName).Single(),
                    PathToXmlFile = (string)xmlReader.Descendants(PathToXmlFileName).Single()
                };
            }
            catch (InvalidOperationException)
            {
                logger.Error("Can't read settings file {0}", path);
                throw new SimpleServerException {ExceptionMessage = String.Format("Can't read settings file {0}",path)};
            }

            logger.Info("Succesfully read settings file. {0}{1}",Environment.NewLine, settings.ToString());
            return settings;
        }
    }
}
