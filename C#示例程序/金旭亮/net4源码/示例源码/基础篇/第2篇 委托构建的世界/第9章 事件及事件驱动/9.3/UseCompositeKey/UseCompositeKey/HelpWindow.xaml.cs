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

namespace UseCompositeKey
{
    /// <summary>
    /// HelpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }
        //用于判断是否是整个程序结束
        public bool WantedShutDown = false;
        //禁止关闭帮助窗体
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WantedShutDown == false) //改关闭为隐藏
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
