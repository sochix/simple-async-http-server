using System;
using SimpleServer.Core;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Core.SimpleServer();
            server.Start();

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