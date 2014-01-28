using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Base.Utils
{
    public class RandomUtils
    {
        private static Random _random;
        public static Random Random
        {
            get 
            {
                if (_random == null)
                {
                    _random = new Random();
                }
                return _random; 
            }
        }
    }
}
