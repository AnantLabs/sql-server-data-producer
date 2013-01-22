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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace SQLDataProducer.Entities.DatabaseEntities
{
    public class ColumnDataTypeDefinition : EntityBase, IEquatable<ColumnDataTypeDefinition>
    {

        public ColumnDataTypeDefinition(string rawDataType, bool nullable)
        {
            Raw = rawDataType;
            DBType = StringToDBDataType(rawDataType);
            IsNullable = nullable;
            _stringFormatter = GetStringFormatter();

            if (DBType == SqlDbType.VarChar 
                || DBType == SqlDbType.NVarChar 
                ||DBType == SqlDbType.Char 
                ||DBType == SqlDbType.NChar)
            {
                int length = GetLengthOfStringDataType(rawDataType);
                MaxLength = length;
            }
        }

        private string GetStringFormatter()
        {
            switch (DBType)
            {
                case SqlDbType.BigInt:
                case SqlDbType.Binary:
               
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Image:
                case SqlDbType.Int:
                case SqlDbType.Money:
                case SqlDbType.Real:
                case SqlDbType.SmallInt:
                case SqlDbType.SmallMoney:
                case SqlDbType.Structured:
                case SqlDbType.Timestamp:
                case SqlDbType.TinyInt:
                case SqlDbType.Udt:

                case SqlDbType.VarBinary:
                    return "{0}";

                case SqlDbType.Char:
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Text:
                case SqlDbType.Time:
                case SqlDbType.UniqueIdentifier:
                case SqlDbType.VarChar:
                case SqlDbType.Variant:
                case SqlDbType.Xml:
                case SqlDbType.Bit:
                    return "'{0}'";
                    
                default:
                    return "'{0}'";
            }
        }

        private static int GetLengthOfStringDataType(string rawDataType)
        {
            // find the LENGTH part of the string datatype(LENGTH)
            Regex r = new Regex(@"\((?<length>[0-9]*|max)\)", RegexOptions.Compiled);
            int length;

            if (r.Matches(rawDataType).Count > 0)
            {
                // a length was found in the string between the paranteses
                var m = r.Match(rawDataType).Result("${length}");

                if (m.ToLower() == "max")
                {
                    length = int.MaxValue;
                }
                else if (!int.TryParse(m, out length))
                {
                    length = 0;
                }
            }
            else
            {
                // no length in the string, this is probably a sysname column
                length = 50;
            }
            return length;
        }

        SqlDbType _dbType;
        public SqlDbType DBType
        {
            get
            {
                return _dbType;
            }
            set
            {
                if (_dbType != value)
                {
                    _dbType = value;
                    OnPropertyChanged("DBType");
                }
            }
        }


        string _stringFormatter;
        public string StringFormatter
        {
            get
            {
                return _stringFormatter;
            }
        }

        bool _isNullable;
        public bool IsNullable
        {
            get
            {
                return _isNullable;
            }
            set
            {
                if (_isNullable != value)
                {
                    _isNullable = value;
                    OnPropertyChanged("IsNullable");
                }
            }
        }


        int _maxLength;
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                if (_maxLength != value)
                {
                    _maxLength = value;
                    OnPropertyChanged("MaxLength");
                }
            }
        }


        string _raw;
        public string Raw
        {
            get
            {
                return _raw;
            }
            set
            {
                if (_raw != value)
                {
                    _raw = value;
                    OnPropertyChanged("Raw");
                }
            }
        }


        public override string ToString()
        {
            return DBType.ToString();
        }

        /// <summary>
        /// Convert raw sql datatype string to SqlDbType enum
        /// </summary>
        /// <param name="dataType">raw sql datatype string</param>
        /// <returns>converted value, SqlDbType.Varchar if conversion failed.</returns>
        private SqlDbType StringToDBDataType(string dataType)
        {
            SqlDbType sqlType;
            if (!Enum.TryParse(dataType.Substring(0, dataType.IndexOf('(') > 0 ? dataType.IndexOf('(') : dataType.Length), true, out sqlType))
            {
                sqlType = SqlDbType.VarChar;
            }

            return sqlType;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be casted return false:
            ColumnDataTypeDefinition p = obj as ColumnDataTypeDefinition;
            if ((object)p == null)
                return false;

            // Return true if the fields match:
            return GetHashCode() == p.GetHashCode();
        }

        public bool Equals(ColumnDataTypeDefinition other)
        {
            return
               this.DBType.Equals(other.DBType) &&
               this.IsNullable.Equals(other.IsNullable) &&
               this.MaxLength.Equals(other.MaxLength) &&
               this.Raw.Equals(other.Raw) &&
               this.StringFormatter.Equals(other.StringFormatter);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 37;
                hash = hash * 23 + DBType.GetHashCode();
                hash = hash * 23 + IsNullable.GetHashCode();
                hash = hash * 23 + MaxLength.GetHashCode();
                hash = hash * 23 + Raw.GetHashCode();
                hash = hash * 23 + StringFormatter.GetHashCode();
                return hash;
            }
        }
    }

   
}
