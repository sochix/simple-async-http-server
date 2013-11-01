using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using SimpleServer;

namespace SimpleServerTest
{
    [TestClass]
    public class PersistenceTest
    {
        [TestMethod]
        public void AddingOneRecordTest()
        {
            var persister = new MemoryDataPersister();
            var server = new LocalHttpListener(persister);
            var task = Task.Factory.StartNew(server.Start);
            
            var user = "TestUser";
            var message = "TestMessage";

            var webRequest = CreateWebRequest(ServerHelpers.PostMethod, ServerHelpers.GuestBookName, String.Format("?user={0}&?message={1}",user, message));

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);                
            }

            Assert.AreEqual(1, persister.GetAllRecords().Count());
            Assert.IsTrue(user.Equals(persister.GetAllRecords().Single().Item1));
            Assert.IsTrue(message.Equals(persister.GetAllRecords().Single().Item2));            

        }


        [TestMethod]
        public void AddingSeveralRecordsTest()
        {
            var persister = new MemoryDataPersister();
            var server = new LocalHttpListener(persister);
            var task = Task.Factory.StartNew(server.Start);

            var user = "TestUser";
            var message = "TestMessage";

            for (var i = 0; i < 100; i++)
            {
                var webRequest = CreateWebRequest(ServerHelpers.PostMethod, ServerHelpers.GuestBookName, String.Format("?user={0}{2}&?message={1}{2}", user, message,i));

                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
                }    
            }

            var records = persister.GetAllRecords().ToList();

            Assert.AreEqual(100, records.Count());


            for (var i=0; i<records.Count(); i++)
            {
                var loadedUser = records[i].Item1;
                var loadedMessage = records[i].Item2;
                Assert.IsTrue(loadedUser.Equals(String.Format("{0}{1}",user,i)));
                Assert.IsTrue(loadedMessage.Equals(String.Format("{0}{1}", message, i)));                
            }            
        }

        [TestMethod]
        public void AddingIncorrectRecordTest()
        {
            var persister = new MemoryDataPersister();
            var server = new LocalHttpListener(persister);
            var task = Task.Factory.StartNew(server.Start);

            var user = "TestUser";
            var message = "TestMessage";

            var webRequest = CreateWebRequest(ServerHelpers.PostMethod, ServerHelpers.GuestBookName, String.Format("?usr={0}&?msg={1}", user, message));

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
            }

            Assert.AreEqual(0, persister.GetAllRecords().Count());            

        }


        private static WebRequest CreateWebRequest(string methodType, string absolutePath = "", string query = "")
        {
            var webRequest = WebRequest.Create(String.Format("{0}{1}/{2}", ServerHelpers.DefaultUrl, absolutePath, query));
            webRequest.Method = methodType;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.ContentLength = 0;

            return webRequest;
        }

        private static string GetResponseBody(HttpWebResponse response)
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
