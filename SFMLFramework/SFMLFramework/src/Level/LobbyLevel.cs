using NetData;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLFramework.src.Helper;
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
        public NetClient Client { get { return client; } }

        protected KeyboardInput keyboard;
        protected RenderWindow window;
        public static EventHandler LobbyIsDone;
        private bool isReady = false;

        public LobbyLevel(ref RenderWindow window)
        {
            sequence = 0;
            this.canvas = new GameObject("Canvas");
            this.labelCommands = new UIText(this.canvas);
            this.canvas.Components.Add(labelCommands);
            this.labelCommands.Display = (v) => this.labelCommands.SetMessage("R - Envia status de READY para o server");
            this.labelCommands.Display.Invoke(V2.Zero);
            this.canvas.Position = new Vector2f(Game.windowSize.X / 2, 0);
            this.gameObjects.Add(this.canvas);

            this.window = window;
            this.keyboard = new KeyboardInput(ref this.window);
            this.window.KeyReleased += (sender, e) =>
            {
                if (e.Code == Keyboard.Key.R && !isReady)
                    Ready();
            };

        }

        private void Ready()
        {
            isReady = true;
            var remoteReady = new RemoteClient();
            remoteReady.type = MessageType.ClientReady;
            this.client.SendMessageToServer(remoteReady);
        }

        public override void Initialize(ref LobbyLevel lobby)
        {
            this.client = new NetClient();
        }
    }
}
