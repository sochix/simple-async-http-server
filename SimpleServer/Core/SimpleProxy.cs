using System;
using System.IO;
using System.Net;
using System.Text;
using NLog;
using SimpleServer.Helpers;

namespace SimpleServer.Core
{
    public class SimpleProxy
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public string Proxify(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                //TODO: should add a validation for params
                logger.Info("Handling proxy get request with url = {0}", url);
                return GetUrlData(url);
            }

            logger.Warn("Handling proxy get request with empty url.");

            return ""; //TODO: this means error
        }

        private string GetUrlData(string host)
        {
            //TODO: check url
            var webRequest = WebRequest.Create(host);
            webRequest.Method = ServerHelpers.GetMethod;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.ContentLength = 0;

            string result;
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (var bodyStream = webResponse.GetResponseStream())
                {
                    if (bodyStream == null)
                    {
                        throw new SimpleServerException
                        {
                            ExceptionMessage = "Can't get body from response"
                        };
                    }
                    using (var streamReader = new StreamReader(bodyStream, Encoding.UTF8))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}
