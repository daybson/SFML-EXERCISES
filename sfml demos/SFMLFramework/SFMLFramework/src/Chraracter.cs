using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

public delegate void OnDirectionChange(EDirection direction);
public enum EDirection
{
    Up,
    Down,
    Left,
    Right,
    None
}

public class Character : Entity
{
    public PlayerKeyboardController keyboardController;

    public Character() : base("dragon.png")
    {
        this.speed = new Vector2f(200, 200);
        this.mass = 2;
        this.keyboardController = new PlayerKeyboardController();
        this.keyboardController.keyPressedActions.Add(Keyboard.Key.A, new Action(() => SetDirectionMove(EDirection.Left, true)));
        this.keyboardController.keyPressedActions.Add(Keyboard.Key.D, new Action(() => SetDirectionMove(EDirection.Right, true)));
        this.keyboardController.keyReleasedActions.Add(Keyboard.Key.A, new Action(() => SetDirectionMove(EDirection.Left, false)));
        this.keyboardController.keyReleasedActions.Add(Keyboard.Key.D, new Action(() => SetDirectionMove(EDirection.Right, false)));
    }


    #region GameLoop

    new public void Update(float deltaTime)
    {
        this.move = new Vector2f();
        ProccessGravity();
        ProccessInput();
        Move(deltaTime);
        base.Update(deltaTime);
        this.spriteSheet.Sprite.Position = this.position;
    }

    #endregion


    #region Movement

    private void ProccessGravity()
    {
        this.move.Y += this.mass * Physx.G;
    }

    private void ProccessInput()
    {
        if (this.moveLeft)
            this.move.X += -this.speed.X;

        if (this.moveRigth)
            this.move.X += this.speed.X;
    }

    private void Move(float deltaTime)
    {
        this.move *= deltaTime;
        this.position += this.move;
        this.spriteSheet.Sprite.Position = this.position;
        this.collider.Position = this.position;

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
