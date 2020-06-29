using System;
using LibraryModel;
using DataAccessLayer;
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
using System.Windows.Shapes;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Clicked(object sender, RoutedEventArgs e)
        {
            Validate();
        }
        async void Validate()
        {
            Admin admin = new Admin();
            admin.LoginId = txtLoginId.Text.Trim();
            admin.LoginPwd = FloatingPasswordBox.Password;
            var myTask = DBHelper.Login(admin);
            int num = await myTask;
            //a.LoginType = cboType.Text.Trim();
            if (num == 1)
            {
                MainWindow MainWindow = new MainWindow();
                //MainWindow.admin = admin;
                MainWindow.Show();
                this.Close();
            }
        }

        private void btnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("确定取消吗！", "提示",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
                this.Close();
        }
    }
}
