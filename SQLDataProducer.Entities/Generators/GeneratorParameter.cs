using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataProducer.Entities.Generators
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

        bool _isWriteEnabled = true;
        public bool IsWriteEnabled
        {
            get
            {
                return _isWriteEnabled;
            }
            internal set
            {
                if (_isWriteEnabled != value)
                {
                    _isWriteEnabled = value;
                    OnPropertyChanged("IsWriteEnabled");
                }
            }
        }
        
        object _defaultValue;
        /// <summary>
        /// Gets the default value for the parameter
        /// </summary>
        public object DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            private set
            {
                if (_defaultValue != value)
                {
                    _defaultValue = value;
                    OnPropertyChanged("DefaultValue");
                }
            }
        }

        public GeneratorParameter(string name, object value)
        {
            ParameterName = name;
            Value = value;
        }


        //private Type ParamType { get; set; }

      
    }
}
