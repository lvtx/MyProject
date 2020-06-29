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
using System.Windows.Threading;

namespace CheckAccess
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
        private void ButtonTrue_Click(object sender, RoutedEventArgs e)
        {
            MyCheckAccess();
        }

        private void ButtonFalse_Click(object sender, RoutedEventArgs e)
        {
            // 通过委托在线程池中的线程上访问UI控件
            Action del = MyCheckAccess;
            del.BeginInvoke(null, null);
        }


        private void MyCheckAccess()
        {
            //是否在UI线程中调用？
            if (txtResult.Dispatcher.CheckAccess())
            {
                SetResultText("True");
            }
            else
            {

                // 不是在UI线程中调用SetResultText方法，则需要通过Dispatcher
                //来访问UI控件
                txtResult.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action<string>(SetResultText),
                    "False");
            }
        }


        private void SetResultText(string result)
        {
            txtResult.Text = result;
        }
    }
}
