using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SQLRepeater.Entities.DatabaseEntities;
using SQLRepeater.Entities.Generators;
using System.Collections.ObjectModel;


namespace SQLRepeater.DatabaseEntities.Entities
{
    public partial class ColumnEntity : SQLRepeater.Entities.EntityBase//, IEquatable<ColumnEntity>
    {
        string _columnDataType;
        [System.ComponentModel.ReadOnly(true)]
        public string ColumnDataType
        {
            get
            {
                return _columnDataType;
            }
            set
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
            set
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
            set
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
            set
            {
                if (_ordinalPosition != value)
                {
                    _ordinalPosition = value;
                    OnPropertyChanged("OrdinalPosition");
                }
            }
        }

        GeneratorBase _generator;
        public GeneratorBase Generator
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

        ObservableCollection<GeneratorBase> _valueGenerators;
        public ObservableCollection<GeneratorBase> PossibleGenerators
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
            set
            {
                if (_isForeignKey != value)
                {
                    _isForeignKey = value;
                    OnPropertyChanged("IsForeignKey");
                }
            }
        }

        private string _referencingTable;
        [System.ComponentModel.ReadOnly(true)]
        public string ReferencingTable
        {
            get
            {
                return _referencingTable;
            }
            set
            {
                if (_referencingTable != value)
                {
                    _referencingTable = value;
                    OnPropertyChanged("ReferencingTable");
                }
            }
        }

        private string _referencingColumn;
        [System.ComponentModel.ReadOnly(true)]
        public string ReferencingColumn
        {
            get
            {
                return _referencingColumn;
            }
            set
            {
                if (_referencingColumn != value)
                {
                    _referencingColumn = value;
                    OnPropertyChanged("ReferencingColumn");
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
        /// <param name="referencingTable">the name of the table that this column is referencing using foreign key. Leave empty if NA</param>
        /// <param name="referencingColumn">the name of the column that this column is referencing using foreign key. Leave empty if NA</param>
        /// <param name="generator">the default generator for this column</param>
        /// <param name="possibleGenerators">the possible generators for this column</param>
        public ColumnEntity(string columnName, string columnDatatype, bool isIdentity, int ordinalPosition, bool isForeignKey, string referencingTable, string referencingColumn,
           GeneratorBase generator, ObservableCollection<GeneratorBase> possibleGenerators)
        {
            this.ColumnName = columnName;
            this.ColumnDataType = columnDatatype;
            this.OrdinalPosition = ordinalPosition;
            this.IsIdentity = isIdentity;

            this.IsForeignKey = isForeignKey;
            this.ReferencingTable = referencingTable;
            this.ReferencingColumn = referencingColumn;


            this.Generator = generator;
            this.PossibleGenerators = possibleGenerators;

        }


        //public bool Equals(ColumnEntity other)
        //{
        //    // Check whether the compared object is null.
        //    if (Object.ReferenceEquals(other, null)) return false;

        //    // Check whether the compared object references the same data.
        //    if (Object.ReferenceEquals(this, other)) return true;

        //    // Check whether the objects’ properties are equal.
        //    return ColumnName.Equals(other.ToString());
        //}

        //// If Equals returns true for a pair of objects,
        //// GetHashCode must return the same value for these objects.

        //public override int GetHashCode()
        //{
        //    // Get the hash code for the Textual field if it is not null.
        //    return ColumnName.GetHashCode();

        //}
    }
}
