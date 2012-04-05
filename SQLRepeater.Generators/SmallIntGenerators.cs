using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities.ValueGeneratorParameters;

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
            Generators.Add(StaticNumber);
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

        public static string StaticNumber(int n, object param)
        {
            IntParameter p = ObjectToIntParameter(param);
            return p.MinValue.ToString();
        }

        private static IntParameter ObjectToIntParameter(object param)
        {
            if (!(param is IntParameter))
                throw new ArgumentException("The supplied Parameter is not IntParameter");

            return param as IntParameter;
        }
    }
}
