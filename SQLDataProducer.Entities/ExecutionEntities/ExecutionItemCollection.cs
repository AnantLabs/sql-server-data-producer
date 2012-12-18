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
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    /// <summary>
    /// Collection of Execution Items. To be used to group the execution of ExecutionItems. The collection can have options that each execution should use.
    /// </summary>
    public class ExecutionItemCollection : ObservableCollection<ExecutionItem> 
    {

        public ExecutionItemCollection()
            : base()
        {
            base.CollectionChanged += (sender, e) =>
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("IsContainingData"));
        }

        
        public bool IsContainingData
        {
            get
            {
                if (base.Items == null)
                    return false;
                
                return base.Items.Count > 0;
            }
        }

        string _collectionName;
        /// <summary>
        /// Name of the collection. To give the option to give a friendly name to a group of execution items.
        /// </summary>
        public string CollectionName
        {
            get
            {
                return _collectionName;
            }
            set
            {
                if (_collectionName != value)
                {
                    _collectionName = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("CollectionName"));
                }
            }
        }

        public new void Add(ExecutionItem item)
        {
            item.Order = this.Items.Count + 1;
            item.ParentCollection = this;
            base.Add(item);
        }

        public void AddRange(IEnumerable<ExecutionItem> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void ReadXml(XDocument doc)
        {
            foreach (var ei in doc.Descendants("ExecutionItem"))
            {
                ExecutionItem i = new ExecutionItem();
                i.ReadXml(ei);
                Items.Add(i);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("ExecutionItemCollection");
            foreach (var item in Items)
            {
                item.WriteXml(writer);
            }
            writer.WriteEndElement();
        }
    }
}
