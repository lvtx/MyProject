using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ButtonCounterForMultiForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private int counter = 0;
        /// <summary>
        /// 显示累计按钮单击次数的结果
        /// </summary>
        public void ShowCounter()
        {
            counter++;
            lblInfo.Text = counter.ToString();
        }

        private void btnShowOtherForm_Click(object sender, EventArgs e)
        {
            frmOther frm = new frmOther();
            //建立两个对象间的关联
            frm.MainForm = this;
            //在屏幕上显示窗体
            frm.Show();
        }
       
    }
}
