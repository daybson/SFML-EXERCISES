using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

public delegate void OnDirectionChange(EDirection direction);
public enum EDirection
{
    Top,
    Botton,
    Left,
    Right,
    None
}

public class Player : Entity
{
    public PlayerKeyboardController keyboardController;
    protected bool isFalling;
    protected bool isJumping;

    public Player() : base("dragon.png")
    {
        this.isFalling = true;
        this.isJumping = true;
        this.speed = new Vector2f(3, -20);
        this.mass = 2;

        this.keyboardController = new PlayerKeyboardController();
        this.keyboardController.keyPressedActions.Add(Keyboard.Key.A, new Action(() => SetDirectionMove(EDirection.Left, true)));
        this.keyboardController.keyPressedActions.Add(Keyboard.Key.D, new Action(() => SetDirectionMove(EDirection.Right, true)));
        this.keyboardController.keyPressedActions.Add(Keyboard.Key.Space, new Action(() =>
        {
            if (!this.isJumping)
            {
                currSpeed.Y = speed.Y;
                this.isJumping = true;
            }
        }));

        this.keyboardController.keyReleasedActions.Add(Keyboard.Key.A, new Action(() => SetDirectionMove(EDirection.Left, false)));
        this.keyboardController.keyReleasedActions.Add(Keyboard.Key.D, new Action(() => SetDirectionMove(EDirection.Right, false)));
        
        SetPosition(new Vector2f());
    }

    [Obsolete]
    public bool IsColliding(RectangleShape obstacle, out CollisionInfo hitInfo)
    {
        hitInfo = null;

        var myLeft = this.fullCollider.Position.X;
        var myRight = this.fullCollider.Position.X + this.fullCollider.Size.X;
        var myTop = this.fullCollider.Position.Y;
        var myBottom = this.fullCollider.Position.Y + this.fullCollider.Size.Y;

        var oLeft = obstacle.Position.X;
        var oRight = obstacle.Position.X + obstacle.Size.X;
        var oTop = obstacle.Position.Y;
        var oBottom = obstacle.Position.Y + obstacle.Size.Y;

        //colisao na direita
        if (((myBottom < oTop || myTop < oTop) && (myBottom > oBottom || myTop > oBottom)) && myRight > oLeft && myLeft < oRight)
        {
            SetPosition(new Vector2f(obstacle.Position.X - this.fullCollider.Size.X, this.fullCollider.Position.Y));
            return true;
        }

        //colisao no topo
        if (myTop > oTop && myTop < oBottom && ((myLeft < oRight || myRight < oRight) && (myLeft > oLeft || myRight > oLeft)))
        {
            SetPosition(new Vector2f(this.fullCollider.Position.X, obstacle.Position.Y + obstacle.Size.Y));
            currSpeed.Y = 0;
            return true;
        }

        //colisao na esquerda
        if (((myBottom < oTop || myTop < oTop) && (myBottom > oBottom || myTop > oBottom)) && myLeft < oRight && myLeft > oLeft)
        {
            SetPosition(new Vector2f(obstacle.Position.X + obstacle.Size.X, this.fullCollider.Position.Y));
            return true;
        }

        //colisao na base
        if (((myLeft < oRight || myRight < oRight) && (myLeft > oLeft || myRight > oLeft)) &&
          myBottom > oTop && myBottom < oBottom)
        {
            SetPosition(new Vector2f(this.fullCollider.Position.X, obstacle.Position.Y - this.fullCollider.Size.Y));
            this.isFalling = false;
            this.isJumping = false;
            currSpeed.Y = 0;
            return true;
        }
        else
        {
            this.isFalling = true;
            return false;
        }
    }

    #region GameLoop

    new public void Update(float deltaTime)
    {
        ProccessGravity();
        ProccessInput();

        this.spriteSheet.Sprite.Position += currSpeed;
        this.fullCollider.Position += currSpeed;
        base.Update(deltaTime);
    }

    new public void Render(RenderTarget window)
    {
        base.Render(window);
    }

    #endregion


    #region Movement

    private void ProccessGravity()
    {
        if (this.isFalling || this.isJumping)
        {
            currSpeed += new Vector2f(0, Physx.G);
            currSpeed += new Vector2f(0, Physx.G);
            if (currSpeed.Y > 3.5f)
                currSpeed.Y = 3.5f;
        }
    }

    private void ProccessInput()
    {
        if (this.moveLeft)
        {
            currSpeed.X += -this.speed.X;
            if (currSpeed.X < -speed.X)
                currSpeed.X = -speed.X;
        }
        else if (this.moveRigth)
        {
            currSpeed.X += this.speed.X;
            if (currSpeed.X > speed.X)
                currSpeed.X = speed.X;
        }
        else
            currSpeed.X = 0;
    }

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

    #endregion   
}
