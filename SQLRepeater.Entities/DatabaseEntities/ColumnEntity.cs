using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SQLRepeater.Entities.DatabaseEntities;
using SQLRepeater.Entities.Generators;
using System.Collections.ObjectModel;


namespace SQLRepeater.DatabaseEntities.Entities
{
    public partial class ColumnEntity : SQLRepeater.Entities.EntityBase
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
    }
}
