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

        if (active.ColliderTop.Bound.Intersects(passive.ColliderBottom.Bound, out overlap))
        {
            var hitInfo = new CollisionInfo(overlap, EDirection.Top, new SFML.System.Vector2f());//active.ImpactForce);
            active.SolveCollision(hitInfo);
            passive.SolveCollision(hitInfo.Inverse());
        }

        if (active.ColliderBottom.Bound.Intersects(passive.ColliderTop.Bound, out overlap))
        {
            var hitInfo = new CollisionInfo(overlap, EDirection.Botton, new SFML.System.Vector2f());// active.ImpactForce);
            active.SolveCollision(hitInfo);
            passive.SolveCollision(hitInfo.Inverse());
        }

        if (active.ColliderRight.Bound.Intersects(passive.ColliderLeft.Bound, out overlap))
        {
            var hitInfo = new CollisionInfo(overlap, EDirection.Right, new SFML.System.Vector2f());//active.ImpactForce);
            active.SolveCollision(hitInfo);
            passive.SolveCollision(hitInfo.Inverse());
        }

        if (active.ColliderLeft.Bound.Intersects(passive.ColliderRight.Bound, out overlap))
        {
            var hitInfo = new CollisionInfo(overlap, EDirection.Left, new SFML.System.Vector2f());// active.ImpactForce);
            active.SolveCollision(hitInfo);
            passive.SolveCollision(hitInfo.Inverse());
        }
    }
}
