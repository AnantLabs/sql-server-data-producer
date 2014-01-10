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
    /// Interaction logic for ExecutionsOptionsView.xaml
    /// </summary>
    public partial class ExecutionsOptionsView : UserControl
    {
        private ViewModels.ExecutionOptionsViewModel opVM;

        public ExecutionsOptionsView(ViewModels.ExecutionOptionsViewModel opVM)
        {
            this.opVM = opVM;
            Loaded += new RoutedEventHandler(ExecutionsOptionsView_Loaded);
            InitializeComponent();
        }

        void ExecutionsOptionsView_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = opVM;
        }
    }
}
