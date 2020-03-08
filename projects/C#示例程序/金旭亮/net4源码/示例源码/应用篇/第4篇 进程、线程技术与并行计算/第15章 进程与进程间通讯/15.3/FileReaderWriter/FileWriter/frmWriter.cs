using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UseFileSystemWatch
{
    public partial class frmWriter : Form
    {
        public frmWriter()
        {
            InitializeComponent();
        }

        string FileName;


        //打开文件
        private void OpenFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog1.FileName;

                using (StreamReader reader = new StreamReader(FileName, Encoding.Default))
                {
                    txtEditor.Text = reader.ReadToEnd();
                  
                }
                try
                {
                    Text = "正在编辑:" + FileName;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }


        //保存文件
        private void OnSave()
        {

            if (String.IsNullOrEmpty(FileName))
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)

                    FileName = saveFileDialog1.FileName;
                else
                    return;
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read), Encoding.Default))
                {
                    writer.Write(txtEditor.Text);
                }
                Text = "正在编辑:" + FileName;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OnSave();
        }
    }
}