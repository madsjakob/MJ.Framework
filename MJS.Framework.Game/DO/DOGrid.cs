using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.DO
{
    public abstract class DOGrid
    {
        public DOGrid()
        {
            _tiles = new DOTileList();
        }

        protected void Add(DOTile tile)
        {
            tile.Grid = this;
            _tiles.Add(tile);
        }

        protected DOTileList _tiles;

        public abstract int IndexOf(DOTile tile);
        public abstract int Heuristic(DOTile tile1, DOTile tile2);
    }
}
