using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleServer.Persisters;

namespace SimpleServerTest.Persisters.Test
{
    public static class AbstractPersisterTest
    {
        public static readonly Tuple<string, string> TestRecord = new Tuple<string, string>("Userame", "Message");
        
        public static void AddRecordTest(IPersister persister)
        {
            persister.AddRecord(TestRecord);

            var records = persister.GetAllRecords();
            Assert.IsNotNull(records);
            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(TestRecord, records.Single());
        }

        public static void AddSeveralRecordsTest(IPersister persister)
        {
            for (var i = 0; i < 100; i++)
            {
                persister.AddRecord(TestRecord);
            }

            var records = persister.GetAllRecords();
            Assert.IsNotNull(records);
            Assert.AreEqual(100, records.Count());
            Assert.AreEqual(TestRecord, records.First());
        }

        public static void AddNullRecordTest(IPersister persister)
        {
            persister.AddRecord(null);

            var records = persister.GetAllRecords();
            Assert.IsNotNull(records);
            Assert.AreEqual(0, records.Count());
        }

        public static void AddTupleWithNullItemsRecordTest(IPersister persister)
        {
            persister.AddRecord(new Tuple<string, string>(null, null));

            var records = persister.GetAllRecords();
            Assert.IsNotNull(records);
            Assert.AreEqual(0, records.Count());
        }

        public static void AddTupleWithEmptyItemsRecordTest(IPersister persister)
        {
            persister.AddRecord(new Tuple<string, string>("", ""));

            var records = persister.GetAllRecords();
            Assert.IsNotNull(records);
            Assert.AreEqual(0, records.Count());
        }

        public static void AddLongRecordTest(IPersister persister)
        {
            var record = new Tuple<string, string>("VeryLongLongLongLongLongLongLongLongLongLongString", "VeryLongLongLongLongLongLongLongLongLongLongString");
            persister.AddRecord(record);

            var records = persister.GetAllRecords();
            Assert.IsNotNull(records);
            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(record, records.Single());
        }
    }
}
