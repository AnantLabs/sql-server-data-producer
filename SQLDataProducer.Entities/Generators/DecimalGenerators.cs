using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities.Generators.Collections;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator
    {

        //private GeneratorBase(string name, ValueCreatorDelegate generator, GeneratorParameterCollection genParams)
        //    : base(name, generator, genParams)
        //{
        //}
        
        internal static ObservableCollection<Generator> GetDecimalGenerators()
        {
            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
            valueGenerators.Add(CreateDecimalUpCounter());
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(CreateRandomDecimalGenerator());

            return valueGenerators;
        }

        private static Generator CreateRandomDecimalGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("MinValue", 0.0));
            paramss.Add(new GeneratorParameter("MaxValue", 10000000.0));

            Generator gen = new Generator("Random Decimal", (n, p) =>
            {
                double maxValue = double.Parse(GetParameterByName(p, "MaxValue").ToString());
                double minValue = double.Parse(GetParameterByName(p, "MinValue").ToString());

                return (((RandomSupplier.Instance.GetNextDouble() * double.MaxValue) % maxValue) + minValue).ToString().Replace(",", ".");
            }
                , paramss);
            return gen;
        }

        private static Generator CreateDecimalUpCounter()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("MinValue", 0.0));
            paramss.Add(new GeneratorParameter("MaxValue", 10000000.0));
            paramss.Add(new GeneratorParameter("Step", 1.0));

            Generator gen = new Generator("Counting up", (n, p) =>
            {
                double maxValue = double.Parse(GetParameterByName(p, "MaxValue").ToString());
                double minValue = double.Parse(GetParameterByName(p, "MinValue").ToString());
                double step = double.Parse(GetParameterByName(p, "Step").ToString());

                return ((minValue + (step * (n - 1))) % maxValue).ToString().Replace(",", ".");
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
