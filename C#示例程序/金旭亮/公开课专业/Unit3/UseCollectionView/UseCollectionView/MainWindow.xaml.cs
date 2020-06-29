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

namespace UseCollectionView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNavigator_Click(object sender, RoutedEventArgs e)
        {
            NavigatorInCollection win = new NavigatorInCollection();
            win.ShowDialog();
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            SortData win = new SortData();
            win.ShowDialog();
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterData win = new FilterData();
            win.ShowDialog();
        }

        private void btnGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupData win = new GroupData();
            win.ShowDialog();
        }
    }
}
