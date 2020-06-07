using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WPFDataBindingMultiThread
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyDataClass obj = null;

        public MainWindow()
        {
            Console.WriteLine("主线程:{0}", Thread.CurrentThread.ManagedThreadId);

            InitializeComponent();

            obj = new MyDataClass()
            {
                Information = "Hello!"
            };

            DataContext = obj;
            //在多线程环境下，应该注释掉以下这句
            //obj.PropertyChanged += obj_PropertyChanged;
        }
        void obj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("主界面中的PropertyChanged事件响应方法的执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            //以下这句在多线程状态下将引发异常
            tbTest.Text = "Visited in PropertyChanged Event";
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            //单线程状态
            //ModifyDataSourceProperty();

            //多线程状态
            Task.Run(() =>
            {
                ModifyDataSourceProperty();
            });
        }
        /// <summary>
        /// 更改数据源对象属性
        /// </summary>
        private void ModifyDataSourceProperty()
        {
            String modified = "Modify @" + DateTime.Now.ToLocalTime();
            Console.WriteLine("\n在线程{0}中修改数据源的值为：{1}",
                Thread.CurrentThread.ManagedThreadId, modified);
            obj.Information = modified;
        }
    }
}
