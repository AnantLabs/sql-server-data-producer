// Copyright 2012-2013 Peter Henell

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
using System.Windows;
using System.Windows.Controls;

namespace ConnectionStringCreatorGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConnectionStringBuilderWindow : Window
    {
        ConnectionStringBuilderWindowViewModel _viewModel;

        public ConnectionStringBuilderWindow(ConnectionStringCreatorGUI.SqlConnectionString connStr, Action<SqlConnectionString> action)
        {
            SqlConnectionString con = new SqlConnectionString();
            InitializeComponent();
            _viewModel = new ConnectionStringBuilderWindowViewModel(connStr, action);

            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        public ConnectionStringBuilderWindow(ConnectionStringCreatorGUI.SqlConnectionString connStr
            , Control[] content                                    
            , Action<SqlConnectionString> action) : this(connStr, action)
            
        {
            foreach (var item in content)
            {
                ContentStackPanel.Children.Add(item);
            }
            
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _viewModel;
            this.Activate(); // To make this window focused
        }
    }
}
