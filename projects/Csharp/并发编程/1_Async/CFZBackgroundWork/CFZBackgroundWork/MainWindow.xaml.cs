using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using WinForms = System.Windows.Forms;

namespace CFZBackgroundWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            folderBrowserDialog = new WinForms.FolderBrowserDialog();
            folderBrowserDialog.Description = "选择文件夹";
            fSearcher = new FileSearcher();
            fSearcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(Searcher_RunWorkerCompleted);
        }

        private void Searcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStop.IsEnabled = false;
            btnSearch.IsEnabled = true;
            if (e.Cancelled)
            {
                txtStatus.Text = "搜索任务取消，已搜索到" + lstShowFiles.Items.Count.ToString() + "个文件";
            }
            else
            {
                txtStatus.Text = "搜索任务已完成";
            }
        }

        private WinForms.FolderBrowserDialog folderBrowserDialog = null;
        /// <summary>
        /// 文件路径名
        /// </summary>
        string path;
        /// <summary>
        /// 文件总大小
        /// </summary>
        int Size;
        /// <summary>
        /// 文件个数
        /// </summary>       
        int Count;
        /// <summary>
        /// 文件搜索器
        /// </summary>
        FileSearcher fSearcher = null;
        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                this.txtFolderName.Text = folderBrowserDialog.SelectedPath;
                path = folderBrowserDialog.SelectedPath;
            }
        }
        private void FileSize()
        {
            txtFilesCount.Text = Size.ToString();
        }
        private void ShowSearchedFiles(FileInfo[] files)
        {
            foreach (var file in files)
            {
                lstShowFiles.Items.Add(file.Name);
                Size = lstShowFiles.Items.Count;
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            lstShowFiles.Items.Clear();
            BeginSearch();
            btnStop.IsEnabled = true;
            btnSearch.IsEnabled = false;
        }
        /// <summary>
        /// 用于搜索的方法
        /// </summary>
        public void BeginSearch()
        {
            fSearcher.Path = path;
            fSearcher.DisplayFileName = this.ShowSearchedFiles;
            fSearcher.DisplayFileCount = this.FileSize;
            fSearcher.RunWorkerAsync(300);
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            fSearcher.CancelAsync();
            btnStop.IsEnabled = false;
            btnSearch.IsEnabled = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (fSearcher.IsBusy)
            {
                MessageBox.Show("搜索正在进行中，请先取消搜索任务后再关闭程序", "提示信息",
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                e.Cancel = true;
            }
        }
    }
}
