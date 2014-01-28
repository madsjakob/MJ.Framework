using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.DO
{
    public class DONoise
    {
        public DONoise(int width, int height)
        {
            _width = width;
            _height = height;
            _data = new float[_width, _height];
        }

        private float[,] _data;
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

        public float this[int x, int y]
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
                while (x < 0)
                {
                    x += Width;
                }
                while (y < 0)
                {
                    y += Height;
                }
                _data[x % Width, y % Height] = value;
            }
        }
    }
}
