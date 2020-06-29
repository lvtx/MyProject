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

namespace UseDispatcherTimer
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Init();

        }
        DispatcherTimer timer = null;
        Random rnd = null;
        private void Init()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            rnd = new Random();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            byte[] vals = new byte[3];
            rnd.NextBytes(vals);
            Color c = Color.FromRgb(vals[0], vals[1], vals[2]);
           ellipse1.Fill = new SolidColorBrush(c);
        }
        private void btnStartTimer_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                btnStartTimer.Content = "启动颜色变换";
            }
            else
            {
                timer.Start();
                btnStartTimer.Content = "停止颜色变换";
            }
        }
    }
}
