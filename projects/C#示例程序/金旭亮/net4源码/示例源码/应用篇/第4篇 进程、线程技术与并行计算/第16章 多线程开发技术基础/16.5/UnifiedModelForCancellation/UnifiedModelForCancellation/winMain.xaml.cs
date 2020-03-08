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

namespace UnifiedModelForCancellation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();


            InitDemo();

        }
        /// <summary>
        /// 初始化演示的相关参数
        /// </summary>
        private void InitDemo()
        {
            threadObjs = new ObservableCollection<ThreadObject>();
            lstThreads.ItemsSource = threadObjs;
          
            tokenSource = new CancellationTokenSource();

            btnNewThread.IsEnabled = true;
            btnCancelThread.IsEnabled = false;
        }

        ObservableCollection<ThreadObject> threadObjs = null;
       
      
        CancellationTokenSource tokenSource = null;

        private void btnNewThread_Click(object sender, RoutedEventArgs e)
        {
           ThreadObject obj=new ThreadObject(tokenSource.Token);
            threadObjs.Add(obj);
            Thread th = new Thread(obj.DoWork);
            th.IsBackground = true;
            th.Start();
            btnCancelThread.IsEnabled = true;
        }

        private void btnCancelThread_Click(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
            btnCancelThread.IsEnabled = false;
            btnNewThread.IsEnabled = false;
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            InitDemo();
        }
    }
}
