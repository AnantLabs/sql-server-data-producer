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

        public static string UpCounter(int n, object param)
        {
            return Wrap(n);
        }

        public static string DownCounter(int n, object param)
        {
            return Wrap(0-n);
        }

        public static string RandomInt(int n, object param)
        {
            return Wrap(RandomSupplier.Instance.GetNextInt());
        }
        
        public static string StaticNumber(int n, object param)
        {
            IntParameter p = ObjectToIntParameter(param);
            return Wrap(p.SpecifiedValue);
        }

        public static string IdentityFromExecutionItem(int n, object param)
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
