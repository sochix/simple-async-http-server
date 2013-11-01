using System;
using System.Net;
using System.Text;

namespace SimpleServer
{
    internal class ServerSettings
    {
        public bool UseSqlDb { get; set; }
        public int Port { get; set; }
        public string PathToXmlFile { get; set; }
        public string PathToSqlFile { get; set; }
    }

    public static class ServerHelpers
    {
        private const int Port = 8888;
        public const string Url = "http://127.0.0.1";

        public const string PostMethod = "POST";
        public const string GetMethod = "GET";
        public const string GuestBookName = "GuestBook";
        public const string ProxyName = "Proxy";
        public const string SettingFilePath = "";
        public const string DefaultSettingsPath = "server_config.xml";

        public static string DefaultUrl
        {
            get { return String.Format("{0}:{1}/", Url, Port); }
        }
    }

    public class SimpleServerException : Exception
    {
        public string ExceptionMessage { get; set; }
    }

    public class WebRequestInfo
    {
        public string Body { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public string HttpMethod { get; set; }
        public Uri Url { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("HttpMethod {0}", HttpMethod));
            sb.AppendLine(string.Format("Url {0}", Url));
            sb.AppendLine(string.Format("ContentType {0}", ContentType));
            sb.AppendLine(string.Format("ContentLength {0}", ContentLength));
            sb.AppendLine(string.Format("Body {0}", Body));
            return sb.ToString();
        }
    }

    public class WebResponseInfo
    {
        public string Body { get; set; }
        public string ContentEncoding { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("StatusCode {0} StatusDescripton {1}", StatusCode, StatusDescription));
            sb.AppendLine(string.Format("ContentType {0} ContentEncoding {1} ContentLength {2}", ContentType, ContentEncoding, ContentLength));
            sb.AppendLine(string.Format("Body {0}", Body));
            return sb.ToString();
        }
    }
}
