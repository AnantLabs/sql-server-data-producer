using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.Generators.ForeignKeyGenerators
{
   public class RandomForeignKeyGenerator : ForeignKeyGeneratorBase
    {
       public static readonly string GENERATOR_NAME = "RandomForeignKeyGenerator";

        public RandomForeignKeyGenerator(List<string> foreignKeys)
            : base(GENERATOR_NAME, foreignKeys)
        {

        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            var foreignKeys = paramas.GetValueOf<List<string>>("Foreign keys");
            int index = (int)((n % foreignKeys.Count) % int.MaxValue);
            return foreignKeys[index];
        }
    }
}
