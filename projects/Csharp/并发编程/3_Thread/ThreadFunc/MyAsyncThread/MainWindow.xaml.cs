using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyAsyncThread
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
        private void MyAsyncMethod(string MethodName)
        {
            Console.WriteLine($"----------------{MethodName} Start {Thread.CurrentThread.ManagedThreadId.ToString()} {DateTime.Now.ToString()}");
            long lResult = 0;
            for (int i = 0; i < 10000000; i++)
            {
                lResult += 1;
            }
            Thread.Sleep(2000);
            Console.WriteLine($"----------------{MethodName} End {Thread.CurrentThread.ManagedThreadId.ToString()} {lResult} {DateTime.Now.ToString()}");
        }
        private void btnMyAsync_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"-----------------MyAsync_Click Start {Thread.CurrentThread.ManagedThreadId.ToString()} {DateTime.Now.ToString()}");
            Action<string> action = MyAsyncMethod;
            for (int i = 0; i < 5; i++)
            {
                action.BeginInvoke($"MyAsyncMethod_{i}", null, null);
            }
        }

        private void MyAsyncAdvanced_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MyAsyncAdvanced_Click Start {0} {1}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now.ToString());
            Action<string> action = MyAsyncMethod;
            IAsyncResult asyncResult = null;
            AsyncCallback callback = new AsyncCallback((ar) =>
            {
                Console.WriteLine(object.ReferenceEquals(ar, asyncResult));
                Console.WriteLine("计算完成");
            });
            asyncResult = action.BeginInvoke("MyAsyncMethod", callback, null);
            //int i = 0;
            //while (!asyncResult.IsCompleted)
            //{
            //    if (i < 10)
            //    {
            //        Console.WriteLine("任务完成……{0}%", i * 10);
            //        i++;
            //    }
            //    else
            //    {
            //        Console.WriteLine("任务完成……99.9%");
            //    }
            //    Thread.Sleep(200);
            //}
            Thread.Sleep(200);
            Console.WriteLine("------------------------正在搞----------------------");
            Console.WriteLine("------------------------正在搞----------------------");
            Console.WriteLine("------------------------正在搞----------------------");
            Console.WriteLine("------------------------正在搞----------------------");
            asyncResult.AsyncWaitHandle.WaitOne(1000);
            action.EndInvoke(asyncResult);
            Console.WriteLine("任务完成100%");
            Console.WriteLine("MyAsyncAdvanced_Click End {0} {1}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now.ToString());
        }

        public void AddLabelCountThread()
        {
            for (int i = 0; i <= 100; i++)
            {
                Action action = () =>
                {
                    int j = i;
                    lblInfo.Content = String.Format("已完成了{0}%", j);
                };
                lblInfo.Dispatcher.Invoke(action);
                Thread.Sleep(200);
            }
        }

        private void btnShowInfo_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(AddLabelCountThread);
            thread.Start();
        }

        private void btnThread_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("btnThread_Click Start {0} {1}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now.ToString());
            ThreadStart method = () => MyAsyncMethod("MyAsyncMethod");
            Thread thread = new Thread(method);
            thread.Start();
            thread.Join(500);
            Console.WriteLine("已等待500ms");
            while (thread.ThreadState != ThreadState.Stopped)
            {
                Thread.Sleep(100);
                Console.WriteLine("已等待100ms");
            }
            Console.WriteLine("btnThread_Click End {0} {1}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now.ToString());

        }

        private void btnThreadPool_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("btnThreadPool_Click Start {0} {1}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now.ToString());
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            //ThreadPool.QueueUserWorkItem((t) => {
            //    this.MyAsyncMethod("MyAsyncMethod");
            //    manualResetEvent.Set();
            //    });
            //ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads); 
            //Console.WriteLine($"workerThreads = {workerThreads}\ncompletionPortThreads = {completionPortThreads}");
            //manualResetEvent.WaitOne();
            for (int i = 0; i < 20; i++)
            {
                int k = i;
                ThreadPool.QueueUserWorkItem(a =>
                {
                    Console.WriteLine(k);
                    if (k < 18)
                    {
                        manualResetEvent.WaitOne();
                    }
                    else
                    {
                        manualResetEvent.Set();
                    }
                });
            }
            if (manualResetEvent.WaitOne())
            {
                Console.WriteLine("没有死锁");
            }
            Console.WriteLine("btnThreadPool_Click End {0} {1}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now.ToString());
        }
    }
}
