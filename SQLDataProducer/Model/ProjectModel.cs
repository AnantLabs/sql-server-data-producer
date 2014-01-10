using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.Model
{
    public class ProjectModel : NotifyingObject
    {
        private ObservableCollection<TableEntity> tables = new ObservableCollection<TableEntity>();
        public ObservableCollection<TableEntity> Tables
        {
            get { return tables; }
            set
            {
                tables = value;
                OnPropertyChanged("Tables");
            }
        }

        public ProjectModel()
        {

        }
    }
}
