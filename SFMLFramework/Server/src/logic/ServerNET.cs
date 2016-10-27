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
                    if (this.clients.Count >= this.minPlayers)
                    {
                        Console.WriteLine("Party is full!");
                        continue;
                    }

                    clients.Add(remote, tcpClient);

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("New client accepted. Clients connected: {0}", clients.Count);

                    var ioThread = new Thread(
                          () =>
                          {
                              Handshake(ref remote);
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
                RemoteClient remote = this.clients.First(c => c.Value.Equals(tcp)).Key;
                NetworkStream inStream = tcp.GetStream();
                int count = inStream.EndRead(ar);

                if (count > 0)
                {
                    var tempBuffer = new byte[count];
                    Buffer.BlockCopy(this.bufferIn, 0, tempBuffer, 0, count);
                    remote = RemoteClient.Deserialize(tempBuffer);

                    this.clients.First(c => c.Value.Equals(tcp)).Key.type = remote.type;

                    switch (remote.type)
                    {
                        case MessageType.Update:
                            //replicar o remote para todos os demais clients (exceto o proprio client sender do remote)
                            break;

                        case MessageType.ClientReady:

                            var totalReady = this.clients.Where(c => c.Key.type == MessageType.ClientReady).Count();
                            if (totalReady == this.clients.Count && totalReady == this.minPlayers)
                            {
                                StartParty();
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

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Received: {0}", remote.ToString());
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

        private void StartParty()
        {
            Console.WriteLine("Party is starting...");
            foreach (var clientTarget in this.clients)
            {
                var stream = clientTarget.Value.GetStream();
                var remote = new RemoteClient();
                remote.clientID = clientTarget.Key.clientID;
                remote.name = clientTarget.Key.name;
                remote.type = MessageType.StartParty;

                this.bufferOut = RemoteClient.Serialize(remote);
                if (stream.CanWrite)
                    stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, clientTarget.Value);
            }

            InstantiatePlayers();
        }

        public void InstantiatePlayers()
        {
            int i = 0;
            foreach (var target in this.clients)
            {
                foreach (var remoteData in this.clients)
                {
                    var stream = target.Value.GetStream();
                    remoteData.Key.type = MessageType.InstantiatePlayers;
                    remoteData.Key.posX = i * 128;
                    remoteData.Key.posY = 0;

                    this.bufferOut = RemoteClient.Serialize(remoteData.Key);
                    if (stream.CanWrite)
                        stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, target.Value);
                    i++;
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

            //envia o handshake para o client
            if (stream.CanWrite)
                stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, clients[remote]);

            //inicia a espera de leitura pelo 'ready' do client
            this.bufferIn = new byte[clients[remote].ReceiveBufferSize];
            if (stream.CanRead)
                stream.BeginRead(this.bufferIn, 0, this.bufferIn.Length, ReadCallback, clients[remote]);
        }
    }
}
