//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : SFML Framework
//  @ File Name : Renderer.cs
//  @ Date : 13/09/2016
//  @ Author : Daybson B. S. Paisante <daybson.paisante@outlook.com>
//
//

using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.System;

public class Renderer : IRender
{
    protected SpriteSheet spriteSheet;
    public SpriteSheet SpriteSheet { get { return spriteSheet; } }

    public IMove iMove;

    public Renderer() : base()
    {

    }

    public void Render(RenderTarget window)
    {
        window.Draw(this.spriteSheet.Sprite);
    }

    public void Update(float deltaTime)
    {
        if (this.iMove != null)
        {
            this.spriteSheet.Sprite.Position = this.iMove.Position;
        }
        else
            Console.WriteLine("Renderer component requires an IMove reference's object to update position");
    }

    public void LoadSpriteSheet(string path)
    {
        this.spriteSheet = new SpriteSheet(path);
        this.iMove.OnChangeDirection += this.spriteSheet.SetDirection;
    }
}