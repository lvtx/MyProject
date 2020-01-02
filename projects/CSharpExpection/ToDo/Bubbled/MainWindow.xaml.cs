using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Bubbled
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //创建一个源数据
            Person person = new Person();
            //使数据可以被绑定
            this.DataContext = person;
        }
        /// <summary>
        /// 附加事件。
        /// 在DockPanel声明一个单击事件
        /// 单击其内部的Button按钮依然
        /// 可以执行此事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockPanel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("我被点了");
        }
    }

    class Person
    {
        private int name;

        public int Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
