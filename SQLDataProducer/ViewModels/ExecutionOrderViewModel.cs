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

using System.Collections.ObjectModel;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.DatabaseEntities.Entities;
using SQLDataProducer.DataAccess;
using SQLDataProducer.ModalWindows;
using System.Linq;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using System;
using System.Collections.Generic;

namespace SQLDataProducer.ViewModels
{
    public class ExecutionOrderViewModel : ViewModelBase
    {

        public DelegateCommand MoveItemUpCommand { get; private set; }
        public DelegateCommand MoveItemLeftCommand { get; private set; }
        public DelegateCommand MoveItemRightCommand { get; private set; }
        public DelegateCommand MoveAllItemsRightCommand { get; private set; }
        public DelegateCommand MoveAllItemsLeftCommand { get; private set; }
        public DelegateCommand MoveItemDownCommand { get; private set; }

        public DelegateCommand CreateTreeWithTableTableAsRootCommand { get; private set; }
        public DelegateCommand CreateTreeWithTableAsLeafCommand { get; private set; }
        
        public DelegateCommand<ExecutionItem> CloneExecutionItemCommand { get; private set; }

        public DelegateCommand ClearSearchCriteraCommand { get; private set; }
        public DelegateCommand MoveUpWithTheSelectorCommand { get; private set; }
        public DelegateCommand MoveDownWithTheSelectorCommand { get; private set; }

        

        SQLDataProducer.Model.ApplicationModel _model;
        public SQLDataProducer.Model.ApplicationModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged("Model");
                }
            }
        }

        ExecutionDetailsViewModel _currentExecutionDetailVM;
        public ExecutionDetailsViewModel SelectedExecutionDetailVM
        {
            get
            {
                return _currentExecutionDetailVM;
            }
            set
            {
                if (_currentExecutionDetailVM != value)
                {
                    _currentExecutionDetailVM = value;
                    OnPropertyChanged("SelectedExecutionDetailVM");
                }
            }
        }


        ObservableCollection<ExecutionDetailsViewModel> _detailsVM;
        public ObservableCollection<ExecutionDetailsViewModel> DetailsVM
        {
            get
            {
                return _detailsVM;
            }
            set
            {
                if (_detailsVM != value)
                {
                    _detailsVM = value;
                    OnPropertyChanged("DetailsVM");
                }
            }
        }


        


        
        

        public ExecutionOrderViewModel(SQLDataProducer.Model.ApplicationModel model)
        {
            Model = model;

            MoveItemRightCommand = new DelegateCommand(() =>
                {
                    if (Model.SelectedTable != null)
                    {
                        AddExecutionItem(Model.SelectedTable);
                    }
                });

            MoveAllItemsRightCommand = new DelegateCommand(() =>
                {
                    if (Model.Tables == null)
                        return;

                    foreach (TableEntity tabl in Model.TablesView)
                    {
                        AddExecutionItem(tabl);
                    }
                });

            MoveAllItemsLeftCommand = new DelegateCommand(() =>
                {
                    Model.ExecutionItems.Clear();
                });

            MoveItemUpCommand = new DelegateCommand(() =>
                {
                    int currentIndex = Model.ExecutionItems.IndexOf(Model.SelectedExecutionItem);
                    int newIndex = Model.ExecutionItems.IndexOf(Model.SelectedExecutionItem) - 1;
                    if (newIndex < 0)
                        return;

                    Model.ExecutionItems[newIndex].Order++;
                    Model.SelectedExecutionItem.Order--;

                    Model.ExecutionItems.Move(currentIndex, newIndex);
                });

            MoveItemDownCommand = new DelegateCommand(() =>
                {
                    int currentIndex = Model.ExecutionItems.IndexOf(Model.SelectedExecutionItem);
                    int newIndex = Model.ExecutionItems.IndexOf(Model.SelectedExecutionItem) + 1;
                    if (newIndex >= Model.ExecutionItems.Count)
                        return;

                    Model.ExecutionItems[newIndex].Order--;
                    Model.SelectedExecutionItem.Order++;

                    Model.ExecutionItems.Move(currentIndex, newIndex);
                });

            MoveItemLeftCommand = new DelegateCommand(() =>
                {
                    if (Model.SelectedExecutionItem != null)
                    {
                        Model.ExecutionItems.Remove(Model.SelectedExecutionItem);
                        for (int i = 0; i < Model.ExecutionItems.Count; i++)
                        {
                            Model.ExecutionItems[i].Order = i + 1;
                        }
                    }
                });

            CloneExecutionItemCommand = new DelegateCommand<ExecutionItem>(item =>
                {
                    if (item == null)
                        return;

                    CloneExecutionItemWindow window = new CloneExecutionItemWindow(clones =>
                    {
                        for (int i = 0; i < clones; i++)
                        {
                            Model.ExecutionItems.Add(item.CloneWithOrderNumber(Model.ExecutionItems.Count + 1));
                        }
                    });
                    window.Show();
                });


            ClearSearchCriteraCommand = new DelegateCommand(() =>
                {
                    Model.SearchCriteria = string.Empty;
                });
            MoveUpWithTheSelectorCommand = new DelegateCommand(() =>
                {
                    Model.TablesView.MoveCurrentToPrevious();
                    Model.SelectedTable = Model.TablesView.CurrentItem as TableEntity;
                });
            MoveDownWithTheSelectorCommand = new DelegateCommand(() =>
            {
                Model.TablesView.MoveCurrentToNext();
                Model.SelectedTable = Model.TablesView.CurrentItem as TableEntity;
            });

            CreateTreeWithTableTableAsRootCommand = new DelegateCommand(() =>
            {
                TableEntityDataAccess tda = new TableEntityDataAccess(Model.ConnectionString);

                IEnumerable<TableEntity> tables = tda.GetTreeStructureFromRoot(Model.SelectedTable, Model.Tables);

                AddExecutionItem(tables);

            });

            CreateTreeWithTableAsLeafCommand = new DelegateCommand(() =>
                {
                    TableEntityDataAccess tda = new TableEntityDataAccess(Model.ConnectionString);

                    IEnumerable<TableEntity> tables = tda.GetTreeStructureWithTableAsLeaf(Model.SelectedTable, Model.Tables);

                    AddExecutionItem(tables);
                });
        }

        private void AddExecutionItem(TableEntity table)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Model.ConnectionString);
            // Clone the selected table so that each generation of that table is configurable uniquely
            TableEntity clonedTable = tda.CloneTable(table);
            Model.ExecutionItems.Add(new ExecutionItem(clonedTable, Model.ExecutionItems.Count + 1));
        }
        private void AddExecutionItem(IEnumerable<TableEntity> tables)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Model.ConnectionString);
            // Clone the selected table so that each generation of that table is configurable uniquely
            foreach (var table in tables)
            {
                TableEntity clonedTable = tda.CloneTable(table);
                Model.ExecutionItems.Add(new ExecutionItem(clonedTable, Model.ExecutionItems.Count + 1));
            }
        }
        private void AddExecutionItem(IEnumerable<ExecutionItem> eiItems)
        {
            // Clone the selected table so that each generation of that table is configurable uniquely
            foreach (var ei in eiItems)
            {
                Model.ExecutionItems.Add(ei);
            }
        }
    }
}
