using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.DO
{
    public abstract class DOTile
    {
        public DOTile()
        {
            Initialize();
        }

        public bool Obstacle { get; set; }
        public DOGrid Grid { get; set; }

        protected DOTile[] _travelOptions;

        protected abstract void Initialize();
        protected virtual DOTile GetTravelOption(int index)
        {
            return _travelOptions[index];
        }
        
        protected virtual void SetTravelOption(int index, DOTile tile)
        {
            _travelOptions[index] = tile;
        }

        public virtual DOTile[] GetTravelOptions()
        {
            DOTileList tileList = new DOTileList();
            foreach (DOTile tile in _travelOptions)
            {
                if (tile != null && !tile.Obstacle)
                {
                    tileList.Add(tile);
                }
            }
            return tileList.ToArray();
        }
    }

    public class DOTileList : List<DOTile>
    {
    }
}
