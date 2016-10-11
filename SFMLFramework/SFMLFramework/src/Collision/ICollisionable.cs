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
    Vector2f NetForce { get; }
    GameObject Root { get; }
    bool IsKinematic { get; }
    Vector2f Momentum { get; }
    void SolveCollision(CollisionInfo hitInfo);
}
