// Copyright 2012-2013 Peter Henell

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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.Entities.Generators
{
    public partial class Generator : INotifyPropertyChanged, IEquatable<Generator>
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
                    if(string.IsNullOrEmpty(GeneratorHelpText))
                        GeneratorHelpText = GetGeneratorHelpText(_generatorName);
                    OnPropertyChanged("GeneratorName");
                }
            }
        }
        private string _generatorHelpText;
        public string GeneratorHelpText
        {
            get
            {
                return _generatorHelpText;
            }
            set
            {
                _generatorHelpText = value;
                OnPropertyChanged("GeneratorHelpText");
            }
        }


        bool _isSqlQueryGenerator = false;
        // Gets whether this generator is generating pure sql queries
        public bool IsSqlQueryGenerator
        {
            get
            {
                return _isSqlQueryGenerator;
            }
        }



        /// <summary>
        /// Column where this generator is attached.
        /// </summary>
        ColumnEntity _parentColumn;
        public ColumnEntity ParentColumn
        {
            get
            {
                return _parentColumn;
            }
            set
            {
                if (_parentColumn != value)
                {
                    _parentColumn = value;
                    OnPropertyChanged("ParentColumn");
                }
            }
        }

        public Generator(string generatorName, ValueCreatorDelegate generator, GeneratorParameterCollection genParams, bool isSqlQueryGenerator = false)
        {
            ValueGenerator = generator;
            GeneratorParameters = genParams ?? new GeneratorParameterCollection();
            GeneratorName = generatorName;
            GeneratorHelpText = GetGeneratorHelpText(generatorName);
            _isSqlQueryGenerator = isSqlQueryGenerator;
        }

        public Generator()
        {
            GeneratorParameters = new GeneratorParameterCollection();
        }

        // cache to hold the generator texts to avoid reading the xml file multiple times.
        private static Dictionary<string, string> generatorHelpTexts;

        private static string GetGeneratorHelpText(string generatorName)
        {
            if (generatorHelpTexts == null)
                generatorHelpTexts = LoadGeneratorHelpTexts();
            
            // If the generator name was not found in the dictionary just return empty string.
            string ret = String.Empty;
            if (generatorHelpTexts.ContainsKey(generatorName))
                ret = generatorHelpTexts[generatorName];

            return ret;
        }

        private static Dictionary<string, string> LoadGeneratorHelpTexts()
        {
            var dic = new Dictionary<string, string>();
            
            try
            {
                string helpTextFile = @".\Generators\resources\GeneratorHelpTexts.xml";

                XDocument doc = XDocument.Load(helpTextFile);
                var texts = from en in doc.Descendants("Text")
                            select new
                            {
                                GenName = en.Attribute("generatorName").Value,
                                Text = en.Value
                            };

                foreach (var kv in texts)
                    dic.Add(kv.GenName, kv.Text);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return dic;
        }

        public override string ToString()
        {
            return GeneratorName;
        }

        internal object GenerateValue(long n)
        {
            return ValueGenerator(n, GeneratorParameters);
        }


        internal static object GetParameterByName(GeneratorParameterCollection paramas, string name)
        {
            return paramas.Where(x => x.ParameterName == name).First().Value;
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



        public void ReadXml(XElement xe)
        {
            this.GeneratorName = xe.Attribute("GeneratorName").Value;
            this.GeneratorParameters.ReadXml(xe);
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

        public static void InitGeneratorStartValues(OptionEntities.ExecutionTaskOptions options)
        {
            Generator.StartDate = options.DateTimeGenerationStartTime;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be casted return false:
            Generator p = obj as Generator;
            if ((object)p == null)
                return false;

            // Return true if the fields match:
            return GetHashCode() == p.GetHashCode();
        }

        public bool Equals(Generator other)
        {
            // TODO: Implament equals on the generator parameter collection
            return
                object.Equals(this.GeneratorParameters, other.GeneratorParameters) &&
                this.IsSqlQueryGenerator == other.IsSqlQueryGenerator &&
                this.GeneratorName == other.GeneratorName;
        }

        public override int GetHashCode()
        {
            return
                this.GeneratorParameters.GetHashCode() ^
                this.IsSqlQueryGenerator.GetHashCode() ^
                this.GeneratorName.GetHashCode()
                ;
        }
    }
}
