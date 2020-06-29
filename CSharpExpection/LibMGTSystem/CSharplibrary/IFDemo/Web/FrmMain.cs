using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using System.Threading;

namespace Web
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        public Admin admin = null;
        BookInfoManager_UI book;
        ReaderManager_UI reader;
        Thread t;
        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = "状态：" + admin.LoginType + "\"" + admin.LoginId + "\"已登录.";
            //图书管理窗体的绑定
            book = new BookInfoManager_UI();
            Control.CheckForIllegalCrossThreadCalls = false;
            //t = new Thread(delegate() { banding(book, tabPage1); });
            //t.IsBackground = true;
            //t.Start();
            banding(book, tabPage1);

            //读者管理窗体的绑定
            reader = new ReaderManager_UI();
            reader.book = book;
            banding(reader, tabPage2);

            //借还历史记录窗体的绑定
            banding(new BorrowHostory_UI(), tabPage3);
        }

        //TabControl的TabPage绑定窗体的公共方法
        public void banding(Form form, TabPage page)
        {
            page.Controls.Clear();
            form.FormBorderStyle = FormBorderStyle.None;
            form.StartPosition = FormStartPosition.Manual;
            form.Size = page.Size;
            form.TopLevel = false;
            page.Controls.Add(form);
            form.Show();

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();  
        }

        //menuStrip新增图书
        private void menuStripAddBookInfo_Click(object sender, EventArgs e)
        {
            book.btnAddBookInfo_Click(null, null);
        }

        private void toolStripBorrow_Click(object sender, EventArgs e)
        {
            BorrowManager_UI b = new BorrowManager_UI();
            b.ShowDialog();
        }
        private void menuStripAddReaderInfo_Click(object sender, EventArgs e)
        {
            reader.btnAddReaderInfo_Click(null, null);
        }

        private void 图书类别管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add a = new Add();
            a.tabName = "图书类型";
            a.Manager = this.book;
            a.ShowDialog();
        }
        private void 读者类型管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add a = new Add();
            a.tabName = "读者类型";
            a.readerManager = this.reader;
            a.ShowDialog();
        }

        //图书管理
        private void toolStripButtonBookManager_Click(object sender, EventArgs e)
        {
            this.tabControl.SelectedIndex = 0;
        }
        //读者管理
        private void toolStripButtonReaderManager_Click(object sender, EventArgs e)
        {
            this.tabControl.SelectedIndex = 1;
        }
        //借还管理
        private void toolStripButtonBorrowReturn_Click(object sender, EventArgs e)
        {
            this.tabControl.SelectedIndex = 2;
        }

        private void 画图工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mspaint.exe");
        }

        private void 计算器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }




        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void 操作员管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (admin.LoginType == "超级管理员")
            {
                //AdminManager_UI a = new AdminManager_UI();
                //a.ShowDialog();
            }
            else
            {
                MessageBox.Show("只有超级管理员能使用此功能");
            }
        }

        

        private void menuStripBorrow_Click(object sender, EventArgs e)
        {
            BorrowManager_UI b = new BorrowManager_UI();
            b.ShowDialog();
        }

        private void 图书借还记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tabControl.SelectedIndex = 2;
        }




    }
}
