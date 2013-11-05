using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer.Persisters;

namespace SimpleServerTest.Persisters.Test
{
    [TestClass]
    public class LoadDataSqlPersisterTest
    {
        private SqlDataPersister persister;
        private const string UnexsistingFilePath = "Files\\unexsisting_db_test.sql";

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
            persister = new SqlDataPersister("Files\\test_db.sql");
            var records = persister.GetAllRecords();

            Assert.IsNotNull(records);
            Assert.AreEqual(100, records.Count());
            Assert.AreEqual(AbstractPersisterTest.TestRecord, records.First());
        }

        [TestMethod]
        public void LoadEmptyDbTest()
        {
            persister = new SqlDataPersister("Files\\empty_db_test.sql");
            var records = persister.GetAllRecords();

            Assert.IsNotNull(records);
            Assert.AreEqual(0, records.Count());
        }

        [TestMethod]
        public void LoadUnexistedDbTest()
        {
            persister = new SqlDataPersister(UnexsistingFilePath);
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
                persister = new SqlDataPersister("Files\\spoiled_db_test.xml");
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
                persister = new SqlDataPersister(null);
            }
            catch
            {
                wasException = true;
            }

            Assert.IsTrue(wasException);
        }
    }
}
