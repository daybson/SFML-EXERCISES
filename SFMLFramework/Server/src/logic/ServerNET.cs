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

        private int minPlayers = 2;


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
                              Handshake(ref remote);

                              /*
                              var stream = tcpClient.GetStream(); //tcpClient.GetStream();
                              this.bufferIn = new byte[tcpClient.ReceiveBufferSize];
                              if (stream.CanRead)
                                  stream.BeginRead(this.bufferIn, 0, this.bufferIn.Length, ReadCallback, tcpClient);
                              */
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
            TcpClient tcp = (TcpClient)ar.AsyncState;
            try
            {
                var remote = this.clients.FirstOrDefault(c => c.Value.Equals(tcp)).Key;
                NetworkStream inStream = tcp.GetStream();
                int count = inStream.EndRead(ar);

                if (count > 0)
                {
                    var tempBuffer = new byte[count];
                    Buffer.BlockCopy(this.bufferIn, 0, tempBuffer, 0, count);
                    remote = RemoteClient.Deserialize(tempBuffer);

                    switch (remote.type)
                    {
                        case MessageType.Update:
                            //replicar o remote para todos os demais clients (exceto o proprio client sender do remote)
                            break;

                        case MessageType.ClientReady:
                            var totalReady = this.clients.Where(c => c.Key.type != MessageType.ClientReady).Count();
                            if (totalReady == this.clients.Count)
                            {
                                //enviar StartParty
                                //carregar novo level
                                //instanciar objeto do remoteClient
                                //iniciar loop de leitura/escrita de posição, status...
                            }
                            break;

                        case MessageType.Disconnect:
                            this.clients.Remove(remote);
                            break;
                    }

                    string data = Encoding.ASCII.GetString(tempBuffer);
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Received: {0}", data);
                }

                inStream.BeginRead(this.bufferIn, 0, tcp.ReceiveBufferSize, ReadCallback, tcp);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Client disconnected!");
                lock (clients)
                {
                    clients.Remove(clients.FirstOrDefault(c => c.Value.Equals(tcp)).Key);
                }
            }
        }

        public void WriteCallback(IAsyncResult ar)
        {
            TcpClient tcp = (TcpClient)ar.AsyncState;
            try
            {
                NetworkStream outStream = tcp.GetStream();
                outStream.EndWrite(ar);
                this.bufferOut = Encoding.ASCII.GetBytes("Server eccho: " + DateTime.Now.ToLongTimeString().ToString());

                outStream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, tcp);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Client disconnected!");
                lock (clients)
                {
                    clients.Remove(clients.FirstOrDefault(c => c.Value.Equals(tcp)).Key);
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
            remote.type = MessageType.Handhsake;

            this.bufferOut = RemoteClient.Serialize(remote);
            Console.WriteLine("HANDSHAKE: [{0}]", remote.clientID);

            if (stream.CanWrite)
                stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, clients[remote]);

            this.bufferIn = new byte[clients[remote].ReceiveBufferSize];
            if (stream.CanRead)
                stream.BeginRead(this.bufferIn, 0, this.bufferIn.Length, ReadCallback, clients[remote]);

        }
    }
}
