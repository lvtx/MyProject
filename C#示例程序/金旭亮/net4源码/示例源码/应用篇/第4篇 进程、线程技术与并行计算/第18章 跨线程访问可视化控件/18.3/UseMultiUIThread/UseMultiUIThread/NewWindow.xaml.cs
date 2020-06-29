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
using System.Threading;

namespace UseMultiUIThread
{
    /// <summary>
    /// NewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewWindow : Window
    {
        public NewWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtInfo.Text = "当前线程:" + Thread.CurrentThread.ManagedThreadId.ToString();
        }

       

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //如果不加入这句，又不在其他地方调用Dispatcher.InvokeShutdown()方法，将导致长命百岁的线程
            this.Dispatcher.InvokeShutdown();
        }
    }
}
