using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using SimpleServer.Persisters;

namespace SimpleServer
{
    /// <summary>
    /// Wrapper around System.Net.HttpListener
    /// </summary>
    internal class LocalHttpListener
    {
        private readonly HttpListener httpListener;
        private readonly string defaultResponseText;
        private readonly ServerSettings settings;
        private readonly IPersister persister;

        public LocalHttpListener()
        {
            try
            {
                settings = XmlSettingsReader.ReadSettings(ServerHelpers.DefaultSettingsPath);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Can't find settings file {0}", e.FileName);
                //TODO: here need to terminate server
            }

            using (StreamReader reader = File.OpenText("Views/DefaultResponse.html"))
            {
                defaultResponseText = reader.ReadToEnd();
            }

            httpListener = new HttpListener();
            httpListener.Prefixes.Add(String.Format("{0}:{1}/", ServerHelpers.Url, settings.Port));
            Console.WriteLine("Server initialized on port: {0}", settings.Port);

            if (settings.UseSqlDb)
            {
                persister = new SqlDataPersister("D:\\test.sqlite");
            }
            else
            {
                persister = new XmlDataPersister("D:\\test.xml");
            }
        }

        public LocalHttpListener(IPersister persister)
        {
            try
            {
                settings = XmlSettingsReader.ReadSettings(ServerHelpers.DefaultSettingsPath);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Can't find settings file {0}", e.FileName);
                //TODO: here need to terminate server
            }

            httpListener = new HttpListener();
            httpListener.Prefixes.Add(String.Format("{0}:{1}/", ServerHelpers.Url, settings.Port));
            Console.WriteLine("Server initialized on port: {0}", settings.Port);

            this.persister = persister;
        }

        public void Start()
        {
            httpListener.Start();
            Console.WriteLine("Server started");

            while (httpListener.IsListening)
                ProcessRequest();
        }

        public void Stop()
        {
            Console.WriteLine("Server stopping...");
            httpListener.Stop();            
        }

        private static void CreateResponse(HttpListenerResponse response, string body)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = HttpStatusCode.OK.ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        private void ProcessRequest()
        {
            var result = httpListener.BeginGetContext(ListenerCallback, httpListener);
            result.AsyncWaitHandle.WaitOne();
        }

        private void ListenerCallback(IAsyncResult result)
        {
            var context = httpListener.EndGetContext(result);
            CreateResponse(context.Response, HandleRequest(context.Request));
        }

        private string HandleRequest(HttpListenerRequest request)
        {
            if (request.HttpMethod.Equals(ServerHelpers.GetMethod))
            {
                if (request.Url.AbsolutePath.ToUpper().StartsWith("/" + ServerHelpers.GuestBookName.ToUpper() + "/"))
                {
                    const string header = "GUESTBOOK will be here. ";
                    var records = persister.GetAllRecords();
                    var sb = new StringBuilder();

                    sb.AppendLine(header);
                    foreach (var record in records)
                    {
                        sb.AppendLine(record.ToString());
                    }

                    return sb.ToString();
                }

                if (request.Url.AbsolutePath.ToUpper().StartsWith("/" + ServerHelpers.ProxyName.ToUpper() + "/"))
                {
                    var values = ParseQueryString(request.Url.ToString());

                    var url = values["url"];
                    if (!String.IsNullOrEmpty(url))
                    {
                        //TODO: should add a validation for params
                        Console.WriteLine("Proxy will be here. Url = {0}", url);
                        return GetUrlData(values["url"]);    
                    }
                    return defaultResponseText;
                }
            }

            if (request.HttpMethod.Equals(ServerHelpers.PostMethod))
            {
                if (request.Url.AbsolutePath.ToUpper().StartsWith("/" + ServerHelpers.GuestBookName.ToUpper() + "/"))
                {
                    var values = ParseQueryString(request.Url.ToString());

                    //TODO: we should add a validation for this params

                    string user = String.IsNullOrEmpty(values["user"]) ? null : values["user"];
                    string message = String.IsNullOrEmpty(values["message"]) ? null : values["message"];

                    if (user != null && message != null)
                    {
                        persister.AddRecord(new Tuple<string, string>(values["user"], values["message"]));    
                    }
                    
                    return String.Format("POST METHOD OF GUEST BOOK.");
                }
            }

            return defaultResponseText;
        }

        private static NameValueCollection ParseQueryString(string url)
        {
            var queryString = string.Join(string.Empty, url.Split('?').Skip(1));
            return HttpUtility.ParseQueryString(queryString);
        }

        private string GetUrlData(string host)
        {
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
