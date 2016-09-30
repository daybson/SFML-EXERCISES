using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using SFMLFramework.src.Helper;

public delegate void OnDirectionChange(EDirection direction);
public enum EDirection
{
    Up,
    Down,
    Left,
    Right,
    None
}

public class Player : GameObject
{
    public Rigidbody Rigidbody { get; set; }

    public Renderer Renderer { get; set; }

    public PlatformPlayerController PlatformPlayerController { get; set; }


    public Player()
    {
        Renderer = new Renderer(Resources.Load("resources/dragon.png"));
        Renderer.Root = this;

        Rigidbody = new Rigidbody(new Vector2f(), new Vector2f(), 2, new Vector2f(), new Vector2f(Renderer.SpriteSheet.TileWidth, Renderer.SpriteSheet.TileHeight), this);
        Rigidbody.Root = this;

        PlatformPlayerController = new PlatformPlayerController(Rigidbody, Renderer);
        PlatformPlayerController.OnSpriteSheetOrientationChange += Renderer.OrientateSpriteSheetTo;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        Rigidbody.Update(deltaTime);
        Renderer.Update(deltaTime);
    }

    public void SetKeyboardInput(ref KeyboardInput keyboardInput)
    {
        keyboardInput.OnKeyPressed += PlatformPlayerController.PlayerKeyboardController.OnKeyPressed;
        keyboardInput.OnKeyReleased += PlatformPlayerController.PlayerKeyboardController.OnKeyReleased;
    }    
}
