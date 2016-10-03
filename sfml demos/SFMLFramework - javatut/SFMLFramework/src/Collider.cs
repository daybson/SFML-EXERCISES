using SFML.Graphics;
using SFML.System;
using SFMLFramework;
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

public class Collider : IComponent
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
    private Vector2f spriteDimension;

    public RectangleShape Shape { get { return shape; } }
    public FloatRect Bound { get { return bound; } }
    public EDirection Direction { get { return direction; } }

    public bool IsEnabled { get; set; }

    public GameObject Root { get; set; }

    public Collider(Vector2f spriteDimension, EDirection direction, int colliderThickness, GameObject root)
    {
        this.Root = root;
        this.direction = direction;
        this.colliderThickness = colliderThickness;
        this.spriteDimension = spriteDimension;

        switch (this.direction)
        {
            case EDirection.Down:
                shape = new RectangleShape(new Vector2f(this.spriteDimension.X, this.colliderThickness));
                bound = new FloatRect(this.Root.Position.X, this.Root.Position.Y + this.spriteDimension.Y - this.colliderThickness, this.spriteDimension.X, this.colliderThickness);
                break;
            case EDirection.Up:
                shape = new RectangleShape(new Vector2f(this.spriteDimension.X, this.colliderThickness));
                bound = new FloatRect(this.Root.Position.X, this.Root.Position.Y, this.spriteDimension.Y, this.colliderThickness);
                break;
            case EDirection.Right:
                shape = new RectangleShape(new Vector2f(this.colliderThickness, this.spriteDimension.Y - 2 * this.colliderThickness));
                bound = new FloatRect(this.Root.Position.X + this.spriteDimension.X - this.colliderThickness, this.Root.Position.Y + this.colliderThickness, this.colliderThickness, this.spriteDimension.Y - this.colliderThickness);
                break;
            case EDirection.Left:
                shape = new RectangleShape(new Vector2f(this.colliderThickness, this.spriteDimension.Y - 2 * this.colliderThickness));
                bound = new FloatRect(this.Root.Position.X, this.Root.Position.Y + this.colliderThickness, this.colliderThickness, this.spriteDimension.X - this.colliderThickness);
                break;
        }

        shape.OutlineColor = Color.Magenta;
        shape.OutlineThickness = 0.6f;
        shape.FillColor = Color.Transparent;
    }

    public void UpdatePosition(Vector2f displacement)
    {
        switch (this.direction)
        {
            case EDirection.Down:
                this.bound.Left = this.Root.Position.X;
                this.bound.Top = this.Root.Position.Y + this.spriteDimension.Y - this.colliderThickness;
                break;
            case EDirection.Up:
                this.bound.Left = this.Root.Position.X;
                this.bound.Top = this.Root.Position.Y;
                break;
            case EDirection.Right:
                this.bound.Left = this.Root.Position.X + this.spriteDimension.X - this.colliderThickness;
                this.bound.Top = this.Root.Position.Y + this.colliderThickness;
                break;
            case EDirection.Left:
                this.bound.Left = this.Root.Position.X;
                this.bound.Top = this.Root.Position.Y + this.colliderThickness;
                break;
        }

        this.shape.Position = new Vector2f(this.bound.Left, this.bound.Top);
    }

    public void Update(float deltaTime)
    {

    }
}
