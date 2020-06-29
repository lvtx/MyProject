using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;

namespace UseSilverlightDispatcher
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private Thread curThread = null;
        private void btnStartThread_Click(object sender, RoutedEventArgs e)
        {
            curThread = new Thread(ThreadFunction);
            curThread.IsBackground = true;
            curThread.Start();
            btnStartThread.IsEnabled = false;
            btnStopThread.IsEnabled = true;
        }

        private int counter = 0;
        private volatile bool IsCancel = false;
        private void ThreadFunction()
        {
            for (; ; )
            {

                counter++;
                string info = "计数器值：" + counter.ToString();
                //停1秒钟
                Thread.Sleep(1000);
                //更新UI的委托
                Action<string> updateUIDelegate = delegate(string information)
                {
                    txtInfo.Text = information;
                };

                if (IsCancel)
                {
                    info = "线程已终止";
                    //在UI线程中显示信息
                    this.Dispatcher.BeginInvoke(updateUIDelegate,  new object[] { info });
                    //也可以通过Deployment对象获取对当前UI线程Dispatcher对象的引用。
                    //Deployment.Current.Dispatcher.BeginInvoke(updateUIDelegate,  new object[] { info });
                    IsCancel = false; //重置标记

                    return;
                }
                else
                    //在UI线程中显示信息
                    this.Dispatcher.BeginInvoke(updateUIDelegate, new object[] { info });

            }
            
        }
        private void btnStopThread_Click(object sender, RoutedEventArgs e)
        {
            IsCancel = true;
            btnStartThread.IsEnabled = true;
            btnStopThread.IsEnabled = false;
        }
    }
}
