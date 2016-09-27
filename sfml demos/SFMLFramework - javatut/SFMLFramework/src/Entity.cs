using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Entity : SFMLFramework.GameObject
{
    #region Fields

    protected SpriteSheet spriteSheet;
    protected string spritePath;
    protected Vector2f movement;
    private float epslonThereshold = 0.01f;

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

    public override void Update(float deltaTime)
    {
        this.isFalling = true;
        this.spriteSheet.UpdateAnimation(deltaTime, this.spriteDirection);

        DoInertialMoving(deltaTime);
        DoReverseMoving(deltaTime);

        Top.UpdatePosition();
        Botton.UpdatePosition();
        Right.UpdatePosition();
        Left.UpdatePosition();
    }

    private void DoInertialMoving(float deltaTime)
    {
        if (CollisionType == ECollisionType.PartialInelastic && this.velocity != new Vector2f())
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
            this.currSpeed = velocity;
        }
    }

    private void DoReverseMoving(float deltaTime)
    {
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
            this.currSpeed = velocity;
        }
    }

    public void Render(RenderTarget window)
    {
        window.Draw(this.spriteSheet.Sprite);

        //window.Draw(this.fullCollider);
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

    public void AddMovement(Vector2f velocity)
    {
        this.velocity = velocity;
    }

    public Rigidbody Rigidbody
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }
}
