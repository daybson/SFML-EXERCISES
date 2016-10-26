using Server.src.logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static ServerNET server;
        static Thread acceptThread;

        static void Main(string[] args)
        {
            server = new ServerNET();

            acceptThread = new Thread(server.AcceptNewClients);
            acceptThread.Start();

            //Console.ReadLine();
        }
    }
}
