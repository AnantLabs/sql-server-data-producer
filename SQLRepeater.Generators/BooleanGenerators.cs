using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Generators
{
    public class BooleanGenerator : GeneratorBase
    {

        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static BooleanGenerator()
        {
            Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(RandomBoolean);
            Generators.Add(EveryOtherTrueEveryOtherFalse);
        }

        public static object EveryOtherTrueEveryOtherFalse(int n, object param)
        {
            return Wrap(n % 2 == 0);
        }

        public static object RandomBoolean(int n, object param)
        {
            return Wrap(RandomSupplier.Instance.GetNextInt() % 2 == 0);
        }
    }
}
