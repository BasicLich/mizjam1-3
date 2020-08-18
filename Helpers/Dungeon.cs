using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace mizjam1.Helpers
{
    internal class Dungeon
    {
        internal bool[,] Grid;
        internal bool[,] GridCopy;
        internal int Size;
        internal bool OpenTop, OpenBottom, OpenLeft, OpenRight;
        internal bool[] Openings;
        internal Point[] OpeningLocations;
        internal HashSet<Point> SafePoints;

        internal Dungeon(int size)
        {
            Size = size;
            do
            {
                MakeOpenings();

            } while (Openings.Count(o => o) < 2);

            int attempts = 0;
            //do
            //{
                Generate();
                attempts++;
            //} while (!HasConnectivity());
            var connectivity = HasConnectivity();
        }

        private void MakeOpenings()
        {
            OpenTop = RandomHelper.NextBool();
            OpenRight = RandomHelper.NextBool();
            OpenBottom = RandomHelper.NextBool();
            OpenLeft = RandomHelper.NextBool();
            Openings = new bool[4];
            Openings[0] = OpenTop;
            Openings[1] = OpenRight;
            Openings[2] = OpenBottom;
            Openings[3] = OpenLeft;

            OpeningLocations = new Point[4];
            OpeningLocations[0] = new Point(Size / 2, 0);
            OpeningLocations[1] = new Point(Size - 1, Size / 2);
            OpeningLocations[2] = new Point(Size / 2, Size - 1);
            OpeningLocations[3] = new Point(0, Size / 2);
        }

        private void Generate()
        {
            Grid = new bool[Size, Size];
            GridCopy = new bool[Size, Size];

            Populate(0.35f);
            for (int _ = 0; _ < 5; _++)
            {
                Iterate();
            }

            MakeBorder(1);
            OpenDoors(2, 12);
        }
        internal bool HasConnectivity()
        {
            SafePoints = new HashSet<Point>();
            if (Openings.Count(o => o) < 2)
            {
                return true;
            }

            var openingsChecked = new List<Point>();
            AStar aStar = null;
            for (int i = 1; i < 4; i++)
            {
                if (!Openings[i])
                {
                    continue;
                }
                if (aStar == null)
                {
                    aStar = new AStar(Grid, OpeningLocations[i]);
                }
                else
                {
                    if (!aStar.GetSafePath().Contains(OpeningLocations[i]))
                    {
                        return false;
                    }
                }
            }
            SafePoints = aStar.GetSafePath();
            return true;
        }

        private void Iterate()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Grid[i, j])
                    {
                        GridCopy[i, j] = AliveNeighbours(i, j) >= 4;
                    }
                    else
                    {
                        GridCopy[i, j] = AliveNeighbours(i, j) >= 5;
                    }
                }
            }
            Copy();
        }
        private void Copy()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = GridCopy[i, j];
                }
            }
        }
        private int AliveNeighbours(int x, int y)
        {
            var neighbours = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }
                    if (OffGrid(i + x, j + y) || Grid[i + x, j + y])
                    {
                        neighbours++;
                    }
                }
            }
            return neighbours;
        }

        private bool OffGrid(int x, int y)
        {
            return x < 0 || x >= Size || y < 0 || y >= Size;
        }
        private void Populate(float chance = 0.5f)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = RandomHelper.NextBool(chance);
                }
            }
            MakeBorder(2);

            OpenDoors(2);
        }

        private void MakeBorder(int width = 1)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Grid[i, j] = true;
                    Grid[j, i] = true;
                    Grid[i, Size - 1 - j] = true;
                    Grid[Size - 1 - j, i] = true;
                }
            }
        }

        private void OpenDoors(int size = 2, int length = 2)
        {
            var min = -size;
            var max = size + 1;
            for (int i = 0; i < Size; i++)
            {
                if (OpenTop && i == Size / 2)
                {
                    for (var x = min; x < max; x++)
                    {
                        for (int y = 0; y < length; y++)
                        {
                            Grid[i + x, y] = false;
                        }
                    }
                }
                if (OpenBottom && i == Size / 2)
                {
                    for (int x = min; x < max; x++)
                    {
                        for (int y = 0; y < length; y++)
                        {
                            Grid[i + x, Size - y - 1] = false;
                        }
                    }
                }
                if (OpenLeft && i == Size / 2)
                {
                    for (int x = min; x < max; x++)
                    {
                        for (int y = 0; y < length; y++)
                        {
                            Grid[y, i + x] = false;
                        }
                    }
                }
                if (OpenRight && i == Size / 2)
                {
                    for (int x = min; x < max; x++)
                    {
                        for (int y = 0; y < length; y++)
                        {
                            Grid[Size - y - 1, i + x] = false;
                        }
                    }
                }
            }
        }
    }
}
