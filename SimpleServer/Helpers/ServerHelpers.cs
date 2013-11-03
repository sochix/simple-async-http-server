using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using SimpleServer.Persisters;

namespace SimpleServer.Helpers
{
    internal sealed class Route
    {
        public string Description { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public Func<NameValueCollection, IPersister, string> Action { get; set; }
    }

    internal sealed class ServerSettings
    {
        public bool UseSqlDb { get; set; }
        public int Port { get; set; }
        public string PathToXmlFile { get; set; }
        public string PathToSqlFile { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Port {0}", Port));
            sb.AppendLine(string.Format("UseSqlDb {0}", UseSqlDb));
            sb.AppendLine(string.Format("PathToXmlFile {0}", PathToXmlFile));
            sb.AppendLine(string.Format("PathToSqlFile {0}", PathToSqlFile));
            
            return sb.ToString();            
        }       
    }

    public static class ServerHelpers
    {
        public const string Url = "http://127.0.0.1";
        public const int Timeout = 1000;
        public const string PostMethod = "POST";
        public const string GetMethod = "GET";
        public const string GuestBookName = "GuestBook";
        public const string ProxyName = "Proxy";
        public const string DefaultSettingsPath = "server_config.xml";
        public const string BodyInjectionToken = "<--DATA-->";
        public const string DefaultResponseViewPath = "Views/DefaultResponse.html";
        public const string GuestBookRecordsViewPath = "Views/GuestBookRecords.html";
        public const string GuestBookThanksViewPath = "Views/ThankYouForRecord.html";

        /// <summary>
        /// Gets query string and parse it
        /// </summary>
        /// <param name="url">Query string</param>
        /// <returns>Collection of name and value</returns>
        public static NameValueCollection ParseQueryString(string url)
        {
            if (String.IsNullOrEmpty(url)) return null;

            var queryString = string.Join(string.Empty, url.Split('?').Skip(1));
            return HttpUtility.ParseQueryString(queryString);
        }

        public static string InjectDataToTheView(string data, string view)
        {
            var indexOfInjection = view.IndexOf(ServerHelpers.BodyInjectionToken);
            var pagePrefix = view.Substring(0, indexOfInjection);
            var pagePostfix = view.Substring(indexOfInjection + ServerHelpers.BodyInjectionToken.Length);
            
            var sb = new StringBuilder();
            return sb.AppendFormat("{0}{1}{2}", pagePrefix, data, pagePostfix).ToString();
        }
    }

    public sealed class SimpleServerException : Exception
    {
        public string ExceptionMessage { get; set; }

        public override string Message
        {
            get { return ExceptionMessage; }
        }
    }
}
