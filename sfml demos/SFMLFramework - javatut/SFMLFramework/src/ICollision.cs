//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : SFML Framework
//  @ File Name : ICollision.cs
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

public interface ICollision
{
    bool CheckCollision(ICollision obstacle, out CollisionInfo hitInfo);
    Shape GetShape();
    void SolveCollision(CollisionInfo hitInfo);
    void SetSprite(Sprite sprite);
    IMove IMove { get; set; }
}
