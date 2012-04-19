using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Entities.Generators
{
    public sealed class RandomSupplier
    {
        static readonly RandomSupplier instance = new RandomSupplier();

        public static RandomSupplier Instance
        {
            get
            {
                return instance;
            }
        }

        static RandomSupplier()
        {        
        }

        RandomSupplier()
        {
            _random = new Random(DateTime.Now.Millisecond);
        }

        public int GetNextInt()
        {
            return _random.Next();
        }
        public double GetNextDouble()
        {
            return _random.NextDouble();
        }
        
        readonly Random _random;
        
    }
}
