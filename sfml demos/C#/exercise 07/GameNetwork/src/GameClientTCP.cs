using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Xml;

namespace GameNetwork
{
    public class GameClientTCP : NetworkAgent, INetworkAgent
    {
        private Thread thread;
        private StreamReader reader;

        public void Start()
        {
            try
            {
                thread = new Thread(new ThreadStart(ReceiveEcho));

                xmlConfig = new XmlDocument();
                xmlConfig.Load("config.xml");

                var ipserver = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/ipserver");
                var nodePort = xmlConfig.DocumentElement.SelectSingleNode("/gamenetwork/port");

                TcpClient tcpClient = new TcpClient(ipserver.InnerText, int.Parse(nodePort.InnerText));
                var netStream = tcpClient.GetStream();
                reader = new StreamReader(netStream);
                var writer = new StreamWriter(netStream);

                thread.Start();

                while (true)
                {
                    Console.Write(">");
                    var line = Console.ReadLine();

                    if (line.Length > 0)
                    {
                        writer.WriteLine(line);
                        writer.Flush();
                    }


                    if (line == "exit")
                        break;
                }

                netStream.Close();
                reader.Close();
                writer.Close();

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ReceiveEcho()
        {
            while (true)
            {
                var echoLine = reader.ReadLine();
                Console.WriteLine(echoLine);
            }
        }

        public void Stop()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception on closing connection: " + e.ToString());
            }

        }
    }
}
