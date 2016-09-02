using GameNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPGameClient
{
    class Program
    {
        private static INetworkAgent networkAgent;

        static void Main(string[] args)
        {
            networkAgent = new GameClientTCP();

            networkAgent.Start();
        }
    }
}
