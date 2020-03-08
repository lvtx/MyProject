using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace CalculateFolderSize
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private string path;

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFolderDialog= new FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFolderName.Text = openFolderDialog.SelectedPath;
                path = openFolderDialog.SelectedPath;
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Files.GetAllFile(path);
            sw.Stop();
            txtTime.Text = sw.ElapsedMilliseconds.ToString();
            ObservableCollection<FileInfo> files = Files.AllFile;
            lstShowFiles.ItemsSource = files;
            txtFilesCount.Text = lstShowFiles.Items.Count.ToString();
            txtFolderSize.Text = Files.Size.ToString() + "GB";
        }
    }
}
