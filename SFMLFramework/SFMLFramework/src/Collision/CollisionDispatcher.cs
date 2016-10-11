using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFMLFramework.src.Helper;
using SFML.System;
using SFMLFramework;

public class CollisionDispatcher
{
    /// <summary>
    /// Retorna true se há uma colsião entre as entidades, instanciando CollisionInfo e disparando uma solução de colisção em ambas entidades
    /// </summary>
    /// <param name="active">Fonte ativa da colisão</param>
    /// <param name="passive">Fonte passiva da colisão</param>
    /// <param name="hitInfo">Inofmrações da colisão caso aconteça</param>
    /// <returns></returns>
    public static void CollisionCheck(ref ICollisionable active, ref ICollisionable passive, float deltaTime)
    {
        if (active == null || passive == null)
            return;

        FloatRect overlap;

        #region Check

        //TOP
        if (active.ColliderTop.Bound.Intersects(passive.ColliderBottom.Bound, out overlap))
        {            
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Up, passive));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Down, active));
            // Console.WriteLine("Colisao T: " + active.Root.Position.ToString() + " - " + passive.Root.Position.ToString());
        }

        //BOTTOM
        if (active.ColliderBottom.Bound.Intersects(passive.ColliderTop.Bound, out overlap))
        {            
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Down, passive));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Up, active));
            // Console.WriteLine("Colisao B" + active.Root.Position.ToString() + " - " + passive.Root.Position.ToString());
        }

        //RIGHT        
        if (active.ColliderRight.Bound.Intersects(passive.ColliderLeft.Bound, out overlap))
        {            
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Right, passive));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Left, active));
            //Console.WriteLine("Colisao R" + active.Root.Position.ToString() + " - " + passive.Root.Position.ToString());
        }

        //LEFT
        if (active.ColliderLeft.Bound.Intersects(passive.ColliderRight.Bound, out overlap))
        {            
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Left, passive));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Right, active));
            // Console.WriteLine("Colisao L" + active.Root.Position.ToString() + " - " + passive.Root.Position.ToString());
        }
        #endregion

        //TODO: obter valor do level/tilemap futuramente
        active.EnvironmentFriction = Physx.Air;
    }
}
