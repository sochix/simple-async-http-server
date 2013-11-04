using System;
using System.Collections.Generic;

namespace SimpleServer.Persisters
{
    /// <summary>
    /// Test persister, which stores all data in RAM
    /// </summary>
    internal class MemoryDataPersister: IPersister
    {
        private readonly List<Tuple<string, string>> recordsList;

        public MemoryDataPersister()
        {
            recordsList = new List<Tuple<string, string>>();
        }
        
        public void AddRecord(Tuple<string, string> record)
        {
            if (record == null) return;

            if (String.IsNullOrEmpty(record.Item1) || String.IsNullOrEmpty(record.Item2))
                return;

            recordsList.Add(record);
        }

        public IEnumerable<Tuple<string, string>> GetAllRecords()
        {
            return recordsList;
        }
    }
}
