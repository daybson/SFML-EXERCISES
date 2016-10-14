using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Extensions
{
    /// <summary>
    /// Return the vector's magnitude. http://www.softschools.com/formulas/physics/unit_vector_formula/83/
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float Magnitude(this Vector2f v)
    {
        return (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
    }

    /// <summary>
    /// A unit vector is a vector that has a magnitude of 1. http://www.softschools.com/formulas/physics/unit_vector_formula/83/
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2f Unit(this Vector2f v)
    {
        var magnitude = v.Magnitude();
        return new Vector2f(v.X / magnitude, v.Y / magnitude);
    }

    /// <summary>
    /// Calculate the dot product between the Vectors
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static float Dot(this Vector2f v1, Vector2f v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y;
    }


    /// <summary>
    ///  Pperpendicular (perpendicularity) is the relationship between two lines/vectors which meet at a right angle (90 degrees)
    ///  https://en.wikipedia.org/wiki/Perpendicular
    /// </summary>
    /// <param name="v1">The clockwise perpendicular vector (-y)</param>
    /// <returns></returns>
    public static Vector2f Perpendicular(this Vector2f v1)
    {
        return new Vector2f(-v1.Y, v1.X);
    }
    
    public static Vector2f ProjectOnto(this Vector2f v1, Vector2f v2)
    {
        var dot = v1.Dot(v2);
        var v2Magnitude = v2.Magnitude();
        return new Vector2f(dot / v2Magnitude * v2.X, dot / v2Magnitude * v2.Y);
    }


    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }
}
