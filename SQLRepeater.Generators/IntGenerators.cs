using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Generators
{
    public class IntGenerators 
    {
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static IntGenerators()
        {
            Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(UpCounter);
            Generators.Add(DownCounter);
            Generators.Add(RandomInt);
        }

        public static string UpCounter(int n, object param)
        {
            return n.ToString();
        }

        public static string DownCounter(int n, object param)
        {
         return (1-n).ToString();
        }

        public static string RandomInt(int n, object param)
        {
            return RandomSupplier.Instance.GetNextInt().ToString(); 
        }
    }
}
