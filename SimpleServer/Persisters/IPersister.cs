using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleServer
{
    internal interface IPersister
    {
        void AddRecord(Tuple<string, string> record);
        IEnumerable<Tuple<string,string>> GetAllRecords();
    }
}
