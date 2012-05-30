using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.Generators.Collections;

namespace SQLRepeater.Entities.Generators
{
    public class DecimalGenerator : GeneratorBase
    {

        private DecimalGenerator(string name, ValueCreatorDelegate generator, GeneratorParameterCollection genParams)
            : base(name, generator, genParams)
        {
        }

        internal static ObservableCollection<GeneratorBase> GetGenerators()
        {
            ObservableCollection<GeneratorBase> valueGenerators = new ObservableCollection<GeneratorBase>();
            valueGenerators.Add(CreateUpCounter());
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(CreateRandomGenerator());

            return valueGenerators;
        }

        private static DecimalGenerator CreateRandomGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("MinValue", 0.0));
            paramss.Add(new GeneratorParameter("MaxValue", 10000000));

            DecimalGenerator gen = new DecimalGenerator("Random Decimal", (n, p) =>
            {
                double maxValue = double.Parse(GetParameterByName(p, "MaxValue").ToString());
                double minValue = double.Parse(GetParameterByName(p, "MinValue").ToString());

                return (RandomSupplier.Instance.GetNextDouble() % maxValue + minValue).ToString().Replace(",", ".");
            }
                , paramss);
            return gen;
        }

        private static DecimalGenerator CreateUpCounter()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("MinValue", 0.0));
            paramss.Add(new GeneratorParameter("MaxValue", 10000000));
            paramss.Add(new GeneratorParameter("Step", 1.0));

            DecimalGenerator gen = new DecimalGenerator("Counting up", (n, p) =>
            {
                double maxValue = double.Parse(GetParameterByName(p, "MaxValue").ToString());
                double minValue = double.Parse(GetParameterByName(p, "MinValue").ToString());
                double step = double.Parse(GetParameterByName(p, "Step").ToString());

                return ((minValue + (step * n)) % maxValue).ToString().Replace(",", ".");
            }
                , paramss);
            return gen;
        }

        //public static object DownCounter(int n, object param)
        //{
        //    return Wrap(new Decimal(0 - n));
        //}


        
    }
}
