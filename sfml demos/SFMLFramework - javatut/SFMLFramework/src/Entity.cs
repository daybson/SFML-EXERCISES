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

    protected Vector2f speed;
    protected Vector2f currSpeed;
    protected Vector2f move;

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
        this.name = "Dragon";
        this.spriteSheet = new SpriteSheet(spritePath);
        this.OnChangeDirection += this.spriteSheet.SetDirection;
        this.fullCollider = new RectangleShape(new Vector2f(this.spriteSheet.Sprite.GetGlobalBounds().Width, this.spriteSheet.Sprite.GetGlobalBounds().Height));

        this.CollisionType = collisionType;
        this.Top = new Collider(spriteSheet, EDirection.Top, colliderThickness);
        this.Botton = new Collider(spriteSheet, EDirection.Botton, colliderThickness);
        this.Left = new Collider(spriteSheet, EDirection.Left, colliderThickness);
        this.Right = new Collider(spriteSheet, EDirection.Right, colliderThickness);

        this.fullCollider.OutlineColor = Color.Magenta;
        this.fullCollider.OutlineThickness = 1;
        this.fullCollider.FillColor = Color.Transparent;

    }

    #region GameLoop

    public void Update(float deltaTime)
    {
        this.isFalling = true;
        this.spriteSheet.UpdateAnimation(deltaTime, this.spriteDirection);

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

    #endregion

    #region Movement

    public void SolveCollision(CollisionInfo hitInfo)
    {
        //TODO: muito repetitivo...

        if(this.CollisionType == ECollisionType.None)
            return;

        switch(CollisionType)
        {
            case ECollisionType.Elastic:
                break;

            case ECollisionType.Inelastic:
                switch(hitInfo.Direction)
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

            case ECollisionType.PartialInelastic:
                break;

            case ECollisionType.Trigger:
                break;
        }
    }

    private void SetDirectionMove(EDirection direction, bool value)
    {
        this.spriteDirection = direction;
        switch(direction)
        {
            case EDirection.Left:
                this.moveLeft = value;
                if(value)
                    this.moveRigth = !value;
                break;
            case EDirection.Right:
                this.moveRigth = value;
                if(value)
                    this.moveLeft = !value;
                break;
        }

        if(value)
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

    #endregion
}
