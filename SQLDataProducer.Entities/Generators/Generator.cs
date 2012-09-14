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
using System.Linq;
using System.ComponentModel;
using SQLDataProducer.Entities.Generators.Collections;
using System.Xml.Serialization;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator : INotifyPropertyChanged, IXmlSerializable
    {
        protected ValueCreatorDelegate ValueGenerator { get; set; }

        GeneratorParameterCollection _genParameters;
        public GeneratorParameterCollection GeneratorParameters
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

        protected Generator(string generatorName, ValueCreatorDelegate generator, GeneratorParameterCollection genParams)
        {
            ValueGenerator = generator;
            GeneratorParameters = genParams ?? new GeneratorParameterCollection();
            GeneratorName = generatorName;
        }

        public Generator()
        {
            GeneratorParameters = new GeneratorParameterCollection();
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

        protected static object GetParameterByName(GeneratorParameterCollection paramas, string name)
        {
            return paramas.Where(x => x.ParameterName == name).First().Value;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.General)]
        protected static Generator CreateQueryGenerator()
        {
            GeneratorParameterCollection paramss = new GeneratorParameterCollection();

            paramss.Add(new GeneratorParameter("Query", "select ..."));

            Generator gen = new Generator("Custom SQL Query", (n, p) =>
            {
                string value = GetParameterByName(p, "Query").ToString();

                return string.Format("({0})", value);
            }
                , paramss);
            return gen;
        }

        [GeneratorMetaData(Generators.GeneratorMetaDataAttribute.GeneratorType.General)]
        public static Generator CreateNULLValueGenerator()
        {
            Generator gen = new Generator("NULL value", (n, p) =>
            {
                return "NULL";
            }
                , null);
            return gen;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal Generator Clone()
        {
            return new Generator(this.GeneratorName, this.ValueGenerator, this.GeneratorParameters.Clone());
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.ReadToDescendant("Generator"))
            {
                this.GeneratorName = reader.GetAttribute("GeneratorName");
                this.GeneratorParameters.ReadXml(reader);
                reader.ReadEndElement();
            }

        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Generator");
            writer.WriteAttributeString("GeneratorName", this.GeneratorName);

            this.GeneratorParameters.WriteXml(writer);

            writer.WriteEndElement();
        }

        public void SetGeneratorParameters(GeneratorParameterCollection generatorParameterCollection)
        {
            this.GeneratorParameters = generatorParameterCollection.Clone();
        }
    }
}
