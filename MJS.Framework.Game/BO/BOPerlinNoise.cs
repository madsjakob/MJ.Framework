using MJS.Framework.Base.Utils;
using MJS.Framework.Game.DO;
using MJS.Framework.Game.ST;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.BO
{
    public class BOPerlinNoise
    {
        private static int[] perm = new int[] {
    151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,
    8,99,37,240,21,10,23,190,6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,
    35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,171,168,68,175,74,165,71,
    134,139,48,27,166,77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,
    55,46,245,40,244,102,143,54,65,25,63,161,1,216,80,73,209,76,132,187,208, 89,
    18,169,200,196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,52,217,226,
    250,124,123,5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,
    189,28,42,223,183,170,213,119,248,152,2,44,154,163,70,221,153,101,155,167,43,
    172,9,129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,218,246,97,
    228,251,34,242,193,238,210,144,12,191,179,162,241,81,51,145,235,249,14,239,
    107,49,192,214,31,181,199,106,157,184,84,204,176,115,121,50,45,127,4,150,254,
    138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
      
    151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,
    8,99,37,240,21,10,23,190,6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,
    35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,171,168,68,175,74,165,71,
    134,139,48,27,166,77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,
    55,46,245,40,244,102,143,54,65,25,63,161,1,216,80,73,209,76,132,187,208, 89,
    18,169,200,196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,52,217,226,
    250,124,123,5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,
    189,28,42,223,183,170,213,119,248,152,2,44,154,163,70,221,153,101,155,167,43,
    172,9,129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,218,246,97,
    228,251,34,242,193,238,210,144,12,191,179,162,241,81,51,145,235,249,14,239,
    107,49,192,214,31,181,199,106,157,184,84,204,176,115,121,50,45,127,4,150,254,
    138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
};

        public static DONoise PerlinNoise(int width, int height)
        {
            return PerlinNoise(width, height, RandomUtils.Random.Next());
        }

        public static DONoise PerlinNoise(int width, int height, int seed)
        {
            Random rand = new Random(seed);
            float max = float.MinValue;
            float min = float.MaxValue;
            int octaves = 10;
            float persistence = .6f;
            float lacunarity = 2f;

            STVector[] varr = new STVector[256];
            for (int index = 0; index < varr.Length; index++)
            {
                varr[index] = CreateGradient(rand);
            }
            float total;
            float amplitude;
            float frequency;
            DONoise noise = new DONoise(width, height);
            for (int x = 0; x < noise.Width; x++)
            {
                for (int y = 0; y < noise.Height; y++)
                {
                    total = 0;
                    amplitude = persistence;
                    frequency = 1f / width;
                    for (int index = 1; index <= octaves; index++)
                    {
                        total += Noise2D(x * frequency, y * frequency, varr) * amplitude;
                        frequency *= lacunarity;
                        amplitude *= persistence;
                        
                    }
                    max = (float)Math.Max(max, total);
                    min = (float)Math.Min(min, total);
                    noise[x, y] = total;
                }
            }
            // Nomalize
            float minmax = max - min;
            float min1 = float.MaxValue;
            float max1 = float.MinValue;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (max != 0)
                    {
                        float v = noise[x, y];
                        v -= min; // range = 0 to max - min
                        v /= minmax; // range = 0 to 1
                        min1 = (float)Math.Min(v, min1);
                        max1 = (float)Math.Max(v, max1);
                        noise[x, y] = v;// (() / minmax);
                    }
                }
            }
            return noise;
        }

        public static float Noise2D(float x, float y, STVector[] varr)
        {
            int x0 = (int)Math.Floor(x);
            int x1 = x0 + 1;
            int y0 = (int)Math.Floor(y);
            int y1 = y0 + 1;
            
            int ig00 = GetGradient(x0, y0, varr);
            int ig01 = GetGradient(x0, y1, varr);
            int ig10 = GetGradient(x1, y0, varr);
            int ig11 = GetGradient(x1, y1, varr);

            float s = varr[ig00].X * (x - x0) + varr[ig00].Y * (y - y0);
            float t = varr[ig10].X * (x - x1) + varr[ig10].Y * (y - y0);
            float u = varr[ig01].X * (x - x0) + varr[ig01].Y * (y - y1);
            float v = varr[ig11].X * (x - x1) + varr[ig11].Y * (y - y1);

            float xd = (x - x0);
            float sx = 3 * xd * xd - 2 * xd * xd * xd;
            float a = s + sx * (t - s);
            float b = u + sx * (v - u);
            float yd = y - y0;
            float sy = 3 * yd * yd - 2 * yd * yd * yd;
            float z = a + sy * (b - a);
            return z;
        }

        private static int FastFloor(double f)
        {
            return (f >= 0 ? (int)f : (int)f - 1);
        }

        private static int GetGradient(int x, int y, STVector[] varr)
        {
            while (x < 0)
            {
                x += 256;
            }
            while (y < 0)
            {
                y += 256;
            }
            return perm[(int)x % 256 + perm[(int)y % 256]];
        }
        
        private static STVector GetGradient(STVector p1, STVector[] varr)
        {
            int p = perm[(int)p1.X % 256 + perm[(int)p1.Y % 256]];
            return varr[p];
        }

        private static STVector CreateGradient(Random rand)
        {
            float factor = (float)(rand.NextDouble() * Math.PI * 2);
            return new STVector((float)Math.Cos(factor), (float)Math.Sin(factor));
        }
    }
}
