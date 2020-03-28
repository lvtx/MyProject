using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
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
            _ = InitializeTreeView();
            InitializeDGRDShowReader();
        }
        //书的类型全局变量
        public BookTypeRepository bookTypeContext = new BookTypeRepository();
        public BookInfoRepository bookInfoContext = new BookInfoRepository();
        public ReaderTypeRepository readerTypeContext = new ReaderTypeRepository();
        public ReaderRepository readerContext = new ReaderRepository();
        /// <summary>
        /// 初始化TreeView
        /// </summary>
        async Task InitializeTreeView()
        {
            List<BookType> typeOfBooks = await bookTypeContext.GetAllClientsAsync();
            tvwTypeOfBooks.ItemsSource = typeOfBooks;
            tvwTypeOfBooks.DisplayMemberPath = "BookTypeName";
            //隐藏展示读者信息的DataGrid
        }
        /// <summary>
        /// 初始化TreeView
        /// </summary>
        async Task InitializeDGRD()
        {
            List<ReaderType> typeOfReaders = await readerTypeContext.GetAllClientsAsync();
            tvwTypeOfBooks.ItemsSource = typeOfReaders;
            tvwTypeOfBooks.DisplayMemberPath = "ReaderTypeName";
            //隐藏展示读者信息的DataGrid
        }
        /// <summary>
        /// 初始化显示Reader信息的DataGrid
        /// </summary>
        void InitializeDGRDShowReader()
        {
            dgrdShowReaderInfo.Visibility = Visibility.Collapsed;
        }

        #region "ToolBar各个按钮对应的事件"
        private void btnLibBooks_OnClick(object sender, RoutedEventArgs e)
        {
            _ = InitializeTreeView();
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
            _ = InitializeDGRD();
            dgrdShowReaderInfo.Visibility = Visibility.Visible;
            //隐藏展示书籍信息的DataGrid
            dgrdShowBookInfo.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region "DataGrid显示书籍信息"
        private void tvwTypeOfBooks_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DisplayInfoAsync();
        }
        /// <summary>
        /// 显示书籍和读者信息
        /// </summary>
        /// <returns></returns>
        private void DisplayInfoAsync()
        {
            object whatIsTypeOfSel = tvwTypeOfBooks.SelectedItem;
            if (whatIsTypeOfSel is BookType)
            {
                BookType bookTypeBySel = (whatIsTypeOfSel as BookType);
                //var books = from b in await bookInfoContext.GetAllClientsAsync()
                //            where b.BookTypeId == bookTypeBySel.BookTypeId
                //            select b;
                //延迟加载
                var books = bookTypeBySel.BookInfoes;
                dgrdShowBookInfo.ItemsSource = books;
                tvwTypeOfBooks.DisplayMemberPath = "BookTypeName";
            }
            if(whatIsTypeOfSel is ReaderType)
            {
                ReaderType readerTypeBySel = (whatIsTypeOfSel as ReaderType);
                //var readers = from r in await readerContext.GetAllClientsAsync()
                //             where r.ReaderTypeId == readerTypeBySel.ReaderTypeId
                //             select r;
                //延迟加载
                var readers = readerTypeBySel.Readers;
                dgrdShowReaderInfo.ItemsSource = readers;
                tvwTypeOfBooks.DisplayMemberPath = "ReaderTypeName";
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
            AddBook AddReaderWindow = new AddBook();
            //window窗体包含所需要的Repository变量
            //将查找到的书籍类型绑定到ComboBox控件
            AddReaderWindow.BindingCBOToType(this.bookTypeContext);
            AddReaderWindow.ShowDialog();
        }
        #endregion
        #endregion
    }
}
