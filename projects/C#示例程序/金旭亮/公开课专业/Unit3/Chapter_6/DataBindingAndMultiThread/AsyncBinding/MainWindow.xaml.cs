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

namespace AsyncBinding
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
                 Information="Hello!"
            };

            DataContext = obj;

            
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            txtInput.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
       
    }
}
