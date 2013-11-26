using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.Generators.DecimalGenerators
{
    public abstract class DecimalGeneratorBase : AbstractValueGenerator<Decimal>
    {
        public DecimalGeneratorBase(string generatorName)
            : base(generatorName)
        {
            GeneratorParameters.Add(new GeneratorParameter("MinValue", 0.0, GeneratorParameterParser.DecimalParser));
            GeneratorParameters.Add(new GeneratorParameter("MaxValue", decimal.MaxValue, GeneratorParameterParser.DecimalParser));
        }

        protected override decimal ApplyTypeSpecificLimits(decimal value)
        {
            return Math.Min(value, decimal.MaxValue);
        }
    }
}
