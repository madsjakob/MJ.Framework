using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.DO
{
    public class DOPathItem
    {
        public DOTile Tile { get; set; }
        private DOPathItem _from;
        public DOPathItem From 
        {
            get { return _from; }
            set 
            { 
                _from = value;
                CostFromStart = (1 + (_from != null ? _from.CostFromStart : 0));
            }
        }
        public int CostFromStart { get; set; }
        public int CostToEnd { get; set; }
        public int Score
        {
            get { return CostFromStart + CostToEnd; }
        }
        public bool Closed { get; set; }

        public bool Contains(DOTile tile)
        {
            DOPathItem temp = this;
            bool found = false;
            while (!found && temp != null)
            {
                if (temp.Tile == tile)
                {
                    found = true;
                }
                else
                {
                    temp = temp.From;
                }
            }
            return found;
        }
    }

    public class DOPathItemList : List<DOPathItem>
    {
        public DOPathItem GetBestOpen()
        {
            int score = int.MaxValue;
            DOPathItem item = null;
            for (int index = 0; index < Count; index++)
            {
                if (!this[index].Closed && this[index].Score <= score)
                {
                    item = this[index];
                    score = item.Score;
                }
            }
            return item;
        }

        public bool Contains(DOTile tile)
        {
            bool found = false;
            int index = 0;
            while(!found && index < Count)
            {
                found = (this[index++].Tile == tile);
            }
            return found;
        }

        public DOPathItem GetElement(DOTile tile)
        {
            DOPathItem result = null;
            int index = 0;
            while (result == null && index < Count)
            {
                if (this[index].Tile == tile)
                {
                    result = this[index];
                }
                else
                {
                    ++index;
                }
            }
            return result;
        }
    }
}
