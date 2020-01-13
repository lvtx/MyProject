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

namespace FolderExplorer
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }
        
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack == true)
            {
                NavigationService.GoBack();               
            }         
        }

        private void btnGoForward_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoForward == true)
            {
                NavigationService.GoForward();
            }        
        }

        private void tvFolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
