using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient clientSocket = new TcpClient();
            NetworkStream serverStream;
            clientSocket.Connect("127.0.0.1", 2929);

            while (true)
            {
                serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes(Console.ReadLine());
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                byte[] inStream = new byte[1024];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                string returndata = Encoding.ASCII.GetString(inStream);
                //Console.WriteLine("Data from Server : " + returndata);
            }
        }
    }
}
