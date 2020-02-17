using System;
using System.Collections.Generic;
using System.IO;
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
    /// FilterData.xaml 的交互逻辑
    /// </summary>
    public partial class FilterData : Window
    {
        public FilterData()
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

            //提取数据并绑定显示
            lstFiles = ListViewContainer.Content as ListView;
            lstFiles.ItemsSource = App.GetFiles(dir);

            lstView = CollectionViewSource.GetDefaultView(lstFiles.ItemsSource) as ListCollectionView;
            FilesArea.Visibility = Visibility.Visible;


        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            txtInput.SelectAll();
            txtInput.Focus();
            if (lstView==null)
                return;
            if ((txtInput.Text.Trim().Length == 0))
            {
                lstView.Filter = null;
                return;
            }
            //设定过滤器对象
            lstView.Filter = (item) => {
                FileInfo file = item as FileInfo;
                if (file!=null && file.Name.IndexOf(txtInput.Text.Trim()) != -1)
                    return true;
                else
                    return false;
            };
        }
    }
}
