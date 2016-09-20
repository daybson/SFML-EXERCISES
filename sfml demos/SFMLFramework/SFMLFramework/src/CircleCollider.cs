using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

public class CircleCollider : Component, ICollision
{
    protected CircleShape collider;
    IMove imove;
    public IMove IMove { get { return imove; } set { imove = value; } }

    public CircleCollider()
    {

    }

    public bool CheckCollision(ICollision obstacle, out CollisionInfo hitInfo)
    {
        hitInfo = null;

        var radius = ((CircleShape)obstacle.GetShape()).Radius;

        var d = Math.Sqrt((obstacle.GetShape().Position.X - this.collider.Position.X) * (obstacle.GetShape().Position.X - this.collider.Position.X) +
                          (obstacle.GetShape().Position.Y - this.collider.Position.Y) * (obstacle.GetShape().Position.Y - this.collider.Position.Y));

        var c = d < this.collider.Radius + radius;

        if (c)
        {
            var distanceX = Math.Abs(obstacle.GetShape().Position.X - this.collider.Position.X);
            distanceX -= radius + this.collider.Radius;
            var distanceY = Math.Abs((obstacle.GetShape().Position.Y - this.collider.Position.Y));
            distanceY -= radius + this.collider.Radius;

            hitInfo = new CollisionInfo(new Vector2f(distanceX, distanceY), imove.Direction);
        }

        this.collider.OutlineColor = c ? Color.Red : Color.Magenta;

        return c;
    }

    public Shape GetShape()
    {
        return collider;
    }

    public void SetSprite(Sprite sprite)
    {
        var radius = (sprite.GetGlobalBounds().Width > sprite.GetGlobalBounds().Height ? sprite.GetGlobalBounds().Width : sprite.GetGlobalBounds().Height) / 2;
        this.collider = new CircleShape(radius);
        this.collider.OutlineColor = Color.Magenta;
        this.collider.FillColor = Color.Transparent;
        this.collider.OutlineThickness = 2;
    }

    public void SolveCollision(CollisionInfo depth)
    {
        var mover = this.root.GetComponent<Mover>();
        if (mover != null)
        {
            Console.WriteLine("Depth collision:" + depth.ToString());
            switch (mover.Direction)
            {
                case Mover.EDirection.Left:
                    mover.Position += depth.Depth;
                    break;
                case Mover.EDirection.Right:
                    mover.Position -= depth.Depth;
                    break;
                case Mover.EDirection.Up:
                    //mover.Position -= depth.Depth;
                    break;
                case Mover.EDirection.Down:
                    //mover.Position += depth.Depth;
                    break;
            }
        }
    }

    public override void Update(float deltaTime)
    {
        if (this.IMove != null)
            this.collider.Position = this.IMove.Position;
        else
            Console.WriteLine("Renderer component requires an IMove reference's object to update position");
    }
}
