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
            //Logger.Log(string.Format("Colisão {0} [{1}, {2}] | [{3}, {4}]", EDirection.Up.ToString(), active.Root.name, passive.Root.name, active.Material.CollisionType, passive.Material.CollisionType));
        }

        //BOTTOM
        if (active.ColliderBottom.Bound.Intersects(passive.ColliderTop.Bound, out overlap))
        {
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Down, passive));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Up, active));
            //Logger.Log(string.Format("Colisão {0} [{1}, {2}] | [{3}, {4}]", EDirection.Down.ToString(), active.Root.name, passive.Root.name, active.Material.CollisionType, passive.Material.CollisionType));
        }

        //RIGHT        
        if (active.ColliderRight.Bound.Intersects(passive.ColliderLeft.Bound, out overlap))
        {
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Right, passive));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Left, active));
            //Logger.Log(string.Format("Colisão {0} [{1}, {2}] | [{3}, {4}]", EDirection.Right.ToString(), active.Root.name, passive.Root.name, active.Material.CollisionType, passive.Material.CollisionType));
        }

        //LEFT
        if (active.ColliderLeft.Bound.Intersects(passive.ColliderRight.Bound, out overlap))
        {
            active.SolveCollision(new CollisionInfo(overlap, EDirection.Left, passive));
            passive.SolveCollision(new CollisionInfo(overlap, EDirection.Right, active));
            //Logger.Log(string.Format("Colisão {0} [{1}, {2}] | [{3}, {4}]", EDirection.Left.ToString(), active.Root.name, passive.Root.name, active.Material.CollisionType, passive.Material.CollisionType));
        }
        #endregion

        //TODO: obter valor do level/tilemap futuramente
        active.EnvironmentFriction = Physx.Air;
        passive.EnvironmentFriction = Physx.Air;
    }
}
