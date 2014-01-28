using MJS.Framework.Base.Utils;
using MJS.Framework.Game.DO;
using MJS.Framework.Game.ST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.BO
{
    public class BOSimplexNoise
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


        public static DONoise SimplexNoise(int width, int height)
        {
            return SimplexNoise(width, height, new Random(RandomUtils.Random.Next()));
        }

        public static DONoise SimplexNoise(int width, int height, Random rand)
        {
            float octaves = 10;
            float lacunarity = 2f;
            float persistence = .65f;
            DONoise noise = new DONoise(width, height);
            STVector[] varr = new STVector[256];
            float max = float.MinValue;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float frequency = 1f / width;
                    float amplitude = persistence;
                    float total = 0f;
                    for (int index = 1; index <= octaves; ++index)
                    {
                        total += Simplex(x * frequency, y * frequency, varr, rand) * amplitude;
                        frequency *= lacunarity;
                        amplitude *= persistence;
                    }
                    max = (float)Math.Max(max, total);
                    noise[x, y] = total;
                }
            }

            // Nomalize
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noise[x, y] = noise[x, y] / max;
                }
            }
            return noise;
        }

        private static float Simplex(float x, float y, STVector[] varr, Random rand)
        {
            const float F2 = 0.366025403f; // F2 = 0.5*(sqrt(3.0)-1.0)
            const float G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

            float s = (x + y) * F2;
            float xs = x + s;
            float ys = y + s;
            int i = (int)Math.Floor(xs);
            int j = (int)Math.Floor(ys);
            float t = (float)(i + j) * G2;
            float X0 = i - t;
            float Y0 = j - t;
            float x0 = x - X0;
            float y0 = y - Y0;
            int i1, j1;
            if (x0 > y0)
            {
                i1 = 1;
                j1 = 0;
            }
            else
            {
                i1 = 0;
                j1 = 1;
            }
            STVector p = new STVector(x, y);
            STVector p1 = new STVector(X0, Y0);
            STVector p2 = new STVector(i1 - G2, j1 - G2);
            STVector p3 = new STVector(1 - 2 * G2, 1 - 2 * G2);
            float x1 = x0 - i1 + G2;
            float y1 = y0 - j1 + G2;
            float x2 = x0 - 1 + 2 * G2;
            float y2 = y0 - 1 + 2 * G2;

            int ii = i % 256;
            int jj = j % 256;
            float result = 0;
            STVector g1 = GetGradient(perm[ii + perm[jj]], varr, rand);
            STVector g2 = GetGradient(perm[ii + i1 + perm[jj + j1]], varr, rand);
            STVector g3 = GetGradient(perm[ii + 1 + perm[jj + 1]], varr, rand);

            STVector d1 = p - p1;
            STVector d2 = p - p2;
            STVector d3 = p - p3;
            float r1 = GetRadius(d1);
            float r2 = GetRadius(d2);
            float r3 = GetRadius(d3);
            result += Contribution(g1, d1, r1);
            result += Contribution(g2, d2, r2);
            result += Contribution(g3, d3, r3);
            return result;
        }

        private static float Contribution(STVector gradient, STVector distance, float radius)
        {
            return radius * radius * radius * radius * (gradient * distance);
        }

        private static float GetRadius(STVector vector)
        {
            float result = .5f - vector.X * vector.X - vector.Y * vector.Y;
            if (result < 0)
            {
                result = 0;
            }
            return result;
        }

        //private static STVector GetGradient(STVector vector, STVector[,] varr, Random rand)
        //{
        //    int width = varr.GetLength(0);
        //    int height = varr.GetLength(1);
        //    int x = ((int)vector.X) % width;
        //    int y = ((int)vector.Y) % height;
        //    if (x < 0)
        //    {
        //        x += width;
        //    }
        //    if (y < 0)
        //    {
        //        y += height;
        //    }
        //    if (varr[x, y] == STVector.Empty)
        //    {
        //        float factor = (float)(rand.NextDouble() * Math.PI * 2);
        //        varr[x, y] = new STVector((float)Math.Cos(factor), (float)Math.Sin(factor));
        //    }
        //    return varr[x, y];
        //}

        private static STVector GetGradient(int p, STVector[] varr, Random rand)
        {
            if (varr[p] == STVector.Empty)
            {
                float factor = (float)(rand.NextDouble() * Math.PI * 2);
                varr[p] = new STVector((float)Math.Cos(factor), (float)Math.Sin(factor));
            }
            return varr[p];
        }
    }
}
