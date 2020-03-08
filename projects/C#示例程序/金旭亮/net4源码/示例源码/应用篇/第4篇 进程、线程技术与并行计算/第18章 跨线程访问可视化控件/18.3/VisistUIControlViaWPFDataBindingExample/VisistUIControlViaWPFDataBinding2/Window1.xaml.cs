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
using System.Collections.ObjectModel;
using System.Threading;

namespace VisistUIControlViaWPFDataBinding2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            data = new ObservableCollection<MyDataObject>();
            lstItems.ItemsSource = data ;

        }

        ObservableCollection<MyDataObject> data = null;

        private int ObjectCounter=0;

        private void btnAddObject_Click(object sender, RoutedEventArgs e)
        {
            ObjectCounter++;
            data.Add(new MyDataObject { Value = ObjectCounter });
        }

        private void btnDeleteObject_Click(object sender, RoutedEventArgs e)
        {
            if (lstItems.SelectedIndex != -1)
                data.RemoveAt(lstItems.SelectedIndex);
        }

        private void btnIncreaseValue_Click(object sender, RoutedEventArgs e)
        { 
            MyDataObject obj = lstItems.SelectedItem as MyDataObject;
            ThreadStart threadFunc = delegate()
            {
              
                if (obj != null)
                    obj.Value++;
            };
            Thread th = new Thread(threadFunc);
            th.IsBackground = true;
            th.Start();
        }
    }
}
