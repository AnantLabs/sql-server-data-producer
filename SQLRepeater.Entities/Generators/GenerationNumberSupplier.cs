using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return _number++;
        }

        public static void Reset()
        {
            _number = 1;
        }
    }
}
