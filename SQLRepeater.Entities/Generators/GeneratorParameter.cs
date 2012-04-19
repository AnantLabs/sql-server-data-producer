using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRepeater.Entities.Generators
{
    public class GeneratorParameter : EntityBase
    {
        string _paramName;
        [System.ComponentModel.ReadOnly(true)]
        public string ParameterName
        {
            get
            {
                return _paramName;
            }
            set
            {
                if (_paramName != value)
                {
                    _paramName = value;
                    OnPropertyChanged("ParameterName");
                }
            }
        }


        object _value;
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public GeneratorParameter(string name, object value)
        {
            ParameterName = name;
            Value = value;
        }

    }
}
