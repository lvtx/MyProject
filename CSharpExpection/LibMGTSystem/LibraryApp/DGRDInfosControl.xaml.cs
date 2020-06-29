using System;
using DataAccessLayer;
using LibraryModel;
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
using System.Collections;
using System.Collections.ObjectModel;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for DGRDInfos.xaml
    /// </summary>
    public partial class DGRDInfosControl : UserControl
    {
        BookTypeRepository bookTypeEntity = new BookTypeRepository();
        ReaderTypeRepository readerTypeEntity = new ReaderTypeRepository();
        RoleTypeRepository roleTypeEntity = new RoleTypeRepository();
        public DGRDInfosControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化TreeView1
        /// 显示书籍类型
        /// </summary>
        private async Task InitializeBookTypeTVWTask()
        {
            ObservableCollection<BookType> bookTypes = await GetBookTypesAsync();
            TVWTypes.ItemsSource = bookTypes;
            TVWTypes.DisplayMemberPath = "BookTypeName";
            //找出TreeView的第一个元素
            var item = TVWTypes.ItemContainerGenerator.ContainerFromIndex(0);
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
            TVWTypes.ItemsSource = readerTypes;
            //tvwTypeOfBooks.ItemsSource = await roleTypeEntity.GetAllClientsAsync();
            TVWTypes.DisplayMemberPath = "RoleTypeName";
            //找出TreeView的第一个元素
            var item = TVWTypes.ItemContainerGenerator.ContainerFromIndex(0);
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

        public IEnumerable DGRDSource
        {
            get { return (IEnumerable)GetValue(DGRDSourceProperty); }
            set { SetValue(DGRDSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DGRDSource.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DGRDSourceProperty =
            DependencyProperty.Register("DGRDSource", typeof(IEnumerable), 
                typeof(DGRDInfosControl), new PropertyMetadata(null));


        private void InitializeDGRD()
        {
            if (DGRDSource != null)
            {
                DGRDInfos.ItemsSource = DGRDSource;
            }
        }
    }
}
