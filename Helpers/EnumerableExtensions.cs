using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mizjam1.Helpers
{
    internal static class EnumerableExtensions
    {
        internal static Point GetRandom(this IEnumerable<Point> e)
        {
            int idx = (int)(RandomHelper.NextFloat() * e.Count());
            return e.ElementAt(idx);
        }
        internal static Vector2 GetRandom(this IEnumerable<Vector2> e)
        {
            int idx = (int)(RandomHelper.NextFloat() * e.Count());
            return e.ElementAt(idx);
        }
    }
}
