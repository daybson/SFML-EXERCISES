using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ICollisionable
{
    ECollisionType CollisionType { get; }
    Collider Top { get; }
    Collider Botton { get; }
    Collider Right { get; }
    Collider Left { get; }
    void SolveCollision(CollisionInfo hitInfo);
}
