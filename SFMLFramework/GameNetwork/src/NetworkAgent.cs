using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameNetwork.src
{
    public abstract class NetworkAgent
    {
        protected static readonly int BUFFER_SIZE = 1024;
        protected byte[] bytes = new byte[BUFFER_SIZE];
        protected IPHostEntry ipHostInfo;
        protected IPAddress ipAddress;
        protected IPEndPoint endPoint;
        protected Socket socket;
        protected int port;
        protected XmlDocument xmlConfig;
    }
}
