using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFMLFramework.src.Helper;

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
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Up, passive.Velocity));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Up, active.Velocity).Inverse());
        }

        if (active.ColliderBottom.Bound.Intersects(passive.ColliderTop.Bound, out overlap))
        {
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Down, passive.Velocity));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Down, active.Velocity).Inverse());
        }

        if (active.ColliderRight.Bound.Intersects(passive.ColliderLeft.Bound, out overlap))
        {
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Right, passive.Velocity));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Right, active.Velocity).Inverse());
        }

        if (active.ColliderLeft.Bound.Intersects(passive.ColliderRight.Bound, out overlap))
        {
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Left, passive.Velocity));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Left, active.Velocity).Inverse());
        }
    }
}
