﻿using SFML.Graphics;
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
        Renderer = new Renderer(Resources.Load("dragon.png"));
        Renderer.Root = this;
        Subscribe(Renderer);

        Rigidbody = new Rigidbody(V2.Zero,
                                  V2.Zero,
                                  5,
                                  V2.Zero,
                                  new Vector2f(Renderer.SpriteSheet.TileWidth, Renderer.SpriteSheet.TileHeight), 
                                  new Material("Personagem", 1, 1, 1, ECollisionType.Inelastic),
                                  false,
                                  this);
        Rigidbody.Root = this;

        PlatformPlayerController = new PlatformPlayerController(Rigidbody, Renderer);
        PlatformPlayerController.OnSpriteSheetOrientationChange += Renderer.OrientateSpriteSheetTo;

        this.Components.AddRange(new List<IComponent> { PlatformPlayerController, Rigidbody, Renderer });
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        this.Components.ForEach(c => c.Update(deltaTime));
    }

    public void SetKeyboardInput(ref KeyboardInput keyboardInput)
    {
        keyboardInput.OnKeyPressed += PlatformPlayerController.PlayerKeyboardController.OnKeyPressed;
        keyboardInput.OnKeyReleased += PlatformPlayerController.PlayerKeyboardController.OnKeyReleased;
    }    
}
