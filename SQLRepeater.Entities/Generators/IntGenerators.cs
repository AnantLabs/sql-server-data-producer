using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.Generators.Collections;

namespace SQLRepeater.Entities.Generators
{
    public class IntGenerator : GeneratorBase
    {

        private IntGenerator(string name, ValueCreatorDelegate generator, GeneratorParameterCollection genParams)
            : base(name, generator, genParams)
        {
        }

        public static ObservableCollection<GeneratorBase> GetGeneratorsForInt()
        {
            int maxValue = int.MaxValue;
            int minValue = int.MinValue;

            return CreateGeneratorsWithSettings(maxValue, minValue);
        }
        public static ObservableCollection<GeneratorBase> GetGeneratorsForSmallInt()
        {
            int maxValue = short.MaxValue;
            int minValue = 0;

            return CreateGeneratorsWithSettings(maxValue, minValue);
        }
        public static ObservableCollection<GeneratorBase> GetGeneratorsForBigInt()
        {
            long maxValue = int.MaxValue;
            long minValue = 0;

            return CreateGeneratorsWithSettings(maxValue, minValue);
        }
        public static ObservableCollection<GeneratorBase> GetGeneratorsForTinyInt()
        {
            long maxValue = byte.MaxValue;
            long minValue = byte.MinValue;

            return CreateGeneratorsWithSettings(maxValue, minValue);
        }
        public static ObservableCollection<GeneratorBase> GetGeneratorsForBit()
        {
            long maxValue = 1;
            long minValue = 0;

            return CreateGeneratorsWithSettings(maxValue, minValue);
        }
        


        private static ObservableCollection<GeneratorBase> CreateGeneratorsWithSettings(long maxValue, long minValue)
        {
            ObservableCollection<GeneratorBase> valueGenerators = new ObservableCollection<GeneratorBase>();
            valueGenerators.Add(CreateRandomGenerator(minValue, maxValue));
            valueGenerators.Add(CreateUpCounter(minValue, maxValue));
            valueGenerators.Add(CreateIdentityFromExecutionItem());
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(StaticNumber());

            return valueGenerators;
        }

        private static IntGenerator CreateRandomGenerator(long min, long max)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("MinValue", 1));
            paramss.Add(new GeneratorParameter("MaxValue", max));

            IntGenerator gen = new IntGenerator("Random Int", (n, p) =>
                {
                    long maxValue = long.Parse(GetParameterByName(p, "MaxValue").ToString());
                    long minValue = long.Parse(GetParameterByName(p, "MinValue").ToString());
                    return (minValue + RandomSupplier.Instance.GetNextInt()) % maxValue;
                }
                , paramss);
            return gen;
        }

        private static IntGenerator CreateUpCounter(long min, long max)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("MinValue", min));
            paramss.Add(new GeneratorParameter("MaxValue", max));

            IntGenerator gen = new IntGenerator("Counting up", (n, p) =>
            {
                long maxValue = long.Parse(GetParameterByName(p, "MaxValue").ToString());
                long minValue = long.Parse(GetParameterByName(p, "MinValue").ToString());
                return (n + minValue) % maxValue;
            }
                , paramss);
            return gen;
        }

        private static IntGenerator CreateIdentityFromExecutionItem()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Item Number#", 1));

            IntGenerator gen = new IntGenerator("Identity from previous item", (n, p) =>
            {
                long value = long.Parse(GetParameterByName(p, "Item Number#").ToString());

                return string.Format("@i{0}_identity", value);
            }
                , paramss);
            return gen;
        }

        private static IntGenerator StaticNumber()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Number", 0));

            IntGenerator gen = new IntGenerator("Static Number", (n, p) =>
            {
                long value = long.Parse(GetParameterByName(p, "Number").ToString());

                return value;
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
