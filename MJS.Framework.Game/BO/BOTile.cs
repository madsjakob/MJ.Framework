using MJS.Framework.Game.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.BO
{
    public class BOTile
    {
        public static DOPathItem FindShortestPath(DOTile start, DOTile end, DOPathItemList pathList = null)
        {
            DOPathItem current = null;
            if (start != null && end != null)
            {
                if (pathList == null)
                {
                    pathList = new DOPathItemList();
                }
                DOPathItem item = new DOPathItem();
                item.Tile = end;
                pathList.Add(item);
                do
                {
                    current = pathList.GetBestOpen();
                    current.Closed = true;
                    if (current != null && current.Tile != start)
                    {
                        DOTile[] optionList = current.Tile.GetTravelOptions();
                        foreach (DOTile option in optionList)
                        {
                            DOPathItem oldItem = pathList.GetElement(option);
                            item = new DOPathItem();
                            item.Tile = option;
                            item.From = current;
                            item.CostToEnd = Math.Abs(option.Grid.Heuristic(start, option));
                            if (oldItem != null)
                            {
                                if (!oldItem.Closed && oldItem.Score > item.Score)
                                {
                                    oldItem.From = current;
                                    oldItem.CostToEnd = Math.Abs(option.Grid.Heuristic(start, option));
                                }
                            }
                            else 
                            {
                                pathList.Add(item);
                            }
                        }
                    }
                }
                while (current != null && current.Tile != start);
            }
            return current;
        }
    }
}
