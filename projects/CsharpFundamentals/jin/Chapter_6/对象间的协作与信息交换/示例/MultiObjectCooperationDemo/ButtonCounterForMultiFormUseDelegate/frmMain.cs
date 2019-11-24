using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseDelegate
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
        private void ShowCounter()
        {
            counter++;
            lblInfo.Text = counter.ToString();
        }

        private void btnShowOtherForm_Click(object sender, EventArgs e)
        {
            frmOther frm = new frmOther();
            //将主窗体的方法“挂接”到从窗体对象上
            frm.CallBackMethod = this.ShowCounter;
            //在屏幕上显示从窗体
            frm.Show();
        }
       
    }
}
