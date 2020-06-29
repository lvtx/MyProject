using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Word=Microsoft.Office.Interop.Word;

namespace UseCOMServer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //Word对象
      private Word.Application wordapp=null;
       
        //文件名
        private string DocFileName
        {
            get
            {
                return txtFileName .Text;
            }
            set
            {
                txtFileName.Text = value;
            }
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //设定文件名
                DocFileName = openFileDialog1.FileName;
                //激活打印预览按钮
                btnPrintPreview.Enabled = true;
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            //创建Word对象
            if (wordapp == null)
                wordapp = new Microsoft.Office.Interop.Word.Application();
            //使用命名参数和可选参数调用Word的功能
            Word.Document doc = wordapp.Documents.Open(FileName: DocFileName);
           
            //打印预览
            doc.PrintPreview();
            //显示Word主窗体
            wordapp.Visible = true;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
           Close();
        }
    }
}