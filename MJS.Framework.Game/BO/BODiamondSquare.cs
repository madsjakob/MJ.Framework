using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJS.Framework.Game.DO;

namespace MJS.Framework.Game.BO
{
    public static class BODiamondSquare
    {
        private static Random _rand = new Random();

        public static DONoise DiamondSquare(int width, int height)
        {
            return DiamondSquare(width, height, 128, 1.0);
        }

        public static DONoise DiamondSquare(int width, int height, int samplesize, double scale)
        {
            float max = float.MinValue;
            float min = float.MaxValue;
            DONoise result = new DONoise(width, height);
            for (int y = 0; y < height; y += samplesize)
            {
                for (int x = 0; x < width; x += samplesize)
                {
                    result[x, y] = (float)Frand();  //IMPORTANT: frand() is a random function that returns a value between -1 and 1.
                    
                }
            }
            while(samplesize > 1) 
            {
                CoreDiamondSquare(result, width, height, samplesize, scale);
                samplesize /= 2;
                scale /= 2;
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    min = (float)Math.Min(result[x, y], min);
                    max = (float)Math.Max(result[x, y], max);
                }
            }
            float minmax = max - min;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (max != 0)
                    {
                        float v = result[x, y];
                        v -= min; 
                        v /= minmax; 
                        result[x, y] = v;
                    }
                }
            }

            return result;
        }

        private static void CoreDiamondSquare(DONoise data, int width, int height, int stepsize, double scale)
        {
            int halfstep = stepsize / 2;
            for (int y = halfstep; y < height + halfstep; y += stepsize)
            {
                for (int x = halfstep; x < width + halfstep; x += stepsize)
                {
                    SampleSquare(data, x, y, stepsize, Frand() * scale);
                }
            }
            for (int y = 0; y < height; y += stepsize)
            {
                for (int x = 0; x < width; x += stepsize)
                {
                    SampleDiamond(data, x + halfstep, y, stepsize, Frand() * scale);
                    SampleDiamond(data, x, y + halfstep, stepsize, Frand() * scale);
                }
            }
        }

        private static float Frand()
        {
            return (float)(2 * (_rand.NextDouble()));
        }

        private static void SampleSquare(DONoise data, int x, int y, int size, double value)
        {
            int halfSize = size / 2;
            double a = data[x - halfSize, y - halfSize];
            double b = data[x - halfSize, y + halfSize];
            double c = data[x + halfSize, y - halfSize];
            double d = data[x + halfSize, y + halfSize];
            data[x, y] = (float)((a + b + c + d) / 4.0 + value); // displacement
        }

        private static void SampleDiamond(DONoise data, int x, int y, int size, double value)
        {
            int halfSize = size / 2;
            double a = data[x - halfSize, y];
            double b = data[x, y - halfSize];
            double c = data[x + halfSize, y];
            double d = data[x, y + halfSize];
            data[x, y] = (float)((a + b + c + d) / 4.0 + value); // displacement
        }
    }
}
