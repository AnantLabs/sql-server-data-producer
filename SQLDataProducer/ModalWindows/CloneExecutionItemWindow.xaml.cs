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

namespace SQLDataProducer.ModalWindows
{
    /// <summary>
    /// Interaction logic for CloneExecutionItemWindow.xaml
    /// </summary>
    public partial class CloneExecutionItemWindow : Window
    {
        private int _NumberOfClones = 1;
        public int NumberOfClones
        {
            get
            {
                return _NumberOfClones;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _NumberOfClones = value;
            }
        }

        private Action<int> Callback;
        public CloneExecutionItemWindow(Action<int> callback)
        {
            Callback = callback;
            Loaded += new RoutedEventHandler(CloneExecutionItemWindow_Loaded);
            InitializeComponent();
        }

        void CloneExecutionItemWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Callback != null)
            {
                Callback(NumberOfClones);
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Callback != null)
            {
                Callback(0);
            }
            this.Close();
        }
    }
}
