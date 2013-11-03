using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NLog;
using SimpleServer.Helpers;
using SimpleServer.Persisters;

namespace SimpleServer.Core
{
    /// <summary>
    /// Wrapper around System.Net.HttpListener
    /// </summary>
    internal class LocalHttpListener
    {
        private readonly HttpListener httpListener;
        private readonly string defaultResponseText;
        private readonly IPersister persister;
        private readonly IList<Route> routeList;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public LocalHttpListener(int port, string url, IPersister persister, IList<Route> routeList)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(String.Format("{0}:{1}/", url, port));
            logger.Info("Server port url set to {0}:{1}", url, port);
            this.persister = persister;
            this.routeList = routeList;
        }

        public void Start()
        {
            httpListener.Start();
            logger.Trace("HttpListener started");

            while (httpListener.IsListening)
            {
                ProcessRequest();
            }                
        }

        public void Stop()
        {
            //TODO: think better about threads while stopping it
            httpListener.Stop();
            logger.Trace("HttpListener stopped");
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
            var method = request.HttpMethod.ToUpper();
            var url = request.Url.AbsolutePath.ToUpper();
            var values = ServerHelpers.ParseQueryString(request.Url.ToString());

            foreach (var route in routeList.Where(route => route.Method.ToUpper().Equals(method) && url.StartsWith(route.Url.ToUpper())))
            {
                logger.Info("Method: {0}, Url: {1}, Handler: {2}", method, url, route.Description);
                return route.Action(values, persister);
            }

            logger.Warn("No handler specified for method {0} at url {1}", method, url);
            return null; //TODO: this means error
        }

        private static void CreateResponse(HttpListenerResponse response, string body)
        {
            response.StatusCode = (int) HttpStatusCode.OK;
            response.StatusDescription = HttpStatusCode.OK.ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
