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
    /// <summary>
    /// Classe responsável por realizar a comunicação em rede do cliente
    /// </summary>
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
                //inicia um novo tcpClient e tenta se conectar no servidor
                this.remote = new RemoteClient();
                this.tcp = new TcpClient(GetIP4Address().ToString(), DEFAULT_PORT);

                //quando conectar, já inicia o recebimento de uma mensagem do server (a primeira mensagem será o handshake)
                ReceiveMessageFromServer();
            }
            catch (Exception e) when (e is SocketException)
            {
                //se não conseguir, servidor está off
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Server is offline");
            }
        }

        /// <summary>
        /// Inicia o recebimento de uma mensagem do servidor, de forma assíncrona
        /// </summary>
        public void ReceiveMessageFromServer()
        {
            try
            {
                NetworkStream inStream = this.tcp.GetStream();
                this.bufferIn = new byte[this.tcp.ReceiveBufferSize];
                if(inStream.CanRead)
                    inStream.BeginRead(bufferIn, 0, bufferIn.Length, ReadCallback, this.tcp);

            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Server is offline");
            }
        }

        /// <summary>
        /// Callback chamada quando a mensagem termina de ser recebida
        /// </summary>
        /// <param name="ar">TCPClient do cliente</param>
        private void ReadCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;

            try
            {
                //obtem a stream e termina a leitura
                NetworkStream inStream = client.GetStream();
                int count = inStream.EndRead(ar);

                //se recebeu ao menos 1 byte
                if(count > 0)
                {
                    //converte a stream de bytes para o RemoteClient
                    var tempBuffer = new byte[count];
                    Buffer.BlockCopy(this.bufferIn, 0, tempBuffer, 0, count);
                    this.remote = RemoteClient.Deserialize(tempBuffer);


                    switch(this.remote.type)
                    {
                        //se for handshake, salva o ID criado pelo servidor e espera por uma nova mensagem
                        case MessageType.Handhsake:
                            this.id = this.remote.clientID;
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.ClientReady:
                            ReceiveMessageFromServer();
                            break;

                        //Se for uma mensagem de que a partida pode começar
                        case MessageType.StartParty:
                            //Determina que o nível de "Lobby" de players pode encerrar e espera a nova mensagem
                            LobbyLevel.LobbyIsDone(this, new EventArgs()); //LobbyIsDone é um evento que invoca o método StartLevel1 da classe Game
                            ReceiveMessageFromServer();
                            break;

                        //Se for a mensagem de instanciar jogadores, o Level1 invoca o evento de instanciar os jogadores
                        case MessageType.InstantiatePlayers:
                            Level1.InstantiatePlayers(this, new EventArgs());
                            ReceiveMessageFromServer();
                            break;

                        // Se recebeu uma mensagem de atualização, define que está sincronizando
                        case MessageType.Update:
                            this.game.IsSyncing = true;
                            //atualia o jogo com essa nova mensagem
                            this.game.UpdateFromServer(remote);
                            ReceiveMessageFromServer();
                            break;

                        case MessageType.Disconnect:
                            break;
                    }
                }
            }
            catch(Exception e) when(e is SocketException || e is System.IO.IOException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Server is offline");
            }
        }

        /// <summary>
        /// Serializa um RemoteClient na stream do TCPClient e envia para o servidor de forma assíncrona
        /// </summary>
        /// <param name="remote"></param>
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

        /// <summary>
        /// Callback de escrita da mensagem enviada para o servidor
        /// </summary>
        /// <param name="ar"></param>
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
