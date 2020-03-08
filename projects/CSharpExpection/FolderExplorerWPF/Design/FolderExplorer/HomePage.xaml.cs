using System;
using System.IO;
using FolderExplorer.Filesystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Drawing;
using System.Globalization;
using System.Windows.Interop;
using System.Diagnostics;
using FolderExplorer.Strategy;
using System.ComponentModel;

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
            InitializeTreeView(); // 初始化文件树
                                  //InitializeHeadNode();// 初始化头节点
            showContextMenu.Text = "1";
            #region "显示其可视化树"
            //VisualTreeDisplay treeDisplay = new VisualTreeDisplay();
            //treeDisplay.ShowVisualTree(this.TestDoubleClick);
            //treeDisplay.Show();
            #endregion
        }
        /// <summary>
        /// 初始化一个导航用的双向链表
        /// </summary>
        DbLink<Folder> HistoryDbLink = new DbLink<Folder>();
        /// <summary>
        /// 创建一个视图
        /// </summary>
        ListCollectionView dgrdView;
        public void InitializeTreeView()
        {

            ObservableCollection<Folder> folders = new ObservableCollection<Folder>();
            //List<string> paths = new List<string>();
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            //paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            Drive d = new Drive();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                string path = drive.Name;
                Folder newFolder = new Folder();
                newFolder.FullPath = path;
                newFolder.Name = d.GetDriveName(drive);
                newFolder.iIcon = IconInfo.GetDriverIcon(path[0], false);
                folders.Add(newFolder);
            }
            //foreach (var path in paths)
            //{
            //    Folder newFolder = new Folder();
            //    newFolder.FullPath = path;
            //    folders.Add(newFolder);
            //}
            tvwDisplayFolder.ItemsSource = folders;
        }
        /// <summary>
        /// 双击打开文件夹（DataGrid控件中）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileOrFolder(object sender, MouseButtonEventArgs e)
        {
            switch (e.ClickCount)
            {
                case 2:
                    {
                        try
                        {
                            FileSys currFolder = dgrd_Display.CurrentItem as FileSys;
                            
                            //view = (ListCollectionView)CollectionViewSource.GetDefaultView(dgrd_Display.CurrentItem);
                            var path = currFolder.FullPath;
                            if (currFolder is Folder)
                            {
                                Folder f = currFolder as Folder;
                                HistoryDbLink.AddNode(f);
                                #region "测试添加了几个节点"
                                //HistoryDbLink.Count++;
                                //nodeCount.Text = HistoryDbLink.Count.ToString();
                                #endregion
                                ShowFoldersOrFiles(f);
                            }
                            else
                            {
                                Process.Start(path);
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 单击TreeView后触发的事件
        /// 将当前选中文件夹的子文件夹显示在DataGrid中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplaySubFoldersAndFilesOnDGRD(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Folder currFolder = tvwDisplayFolder.SelectedItem as Folder;
            //将打开的文件夹加入节点
            HistoryDbLink.AddNode(currFolder);
            #region "测试添加了几个节点"
            //HistoryDbLink.Count++;
            //nodeCount.Text = HistoryDbLink.Count.ToString();
            #endregion
            ShowFoldersOrFiles(currFolder);           
        }
        /// <summary>
        /// 导航，向后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (!HistoryDbLink.IsCurrentBeforeFirst)
            {
                HistoryDbLink.GoBack();
                Folder tf = HistoryDbLink.CurrElement;
                ShowFoldersOrFiles(tf);
                btnGoForward.IsEnabled = true;
                if (HistoryDbLink.IsCurrentBeforeFirst)
                    btnGoBack.IsEnabled = false;
            }
        }
        /// <summary>
        /// 导航，向前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoForward_Click(object sender, RoutedEventArgs e)
        {
            if (!HistoryDbLink.IsCurrentAfterLast)
            {
                HistoryDbLink.GoFoward();
                Folder tf = HistoryDbLink.CurrElement;
                ShowFoldersOrFiles(tf);
                btnGoBack.IsEnabled = true;
                if (HistoryDbLink.IsCurrentAfterLast)
                    btnGoForward.IsEnabled = false;
            }
        }
        /// <summary>
        /// 单击TreeView或者双击DataGrid时显示文件夹内容
        /// </summary>
        /// <param name="folder"></param>
        private void ShowFoldersOrFiles(Folder folder)
        {
            //dgrd_Display.Items.Clear();
            ObservableCollection<FileSys> folderAndFiles = folder.FolderAndFiles;
            //创建视图用来过滤排序
            //foreach (var folderAndfile in folderAndFiles)
            //{
            //    dgrd_Display.Items.Add(folderAndfile);
            //}
            dgrd_Display.ItemsSource = folderAndFiles;
            dgrdView = (ListCollectionView)CollectionViewSource.GetDefaultView(dgrd_Display.ItemsSource);
            btnGoBack.IsEnabled = true;
            //对当前文件夹进行监视
            FileSystemWatcher watcher = new FileSystemWatcher();
            try
            {
                watcher.Path = folder.FullPath;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            watcher.NotifyFilter = NotifyFilters.LastWrite
                | NotifyFilters.FileName
                | NotifyFilters.DirectoryName;
            watcher.Filter = "";
            watcher.Created += Watcher_Created;
            watcher.EnableRaisingEvents = true;
        }
        /// <summary>
        /// 刷新GridView中的文件夹
        /// </summary>
        /// <param name="folderAndFiles"></param>
        private void Refresh(ObservableCollection<FileSys> folderAndFiles)
        {
            dgrd_Display.Items.Clear();
            foreach (var folderAndfile in folderAndFiles)
            {
                dgrd_Display.Items.Add(folderAndfile);
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            //Folder folder = new Folder();
            //folder.FullPath = Directory.GetParent(e.FullPath).FullName;
            //ObservableCollection<FileSys> folderAndFiles = folder.FolderAndFiles;
            //Action<ObservableCollection<FileSys>> action = (ff) =>
            //{
            //    Refresh(ff);
            //};
            //dgrd_Display.Dispatcher.BeginInvoke(action,folderAndFiles);

            Action action = () =>
            {
                Folder folder = new Folder();
                folder.FullPath = e.FullPath;
                dgrd_Display.Items.Add(folder);
            };
            dgrd_Display.Dispatcher.BeginInvoke(action);
        }
        /// <summary>
        /// 对DataGrid进行分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBy(object sender, RoutedEventArgs e)
        {
            if (dgrdView == null)
            { return; }
            MenuItem cm = e.Source as MenuItem;
            if (cm == null)
            { return; }
            dgrdView.GroupDescriptions.Clear();
            switch (cm.Name)
            {
                case "cmName":
                    dgrdView.GroupDescriptions.Add(new PropertyGroupDescription("Name", new FileNameGrouper()));
                    break;
                case "cmTime":
                    dgrdView.GroupDescriptions.Add(new PropertyGroupDescription("ChangeTime", new FileNameGrouper()));
                    break;
                case "cmType":
                    dgrdView.GroupDescriptions.Add(new PropertyGroupDescription("FType", new FileNameGrouper()));
                    break;
                case "cmSize":
                    {
                        dgrdView.GroupDescriptions.Add(new PropertyGroupDescription("Size",new FileSizeGrouper()));
                        showContextMenu.Text = "选中大小分组";
                    }
                    break;
                default:
                    break;
            }
        }

        private void SortBy(object sender, RoutedEventArgs e)
        {
            MenuItem cm = e.Source as MenuItem;
            if (dgrdView == null || cm == null)
            {
                return;
            }
            switch (cm.Name)
            {
                case "cmSortByName":
                    dgrdView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    break;
                case "cmSortByTime":
                    dgrdView.SortDescriptions.Add(new SortDescription("ChangeTime", ListSortDirection.Ascending));
                    break;
                case "cmSortByType":
                    dgrdView.SortDescriptions.Add(new SortDescription("FType", ListSortDirection.Ascending));
                    break;
                case "cmSortBySize":
                    dgrdView.SortDescriptions.Add(new SortDescription("Size", ListSortDirection.Ascending));
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 对获取到的Icon类型的图标进行转换
    /// （DataGrid中的显示文件图标）
    /// </summary>
    [ValueConversion(typeof(Icon), typeof(ImageSource))]   // Icon是源类型，ImageSource是目标类型。
    public class IconConverter : IValueConverter           //继承了 IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Icon icon = (Icon)value;
                Bitmap bitmap = icon.ToBitmap();
                IntPtr hBitmap = bitmap.GetHbitmap();
                ImageSource bitmapSource =
                Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
                return bitmapSource;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        //逆操作用不到
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

