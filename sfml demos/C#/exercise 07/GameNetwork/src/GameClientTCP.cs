using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

namespace GameNetwork
{
    public class GameClientTCP : NetworkAgent, INetworkAgent
    {
        public void Start()
        {
            //using(StreamWriter file = new StreamWriter(@"log.txt", true))
            {
                bytes = new byte[BUFFER_SIZE];

                try
                {
                    xmlConfig = new XmlDocument();
                    xmlConfig.Load("config.xml");

                    var myip = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/myip");
                    var ipserver = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/ipserver");
                    var hasRouter = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/hasrouter");
                    var nodePort = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/port");

                    //file.Write("Connecting from " + myip.InnerText + " to " + ipserver.InnerText + " at port " + nodePort.InnerText + " with router: " + hasRouter.InnerText);

                    ipHostInfo = Dns.Resolve(Dns.GetHostName());

                    if(hasRouter.InnerText == "false")
                        ipAddress = IPAddress.Parse(ipserver.InnerText); //ipHostInfo.AddressList[0];
                    else
                        ipAddress = ipHostInfo.AddressList[0];

                    endPoint = new IPEndPoint(ipAddress, int.Parse(nodePort.InnerText));
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        socket.Connect(endPoint);
                        Console.WriteLine("Connected to {0}", socket.RemoteEndPoint.ToString());

                        while(true)
                        {
                            //var msg = Encoding.ASCII.GetBytes("Hello server " + socket.RemoteEndPoint.ToString() + Environment.NewLine);
                            var msg = Encoding.ASCII.GetBytes(Console.ReadLine() + Environment.NewLine);
                            string data = null;
                            var bytesSent = socket.Send(msg);

                            var v = new Vector3(1f, 2f, 3f);

                            IFormatter formatter = new BinaryFormatter();
                            var netStream = new NetworkStream(socket, true);
                            Stream stream = new MemoryStream();
                            formatter.Serialize(stream, v);
                            byte[] buffer = ((MemoryStream)stream).ToArray();

                            var writer = new BinaryWriter(netStream);

                            while(true)
                            {
                                bytes = new byte[BUFFER_SIZE];
                                var bytesReceived = socket.Receive(bytes);
                                data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);
                                if(data.IndexOf(Environment.NewLine) > -1)
                                    break;
                            }
                            //var bytesReceived = socket.Receive(bytes);

                            Console.WriteLine("Serevr says: {0}", data);
                            Console.Write(">");
                            //Stop();
                        }
                    }

                    catch(ArgumentNullException ane)
                    {
                        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    }
                    catch(SocketException se)
                    {
                        Console.WriteLine("SocketException : {0}", se.ToString());
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void Stop()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception on closing connection: " + e.ToString());
            }

        }
    }
}
