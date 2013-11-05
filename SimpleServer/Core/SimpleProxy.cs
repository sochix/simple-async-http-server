using System;
using System.IO;
using System.Net;
using System.Text;
using NLog;
using SimpleServer.Helpers;

namespace SimpleServer.Core
{
    /// <summary>
    /// Simple synchronius proxy 
    /// </summary>
    internal sealed class SimpleProxy
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
       
        /// <summary>
        /// Proxify a given url
        /// </summary>
        /// <param name="url">Url to proxify</param>
        /// <returns>String contains a url response</returns>
        public string Proxify(string url)
        {
            return GetUrlData(url);            
        }

        private static string GetUrlData(string host)
        {
            WebRequest webRequest = null;
            try
            {
                webRequest = WebRequest.Create(host);
                webRequest.Method = ServerHelpers.GetMethod;
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                webRequest.ContentLength = 0;
            }
            catch (UriFormatException e)
            {
                logger.ErrorException("Wrong url requested for proxifying", e);
                return null;
            }
            catch (ArgumentNullException e)
            {
                logger.ErrorException("Empty url requested for proxifying", e);
                return null;
            }
            catch (Exception e)
            {
                logger.ErrorException("Internal proxy error", e);
                return null;
            }

            logger.Info("Handling proxy get request with url = {0}", host);
            string result;

            try
            {

                using (var webResponse = (HttpWebResponse) webRequest.GetResponse())
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
            }
            catch (WebException)
            {
                logger.Error("Internal error during proxyfying");
                result = null;
            }

            return result;
        }
    }
}
