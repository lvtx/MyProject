using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFileSearcher
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnChooseSearchDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtBeginDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBeginSearch_Click(object sender, EventArgs e)
        {
            if (txtSearchFileName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入要查找的文件名：", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearchFileName.Focus();
                return;
            }
            if (this.txtBeginDirectory.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择查找起始目录：", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnChooseSearchDirectory.Focus();
                return;
            }
            SearchInfo info = new SearchInfo { BeginDirectory = this.txtBeginDirectory.Text.Trim(), SearchFile = txtSearchFileName.Text.Trim() };
            frmFileSearcher frm = new frmFileSearcher(info);
            frm.Show();
           
        }
    }
}
