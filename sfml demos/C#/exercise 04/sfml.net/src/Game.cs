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
        private Gamepad gamepad0;

        public static Clock DeltaTime = new Clock();
        private Tile map;
        private Tile background;
        private int[] level;

        private UIButton buttonClose;
        private UIButton buttonRestartClock;
        private UIButton buttonLoadTileMap;
        private List<UIButton> buttons;

        #endregion


        #region public

        public Game(string title)
        {
            this.windowTitle = title;
            this.window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), windowTitle);
            this.window.SetFramerateLimit(maxFPS);

            this.player = new Player();
            this.world = new World();
            this.gamepad0 = new Gamepad(0);

            this.window.KeyPressed += ProcessKeyboardPressed;
            this.window.KeyReleased += ProcessKeyboardReleased;

            this.window.MouseButtonPressed += ProcessMousePressed;
            this.window.MouseButtonReleased += ProcessMouseReleased;
            this.window.MouseWheelMoved += ProcessMouseWheelMoved;

            this.window.JoystickConnected += ProcessJoystickConnected;
            this.window.JoystickDisconnected += ProcessJoystickDisconnected;
            this.window.JoystickMoved += ProcessJoystickMoved;
            this.window.JoystickButtonPressed += ProcessJoystickButtonPressed;
            this.window.JoystickButtonReleased += ProcessJoystickButtonReleased;

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

            this.buttonClose = new UIButton("buttonCloseNormal.png", "buttonCloseClicked.png", new Vector2f(WINDOW_WIDTH - 96, 0));
            this.buttonClose.OnClickEvent += () =>
            {
                this.window.Close();
            };

            this.buttonLoadTileMap = new UIButton("buttonLoadNormal.png", "buttonLoadClicked.png", new Vector2f(WINDOW_WIDTH - 96, 32));
            this.buttonLoadTileMap.OnClickEvent += () =>
            {
                this.map = new Tile();
                if (!this.map.Load("tilemap.png", new Vector2u(32, 32), level, 16, 8))
                    throw new Exception("Error loading texture");
                this.buttonLoadTileMap.IsInteractable = false;
            };

            this.buttonRestartClock = new UIButton("buttonRestartNormal.png", "buttonRestartClicked.png", new Vector2f(WINDOW_WIDTH - 96, 64));
            this.buttonRestartClock.OnClickEvent += () =>
            {
                Console.WriteLine("Timer restarted manually:" + DeltaTime.Restart().AsSeconds() + "s");
            };

            this.buttons = new List<UIButton> { this.buttonRestartClock, this.buttonLoadTileMap, this.buttonClose };

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


        #region Keyboad

        private void ProcessKeyboardPressed(object sender, KeyEventArgs e)
        {
            ProcessKeyboardInput(e.Code, true);
        }

        private void ProcessKeyboardReleased(object sender, KeyEventArgs e)
        {
            ProcessKeyboardInput(e.Code, false);
        }

        private void ProcessKeyboardInput(Keyboard.Key key, bool isPressed)
        {
            if (key == Keyboard.Key.A)
                this.player.IsMovingLeft = isPressed;
            if (key == Keyboard.Key.S)
                this.player.IsMovingDown = isPressed;
            if (key == Keyboard.Key.D)
                this.player.IsMovingRight = isPressed;
            if (key == Keyboard.Key.W)
                this.player.IsMovingUp = isPressed;
        }

        #endregion


        #region Mouse 

        private void ProcessMousePressed(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Pressed: " + e.Button.ToString());
            if (e.Button == Mouse.Button.Left)
            {
                buttonClose.CheckClick(new Vector2f(e.X, e.Y));
                buttonRestartClock.CheckClick(new Vector2f(e.X, e.Y));
                buttonLoadTileMap.CheckClick(new Vector2f(e.X, e.Y));
            }
        }

        private void ProcessMouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta != 0)
                Console.WriteLine("Mouse wheel delta: " + e.Delta);
        }

        private void ProcessMouseReleased(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Released: " + e.Button.ToString());
            if (e.Button == Mouse.Button.Left)
            {
                buttonClose.CheckClick(new Vector2f(e.X, e.Y));
                buttonRestartClock.CheckClick(new Vector2f(e.X, e.Y));
                buttonLoadTileMap.CheckClick(new Vector2f(e.X, e.Y));
            }
        }

        #endregion


        #region Joystick

        private void ProcessJoystickConnected(object sender, JoystickConnectEventArgs e)
        {
            this.gamepad0 = new Gamepad(0);
        }

        private void ProcessJoystickDisconnected(object sender, JoystickConnectEventArgs e)
        {
            //this.gamepad0 = null;
        }

        private void ProcessJoystickMoved(object sender, JoystickMoveEventArgs e)
        {
            var speed = new Vector2f(this.gamepad0.GetAxisPosition(Joystick.Axis.X), this.gamepad0.GetAxisPosition(Joystick.Axis.Y));

            if (speed.X < 0)
                this.player.IsMovingLeft = true;
            if (speed.Y > 0)
                this.player.IsMovingDown = true;
            if (speed.X > 0)
                this.player.IsMovingRight = true;
            if (speed.Y < 0)
                this.player.IsMovingUp = true;
        }

        private void ProcessJoystickButtonPressed(object sender, JoystickButtonEventArgs e)
        {
            Console.WriteLine(string.Format("Joystick {0} pressed {1}", gamepad0.Index, e.Button));
        }

        private void ProcessJoystickButtonReleased(object sender, JoystickButtonEventArgs e)
        {
            Console.WriteLine(string.Format("Joystick {0} released {1}", gamepad0.Index, e.Button));
        }

        #endregion


        #region Private

        private void Update()
        {
            this.player.Update();
        }

        private void Render()
        {
            window.Clear();
            if (this.map != null)
                window.Draw(map);

            player.Display(window);

            this.buttons.ForEach(b => window.Draw(b.Sprite));

            window.Display();
        }

        #endregion
    }
}
