using GameNetwork.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static INetworkAgent networkAgent;

        static void Main(string[] args)
        {
            networkAgent = new GameServer();

            networkAgent.Start();
        }
    }
}
