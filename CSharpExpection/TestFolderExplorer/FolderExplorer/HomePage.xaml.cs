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
            //InitializeTreeView(); // 初始化文件树
            MyFolders myFolders = new MyFolders();
            ListCollectionView listingDataView = (ListCollectionView)CollectionViewSource.GetDefaultView(myFolders.Folders);            
            view = listingDataView;
            tvwDisplayFolder.ItemsSource = view;
            //foreach (var item in view)
            //{
            //    txtViewElement.Text += (item as Folder).FullPath + '\r';
            //}
            //txtViewElement.Text += "currentItem " + (view.CurrentItem as Folder).FullPath;
            //txtViewElement.Text += "Position" + (view.CurrentPosition);
        }
        ListCollectionView view;
        /// <summary>
        /// 初始化文件树
        /// （TreeView控件中）
        /// </summary>
        public void InitializeTreeView()
        {
            ObservableCollection<Folder> folders = new ObservableCollection<Folder>();
            Drive d = new Drive();
            DriveInfo[] drives = DriveInfo.GetDrives();          
            foreach (var drive in drives)
            {
                string path = drive.Name;
                Folder newFolder = new Folder();
                newFolder.FullPath = path;
                newFolder.Name = d.GetDriveName(drive);
                newFolder.iIcon = IconInfo.GetDriverIcon(path[0],false);
                folders.Add(newFolder);
            }
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
                            var path = currFolder.FullPath;
                            if (currFolder is Folder)
                            {
                                Folder f = currFolder as Folder;
                                ShowFoldersOrFiles(f);
                            }
                            else
                            {
                                Process.Start(path);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 单击TreeView或者双击DataGrid时显示文件夹内容
        /// </summary>
        /// <param name="folder"></param>
        private void ShowFoldersOrFiles(Folder folder)
        {
            try
            {
                dgrd_Display.Items.Clear();
                ObservableCollection<FileSys> folderAndFiles = folder.FolderAndFiles;
                foreach (var folderAndfile in folderAndFiles)
                {
                    dgrd_Display.Items.Add(folderAndfile);
                }
                btnGoBack.IsEnabled = true;
            }
            //FileSystemWatcher watcher = new FileSystemWatcher();
            //try
            //{
            //    watcher.Path = folder.FullPath;
            //}
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void DisplayFolderOrFilesOnDGRD(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Folder currFolder = tvwDisplayFolder.SelectedItem as Folder;
            ShowFoldersOrFiles(currFolder);
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
    public class MyFolders
    {
        private ObservableCollection<Folder> folders;
        public ObservableCollection<Folder> Folders 
        {
            get
            {
                Drive d = new Drive();
                DriveInfo[] drives = DriveInfo.GetDrives();
                folders = new ObservableCollection<Folder>();
                foreach (var drive in drives)
                {
                    string path = drive.Name;
                    Folder newFolder = new Folder();
                    newFolder.FullPath = path;
                    newFolder.Name = d.GetDriveName(drive);
                    newFolder.iIcon = IconInfo.GetDriverIcon(path[0], false);
                    folders.Add(newFolder);               
                }
                return folders;
            }
        }
    }
}
