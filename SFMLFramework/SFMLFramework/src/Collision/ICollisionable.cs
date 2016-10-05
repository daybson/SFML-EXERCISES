using SFML.System;
using SFMLFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ICollisionable
{
    Material Material { get; }
    float EnvironmentFriction { get; set; }
    Collider ColliderTop { get; }
    Collider ColliderBottom { get; }
    Collider ColliderRight { get; }
    Collider ColliderLeft { get; }
    Vector2f Velocity { get; }
    float Mass { get; }
    void SolveCollision(CollisionInfo hitInfo);
}
