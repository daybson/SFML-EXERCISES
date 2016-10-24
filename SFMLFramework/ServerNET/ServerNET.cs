using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using ServerData;

namespace ServerNET
{
    class ServerNET
    {
        static Socket listenerSocket;
        static List<ClientData> clients;
        public string id;

        //start server
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server on " + Packet.GetIP4Address());

            clients = new List<ClientData>();

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Packet.GetIP4Address()), 2929);
            listenerSocket.Bind(ip);

            ListenThread();
        }


        //listens for clients trying to connect
        static void ListenThread()
        {
            Console.WriteLine("Server online listening for connections...");
            while (true)
            {
                listenerSocket.Listen(0);
                var c = new ClientData(listenerSocket.Accept());
                lock (clients)
                {
                    clients.Add(c);
                }

                Console.WriteLine("A new client is connected! " + c.clientSocket.RemoteEndPoint.ToString());

                Thread t = new Thread(new ThreadStart(ReceiveFromClients));
                t.Start();

            }
        }

        //clientdata thread - receives data from client individually
        public static void ReceiveFromClients()
        {
            byte[] buffer;
            int readBytes;
            Console.WriteLine("New thread for a new client");

            while (true)
            {
                lock (clients)
                {
                    foreach (ClientData c in clients.Reverse<ClientData>())
                    {
                        try
                        {
                            buffer = new byte[Packet.PacketSize];
                            readBytes = c.clientSocket.Receive(buffer);

                            if (readBytes > 0)
                            {
                                var packet = new Packet(buffer);
                                Console.WriteLine("~" + packet.SenderID + ": " + packet.Data);
                                ReplicateToClients(packet);
                            }
                        }
                        catch (SocketException e)
                        {
                            Console.WriteLine("A client was disconnected!");
                            clients.Remove(c);
                        }
                    }
                }
            }
        }


        public static void ReplicateToClients(Packet p)
        {
            lock (clients)
            {
                if (p.PacketType == PacketType.Chat)
                {
                    foreach (ClientData c in clients.Reverse<ClientData>())
                    {
                        c.clientSocket.Send(p.ToBytes());
                    }
                }
            }
        }
    }

    //==============================================================================================================================

    class ClientData
    {
        public Socket clientSocket;
        public Thread clientThread;
        public string id;

        public ClientData()
        {
            this.id = Guid.NewGuid().ToString();
            //clientThread = new Thread(ServerNET.ReceiveFromClients);
            //clientThread.Start(clientSocket);
            SendRegistrationPacket();
        }

        public ClientData(Socket clientSocket)
        {
            this.id = Guid.NewGuid().ToString();
            this.clientSocket = clientSocket;
            //clientThread = new Thread(ServerNET.ReceiveFromClients);
            //clientThread.Start(clientSocket);
            SendRegistrationPacket();
        }

        public void SendRegistrationPacket()
        {
            var p = new Packet(PacketType.Registration, "server");
            p.Data = id;
            clientSocket.Send(p.ToBytes());
        }
    }
}
