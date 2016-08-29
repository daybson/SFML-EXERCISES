using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.System;

namespace sfml.net.src
{
    public class Game
    {
        #region Fields

        public static readonly uint WINDOW_WIDTH = 800;
        public static readonly uint WINDOW_HEIGHT = 600;

        private RenderWindow window;
        private string windowTitle = "SFML.Net";
        private readonly uint maxFPS = 60;

        private Player player;
        private World world;

        public static Clock DeltaTime = new Clock();
        Tile map;
        Tile background;
        int[] level;

        UIButton button;

        #endregion


        #region public

        public Game(string title)
        {
            this.windowTitle = title;
            this.window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), windowTitle);
            this.window.SetFramerateLimit(maxFPS);

            this.player = new Player();
            this.world = new World();

            this.window.KeyPressed += ProcessPressedEvents;
            this.window.KeyReleased += ProcessReleasedEvents;
            this.window.MouseButtonPressed += ProcessMouseInput;

            level = new int[128]
            {
                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0,
                1, 1, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3,
                0, 1, 0, 0, 2, 0, 3, 3, 3, 0, 1, 1, 1, 0, 0, 0,
                0, 1, 1, 0, 3, 3, 3, 0, 0, 0, 1, 1, 1, 2, 0, 0,
                0, 0, 1, 0, 3, 0, 2, 2, 0, 0, 1, 1, 1, 1, 2, 0,
                2, 0, 1, 0, 3, 0, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1,
                0, 0, 1, 0, 3, 2, 2, 2, 0, 0, 0, 0, 1, 1, 1, 1
            };

            map = new Tile();
            background = new Tile();

            if (!map.Load("tilemap.png", new Vector2u(32, 32), level, 16, 8))
                throw new Exception("Error loading texture");

            if (!background.Load("background.png", 512, 256))
                throw new Exception("Error loading texture");

            button = new UIButton("buttonNormal.png", "buttonClicked.png", new Vector2f(10, 10));

            this.window.Closed += (sender, e) =>
            {
                Console.WriteLine("Game Over!");
                ((RenderWindow)sender).Close();
            };
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                Update();
                Render();
                DeltaTime.Restart();
            }
        }

        #endregion


        #region Private


        private void ProcessPressedEvents(object sender, KeyEventArgs e)
        {
            ProcessInput(e.Code, true);
        }

        private void ProcessReleasedEvents(object sender, KeyEventArgs e)
        {
            ProcessInput(e.Code, false);
        }

        private void ProcessMouseInput(object sender, MouseButtonEventArgs args)
        {
            if (args.Button == Mouse.Button.Left)
                button.CheckClick(new Vector2f(args.X, args.Y));
        }

        private void ProcessInput(Keyboard.Key key, bool isPressed)
        {
            if (key == Keyboard.Key.A)
            {
                this.player.IsMovingLeft = isPressed;
            }
            if (key == Keyboard.Key.S)
            {
                this.player.IsMovingDown = isPressed;
            }
            if (key == Keyboard.Key.D)
            {
                this.player.IsMovingRight = isPressed;
            }
            if (key == Keyboard.Key.W)
            {
                this.player.IsMovingUp = isPressed;
            }
        }

        private void Update()
        {
            this.player.Update();
        }

        private void Render()
        {
            window.Clear();
            window.Draw(map);
            //window.Draw(background);
            player.Display(window);
            window.Draw(button.Sprite);
            window.Display();
        }

        #endregion
    }
}
