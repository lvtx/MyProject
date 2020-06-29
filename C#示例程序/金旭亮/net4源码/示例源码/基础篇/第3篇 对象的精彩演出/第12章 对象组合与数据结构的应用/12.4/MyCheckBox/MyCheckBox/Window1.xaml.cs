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

namespace MyCheckBox
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            txtInfo.Text ="当前状态:"+ Check1.IsChecked.Value.ToString();
        }

        private void Check1_Unchecked(object sender, RoutedEventArgs e)
        {
            txtInfo.Text ="当前状态:"+ Check1.IsChecked.Value.ToString();
        }
    }
}
