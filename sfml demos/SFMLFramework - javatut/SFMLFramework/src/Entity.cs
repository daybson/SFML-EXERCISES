using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Entity : ICollisionable
{
    #region Fields

    protected string name;

    protected SpriteSheet spriteSheet;
    protected string spritePath;

    protected Vector2f velocity;
    protected Vector2f currSpeed;
    protected Vector2f movement;
    private float mass;
    private float friction = 10f;
    private float epslonThereshold = 0.01f;

    public Vector2f ImpactForce { get { return currSpeed * mass; } }

    protected bool isFalling;
    protected bool isJumping;
    protected bool moveLeft;
    protected bool moveRigth;
    protected EDirection spriteDirection;
    public OnDirectionChange OnChangeDirection { get; set; }

    protected RectangleShape fullCollider;
    public RectangleShape FullCollider { get { return fullCollider; } }
    public ECollisionType CollisionType { get; protected set; }
    public Collider Top { get; protected set; }
    public Collider Botton { get; protected set; }
    public Collider Right { get; protected set; }
    public Collider Left { get; protected set; }

    protected int colliderThickness = 5;

    #endregion



    public Entity(string spritePath, ECollisionType collisionType)
    {
        this.isFalling = true;
        this.isJumping = true;
        this.spritePath = spritePath;
        this.name = spritePath.Replace(".png", "");
        this.spriteSheet = new SpriteSheet(spritePath);
        this.OnChangeDirection += this.spriteSheet.SetDirection;
        this.fullCollider = new RectangleShape(new Vector2f(this.spriteSheet.Sprite.GetGlobalBounds().Width, this.spriteSheet.Sprite.GetGlobalBounds().Height));
        this.mass = 2.0f;
        this.CollisionType = collisionType;
        this.Top = new Collider(spriteSheet, EDirection.Top, colliderThickness);
        this.Botton = new Collider(spriteSheet, EDirection.Botton, colliderThickness);
        this.Left = new Collider(spriteSheet, EDirection.Left, colliderThickness);
        this.Right = new Collider(spriteSheet, EDirection.Right, colliderThickness);

        this.fullCollider.OutlineColor = Color.Magenta;
        this.fullCollider.OutlineThickness = 1;
        this.fullCollider.FillColor = Color.Transparent;

    }

    public void Update(float deltaTime)
    {
        this.isFalling = true;
        this.spriteSheet.UpdateAnimation(deltaTime, this.spriteDirection);

        if (name == "brick2")
            Console.WriteLine(name + ": " + velocity.ToString());

        if (name == "brick3")
            Console.WriteLine(name + ": " + velocity.ToString());

        if (CollisionType == ECollisionType.Elastic && this.velocity != new Vector2f())
        {
            if (this.velocity.X > 0)
            {
                velocity.X = velocity.X - friction * deltaTime;
                if (velocity.X < epslonThereshold)
                    velocity.X = 0;
            }
            else
            {
                velocity.X = velocity.X + friction * deltaTime;
                if (velocity.X > epslonThereshold)
                    velocity.X = 0;
            }

            if (this.velocity.Y > 0)
            {
                velocity.Y = velocity.Y - friction * deltaTime;
                if (velocity.Y < epslonThereshold)
                    velocity.Y = 0;
            }
            else
            {
                velocity.Y = velocity.Y + friction * deltaTime;
                if (velocity.Y > epslonThereshold)
                    velocity.Y = 0;
            }

            this.spriteSheet.Sprite.Position += this.velocity;
            this.fullCollider.Position += this.velocity;
            currSpeed = velocity;
        }

        Top.UpdatePosition();
        Botton.UpdatePosition();
        Right.UpdatePosition();
        Left.UpdatePosition();
    }

    public void Render(RenderTarget window)
    {
        window.Draw(this.spriteSheet.Sprite);

        //window.Draw(this.fullCollider);
    }

    public void SolveCollision(CollisionInfo hitInfo)
    {
        if (this.CollisionType == ECollisionType.None)
            return;

        switch (CollisionType)
        {
            case ECollisionType.Elastic:
                #region

                AddMovement(hitInfo.Force - this.currSpeed);
                break;
            #endregion

            case ECollisionType.Inelastic:
                #region
                switch (hitInfo.Direction)
                {
                    case EDirection.Botton:
                        SetPosition(new Vector2f(this.spriteSheet.Sprite.Position.X, this.spriteSheet.Sprite.Position.Y - hitInfo.Overlap.Height));
                        this.isFalling = false;
                        this.isJumping = false;
                        this.currSpeed.Y = 0;
                        break;
                    case EDirection.Top:
                        SetPosition(new Vector2f(this.spriteSheet.Sprite.Position.X, spriteSheet.Sprite.Position.Y + hitInfo.Overlap.Height));
                        currSpeed.Y = 0;
                        break;
                    case EDirection.Right:
                        SetPosition(new Vector2f(this.spriteSheet.Sprite.Position.X - hitInfo.Overlap.Width, this.spriteSheet.Sprite.Position.Y));
                        break;
                    case EDirection.Left:
                        SetPosition(new Vector2f(this.spriteSheet.Sprite.Position.X + hitInfo.Overlap.Width, this.spriteSheet.Sprite.Position.Y));
                        break;
                }
                break;
            #endregion

            case ECollisionType.PartialInelastic:
                #region
                break;
            #endregion

            case ECollisionType.Trigger:
                #region
                break;
                #endregion
        }
    }

    private void SetDirectionMove(EDirection direction, bool value)
    {
        this.spriteDirection = direction;
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

        this.Top = new Collider(spriteSheet, EDirection.Top, colliderThickness);
        this.Botton = new Collider(spriteSheet, EDirection.Botton, colliderThickness);
        this.Left = new Collider(spriteSheet, EDirection.Left, colliderThickness);
        this.Right = new Collider(spriteSheet, EDirection.Right, colliderThickness);
    }

    public void AddMovement(Vector2f velocity)
    {
        this.velocity = velocity;
    }
}
