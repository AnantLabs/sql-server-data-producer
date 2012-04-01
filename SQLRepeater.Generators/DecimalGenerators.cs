using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.Entities.ValueGeneratorParameters;

namespace SQLRepeater.Generators
{
    public class DecimalGenerators 
    {
        public static ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static DecimalGenerators()
        {
            Generators = new ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(UpCounter);
            Generators.Add(DownCounter);
            Generators.Add(SmallRandomValues);
        }


        public static string UpCounter(int n, object param)
        {
            DecimalParameter p = ObjectToDecimalParameter(param);
            // we can now use parameter while generating the values
            return new Decimal(n).ToString();
        }

        private static DecimalParameter ObjectToDecimalParameter(object param)
        {
            if (!(param is DecimalParameter))
                throw new ArgumentException("The supplied Parameter is not DecimalParameter");

            return param as DecimalParameter;
        }

        public static string DownCounter(int n, object param)
        {
            return new Decimal(0-n).ToString();
        }

        public static string SmallRandomValues(int n, object param)
        {
            return new Decimal(RandomSupplier.Instance.GetNextInt() % 500).ToString();
        }

    }
}
