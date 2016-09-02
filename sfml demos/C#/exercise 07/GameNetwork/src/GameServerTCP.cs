using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace GameNetwork
{
    public class GameServerTCP : NetworkAgent, INetworkAgent
    {
        public void Start()
        {
            using(StreamWriter file = new StreamWriter(@"log.txt"))
            {
                xmlConfig = new XmlDocument();
                xmlConfig.Load("config.xml");

                var myip = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/myip");
                var ipserver = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/ipserver");
                var hasRouter = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/hasrouter");
                var nodePort = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/port");

                file.WriteLine("Connecting from " + myip.InnerText + " to " + ipserver.InnerText + " at port " + nodePort.InnerText + " with router: " + hasRouter.InnerText);

                ipHostInfo = Dns.Resolve(Dns.GetHostName());

                if(hasRouter.InnerText == "false")
                    ipAddress = IPAddress.Parse(ipserver.InnerText); //ipHostInfo.AddressList[0];
                else
                    ipAddress = ipHostInfo.AddressList[0];

                endPoint = new IPEndPoint(ipAddress, int.Parse(nodePort.InnerText));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    socket.Bind(endPoint);
                    socket.Listen(10);

                    Console.WriteLine("Waiting for a player...");
                    var handler = socket.Accept();
                    while(true)
                    {
                        string data = null;

                        while(true)
                        {
                            bytes = new byte[BUFFER_SIZE];
                            var bytesReceived = handler.Receive(bytes);
                            data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);
                            if(data.IndexOf(Environment.NewLine) > -1)
                                break;
                        }

                        Console.WriteLine("Client says: {0}", data);
                        Console.Write(">");
                        var msg = Encoding.ASCII.GetBytes(Console.ReadLine() + Environment.NewLine);
                        handler.Send(msg);
                        //handler.Shutdown(SocketShutdown.Both);
                        //handler.Close();
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.WriteLine("ENTER to exit");
                Console.Read();
            }
        }

        public void Stop()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
