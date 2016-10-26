using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NetData
{
    [Serializable]
    public enum Type
    {
        Handhsake,
        Update
    }

    [Serializable]
    public class RemoteClient
    {
        #region Fields
        //public TcpClient tcpClient;
        public string clientID = string.Empty;
        public string name;
        public float posX;
        public float posY;
        public Type type;

        #endregion


        #region Public

        //public RemoteClient(TcpClient tcpClient)
        //{
        //this.tcpClient = tcpClient;
        //}

        public static byte[] Serialize(RemoteClient remote)
        {
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, remote);
                return memoryStream.ToArray();
            }
        }

        public static RemoteClient Deserialize(byte[] buffer)
        {
            try
            {
                using (var memoryStream = new MemoryStream(buffer))
                {
                    var remote = new BinaryFormatter().Deserialize(memoryStream);
                    return (RemoteClient)remote;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Deserialize error! {0}", e.Message);
                return null;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is RemoteClient)
            {
                var o = (RemoteClient)obj;
                return this.clientID.Equals(o.clientID);
            }

            return false;
        }

        #endregion
    }
}
