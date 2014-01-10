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

using System.ComponentModel;
using System.Windows;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.ViewModels;
//using SQLDataProducer.ViewModels;

namespace SQLDataProducer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ProjectViewModel projectViewMode;

        public ProjectViewModel ProjectViewModel
        {
            get { return projectViewMode; }
            set { 
                projectViewMode = value;
                
            }
        }

        ExecutionTaskOptions _options;
        public MainWindow(ExecutionTaskOptions options)
        {
            InitializeComponent();
            _options = options;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            projectViewMode = new ProjectViewModel(new Model.ProjectModel());
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
