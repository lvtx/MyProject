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

namespace UseUpdateSourceTrigger
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MyData dataobj1 = new MyData();
            StackPanel1.DataContext = dataobj1;
            MyData databoj2 = new MyData();
            StackPanel2.DataContext = databoj2;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression be = this.txtInput2.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        }
    }
}
