using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mizjam1.Helpers
{
    internal class AStar
    {
        private HashSet<Point> OpenSet;
        private HashSet<Point> ClosedSet;
        private bool[,] Grid;
        private int Size;
        internal AStar(bool[,] grid, Point start)
        {
            Grid = grid;
            Size = Grid.GetUpperBound(0);
            OpenSet = new HashSet<Point> { start };
            ClosedSet = new HashSet<Point>();
            Fill();
        }
        
        internal HashSet<Point> GetSafePath()
        {
            var set = new HashSet<Point>();
            foreach (var n in ClosedSet)
            {
                set.Add(n);
            }
            return set;
        }
        internal List<Point> Neighbours(Point source)
        {
            var toTest = new List<Point>
            {
                source + new Point(1, 0),
                source + new Point(-1, 0),
                source + new Point(0, 1),
                source + new Point(0, -1),
            };
            var n = new List<Point>();
            foreach (var p in toTest)
            {
                if (p.OffGrid(Size))
                {
                    continue;
                }

                if (Grid[p.X, p.Y])
                {
                    continue;
                }

                n.Add(p);
            }
            return n;
        }
        internal void Fill()
        {
            while (OpenSet.Any())
            {
                var current = OpenSet.ElementAt(0);
                OpenSet.Remove(current);
                ClosedSet.Add(current);
                foreach (var n in Neighbours(current).Where(n => !ClosedSet.Contains(n)))
                {
                    OpenSet.Add(n);
                }
            }
        }
    }
}
