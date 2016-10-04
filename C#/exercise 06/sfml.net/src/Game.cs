using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.System;
using SFML.Audio;

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

        public static Clock DeltaTime = new Clock();

        private Music music;
        private string musicFilePath = "98_Lost_Mine.wav";

        private Sound bells;
        private SoundBuffer bellsBuffer;
        private string bellsFilePath = "bells004.wav";
        private CircleShape gizmo;

        ListenerAgent listenerAgent;

        #endregion


        #region public

        public Game(string title)
        {
            this.listenerAgent = new ListenerAgent(new Vector2f(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2));

            bellsBuffer = new SoundBuffer(bellsFilePath);
            bells = new Sound(bellsBuffer);
            bells.MinDistance = 50;
            bells.Attenuation = 2;
            bells.Loop = true;
            bells.Position = new Vector3f(WINDOW_WIDTH / 3, 0, WINDOW_HEIGHT / 3);
            bells.Volume = 100;           
            gizmo = new CircleShape(3);
            gizmo.Position = new Vector2f(bells.Position.X, bells.Position.Z);
            gizmo.OutlineColor = Color.Red;
            gizmo.OutlineThickness = 2;
            gizmo.FillColor = Color.Transparent;
            bells.Play();

            this.windowTitle = title;
            this.window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), windowTitle);
            this.window.SetFramerateLimit(maxFPS);

            this.player1 = new Player();

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

            music = new Music(musicFilePath);
            music.Play();

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

            if (key == Keyboard.Key.Return)
                this.player1.TriggerRoarFX();

            if (key == Keyboard.Key.Equal && isPressed)
            {
                if (music.Status == SoundStatus.Playing)
                    music.Pause();
                else
                    music.Play();
            }

            if (key == Keyboard.Key.Add && isPressed)
                music.Volume++;

            if (key == Keyboard.Key.Subtract && isPressed)
                music.Volume--;
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

            this.listenerAgent.Update(player1);
        }

        private void Render()
        {
            window.Clear();

            player1.Display(window);

            Collision.collisionShapes.ForEach(s => window.Draw(s));

            window.Draw(this.listenerAgent.Gizmo);
            window.Draw(gizmo);

            window.Display();
        }

        #endregion
    }
}
