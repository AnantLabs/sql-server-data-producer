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
        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
            this.Exit += new ExitEventHandler(App_Exit);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            ExecutionTaskOptionsManager.Instance.Options =
                (ExecutionTaskOptions)SQLRepeaterSettings.Default.ExecutionOptions;
            if (ExecutionTaskOptionsManager.Instance.Options == null)
            {
                ExecutionTaskOptionsManager.Instance.Options = new ExecutionTaskOptions();
            }
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            SQLRepeaterSettings.Default.ExecutionOptions = ExecutionTaskOptionsManager.Instance.Options;
            SQLRepeaterSettings.Default.Save();
        }
    }
}
