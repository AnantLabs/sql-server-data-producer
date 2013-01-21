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
using SQLDataProducer.Entities.Generators;
using System.Collections.ObjectModel;
using SQLDataProducer.Entities;
using System.Xml.Serialization;
using SQLDataProducer.Entities.DatabaseEntities;
using System.Xml.Linq;


namespace SQLDataProducer.Entities.DatabaseEntities
{
    public partial class ColumnEntity : EntityBase, IEquatable<ColumnEntity>
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
                    _generator.ParentColumn = this;
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
            set
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

                    //if (_foreignKey != null)
                    //{
                    //    _foreignKey.PropertyChanged += (sender, e) =>
                    //    {
                    //        if (e.PropertyName == "Keys")
                    //        {
                    //            if (ForeignKey.Keys.Count > 0)
                    //            {
                    //                HasWarning = false;
                    //            }
                    //        }
                    //    };
                    //}
                    OnPropertyChanged("ForeignKey");
                }
            }
        }


        string _constraints;
        [System.ComponentModel.ReadOnly(true)]
        public string Constraints
        {
            get
            {
                return _constraints;
            }
            private set
            {
                if (_constraints != value)
                {
                    _constraints = value;
                    _hasConstraints = !string.IsNullOrEmpty(_constraints);
                    OnPropertyChanged("Constraints");
                    OnPropertyChanged("HasConstraints");
                }
            }
        }

        bool _hasConstraints;
        [System.ComponentModel.ReadOnly(true)]
        public bool HasConstraints
        {
            get
            {
                return _hasConstraints;
            }
        }


        internal ColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, string constraintDefinition, ForeignKeyEntity foreignKeyEntity)
        {
            this.ColumnName = columnName;
            this.ColumnDataType = columnDatatype;
            this.OrdinalPosition = ordinalPosition;
            this.IsIdentity = isIdentity;

            this.IsForeignKey = isForeignKey;
            this.ForeignKey = foreignKeyEntity;

            this.Constraints = constraintDefinition;

            RefreshWarningStatus();

        }

        ///// <summary>
        ///// Constructor of the ColumnEntity
        ///// </summary>
        ///// <param name="columnName">name of the column</param>
        ///// <param name="columnDatatype">string name of the SQL datatype of the column</param>
        ///// <param name="isIdentity">true if the column is identity, otherwise false</param>
        ///// <param name="ordinalPosition">the ordinal position of the column</param>
        ///// <param name="isForeignKey">true if this table is referencing another table using foreign key</param>
        ///// <param name="generator">the default generator for this column</param>
        ///// <param name="possibleGenerators">the possible generators for this column</param>
        //internal ColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, ForeignKeyEntity foreignKeyEntity)
        //    : this(columnName, columnDatatype, isIdentity, ordinalPosition, isForeignKey, foreignKeyEntity)
        //{
        //    //this.Generator = generator ?? possibleGenerators.First();
        //}

        //internal ColumnEntity(string columnName, ColumnDataTypeDefinition columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, ForeignKeyEntity foreignKeyEntity)
        //    : this(columnName, columnDatatype, isIdentity, ordinalPosition, isForeignKey, foreignKeyEntity)
        //{
        //    //this.Generator = possibleGenerators.Where(g => g.GeneratorName == generatorName).First();
        //}

        public ColumnEntity()
        {

        }

        public void ReadXml(XElement xe)
        {
            this.ColumnName = xe.Attribute("ColumnName").Value;
            Generator g = new Generator();
            g.ReadXml(xe.Descendants("Generator").FirstOrDefault());
            this.Generator = g;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Column");
            writer.WriteAttributeString("ColumnName", this.ColumnName);
           
            this.Generator.WriteXml(writer);

            writer.WriteEndElement();
        }

        public void GenerateValue(long n)
        {
            if (Generator.GENERATOR_ValueFromOtherColumn == Generator.GeneratorName)
                return;

            PreviouslyGeneratedValue  = Generator.GenerateValue(n);
        }


        object _previouslyGeneratedValue;
        public object PreviouslyGeneratedValue
        {
            get
            {
                if (Generator.GENERATOR_ValueFromOtherColumn == Generator.GeneratorName)
                    return Generator.GenerateValue(0);
                
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

        private bool _hasWarning = false;
        /// <summary>
        /// This Item have some kind of warning that might cause problems during execution
        /// </summary>
        public bool HasWarning
        {
            get
            {
                return _hasWarning;
            }
            set
            {
                _hasWarning = value;
                OnPropertyChanged("HasWarning");
            }
        }

        private string _warningText = string.Empty;
        /// <summary>
        /// Contains warning text if the this item have a warning that might cause problems during execution.
        /// </summary>
        public string WarningText 
        {
            get
            {
                return _warningText;
            }
            set
            {
                _warningText = value;
                OnPropertyChanged("WarningText");
            }
        }


        TableEntity _parentTable;
        public TableEntity ParentTable
        {
            get
            {
                return _parentTable;
            }
            set
            {
                if (_parentTable != value)
                {
                    _parentTable = value;
                    OnPropertyChanged("ParentTable");
                }
            }
        }


        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be casted return false:
            ColumnEntity p = obj as ColumnEntity;
            if ((object)p == null)
                return false;

            // Return true if the fields match:
            return GetHashCode() == p.GetHashCode();
        }

        public bool Equals(ColumnEntity other)
        {
            return
                this.ColumnDataType.Equals(other.ColumnDataType) &&
                this.ColumnName.Equals(other.ColumnName) &&
                this.Constraints.Equals(other.Constraints) &&
                // this.Generator == other.Generator &&
                this.HasWarning.Equals(other.HasWarning) &&
                this.IsForeignKey.Equals(other.IsForeignKey) &&
                this.IsIdentity.Equals(other.IsIdentity) &&
                this.OrdinalPosition.Equals(other.OrdinalPosition) &&
                // this.PossibleGenerators == other.PossibleGenerators &&
                this.WarningText.Equals(other.WarningText);
        }

//        List of coprimes in between 1 and 31
//1 and 2-31
//2 and 3,5,7,9,11,13,15,17,19,21,23,25,27,29
//3 and 4,5,7,8,10,11,13,14,16,17,19,20,22,23,25…
//4 and all odd numbers 5-30
//5 and 6-9,11-14,16-19,21-24,26-29
//6 and 7,11,13,17,19,23,25,29
//7 and 8-13,15-20,22-27,29-30
//8 and all odd numbers 9-29
//9 and 10,11,13,14,16,17,19,20,22,23,25,26,28,2…
//10 and 11,13,17,19,21,23,27,29
//11 and 12-21,23-30
//12 and 13,17,19,23,25,29
//13 and 14-25,27-30
//14 and 15,17,19,23,25,27,29
//15 and 16,17,19,22,23,26,28,29
//16 and all odd numbers 17-29
//17 and 18-30
//18 and 19,23,25,29
//19 and 20-30
//20 and 21,23,27,29
//21 and 22,23,25,26,29
//22 and 23,25,27,29
//23 and 24-30
//24 and 25,29
//25 and 26-29
//26 and 27,29
//27 and 28,29
//28 and 29
//29 and 30
        
        public override int GetHashCode()
        {
            int hash = 37;
            hash = hash * 23 + base.GetHashCode();
            hash = hash * 23 + ColumnDataType.GetHashCode();
            hash = hash * 23 + ColumnName.GetHashCode();
            hash = hash * 23 + Constraints.GetHashCode();
            hash = hash * 23 + HasWarning.GetHashCode();
            hash = hash * 23 + IsForeignKey.GetHashCode();
            hash = hash * 23 + IsIdentity.GetHashCode();
            hash = hash * 23 + OrdinalPosition.GetHashCode();
            hash = hash * 23 + WarningText.GetHashCode();
            return hash;
        }

        internal void RefreshWarningStatus()
        {
            if (IsForeignKey && ForeignKey.Keys.Count == 0)
            {
                HasWarning = true;
                WarningText = "This column is referencing a table without foreign keys. Insertion might fail unless you use the Identity from item# generator.";
            }
            else
            {
                HasWarning = false;
                WarningText = string.Empty;
            }
        }
    }
}
