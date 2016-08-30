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

        private Player player1;
        private Player player2;

        public static Clock DeltaTime = new Clock();

        #endregion


        #region public

        public Game(string title)
        {
            this.windowTitle = title;
            this.window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), windowTitle);
            this.window.SetFramerateLimit(maxFPS);

            this.player1 = new Player();
            this.player2 = new Player();

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
                this.player1.IsMovingLeft = isPressed;
            if (key == Keyboard.Key.S)
                this.player1.IsMovingDown = isPressed;
            if (key == Keyboard.Key.D)
                this.player1.IsMovingRight = isPressed;
            if (key == Keyboard.Key.W)
                this.player1.IsMovingUp = isPressed;

            if (key == Keyboard.Key.Left)
                this.player2.IsMovingLeft = isPressed;
            if (key == Keyboard.Key.Down)
                this.player2.IsMovingDown = isPressed;
            if (key == Keyboard.Key.Right)
                this.player2.IsMovingRight = isPressed;
            if (key == Keyboard.Key.Up)
                this.player2.IsMovingUp = isPressed;
        }

        #endregion


        #region Mouse 

        private void ProcessMousePressed(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Pressed: " + e.Button.ToString());
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

        #endregion


        #region Joystick

        private void ProcessJoystickConnected(object sender, JoystickConnectEventArgs e)
        {
            //this.gamepad0 = new Gamepad(0);
        }

        private void ProcessJoystickDisconnected(object sender, JoystickConnectEventArgs e)
        {
            //this.gamepad0 = null;
        }

        private void ProcessJoystickMoved(object sender, JoystickMoveEventArgs e)
        {
            /*
            var speed = new Vector2f(this.gamepad0.GetAxisPosition(Joystick.Axis.X), this.gamepad0.GetAxisPosition(Joystick.Axis.Y));

            if (speed.X < 0)
                this.player1.IsMovingLeft = true;
            if (speed.Y > 0)
                this.player1.IsMovingDown = true;
            if (speed.X > 0)
                this.player1.IsMovingRight = true;
            if (speed.Y < 0)
                this.player1.IsMovingUp = true;
                */
        }

        private void ProcessJoystickButtonPressed(object sender, JoystickButtonEventArgs e)
        {
            //Console.WriteLine(string.Format("Joystick {0} pressed {1}", gamepad0.Index, e.Button));
        }

        private void ProcessJoystickButtonReleased(object sender, JoystickButtonEventArgs e)
        {
            //Console.WriteLine(string.Format("Joystick {0} released {1}", gamepad0.Index, e.Button));
        }

        #endregion


        #region Private

        private void Update()
        {
            this.player1.Update();
            this.player2.Update();

            //Console.WriteLine("Collision state: " + Collision.CheckCollisionRectangleAxisAligned(this.player1.BoundingBox, this.player2.BoundingBox));
            //Console.WriteLine("Collision circle state: " + Collision.CheckCollisionSphere(this.player1.BoundingBox, this.player2.BoundingBox));
            //Console.WriteLine("Collision circle state: " + Collision.CheckCollisionExtentsRectangleAxisAligned(this.player1.BoundingBox, this.player2.BoundingBox));
            Console.WriteLine("Collision axis separating state: " + Collision.CheckCollistionSeparatingAxisTheorem(this.player1.BoundingBox, this.player2.BoundingBox));

        }

        private void Render()
        {
            window.Clear();

            player1.Display(window);
            player2.Display(window);

            Collision.collisionShapes.ForEach(s => window.Draw(s));

            window.Display();
        }

        #endregion
    }
}
