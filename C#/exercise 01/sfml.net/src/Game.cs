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

        #endregion


        #region public

        public Game(string title)
        {
            this.windowTitle = title;
            this.window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), windowTitle);
            this.window.SetFramerateLimit(maxFPS);

            this.player = new Player();
            this.player.Move(new Vector2f(WINDOW_WIDTH/2, WINDOW_HEIGHT/2));
            this.world = new World();

            this.window.KeyPressed += ProcessPressedEvents;
            this.window.KeyReleased += ProcessReleasedEvents;

            this.window.Closed += (sender, e) =>
            {
                Console.WriteLine("Game Over!");
                ((RenderWindow)sender).Close();
            };
        }

        public void Run()
        {
            while(window.IsOpen)
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

        private void ProcessInput(Keyboard.Key key, bool isPressed)
        {
            if(key == Keyboard.Key.A)
            {
                this.player.IsMovingLeft = isPressed;
            }
            if(key == Keyboard.Key.S)
            {
                this.player.IsMovingDown = isPressed;
            }
            if(key == Keyboard.Key.D)
            {
                this.player.IsMovingRight = isPressed;
            }
            if(key == Keyboard.Key.W)
            {
                this.player.IsMovingUp = isPressed;
            }
        }

        private void Update()
        {
            var movement = new Vector2f();

            if (this.player.IsMovingUp)
                movement.Y -= 1f;
            if (this.player.IsMovingDown)
                movement.Y += 1f;
            if (this.player.IsMovingLeft)
                movement.X -= 1f;
            if (this.player.IsMovingRight)
                movement.X += 1f;

            this.player.Move(movement);
        }

        private void Render()
        {
            window.Clear();
            world.Display(window);
            player.Display(window);
            window.Display();
        }

        #endregion
    }
}
