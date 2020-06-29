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

namespace UIEmbedUseContentControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Compose();
        }
        /// <summary>
        /// 完成界面的组合
        /// </summary>
        private void Compose()
        {
           
            top.Content = new topUIPart();
            left.Content = new leftUIPart();
            right.Content=new rightUIPart();
            bottom.Content = new bottomUIPart();
            center.Content = new CenterUIPart();
        }
    }
}
