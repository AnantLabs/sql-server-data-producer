using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

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


        public static string UpCounter(int n)
        {
            return new Decimal(n).ToString();
        }

        public static string DownCounter(int n)
        {
            return new Decimal(0-n).ToString();
        }

        public static string SmallRandomValues(int n)
        {
            return new Decimal(RandomSupplier.Instance.GetNextInt() % 500).ToString();
        }

    }
}
