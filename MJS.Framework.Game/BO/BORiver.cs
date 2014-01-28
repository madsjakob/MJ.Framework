using MJS.Framework.Game.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.BO
{
    public class BORiver
    {
        public static DONoise Generate(DONoise data, int n)
        {
            DONoise river = new DONoise(data.Width, data.Height);
            for (int index = 0; index < n; index++)
            {
                CreateRiver(data, river);
            }
            return river;
        }

        private static void CreateRiver(DONoise data, DONoise river)
        {
            // Select start
            Random rand = new Random();
            int xStart, yStart;
            do
            {
                xStart = rand.Next(data.Width);
                yStart = rand.Next(data.Height);
            }
            while (data[xStart, yStart] < .6);
            DOWaterPathList waterPathList = new DOWaterPathList();
            DOWaterPath waterPath = new DOWaterPath();
            waterPath.X = xStart;
            waterPath.Y = yStart;
            waterPathList.Add(waterPath);
            int count = 0;
            while (count < 500)
            {
                count++;
                river[xStart, yStart] += .01f;
                MoveWater(data, river, waterPathList);
            }

            float max = float.MinValue;
            float min = float.MaxValue;
            for (int y = 0; y < river.Height; y++)
            {
                for (int x = 0; x < river.Width; x++)
                {
                    max = Math.Max(max, river[x, y]);
                    min = Math.Min(min, river[x, y]);
                }
            }

            for (int y = 0; y < river.Height; y++)
            {
                for (int x = 0; x < river.Width; x++)
                {
                    max = Math.Max(max, river[x, y]);
                    river[x, y] = river[x, y] / max;
                }
            }

        }

        private static void MoveWater(DONoise data, DONoise river, DOWaterPathList waterPathList)
        {
            DOWaterPathList newPath = new DOWaterPathList();
            for(int index = waterPathList.Count - 1; index >= 0; index--)
            //foreach(DOWaterPath item in waterPathList)
            {
                DOWaterPath item = waterPathList[index];
                int x = item.X;
                int y = item.Y;
                float v = 0;
                float v1 = GetValue(x - 1, y, data, river);
                float v2 = GetValue(x + 1, y, data, river);
                float v3 = GetValue(x, y - 1, data, river);
                float v4 = GetValue(x, y + 1, data, river);
                if (v1 <= v2 && v1 <= v3 && v1 <= v4)
                {
                    x--;
                    v = v1;
                }
                else if (v2 <= v1 && v2 <= v3 && v2 <= v4)
                {
                    x++;
                    v = v2;
                }
                else if (v3 <= v1 && v3 <= v2 && v3 <= v4)
                {
                    y--;
                    v = v3;
                }
                else if (v4 <= v1 && v4 <= v2 && v4 <= v3)
                {
                    y++;
                    v = v4;
                }
                if (x >= 0 && x < data.Width && y >= 0 && y < data.Height && data[x,y] > .5f)
                {
                    float f = GetValue(item, data, river) - v;
                    if (f > 0)
                    {
                        float maxFlow = f / 2f;
                        float flow = (float)Math.Min(maxFlow, river[item.X, item.Y]);
                        river[item.X, item.Y] -= flow;
                        river[x, y] += flow;
                        int iP = waterPathList.IndexOf(x, y);
                        if (iP == -1)
                        {
                            DOWaterPath newItem = new DOWaterPath();
                            newItem.X = x;
                            newItem.Y = y;

                            item.Next.Add(newItem);
                            newPath.Add(newItem);
                        }
                        else
                        {
                            if (!item.Next.Contains(waterPathList[iP]))
                            {
                                item.Next.Add(waterPathList[iP]);
                            }
                        }
                    }
                }

            }
            waterPathList.AddRange(newPath);
        }

        private static float GetValue(DOWaterPath item, DONoise data, DONoise river)
        {
            return GetValue(item.X, item.Y, data, river);
        }
        private static float GetValue(int x, int y, DONoise data, DONoise river)
        {
            return data[x, y] + river[x, y];
        }
        
    }
}
