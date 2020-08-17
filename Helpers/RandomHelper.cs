using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Helpers
{
    internal class RandomHelper
    {
        public readonly static Random Random = new Random();

        public static float NextFloat()
        {
            return (float)Random.NextDouble();
        }
    }
}
