using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.Generators
{
    public class GeneratorBase: IValueCreator, INotifyPropertyChanged 
    {

        protected ValueCreatorDelegate ValueGenerator { get; set; }
       
        ObservableCollection<GeneratorParameter> _genParameters;
        public ObservableCollection<GeneratorParameter> GeneratorParameters
        {
            get
            {
                return _genParameters;
            }
            set
            {
                if (_genParameters != value)
                {
                    _genParameters = value;
                    OnPropertyChanged("GeneratorParameters");
                }
            }
        }


        string _generatorName;
        private string GeneratorName
        {
            get
            {
                return _generatorName;
            }
            set
            {
                if (_generatorName != value)
                {
                    _generatorName = value;
                    OnPropertyChanged("GeneratorName");
                }
            }
        }

        protected GeneratorBase(string generatorName, ValueCreatorDelegate generator, ObservableCollection<GeneratorParameter> genParams)
        {
            ValueGenerator = generator;
            GeneratorParameters = genParams;
            GeneratorName = generatorName;
        }
        
        public override string ToString()
        {
            return GeneratorName;
        }

        public object GenerateValue(int n)
        {
            return ValueGenerator(n, GeneratorParameters);
        }

        /// <summary>
        /// For wrapping values in single quoutation marks (to make sql server happy)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected static string Wrap(object s)
        {
            return string.Format("'{0}'", s);
        }

        /// <summary>
        /// Helper function to retrieve parameter value from the collection
        /// </summary>
        /// <typeparam name="TRes">The type of value that it should return</typeparam>
        /// <param name="parms">The observerablecollection containing the parmaeters</param>
        /// <param name="name">the name of the parameter to get the value for</param>
        /// <returns></returns>
        protected static TRes GetParameterByName<TRes>(ObservableCollection<GeneratorParameter> parms, string name)
        {
            return (TRes)parms.Where(x => x.ParameterName == name).Select(x => x.Value).FirstOrDefault();
        }


        public IValueCreator Clone()
        {
            return new GeneratorBase(GeneratorName, ValueGenerator, GeneratorParameters);
        }




        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
