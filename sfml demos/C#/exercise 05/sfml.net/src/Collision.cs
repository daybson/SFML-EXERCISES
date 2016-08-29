using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sfml.net.src
{
    class Collision
    {
        public static List<Shape> collisionShapes = new List<Shape>();

        public static bool CheckCollisionRectangleAxisAligned(RectangleShape r1, RectangleShape r2)
        {
            return (Math.Abs(r1.Position.X - r2.Position.X) * 2 < (r1.Size.X + r2.Size.X)) &&
                   (Math.Abs(r1.Position.Y - r2.Position.Y) * 2 < (r1.Size.Y + r2.Size.Y));
        }

        public static bool CheckCollisionExtentsRectangleAxisAligned(RectangleShape r1, RectangleShape r2)
        {
            var x1 = r1.Position.X;
            var y1 = r1.Position.Y;
            var x3 = r1.Size.X;
            var y3 = r1.Size.Y;

            var x2 = r2.Position.X;
            var y2 = r2.Position.Y;
            var x4 = r2.Size.X;
            var y4 = r2.Size.Y;

            if ((x1 + x3 < x2) || (x1 > x2 + x4) || (y1 + y3 < y2) || (y1 > y2 + y4))
                return false;

            return true;
        }

        public static bool CheckCollisionSphere(RectangleShape r1, RectangleShape r2)
        {
            var radius1 = (r1.Size.X > r1.Size.Y ? r1.Size.X : r1.Size.Y) / 2;
            var radius2 = (r2.Size.X > r2.Size.Y ? r2.Size.X : r2.Size.Y) / 2;

            var c1 = new CircleShape(radius1);
            var c2 = new CircleShape(radius2);

            c1.FillColor = new Color(0, 0, 0, 0);
            c1.OutlineColor = new Color(255, 0, 0);
            c1.OutlineThickness = 1;
            c1.Origin = new Vector2f(0, 0);
            c1.Position = r1.Position;

            c2.FillColor = c1.FillColor;
            c2.OutlineColor = c1.OutlineColor;
            c2.OutlineThickness = 1;
            c2.Origin = new Vector2f(0, 0);
            c2.Position = r2.Position;

            var d = Math.Sqrt((c2.Position.X - c1.Position.X) * (c2.Position.X - c1.Position.X) +
                               (c2.Position.Y - c1.Position.Y) * (c2.Position.Y - c1.Position.Y));

            collisionShapes.Clear();
            collisionShapes.Add(c1);
            collisionShapes.Add(c2);

            return d <= radius1 + radius2;
        }
    }
}
