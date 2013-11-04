using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer;

namespace SimpleServerTest
{
//    [TestClass]
//    public class RouteTest
//    {
//        [TestMethod]
//        public void DefaultRoutePostTest()
//        {
//            var server = new LocalHttpListener();
//            var task = Task.Factory.StartNew(server.Start);
//
//            var webRequest = CreateWebRequest(ServerHelpers.PostMethod);
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("TEST"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "asdasdasdasdasdasdasd");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("TEST"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "21312312123123123");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("TEST"));
//            }
//        }
//
//        [TestMethod]
//        public void ProxyRoutePostTest()
//        {
//            var server = new LocalHttpListener();
//            var task = Task.Factory.StartNew(server.Start);
//
//            var webRequest = CreateWebRequest(ServerHelpers.PostMethod, ServerHelpers.ProxyName);
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsFalse(GetResponseBody(webResponse).Contains("proxy"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "PROXY");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsFalse(GetResponseBody(webResponse).Contains("proxy"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "PrOxY");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsFalse(GetResponseBody(webResponse).Contains("proxy"));
//            }          
//        }
//
//        [TestMethod]
//        public void GuestBookRoutePostTest()
//        {
//            var server = new LocalHttpListener();
//            var task = Task.Factory.StartNew(server.Start);
//
//            var webRequest = CreateWebRequest(ServerHelpers.PostMethod, ServerHelpers.GuestBookName);
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("POST METHOD OF GUEST BOOK"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "GuEstBook");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("POST METHOD OF GUEST BOOK"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "guestbook");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("POST METHOD OF GUEST BOOK"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "GUESTBOOK");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("POST METHOD OF GUEST BOOK"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "guest-book");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsFalse(GetResponseBody(webResponse).Contains("POST METHOD OF GUEST BOOK"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.PostMethod, "guestbook/");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("POST METHOD OF GUEST BOOK"));
//            }
//        }
//
//        [TestMethod]
//        public void DefaultRouteGetTest()
//        {
//            var server = new LocalHttpListener();
//            var task = Task.Factory.StartNew(server.Start);
//
//            var webRequest = CreateWebRequest(ServerHelpers.GetMethod);
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("TEST"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.GetMethod, "asdasdasdqdqwdqwdqw");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("TEST"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.GetMethod, "1231231212312");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("TEST"));
//            }
//
//        }
//
//        [TestMethod]
//        public void ProxyRouteGetTest()
//        {
//            var server = new LocalHttpListener();
//            var task = Task.Factory.StartNew(server.Start);
//            
//            var webRequest = CreateWebRequest(ServerHelpers.GetMethod, ServerHelpers.ProxyName);
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("Proxy"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.GetMethod, "ProXy");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("Proxy"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.GetMethod, "PROXY");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("Proxy"));
//            }
//        }
//
//
//        [TestMethod]
//        public void GuestBookRouteGetTest()
//        {
//            var server = new LocalHttpListener();
//            var task = Task.Factory.StartNew(server.Start);
//
//            var webRequest = CreateWebRequest(ServerHelpers.GetMethod, ServerHelpers.GuestBookName);
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("GUESTBOOK will be here"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.GetMethod, "guestbook");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("GUESTBOOK will be here"));
//            }
//
//            webRequest = CreateWebRequest(ServerHelpers.GetMethod, "GUESTBOOK");
//
//            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
//            {
//                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
//                Assert.IsTrue(GetResponseBody(webResponse).Contains("GUESTBOOK will be here"));
//            }
//
//        }
//
//        private static WebRequest CreateWebRequest(string methodType, string absolutePath = "")
//        {
//            var webRequest = WebRequest.Create(String.Format("{0}{1}/",ServerHelpers.DefaultUrl, absolutePath));
//            webRequest.Method = methodType;
//            webRequest.Credentials = CredentialCache.DefaultCredentials;
//            webRequest.ContentLength = 0;
//            
//            return webRequest;
//        }
//
//        private static string GetResponseBody(HttpWebResponse response)
//        {
//            var result = "";
//            using (var bodyStream = response.GetResponseStream())
//            {
//                if (bodyStream == null)
//                {
//                    throw new SimpleServerException 
//                            {
//                                  ExceptionMessage = "Can't get body from response"
//                            };
//                }
//                using (var streamReader = new StreamReader(bodyStream, Encoding.UTF8))
//                {
//                    result = streamReader.ReadToEnd();
//                }
//            }
//
//            return result;
//        }
//    }
}
