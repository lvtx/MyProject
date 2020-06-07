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

namespace UseValueConverter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitDataBinding();

        }

        private void InitDataBinding()
        {
            List<MyDataItem> items = new List<MyDataItem>();

            MyDataItem obj = new MyDataItem();
            obj.FileLength = 120;
            items.Add(obj);

            obj = new MyDataItem();
            obj.FileLength = 10240;
            items.Add(obj);

            obj = new MyDataItem();
            obj.FileLength = 40968000;
            items.Add(obj);
            obj = new MyDataItem();

            obj.FileLength = 2048500000;
            items.Add(obj);

            lstItems.ItemsSource = items;
            lstItems.SelectedIndex = 0;
        }
    }
}
