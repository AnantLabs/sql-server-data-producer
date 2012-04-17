using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Generators
{
    public class GeneratorBase
    {
        protected static string Wrap(object s)
        {
            return string.Format("'{0}'", s);
        }
    }
}
