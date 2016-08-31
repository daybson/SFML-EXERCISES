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
        public static Vertex[] gap;
        public static Vertex[] perpendicular;


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

            if((x1 + x3 < x2) || (x1 > x2 + x4) || (y1 + y3 < y2) || (y1 > y2 + y4))
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


        public static bool CheckCollistionSeparatingAxisTheorem(Shape s1, Shape s2)
        {
            collisionShapes.Clear();
            if(IsThereAxisSeparating(s1, s2))
                return false;

            return !IsThereAxisSeparating(s2, s1);
        }

        private static bool IsThereAxisSeparating(Shape s1, Shape s2)
        {
            uint polycountS1 = s1.GetPointCount();

            for(uint i = 0; i < polycountS1; i++)
            {
                var nextPoint = (i + 1) % polycountS1;
                var side = s1.Transform.TransformPoint(s1.GetPoint(nextPoint)) - s1.Transform.TransformPoint(s1.GetPoint(i));
                var perpendicular = side.Perpendicular().Unit();

                var minMax1 = ProjectShape(s1, perpendicular);
                var minMax2 = ProjectShape(s2, perpendicular);

                if(minMax1.Y < minMax2.X || minMax2.Y < minMax1.X)
                    return true;
            }

            return false;
        }

        private static Vector2f ProjectShape(Shape shape, Vector2f axis)
        {
            var point = shape.Transform.TransformPoint(shape.GetPoint(0));
            var initial = point.Dot(axis);
            var minmax = new Vector2f(initial, initial);

            for(uint i = 0; i < shape.GetPointCount(); i++)
            {
                point = shape.Transform.TransformPoint(shape.GetPoint(i));
                var projected = point.Dot(axis);

                perpendicular = new Vertex[2] { new Vertex(point), new Vertex(point*projected) };

                if(projected < minmax.X)
                    minmax.X = projected;
                if(projected > minmax.Y)
                    minmax.Y = projected;
            }
            return minmax;
        }

        public static bool SAT(Shape s1, Shape s2)
        {
            collisionShapes.Clear();

            //calculate the gap 
            var length = s1.Position.X + s1.GetGlobalBounds().Width / 2 - s2.Position.X + s2.GetGlobalBounds().Width / 2;
            var halfWidthS1 = s1.GetGlobalBounds().Width / 2;
            var halfWidthS2 = s2.GetGlobalBounds().Width / 2;
            var gap = length - halfWidthS1 - halfWidthS2;
            //Console.WriteLine("Gap: " + gap);

            //create gizmo
            var gapS1 = new Vector2f(s1.Position.X + s1.GetGlobalBounds().Width / 2, s1.Position.Y + s1.GetGlobalBounds().Height / 2);
            var gapS2 = new Vector2f(s2.Position.X + s2.GetGlobalBounds().Width / 2, s2.Position.Y + s2.GetGlobalBounds().Height / 2);
            Collision.gap = new Vertex[2];
            Collision.gap[0] = new Vertex(gapS1, Color.Yellow);
            Collision.gap[1] = new Vertex(gapS2, Color.Yellow);

            float minProj1 = 0;
            float maxProj1 = 0;
            uint minIndex1 = 0;
            uint maxIndex1 = 0;
            float minProj2 = 0;
            float maxProj2 = 0;
            uint minIndex2 = 0;
            uint maxIndex2 = 0;


            for(uint i = 0; i < s1.GetPointCount(); i++)
            {
                //Define edge's normal perpendicularAxis
                var p2Index = (i + 1) % s1.GetPointCount(); // i + 1 <= s1.GetPointCount() ? i + 1 : 0;
                var side = s1.Transform.TransformPoint(s1.GetPoint(p2Index));
                var edgeNormal = side.Perpendicular().Unit();

                minProj1 =s1.GetPoint(0).Dot(edgeNormal);
                maxProj1 =s1.GetPoint(0).Dot(edgeNormal);

                minProj2 = s2.GetPoint(0).Dot(edgeNormal);
                maxProj2 = s2.GetPoint(0).Dot(edgeNormal);

                for(uint j = 0; j < s1.GetPointCount(); j++)
                {
                    var projection = s1.Transform.TransformPoint(s1.GetPoint(j)).Dot(edgeNormal);
                    if(minProj1 > projection)
                    {
                        minProj1 = projection;
                        minIndex1 = j;
                    }
                    if(maxProj1 < projection)
                    {
                        maxProj1 = projection;
                        maxIndex1 = j;
                    }

                    if(minProj2 > projection)
                    {
                        minProj2 = projection;
                        minIndex2 = j;
                    }
                    if(maxProj2 < projection)
                    {
                        maxProj2 = projection;
                        maxIndex2 = j;
                    }
                }

            }
            if(maxProj2 < minProj1 || maxProj1 < minProj2)
                Console.WriteLine("Separado");
            else
                Console.WriteLine("Não Separado");

            return false;
        }
    }
}
