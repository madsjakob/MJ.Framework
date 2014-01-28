using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.DO
{
    public class DOWaterPath
    {
        public int X { get; set; }
        public int Y { get; set; }
        private DOWaterPathList _next = new DOWaterPathList();
        public List<DOWaterPath> Next
        {
            get { return _next; } 
        }
    }

    public class DOWaterPathList : List<DOWaterPath>
    {
        public int IndexOf(int x, int y)
        {
            int index = 0;
            bool found = false;
            while (index < Count && !found)
            {
                if (this[index].X == x && this[index].Y == y)
                {
                    found = true;
                }
                else
                {
                    ++index;
                }
            }
            if (!found)
            {
                index = -1;
            }
            return index;
        }
    }
}
