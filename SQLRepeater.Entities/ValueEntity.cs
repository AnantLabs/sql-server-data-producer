using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRepeater.Entities.ValueGeneratorParameters;

namespace SQLRepeater.Entities
{
    public partial class ValueEntity : EntityBase
    {


        ValueCreatorDelegate _valueGenerator;
        public ValueCreatorDelegate ValueGenerator
        {
            get
            {
                return _valueGenerator;
            }
            set
            {
                if (_valueGenerator != value)
                {
                    _valueGenerator = value;
                    OnPropertyChanged("ValueGenerator");
                }
            }
        }

        object[] _valueGeneratorParameter;
        public object[] ValueGeneratorParameter
        {
            get
            {
                return _valueGeneratorParameter;
            }
            set
            {
                if (_valueGeneratorParameter != value)
                {
                    _valueGeneratorParameter = value;
                    OnPropertyChanged("ValueGeneratorParameter");
                }
            }
        }

        

        string _value = string.Empty;
        public string Value
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


        public override string ToString()
        {
            return _value;
        }
    }
}
