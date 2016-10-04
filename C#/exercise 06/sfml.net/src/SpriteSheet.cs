using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace sfml.net.src
{
    class SpriteSheet
    {
        #region

        private Texture texture;
        private IntRect tile;
        private Sprite sprite;
        public Sprite Sprite { get { return sprite; } }
        private string pathTexture;

        private int tileHeight;
        private int tileWidth;
        private int rows;
        private int columns;
        private int frameCount;
        private float animationTime;
        private float frameTime;
        private float currentFrameTime;
        private float currentFrame;

        #endregion


        #region

        public SpriteSheet(string pathTexture)
        {
            this.pathTexture = pathTexture;

            //read a txt file 'metadata' with informations about the sprite sheet
            var metaFile = pathTexture.Replace(".png", ".txt");
            var lines = File.ReadAllLines(metaFile);

            int.TryParse(lines[0], out tileWidth);
            int.TryParse(lines[1], out tileHeight);
            int.TryParse(lines[2], out rows);
            int.TryParse(lines[3], out columns);
            int.TryParse(lines[4], out frameCount);
            float.TryParse(lines[5], out animationTime);
            float.TryParse(lines[6], out frameTime);


            //load texture or throw expcetion
            texture = new Texture(pathTexture);
            tile = new IntRect(0, 0, tileWidth, tileHeight);
            sprite = new Sprite(texture, tile);
            sprite.Position = new Vector2f(0, 0);
            sprite.Position = new Vector2f(Game.WINDOW_WIDTH / 2, Game.WINDOW_HEIGHT / 2);

            currentFrame = 0;
            currentFrameTime = 0;

        }

        public void UpdateAnimation()
        {
            currentFrameTime += Game.DeltaTime.ElapsedTime.AsSeconds() * 100;

            if(currentFrameTime >= frameTime)
            {
                if(currentFrame + 1 == columns)
                {
                    currentFrame = 0;
                    tile.Left = 0;
                }
                else
                {
                    currentFrame++;
                    tile.Left += tileWidth;
                }

                currentFrameTime = 0.0f;
                sprite.TextureRect = tile;
            }
        }

        public void SetDirection(Vector2f direction)
        {
            int newTop = 0;
            if(direction.X < 0) //left
                newTop = tileHeight;
            else if(direction.X > 0) //right
                newTop = tileHeight * (rows - 2);
            else if(direction.Y > 0) //down
                newTop = 0;
            else if(direction.Y < 0) //up
                newTop = tileHeight * (rows - 1);

            if(tile.Top != newTop)
            {
                tile.Top = newTop;
                tile.Left = 0;
                currentFrame = 0;
                currentFrameTime = 0.0f;
                sprite.TextureRect = tile;
            }
        }

        #endregion
    }
}
