using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Generators
{
    public class TinyIntGenerators 
    {
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static TinyIntGenerators()
        {
            Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(UpCounter);
            Generators.Add(RandomSmallInt);
        }

        public static string UpCounter(int n, object param)
        {
            return (n % byte.MaxValue).ToString();
        }

        public static string RandomSmallInt(int n, object param)
        {
            return (RandomSupplier.Instance.GetNextInt() % byte.MaxValue).ToString(); 
        }
    }
}
