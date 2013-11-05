using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer.Helpers;

namespace SimpleServerTest.RouteHandlers.Test
{
    [TestClass]
    public class ProxyRouteHandler
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
        public void GetProxyRouteHandlerTest()
        {
            var webRequest = TestHelpers.CreateWebRequest(ServerHelpers.GetMethod, "/proxy/?url=http://ya.ru");

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
                Assert.IsTrue(TestHelpers.GetResponseBody(webResponse).Contains("yandex"));                
            }     
        }
    }
}
