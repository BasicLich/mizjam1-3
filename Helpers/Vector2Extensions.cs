using Microsoft.Xna.Framework;
using System;

namespace mizjam1.Helpers
{
    internal static class Vector2Extensions
    {
        internal static float Manhattan(this Vector2 own, Vector2 other)
        {
            return Math.Abs(own.X - other.X) + Math.Abs(own.Y - other.Y);
        }
        internal static float Manhattan(this Point own, Point other)
        {
            return Math.Abs(own.X - other.X) + Math.Abs(own.Y - other.Y);
        }
        internal static float Dist(this Point own, Point other)
        {
            var a = own.X - other.X;
            var b = own.Y - other.Y;
            return MathF.Sqrt(a * a + b * b);
        }
        internal static bool OffGrid(this Vector2 p, int Size)
        {
            return p.X < 0 || p.X > Size || p.Y < 0 || p.Y > Size;
        }
        internal static bool OffGrid(this Point p, int Size)
        {
            return p.X < 0 || p.X > Size || p.Y < 0 || p.Y > Size;
        }
    }
}
