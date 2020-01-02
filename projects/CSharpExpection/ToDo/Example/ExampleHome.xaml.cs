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

namespace Example
{
    /// <summary>
    /// Interaction logic for ExampleHome.xaml
    /// </summary>
    public partial class ExampleHome : Page
    {
        public ExampleHome()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExampleReportPage exampleReportPage = new ExampleReportPage(this.peopleListBox.SelectedItem);
            this.NavigationService.Navigate(exampleReportPage);
        }
    }
}
