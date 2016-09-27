using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ICollisionable
{
    ECollisionType CollisionType { get; }
    Collider ColliderTop { get; }
    Collider ColliderBottom { get; }
    Collider ColliderRight { get; }
    Collider ColliderLeft { get; }
    void SolveCollision(CollisionInfo hitInfo);
}
