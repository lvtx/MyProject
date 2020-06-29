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
using System.Threading;

namespace VisistUIControlViaWPFDataBinding1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            //创建数据对象并绑定到UI控件
            dataobj=new MyDataObject();
            textBlock1.DataContext = dataobj;
        }

        private MyDataObject dataobj = null;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //在另一个线程中修改数据对象的值，会导致UI控件的自动更新
            ThreadStart threadFunc = delegate()
            {
                dataobj.Value++;
            };
            Thread th = new Thread(threadFunc);
            th.IsBackground = true;
            th.Start();
                        
        }
    }
}
