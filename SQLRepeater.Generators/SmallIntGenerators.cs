using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Generators
{
    public class SmallIntGenerators 
    {
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static SmallIntGenerators()
        {
            Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(UpCounter);
            Generators.Add(DownCounter);
            Generators.Add(RandomSmallInt);
        }

        public static string UpCounter(int n, object genParameter)
        {
            return (n % short.MaxValue).ToString();
        }

        public static string DownCounter(int n, object genParameter)
        {
            return (1-(n % short.MaxValue)).ToString();
        }

        public static string RandomSmallInt(int n, object genParameter)
        {
            return (RandomSupplier.Instance.GetNextInt() % short.MaxValue).ToString(); 
        }
    }
}
