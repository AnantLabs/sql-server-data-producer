using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.Generators
{
    public class IntGenerator : GeneratorBase
    {

        private IntGenerator(string name, ValueCreatorDelegate generator, ObservableCollection<GeneratorParameter> genParams)
            : base(name, generator, genParams)
        {
        }

        static ObservableCollection<GeneratorBase> _valueGenerators;
        public static ObservableCollection<GeneratorBase> ValueGenerators
        {
            get
            {
                return _valueGenerators;
            }
            set
            {
                _valueGenerators = value;
            }
        }

        static IntGenerator()
        {
            ValueGenerators = new ObservableCollection<GeneratorBase>();
            ValueGenerators.Add(CreateRandomGenerator());
        }

        private static IntGenerator CreateRandomGenerator()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            paramss.Add(new GeneratorParameter("MinValue", int.MinValue));
            paramss.Add(new GeneratorParameter("MaxValue", int.MaxValue));

            IntGenerator gen = new IntGenerator("Random Int", (n, p) =>
                {
                    int maxValue = GetParameterByName<int>("MaxValue");
                    return RandomSupplier.Instance.GetNextInt() % maxValue;
                }
                , paramss);
            return gen;
        }



        //public static object UpCounter(int n, object param)
        //{

        //    return n;
        //}

        //public static object DownCounter(int n, object param)
        //{
        //    return 0 - n;
        //}

        //private static object RandomInt(int n, ObservableCollection<GeneratorParameter> paramas)
        //{
        //    return RandomSupplier.Instance.GetNextInt();
        //}

        //public static object StaticNumber(int n, object param)
        //{
        //    IntParameter p = ObjectToIntParameter(param);
        //    return p.SpecifiedValue;
        //}

        //public static object IdentityFromExecutionItem(int n, object param)
        //{
        //    IntParameter p = ObjectToIntParameter(param);
        //    // do not wrap
        //    return string.Format("@i{0}_identity", p.SpecifiedValue);
        //}


        //private static IntParameter ObjectToIntParameter(object param)
        //{
        //    if (!(param is IntParameter))
        //        throw new ArgumentException("The supplied Parameter is not IntParameter");

        //    return param as IntParameter;
        //}





        internal static GeneratorBase DefaultGenerator()
        {
            return CreateRandomGenerator();
        }
    }
}
