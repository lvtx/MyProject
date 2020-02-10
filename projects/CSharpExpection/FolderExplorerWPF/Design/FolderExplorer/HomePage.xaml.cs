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
            InitializeTreeView();
        }
        /// <summary>
        /// 初始化文件树
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
            treeFileSystem.ItemsSource = folders;
        }
    }

    /// <summary>
    /// 对获取到的Icon类型的图标进行转换
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
