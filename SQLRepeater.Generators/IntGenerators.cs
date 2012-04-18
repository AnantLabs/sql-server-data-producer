using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities.ValueGeneratorParameters;

namespace SQLRepeater.Generators
{
    public class IntGenerators : GeneratorBase
    {
        public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        static IntGenerators()
        {
            Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
            Generators.Add(UpCounter);
            Generators.Add(DownCounter);
            Generators.Add(RandomInt);
            Generators.Add(StaticNumber);
            Generators.Add(IdentityFromExecutionItem);
        }

        public static object UpCounter(int n, object param)
        {
            return n;
        }

        public static object DownCounter(int n, object param)
        {
            return 0-n;
        }

        public static object RandomInt(int n, object param)
        {
            return RandomSupplier.Instance.GetNextInt();
        }
        
        public static object StaticNumber(int n, object param)
        {
            IntParameter p = ObjectToIntParameter(param);
            return p.SpecifiedValue;
        }

        public static object IdentityFromExecutionItem(int n, object param)
        {
            IntParameter p = ObjectToIntParameter(param);
            // do not wrap
            return string.Format("@i{0}_identity", p.SpecifiedValue);
        }


        private static IntParameter ObjectToIntParameter(object param)
        {
            if (!(param is IntParameter))
                throw new ArgumentException("The supplied Parameter is not IntParameter");

            return param as IntParameter;
        }
    }
}
