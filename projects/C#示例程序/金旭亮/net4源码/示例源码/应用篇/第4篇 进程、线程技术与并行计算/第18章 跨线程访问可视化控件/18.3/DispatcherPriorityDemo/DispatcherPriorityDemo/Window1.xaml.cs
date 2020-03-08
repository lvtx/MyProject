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
using System.Threading;

namespace DispatcherPriorityDemo
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();


            //如果在此处调用LoadNumbers()方法,要等到此方法调用结束之后,窗体才能显示
            //LoadNumbers();

            //如果在此处调用LoadNumbers()方法时指定一个比DispatcherPriority.Render更高的优先级
            //比如DispatcherPriority.Normal
            //则等到此方法调用结束之后,窗体才能显示并绘制
            this.Dispatcher.BeginInvoke(
               DispatcherPriority.Background,
               new Action(LoadNumbers));


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //如果在此处调用LoadNumbers()方法,要等到此方法调用结束之后,
            //窗体才能正确绘制
            //LoadNumbers();

        }

       

        // 通过数据绑定向ListBox中填充100万个整数
        private void LoadNumbers()
        {
            List<string> numberDescriptions = new List<string>();
            Thread.Sleep(3000);
            for (int i = 1; i <= 1000000; i++)
            {
                numberDescriptions.Add("数据项 " + i.ToString());
            }
 
            //实现数据绑定
            listBox.ItemsSource = numberDescriptions;
           
        }
    }
}
