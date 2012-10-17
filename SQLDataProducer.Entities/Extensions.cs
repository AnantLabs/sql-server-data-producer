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
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace SQLDataProducer.Entities
{
    public static class Extensions
    {
        public static int TryGetIntAttribute(this System.Xml.XmlReader reader, string attributeName, int fallback)
        {
            int a;
            if (int.TryParse(reader.GetAttribute(attributeName), out a))
                return a;
            
            return fallback;
        }

        public static bool TryGetBoolAttribute(this System.Xml.XmlReader reader, string attributeName, bool fallback)
        {
            bool a;
            if (bool.TryParse(reader.GetAttribute(attributeName), out a))
                return a;

            return fallback;
        }

        public static string SubstringWithMaxLength(this string str, int maxLength)
        { 
            int l = Math.Min(maxLength, str.Length);
            return str.Substring(0, l);
        }

        public static int LongToInt(this long value)
        {
            if (value < int.MinValue)
                return int.MinValue;
            
            if (value > int.MaxValue)
	            return int.MaxValue;

            return Convert.ToInt32(value);
        }

        //public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        //{
        //    var c = new ObservableCollection<T>();
        //    foreach (var e in coll)
        //        c.Add(e);
        //    return c;
        //}
    }
}
