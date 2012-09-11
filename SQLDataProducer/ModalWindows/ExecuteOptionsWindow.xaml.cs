using System.Windows;
using SQLDataProducer.Model;

namespace SQLDataProducer.ModalWindows
{
    /// <summary>
    /// Interaction logic for ExecuteOptionsWindow.xaml
    /// </summary>
    public partial class ExecuteOptionsWindow : Window
    {
        ApplicationModel _model;
        public ExecuteOptionsWindow(ApplicationModel model)
        {
            _model = model;
            Loaded += new RoutedEventHandler(ExecuteOptionsWindow_Loaded);
            InitializeComponent();
        }

        void ExecuteOptionsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _model; 
        }
    }
}
