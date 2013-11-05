using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer.Helpers;
using SimpleServer.RouteHandlers;

namespace SimpleServerTest.RouteHandlers.Test
{
    [TestClass]
    public class DefaultRouteHandlerTest
    {
        private SimpleServer.Core.SimpleServer server;
        [TestInitialize]
        public void TestInitialize()
        {
            server = new SimpleServer.Core.SimpleServer("Files\\server_config.xml");
            server.Start();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            server.Stop();
        }

        [TestMethod]
        public void GetDefaultRouteTest()
        {
            var webRequest = TestHelpers.CreateWebRequest(ServerHelpers.GetMethod, "/");

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
                Assert.AreEqual(DefaultRouteHandler.GetDefaultResponseView, TestHelpers.GetResponseBody(webResponse));
            }            
        }

        [TestMethod]
        public void PostDefaultRouteTest()
        {
            var webRequest = TestHelpers.CreateWebRequest(ServerHelpers.PostMethod, "/");

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
                Assert.AreEqual(DefaultRouteHandler.GetDefaultResponseView, TestHelpers.GetResponseBody(webResponse));
            }
        }
    }
}
