using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.IsWriteEnabled = bool.Parse(reader.GetAttribute("IsWriteEnabled"));
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
