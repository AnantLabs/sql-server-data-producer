// Copyright 2012 Peter Henell

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

namespace SQLDataProducer.Entities.Generators
{
    public class GeneratorParameter : EntityBase, IXmlSerializable
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

        //private Type ParamType { get; set; }



        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            this.ParameterName = reader.GetAttribute("ParameterName");
            this.Value = reader.GetAttribute("Value");
            this.IsWriteEnabled = reader.TryGetBoolAttribute("IsWriteEnabled", true);
            //reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("GeneratorParameter");
            writer.WriteAttributeString("ParameterName", this.ParameterName);
            writer.WriteAttributeString("Value", this.Value.ToString());
            writer.WriteAttributeString("IsWriteEnabled", this.IsWriteEnabled.ToString());
            writer.WriteEndElement();
        }
    }
}
