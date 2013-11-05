using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer.Persisters;

namespace SimpleServerTest.Persisters.Test
{
    [TestClass]
    public class LoadDataXmlPersisterTest
    {
        private XmlDataPersister persister;
        private const string UnexsistingFilePath = "Files\\unexsisting_db_test.xml";

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(UnexsistingFilePath))
            {
                File.Delete(UnexsistingFilePath);                  
            }
        }

        [TestMethod]
        public void LoadDataTest()
        {
            persister = new XmlDataPersister("Files//test_db.xml");
            var records = persister.GetAllRecords();

            Assert.IsNotNull(records);
            Assert.AreEqual(100, records.Count());
            Assert.AreEqual(TestHelpers.TestRecord, records.First());
        }

        [TestMethod]
        public void LoadEmptyDbTest()
        {
            persister = new XmlDataPersister("Files\\empty_db_test.xml");
            var records = persister.GetAllRecords();

            Assert.IsNotNull(records);
            Assert.AreEqual(0, records.Count());            
        }

        [TestMethod]
        public void LoadUnexistedDbTest()
        {           
            persister = new XmlDataPersister(UnexsistingFilePath);
            var records = persister.GetAllRecords();

            Assert.IsNotNull(records);
            Assert.AreEqual(0, records.Count());
        }

        [TestMethod]
        public void LoadSpoiledDbTest()
        {
            bool wasException = false;
            try
            {
                persister = new XmlDataPersister("Files\\spoiled_db_test.xml");
            }
            catch
            {
                wasException = true;
            }
            
            Assert.IsTrue(wasException);
        }

        [TestMethod]
        public void LoadNullPathDbTest()
        {
            bool wasException = false;
            try
            {
                persister = new XmlDataPersister(null);
            }
            catch
            {
                wasException = true;
            }

            Assert.IsTrue(wasException);
        }        
    }
}
