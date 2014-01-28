using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.ST
{
    public struct STVector
    {
        public STVector(float x, float y)
        {
            X = x;
            Y = y;
        }

        private static STVector _empty = new STVector();
        public static STVector Empty
        {
            get { return _empty; }
        }

        public float X;
        public float Y;

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y);
            }
        }

        public static bool operator ==(STVector v1, STVector v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(STVector v1, STVector v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y;
        }

        public static STVector operator +(STVector v1, STVector v2)
        {
            return new STVector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static STVector operator -(STVector v1, STVector v2)
        {
            return new STVector(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static float operator *(STVector v1, STVector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}", X, Y);
        }
    }
}
