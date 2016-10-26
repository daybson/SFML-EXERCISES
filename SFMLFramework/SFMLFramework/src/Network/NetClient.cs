﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetData;

namespace SFMLFramework.src.Network
{
    public class NetClient
    {
        private const int DEFAULT_PORT = 2929;
        private RemoteClient remote;
        private TcpClient tcp;
        private byte[] bufferOut;
        private byte[] bufferIn;
        private string guid;

        public NetClient()
        {
            this.remote = new RemoteClient();
            this.tcp = new TcpClient(GetIP4Address().ToString(), DEFAULT_PORT);
        }

        public void SendMessageToServer(string data)
        {
            this.bufferOut = Encoding.ASCII.GetBytes(this.guid + "::" + data);
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
                bufferOut = Encoding.ASCII.GetBytes(this.guid + "::abcdefgh");
                outStream.BeginWrite(bufferOut, 0, bufferOut.Length, WriteCallback, outStream);
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
                        case NetData.MessageType.Handhsake:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("HANDSHAKE: [{0}]", this.remote.clientID);
                            //aguardar input de Ready
                            break;

                        case NetData.MessageType.Update:
                            //atualizar o objeto do remoteClient dentro do Level
                            break;

                        case NetData.MessageType.ClientReady:
                            //aguardar resposta de StartParty do servidor (só chega quando todos os clientes enviarem um Ready)
                            //carregar novo level
                            //instanciar objeto do remoteClient
                            //iniciar loop de leitura/escrita de posição, status...
                            break;

                        case NetData.MessageType.Disconnect:
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
