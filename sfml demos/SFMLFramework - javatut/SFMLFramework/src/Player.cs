using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLFramework;
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

public class Player : GameObject
{
    public Rigidbody Rigidbody
    {
        get;

        set;
    }

    public Renderer Renderer
    {
        get;

        set;
    }

    public PlatformPlayerController PlatformPlayerController
    {
        get;

        set;
    }


    public Player() 
    {
        Renderer = new Renderer(Resources.Load("resources/dragon.png"));
        Renderer.Root = this;

        Rigidbody = new Rigidbody(new Vector2f(), new Vector2f(), 2, new Vector2f(), new Vector2f(Renderer.SpriteSheet.TileWidth, Renderer.SpriteSheet.TileHeight), this);
        Rigidbody.Root = this;

        PlatformPlayerController = new PlatformPlayerController();
        //PlatformPlayerController.PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.A, new Action(() => PlatformPlayerController.AddForce(new Vector2f(-1, 0))));
        //PlatformPlayerController.PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.D, new Action(() => PlatformPlayerController.AddForce(new Vector2f( 1, 0))));
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        Rigidbody.Update(deltaTime);
        Renderer.Update(deltaTime);
    }

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
