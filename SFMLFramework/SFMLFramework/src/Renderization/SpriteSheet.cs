using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

/// <summary>
/// Define um objeto que armazena uma Texture com sequencias de animações quadro a quadro baseadas em Tiles, e que é capaz de gerenciá-los para se comportarem como uma animação
/// </summary>
public class SpriteSheet
{
    #region Fields

    /// <summary>
    /// Textura carregada
    /// </summary>
    private Texture texture;

    /// <summary>
    /// tile do sprite
    /// </summary>
    private IntRect tile;

    /// <summary>
    /// Sprite em exibição no moment
    /// </summary>
    private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }

    public int TileHeight { get { return tileHeight; } }
    public int TileWidth { get { return tileWidth; } }

    public Vector2i size;
    public Vector2i Size { get { return size; } }

    private string pathTexture;

    /// <summary>
    /// Altura dos tiles da spritesheet
    /// </summary>
    private int tileHeight;

    /// <summary>
    /// Largura dos tiles da spritesheet
    /// </summary>
    private int tileWidth;

    /// <summary>
    /// Quantidade de colunas da spritesheet
    /// </summary>
    private int rows;

    /// <summary>
    /// Quantidade de colunas do spritesheet
    /// </summary>
    private int columns;

    /// <summary>
    /// Total de frames do spritesheet
    /// </summary>
    private int frameCount;

    /// <summary>
    /// Tempo total de duração da animação
    /// </summary>
    private float animationTime;

    /// <summary>
    /// Tempo de exibição de cada frame
    /// </summary>
    private float frameTime;

    /// <summary>
    /// Contador do tempo de exibição do frame atual
    /// </summary>
    private float currentFrameTime;

    /// <summary>
    /// Frame atual da animação
    /// </summary>
    private float currentFrame;

    #endregion


    #region Public

    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="pathTexture">Caminho da textura a ser carregada</param>
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

        this.size = new Vector2i(tileWidth, tileHeight);

        currentFrame = 0;
        currentFrameTime = 0;

    }

    /// <summary>
    /// Atualiza o frame da animação da spritesheet
    /// </summary>
    public void UpdateAnimation(float deltaTime)
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

    /// <summary>
    /// Orienta o sprite de acordo com a direção de movimento
    /// </summary>
    /// <param name="direction">Orientação de movimento</param>
    public void SetDirection(EDirection direction)
    {
        int newTop = 0;

        switch (direction)
        {
            case EDirection.Left: newTop = tileHeight; break;
            case EDirection.Right: newTop = tileHeight * (rows - 2); break;
            case EDirection.Up: newTop = tileHeight * (rows - 1); break;
            case EDirection.Down: newTop = 0; break;
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
