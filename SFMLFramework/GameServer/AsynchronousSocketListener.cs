using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class AsynchronousServer
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static List<Socket> clients = new List<Socket>();
        private Thread thread;
        private static IPHostEntry ipHostInfo;
        private static IPAddress ipAddress;
        private static IPEndPoint localEndPoint;
        private static Socket socket;
        private static TcpListener tcpListener;

        public AsynchronousServer()
        {
            ipHostInfo = Dns.Resolve("127.0.0.1");
            ipAddress = ipHostInfo.AddressList[0];

            tcpListener = new TcpListener(ipAddress, 2929);
            tcpListener.Start();
            Console.WriteLine("Waiting for clients...");

            /*
            // Establish the local endpoint for the socket. The DNS name of the computer running the listener is "host.contoso.com".
            localEndPoint = new IPEndPoint(ipAddress, 2929);

            // Create a TCP/IP socket.
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEndPoint);
            serverSocket.Listen(100);

            thread = new Thread(new ThreadStart(StartListening));
            thread.Start();
            */
        }

        public void Start()
        {
            var t = new Thread(new ThreadStart(StartListening));
            t.Start();
        }


        public static void StartListening()
        {
            var t = new Thread(new ThreadStart(StartListening));

            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                // Set the event to nonsignaled state.
                allDone.Reset();
                var ar = tcpListener.BeginAcceptSocket(new AsyncCallback(AcceptCallback), tcpListener);
                allDone.WaitOne();

                /*lock (clients)
                {
                    clients.Add(socket);
                }*/
                t.Start();

                while (true)
                {
                    lock (clients)
                    {
                        foreach (var sock in clients)
                        {
                            // Create the state object.
                            //StateObject state = new StateObject();
                           // sock.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            var tcp = (TcpListener)ar.AsyncState;
            socket = tcp.EndAcceptSocket(ar);
            clients.Add(socket);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = socket;

            /*
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            */
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read more data.
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                    // Echo the data back to the client.
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                if (!handler.Connected)
                {
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    Console.WriteLine("Client offline");
                }

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
