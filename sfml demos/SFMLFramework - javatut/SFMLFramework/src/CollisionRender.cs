using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

public class CollisionRender : IRender
{
    public Shape shape;
   

    public void LoadSpriteSheet(string path)
    {
        throw new NotImplementedException();
    }

    public void Render(ref RenderWindow window)
    {
         window.Draw(this.shape);
    }

    public void Update(float deltaTime)
    {
    }
}

