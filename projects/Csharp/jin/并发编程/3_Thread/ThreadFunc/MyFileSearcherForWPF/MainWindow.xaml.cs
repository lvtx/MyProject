using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFileSearcherForWPF.Search;
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
using System.ComponentModel;
using System.IO;

namespace MyFileSearcherForWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitfileSearcher();
            InitBdSFInfoToSb();
            InitBeginDirectory();
            InitBdBeginDirToCBox();
        }
        FileSearcher fileSearcher = new FileSearcher();
        /// <summary>
        /// 初始化fileSearcher对象
        /// </summary>
        public void InitfileSearcher()
        {
            fileSearcher.SearchInfoObj = new SearchInfo();
            fileSearcher.ProgressChanged += fileSearcher_ProgressChanged;
            fileSearcher.RunWorkerCompleted += fileSearcher_RunWorkerCompleted;
        }

        private void fileSearcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblInfo.Content = e.UserState.ToString();
        }

        private void fileSearcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
                lblInfo.Content = string.Format("用户取消了操作，到目前为止，找到了{0}个文件", fileSearcher.FoundFiles.Count);
            else
                lblInfo.Content = string.Format("搜索完成，共找到{0}个文件。", fileSearcher.FoundFiles.Count);
        }

        /// <summary>
        /// 将SearchInfo绑定到SearchBox
        /// </summary>
        public void InitBdSFInfoToSb()
        {
            Binding bindingSFInfoToSB = new Binding("SearchFile") { Source = fileSearcher.SearchInfoObj };
            bindingSFInfoToSB.Mode = BindingMode.OneWayToSource;
            this.SrCt.SearchBox.SetBinding(TextBox.TextProperty, bindingSFInfoToSB);
        }

        /// <summary>
        /// 初始化ComboBox开始目录
        /// </summary>
        public void InitBeginDirectory()
        {
            string[] beginDirectorys = new string[] 
            {
                @"D:\windows\files\package\安装包",
                @"D:\windows\files\document\ebook\教材\大学\软件\软件工程",
                @"D:\windows\files\document\ebook\教材\大学\软件\操作系统"
            };
            foreach (var directory in beginDirectorys)
            {
                cBoxBeginDirectory.Items.Add(directory);
            }
            cBoxBeginDirectory.SelectedIndex = 0;
        }
        /// <summary>
        /// 将开始目录绑定到ComboBox上
        /// </summary>
        public void InitBdBeginDirToCBox() 
        {
            Binding bdBeginDirToCBox = new Binding("BeginDirectory") { Source = fileSearcher.SearchInfoObj };
            bdBeginDirToCBox.Mode = BindingMode.OneWayToSource;
            cBoxBeginDirectory.SetBinding(ComboBox.SelectedValueProperty, bdBeginDirToCBox);
        }
        
        /// <summary>
        /// 显示当前已找到的文件
        /// </summary>
        /// <param name="files"></param>
        private void ShowSearchedFiles(FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                string str = null;
                if (file.Length > 100000000)
                {
                    str = string.Format(file.Name + " 文件大小为 " + file.Length + "bytes");
                    lstSearchFiles.Items.Add(str);
                }
            }
        }

        private void btnStartButton_Click(object sender, RoutedEventArgs e)
        {
            fileSearcher.ShowSearchResult = this.ShowSearchedFiles;
            fileSearcher.RunWorkerAsync();
        }
    }
}
