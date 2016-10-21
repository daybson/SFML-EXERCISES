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
        private static TcpClient clientSocket;
        static int count = 0;
        private static TcpListener serverSocket;

        static void Main(string[] args)
        {
            //networkAgent = new GameServer();
            //networkAgent.Start();

            //var s = new AsynchronousServer();
            //s.Start();

            clients = new List<HandleClient>();

            serverSocket = new TcpListener(2929);
            clientSocket = default(TcpClient);
            serverSocket.Start();

            while (true)
            {
                serverSocket.BeginAcceptSocket(new AsyncCallback(AcceptCallback), serverSocket);
                lock (clients)
                {
                    foreach (var c in clients.Reverse<HandleClient>())
                    {
                        //if (!c.Equals(client))
                        {
                            c.SendMessage(c.DataFromClient);
                            Console.WriteLine("enviou");
                        }
                    }
                }
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(">> Exiting...");
            Console.ReadLine();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            var tcpl = (TcpListener)ar.AsyncState;

            var tcpClient = tcpl.EndAcceptTcpClient(ar);
            count++;
            Console.WriteLine(">> Client {0} has joined", count);
            HandleClient client = new HandleClient();
            clients.Add(client);
            client.StartClient(tcpClient, Convert.ToString(count));
            //serverSocket.BeginAcceptSocket(new AsyncCallback(AcceptCallback), serverSocket);
        }
    }    
}
