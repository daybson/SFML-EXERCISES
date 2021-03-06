﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NetData
{
    [Serializable]
    public enum MessageType
    {
        Handhsake,
        ClientReady,
        StartParty,
        InstantiatePlayers,
        Disconnect,
        Update
    }

    [Serializable]
    public class RemoteClient
    {
        #region Fields

        public string clientID = string.Empty;
        public string name;
        public float posX;
        public float posY;
        public MessageType type;

        #endregion


        #region Public

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

        public string ToString()
        {
            return String.Format("Client ID: {0}\nName: {1}\nPosX: {2}\nPosY: {3}\nType: {4}", this.clientID, this.name, this.posX, this.posY, this.type.ToString());
        }
        #endregion
    }
}
