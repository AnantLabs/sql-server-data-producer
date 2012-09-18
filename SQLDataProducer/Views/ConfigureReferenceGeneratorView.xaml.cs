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

namespace SQLDataProducer.Views
{
    /// <summary>
    /// Interaction logic for ConfigureReferenceGeneratorView.xaml
    /// </summary>
    public partial class ConfigureReferenceGeneratorView : UserControl
    {
        private ViewModels.ConfigureReferenceGeneratorViewModel configureReferenceGeneratorViewModel;

        public ConfigureReferenceGeneratorView(ViewModels.ConfigureReferenceGeneratorViewModel configureReferenceGeneratorViewModel)
        {
            this.configureReferenceGeneratorViewModel = configureReferenceGeneratorViewModel;
            
            Loaded += new RoutedEventHandler(ConfigureReferenceGeneratorView_Loaded);
            InitializeComponent();
        }

        void ConfigureReferenceGeneratorView_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = configureReferenceGeneratorViewModel;
        }


    }
}
