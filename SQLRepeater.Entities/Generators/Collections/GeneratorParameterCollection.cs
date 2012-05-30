using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SQLRepeater.Entities.Generators.Collections
{
    public class GeneratorParameterCollection : ObservableCollection<GeneratorParameter>
    {

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var s in this.Items)
            {
                sb.AppendFormat("{{{0}: {1}}}; ", s.ParameterName, s.Value);
            }
            return sb.ToString();
        }


        public string NiceString
        {
            get { return this.ToString(); }
        }

        
        public GeneratorParameterCollection() 
            : base()
        {
            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(GeneratorParameterCollection_CollectionChanged);
            
        }

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
                    break;
                default:
                    break;
            }
        }

        void GeneratorParameterCollection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("NiceString"));
        }
    }
}
