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
using System.Dynamic;

namespace DataBindUseExpandoObject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            rootPanel.DataContext = MyExpandoObject;
            MyExpandoObject.Value = 0;
        }

        private dynamic MyExpandoObject = new ExpandoObject();
        private void btnIncrement_Click(object sender, RoutedEventArgs e)
        {
            MyExpandoObject.Value++;
        }
    }
}
