using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace OneWayBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyData DataItem = null;
        private System.Timers.Timer timer = null;
        public MainWindow()
        {
            InitializeComponent();
            DataItem = new MyData
            {
                Name = "My Data Object",
                Value = 100
                
            };
            //设定数据源
            StackPanelForMyData.DataContext = DataItem;
            Console.WriteLine("主线程：{0}", Thread.CurrentThread.ManagedThreadId);
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += timer_Elapsed;
            

        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //WPF数据绑定，支持跨线程更新UI界面元素
            Console.WriteLine("Timer线程：{0}", Thread.CurrentThread.ManagedThreadId);
            DataItem.Time = DateTime.Now;
        }

        private void btnIncrease_Click(object sender, RoutedEventArgs e)
        {
            DataItem.Value++;

            //WPF数据绑定，支持跨线程更新UI界面元素
            //Task.Run(() => { DataItem.Value++; });

        }

        private void StackPanelForMyData_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            Console.WriteLine("TargetUpdated:Name={0},Value={1}", DataItem.Name, DataItem.Value);
        }

        private void StackPanelForMyData_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            Console.WriteLine("SourceUpdated:Name={0},Value={1}", DataItem.Name, DataItem.Value);
        }

        private void chkShowTime_Checked(object sender, RoutedEventArgs e)
        {
            timer.Enabled = chkShowTime.IsChecked??false;
        }
    }
}
