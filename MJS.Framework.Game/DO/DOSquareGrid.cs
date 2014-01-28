using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.DO
{
    public class DOSquareGrid : DOGrid
    {
        public DOSquareGrid(int width, int height)
        {
            _width = width;
            _height = height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    DOSquareTile tile = new DOSquareTile();
                    if (y > 0)
                    {
                        this[x, y - 1].South = tile;
                        tile.North = this[x, y - 1];
                    }
                    if (x > 0)
                    {
                        this[x - 1, y].East = tile;
                        tile.West = this[x - 1, y];
                    }
                    Add(tile);
                }
            }
        }

        public override int IndexOf(DOTile tile)
        {
            return _tiles.IndexOf(tile);
        }

        public override int Heuristic(DOTile tile1, DOTile tile2)
        {
            int index1 = IndexOf(tile1);
            int x1 = index1 % Width;
            int y1 = index1 / Width;
            int index2 = IndexOf(tile2);
            int x2 = index2 % Width;
            int y2 = index2 / Width;
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        private int _width;
        public int Width
        {
            get { return _width; }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
        }

        public DOSquareTile this[int x, int y]
        {
            get
            {
                return (DOSquareTile)_tiles[x + y * Width];
            }
        }
    }
}
