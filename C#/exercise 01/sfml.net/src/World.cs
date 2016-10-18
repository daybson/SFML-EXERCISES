using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace sfml.net.src
{
    class World
    {
        #region Fields

        private RectangleShape sky;
        private RectangleShape lineStreetLeft;
        private RectangleShape lineStreetRight;
        private RectangleShape street;

        private ConvexShape forestLeft;
        private ConvexShape forestRight;

        #endregion


        #region Public

        public World()
        {
            var origin = new Vector2f(0.0f, 0.0f);

            //sky preenche 1/3 da screen
            sky = new RectangleShape(new Vector2f(Game.WINDOW_WIDTH, Game.WINDOW_HEIGHT / 3));
            sky.Origin = origin;
            sky.FillColor = Color.Blue;
            sky.Position = new Vector2f(0, 0);

            street = new RectangleShape(new Vector2f(Game.WINDOW_WIDTH, Game.WINDOW_HEIGHT));
            street.FillColor = (new Color(127, 127, 127));
            street.Origin = origin;
            street.Position = new Vector2f(0, 200);

            //calcula a hipotenusa correspondente a metade da tela horizontal (ponto de fuga da estrada) com a altura até o sky (1/3 screen)
            var hipo = Math.Sqrt((double)((Game.WINDOW_WIDTH / 2 * Game.WINDOW_WIDTH / 2) + (2 * Game.WINDOW_HEIGHT / 3 * 2 * Game.WINDOW_HEIGHT / 3)));

            lineStreetLeft = new RectangleShape(new Vector2f((float)hipo, 5));
            lineStreetLeft.FillColor = Color.White;
            lineStreetLeft.Origin = origin;
            lineStreetLeft.Position = new Vector2f(0, Game.WINDOW_HEIGHT);
            lineStreetLeft.Rotation = -45;

            lineStreetRight = new RectangleShape(new Vector2f((float)hipo, 5));
            lineStreetRight.FillColor = Color.White;
            lineStreetRight.Origin = origin;
            lineStreetRight.Position = new Vector2f(Game.WINDOW_WIDTH, Game.WINDOW_HEIGHT);
            lineStreetRight.Rotation = -135;

            forestLeft = new ConvexShape(3);
            forestLeft.FillColor = Color.Green;
            forestLeft.SetPoint(0, new Vector2f(Game.WINDOW_WIDTH / 2, Game.WINDOW_HEIGHT / 3));
            forestLeft.SetPoint(1, new Vector2f(0, Game.WINDOW_HEIGHT / 3));
            forestLeft.SetPoint(2, new Vector2f(0, Game.WINDOW_HEIGHT));

            forestRight = new ConvexShape(3);
            forestRight.FillColor = Color.Green;
            forestRight.SetPoint(0, new Vector2f(Game.WINDOW_WIDTH / 2, Game.WINDOW_HEIGHT / 3));
            forestRight.SetPoint(1, new Vector2f(Game.WINDOW_WIDTH, Game.WINDOW_HEIGHT / 3));
            forestRight.SetPoint(2, new Vector2f(Game.WINDOW_WIDTH, Game.WINDOW_HEIGHT));
        }

        public void Display(RenderTarget window)
        {
            window.Draw(sky);
            window.Draw(street);
            window.Draw(forestLeft);
            window.Draw(forestRight);
            window.Draw(lineStreetLeft);
            window.Draw(lineStreetRight);
        }

        #endregion
    }
}
