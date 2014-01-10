using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.ExecutionEntities;

namespace SQLDataProducer.GUI.Model
{
    public class ProjectModel : GalaSoft.MvvmLight.ObservableObject
    {
        private ObservableCollection<TableEntity> _tables = new ObservableCollection<TableEntity>();
        public ObservableCollection<TableEntity> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                RaisePropertyChanged("Tables");
            }
        }

        /// <summary>
        /// The <see cref="RootNode" /> property's name.
        /// </summary>
        public const string RootNodePropertyName = "RootNode";
        private ExecutionNode _exectionNode = ExecutionNode.CreateLevelOneNode(1, "Root");

        /// <summary>
        /// Sets and gets the RootNode property.
        /// </summary>
        public ExecutionNode RootNode
        {
            get
            {
                return _exectionNode;
            }

            set
            {
                if (_exectionNode == value)
                {
                    return;
                }

                RaisePropertyChanging(RootNodePropertyName);
                _exectionNode = value;
                RaisePropertyChanged(RootNodePropertyName);
            }
        }

        public ProjectModel()
        {
           
        }
    }
}
