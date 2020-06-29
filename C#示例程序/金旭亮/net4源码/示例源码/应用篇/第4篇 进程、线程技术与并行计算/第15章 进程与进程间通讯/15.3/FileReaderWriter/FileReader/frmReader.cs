using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Reader
{
    public partial class frmReader : Form
    {
        public frmReader()
        {
            InitializeComponent();
        }


        string FileName="";
        //选择文档并显示
        private void OnChooseFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog1.FileName;
                //装入文档并显示
                LoadFile();
                //设定文件监控参数
                SetupFileSystemWatcher();

                Text = "监控：" + FileName;


            }
        }
        //设定文件监控参数
        private void SetupFileSystemWatcher()
        {
            //设定监控的文件名
            fileSystemWatcher1.Filter = Path.GetFileName(FileName);
            //设定监控的文件夹名
            fileSystemWatcher1.Path = Path.GetDirectoryName(FileName);
            //指明只监控文件大小的改变
            fileSystemWatcher1.NotifyFilter = NotifyFilters.Size;
        }
        //装入文档
        private void LoadFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.Default))
                {
                    txtReader.Text = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OnChooseFile();
        }
        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            //重新装入文件并显示    
            LoadFile();
        }

    }
}