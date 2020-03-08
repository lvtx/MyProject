using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UseAllCodeToCreateWPFWindow
{
    public class Window1 : Window
    {
        private Button btnClickMe;

        public Window1()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            // 设置窗体属性
            this.Width = this.Height = 285;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Title = "完全用代码创建的窗体";
            // 创建一个面板
            DockPanel panel = new DockPanel();
            // 创建按钮对象
            btnClickMe = new Button();
            btnClickMe.Content = "请单击我";
            btnClickMe.Margin = new Thickness(30);

            // 挂接事件
            btnClickMe.Click += btnClickMe_Click;
            // 将按钮加入到面板中
            panel.Children.Add(btnClickMe);
            // 将面板加入到窗体中
            this.AddChild(panel);
           
        }

        private int Counter = 0;
        private void btnClickMe_Click(object sender, RoutedEventArgs e)
        {
            Counter++;
            btnClickMe.Content = string.Format("我被单击了{0}次",Counter);
        }
    }
}
