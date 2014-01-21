// Copyright 2012-2014 Peter Henell

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
    public class GeneratorParameterCollection : IEnumerable<GeneratorParameter>
    {
        public GeneratorParameterCollection()
        {
            parameters = new Dictionary<string, GeneratorParameter>();
        }

        private Dictionary<string, GeneratorParameter> parameters;


        /// <summary>
        /// Returns a humanly readable string that describes the GeneratorParameters in the collection.
        /// Format: Name: Value
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var key in parameters.Keys)
            {
                sb.AppendFormat("{0};", parameters[key]);
            }
            return sb.ToString();
        }

        private string ToNiceString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in parameters.Keys)
            {
                var s = parameters[key];
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
            parameters.Add(parameter.ParameterName, parameter);
        }

        public GeneratorParameterCollection Clone()
        {
            // TODO: Clone using the same entity as the LOAD/SAVE functionality
            var paramCollection = new GeneratorParameterCollection();
            foreach (var g in parameters.Values)
            {
                paramCollection.Add(g.Clone());
            }
            return paramCollection;
        }

        public GeneratorParameter this[string parameterName]
        {
            get
            {
                return parameters[parameterName];
            }
        }

        public int Count
        {
            get
            {
                return parameters.Count;
            }
        }

        public T GetValueOf<T>(string parameterName)
        {
            GeneratorParameter param = null;
            if (!parameters.TryGetValue(parameterName, out param))
            {
                throw new KeyNotFoundException(parameterName);
            }
            if (param.Value == null)
                throw new ArgumentNullException(parameterName);
            else if (param.Value is T)
                return (T)param.Value;
            else
                throw new InvalidCastException("The parameter did not match the supplied type. parameterName: " + parameterName + ", requested type: " + typeof(T).ToString());
        }

        public IEnumerator<GeneratorParameter> GetEnumerator()
        {
            return parameters.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return parameters.Values.GetEnumerator();
        }
    }
}
