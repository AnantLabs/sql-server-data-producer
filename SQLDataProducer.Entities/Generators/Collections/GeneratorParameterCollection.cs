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

namespace SQLDataProducer.Entities.Generators.Collections
{
    /// <summary>
    /// To enable binding to the NiceString property to the DataGrid.
    /// </summary>
    public class GeneratorParameterCollection : ObservableCollection<GeneratorParameter>
    {

        /// <summary>
        /// Returns a humanly readable string that describes the GeneratorParameters in the collection.
        /// Format: Name: Value
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var s in this.Items)
            {
                // Avoid showing parameters that cannot be changed anyway
                if (!s.IsWriteEnabled)
                    continue;
                
                sb.AppendFormat("{{{0}: {1}}}; ", s.ParameterName, s.Value);
            }
            return sb.ToString();
        }


        /// <summary>
        /// Bindable ToString property
        /// </summary>
        [XmlIgnore]
        public string NiceString
        {
            get { return this.ToString(); }
        }

        
        public GeneratorParameterCollection() 
            : base()
        {
            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(GeneratorParameterCollection_CollectionChanged);
            
        }

        /// <summary>
        /// To notify about updates of the NiceString property we need to hook up events of the internal collection and add event handler to all the added items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GeneratorParameterCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        ((GeneratorParameter)item).PropertyChanged += GeneratorParameterCollection_PropertyChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        ((GeneratorParameter)item).PropertyChanged -= GeneratorParameterCollection_PropertyChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (var item in e.OldItems)
                    {
                        ((GeneratorParameter)item).PropertyChanged -= GeneratorParameterCollection_PropertyChanged;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// If any of the properties of the Items in the collections change, then we will trigger the PropertyChanged event for the NiceString property.
        /// This way we can bind to NiceString in the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GeneratorParameterCollection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("NiceString"));
        }

        internal GeneratorParameterCollection Clone()
        {
            var paramCollection = new GeneratorParameterCollection();
            foreach (var c in this.Items)
                paramCollection.Add(c.Clone());
            return paramCollection;
        }

   
        public void ReadXml(XElement xe)
        {
            foreach (var genElement in xe.Descendants("GeneratorParameter"))
            {
                GeneratorParameter p2 = new GeneratorParameter();
                p2.ReadXml(genElement);
                Items.Add(p2);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("GeneratorParameters");
            foreach (var item in Items)
            {
                item.WriteXml(writer);
            }

            writer.WriteEndElement();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be casted return false:
            GeneratorParameterCollection p = obj as GeneratorParameterCollection;
            if (p == null)
                return false;

            // Return true if the fields match:
            return GetHashCode() == p.GetHashCode();
        }

        public bool Equals(GeneratorParameterCollection other)
        {
            return object.Equals(this, other);
        }

        public override int GetHashCode()
        {
            int hc = 0;
            if (Items != null)
                foreach (var p in Items)
                    hc ^= p.GetHashCode();
            return hc;
        }
    }
}
