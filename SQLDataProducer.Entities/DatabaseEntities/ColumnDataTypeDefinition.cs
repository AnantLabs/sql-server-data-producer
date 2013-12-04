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
        public static readonly int STRING_DEFAULT_LENGTH = 50;
        public static readonly int DECIMAL_DEFAULT_LENGTH = 18;
        public static readonly int DECIMAL_DEFAULT_SCALE = 0;

        
        public SqlDbType DBType { get; private set; }
        public string StringFormatter { get; private set; }
        public bool IsNullable { get; private set; }
        public int Scale { get; private set; }
        public string Raw { get; private set; }
        
        public int MaxLength { get; private set; }
        public decimal MaxValue { get; private set; }
        public decimal MinValue { get; private set; }

        public ColumnDataTypeDefinition(string rawDataType, bool nullable)
        {
            Raw = rawDataType.ToLower();
            DBType = StringToDBDataType(rawDataType);
            IsNullable = nullable;
            StringFormatter = GetStringFormatter();

            if (DBType == SqlDbType.VarChar 
                || DBType == SqlDbType.NVarChar 
                ||DBType == SqlDbType.Char 
                ||DBType == SqlDbType.NChar)
            {
                int length = GetLengthOfStringDataType(rawDataType);
                MaxLength = length;
            }
            if (DBType == SqlDbType.Decimal)
            {
                SetLengthAndScale(rawDataType);
            }
            SetMaxMinValues();
        }

        private void SetMaxMinValues()
        {
            switch (DBType)
            {
                case SqlDbType.BigInt:
                    MaxValue = long.MaxValue;
                    MinValue = long.MinValue;
                    break;
                case SqlDbType.Bit:
                    MaxValue = 1;
                    MinValue = 0;
                    break;
                case SqlDbType.Decimal:
                    SetMaxAndMinForDecimalType();
                    break;
                case SqlDbType.Float:
                    MaxValue = decimal.MaxValue;
                    MinValue = decimal.MinValue;
                    break;
                case SqlDbType.Int:
                    MaxValue = int.MaxValue;
                    MinValue = int.MinValue;
                    break;
                case SqlDbType.Money:
                    //     System.Decimal. A currency value ranging from -2 63 (or -9,223,372,036,854,775,808)
                    //     to 2 63 -1 (or +9,223,372,036,854,775,807) with an accuracy to a ten-thousandth
                    //     of a currency unit.
                    MaxValue = 9223372036854775807.999m;
                    MinValue = -9223372036854775808.999m;
                    break;
                case SqlDbType.Real:
                    MaxValue = decimal.MaxValue;
                    MinValue = decimal.MinValue;
                    break;
                case SqlDbType.SmallInt:
                    MaxValue = short.MaxValue;
                    MinValue = short.MinValue;
                    break;
                case SqlDbType.SmallMoney:
                    MinValue = -214748.3648m; 
                    MaxValue = 214748.3647m;
                    break;
                case SqlDbType.TinyInt:
                    MaxValue = byte.MaxValue;
                    MinValue = byte.MinValue;
                    break;
                default:
                    break;
            }
        }

        private void SetMaxAndMinForDecimalType()
        {
            double a = Math.Pow(10, MaxLength - Scale) - 1;
            double b = 1 - Math.Pow(0.1, Scale);
            decimal max = (decimal)(a + b);
            MaxValue = max;
            MinValue = -1 * max;
        }

        private void SetLengthAndScale(string rawDataType)
        {
            // find the precision part of the decimal datatype - decimal(precision, scale)
            Regex r = new Regex(@"\((?<precision>[0-9]*)(,[ ]?)?(?<scale>[0-9]*?)\)", RegexOptions.Compiled);
            int length;
            int scale;

            if (r.Matches(rawDataType).Count > 0)
            {
                // a length was found in the string between the paranteses
                var precision = r.Match(rawDataType).Result("${precision}");
                if (!int.TryParse(precision, out length))
                {
                    length = DECIMAL_DEFAULT_LENGTH;
                }
                var s = r.Match(rawDataType).Result("${scale}");
                if (!int.TryParse(s, out scale))
                {
                    scale = DECIMAL_DEFAULT_SCALE;
                }
            }
            else
            {
                // no length in the string, use default values
                length = DECIMAL_DEFAULT_LENGTH;
                scale = DECIMAL_DEFAULT_SCALE;
            }
            if (length == 0)
            {
                scale = DECIMAL_DEFAULT_SCALE;
                length = DECIMAL_DEFAULT_LENGTH;
            }
            MaxLength = length;
            Scale = scale;
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
                    length = STRING_DEFAULT_LENGTH;
                }
            }
            else
            {
                // no length in the string, this is probably a sysname column
                length = STRING_DEFAULT_LENGTH;
            }
            return length;
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
                // fallback to varchar
                sqlType = SqlDbType.VarChar;
            }

            return sqlType;
        }

        public override bool Equals(System.Object obj)
        {
            ColumnDataTypeDefinition p = obj as ColumnDataTypeDefinition;
            if ((object)p == null)
                return false;

            return GetHashCode().Equals(p.GetHashCode());
        }

        public bool Equals(ColumnDataTypeDefinition other)
        {
            return
               this.Raw.Equals(other.Raw);//&&
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 37;
                hash = hash * 23 + Raw.GetHashCode();
                return hash;
            }
        }





        public ColumnDataTypeDefinition Clone()
        {
            return new ColumnDataTypeDefinition(Raw, this.IsNullable);
        }
    }

   
}
