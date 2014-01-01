// Copyright 2012-2014 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace SQLDataProducer.Entities.Generators
{
    public class GeneratorParameter : EntityBase, IEquatable<GeneratorParameter>
    {

        

        string _paramName;
        [System.ComponentModel.ReadOnly(true)]
        public string ParameterName
        {
            get
            {
                return _paramName;
            }
            private set
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
                    // Parse the incomming value to the requested value type
                    try
                    {
                        _value = ValueParser.ParseValue(value);
                    }
                    catch (Exception)
                    { 
                        /* If the user inputs the wrong format */
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(_value.ToString()))
                    {
                        // If value is empty then resort to default value.
                        _value = DefaultValue;
                    }
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

        /// <summary>
        /// Provided function to parse the value set by the user to the requested datatype
        /// </summary>
        GeneratorParameterParser ValueParser = GeneratorParameterParser.ObjectParser;

        public GeneratorParameter(string name, object value, GeneratorParameterParser parser, bool isWriteEnabled = true)
        {
            if (parser == null)
                throw new ArgumentNullException("parser", "parser cannot be null");

            ValueParser = parser;
            ParameterName = name;
            DefaultValue = parser.ParseValue(value);
            Value = parser.ParseValue(value);
            IsWriteEnabled = isWriteEnabled;
        }


        public override string ToString()
        {
            return string.Format("ParameterName = {0}, ValueParser = {1}, IsWriteEnabled = {2}, Value = {3}", ParameterName, ValueParser.ParserName, IsWriteEnabled, Value);
        }

        public override bool Equals(System.Object obj)
        {
            GeneratorParameter p = obj as GeneratorParameter;
            if ((object)p == null)
                return false;

            return GetHashCode().Equals(p.GetHashCode());
        }

        public bool Equals(GeneratorParameter other)
        {
            if (other == null)
                return false;
            return
                this.ParameterName == other.ParameterName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 37;
                hash = hash * 23 + ParameterName.GetHashCode();
                return hash;
            }
        }

        internal GeneratorParameter Clone()
        {
            // TODO: Clone using the same entity as the LOAD/SAVE functionality
            var para = new GeneratorParameter(this.ParameterName, this.ValueParser.FormatToString(this.Value), this.ValueParser);
            para.IsWriteEnabled = this.IsWriteEnabled;
            para.DefaultValue = this.DefaultValue;
            return para;
        }
    }
}
