using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetData;
using SFMLFramework.src.Level;

namespace SFMLFramework.src.Network
{
    public class NetClient
    {

        private const int DEFAULT_PORT = 2929;
        private RemoteClient remote;
        public RemoteClient Remote { get { return remote; } }
        private TcpClient tcp;
        private byte[] bufferOut;
        private byte[] bufferIn;
        private string id;
        public string ID { get { return id; } }

        public NetClient()
        {
            this.remote = new RemoteClient();
            this.tcp = new TcpClient(GetIP4Address().ToString(), DEFAULT_PORT);
            ReceiveMessageFromServer();
        }

        public void SendMessageToServer(RemoteClient remote)
        {
            remote.clientID = this.remote.clientID;
            remote.name = this.remote.name;
            this.bufferOut = RemoteClient.Serialize(remote);

            try
            {
                NetworkStream outStream = this.tcp.GetStream();
                if (outStream.CanWrite)
                    outStream.BeginWrite(bufferOut, 0, bufferOut.Length, WriteCallback, outStream);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Server is offline!");
            }
        }

        private void WriteCallback(IAsyncResult ar)
        {
            try
            {
                NetworkStream outStream = (NetworkStream)ar.AsyncState;
                outStream.EndWrite(ar);
            }
            catch (Exception e) when (e is SocketException || e is System.IO.IOException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Server is offline!");
            }
        }

        public void ReceiveMessageFromServer()
        {
            try
            {
                NetworkStream inStream = this.tcp.GetStream();
                this.bufferIn = new byte[this.tcp.ReceiveBufferSize];
                if (inStream.CanRead)
                    inStream.BeginRead(bufferIn, 0, bufferIn.Length, ReadCallback, this.tcp);

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Server is offline!");
            }
        }

        private void ReadCallback(IAsyncResult ar)
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

                    this.remote = RemoteClient.Deserialize(tempBuffer);

                    switch (this.remote.type)
                    {
                        case MessageType.Handhsake:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("CONECTADO!\nHANDSHAKE: [{0}]\n", this.remote.clientID);
                            this.id = this.remote.clientID;
                            //aguardar input de Ready
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.Update:
                            //atualizar o objeto do remoteClient dentro do Level
                            break;

                        case MessageType.ClientReady:
                            Console.WriteLine(this.remote.ToString() + "\n");
                            ReceiveMessageFromServer();
                            //aguardar resposta de StartParty do servidor (só chega quando todos os clientes enviarem um Ready)
                            //carregar novo level
                            //instanciar objeto do remoteClient
                            //iniciar loop de leitura/escrita de posição, status...
                            break;

                        case MessageType.StartParty:
                            Console.WriteLine(this.remote.ToString() + "\n");
                            LobbyLevel.LobbyIsDone(this, new EventArgs());
                            //criar novo nivel em Game
                            //instanciar os players
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.InstantiatePlayers:
                            Console.WriteLine("InstantiatePlayers");
                            Level1.InstantiatePlayers(this, new EventArgs());
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.Disconnect:
                            break;

                        default:
                            Console.WriteLine("Unknow type");
                            break;
                    }
                }

                //inStream.BeginRead(this.bufferIn, 0, client.ReceiveBufferSize, ReadCallback, client);
            }
            catch (Exception e) when (e is SocketException || e is System.IO.IOException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Server is offline!");
            }
        }

        private static IPAddress GetIP4Address()
        {
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

            foreach (var ip in ips)
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                    return ip;

            return IPAddress.Parse("127.0.0.1");
        }
    }
}
