using SFML.System;
using SFMLFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ICollisionable
{
    Material Material { get; }
    Collider ColliderTop { get; }
    Collider ColliderBottom { get; }
    Collider ColliderRight { get; }
    Collider ColliderLeft { get; }
    void SolveCollision(CollisionInfo hitInfo);
}
