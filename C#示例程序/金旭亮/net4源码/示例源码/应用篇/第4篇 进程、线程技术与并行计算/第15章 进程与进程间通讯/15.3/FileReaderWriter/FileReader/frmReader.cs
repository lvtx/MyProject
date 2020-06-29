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
        //ѡ���ĵ�����ʾ
        private void OnChooseFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog1.FileName;
                //װ���ĵ�����ʾ
                LoadFile();
                //�趨�ļ���ز���
                SetupFileSystemWatcher();

                Text = "��أ�" + FileName;


            }
        }
        //�趨�ļ���ز���
        private void SetupFileSystemWatcher()
        {
            //�趨��ص��ļ���
            fileSystemWatcher1.Filter = Path.GetFileName(FileName);
            //�趨��ص��ļ�����
            fileSystemWatcher1.Path = Path.GetDirectoryName(FileName);
            //ָ��ֻ����ļ���С�ĸı�
            fileSystemWatcher1.NotifyFilter = NotifyFilters.Size;
        }
        //װ���ĵ�
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
            //����װ���ļ�����ʾ    
            LoadFile();
        }

    }
}