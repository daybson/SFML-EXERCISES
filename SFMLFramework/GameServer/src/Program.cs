using GameNetwork.src;
using GameNetwork.src.ServerChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static List<HandleClient> clients = new List<HandleClient>();

        static void Main(string[] args)
        {
            //networkAgent = new GameServer();
            //networkAgent.Start();

            //var s = new AsynchronousServer();
            //s.Start();

            clients = new List<HandleClient>();

            TcpListener serverSocket = new TcpListener(2929);
            TcpClient clientSocket = default(TcpClient);
            int count = 0;
            serverSocket.Start();

            while (true)
            {
                count++;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(">> Client {0} has joined", count);
                HandleClient client = new HandleClient();
                //clients.Add(client);
                client.StartClient(clientSocket, Convert.ToString(count));
                
                /*
                lock (clients)
                {
                    foreach (var c in clients)
                    {
                        if (!c.Equals(client))
                        {
                            c.SendMessage(c.DataFromClient);
                        }
                    }
                }*/
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(">> Exiting...");
            Console.ReadLine();
        }
    }
}
