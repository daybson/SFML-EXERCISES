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
        private List<Thread> clientsOnline;
        TcpListener tcpListener;

        public GameServerTCP()
        {
            xmlConfig = new XmlDocument();
            xmlConfig.Load("config.xml");

            var ipserver = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/ipserver");
            var nodePort = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/port");

            ipAddress = IPAddress.Parse(ipserver.InnerText);

            tcpListener = new TcpListener(ipAddress, int.Parse(nodePort.InnerText));
            tcpListener.Start();

            Console.WriteLine("Waiting for clients...");
        }

        private void AcceptNewClient()
        {
            try
            {
                var socket = tcpListener.AcceptSocket();
                var t = new Thread(new ThreadStart(AcceptNewClient));
                t.Start();

                if (socket.Connected)
                {
                    Console.WriteLine("Client " + socket.RemoteEndPoint.ToString() + " connected");

                    var netStream = new NetworkStream(socket);
                    var writer = new StreamWriter(netStream);
                    var reader = new StreamReader(netStream);

                    while (true)
                    {
                        if (netStream.CanRead)
                        {
                            var line = reader.ReadLine();
                            Console.WriteLine(socket.RemoteEndPoint.ToString() + ">" + line);

                            if (line == "exit")
                                break;
                        }
                    }

                    netStream.Close();
                    reader.Close();
                    writer.Close();
                }
                socket.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Start()
        {
            //kill remaining threads and start a new set 
            if (clientsOnline != null)
                clientsOnline.ForEach(c => c.Abort());
            clientsOnline = new List<Thread>();

            var t = new Thread(new ThreadStart(AcceptNewClient));
            t.Start();
        }

        public void Stop()
        {
            if (clientsOnline != null)
                clientsOnline.ForEach(c => c.Abort());

            clientsOnline.Clear();
        }
    }
}
