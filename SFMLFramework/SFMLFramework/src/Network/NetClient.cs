using GameNetwork.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace SFMLFramework.src.Network
{
    public class NetClient : NetworkAgent, INetworkAgent, IObserver<GameObject>, IComponent
    {
        private Thread thread;
        private StreamReader reader;

        string lastSent;
        private string receivedMessage = string.Empty;
        public string ReceivedMessage
        {
            get
            {
                lock (this)
                {
                    return receivedMessage;
                }
            }
        }

        public bool IsEnabled { get; set; }
        public GameObject Root { get; set; }


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
                    if (receivedMessage.Length > 0)
                    {
                        writer.WriteLine(receivedMessage);
                        writer.Flush();
                    }

                    receivedMessage = String.Empty;
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
                Thread.Sleep(1);
                //receivedMessage = reader.ReadLine();
                //Console.WriteLine("Received: " + receivedMessage);
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

        public void OnNext(GameObject value)
        {
            receivedMessage = value.Position.ToString();
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }

        public void Update(float deltaTime)
        {
        }
    }
}