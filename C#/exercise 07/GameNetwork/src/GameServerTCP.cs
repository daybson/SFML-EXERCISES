using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace GameNetwork
{
    public class GameServerTCP : NetworkAgent, INetworkAgent
    {
        private List<Socket> clients;
        private TcpListener tcpListener;

        public GameServerTCP()
        {
            xmlConfig = new XmlDocument();
            xmlConfig.Load("config.xml");

            var ipserver = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/ipserver");
            var nodePort = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/port");

            ipAddress = IPAddress.Parse(ipserver.InnerText);

            clients = new List<Socket>();

            tcpListener = new TcpListener(ipAddress, int.Parse(nodePort.InnerText));
            tcpListener.Start();

            Console.WriteLine("Waiting for clients...");
        }

        private void AcceptNewClient()
        {
            var t = new Thread(new ThreadStart(AcceptNewClient));
            NetworkStream netStream = null;
            StreamWriter writer = null;
            StreamReader reader = null;

            try
            {
                socket = tcpListener.AcceptSocket();

                lock (clients)
                {
                    clients.Add(socket);
                }

                t.Start();

                if (socket.Connected)
                {
                    Console.WriteLine("Client " + socket.RemoteEndPoint.ToString() + " connected");

                    netStream = new NetworkStream(socket);
                    writer = new StreamWriter(netStream);
                    reader = new StreamReader(netStream);

                    while (true)
                    {
                        if (netStream.CanRead)
                        {
                            var line = reader.ReadLine();
                            Console.WriteLine(socket.RemoteEndPoint.ToString() + ">" + line);

                            if (line == "exit")
                                break;

                            lock (clients)
                            {
                                foreach (var c in clients)
                                {
                                    if (c.RemoteEndPoint != socket.RemoteEndPoint)
                                    {
                                        using (var nstream = new NetworkStream(c))
                                        {
                                            using (var w = new StreamWriter(nstream))
                                            {
                                                w.WriteLine(socket.RemoteEndPoint.ToString() + ": " + line);
                                                w.Flush();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    //t.Abort();
                }
                socket.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                clients.Remove(socket);
                netStream.Close();
                reader.Close();
                writer.Close();
                Console.WriteLine(socket.RemoteEndPoint.ToString() + " is out");
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                //t.Abort();
            }
        }

        public void Start()
        {
            var t = new Thread(new ThreadStart(AcceptNewClient));
            t.Start();
        }

        public void Stop()
        {

        }
    }
}
