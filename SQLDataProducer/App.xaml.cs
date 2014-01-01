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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SQLDataProducer.Entities.OptionEntities;

namespace SQLDataProducer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        ExecutionTaskOptions _options;

        public App()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += (sender, e) =>
                MessageBox.Show(e.ExceptionObject.ToString());

            this.Startup += new StartupEventHandler(App_Startup);
            this.Exit += new ExitEventHandler(App_Exit);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            _options = (ExecutionTaskOptions)SQLDataProducerSettings.Default.ExecutionOptions;
            if (_options == null)
                _options = new ExecutionTaskOptions();

            this.MainWindow = new MainWindow(_options);
            this.MainWindow.Show();
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            SQLDataProducerSettings.Default.ExecutionOptions = _options;
            SQLDataProducerSettings.Default.Save();
        }
    }
}
