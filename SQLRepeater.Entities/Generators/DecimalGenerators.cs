using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.Generators
{
    public class DecimalGenerator : GeneratorBase
    {

        private DecimalGenerator(string name, ValueCreatorDelegate generator, ObservableCollection<GeneratorParameter> genParams)
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
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            DecimalGenerator gen = new DecimalGenerator("Random Decimal", (n, p) =>
            {
                return new Decimal(RandomSupplier.Instance.GetNextInt());
            }
                , paramss);
            return gen;
        }

        private static DecimalGenerator CreateUpCounter()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            paramss.Add(new GeneratorParameter("MinValue", 0.0));
            paramss.Add(new GeneratorParameter("MaxValue", Decimal.MaxValue));
            //paramss.Add(new GeneratorParameter("NumDecimals", 2));
            paramss.Add(new GeneratorParameter("Step", 1.0));

            DecimalGenerator gen = new DecimalGenerator("Counting up", (n, p) =>
            {
                decimal maxValue = GetParameterByName<int>(p, "MaxValue");
                decimal minValue = GetParameterByName<int>(p, "MinValue");
                //int decimals = GetParameterByName<int>(p, "NumDecimals");
                decimal step = GetParameterByName<decimal>(p, "Step");

                return (minValue + (step * n)) % maxValue;
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
