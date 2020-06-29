using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ResponseToDataBindingEvents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyData DataItem = null;
        ObservableCollection<String> messages = null;
        public MainWindow()
        {
            InitializeComponent();

            DataItem = new MyData()
            {
                Information="数据源对象的Information属性"
            };

            DataContext = DataItem;

            messages = new ObservableCollection<string>();
            lstMessage.ItemsSource = messages;
            
        }

        private void Container_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            String message = String.Format("TargetUpdated : Information={0}", 
                DataItem.Information);
            messages.Add(message);
        }

        private void Container_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            String message = String.Format("SourceUpdated : Information={0}", 
                DataItem.Information);
            messages.Add(message);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            messages.Clear();
        }
    }
}
