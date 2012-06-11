using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SQLRepeater.Entities.OptionEntities;

namespace SQLRepeater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        ExecutionTaskOptions _options;

        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
            this.Exit += new ExitEventHandler(App_Exit);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            _options = (ExecutionTaskOptions)SQLRepeaterSettings.Default.ExecutionOptions;
            if (_options == null)
                _options = new ExecutionTaskOptions();

            this.MainWindow = new MainWindow(_options);
            this.MainWindow.Show();
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            SQLRepeaterSettings.Default.ExecutionOptions = _options;
            SQLRepeaterSettings.Default.Save();
        }
    }
}
