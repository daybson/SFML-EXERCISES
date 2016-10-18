using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace sfml.net.src
{
    class Player
    {
        #region Fields

        public bool IsMovingLeft { get; set; }
        public bool IsMovingDown { get; internal set; }
        public bool IsMovingUp { get; internal set; }

        RectangleShape body;
        RectangleShape legLeft;
        RectangleShape legRight;
        RectangleShape armLeft;
        RectangleShape armRight;
        CircleShape head;
        CircleShape hat;
        CircleShape nose;
        internal bool IsMovingRight;

        #endregion


        #region Public

        public Player()
        {
            Vector2f position = new Vector2f(0.0f, 0.0f);

            body = new RectangleShape(new Vector2f(15, 50));
            body.Origin = (position);
            body.Position = new Vector2f(-7.5f, -50);
            body.FillColor = (Color.Magenta);

            head = new CircleShape(10);
            head.Origin = (position);
            head.Position = new Vector2f(body.Position.X - 2.5f, -body.Size.Y - 10);
            head.FillColor = (Color.Yellow);

            legLeft = new RectangleShape(new Vector2f(5, 20));
            legLeft.Origin = (position);
            legLeft.Position = new Vector2f(body.Position.X - 2.5f, 0);
            legLeft.FillColor = (Color.Yellow);

            legRight = new RectangleShape(new Vector2f(5, 20));
            legRight.Origin = (position);
            legRight.Position = new Vector2f(-body.Position.X - 2.5f, 0);
            legRight.FillColor = (Color.Yellow);

            armLeft = new RectangleShape(new Vector2f(-30, 3));
            armLeft.Origin = (position);
            armLeft.Position = new Vector2f(body.Position.X - 5, -45);
            armLeft.Rotation = (-75);
            armLeft.FillColor = (Color.Yellow);

            armRight = new RectangleShape(new Vector2f(30, 3));
            armRight.Origin = (position);
            armRight.Position = new Vector2f(-body.Position.X + 5, -45);
            armRight.Rotation = (75);
            armRight.FillColor = (Color.Yellow);

            hat = new CircleShape(10, 3);
            hat.Origin = (position);
            hat.Position = new Vector2f(body.Position.X - 2.5f, -body.Size.Y - 10 - 15);
            hat.FillColor = (Color.Red);

            nose = new CircleShape(3);
            nose.Origin = (position);
            nose.Position = new Vector2f(body.Position.X + head.Radius / 2f, head.Position.Y + head.Radius);
            nose.FillColor = (Color.Red);
        }

        internal void Display(RenderWindow mWindow)
        {

            mWindow.Draw(body);
            mWindow.Draw(head);
            mWindow.Draw(legLeft);
            mWindow.Draw(legRight);
            mWindow.Draw(armLeft);
            mWindow.Draw(armRight);
            mWindow.Draw(hat);
            mWindow.Draw(nose);
        }

        public void Move(Vector2f delta)
        {
            body.Position += (delta);
            head.Position += (delta);
            legLeft.Position += (delta);
            legRight.Position += (delta);
            armLeft.Position += (delta);
            armRight.Position += (delta);
            hat.Position += (delta);
            nose.Position += (delta);
        }

        #endregion
    }
}
