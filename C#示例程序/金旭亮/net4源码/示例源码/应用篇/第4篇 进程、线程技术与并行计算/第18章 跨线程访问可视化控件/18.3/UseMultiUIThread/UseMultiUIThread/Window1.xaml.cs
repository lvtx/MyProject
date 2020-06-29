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
using System.Threading;
using System.Windows.Threading;

namespace UseMultiUIThread
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

        private void btnNewWindow_Click(object sender, RoutedEventArgs e)
        {

            Thread th = new Thread(CreateNewWindow);
            th.SetApartmentState(ApartmentState.STA);
            //th.IsBackground = true; //加入这句，当主窗体退出时，从窗体自动关闭。
            th.Start();
        }

        List<Dispatcher> dispatchers = new List<Dispatcher>();

        private void CreateNewWindow()
        {
            NewWindow win = new NewWindow();
          
            win.Show();

            dispatchers.Add(win.Dispatcher);
            //启动消息循环
            System.Windows.Threading.Dispatcher.Run();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (chkCloseOtherWindow.IsChecked == true)
            {
                foreach (Dispatcher obj in dispatchers)
                {
                    obj.BeginInvokeShutdown(DispatcherPriority.Normal);
                }
            }
        }
    }
}
