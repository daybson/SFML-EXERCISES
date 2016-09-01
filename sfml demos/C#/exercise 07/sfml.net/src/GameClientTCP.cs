using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sfml.net.src
{
    class GameClientTCP : NetworkAgent, INetworkAgent
    {
        public void Start()
        {
            bytes = new byte[BUFFER_SIZE];

            try
            {
                ipHostInfo = Dns.Resolve(Dns.GetHostName());
                ipAddress = ipHostInfo.AddressList[0];
                endPoint = new IPEndPoint(ipAddress, port);
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
