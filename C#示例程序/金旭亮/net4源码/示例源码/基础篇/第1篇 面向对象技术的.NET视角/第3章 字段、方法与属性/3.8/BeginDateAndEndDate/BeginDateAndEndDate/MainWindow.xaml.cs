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

namespace BeginDateAndEndDate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            root.DataContext = obj;
        }

        private Test obj = new Test();
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            (obj as DependencyObject).ClearValue(Test.BeginDateProperty);
            (obj as DependencyObject).ClearValue(Test.EndDateProperty);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format("{0}-{1}", obj.BeginDate.ToShortDateString(), obj.EndDate.ToShortDateString()));
            Close();
        }
    }
}
