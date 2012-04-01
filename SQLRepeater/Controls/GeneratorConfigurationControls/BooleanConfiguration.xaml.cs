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
    /// Interaction logic for BooleanConfiguration.xaml
    /// </summary>
    public partial class BooleanConfiguration : UserControl
    {
        private Entities.ValueGeneratorParameters.BooleanParameter booleanParameter;


        public BooleanConfiguration(Entities.ValueGeneratorParameters.BooleanParameter booleanParameter)
        {
            this.DataContext = null;
            this.booleanParameter = booleanParameter;
            Loaded += new RoutedEventHandler(BooleanConfiguration_Loaded);
            InitializeComponent();
        }

        void BooleanConfiguration_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = booleanParameter;
        }
    }
}
