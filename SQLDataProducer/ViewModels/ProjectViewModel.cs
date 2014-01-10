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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Model;

namespace SQLDataProducer.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        DatabaseTablesViewModel databaseTablesViewModel;
        public DatabaseTablesViewModel DatabaseTablesViewModel
        {
            get { return databaseTablesViewModel; }
            set { 
                databaseTablesViewModel = value;
                OnPropertyChanged("DatabaseTablesViewModel");
            }
        }

        ExecutionNodeViewModel executionNodeViewModel;
        public ExecutionNodeViewModel ExecutionNodeViewModel
        {
            get { return executionNodeViewModel; }
            set
            {
                executionNodeViewModel = value;
                OnPropertyChanged("ExecutionNodeViewModel");
            }
        }

        ColumnDetailsViewModel columnDetailsViewModel;
        public ColumnDetailsViewModel ColumnDetailsViewModel
        {
            get { return columnDetailsViewModel; }
            set
            {
                columnDetailsViewModel = value;
                OnPropertyChanged("ColumnDetailsViewModel");
            }
        }

        TableDetailsViewModel tableDetailsViewModel;
        public TableDetailsViewModel TableDetailsViewModel
        {
            get { return tableDetailsViewModel; }
            set
            {
                tableDetailsViewModel = value;
                OnPropertyChanged("TableDetailsViewModel");
            }
        }

        public ProjectViewModel(ProjectModel model)
            : base(model)
        {
            DatabaseTablesViewModel = new DatabaseTablesViewModel(model);
            TableDetailsViewModel = new ViewModels.TableDetailsViewModel(model);
            ColumnDetailsViewModel = new ViewModels.ColumnDetailsViewModel(model);
            ExecutionNodeViewModel = new ExecutionNodeViewModel(model);

        }

    }
}
