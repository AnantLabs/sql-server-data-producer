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

using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using SQLDataProducer.DatabaseEntities.Entities;

namespace SQLDataProducer.Entities.DatabaseEntities.Collections
{
    public class TableEntityCollection : ObservableCollection<TableEntity>
    {
        public TableEntityCollection()
            : base()
        {
             base.CollectionChanged += (sender, e) =>
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("IsContainingData"));
        }

        public TableEntityCollection(IEnumerable<TableEntity> tables)
            : base(tables)
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


    }
}
