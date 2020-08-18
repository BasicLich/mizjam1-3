using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Helpers
{
    internal class Dungeon
    {
        internal bool[,] Grid;
        internal bool[,] GridCopy;
        internal int Size;

        internal Dungeon(int size)
        {
            Size = size;
            Grid = new bool[Size, Size];
            GridCopy = new bool[Size, Size];
            Populate(0.3f);
            for (int _ = 0; _ < 15; _++)
            {
                Iterate();
            }
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
        }
    }
}
