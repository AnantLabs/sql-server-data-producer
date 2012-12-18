﻿// Copyright 2012 Peter Henell

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
    public class GeneratorParameter : EntityBase , IEquatable<GeneratorParameter>
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
                    _value = value;
                    if (string.IsNullOrEmpty(_value.ToString()) )
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

        public GeneratorParameter(string name, object value, bool isWriteEnabled = true)
        {
            ParameterName = name;
            DefaultValue = value;
            Value = value;
            IsWriteEnabled = isWriteEnabled;
        }
        public GeneratorParameter()
        {

        }


        public void ReadXml(XElement xe)
        {
            this.ParameterName = xe.Attribute("ParameterName").Value;
         // TODO: how to handle datatypes?
            this.Value = xe.Attribute("Value").Value;
            this.IsWriteEnabled = bool.Parse(xe.Attribute("IsWriteEnabled").Value);
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("GeneratorParameter");
            writer.WriteAttributeString("ParameterName", this.ParameterName);
            writer.WriteAttributeString("Value", this.Value.ToString());
            writer.WriteAttributeString("IsWriteEnabled", this.IsWriteEnabled.ToString());
            writer.WriteEndElement();
        }


        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be casted return false:
            GeneratorParameter p = obj as GeneratorParameter;
            if ((object)p == null)
                return false;

            // Return true if the fields match:
            return GetHashCode() == p.GetHashCode();
        }

        public bool Equals(GeneratorParameter other)
        {
            return
                this.IsWriteEnabled == other.IsWriteEnabled &&
                this.ParameterName == other.ParameterName;
        }

        public override int GetHashCode()
        {
            return
                this.IsWriteEnabled.GetHashCode() ^
                this.ParameterName.GetHashCode();
        }

        internal GeneratorParameter Clone()
        {
            var para = new GeneratorParameter(this.ParameterName, this.Value);
            para.IsWriteEnabled = this.IsWriteEnabled;
            para.DefaultValue = this.DefaultValue;

            return para;
        }
    }
}
