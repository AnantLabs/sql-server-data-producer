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
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace SQLDataProducer.Entities.Generators.Collections
{
    /// <summary>
    /// To enable binding to the NiceString property to the DataGrid.
    /// </summary>
    public class GeneratorParameterCollection : Dictionary<string, GeneratorParameter>
    {
        //Dictionary<string, GeneratorParameter> items;

        public GeneratorParameterCollection()
            : base()
        {
            //items = new Dictionary<string, GeneratorParameter>();
        }

        /// <summary>
        /// Returns a humanly readable string that describes the GeneratorParameters in the collection.
        /// Format: Name: Value
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var key in this.Keys)
            {
                sb.AppendFormat("{0};", this[key]);
            }
            return sb.ToString();
        }

        private string ToNiceString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in this.Keys)
            {
                var s = this[key];
                // Avoid showing parameters that cannot be changed anyway
                if (!s.IsWriteEnabled)
                    continue;

                sb.AppendFormat("{0}='{1}';", s.ParameterName, s.Value);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Bindable ToString property
        /// </summary>
        public string NiceString
        {
            get { return this.ToNiceString(); }
        }

        public void Add(GeneratorParameter parameter)
        {
            this.Add(parameter.ParameterName, parameter);
        }

        //public GeneratorParameter this[int index]
        //{
        //    get
        //    {
        //        return this.Keys[index];
        //    }
        //}

        //public int Count { get { return this.Count; } }

        public GeneratorParameterCollection Clone()
        {
            // TODO: Clone using the same entity as the LOAD/SAVE functionality
            var paramCollection = new GeneratorParameterCollection();
            foreach (var g in this.Values)
            {
                paramCollection.Add(g.Clone());
            }
            return paramCollection;
        }


        public T GetValueOf<T>(string parameterName)
        {
            var param = this[parameterName];
            if (param == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (param.Value is T)
                return (T)param.Value;
            else
                throw new InvalidCastException("The parameter did not match the supplied type. parameterName: " + parameterName + ", requested type: " + typeof(T).ToString());
        }
    }
}
