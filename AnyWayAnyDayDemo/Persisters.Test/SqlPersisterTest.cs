using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer.Persisters;

namespace SimpleServerTest.Persisters.Test
{
    [TestClass]
    public class SqlPersisterTest
    {
        private SqlDataPersister persister;
        private const string Path = "test.sql";

        [TestInitialize]
        public void TestInitialize()
        {
            persister = new SqlDataPersister(Path);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            File.Delete(Path);
        }

        #region AbstractPersisterTests
        [TestMethod]
        public void AddRecordTest()
        {
            AbstractPersisterTest.AddRecordTest(persister);
        }

        [TestMethod]
        public void AddSeveralRecordsTest()
        {
            AbstractPersisterTest.AddSeveralRecordsTest(persister);
        }

        [TestMethod]
        public void AddNullRecordTest()
        {
            AbstractPersisterTest.AddNullRecordTest(persister);
        }

        [TestMethod]
        public void AddTupleWithNullItemsRecordTest()
        {
            AbstractPersisterTest.AddTupleWithNullItemsRecordTest(persister);
        }

        [TestMethod]
        public void AddTupleWithEmptyItemsRecordTest()
        {
            AbstractPersisterTest.AddTupleWithEmptyItemsRecordTest(persister);
        }

        [TestMethod]
        public void AddLongRecordTest()
        {
            AbstractPersisterTest.AddLongRecordTest(persister);
        }
        #endregion
    }
}
