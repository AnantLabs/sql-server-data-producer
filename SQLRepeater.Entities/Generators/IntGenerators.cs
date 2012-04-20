using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.Generators
{
    public class IntGenerator : GeneratorBase
    {

        private IntGenerator(string name, ValueCreatorDelegate generator, ObservableCollection<GeneratorParameter> genParams)
            : base(name, generator, genParams)
        {
        }

        public static ObservableCollection<GeneratorBase> GetGenerators()
        {
            ObservableCollection<GeneratorBase> valueGenerators = new ObservableCollection<GeneratorBase>();
            valueGenerators.Add(CreateRandomGenerator());
            valueGenerators.Add(CreateUpCounter());
            valueGenerators.Add(CreateIdentityFromExecutionItem());
            valueGenerators.Add(Query());
            valueGenerators.Add(StaticNumber());

            return valueGenerators;
        }   

        private static IntGenerator CreateRandomGenerator()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            paramss.Add(new GeneratorParameter("MinValue", 0));
            paramss.Add(new GeneratorParameter("MaxValue", int.MaxValue));

            IntGenerator gen = new IntGenerator("Random Int", (n, p) =>
                {
                    int maxValue = GetParameterByName<int>(p, "MaxValue");
                    int minValue = GetParameterByName<int>(p, "MinValue");
                    return (minValue + RandomSupplier.Instance.GetNextInt()) % maxValue;
                }
                , paramss);
            return gen;
        }

        private static IntGenerator CreateUpCounter()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            paramss.Add(new GeneratorParameter("MinValue", 0));
            paramss.Add(new GeneratorParameter("MaxValue", int.MaxValue));

            IntGenerator gen = new IntGenerator("Counting up", (n, p) =>
            {
                int maxValue = GetParameterByName<int>(p, "MaxValue");
                int minValue = GetParameterByName<int>(p, "MinValue");
                return (n + minValue) % maxValue;
            }
                , paramss);
            return gen;
        }

        private static IntGenerator CreateIdentityFromExecutionItem()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();
            paramss.Add(new GeneratorParameter("Item Number#", 1));

            IntGenerator gen = new IntGenerator("Identity from previous item", (n, p) =>
            {
                int value = GetParameterByName<int>(p, "Item Number#");

                return string.Format("@i{0}_identity", value);
            }
                , paramss);
            return gen;
        }

        private static IntGenerator StaticNumber()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            paramss.Add(new GeneratorParameter("Number", 0));

            IntGenerator gen = new IntGenerator("Static Number", (n, p) =>
            {
                int value = GetParameterByName<int>(p, "Number");

                return value;
            }
                , paramss);
            return gen;
        }

        private static IntGenerator Query()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            paramss.Add(new GeneratorParameter("Query", 0));

            IntGenerator gen = new IntGenerator("Custom SQL Query", (n, p) =>
            {
                string value = GetParameterByName<string>(p, "Query");

                return string.Format("({0})",value);
            }
                , paramss);
            return gen;
        }

        //public static object DownCounter(int n, object param)
        //{
        //    return 0 - n;
        //}
    }
}
