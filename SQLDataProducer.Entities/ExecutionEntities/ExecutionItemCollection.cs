using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

namespace SQLDataProducer.Entities.ExecutionEntities
{
    /// <summary>
    /// Collection of Execution Items. To be used to group the execution of ExecutionItems. The collection can have options that each execution should use.
    /// </summary>
    public class ExecutionItemCollection : ObservableCollection<ExecutionItem>, IXmlSerializable
    {

        public ExecutionItemCollection()
            : base()
        {
            
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

               
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.MoveToContent() == System.Xml.XmlNodeType.Element && reader.LocalName == "ExecutionItemCollection")
            {
                while (reader.ReadToFollowing("ExecutionItem"))
                {
                    ExecutionItem i = new ExecutionItem();
                    i.ReadXml(reader);
                    Items.Add(i);

                }

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
