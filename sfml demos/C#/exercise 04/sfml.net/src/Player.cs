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

        public Vector2f Direction;

        private RectangleShape body;
        private RectangleShape legLeft;
        private RectangleShape legRight;
        private RectangleShape armLeft;
        private RectangleShape armRight;
        private CircleShape head;
        private CircleShape hat;
        private CircleShape nose;

        private float speed = 4f;
        public bool IsMovingRight;

        private SpriteSheet spriteSheet;
        private string spriteSheetName = "dragon.png";

        #endregion


        #region Public

        public Player()
        {
            spriteSheet = new SpriteSheet(spriteSheetName);
            Direction = new Vector2f(0.0f, 0.0f);
            spriteSheet.Sprite.Position = Direction;
            SetAbsolutePosition(new Vector2f(Game.WINDOW_WIDTH / 2 / speed, Game.WINDOW_HEIGHT / 2 / speed));
        }

        public void Update()
        {
            Direction = new Vector2f(0, 0);

            if(IsMovingLeft)
                Direction.X -= 1;
            if(IsMovingDown)
                Direction.Y += 1;
            if(IsMovingRight)
                Direction.X += 1;
            if(IsMovingUp)
                Direction.Y -= 1;

            if(Direction != new Vector2f(0, 0))
            {
                Translate();
                spriteSheet.SetDirection(Direction);
            }

            spriteSheet.UpdateAnimation();
        }

        public void Display(RenderTarget window)
        {
            window.Draw(spriteSheet.Sprite);
        }

        #endregion


        #region Private

        private void SetAbsolutePosition(Vector2f position)
        {
            Direction = position;
            Translate();
            Direction = new Vector2f(0, 0);
        }

        private void Translate()
        {
            Direction *= speed;
            spriteSheet.Sprite.Position += Direction;
        }

        #endregion
    }
}
