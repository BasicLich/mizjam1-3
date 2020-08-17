using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Helpers
{
    internal class MathHelpers
    {
        internal static Vector2 ClosestPointOnLine(float lx1, float ly1, float lx2, float ly2, float x0, float y0)
        {
            float A1 = ly2 - ly1;
            float B1 = lx1 - lx2;
            float C1 = (ly2 - ly1) * lx1 + (lx1 - lx2) * ly1;
            float C2 = -B1 * x0 + A1 * y0;
            float det = A1 * A1 - -B1 * B1;
            float cx;
            float cy;
            if (det != 0)
            {
                cx = (float)((A1 * C1 - B1 * C2) / det);
                cy = (float)((A1 * C2 - -B1 * C1) / det);
            }
            else
            {
                cx = x0;
                cy = y0;
            }
            return new Vector2(cx, cy);
        }
    }
}
