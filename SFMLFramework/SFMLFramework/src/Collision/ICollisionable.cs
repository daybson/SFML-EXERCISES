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
    Vector2f MaxVelocity { get; }
    float Mass { get; }
    float Elasticity { get; }
    void SolveCollision(CollisionInfo hitInfo);
}
