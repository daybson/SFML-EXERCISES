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
    #region Fields

    public Rigidbody Rigidbody { get; set; }

    public Renderer Renderer { get; set; }

    public PlatformPlayerController PlatformPlayerController { get; set; }

    #endregion


    #region Methods

    public Player()
    {
        name = "Player";
        Renderer = new Renderer(Resources.LoadSpriteSheet("dragon.png"), this);
        Subscribe(Renderer);

        Rigidbody = new Rigidbody(8f,
                                  0,
                                  new Vector2f(Renderer.SpriteSheet.TileWidth, Renderer.SpriteSheet.TileHeight),
                                  new Material("Player 1", 1, 1, 1, ECollisionType.Inelastic),
                                  false,
                                  this,
                                  new Vector2f(200, 60));

        PlatformPlayerController = new PlatformPlayerController(Rigidbody, Renderer);
        PlatformPlayerController.OnSpriteSheetOrientationChange += Renderer.OrientateSpriteSheetTo;

        this.Components.AddRange(new List<IComponent> { PlatformPlayerController, Rigidbody, Renderer });
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    public void SetKeyboardInput(ref KeyboardInput keyboardInput)
    {
        keyboardInput.OnKeyPressed += PlatformPlayerController.PlayerKeyboardController.OnKeyPressed;
        keyboardInput.OnKeyReleased += PlatformPlayerController.PlayerKeyboardController.OnKeyReleased;
    }

    #endregion
}
