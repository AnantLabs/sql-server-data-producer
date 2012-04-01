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
    /// Interaction logic for DecimalConfiguration.xaml
    /// </summary>
    public partial class DecimalConfiguration : UserControl
    {
        private Entities.ValueGeneratorParameters.DecimalParameter decimalParameter;

        public DecimalConfiguration(Entities.ValueGeneratorParameters.DecimalParameter decimalParameter)
        {
            this.DataContext = null;
            this.decimalParameter = decimalParameter;
            Loaded += new RoutedEventHandler(DecimalConfiguration_Loaded);
            InitializeComponent();
        }

        void DecimalConfiguration_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = decimalParameter;
        }
    }
}
