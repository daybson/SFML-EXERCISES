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
    /// <summary>
    /// Classe de representação do servidor do jogo.
    /// Ele utiliza sockets (encapsulados no TCPListener) para aceitar o pedido de conexão dos clients.
    /// Mantém um dicionátio com o objeto RemoteClient referente ao cliente conectado, e o TCPClient dele
    /// 
    /// </summary>
    public class ServerNET
    {
        #region Fields

        private TcpListener listener;
        private Dictionary<RemoteClient, TcpClient> clients;
        private const int DEFAULT_PORT = 2929;
        private bool isListening;
        private byte[] bufferIn;
        private byte[] bufferOut;
        public Dictionary<RemoteClient, TcpClient> Clients { get { return clients; } }
        public bool IsListening { get { return isListening; } }

        private int minPlayers = 2;
        private Thread ioThread;

        #endregion


        /// <summary>
        /// Construtor padrão. Usa IP local e porta 2929
        /// </summary>
        public ServerNET()
        {
            listener = new TcpListener(GetIP4Address(), DEFAULT_PORT);
            listener?.Start(); //c# 6.0 -> mesmo que if(listener!=null) listener.Start();
            clients = new Dictionary<RemoteClient, TcpClient>();
            isListening = true;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Server is online.");
        }

        /// <summary>
        /// Começa a escutar pelo pedido de novos clientes.
        /// </summary>
        public void AcceptNewClients()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\nServer is listening...");

                //A thread fica esperando o novo cliente se conectar para aceitá-lo
                TcpClient tcpClient = listener.AcceptTcpClient();
                RemoteClient remote = new RemoteClient();

                //thread safe
                lock(clients) 
                {
                    if (this.clients.Count >= this.minPlayers)
                    {
                        Console.WriteLine("Party is full!");
                        continue;
                    }

                    //Adiciona o client no dicionário
                    clients.Add(remote, tcpClient);

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("New client accepted. Clients connected: {0}", clients.Count);

                    //Cria uma nova thread para o novo cliente
                    this.ioThread = new Thread(
                          () =>
                          {
                              //Faz o hanshake entre o cliente e o servidor
                              Handshake(ref remote);
                          }
                      );
                    ioThread.Start();
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// O servidor registra um ID único para o novo cliente e envia esse ID para o cliente, 
        /// para que o cliente entenda que foi aceito na partida
        /// </summary>
        /// <param name="remote">Objeto </param>
        private void Handshake(ref RemoteClient remote)
        {
            var stream = clients[remote].GetStream();

            //cria o guid do client
            remote.clientID = Guid.NewGuid().ToString();
            remote.name = "Player " + clients.Count;
            remote.posX = 0.0f;
            remote.posY = 0.0f;

            //define o tipo da mensagem como de hanshake, para que o cliente ao recebê-la saiba o que fazer
            remote.type = MessageType.Handhsake;

            //serializa o RemoteClient para formato de bytes para ser enviado pelo stream do TcpClient do cliente
            this.bufferOut = RemoteClient.Serialize(remote);
            Console.WriteLine("HANDSHAKE: [{0}]", remote.clientID);

            //envia o handshake para o client de forma assíncrona
            if(stream.CanWrite)
                stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, clients[remote]);

            //inicia a espera de leitura de uma mensagem do tipo 'Ready' do client
            this.bufferIn = new byte[clients[remote].ReceiveBufferSize];
            if(stream.CanRead)
                stream.BeginRead(this.bufferIn, 0, this.bufferIn.Length, ReadCallback, clients[remote]);
        }

        /// <summary>
        /// Callback assíncrona de escrita dos bytes pelo stream de um client
        /// </summary>
        /// <param name="ar">IAsync é o parâmetro passado em BeginRead, ou seja, o TCPClient do cliente para quem a mensagem foi enviada</param>
        public void WriteCallback(IAsyncResult ar)
        {
            TcpClient tcp = (TcpClient)ar.AsyncState;
            try
            {
                //termina de escrever
                NetworkStream outStream = tcp.GetStream();
                outStream.EndWrite(ar);
            }
            catch (Exception e)
            {
                //Se houve erro na escrita, o cliente está offline.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Client disconnected!");
                lock (clients)
                {
                    //remove ele da lista de clientes conectados
                    clients.Remove(clients.FirstOrDefault(c => c.Value.Equals(tcp)).Key);
                }
            }
        }

        /// <summary>
        /// Callback assíncrona de recebimento de mensagens enviadas pelos clientes
        /// </summary>
        /// <param name="ar">TCPClient do cliente</param>
        public void ReadCallback(IAsyncResult ar)
        {
            TcpClient tcp = (TcpClient)ar.AsyncState;
            try
            {
                //descobre qual cliente está mandando a mensagem (o TCPClient é o valor do dicionário)
                RemoteClient remote = this.clients.First(c => c.Value.Equals(tcp)).Key;
                
                //termina de ler a mensagem
                NetworkStream inStream = tcp.GetStream();
                int count = inStream.EndRead(ar);

                //se leu pelo menos 1 byte
                if (count > 0)
                {
                    //converte os bytes para o tipo RemoteClient
                    var tempBuffer = new byte[count];
                    Buffer.BlockCopy(this.bufferIn, 0, tempBuffer, 0, count);
                    remote = RemoteClient.Deserialize(tempBuffer);

                    this.clients.First(c => c.Value.Equals(tcp)).Key.type = remote.type;

                    switch (remote.type)
                    {
                        //Se é mensagem de atualização:
                        case MessageType.Update:
                            //TODO: consertar...
                            //replicar a mensagem para todos os demais clients (exceto o proprio client que enviou a mensagem)
                            Broadcast(remote);
                            break;

                        //Se é mensagem de que o cliente está pronto para começar
                        case MessageType.ClientReady:
                            //descobre quantos clientes conectados já estão prontos
                            var totalReady = this.clients.Where(c => c.Key.type == MessageType.ClientReady).Count();

                            //Se todos os players estão prontos, e o mínimo de players esperado se conectou
                            if (totalReady == this.clients.Count && totalReady == this.minPlayers)
                            {
                                //Ininia a partida
                                StartParty();
                                InstantiatePlayers();
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

        /// <summary>
        /// Cria uma mensagem de StartParty e envia para todos os clientes
        /// </summary>
        private void StartParty()
        {
            Console.WriteLine("Party is starting...");
            foreach(var clientTarget in this.clients)
            {
                var stream = clientTarget.Value.GetStream();
                var remote = new RemoteClient();
                remote.clientID = clientTarget.Key.clientID;
                remote.name = clientTarget.Key.name;
                remote.type = MessageType.StartParty;

                this.bufferOut = RemoteClient.Serialize(remote);
                if(stream.CanWrite)
                    stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, clientTarget.Value);
            }
        }


        /// <summary>
        /// Cria uma mensagem de InstantiatePlayers e envia para os clientes.
        /// A posição de cada player é calculada, mantendo um espaço entre eles para que não iniciem colidindo
        /// </summary>
        public void InstantiatePlayers()
        {
            float[] posPlayers = new float[2];
            posPlayers[0] = 64;
            posPlayers[1] = 192;

            foreach(var target in this.clients)
            {
                int i = 0;
                foreach(var remoteData in this.clients)
                {
                    //if (remoteData.Key.clientID.Equals(target.Key.clientID))
                    {
                        var stream = target.Value.GetStream();
                        remoteData.Key.type = MessageType.InstantiatePlayers;
                        remoteData.Key.posX = posPlayers[i];
                        remoteData.Key.posY = 0;
                        this.bufferOut = RemoteClient.Serialize(remoteData.Key);

                        if(stream.CanWrite)
                            stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, target.Value);

                        i++;
                        //TOOD: sem esse sleep acontece exception durante a escrita... 
                        Thread.Sleep(100);
                    }
                }
            }
            //TODO: consertar erro...!!
            //UpdatePlayers();
        }


        /// <summary>
        /// Itera por todos os clientes e envia uma mensagem com a posição de um cliente para os demais
        /// </summary>
        public void UpdatePlayers()
        {
            float[] posPlayers = new float[2];
            posPlayers[0] = 64;
            posPlayers[1] = 192;

            foreach (var target in this.clients)
            {
                int i = 0;
                foreach (var remoteData in this.clients)
                {
                    if (!remoteData.Key.clientID.Equals(target.Key.clientID))
                    {
                        var stream = target.Value.GetStream();
                        remoteData.Key.type = MessageType.Update;
                        remoteData.Key.posX = posPlayers[i];
                        remoteData.Key.posY = 0;
                        this.bufferOut = RemoteClient.Serialize(remoteData.Key);

                        if (stream.CanWrite)
                            stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, target.Value);

                        i++;
                        Thread.Sleep(150);
                    }
                }
            }
        }

        /// <summary>
        /// Repete uma mensagem para todos os demais clientes
        /// </summary>
        /// <param name="remote">Mensagem a ser replicada</param>
        private void Broadcast(RemoteClient remote)
        {
            //Console.WriteLine("Replicating...");
            foreach (var clientTarget in this.clients)
            {
                if (clientTarget.Key.clientID.Equals(remote.clientID))
                    continue;

                var stream = clientTarget.Value.GetStream();
                this.bufferOut = RemoteClient.Serialize(remote);
                if (stream.CanWrite)
                    stream.BeginWrite(this.bufferOut, 0, this.bufferOut.Length, WriteCallback, clientTarget.Value);
            }
        }

        //Interrompe a escuta de novos clientes e 'mata' o servidor
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
    }
}
