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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace SQLDataProducer.Entities.DatabaseEntities
{
    public class ColumnDataTypeDefinition : EntityBase
    {

        public ColumnDataTypeDefinition(string rawDataType, bool nullable)
        {
            Raw = rawDataType;
            DBType = StringToDBDataType(rawDataType);
            IsNullable = nullable;

            if (DBType == SqlDbType.VarChar 
                || DBType == SqlDbType.NVarChar 
                ||DBType == SqlDbType.Char 
                ||DBType == SqlDbType.NChar)
            {
                int length = GetLengthOfStringDataType(rawDataType);
                MaxLength = length;
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
    }

   
}
