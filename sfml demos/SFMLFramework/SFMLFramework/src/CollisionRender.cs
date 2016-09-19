using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

public class CollisionRender : Component, IRender
{
    public Shape shape;
   

    public void LoadSpriteSheet(string path)
    {
        throw new NotImplementedException();
    }

    public void Render(RenderTarget window)
    {
        window.Draw(this.shape);
    }

    public override void Update(float deltaTime)
    {
    }
}

