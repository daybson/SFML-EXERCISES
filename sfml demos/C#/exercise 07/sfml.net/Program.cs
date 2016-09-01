using sfml.net.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfml.net
{
    class Program
    {
        private static INetworkAgent networkAgent;

        static void Main(string[] args)
        {
            Console.WriteLine("Escolha o modo: [S] Servidor   [C] Cliente " + Environment.NewLine);
            var r = Console.ReadKey();
            Console.WriteLine(Environment.NewLine);

            if(r.Key == ConsoleKey.S)
                networkAgent = new GameServerTCP();
            else if(r.Key == ConsoleKey.C)
                networkAgent = new GameClientTCP();

            networkAgent.Start();
        }
    }
}
