using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SQLRepeater.Entities.Generators
{
    public static class GenerationNumberSupplier
    {
        static int _number;

        static GenerationNumberSupplier()
        {
            _number = 1;
        }

        public static int GetNextNumber()
        {
            return Interlocked.Increment(ref _number);
        }

        public static void Reset()
        {
            _number = 1;
        }

        public static int CurrentNumber()
        {
            return _number;
        }
    }
}
