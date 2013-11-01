using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new LocalHttpListener();
            var task = Task.Factory.StartNew(server.Start);

            while (true)
            {
                var key = Console.Read();
                if (key == 'q')
                {
                    server.Stop();
                    break;
                }    
            }                                  
        }       
    }
}