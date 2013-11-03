using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NLog;
using SimpleServer.Helpers;
using SimpleServer.Persisters;
using SimpleServer.RouteHandlers;

namespace SimpleServer.Core
{
    /// <summary>
    /// Wrapper around System.Net.HttpListener
    /// </summary>
    internal sealed class LocalHttpListener
    {
        private readonly HttpListener httpListener;
        private readonly IPersister persister;
        private readonly IList<Route> routeList;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Default .ctor of LocalHttpListener
        /// </summary>
        /// <param name="port">Port to listen</param>
        /// <param name="url">Url to listen</param>
        /// <param name="persister">Persister to use by route handlers</param>
        /// <param name="routeList">List of route handlers</param>
        public LocalHttpListener(int port, string url, IPersister persister, IList<Route> routeList)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(String.Format("{0}:{1}/", url, port));
            logger.Info("Server url set to {0}:{1}", url, port);
            this.persister = persister;
            this.routeList = routeList;
        }

        /// <summary>
        /// Finalizer to correctly close httpListener during garbage collection
        /// </summary>
        ~LocalHttpListener()
        {
            httpListener.Stop();
            httpListener.Close();
            logger.Trace("HttpListener stopped");
        }

        /// <summary>
        /// Start listening requests
        /// </summary>
        public void Start()
        {
            httpListener.Start();
            logger.Trace("HttpListener started");

            while (httpListener.IsListening)
            {
                ProcessRequest();
            }
        }
        
        private void ProcessRequest()
        {
            var result = httpListener.BeginGetContext(ListenerCallback, httpListener);
            result.AsyncWaitHandle.WaitOne(ServerHelpers.Timeout);
            result.AsyncWaitHandle.Close();
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (httpListener == null) return;

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
            //TODO: if body == null return default page
            body = body ?? DefaultRouteHandler.GetDefaultResponseView;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = HttpStatusCode.OK.ToString();
            response.ContentType = "text/html";
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            response.ContentLength64 = buffer.Length;
            //Ofcourse, we can do here async write, but is it really needed for demo application?
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
