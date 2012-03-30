using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
