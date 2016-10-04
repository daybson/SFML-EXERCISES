using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;

namespace sfml.net.src
{
    class Player
    {
        #region Fields

        public bool IsMovingLeft { get; set; }
        public bool IsMovingDown { get; internal set; }
        public bool IsMovingUp { get; internal set; }
        public Vector2f Origin { get { return origin.Position; } }
        public Vector2f Direction;

        private float speed = 4f;
        public bool IsMovingRight;

        private SpriteSheet spriteSheet;
        public SpriteSheet SpriteSheet { get { return spriteSheet; } }

        private CircleShape collisionBound;
        private CircleShape origin;
        public CircleShape BoundingBox { get { return collisionBound; } }

        private string spriteSheetName = "dragon.png";
        private string roarFXFilePath = "dragonRoarFX.wav";
        private Sound roarFX;
        private SoundBuffer roarFXBuffer;

        #endregion


        #region Public

        public Player()
        {
            spriteSheet = new SpriteSheet(spriteSheetName);
            Direction = new Vector2f(0.0f, 0.0f);
            spriteSheet.Sprite.Position = Direction;

            collisionBound = new CircleShape(spriteSheet.Sprite.GetLocalBounds().Width / 2, 5);//(new Vector2f(spriteSheet.Sprite.GetLocalBounds().Width, spriteSheet.Sprite.GetLocalBounds().Height));
            collisionBound.OutlineColor = Color.Magenta;
            collisionBound.FillColor = new Color(0, 0, 0, 0);
            collisionBound.OutlineThickness = 2;

            origin = new CircleShape(2);//(new Vector2f(spriteSheet.Sprite.GetLocalBounds().Width, spriteSheet.Sprite.GetLocalBounds().Height));
            origin.OutlineColor = Color.Magenta;
            origin.FillColor = Color.Magenta;
            origin.OutlineThickness = 2;
            origin.Position = new Vector2f(spriteSheet.Sprite.GetLocalBounds().Width / 2, spriteSheet.Sprite.GetLocalBounds().Height / 2);
            SetAbsolutePosition(new Vector2f(Game.WINDOW_WIDTH / 2 / speed, Game.WINDOW_HEIGHT / 2 / speed));

            roarFXBuffer = new SoundBuffer(roarFXFilePath);
            roarFX = new Sound(roarFXBuffer);
            roarFX.Loop = false;
        }

        public void Update()
        {
            Direction = new Vector2f(0, 0);

            if (IsMovingLeft)
                Direction.X -= 1;
            if (IsMovingDown)
                Direction.Y += 1;
            if (IsMovingRight)
                Direction.X += 1;
            if (IsMovingUp)
                Direction.Y -= 1;

            if (Direction != new Vector2f(0, 0))
            {
                Translate();
                spriteSheet.SetDirection(Direction);
            }

            spriteSheet.UpdateAnimation();
        }

        public void Display(RenderTarget window)
        {
            window.Draw(spriteSheet.Sprite);
            window.Draw(collisionBound);
            window.Draw(origin);
        }

        public void TriggerRoarFX()
        {
            if (roarFX != null)
                roarFX.Play();
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
            collisionBound.Position += Direction;
            origin.Position += Direction;
        }

        #endregion
    }
}
