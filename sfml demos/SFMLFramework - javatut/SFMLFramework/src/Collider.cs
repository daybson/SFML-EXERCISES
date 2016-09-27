using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Determina os tipos de colisão possíveis
/// </summary>
public enum ECollisionType
{
    Elastic,
    PartialInelastic,
    Inelastic,
    Trigger,
    None
}

public class Collider
{
    /// <summary>
    /// Shape de visualização do collider
    /// </summary>
    private RectangleShape shape;
    private FloatRect bound;
    /// <summary>
    /// Direção onde o collider será posicionado em relação ao sprite
    /// </summary>
    private EDirection direction;
    /// <summary>
    /// Espessura do collider quando renderizado
    /// </summary>
    private int colliderThickness;
    /// <summary>
    /// Dimensão do sprite (wifth, height)
    /// </summary>
    private SFML.System.Vector2f spriteDimension;

    public RectangleShape Shape { get { return shape; } }
    public FloatRect Bound { get { return bound; } }
    public EDirection Direction { get { return direction; } }

    public Collider(SFML.System.Vector2f spriteDimension, EDirection direction, int colliderThickness)
    {
        this.direction = direction;
        this.colliderThickness = colliderThickness;
        this.spriteSheet = spriteDimension;

        switch (this.direction)
        {
            case EDirection.Botton:
                shape = new RectangleShape(new Vector2f(this.spriteSheet.TileWidth, this.colliderThickness));
                bound = new FloatRect(this.spriteSheet.Sprite.Position.X, this.spriteSheet.Sprite.Position.Y + this.spriteSheet.TileHeight - this.colliderThickness, this.spriteSheet.TileWidth, this.colliderThickness);
                break;
            case EDirection.Top:
                shape = new RectangleShape(new Vector2f(this.spriteSheet.TileWidth, this.colliderThickness));
                bound = new FloatRect(this.spriteSheet.Sprite.Position.X, this.spriteSheet.Sprite.Position.Y, this.spriteSheet.TileWidth, this.colliderThickness);
                break;
            case EDirection.Right:
                shape = new RectangleShape(new Vector2f(this.colliderThickness, this.spriteSheet.TileHeight - 2 * this.colliderThickness));
                bound = new FloatRect(this.spriteSheet.Sprite.Position.X + this.spriteSheet.TileWidth - this.colliderThickness, this.spriteSheet.Sprite.Position.Y + this.colliderThickness, this.colliderThickness, this.spriteSheet.TileHeight - this.colliderThickness);
                break;
            case EDirection.Left:
                shape = new RectangleShape(new Vector2f(this.colliderThickness, this.spriteSheet.TileHeight - 2 * this.colliderThickness));
                bound = new FloatRect(this.spriteSheet.Sprite.Position.X, this.spriteSheet.Sprite.Position.Y + this.colliderThickness, this.colliderThickness, this.spriteSheet.TileHeight - this.colliderThickness);
                break;
        }

        shape.OutlineColor = Color.Magenta;
        shape.OutlineThickness = 0.6f;
        shape.FillColor = Color.Transparent;
    }

    public void UpdatePosition(SFML.System.Vector2f displacement)
    {
        switch (this.direction)
        {
            case EDirection.Botton:
                this.bound.Left = this.spriteSheet.Sprite.Position.X;
                this.bound.Top = this.spriteSheet.Sprite.Position.Y + this.spriteSheet.TileHeight - this.colliderThickness;
                break;
            case EDirection.Top:
                this.bound.Left = this.spriteSheet.Sprite.Position.X;
                this.bound.Top = this.spriteSheet.Sprite.Position.Y;
                break;
            case EDirection.Right:
                this.bound.Left = this.spriteSheet.Sprite.Position.X + this.spriteSheet.TileWidth - this.colliderThickness;
                this.bound.Top = this.spriteSheet.Sprite.Position.Y + this.colliderThickness;
                break;
            case EDirection.Left:
                this.bound.Left = this.spriteSheet.Sprite.Position.X;
                this.bound.Top = this.spriteSheet.Sprite.Position.Y + this.colliderThickness;
                break;
        }

        this.shape.Position = new Vector2f(this.bound.Left, this.bound.Top);
    }

    override public string ToString()
    {
        return this.overlap.ToString() + " " + this.direction.ToString();
    }
}
