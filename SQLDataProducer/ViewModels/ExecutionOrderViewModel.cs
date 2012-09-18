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
using SQLDataProducer.DataAccess.Factories;

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

        public DelegateCommand<TableEntity> CreateTreeWithTableTableAsRootCommand { get; private set; }
        public DelegateCommand<TableEntity> CreateTreeWithTableAsLeafCommand { get; private set; }
        
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

        public ExecutionOrderViewModel(SQLDataProducer.Model.ApplicationModel model)
        {
            Model = model;

            MoveItemRightCommand = new DelegateCommand(AddSelectedItemToExecutionItemList);
            MoveAllItemsRightCommand = new DelegateCommand(AddAllTablesToExecutionItemList);
            MoveAllItemsLeftCommand = new DelegateCommand(ClearExecutionItemList);
            MoveItemUpCommand = new DelegateCommand(MoveSelectedExecutionItemUp);
            MoveItemDownCommand = new DelegateCommand(MoveSelectedExecutionItemDown);
            MoveItemLeftCommand = new DelegateCommand(RemoveSelectedExecutionItemFromExecutionItemList);
            CloneExecutionItemCommand = new DelegateCommand<ExecutionItem>(CloneSelectedExecutionItem);
            ClearSearchCriteraCommand = new DelegateCommand(ClearSearchCriteriaValue);
            MoveUpWithTheSelectorCommand = new DelegateCommand(MoveTheTableSelectorUp);
            MoveDownWithTheSelectorCommand = new DelegateCommand(MoveTheTableSelectorDown);
            CreateTreeWithTableTableAsRootCommand = new DelegateCommand<TableEntity>(CreateTreeWithTableAsRoot);
            CreateTreeWithTableAsLeafCommand = new DelegateCommand<TableEntity>(CreateTreeWithTableAsLeaf);
        }

        private void CreateTreeWithTableAsLeaf(TableEntity table)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Model.ConnectionString);
            IEnumerable<TableEntity> tables = tda.GetTreeStructureWithTableAsLeaf(table, Model.Tables);

            AddExecutionItem(tables);
        }

        private void CreateTreeWithTableAsRoot(TableEntity table)
        {
            TableEntityDataAccess tda = new TableEntityDataAccess(Model.ConnectionString);
            IEnumerable<TableEntity> tables = tda.GetTreeStructureFromRoot(table, Model.Tables);

            AddExecutionItem(tables);
        }

        private void MoveTheTableSelectorDown()
        {
            Model.TablesView.MoveCurrentToNext();
            Model.SelectedTable = Model.TablesView.CurrentItem as TableEntity;
        }

        private void MoveTheTableSelectorUp()
        {
            Model.TablesView.MoveCurrentToPrevious();
            Model.SelectedTable = Model.TablesView.CurrentItem as TableEntity;
        }

        private void ClearSearchCriteriaValue()
        {
            Model.SearchCriteria = string.Empty;
        }

        private void CloneSelectedExecutionItem(ExecutionItem item)
        {
            if (item == null)
                return;

            CloneExecutionItemWindow window = new CloneExecutionItemWindow(clones =>
            {
                for (int i = 0; i < clones; i++)
                {
                    Model.ExecutionItems.Add(item.Clone());
                }
            });
            window.Show();
        }

        private void RemoveSelectedExecutionItemFromExecutionItemList()
        {
            if (Model.SelectedExecutionItem != null)
            {
                Model.ExecutionItems.Remove(Model.SelectedExecutionItem);
                for (int i = 0; i < Model.ExecutionItems.Count; i++)
                {
                    Model.ExecutionItems[i].Order = i + 1;
                }
            }
        }

        private void MoveSelectedExecutionItemDown()
        {
            int currentIndex = Model.ExecutionItems.IndexOf(Model.SelectedExecutionItem);
            int newIndex = Model.ExecutionItems.IndexOf(Model.SelectedExecutionItem) + 1;
            if (newIndex >= Model.ExecutionItems.Count)
                return;

            Model.ExecutionItems[newIndex].Order--;
            Model.SelectedExecutionItem.Order++;

            Model.ExecutionItems.Move(currentIndex, newIndex);
        }

        private void MoveSelectedExecutionItemUp()
        {
            int currentIndex = Model.ExecutionItems.IndexOf(Model.SelectedExecutionItem);
            int newIndex = Model.ExecutionItems.IndexOf(Model.SelectedExecutionItem) - 1;
            if (newIndex < 0)
                return;

            Model.ExecutionItems[newIndex].Order++;
            Model.SelectedExecutionItem.Order--;

            Model.ExecutionItems.Move(currentIndex, newIndex);
        }

        private void ClearExecutionItemList()
        {
            Model.ExecutionItems.Clear();
        }

        private void AddExecutionItem(TableEntity table)
        {
            // Clone the selected table so that each generation of that table is configurable uniquely
            ExecutionItemFactory factory = new ExecutionItemFactory(Model.ConnectionString);
            Model.ExecutionItems.Add(factory.CloneFromTable(table));
        }
        
        private void AddExecutionItem(IEnumerable<TableEntity> tables)
        {
            ExecutionItemFactory factory = new ExecutionItemFactory(Model.ConnectionString);
            Model.ExecutionItems.AddRange(factory.GetClones(tables));
        }

        private void AddExecutionItem(IEnumerable<ExecutionItem> eiItems)
        {
            Model.ExecutionItems.AddRange(eiItems);
        }

        private void AddSelectedItemToExecutionItemList()
        {
            if (Model.SelectedTable != null)
            {
                AddExecutionItem(Model.SelectedTable);
            }
        }

        private void AddAllTablesToExecutionItemList()
        {
            if (Model.Tables == null)
                return;

            foreach (TableEntity tabl in Model.TablesView)
            {
                AddExecutionItem(tabl);
            }
        }
    }
}
