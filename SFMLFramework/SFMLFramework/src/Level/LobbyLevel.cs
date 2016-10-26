using NetData;
using SFMLFramework.src.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework.src.Level
{
    public class LobbyLevel : GameLevel
    {
        protected NetClient client;

        public override void Initialize()
        {
            this.client = new NetClient();
            this.client.ReceiveMessageFromServer();
        }
    }
}
