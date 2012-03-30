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

namespace SQLRepeater
{
    /// <summary>
    /// Interaction logic for ValueEntityConfigurationView.xaml
    /// </summary>
    public partial class ColumnEntityValueConfigurationView : Window
    {
        private ColumnEntityValueConfigurationViewModel _viewModel;
        public ColumnEntityValueConfigurationView(ColumnEntityValueConfigurationViewModel vm)
        {
            _viewModel = vm;
            Loaded += new RoutedEventHandler(ValueEntityConfigurationView_Loaded);
            InitializeComponent();
        }

        void ValueEntityConfigurationView_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _viewModel;
        }
    }
}
