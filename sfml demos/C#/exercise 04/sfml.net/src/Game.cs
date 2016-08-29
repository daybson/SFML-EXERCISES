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

            this.window.KeyPressed += ProcessKeyboardPressed;
            this.window.KeyReleased += ProcessKeyboardReleased;
            this.window.MouseButtonPressed += ProcessMousePressed;
            this.window.MouseButtonReleased += ProcessMouseReleased;
            this.window.MouseWheelMoved += ProcessMouseWheelMoved;

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


        private void ProcessKeyboardPressed(object sender, KeyEventArgs e)
        {
            ProcessKeyboardInput(e.Code, true);
        }

        private void ProcessKeyboardReleased(object sender, KeyEventArgs e)
        {
            ProcessKeyboardInput(e.Code, false);
        }

        private void ProcessMousePressed(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Pressed: " + e.Button.ToString());
            if (e.Button == Mouse.Button.Left)
                button.CheckClick(new Vector2f(e.X, e.Y));
        }

        private void ProcessMouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta != 0)
                Console.WriteLine("Mouse wheel delta: " + e.Delta);
        }

        private void ProcessMouseReleased(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Released: " + e.Button.ToString());
        }

        private void ProcessKeyboardInput(Keyboard.Key key, bool isPressed)
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
