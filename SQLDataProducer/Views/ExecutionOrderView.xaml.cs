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

using System.Windows.Controls;
using SQLDataProducer.ViewModels;
using System;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.Views
{
    /// <summary>
    /// Interaction logic for ExecutionOrderView.xaml
    /// </summary>
    public partial class ExecutionOrderView : UserControl
    {
        //ExecutionOrderViewModel vm;
        //public System.ComponentModel.ICollectionView DefaultView;

        public ExecutionOrderView()
        {
            InitializeComponent();

          //  Loaded += new System.Windows.RoutedEventHandler(ExecutionOrderView_Loaded);
        }

        //void ExecutionOrderView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    vm = (ExecutionOrderViewModel)this.DataContext;
        //    DefaultView = System.Windows.Data.CollectionViewSource.GetDefaultView(vm.Model.Tables);
        //}

        //private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    DefaultView.Refresh();
        //}
    }

    //public class TextSearchFilter
    //{
    //    public TextSearchFilter(
    //        System.ComponentModel.ICollectionView filteredView,
    //        TextBox textBox)
    //    {
            
    //    }
    //}
}
