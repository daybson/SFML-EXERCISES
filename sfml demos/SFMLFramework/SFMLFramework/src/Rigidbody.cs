using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Rigidbody : Component
{
    #region Fields

    public IMove mover;
    public float mass;
    public Vector2f force;
    
    #endregion

    
    #region Public

    public Rigidbody()
    {        
    }

    public override void Update(float deltaTime)
    {
        var gForce = new Vector2f(0, Physx.G * mass * deltaTime);
        this.mover.ApplyMovement(gForce, Mover.EDirection.Down);
    }

    #endregion
}
