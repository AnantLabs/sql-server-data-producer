using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SQLDataProducer.Entities.Generators.ForeignKeyGenerators
{
    public abstract class ForeignKeyGeneratorBase : AbstractValueGenerator
    {
        public ForeignKeyGeneratorBase(string generatorName, List<string> foreignKeys)
            : base(generatorName, false)
        {
            // add foreign keys to parameters
           GeneratorParameters.Add(new GeneratorParameter("Foreign keys"
                                    , foreignKeys
                                    , GeneratorParameterParser.ObjectParser
                                    , false));
        }

        protected override object ApplyGeneratorTypeSpecificLimits(object value)
        {
            return value;
        }
    }
}
