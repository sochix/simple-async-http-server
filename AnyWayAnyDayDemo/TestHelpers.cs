using System;
using System.IO;
using System.Net;
using System.Text;
using SimpleServer.Helpers;

namespace SimpleServerTest
{
    public static class TestHelpers
    {
        public static WebRequest CreateWebRequest(string methodType, string absolutePath = "")
        {
            var webRequest = WebRequest.Create(String.Format("{0}:{1}/{2}/", ServerHelpers.Url,8888, absolutePath));
            webRequest.Method = methodType;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.ContentLength = 0;

            return webRequest;
        }

        public static string GetResponseBody(HttpWebResponse response)
        {
            var result = "";
            using (var bodyStream = response.GetResponseStream())
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

            return result;
        }
    }
}
