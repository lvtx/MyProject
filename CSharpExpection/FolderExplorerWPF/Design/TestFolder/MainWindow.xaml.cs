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

namespace TestFolder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AllDrives d = new AllDrives();
            treeView.ItemsSource = d.Drives;
        }

        private void ChangeBackground(object sender, MouseButtonEventArgs e)
        {
            if (btn.Background == Brushes.Red)
            {
                btn.Background = new LinearGradientBrush(Colors.LightBlue, Colors.SlateBlue, 90);
                btn.Content = "Control background changes from red to a blue gradient.";
            }
            else
            {
                btn.Background = Brushes.Red;
                btn.Content = "Background";
            }
        }
    }
}
