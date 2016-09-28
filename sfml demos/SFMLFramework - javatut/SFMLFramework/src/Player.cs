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

public class Player : SFMLFramework.GameObject
{
    /*
    public Player() : base("resources/dragon.png", ECollisionType.Inelastic)
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
            if (!this.isJumping)
            {
                currSpeed.Y = velocity.Y;
                this.isJumping = true;
            }
        }));

        this.keyboardController.keyReleasedActions.Add(Keyboard.Key.A, new Action(() => SetDirectionMove(EDirection.Left, false)));
        this.keyboardController.keyReleasedActions.Add(Keyboard.Key.D, new Action(() => SetDirectionMove(EDirection.Right, false)));

        SetPosition(new Vector2f());
    }

    public SFMLFramework.PlatformPlayerController PlatformPlayerController
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }
    */
}
