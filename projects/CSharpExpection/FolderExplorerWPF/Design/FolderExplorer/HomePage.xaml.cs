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
            VisualTreeDisplay treeDisplay = new VisualTreeDisplay();
            treeDisplay.ShowVisualTree(this.TestDoubleClick);
            treeDisplay.Show();
        }
        /// <summary>
        /// 初始化一个导航用的双向链表
        /// </summary>
        DbLink<Folder> HistoryDbLink = new DbLink<Folder>();
        /// <summary>
        /// 头节点
        /// </summary>
        //DbNode headNode;
        /// <summary>
        /// 当前节点
        /// </summary>
        //DbNode currentNode = new DbNode();
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
                newFolder.iIcon = IconInfo.GetDriverIcon(path[0], false);
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
            FileSys currFolder = dgrd_Display.CurrentItem as FileSys;
            var path = currFolder.FullPath;
            if (currFolder is Folder)
            {
                Folder f = currFolder as Folder;
                HistoryDbLink.AddNode(f);
                ShowFoldersOrFiles(f);
            }
            else
            {
                Process.Start(path);
            }
            //DbNode newNode = new DbNode();
            //if (headNode == null)
            //{
            //    headNode = newNode;
            //    newNode.Prev = null;
            //}
            //else
            //{
            //    currentNode.Next = newNode;
            //    newNode.Prev = currentNode;
            //}
            //newNode.TFolder = currFolder;
            //newNode.Next = null;
            //currentNode = newNode;
        }

        private void DisplaySubFoldersAndFilesOnDGRD(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Folder currFolder = tvwDisplayFolder.SelectedItem as Folder;
            HistoryDbLink.AddNode(currFolder);
            ShowFoldersOrFiles(currFolder);
            //DbNode newNode = new DbNode();
            //if (headNode == null)
            //{
            //    headNode = newNode;
            //    newNode.Prev = null;
            //}
            //else
            //{
            //    currentNode.Next = newNode;
            //    newNode.Prev = currentNode;
            //}
            //newNode.TFolder = currFolder;
            //newNode.Next = null;
            //currentNode = newNode;
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
            dgrd_Display.Items.Clear();
            ObservableCollection<FileSys> folderAndFiles = folder.FolderAndFiles;
            foreach (var folderAndfile in folderAndFiles)
            {
                dgrd_Display.Items.Add(folderAndfile);
            }
            btnGoBack.IsEnabled = true;
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

