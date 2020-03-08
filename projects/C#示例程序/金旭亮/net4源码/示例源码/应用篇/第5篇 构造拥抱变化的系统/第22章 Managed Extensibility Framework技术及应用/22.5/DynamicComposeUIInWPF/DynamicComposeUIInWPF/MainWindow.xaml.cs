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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading;

namespace DynamicComposeUIInWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        [Import(typeof(UserControl))]
        private UserControl DynamicComposedUIPart = null;

        /// <summary>
        /// 在背景线程里组合UI部件
        /// </summary>
        private void ComposeParts()
        {
            //动态搜索当前文件夹下的可装载部件
            DirectoryCatalog cata = new DirectoryCatalog(Environment.CurrentDirectory);
            //创建部件组合容器
            CompositionContainer container = new CompositionContainer(cata);
           
            //以下方法将被“推送”到创建主窗体的线程中执行
            Action ShowUIPart = delegate()
            { 
                //实例化UI部件
                container.ComposeParts(this);
                Thread.Sleep(5000);
                //显示UI组件
                if (DynamicComposedUIPart != null)
                    ControlContainer.Content = DynamicComposedUIPart;
            };
            //在创建主窗体的线程中显示UI组件
            Dispatcher.Invoke(ShowUIPart);

        }

        private void LoadUIPartInBackground()
        {
            Action<string> showInfo = delegate(string info)
            {
                tbInfo.Text = info;
            };

            Dispatcher.Invoke(showInfo, "正在加载部件……");
            ComposeParts();
            Dispatcher.Invoke(showInfo, "加载完成！");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //将以多线程方式调用LoadUIPartInBackground()方法
            Thread th = new Thread(LoadUIPartInBackground);
            //设置为背景线程
            th.IsBackground = true;
            th.Start();//启动
        }
    }
}
