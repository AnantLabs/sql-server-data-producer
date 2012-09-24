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
using SQLDataProducer.ViewModels;

namespace SQLDataProducer.ModalWindows
{
    /// <summary>
    /// Interaction logic for YesNoWindow.xaml
    /// </summary>
    public partial class YesNoWindow : Window
    {
        IYesNoViewModel _vm;
        Control _content;
        bool _callBackRan = false;

        public YesNoWindow(Control content, IYesNoViewModel vm)
        {
            _content = content;
            _vm = vm; ;

            Loaded += new RoutedEventHandler(YesNoWindow_Loaded);
            Closing += new System.ComponentModel.CancelEventHandler(YesNoWindow_Closing);
            InitializeComponent();
        }

        void YesNoWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_callBackRan)
                _vm.OnNo();
            _callBackRan = true;
        }

        void YesNoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            contentPlaceHolder.Child = _content;
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_callBackRan)
                _vm.OnYes();
            _callBackRan = true;
            this.Close();
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_callBackRan)
                _vm.OnNo();
            _callBackRan = true;
            this.Close();
        }

     

        
    }
}
