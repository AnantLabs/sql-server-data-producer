// Copyright 2012-2013 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;

namespace SQLDataProducer.Entities.Generators
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
            _random = new Random((int)System.DateTime.Now.Ticks % System.Int32.MaxValue);
        }

        public int GetNextInt()
        {
            lock (_random)
            {
                return _random.Next();
            }
        }
        public int GetNextInt(int min, int max)
        {
            lock (_random)
            {
                return _random.Next(min, max);
            }
        }
        public long GetNextLong()
        {
            //http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/2e08381b-1e2d-459f-a7c9-986954321958/
            lock (_random)
            {
                return (long)((_random.NextDouble() * 2.0 - 1.0) * long.MaxValue);
            }
        }
        public decimal GetNextDecimal()
        {
            lock (_random)
            {
                return (decimal)_random.NextDouble();
            }
        }
        public double GetNextDouble()
        {
            lock (_random)
            {
                return _random.NextDouble();
            }
        }
        readonly Random _random;
        
    }
}
