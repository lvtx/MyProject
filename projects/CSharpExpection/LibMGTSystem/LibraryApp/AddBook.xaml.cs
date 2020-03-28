using System;
using Model;
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
    /// Interaction logic for AddReader.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        public AddBook()
        {
            InitializeComponent();
        }
        //public AddBook(MainWindow mainWindow)
        //{
        //    _mainWindow = mainWindow;
        //}
        #region "初始化ComboBox"
        public void BindingCBOToType(BookTypeRepository bookTypeContext)
        {
            List<BookType> bookTypes = bookTypeContext.GetAllClient();
            cboBookTypeId.ItemsSource = bookTypes;
            cboBookTypeId.DisplayMemberPath = "BookTypeName";
            cboBookTypeId.SelectedValuePath = "BookTypeId";
            txtAddress.Text = "触发一次";
        }
        #endregion
    }
}
