using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using NetData;

namespace Server.src.logic
{
    public class ServerNET
    {
        private TcpListener listener;
        private Dictionary<RemoteClient, TcpClient> clients;
        private const int DEFAULT_PORT = 2929;
        private bool isListening;
        private byte[] bufferIn;
        private byte[] bufferOut;
        public Dictionary<RemoteClient, TcpClient> Clients { get { return clients; } }
        public bool IsListening { get { return isListening; } }


        /// <summary>
        /// Construtor padrão. Usa IP local e porta 2929
        /// </summary>
        public ServerNET()
        {
            listener = new TcpListener(GetIP4Address(), DEFAULT_PORT);
            listener?.Start();
            clients = new Dictionary<RemoteClient, TcpClient>();
            isListening = true;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Server is online.");
        }

        public void AcceptNewClients()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\nServer is listening...");
                TcpClient tcpClient = listener.AcceptTcpClient();
                RemoteClient remote = new RemoteClient();

                lock (clients)
                {
                    clients.Add(remote, tcpClient);

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("New client accepted. Clients connected: {0}", clients.Count);

                    var ioThread = new Thread(
                          () =>
                          {
                              var stream = tcpClient.GetStream(); //tcpClient.GetStream();

                              this.bufferIn = new byte[tcpClient.ReceiveBufferSize];
                              if (stream.CanRead)
                                  stream.BeginRead(this.bufferIn, 0, this.bufferIn.Length, ReadCallback, tcpClient);

                              Handshake(ref remote);
                              //this.bufferOut = Encoding.ASCII.GetBytes("Server eccho"); //new byte[newClient.SendBufferSize];
                              //if (stream.CanWrite)
                              //stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, rc.tcpClient);
                          }
                      );
                    ioThread.Start();
                }
                Thread.Sleep(10);
            }
        }

        public void ReadCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            try
            {
                NetworkStream inStream = client.GetStream();
                int count = inStream.EndRead(ar);

                if (count > 0)
                {
                    var tempBuffer = new byte[count];
                    Buffer.BlockCopy(this.bufferIn, 0, tempBuffer, 0, count);
                    string data = Encoding.ASCII.GetString(tempBuffer);
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Received: {0}", data);
                }

                inStream.BeginRead(this.bufferIn, 0, client.ReceiveBufferSize, ReadCallback, client);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Client disconnected!");
                lock (clients)
                {
                    foreach (var c in clients.Reverse())
                    {
                        if (c.Value.Equals(client))
                        {
                            clients.Remove(c.Key);
                            break;
                        }
                    }
                }
            }
        }

        public void WriteCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            try
            {
                NetworkStream outStream = client.GetStream();
                outStream.EndWrite(ar);
                this.bufferOut = Encoding.ASCII.GetBytes("Server eccho: " + DateTime.Now.ToLongTimeString().ToString());

                outStream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, client);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Client disconnected!");
                lock (clients)
                {
                    foreach (var c in clients.Reverse())
                    {
                        if (c.Value.Equals(client))
                        {
                            clients.Remove(c.Key);
                            break;
                        }
                    }
                }
            }
        }

        public void StopListen()
        {
            isListening = false;
            clients.Clear();
            listener.Stop();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nServer is offline.");
            Thread.CurrentThread.Abort();
        }

        private static IPAddress GetIP4Address()
        {
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

            foreach (var ip in ips)
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                    return ip;

            return IPAddress.Parse("127.0.0.1");
        }


        private void Handshake(ref RemoteClient remote)
        {
            var stream = clients[remote].GetStream();
            remote.clientID = Guid.NewGuid().ToString();
            remote.name = "Player " + clients.Count;
            remote.posX = 0.0f;
            remote.posY = 0.0f;
            remote.type = NetData.Type.Handhsake;

            this.bufferOut = RemoteClient.Serialize(remote);
            Console.WriteLine("HANDSHAKE: [{0}]", remote.clientID);

            if (stream.CanWrite)
                stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, clients[remote]);
        }
    }
}
