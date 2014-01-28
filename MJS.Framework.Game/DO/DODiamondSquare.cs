using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJS.Framework.Game.DO
{
    public class DODiamondSquare
    {
        public DODiamondSquare(int width, int height)
        {
            Width = width;
            Height = height;
            _data = new double[Width, Height];
            Min = 100;
            Max = -100;
        }

        public double Min { get; set; }
        public double Max { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private double[,] _data;

        public double this[int x, int y]
        {
            get
            {
                while (x < 0)
                {
                    x += Width;
                }
                while (y < 0)
                {
                    y += Height;
                }
                return _data[x % Width, y % Height];
            }
            set
            {
                _data[(x + Width) % Width, (y + Height) % Height] = value;
                Max = Math.Max(value, Max);
                Min = Math.Min(value, Min);
            }
        }

        public double GetUnifiedValue(int x, int y)
        {
            return (this[x, y] - Min) / (Max - Min);
        }

        internal void CalcMinMax()
        {
            Min = 100;
            Max = -100;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Max = Math.Max(_data[x,y], Max);
                    Min = Math.Min(_data[x, y], Min);
                }
            }
        }
    }
}
