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
        
        public static ObservableCollection<Generator> GetGeneratorsForInt()
        {
            int maxValue = int.MaxValue;
            int minValue = int.MinValue;

            return CreateIntGeneratorsWithSettings(maxValue, minValue);
        }
        public static ObservableCollection<Generator> GetGeneratorsForSmallInt()
        {
            int maxValue = short.MaxValue;
            int minValue = 0;

            return CreateIntGeneratorsWithSettings(maxValue, minValue);
        }
        public static ObservableCollection<Generator> GetGeneratorsForBigInt()
        {
            long maxValue = int.MaxValue;
            long minValue = 0;

            return CreateIntGeneratorsWithSettings(maxValue, minValue);
        }
        public static ObservableCollection<Generator> GetGeneratorsForTinyInt()
        {
            long maxValue = byte.MaxValue;
            long minValue = byte.MinValue;

            return CreateIntGeneratorsWithSettings(maxValue, minValue);
        }
        public static ObservableCollection<Generator> GetGeneratorsForBit()
        {
            long maxValue = 1;
            long minValue = 0;

            return CreateIntGeneratorsWithSettings(maxValue, minValue);
        }



        private static ObservableCollection<Generator> CreateIntGeneratorsWithSettings(long maxValue, long minValue)
        {
            ObservableCollection<Generator> valueGenerators = new ObservableCollection<Generator>();
            valueGenerators.Add(CreateRandomIntGenerator(minValue, maxValue));
            valueGenerators.Add(CreateIntUpCounter(minValue, maxValue));
            valueGenerators.Add(CreateIdentityFromExecutionItem());
            valueGenerators.Add(CreateQueryGenerator());
            valueGenerators.Add(CreateStaticNumberGenerator());

            return valueGenerators;
        }

        public static Generator CreateRandomForeignKeyGenerator(ObservableCollection<int> fkkeys)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            GeneratorParameter foreignParam = new GeneratorParameter("Keys", fkkeys);
            foreignParam.IsWriteEnabled = false;
            paramss.Add(foreignParam);
            Generator gen = new Generator("Random FOREIGN KEY Value (EAGER)", (n, p) =>
            {
                ObservableCollection<int> keys = (ObservableCollection<int>)GetParameterByName(p, "Keys");
                return keys[RandomSupplier.Instance.GetNextInt() % keys.Count];
            }
                , paramss);
            return gen;
        }
        public static Generator CreateSequentialForeignKeyGenerator(ObservableCollection<int> fkkeys)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            GeneratorParameter foreignParam = new GeneratorParameter("Keys", fkkeys);
            foreignParam.IsWriteEnabled = false;
            GeneratorParameter startIndex = new GeneratorParameter("Start Index", 1);
            GeneratorParameter maxIndex = new GeneratorParameter("Max Index", 1000);
            
            paramss.Add(foreignParam);
            paramss.Add(startIndex);
            paramss.Add(maxIndex);

            Generator gen = new Generator("Sequential FOREIGN KEY Value (EAGER)", (n, p) =>
            {
                ObservableCollection<int> keys = (ObservableCollection<int>)GetParameterByName(p, "Keys");
                int si = int.Parse(GetParameterByName(p, "Start Index").ToString());
                int mi = int.Parse(GetParameterByName(p, "Max Index").ToString());
                if (mi > fkkeys.Count)
                {
                    mi = fkkeys.Count;
                }
                return keys[n % keys.Count];
            }
                , paramss);
            return gen;
        }

        // Todo: Cannot implemtent this as the generator does not know anything about the FK
        //public static GeneratorBase CreateSequentialForeignKeyGeneratorLazy()
        //{
        //    GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            
        //    GeneratorBase gen = new GeneratorBase("Random FOREIGN KEY Value (LAZY)", (n, p) =>
        //    {
        //        return keys[n % keys.Count];
        //    }
        //        , paramss);
        //    return gen;
        //}

        private static Generator CreateRandomIntGenerator(long min, long max)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("MinValue", 1));
            paramss.Add(new GeneratorParameter("MaxValue", max));

            Generator gen = new Generator("Random Int", (n, p) =>
                {
                    long maxValue = long.Parse(GetParameterByName(p, "MaxValue").ToString());
                    long minValue = long.Parse(GetParameterByName(p, "MinValue").ToString());
                   
                    return (RandomSupplier.Instance.GetNextInt() % maxValue) + minValue;;
                }
                , paramss);
            return gen;
        }

        private static Generator CreateIntUpCounter(long min, long max)
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("MinValue", min));
            paramss.Add(new GeneratorParameter("MaxValue", max));

            Generator gen = new Generator("Counting up", (n, p) =>
            {
                long maxValue = long.Parse(GetParameterByName(p, "MaxValue").ToString());
                long minValue = long.Parse(GetParameterByName(p, "MinValue").ToString());
                return (n + minValue) % maxValue;
            }
                , paramss);
            return gen;
        }

        private static Generator CreateIdentityFromExecutionItem()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();
            paramss.Add(new GeneratorParameter("Item Number#", 1));

            Generator gen = new Generator("Identity from previous item", (n, p) =>
            {
                long value = long.Parse(GetParameterByName(p, "Item Number#").ToString());

                return string.Format("@i{0}_identity", value);
            }
                , paramss);
            return gen;
        }

        private static Generator CreateStaticNumberGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Number", 0));

            Generator gen = new Generator("Static Number", (n, p) =>
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
