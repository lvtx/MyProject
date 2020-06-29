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

namespace UseXAMLAndCodeToCreateWPFWindow
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

        private int Counter = 0;
        private void btnClickMe_Click(object sender, RoutedEventArgs e)
        {
            Counter++;
            btnClickMe.Content = string.Format("我被单击了{0}次", Counter);
        }
    }
}
