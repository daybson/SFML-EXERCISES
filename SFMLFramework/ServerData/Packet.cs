using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ServerData
{
    [Serializable]
    public class Packet
    {
        public string Data;
        public int PacketInt;
        public bool PacketBool;
        public string SenderID;
        public PacketType PacketType;
        public static readonly int PacketSize = 8096;

        public Packet(PacketType type, string senderID)
        {
            Data = string.Empty;
            SenderID = senderID;
            PacketType = type;
        }

        public Packet(byte[] packetBytes)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream(packetBytes);

            var p = (Packet)bf.Deserialize(ms);
            ms.Close();
            Data = p.Data;
            PacketInt = p.PacketInt;
            SenderID = p.SenderID;
            PacketBool = p.PacketBool;
            PacketType = p.PacketType;
        }


        public byte[] ToBytes()
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, this);
            byte[] bytes = ms.ToArray();
            ms.Close();

            return bytes;
        }


        public static string GetIP4Address()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (var ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip.ToString();
            }

            return "127.0.0.1";
        }
    }


    public enum PacketType
    {
        Registration,
        Chat
    }
}