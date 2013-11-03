using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NLog;
using SimpleServer.Helpers;

namespace SimpleServer.Core
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

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                
            var xmlReader = XDocument.Load(path);
            ServerSettings settings = null;
            try
            {
                settings = new ServerSettings
                           {
                               Port = (int) xmlReader.Descendants(PortName).Single(),
                               UseSqlDb = (bool) xmlReader.Descendants(UseSqlDbName).Single(),
                               PathToSqlFile = (string) xmlReader.Descendants(PathToSqlFileName).Single(),
                               PathToXmlFile = (string) xmlReader.Descendants(PathToXmlFileName).Single()
                           };
            }
            catch (InvalidOperationException e)
            {
                logger.Error("Can't parse settings file {0}. Exception message: {1}", path, e.Message);
                throw new SimpleServerException {ExceptionMessage = String.Format("Can't parse settings file {0}. Probably, file corrupted or you make a typo", path)};
            }
            catch (FileNotFoundException e)
            {
                logger.Error("Can't find settings file {0}. Exception message: {1}", path, e.Message);
                throw new SimpleServerException { ExceptionMessage = String.Format("Can't find settings file {0}", path) };
            }
            catch (Exception e)
            {
                logger.Error("Internal error during reading settings file {0}. Exception message: {1}",path, e.Message);
                throw new SimpleServerException { ExceptionMessage = String.Format("Internal error during reading settings file {0}", path) };
            }

            logger.Info("Succesfully read settings file");
            logger.Trace("Readed settings values is {0}{1}",Environment.NewLine, settings.ToString());
            return settings;
        }
    }
}
