using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GameNetwork.src
{
    public class GameClient : NetworkAgent, INetworkAgent
    {
        private Thread thread;
        private StreamReader reader;

        private string receivedMessage;
        public string ReceivedMessage
        {
            get
            {
                return receivedMessage;
            }
        }

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
                receivedMessage = reader.ReadLine();
                Console.WriteLine(receivedMessage);
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
