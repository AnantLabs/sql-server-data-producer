using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Snippets
{
    public static class RandomSupplier
    {
        static Random _random = new Random(DateTime.Now.Millisecond);
        public static Random Randomer
        {
            get
            {
                return _random;
            }
        }
    }
}
