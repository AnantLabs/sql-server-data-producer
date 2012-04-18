using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Generators
{
    public class TinyIntGenerator : GeneratorBase
    {
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static TinyIntGenerator()
        {
            Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(UpCounter);
            Generators.Add(RandomSmallInt);
        }

        public static object UpCounter(int n, object param)
        {
            return (n % byte.MaxValue);
        }

        public static object RandomSmallInt(int n, object param)
        {
            return (RandomSupplier.Instance.GetNextInt() % byte.MaxValue); 
        }
    }
}
