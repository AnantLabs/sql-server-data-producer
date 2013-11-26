using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.Generators.DecimalGenerators
{
    public class CountingUpDecimalGenerator : DecimalGeneratorBase
    {
        public CountingUpDecimalGenerator()
            : base("CountingUpDecimalGenerator")
        {
            GeneratorParameters.Add(new GeneratorParameter("StartValue", 0.0, GeneratorParameterParser.DecimalParser));
            GeneratorParameters.Add(new GeneratorParameter("Step", 1.0, GeneratorParameterParser.DecimalParser));
        }

        protected override object InternalGenerateValue(long n, Collections.GeneratorParameterCollection paramas)
        {
            decimal step = (decimal)(double)paramas.GetParameterByName("Step");
            decimal startValue = (decimal)(double)paramas.GetParameterByName("StartValue");

            return startValue + (step * (n - 1));
        }
    }
}
