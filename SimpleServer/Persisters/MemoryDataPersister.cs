using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleServer
{
    internal class MemoryDataPersister: IPersister
    {
        private readonly List<Tuple<string, string>> recordsList;

        public MemoryDataPersister()
        {
            recordsList = new List<Tuple<string, string>>();
        }
        
        public void AddRecord(Tuple<string, string> record)
        {
            recordsList.Add(record);
        }

        public IEnumerable<Tuple<string, string>> GetAllRecords()
        {
            return recordsList;
        }
    }
}
