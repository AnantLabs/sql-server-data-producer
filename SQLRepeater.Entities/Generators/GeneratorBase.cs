using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.Generators
{
    public class GeneratorBase: INotifyPropertyChanged 
    {

        protected ValueCreatorDelegate ValueGenerator { get; set; }
       
        ObservableCollection<GeneratorParameter> _genParameters;
        public ObservableCollection<GeneratorParameter> GeneratorParameters
        {
            get
            {
                return _genParameters;
            }
            private set
            {
                if (_genParameters != value)
                {
                    _genParameters = value;
                    OnPropertyChanged("GeneratorParameters");
                }
            }
        }


        string _generatorName;
        public string GeneratorName
        {
            get
            {
                return _generatorName;
            }
            private set
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

        protected static object GetParameterByName(ObservableCollection<GeneratorParameter> paramas, string name)
        {
            return paramas.Where(x => x.ParameterName == name).First().Value;
        }


        protected static GeneratorBase CreateQueryGenerator()
        {
            ObservableCollection<GeneratorParameter> paramss = new ObservableCollection<GeneratorParameter>();

            paramss.Add(new GeneratorParameter("Query", "select ..."));

            GeneratorBase gen = new GeneratorBase("Custom SQL Query", (n, p) =>
            {
                string value = GetParameterByName(p, "Query").ToString();

                return string.Format("({0})", value);
            }
                , paramss);
            return gen;
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
