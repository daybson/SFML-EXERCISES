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
    public static void CollisionCheck(ICollisionable active, ICollisionable passive, float deltaTime)
    {
        var isColliding = false;
        var activeInitialPosition = active.Root.Position;
        var passiveInitialPosition = passive.Root.Position;

        //Para evitar atravessar as paredes quando um objeto se move muito rápido, calcula-se displacement de movimento que será realizado no frame atual
        //e itera-se pequenos incrementos sobre esse displacement para identificar se realmente não há colisão
        for (var i = 0f; i < deltaTime; i += 0.001f)
        {
            FloatRect overlap;

            var activeForceByFrame = active.NetForce * deltaTime * i;
            var activePositionDisplacement = new Vector2f(active.Root.Position.X * activeForceByFrame.X, active.Root.Position.Y * activeForceByFrame.Y);
            var activePriorFramePosition = new Vector2f(active.Root.Position.X - activePositionDisplacement.X, active.Root.Position.Y - activePositionDisplacement.Y);
            active.Root.Position = activePriorFramePosition;

            var passiveForceByFrame = passive.NetForce * deltaTime * i;
            var passivePositionDisplacement = new Vector2f(passive.Root.Position.X * passiveForceByFrame.X, passive.Root.Position.Y * passiveForceByFrame.Y);
            var passivePriorFramePosition = new Vector2f(passive.Root.Position.X - passivePositionDisplacement.X, passive.Root.Position.Y - passivePositionDisplacement.Y);
            passive.Root.Position = passivePriorFramePosition;

            #region Check

            //TOP
            if (active.ColliderTop.Bound.Intersects(passive.ColliderBottom.Bound, out overlap))
            {
                isColliding = true;
                active.SolveCollision(new CollisionInfo(overlap, EDirection.Up, passive));
                passive.SolveCollision(new CollisionInfo(overlap, EDirection.Down, active));
                // Console.WriteLine("Colisao T: " + active.Root.Position.ToString() + " - " + passive.Root.Position.ToString());
                break;
            }

            //BOTTOM
            if (active.ColliderBottom.Bound.Intersects(passive.ColliderTop.Bound, out overlap))
            {
                isColliding = true;
                active.SolveCollision(new CollisionInfo(overlap, EDirection.Down, passive));
                passive.SolveCollision(new CollisionInfo(overlap, EDirection.Up, active));
                // Console.WriteLine("Colisao B" + active.Root.Position.ToString() + " - " + passive.Root.Position.ToString());
                break;
            }

            //RIGHT
            if (active.ColliderRight.Bound.Intersects(passive.ColliderLeft.Bound, out overlap))
            {
                isColliding = true;
                active.SolveCollision(new CollisionInfo(overlap, EDirection.Right, passive));
                passive.SolveCollision(new CollisionInfo(overlap, EDirection.Left, active));
                // Console.WriteLine("Colisao R" + active.Root.Position.ToString() + " - " + passive.Root.Position.ToString());
                break;
            }

            //LEFT
            if (active.ColliderLeft.Bound.Intersects(passive.ColliderRight.Bound, out overlap))
            {
                isColliding = true;
                active.SolveCollision(new CollisionInfo(overlap, EDirection.Left, passive));
                passive.SolveCollision(new CollisionInfo(overlap, EDirection.Right, active));
                // Console.WriteLine("Colisao L" + active.Root.Position.ToString() + " - " + passive.Root.Position.ToString());
                break;
            }
            #endregion
        }

        //TODO: obter valor do level/tilemap futuramente
        if (!isColliding)
        {
            active.Root.Position = activeInitialPosition;
            passive.Root.Position = passiveInitialPosition;
            active.EnvironmentFriction = Physx.Air;
        }
    }
}
