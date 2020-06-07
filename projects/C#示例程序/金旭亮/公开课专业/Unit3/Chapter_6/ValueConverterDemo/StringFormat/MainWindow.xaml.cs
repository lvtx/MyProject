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

namespace StringFormat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyDataClass dataSource = null;
        public MainWindow()
        {
            InitializeComponent();

            dataSource = new MyDataClass()
            {
                FloatNumber=3.141592653589,
                IsValid=true
            };

            DataContext = dataSource;
        }
    }
}
