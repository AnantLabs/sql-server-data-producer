using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {
        public static readonly string GENERATOR_BinaryStaticGenerator = "Static Binary Value";

        private static Generator Create_Binary_StaticGenerator(int length)
        {
            //GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            // paramss.Add(new GeneratorParameter("Number", 0));

            Generator gen = new Generator(GENERATOR_BinaryStaticGenerator, (n, p) =>
            {
                return 0x10;
            }
                , null);
            return gen;
        }
    }
}
