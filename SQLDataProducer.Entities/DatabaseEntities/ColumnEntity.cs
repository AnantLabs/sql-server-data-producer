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
using SQLDataProducer.Entities.Generators;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities;
using System.Xml.Serialization;
using SQLDataProducer.Entities.DatabaseEntities;


namespace SQLDataProducer.DatabaseEntities.Entities
{
    public partial class ColumnEntity : SQLDataProducer.Entities.EntityBase, IXmlSerializable
    {
        ColumnDataTypeDefinition _columnDataType;
        [System.ComponentModel.ReadOnly(true)]
        public ColumnDataTypeDefinition ColumnDataType
        {
            get
            {
                return _columnDataType;
            }
            private set
            {
                if (_columnDataType != value)
                {
                    _columnDataType = value;
                    OnPropertyChanged("ColumnDataType");
                }
            }
        }

        bool _isIdentity;
        [System.ComponentModel.ReadOnly(true)]
        public bool IsIdentity
        {
            get
            {
                return _isIdentity;
            }
            private set
            {
                if (_isIdentity != value)
                {
                    _isIdentity = value;
                    OnPropertyChanged("IsIdentity");
                    OnPropertyChanged("IsNotIdentity");
                }
            }
        }

        [System.ComponentModel.ReadOnly(true)]
        public bool IsNotIdentity
        {
            get
            {
                return !_isIdentity;
            }
        }

        string _columnName;
        [System.ComponentModel.ReadOnly(true)]
        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            private set
            {
                if (_columnName != value)
                {
                    _columnName = value;
                    OnPropertyChanged("ColumnName");
                }
            }
        }


        int _ordinalPosition;
        [System.ComponentModel.ReadOnly(true)]
        public int OrdinalPosition
        {
            get
            {
                return _ordinalPosition;
            }
            private set
            {
                if (_ordinalPosition != value)
                {
                    _ordinalPosition = value;
                    OnPropertyChanged("OrdinalPosition");
                }
            }
        }

        Generator _generator;
        public Generator Generator
        {
            get
            {
                return _generator;
            }
            set
            {
                if (_generator != value)
                {
                    _generator = value;
                    OnPropertyChanged("Generator");
                }
            }
        }

        ObservableCollection<Generator> _valueGenerators;
        public ObservableCollection<Generator> PossibleGenerators
        {
            get
            {
                return _valueGenerators;
            }
            private set
            {
                if (_valueGenerators != value)
                {
                    _valueGenerators = value;
                    OnPropertyChanged("PossibleGenerators");
                }
            }
        }

        private bool _isForeignKey;
        [System.ComponentModel.ReadOnly(true)]
        public bool IsForeignKey
        {
            get
            {
                return _isForeignKey;
            }
            private set
            {
                if (_isForeignKey != value)
                {
                    _isForeignKey = value;
                    OnPropertyChanged("IsForeignKey");
                }
            }
        }

        private ForeignKeyEntity _foreignKey;
        [System.ComponentModel.ReadOnly(true)]
        public ForeignKeyEntity ForeignKey
        {
            get
            {
                return _foreignKey;
            }
            private set
            {
                if (_foreignKey != value)
                {
                    _foreignKey = value;
                    OnPropertyChanged("ForeignKey");
                }
            }
        }
        
       

        /// <summary>
        /// Constructor of the ColumnEntity
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="columnDatatype">string name of the SQL datatype of the column</param>
        /// <param name="isIdentity">true if the column is identity, otherwise false</param>
        /// <param name="ordinalPosition">the ordinal position of the column</param>
        /// <param name="isForeignKey">true if this table is referencing another table using foreign key</param>
        /// <param name="generator">the default generator for this column</param>
        /// <param name="possibleGenerators">the possible generators for this column</param>
        public ColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, ForeignKeyEntity foreignKeyEntity, ObservableCollection<Generator> possibleGenerators, Generator generator)
        {
            this.ColumnName = columnName;
            this.ColumnDataType = columnDatatype;
            this.OrdinalPosition = ordinalPosition;
            this.IsIdentity = isIdentity;

            this.IsForeignKey = isForeignKey;
            this.ForeignKey = foreignKeyEntity;

            this.Generator = generator ?? possibleGenerators.First();
            this.PossibleGenerators = possibleGenerators;

        }
        public ColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, ForeignKeyEntity foreignKeyEntity, ObservableCollection<Generator> generators, string generatorName)
        {
            this.ColumnName = columnName;
            this.ColumnDataType = columnDatatype;
            this.OrdinalPosition = ordinalPosition;
            this.IsIdentity = isIdentity;

            this.IsForeignKey = isForeignKey;
            this.ForeignKey = foreignKeyEntity;

            this.Generator = generators.Where(g => g.GeneratorName == generatorName).First();
            this.PossibleGenerators = generators;
        }
        public ColumnEntity()
        {

        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            this.ColumnName = reader.GetAttribute("ColumnName");
            
            Generator g = new Generator();
            g.ReadXml(reader);
            this.Generator = g;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Column");
            writer.WriteAttributeString("ColumnName", this.ColumnName);
           
            this.Generator.WriteXml(writer);

            writer.WriteEndElement();
        }

        public object GenerateValue(int n)
        {
            var val = Generator.GenerateValue(n);
            PreviouslyGeneratedValue = val;
            return val;
        }


        object _previouslyGeneratedValue;
        public object PreviouslyGeneratedValue
        {
            get
            {
                return _previouslyGeneratedValue;
            }
            set
            {
                if (_previouslyGeneratedValue != value)
                {
                    _previouslyGeneratedValue = value;
                    OnPropertyChanged("PreviouslyGeneratedValue");
                }
            }
        }

        public override string ToString()
        {
            return ColumnName;
        }
    }
}
