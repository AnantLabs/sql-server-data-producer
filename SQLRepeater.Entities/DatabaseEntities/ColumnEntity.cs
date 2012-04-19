using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SQLRepeater.Entities.DatabaseEntities;
using SQLRepeater.Entities.Generators;


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

        //private object _genParameter;
        //public object GeneratorParameter
        //{
        //    get
        //    {
        //        return _genParameter;
        //    }
        //    set
        //    {
        //        if (_genParameter != value)
        //        {
        //            _genParameter = value;
        //            OnPropertyChanged("GeneratorParameter");
        //        }
        //    }
        //}


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
                }
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

        public ColumnEntity(string columnName, string columnDatatype, bool isIdentity, int ordinalPosition, GeneratorBase generator)
        {
            this.ColumnName = columnName;
            this.ColumnDataType = columnDatatype;
            this.OrdinalPosition = ordinalPosition;
            this.IsIdentity = isIdentity;
            this.Generator = generator;
        }

      
    }
}
