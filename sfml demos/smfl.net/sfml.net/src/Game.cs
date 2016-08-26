using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sfml.net.src
{
    public class Game
    {
        #region Fields

        private RenderWindow window;
        private uint resolutionWidth = 800;
        private uint resolutionHeight = 600;
        private string windowTitle = "SFML.Net";
        private readonly uint maxFPS = 60;

        #endregion


        #region public

        public RenderWindow Window
        {
            get { return window; }
        }

        public Game(string title)
        {
            this.windowTitle = title;
            this.window = new RenderWindow(new VideoMode(resolutionWidth, resolutionHeight), windowTitle);
            this.window.SetFramerateLimit(maxFPS);

            this.window.Closed += (sender, e) =>
            {
                ((RenderWindow)sender).Close();
            };
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);
                window.Display();
            }
        }

        #endregion
    }
}
