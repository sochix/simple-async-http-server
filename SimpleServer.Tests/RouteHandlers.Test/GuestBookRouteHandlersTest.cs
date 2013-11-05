using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer.Helpers;
using SimpleServer.RouteHandlers;

namespace SimpleServerTest.RouteHandlers.Test
{
    [TestClass]
    public class GuestBookRouteHandlersTest
    {
        private SimpleServer.Core.SimpleServer server;
        [TestInitialize]
        public void TestInitialize()
        {
            server = new SimpleServer.Core.SimpleServer();
            server.Start();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            server.Stop();
        }

        [TestMethod]
        public void GetGuestBookRecordsTest()
        {
            var webRequest = TestHelpers.CreateWebRequest(ServerHelpers.GetMethod, "/guestbook/");

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
                Assert.IsTrue(TestHelpers.GetResponseBody(webResponse).Contains("All records of guest book"));
            }       
        }

        [TestMethod]
        public void AddGuestBookRecordTest()
        {
            var webRequest = TestHelpers.CreateWebRequest(ServerHelpers.PostMethod, "/guestbook/?user=Test&?message=test");

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
                Assert.IsTrue(TestHelpers.GetResponseBody(webResponse).Contains("Thank"));
            }    
        }

    }
}
