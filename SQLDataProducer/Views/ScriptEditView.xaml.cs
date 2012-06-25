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
using SQLDataProducer.ViewModels;

namespace SQLDataProducer.Views
{
    /// <summary>
    /// Interaction logic for ScriptEditView.xaml
    /// </summary>
    public partial class ScriptEditView : UserControl
    {
        private ScriptEditViewModel _viewModel;
        public ScriptEditView(ScriptEditViewModel vm)
        {
            _viewModel = vm;
            Loaded += new RoutedEventHandler(ScriptEditView_Loaded);
            InitializeComponent();
        }

        void ScriptEditView_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _viewModel;
            
        }
    }
}
