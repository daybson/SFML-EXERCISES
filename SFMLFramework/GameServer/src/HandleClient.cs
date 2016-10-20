using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameNetwork.src.ServerChat
{
    public class HandleClient
    {
        TcpClient clientSocket;
        string clientNumber;
        private Thread t;
        private string dataFromClient = string.Empty;
        public string DataFromClient { get { return dataFromClient; } }

        public void StartClient(TcpClient inClientSocket, string clientNumber)
        {
            this.clientSocket = inClientSocket;
            this.clientNumber = clientNumber;

            t = new Thread(new ThreadStart(doChat));
            t.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[1024];
            dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            while (true)
            {
                try
                {
                    requestCount++;
                    NetworkStream stream = clientSocket.GetStream();
                    stream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    //dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    //Logger.Log(string.Format(">>{0} says: {1}", clientNumber, dataFromClient));

                    /*
                    rCount = Convert.ToString(requestCount);
                    serverResponse = string.Format("Server to client {0} {1}", clientNumber, rCount);
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    stream.Write(sendBytes, 0, sendBytes.Length);
                    stream.Flush();
                    Console.WriteLine("Server says: {0}", serverResponse);
                    */
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                    clientSocket.Close();
                    t.Abort();
                }
            }
        }

        internal void SendMessage(string dataFromClient)
        {
            byte[] bytesFrom = new byte[1024];
            NetworkStream stream = clientSocket.GetStream();
            stream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
            dataFromClient = Encoding.ASCII.GetString(bytesFrom);
            Logger.Log(string.Format(">>{0} says: {1}", clientNumber, dataFromClient));
        }
    }
}
