using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using SimpleServer.Helpers;
using SimpleServer.Persisters;
using SimpleServer.RouteHandlers;

namespace SimpleServer.Core
{
    public class SimpleServer
    {
        private readonly LocalHttpListener httpListener;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SimpleServer()
        {
            try
            {
                var settings = XmlSettingsReader.ReadSettings(ServerHelpers.DefaultSettingsPath);

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
                                    Action = (v, p) =>
                                             {
                                                 return new SimpleProxy().Proxify(v["url"]);
                                             }
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

                httpListener = new LocalHttpListener(settings.Port, ServerHelpers.Url, persister, routeList);
                logger.Trace("Initialized httpListener");
            }
            catch (Exception)
            {
                Environment.Exit(-1);
            }
        }

        public void Start()
        {
            Task.Factory.StartNew(httpListener.Start);
            logger.Info("Server started");
        }

        public void Stop()
        {
            //TODO: think about multi threading
            httpListener.Stop();
            logger.Info("Server stopped");
        }
    }
}
