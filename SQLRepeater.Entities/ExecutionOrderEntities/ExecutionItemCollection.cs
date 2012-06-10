using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

namespace SQLRepeater.Entities.ExecutionOrderEntities
{
    /// <summary>
    /// Collection of Execution Items. To be used to group the execution of ExecutionItems. The collection can have options that each execution should use.
    /// </summary>
    public class ExecutionItemCollection : ObservableCollection<ExecutionItem>
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

        //public void Save(string fileName)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(ExecutionItemCollection));
        //    using (FileStream stream = new FileStream(fileName, FileMode.Create))
        //    {
        //        serializer.Serialize(stream, this);
        //    }
        //}

        //public static ExecutionItemCollection Load(string fileName)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(ExecutionItemCollection));
        //    using (FileStream stream = new FileStream(fileName, FileMode.Open))
        //    {
        //        return (ExecutionItemCollection)serializer.Deserialize(stream);
        //    }
        //}

        object _executionGroupOptions;
        /// <summary>
        /// Not implemented yet. Options should include: Number of times to execute the ExecutionItems in the collection.
        /// </summary>
        public object ExecutionGroupOptions
        {
            get
            {
                return _executionGroupOptions;
            }
            set
            {
                if (_executionGroupOptions != value)
                {
                    _executionGroupOptions = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("ExecutionGroupOptions"));
                }
            }
        }

    }
}
