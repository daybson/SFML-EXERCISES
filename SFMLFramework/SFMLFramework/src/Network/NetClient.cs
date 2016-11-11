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
        #region Fields

        private const int DEFAULT_PORT = 2929;
        private RemoteClient remote;
        public RemoteClient Remote { get { return remote; } }
        private TcpClient tcp;
        private byte[] bufferOut;
        private byte[] bufferIn;
        private string id;
        public string ID { get { return id; } }
        Game game;
        
        #endregion


        public NetClient(Game game)
        {
            this.game = game;
            try
            {
                this.remote = new RemoteClient();
                this.tcp = new TcpClient(GetIP4Address().ToString(), DEFAULT_PORT);
                ReceiveMessageFromServer();
            }
            catch (Exception e) when (e is SocketException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Server is offline");
            }
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
                Console.WriteLine("Error: Server is offline");
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
                            this.id = this.remote.clientID;
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.ClientReady:
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.StartParty:
                            LobbyLevel.LobbyIsDone(this, new EventArgs());
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.InstantiatePlayers:
                            Level1.InstantiatePlayers(this, new EventArgs());
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.Update:
                            this.game.IsSyncing = true;
                            this.game.UpdateFromServer(remote);
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.Disconnect:
                            break;
                    }
                }
            }
            catch (Exception e) when (e is SocketException || e is System.IO.IOException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Server is offline");
            }
        }

        private void ReceiveUpdate(RemoteClient remote)
        {
            //Console.WriteLine("Client Update {0}", remote.ToString());
            //this.game.UpdateFromServer(remote);
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
