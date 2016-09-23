using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Entity
{
    #region Fields

    protected string name;
    protected SpriteSheet spriteSheet;
    protected string spritePath;
    protected bool moveLeft;
    protected bool moveRigth;
    protected Vector2f speed;
    protected Vector2f currSpeed;
    protected Vector2f move;
    protected EDirection direction;

    protected RectangleShape fullCollider;

    protected float mass;
    public RectangleShape Collider { get { return fullCollider; } }
    public OnDirectionChange OnChangeDirection { get; set; }

    private List<Collider> colliders;
    protected List<Collider> Colliders { get { return colliders; } }
    protected int colliderThickness = 2;

    #endregion

    public Entity(string spritePath)
    {
        this.spritePath = spritePath;
        this.name = "Dragon";
        this.spriteSheet = new SpriteSheet(spritePath);
        this.OnChangeDirection += this.spriteSheet.SetDirection;
        this.mass = 1;
        this.fullCollider = new RectangleShape(new Vector2f(this.spriteSheet.Sprite.GetGlobalBounds().Width, this.spriteSheet.Sprite.GetGlobalBounds().Height));

        this.fullCollider.OutlineColor = Color.Magenta;
        this.fullCollider.OutlineThickness = 1;
        this.fullCollider.FillColor = Color.Transparent;
        this.colliders = new List<Collider>
        {
            new Collider(spriteSheet, EDirection.Top, colliderThickness),
            new Collider(spriteSheet, EDirection.Botton, colliderThickness),
            new Collider(spriteSheet, EDirection.Left, colliderThickness),
            new Collider(spriteSheet, EDirection.Right, colliderThickness)
        };
    }

    #region GameLoop

    public void Update(float deltaTime)
    {
        this.spriteSheet.UpdateAnimation(deltaTime, this.direction);
        this.colliders.ForEach(c => c.UpdatePosition());
    }

    public void Render(RenderTarget window)
    {
        window.Draw(this.spriteSheet.Sprite);
        this.colliders.ForEach(c => window.Draw(c.Shape));

        //window.Draw(this.fullCollider);
    }

    #endregion


    public bool IsColliding(Entity obstacle)
    {
        FloatRect overlap;

        //top
        if (this.colliders.Find(c => c.Direction == EDirection.Top).Bound.Intersects(obstacle.Colliders.Find(o => o.Direction == EDirection.Botton).Bound, out overlap))
        {
            Console.WriteLine("TOP");
        }

        //bottom
        if (this.colliders.Find(c => c.Direction == EDirection.Botton).Bound.Intersects(obstacle.Colliders.Find(o => o.Direction == EDirection.Top).Bound, out overlap))
        {
            SetPosition(new Vector2f(this.spriteSheet.Sprite.Position.X, this.spriteSheet.Sprite.Position.Y - overlap.Height));
            Console.WriteLine("BOTTOM");
        }

        //right
        if (this.colliders.Find(c => c.Direction == EDirection.Right).Bound.Intersects(obstacle.Colliders.Find(o => o.Direction == EDirection.Left).Bound, out overlap))
        {
            SetPosition(new Vector2f(this.spriteSheet.Sprite.Position.X - overlap.Width, this.spriteSheet.Sprite.Position.Y));
            Console.WriteLine("RIGHT");
        }

        //left
        if (this.colliders.Find(c => c.Direction == EDirection.Left).Bound.Intersects(obstacle.Colliders.Find(o => o.Direction == EDirection.Right).Bound, out overlap))
        {
            Console.WriteLine("LEFT");
        }

        return false;
    }

    #region Movement

    private void SetDirectionMove(EDirection direction, bool value)
    {
        this.direction = direction;
        switch (direction)
        {
            case EDirection.Left:
                this.moveLeft = value;
                if (value)
                    this.moveRigth = !value;
                break;
            case EDirection.Right:
                this.moveRigth = value;
                if (value)
                    this.moveLeft = !value;
                break;
        }

        if (value)
            this.OnChangeDirection(direction);
    }

    public void SetPosition(Vector2f position)
    {
        this.spriteSheet.Sprite.Position = position;
        this.fullCollider.Position = position;
        this.colliders.ForEach(c => c.UpdatePosition());
    }

    #endregion
}
