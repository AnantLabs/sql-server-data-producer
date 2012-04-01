using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Generators
{
    public static class BooleanGenerators
    {

        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static BooleanGenerators()
        {
            Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(RandomBoolean);
            Generators.Add(EveryOtherTrueEveryOtherFalse);
        }

        public static string EveryOtherTrueEveryOtherFalse(int n, object param)
        {
            return (n % 2 == 0).ToString();
        }

        public static string RandomBoolean(int n, object param)
        {
            return (RandomSupplier.Instance.GetNextInt() % 2 == 0).ToString();
        }
    }
}
