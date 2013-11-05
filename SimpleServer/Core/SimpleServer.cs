using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NLog;
using SimpleServer.Helpers;
using SimpleServer.Persisters;
using SimpleServer.RouteHandlers;

namespace SimpleServer.Core
{
    /// <summary>
    /// Server class that hides all complexity from end user
    /// </summary>
    public sealed class SimpleServer
    {
        private readonly LocalHttpListener httpListener;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool isAlreadyStarted = false;
        private readonly SimpleProxy proxy;
        
        /// <summary>
        /// Default server .ctor
        /// </summary>
        public SimpleServer(string path = null)
        {
            path = path ?? ServerHelpers.DefaultSettingsPath;

            try
            {
                //read settings
                var settings = XmlSettingsReader.ReadSettings(path);

                //set persister
                IPersister persister = null;
                if (settings.UseSqlDb)
                {
                    persister = new SqlDataPersister(settings.PathToSqlFile);
                    logger.Info("Server using SQL database, located at {0}", settings.PathToSqlFile);
                }
                else
                {
                    persister = new XmlDataPersister(settings.PathToXmlFile);
                    logger.Info("Server using XML database, located at {0}", settings.PathToXmlFile);
                }

                //create proxy
                proxy = new SimpleProxy();

                //set routes
                var routeList = new List<Route>
                            {
                                new Route
                                {
                                    Description = "Get all records from guestbook",
                                    Method = ServerHelpers.GetMethod,
                                    Url = "/" + ServerHelpers.GuestBookName + "/",
                                    Action = GuestBookRouteHandler.GetAllRecords
                                },
                                new Route
                                {
                                    Description = "Return a proxified page",
                                    Method = ServerHelpers.GetMethod,
                                    Url = "/" + ServerHelpers.ProxyName + "/",
                                    Action = (v, p) => proxy.Proxify(v["url"])
                                },
                                new Route
                                {
                                    Description = "Add new guestbook record",
                                    Method = ServerHelpers.PostMethod,
                                    Url = "/" + ServerHelpers.GuestBookName + "/",
                                    Action = GuestBookRouteHandler.AddRecord
                                },
                                new Route
                                {
                                    Description = "Default route for all POST requests",
                                    Method = ServerHelpers.PostMethod,
                                    Url = "/",
                                    Action = DefaultRouteHandler.DefaultRoute
                                },
                                new Route
                                {
                                    Description = "Default route for all GET requests",
                                    Method = ServerHelpers.GetMethod,
                                    Url = "/",
                                    Action = DefaultRouteHandler.DefaultRoute
                                }                              
                            };

                //create httpListener
                httpListener = new LocalHttpListener(settings.Port, ServerHelpers.Url, persister, routeList);
                logger.Trace("Initialized httpListener");
            }
            catch (Exception)
            {
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Start server
        /// </summary>
        public void Start()
        {
            if (isAlreadyStarted) return;

            Task.Factory.StartNew(httpListener.Start, TaskCreationOptions.LongRunning);
            isAlreadyStarted = true;
            logger.Info("Server started");
        }

        /// <summary>
        /// Stop server
        /// </summary>
        public void Stop()
        {
            //we don't need to wait until httpListener thread will ends, because it longRunning task. And it will close httpListener in finalizer, can't imagine smthng better now
            if (!isAlreadyStarted) return;
            
            isAlreadyStarted = false;
            logger.Info("Server stopped");
        }
    }
}
