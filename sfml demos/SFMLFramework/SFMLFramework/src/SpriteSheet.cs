using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

public class SpriteSheet
{
    #region Fields

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


    #region Public

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
        sprite.Position = new Vector2f(200, 200);

        currentFrame = 0;
        currentFrameTime = 0;

    }

    public void UpdateAnimation(float deltaTime, Mover.EDirection direction)
    {
        currentFrameTime += deltaTime;

        if (currentFrameTime >= frameTime)
        {
            if (currentFrame + 1 == columns)
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

    public void SetDirection(Mover.EDirection direction)
    {
        int newTop = 0;

        switch (direction)
        {
            case Mover.EDirection.Left: newTop = tileHeight; break;
            case Mover.EDirection.Right: newTop = tileHeight * (rows - 2); break;
            case Mover.EDirection.Up: newTop = tileHeight * (rows - 1); break;
            case Mover.EDirection.Down: newTop = 0; break;           
        }

        if (tile.Top != newTop)
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
