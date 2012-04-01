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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLRepeater.Controls.GeneratorConfigurationControls
{
    /// <summary>
    /// Interaction logic for DateTimeConfiguration.xaml
    /// </summary>
    public partial class DateTimeConfiguration : UserControl
    {
        private Entities.ValueGeneratorParameters.DateTimeParameter dateTimeParameter;

        public DateTimeConfiguration(Entities.ValueGeneratorParameters.DateTimeParameter dateTimeParameter)
        {
            this.DataContext = null;
            this.dateTimeParameter = dateTimeParameter;
            Loaded += new RoutedEventHandler(DateTimeConfiguration_Loaded);
            InitializeComponent();
        }

        void DateTimeConfiguration_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = dateTimeParameter;
        }
    }
}
