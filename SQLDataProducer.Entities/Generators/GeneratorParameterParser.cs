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

namespace SQLDataProducer.Entities.Generators
{
    public class GeneratorParameterParser : IEquatable<GeneratorParameterParser>
    {
        public string ParserName { get; private set; }
        private Func<object, object> ValueParser { get; set; }
        private Func<object, string> StringFormater { get; set; }

        private GeneratorParameterParser()
        {
        }

        public object ParseValue(object o)
        {
            if (o == null)
                return o;
            
            return ValueParser(o);
        }
        public string FormatToString(object parameterValue)
        {
            return StringFormater(parameterValue);
        }

        public static GeneratorParameterParser DecimalParser = new GeneratorParameterParser
        {
            ParserName = "Decimal Parser",
            ValueParser = new Func<object, object>(o =>
                {
                    return double.Parse(o.ToString().Replace(".", ","));
                }),
            StringFormater = new Func<object, string>(o =>
                {
                    if (o == null)
                        return null;

                    return o.ToString();
                })
        };

        public static GeneratorParameterParser LonglParser = new GeneratorParameterParser
        {
            ParserName = "Long Parser",
            ValueParser = new Func<object, object>(o =>
                {
                    return long.Parse(o.ToString());
                }),
            StringFormater = new Func<object, string>(o =>
                {
                    if (o == null)
                        return null;

                    return o.ToString();
                })
        };
        public static GeneratorParameterParser IntegerParser = new GeneratorParameterParser
        {
            ParserName = "Integer Parser",
            ValueParser = new Func<object, object>(o =>
            {
                return int.Parse(o.ToString());
            }),
            StringFormater = new Func<object, string>(o =>
                {
                    if (o == null)
                        return null;

                    return o.ToString();
                })
        };
        public static GeneratorParameterParser DateTimeParser = new GeneratorParameterParser
        {
            ParserName = "DateTime Parser",
            ValueParser = new Func<object, object>(o =>
                {
                    if (o is DateTime)
                        return (DateTime)o;
                    
                    return DateTime.Parse(o.ToString());
                }),
            StringFormater = new Func<object, string>(o =>
                {
                    if (o == null)
                        return null;

                    return ((DateTime)o).ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
                })
        };
        public static GeneratorParameterParser StringParser = new GeneratorParameterParser
        {
            ParserName = "String Parser",
            ValueParser = new Func<object, object>(o =>
                {
                    return o.ToString();
                }),
            StringFormater = new Func<object, string>(o =>
                {
                    if (o == null)
                        return null;

                    return o.ToString();
                })
        };
        public static GeneratorParameterParser ObjectParser = new GeneratorParameterParser
        {
            ParserName = "Object Parser",
            ValueParser = new Func<object, object>(o =>
            {
                return o;
            }),
            StringFormater = new Func<object, string>(o =>
                {
                    if (o == null)
                        return null;
                    
                    return o.ToString();
                })
        };

        //public static GeneratorParameterParser ColumnParser = new GeneratorParameterParser
        //{
        //    ParserName = "Column Parser",
        //    ValueParser = new Func<object, object>(o =>
        //    {
        //        return o;
        //    }),
        //    StringFormater = new Func<object, string>(o =>
        //    {
        //        if (o == null)
        //            return null;

        //        return o.ToString();
        //    })
        //};


        public static GeneratorParameterParser FromName(string name)
        {
            switch (name)
            {
                case "Object Parser":
                    return ObjectParser;
                case "Decimal Parser":
                    return DecimalParser;
                case "String Parser":
                    return StringParser;
                case "DateTime Parser":
                    return DateTimeParser;
                case "Long Parser":
                    return LonglParser;
                case "Integer Parser":
                    return IntegerParser;

                default:
                    throw new ArgumentException("Supplied parser name cannot be found", name);
            }
        }



        public override bool Equals(object obj)
        {
             // If parameter cannot be casted return false:
            GeneratorParameterParser p = obj as GeneratorParameterParser;
            if ((object)p == null)
                return false;

            // Return true if the fields match:
            return GetHashCode() == p.GetHashCode();
        }

        public override int GetHashCode()
        {
            return ParserName.GetHashCode();
        }

        public bool Equals(GeneratorParameterParser other)
        {
            return ParserName.Equals(other.ParserName);
        }
    }
}
