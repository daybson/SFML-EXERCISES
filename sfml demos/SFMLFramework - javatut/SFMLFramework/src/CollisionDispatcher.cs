using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

public class CollisionDispatcher
{
    /// <summary>
    /// Retorna true se há uma colsião entre as entidades, instanciando CollisionInfo e disparando uma solução de colisção em ambas entidades
    /// </summary>
    /// <param name="active">Fonte ativa da colisão</param>
    /// <param name="passive">Fonte passiva da colisão</param>
    /// <param name="hitInfo">Inofmrações da colisão caso aconteça</param>
    /// <returns></returns>
    public static void CollisionCheck(ICollisionable active, ICollisionable passive)
    {
        FloatRect overlap;

        if(active.Top.Bound.Intersects(passive.Botton.Bound, out overlap))
        {
            var hitInfo = new CollisionInfo(overlap, EDirection.Top, active.ImpactForce);
            active.SolveCollision(hitInfo);
            passive.SolveCollision(hitInfo.Inverse());
        }

        if(active.Botton.Bound.Intersects(passive.Top.Bound, out overlap))
        {
            var hitInfo = new CollisionInfo(overlap, EDirection.Botton, active.ImpactForce);
            active.SolveCollision(hitInfo);
            passive.SolveCollision(hitInfo.Inverse());
        }

        if(active.Right.Bound.Intersects(passive.Left.Bound, out overlap))
        {
            var hitInfo = new CollisionInfo(overlap, EDirection.Right, active.ImpactForce);
            active.SolveCollision(hitInfo);
            passive.SolveCollision(hitInfo.Inverse());
        }

        if(active.Left.Bound.Intersects(passive.Right.Bound, out overlap))
        {
            var hitInfo = new CollisionInfo(overlap, EDirection.Left, active.ImpactForce);
            active.SolveCollision(hitInfo);
            passive.SolveCollision(hitInfo.Inverse());
        }
    }
}
