using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Collider
{
    private RectangleShape shape;
    private FloatRect bound;
    private EDirection direction;
    private int colliderThickness;
    private SpriteSheet spriteSheet;

    public RectangleShape Shape { get { return shape; } }
    public FloatRect Bound { get { return bound; } }
    public EDirection Direction { get { return direction; } }

    public Collider(SpriteSheet spriteSheet, EDirection direction, int colliderThickness)
    {
        this.direction = direction;
        this.colliderThickness = colliderThickness;
        this.spriteSheet = spriteSheet;

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

        shape.OutlineColor = Color.Cyan;
        shape.OutlineThickness = 0.6f;
        shape.FillColor = Color.Transparent;
    }

    public void UpdatePosition()
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
}
