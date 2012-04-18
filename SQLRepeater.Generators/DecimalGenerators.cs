using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.ValueGeneratorParameters;

namespace SQLRepeater.Generators
{
    public class DecimalGenerator : GeneratorBase
    {
        public static ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static DecimalGenerator()
        {
            Generators = new ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(UpCounter);
            Generators.Add(DownCounter);
            Generators.Add(SmallRandomValues);
        }


        public static object UpCounter(int n, object param)
        {
            DecimalParameter p = ObjectToDecimalParameter(param);
            // we can now use parameter while generating the values
            return Wrap(new Decimal(n));
        }

        private static DecimalParameter ObjectToDecimalParameter(object param)
        {
            if (!(param is DecimalParameter))
                throw new ArgumentException("The supplied Parameter is not DecimalParameter");

            return param as DecimalParameter;
        }

        public static object DownCounter(int n, object param)
        {
            return Wrap(new Decimal(0 - n));
        }

        public static object SmallRandomValues(int n, object param)
        {
            return Wrap(new Decimal(RandomSupplier.Instance.GetNextInt() % 500));
        }

    }
}
