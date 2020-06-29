using DataAccessLayer;
using LibraryModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            _ = InitializeBookTypeTVWTask();
            dgrdShowReader.Visibility = Visibility.Collapsed;
            InitializeTbStatus();
            InitializeTxtPage();
            DataContext = this;
            InitializeCmbType();
            //InitializeDGRD();
            //InitializeDGRDShowReader();
            //DataContext = this;

        }
        //全局变量(仓库类)
        public BookTypeRepository bookTypeEntity = new BookTypeRepository();
        public BookInfoRepository bookEntity = new BookInfoRepository();
        public ReaderRepository readerEntity = new ReaderRepository();
        public ReaderTypeRepository readerTypeEntity = new ReaderTypeRepository();
        public RoleTypeRepository roleTypeEntity = new RoleTypeRepository();
        //当前所在的页码或者希望跳转到的页码


        public int Page
        {
            get { return (int)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Page.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(int), typeof(MainWindow), new PropertyMetadata(1));


        public String Status
        {
            get { return (String)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(String), typeof(MainWindow), new PropertyMetadata(" "));
        /// <summary>
        /// 初始化tbStatus
        /// 显示状态
        /// </summary>
        private void InitializeTbStatus()
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Status");
            tbStatus.SetBinding(TextBlock.TextProperty, binding);
        }
        /// <summary>
        /// TextBox与Page绑定
        /// </summary>
        private void InitializeTxtPage()
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Page");
            tbStatus.SetBinding(TextBox.TextProperty, binding);
        }

        private void InitializeDGRD()
        {
            var bookType = tvwShowTypeInfo.Items.CurrentItem as BookType;
            IInfo<BookInfo> bookInfo = new BookFactory(bookType);
            dgrdShowBookInfo.ItemsSource = bookInfo.GetInfos(Page);
        }
        /// <summary>
        /// 初始化TreeView1
        /// 显示书籍类型
        /// </summary>
        private async Task InitializeBookTypeTVWTask()
        {
            ObservableCollection<BookType> bookTypes = await GetBookTypesAsync();
            tvwShowTypeInfo.ItemsSource = bookTypes;
            tvwShowTypeInfo.DisplayMemberPath = "BookTypeName";
            //找出TreeView的第一个元素
            var item = tvwShowTypeInfo.ItemContainerGenerator.ContainerFromIndex(0);
            TreeViewItem firstItem = item as TreeViewItem;
            firstItem.IsSelected = true;
        }
        /// <summary>
        /// 获取图书类型
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<BookType>> GetBookTypesAsync()
        {
            List<BookType> rets = await bookTypeEntity.GetAllClientsAsync();
            ObservableCollection<BookType> bookTypes = new ObservableCollection<BookType>(rets);
            return bookTypes;
        }
        /// <summary>
        /// 初始化TreeView2
        /// 显示读者类型
        /// </summary>
        private async Task InitializeReaderTypeTVWTask()
        {
            ObservableCollection<RoleType> readerTypes = await GetReaderTypeAsync();
            tvwShowTypeInfo.ItemsSource = readerTypes;
            //tvwTypeOfBooks.ItemsSource = await roleTypeEntity.GetAllClientsAsync();
            tvwShowTypeInfo.DisplayMemberPath = "RoleTypeName";
            //找出TreeView的第一个元素
            var item = tvwShowTypeInfo.ItemContainerGenerator.ContainerFromIndex(0);
            TreeViewItem firstItem = item as TreeViewItem;
            firstItem.IsSelected = true;
        }
        /// <summary>
        /// 获取读者类型
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<RoleType>> GetReaderTypeAsync()
        {
            //源读者类型
            List<RoleType> sReaderTypes = await readerTypeEntity.GetAllClientsAsync();
            List<RoleType> roleTypes = await roleTypeEntity.GetAllClientsAsync();
            List<RoleType> items = new List<RoleType>();
            foreach (var sReaderType in sReaderTypes)
            {
                RoleType item = roleTypes.Find((r) => r.RoleTypeName == sReaderType.RoleTypeName);
                items.Add(item);
            }
            ObservableCollection<RoleType> readerTypes =
                new ObservableCollection<RoleType>(roleTypes.Intersect(items));
            return readerTypes;
        }
        //void InitializeDGRDShowReader()
        //{
        //    dgrdShowReaderInfo.Visibility = Visibility.Collapsed;
        //}
        private void InitializeCmbType()
        {
            if ((tvwShowTypeInfo.SelectedItem is BookType)
                || tvwShowTypeInfo.SelectedItem == null)
            {
                Console.WriteLine("当前类型为书籍");
                cmbType.ItemsSource = dgrdShowBookInfo.Columns;
                cmbType.DisplayMemberPath = "Header";
                cmbType.SelectedIndex = 1;
            }
            else if (tvwShowTypeInfo.SelectedItem is RoleType)
            {
                Console.WriteLine("当前类型为读者");
                cmbType.ItemsSource = dgrdShowReader.Columns;
                cmbType.DisplayMemberPath = "Header";
                cmbType.SelectedIndex = 1;
            }
        }
        #region "ToolBar各个按钮对应的事件"
        /// <summary>
        /// 图书管理按钮对应的事件 (DataGrid控件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLibBooks_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshBookAsync();
            dgrdShowBookInfo.Visibility = Visibility.Visible;
            //隐藏展示读者信息的DataGrid
            dgrdShowReader.Visibility = Visibility.Collapsed;
        }
        private async void RefreshBookAsync()
        {
            await InitializeBookTypeTVWTask();
            InitializeCmbType();
        }

        /// <summary>
        /// 读者管理按钮对应的事件 (DataGrid控件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReaderMGT_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshReaderAsync();
            InitializeCmbType();
            dgrdShowReader.Visibility = Visibility.Visible;
            //隐藏展示书籍信息的DataGrid
            dgrdShowBookInfo.Visibility = Visibility.Collapsed;
        }
        private async void RefreshReaderAsync()
        {
            await InitializeReaderTypeTVWTask();
            InitializeCmbType();
        }
        #endregion

        #region "DataGrid刷新信息"
        /// <summary>
        /// TreeView切换类型时刷新DataGrid绑定的信息 (TreeView控件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwShowTypeInfo_SelectedItemChanged
            (object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Page = 1;
            DisplayInfoDGRD();
        }
        /// <summary>
        /// 显示书籍或读者信息 (刷新DataGrid控件所需方法)
        /// </summary>
        /// <returns></returns>
        private async void DisplayInfoDGRD()
        {
            int count = await BindingInfosToDGRDAsync(tvwShowTypeInfo.SelectedItem);
            GetTotalPagesAsync(count);
        }

        #region "将所选中类型对应的信息绑定到DataGrid"
        /// <summary>
        /// 将选中的类型所对应的
        /// 信息绑定到DataGrid上
        /// 书籍类型、读者类型
        /// 书籍信息、读者信息
        /// </summary>
        /// <param name="selectedElement"></param>
        /// <returns></returns>
        private int BindingInfosToDGRD(object selectedElement)
        {
            object SelectedElement = selectedElement;
            if (SelectedElement is BookType)
            {
                BookType bookType = (SelectedElement as BookType);
                int count = bookType.BookInfo.Count;
                IInfo<BookInfo> bookFactory = new BookFactory(bookType);
                this.Dispatcher.Invoke(() =>
                {
                    dgrdShowBookInfo.ItemsSource = bookFactory.GetInfos(Page);
                });
                return count;
            }
            else if (SelectedElement is RoleType)
            {
                //var readerType = (SelectedElement as RoleType);
                ////获取所有的读者
                //List<Reader> allReaders = await readerEntity.GetAllClientsAsync();
                ////选出符合条件的读者
                //var readers = allReaders.Where((r) => r.SourceRoleTypeId == readerType.RoleTypeId);
                //dgrdShowReaderInfo.ItemsSource = readers;
                RoleType readerType = (SelectedElement as RoleType);
                IInfo<Reader> readerFactory = new ReaderFactory(readerType);
                //源角色类型(例如老师学生)对应的读者
                this.Dispatcher.Invoke(() =>
                {
                    dgrdShowReader.ItemsSource = readerFactory.GetInfos(Page);
                });
                //dgrdShowInfo.ItemsSource = readers;
                //这个放在开头会导致连接未关闭就开始下一个连接
                int count = readerType.SReader.Count;
                return count;
            }
            else
            {
                return -1;
            }
        }
        private Task<int> BindingInfosToDGRDTask(object selectedElement)
        {
            return Task.Run(() => BindingInfosToDGRD(selectedElement));
        }
        private async Task<int> BindingInfosToDGRDAsync(object selectedElement)
        {
            int count = await BindingInfosToDGRDTask(selectedElement);
            return count;
        }
        #endregion

        #region "获取总页数"
        /// <summary>
        /// 每页20个元素可以分多少页
        /// </summary>
        private void GetTotalPages(int count)
        {
            //每页20个元素
            const int num = 20;
            //计算总页数
            int r = count / num;
            int j = count % num;
            int p = (j == 0) ? r : r + 1;
            //一共要分多少页
            this.Dispatcher.Invoke(
                () =>
                {
                    tbTotalPages.Text = string.Format("{0}", p);
                    Status = string.Format("一共 " + count + " 项");
                });
        }
        private Task GetTotalPagesTask(int count)
        {
            return Task.Run(() => GetTotalPages(count));
        }
        private async void GetTotalPagesAsync(int count)
        {
            await GetTotalPagesTask(count);
        }
        #endregion

        #region "TextBox搜索框"
        private Object PreSelectedType = null;
        private Object CurrentSelectedType = null;
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingAndSearch();
        }
        private void BindingAndSearch()
        {
            dgrdShowBookInfo.Visibility = Visibility.Hidden;
            var SelectedElement = tvwShowTypeInfo.SelectedItem;
            CurrentSelectedType = SelectedElement;
            bool IsChanged = false;
            if (PreSelectedType != null)
            {
                IsChanged = !(PreSelectedType.Equals(CurrentSelectedType));
            }
            else
            {
                BindingAllInfosToDGRD(SelectedElement);
                Status = "初始化搜索绑定";
            }
            if (SelectedElement != null && IsChanged == true)
            {
                BindingAllInfosToDGRD(SelectedElement);
                Status = "选中类型改变,搜索时重新绑定";
            }
            DGRDViewFilter(dgrdShowBookInfo.ItemsSource,txtSearch.Text);
            PreSelectedType = CurrentSelectedType;
            dgrdShowBookInfo.Visibility = Visibility.Visible;
        }
        private void BindingAllInfosToDGRD(object selectedElement)
        {
            object SelectedElement = selectedElement;
            if (SelectedElement is BookType)
            {
                BookType bookType = (SelectedElement as BookType);
                IInfo<BookInfo> bookFactory = new BookFactory(bookType);
                dgrdShowBookInfo.ItemsSource = bookFactory.GetInfos();
                //DGRDViewFilter(dgrdShowBookInfo.ItemsSource);
            }
            else if (SelectedElement is RoleType)
            {
                RoleType readerType = (SelectedElement as RoleType);
                //源角色类型(例如老师学生)对应的读者
                IInfo<Reader> readerFactory = new ReaderFactory(readerType);
                dgrdShowReader.ItemsSource = readerFactory.GetInfos();
                //DGRDViewFilter(dgrdShowReaderInfo.ItemsSource);
            }
        }
        /// <summary>
        /// BindingAllInfosToDGRD异步方法
        /// </summary>
        /// <param name="selectedElement"></param>
        /// <returns></returns>
        //private Task BindingAllInfosToDGRDTask(object selectedElement)
        //{
        //    return Task.Run(() => BindingInfosToDGRD(selectedElement));
        //}
        //private async void BindingAllInfosToDGRDAsync(object selectedElement)
        //{
        //    await BindingInfosToDGRDTask(selectedElement);
        //}
        private void DGRDViewFilter(IEnumerable source,string keyWord)
        {
            string item = (cmbType.SelectedItem as DataGridTextColumn).Header.ToString();
            SearchFactory searcher = new SearchFactory();
            searcher.Search(source, item, keyWord);
        }
        #endregion

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
            //BookTypeWindow.ShowBookType(bookTypes);
            BookTypeWindow.ShowDialog();
        }
        #endregion

        #endregion

        private void txtPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            Paging();
        }
        /// <summary>
        /// 根据页码显示对应的信息
        /// 用到了BindingInfosToDGRD()异步方法
        /// BindingInfosToDGRD()实现DataGrid刷新
        /// 在ToolBar中
        /// </summary>
        private async void Paging()
        {
            var SelectedElement = tvwShowTypeInfo.SelectedItem;
            if (SelectedElement != null && !string.IsNullOrEmpty(txtPage.Text))
            {
                Page = int.Parse(txtPage.Text);
                await BindingInfosToDGRDAsync(SelectedElement);
                Status = "页码已经改变:" + Page;
            }
            else
            {
                Status = "请选择一个类型";
            }
        }

        private void btnPageUp_Click(object sender, RoutedEventArgs e)
        {
            txtPage.Text = (Page - 1).ToString();
            Paging();
        }

        private void btnPageDown_Click(object sender, RoutedEventArgs e)
        {
            txtPage.Text = (Page + 1).ToString();
            Paging();
        }

    }
    /// <summary>
    /// 模板选择器
    /// 暂时用不上
    /// 占个位
    /// </summary>
    public class ItemTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BooksTemplate { get; set; }
        public DataTemplate ReadersTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BookInfo)
            {
                Console.WriteLine("书籍类型");
                return BooksTemplate;
            }
            else if (item is Reader)
            {
                Console.WriteLine("读者类型");
                return ReadersTemplate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }

    }
}