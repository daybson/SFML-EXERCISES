using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SFMLFramework.src.Network
{
    public class AsynchronousClient
    {
        private const int port = 2929;

        private static ManualResetEvent conectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receivetDone = new ManualResetEvent(false);

        private static string response = string.Empty;
        private static byte[] byteData;
        private static Socket client;

        public static void StartClient()
        {
            try
            {
                //define o remote endpoint para o socket
                IPHostEntry ipHostInfo = Dns.Resolve("127.0.0.1");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, port);

                //cria o socket tpcip
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //conecta no endpoit remoto
                client.BeginConnect(remoteEndPoint, new AsyncCallback(ConnectCallback), client);
                conectDone.WaitOne();
                
                /*
                //envia dado de teste ao dispositivo remoto
                Send(client, "Teste<EOF>");
                sendDone.WaitOne();

                //recebe uma resposta do servidor remoto
                Receive(client);
                receivetDone.WaitOne();

                Console.WriteLine(response);

                client.Shutdown(SocketShutdown.Both);
                client.Close();
                */

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);
                if(bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (state.sb.Length > 1)
                        response = state.sb.ToString();
                    receivetDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Send(Socket client, string v)
        {
            byteData = Encoding.ASCII.GetBytes(v);
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        public static void Send(string v)
        {
            byteData = Encoding.ASCII.GetBytes(v);
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
            sendDone.WaitOne();
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                //Console.WriteLine("Sent: {0}bytes -> {1}", bytesSent, Encoding.ASCII.GetString(byteData));

                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);
                Console.WriteLine("conectado em {0}", client.RemoteEndPoint.ToString());

                //subakuza que a conexao foi estabelecida
                conectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Close()
        {
            if (client != null)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }
    }
}
