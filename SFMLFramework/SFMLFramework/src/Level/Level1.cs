using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLFramework.src.Audio;
using SFMLFramework.src.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework.src.Level
{
    public class Level1 : GameLevel
    {
       
        public Level1(ref RenderWindow window, ref KeyboardInput keyboard)
        {
            this.window = window;
            this.keyboard = keyboard;
            this.gameObjects = new List<GameObject>();
            this.canvas = new GameObject("Canvas");
            this.labelCommands = new UIText(this.canvas);
            this.canvas.Components.Add(labelCommands);
            this.levelMusicController = new MusicController();
        }


        public override void Initialize()
        {
            LoadScenario();
            SetupGameObjects();
            StartNetworkRoom();
        }

        private void LoadScenario()
        {
            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Down, new Vector2f(0, Game.windowSize.Y - 32)));
            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Right, new Vector2f(Game.windowSize.X - 33, 29)));
            this.gameObjects.Add(GameObjectCreator.CreatePlatform(EDirection.Left, new Vector2f(0, 32)));
        }

        private void SetupGameObjects()
        {
            this.levelMusicController.LoadMusic("nature024.wav");
            this.levelMusicController.PlayAudio("nature024");
            this.levelMusicController.ChangeVolume(25);

            this.labelCommands.Display = (v) => this.labelCommands.SetMessage(
            "A = move player esquerda\n" +
            "D = move player direita\n" +
            "Seta esqueda = move cubo1 esquerda\n" +
            "Seta direita = move cubo1 direita\n" +
            "M = ataque magia\n" +
            "P = ataque `punch\n" +
            "K = ataque kick\n");
            this.labelCommands.Display.Invoke(V2.Zero);

            this.canvas.Position = new Vector2f(Game.windowSize.X / 2, 0);
            this.gameObjects.Add(this.canvas);

            var p = GameObjectCreator.CreatePlayer(ref this.keyboard);
            p.Position = V2.Right * 650;
            this.gameObjects.Add(p);

            var wolf = GameObjectCreator.CreateWolf(new Vector2f(33, 450));
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Right) wolf.GetComponent<Rigidbody>().AddForce(V2.Right * 200); };
            this.window.KeyPressed += (sender, e) => { if (e.Code == Keyboard.Key.Left) wolf.GetComponent<Rigidbody>().AddForce(V2.Left * 200); };

            this.gameObjects.Add(wolf);
        }

        private void StartNetworkRoom()
        {
            WaitForPlayers();
        }

        private void WaitForPlayers()
        {
            //...   
            InstantiatePlayer();
        }

        private void InstantiatePlayer()
        {

        }
    }
}
