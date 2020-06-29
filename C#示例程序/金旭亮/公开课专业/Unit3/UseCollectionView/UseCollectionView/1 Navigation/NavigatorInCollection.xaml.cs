using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UseCollectionView
{
    /// <summary>
    /// NavigatorInCollection.xaml 的交互逻辑
    /// </summary>
    public partial class NavigatorInCollection : Window
    {
        public NavigatorInCollection()
        {
            InitializeComponent();
        }

        private ListCollectionView lstView = null;
        ListView lstFiles = null;
        private void btnChooseDirectory_Click(object sender, RoutedEventArgs e)
        {
            string dir = App.ChooseDirectory();
            if (string.IsNullOrEmpty(dir))
            {
                txtDir.Text = "当前没有选中文件夹";
                FilesArea.Visibility = Visibility.Collapsed;
                lstView = null;
                return;
            }
            else
                txtDir.Text = "当前文件夹：" + dir;

            //获取用于显示文件列表的ListView控件的引用
            lstFiles = ListViewContainer.Content as ListView;

            //提取数据并绑定显示
            lstFiles.ItemsSource = App.GetFiles(dir);
            //获取集合视图对象
            lstView = CollectionViewSource.GetDefaultView(lstFiles.ItemsSource) as ListCollectionView;
            FilesArea.Visibility = Visibility.Visible;
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            lstView.MoveCurrentToFirst();
            //确保选择项可见
            lstFiles.ScrollIntoView(lstFiles.SelectedItem);
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (lstView.IsCurrentBeforeFirst == false)
                lstView.MoveCurrentToPrevious();
            else
                lstView.MoveCurrentToFirst();
            //让当前选中项可见
            lstFiles.ScrollIntoView(lstFiles.SelectedItem);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (lstView.IsCurrentAfterLast == false)
                lstView.MoveCurrentToNext();
            else
                lstView.MoveCurrentToLast();
            //让当前选中项可见
            lstFiles.ScrollIntoView(lstFiles.SelectedItem);
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            lstView.MoveCurrentToLast();
            //让当前选中项可见
            lstFiles.ScrollIntoView(lstFiles.SelectedItem);
        }
    }
}
