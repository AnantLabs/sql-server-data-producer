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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using SQLDataProducer.Entities.DatabaseEntities;


namespace SQLDataProducer.ModalWindows
{
    /// <summary>
    /// Interaction logic for ShowPreviewWindow.xaml
    /// </summary>
    public partial class ShowPreviewWindow : Window
    {
        //DataRowSet _dt;
        Object _dt;
        public ShowPreviewWindow(Object dt)
        {
            _dt = dt;
            Loaded += new RoutedEventHandler(ShowPreviewWindow_Loaded);
            InitializeComponent();
        }

        void ShowPreviewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _dt;   
        }
    }
}
