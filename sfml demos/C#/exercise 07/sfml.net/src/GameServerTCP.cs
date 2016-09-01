using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sfml.net.src
{
    class GameServerTCP : NetworkAgent, INetworkAgent
    {
        public void Start()
        {

            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            endPoint = new IPEndPoint(ipAddress, port);
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

        public void Stop()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
