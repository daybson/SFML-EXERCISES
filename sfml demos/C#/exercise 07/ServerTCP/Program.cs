﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameNetwork;
using System.Runtime.InteropServices;

namespace TCPGameServer
{
    class Program
    {
        private static INetworkAgent networkAgent;
        
        static void Main(string[] args)
        {
            networkAgent = new GameServerTCP();

            networkAgent.Start();
        }        
    }
}
