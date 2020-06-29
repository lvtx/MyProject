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

namespace ValidateValueCallbackExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

  Test obj = new Test();
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int number = Convert.ToInt32(txtNum.Text);
                obj.MyIntProperty = number;
                MessageBox.Show("MyIntProperty当前值=" + obj.MyIntProperty.ToString());
            }
            catch (ArgumentException)
            {
                MessageBox.Show("必须输入一个大于等于0的整数,MyIntProperty当前值="+obj.MyIntProperty.ToString());
                txtNum.Focus();
            }
            catch (FormatException)
            {
                MessageBox.Show("输入的不是一个有效的数字");
                txtNum.Focus();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
    }
}
