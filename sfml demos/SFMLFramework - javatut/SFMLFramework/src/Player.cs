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

    public Player() : base("dragon.png", ECollisionType.Inelastic)
    {
        this.isFalling = true;
        this.isJumping = true;
        this.velocity = new Vector2f(3, -20);

        this.CollisionType = ECollisionType.Inelastic;


        this.keyboardController = new PlayerKeyboardController();
        this.keyboardController.keyPressedActions.Add(Keyboard.Key.A, new Action(() => SetDirectionMove(EDirection.Left, true)));
        this.keyboardController.keyPressedActions.Add(Keyboard.Key.D, new Action(() => SetDirectionMove(EDirection.Right, true)));
        this.keyboardController.keyPressedActions.Add(Keyboard.Key.Space, new Action(() =>
        {
            if(!this.isJumping)
            {
                currSpeed.Y = velocity.Y;
                this.isJumping = true;
            }
        }));

        this.keyboardController.keyReleasedActions.Add(Keyboard.Key.A, new Action(() => SetDirectionMove(EDirection.Left, false)));
        this.keyboardController.keyReleasedActions.Add(Keyboard.Key.D, new Action(() => SetDirectionMove(EDirection.Right, false)));

        SetPosition(new Vector2f());
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
        window.Draw(Top.Shape);
        window.Draw(Botton.Shape);
        window.Draw(Right.Shape);
        window.Draw(Left.Shape);

        base.Render(window);
    }

    #endregion

    #region Movement

    private void ProccessGravity()
    {
        if(this.isFalling || this.isJumping)
        {
            currSpeed += new Vector2f(0, Physx.G);
            currSpeed += new Vector2f(0, Physx.G);
            if(currSpeed.Y > 3.5f)
                currSpeed.Y = 3.5f;
        }
    }

    private void ProccessInput()
    {
        if(this.moveLeft)
        {
            currSpeed.X += -this.velocity.X;
            if(currSpeed.X < -velocity.X)
                currSpeed.X = -velocity.X;
        }
        else if(this.moveRigth)
        {
            currSpeed.X += this.velocity.X;
            if(currSpeed.X > velocity.X)
                currSpeed.X = velocity.X;
        }
        else
            currSpeed.X = 0;
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

    new public void SetPosition(Vector2f position)
    {
        base.SetPosition(position);
    }

    #endregion
}
