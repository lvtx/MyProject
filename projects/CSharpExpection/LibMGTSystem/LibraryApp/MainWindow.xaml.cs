using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryModel;
using DataAccessLayer;
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
using MaterialDesignThemes.Wpf;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _ = InitializeBookTypeTVW();
            InitializeDGRDShowReader();
        }
        //全局变量(仓库类)
        public BookTypeRepository bookTypeEntity = new BookTypeRepository();
        public BookInfoRepository bookEntity = new BookInfoRepository();
        public ReaderRepository readerEntity = new ReaderRepository();
        public ReaderTypeRepository readerTypeEntity = new ReaderTypeRepository();
        /// <summary>
        /// 初始化TreeView1
        /// 显示书籍类型
        /// </summary>
        async Task InitializeBookTypeTVW()
        {
            List<BookType> bookTypes = await bookTypeEntity.GetAllClientsAsync();
            tvwTypeOfBooks.ItemsSource = bookTypes;
            tvwTypeOfBooks.DisplayMemberPath = "BookTypeName";
            //初始化时使datagrid显示第一个类型的图书
            //dgrdShowBookInfo.ItemsSource = bookTypes.FirstOrDefault().BookInfo;
        }
        /// <summary>
        /// 初始化TreeView2
        /// 显示读者类型
        /// </summary>
        async Task InitializeReaderTypeTVW()
        {
            List<RoleType> readerTypes = await readerTypeEntity.GetAllClientsAsync();
            tvwTypeOfBooks.ItemsSource = readerTypes;
            tvwTypeOfBooks.DisplayMemberPath = "RoleTypeName";
        }
        void InitializeDGRDShowReader()
        {
            dgrdShowReaderInfo.Visibility = Visibility.Collapsed;
        }

        #region "ToolBar各个按钮对应的事件"
        private void btnLibBooks_OnClick(object sender, RoutedEventArgs e)
        {
            _ = InitializeBookTypeTVW();
            dgrdShowBookInfo.Visibility = Visibility.Visible;
            //隐藏展示读者信息的DataGrid
            dgrdShowReaderInfo.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 读者管理按钮对应的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReaderMGT_OnClick(object sender, RoutedEventArgs e)
        {
            _ = InitializeReaderTypeTVW();
            dgrdShowReaderInfo.Visibility = Visibility.Visible;
            //隐藏展示书籍信息的DataGrid
            dgrdShowBookInfo.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region "DataGrid显示书籍信息"
        private void tvwTypeOfBooks_SelectedItemChanged
            (object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _ = DisplayInfoDGRD();
        }
        /// <summary>
        /// 显示书籍或读者信息
        /// </summary>
        /// <returns></returns>
        async Task DisplayInfoDGRD()
        {
            object SelectedElement = tvwTypeOfBooks.SelectedItem;
            if (SelectedElement is BookType)
            {
                BookType bookType = (SelectedElement as BookType);
                var books = bookType.BookInfo;
                dgrdShowBookInfo.ItemsSource = books;
                //tvwTypeOfBooks.DisplayMemberPath = "BookTypeName";
            }
            else if (SelectedElement is RoleType)
            {
                var readerType = (SelectedElement as RoleType);
                //获取所有的读者
                List<Reader> allReaders = await readerEntity.GetAllClientsAsync();
                //选出符合条件的读者
                var readers = allReaders.Where((r) => r.SourceRoleTypeId == readerType.RoleTypeId);
                dgrdShowReaderInfo.ItemsSource = readers;
            }
        }
        #endregion
        #region "Menu"
        #region "小工具"
        private void OpenPaint(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("mspaint.exe");
        }

        private void OpenCalc(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }
        #endregion
        #region "读者管理"
        /// <summary>
        /// 打开添加读者的窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenAddBook(object sender, RoutedEventArgs e)
        {
            //定义一个添加图书的窗口
            AddBook AddBookWindow = new AddBook();
            //window窗体包含所需要的Repository变量
            //将查找到的书籍类型绑定到ComboBox控件
            AddBookWindow.BindingCBOToType(this.bookTypeEntity);
            AddBookWindow.ShowDialog();
        }
        #endregion
        #region "管理图书类型"
        private void ManageBookType(object sender, RoutedEventArgs e)
        {
            //定义一个修改图书类型的窗口
            ModifyBookType BookTypeWindow = new ModifyBookType();
            List<BookType> bookTypes = bookTypeEntity.GetAllClient();
            //BookTypeWindow.ShowBookType(bookTypes);
            BookTypeWindow.ShowDialog();
        }
        #endregion

        #endregion

    }
}
