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
    /// GroupData.xaml 的交互逻辑
    /// </summary>
    public partial class GroupData : Window
    {
        public GroupData()
        {
            InitializeComponent();
        }
        private ListCollectionView lstView = null;
        private ListView lstFiles = null;
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
            rdoNoGroup.IsChecked = true;

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (lstView == null)
                return;
            RadioButton rdo = e.OriginalSource as RadioButton;
            if (rdo == null)
                return;
            lstView.GroupDescriptions.Clear();
            switch (rdo.Name)
            {
                case "rdoNoGroup":
                    break;
                case "rdoGroupByFileName":
                    lstView.GroupDescriptions.Add(new PropertyGroupDescription("Name", new FileExtensionGrouper()));
                    break;
                case "rdoGroupByFileSize":
                    lstView.GroupDescriptions.Add(new PropertyGroupDescription("Length", new FileSizeGrouper()));
                    break;
            }
        }
    }
}
