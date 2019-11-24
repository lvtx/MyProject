using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseEvent
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private int counter = 0;

        /// <summary>
        /// 主窗体响应从窗体的MyClick事件
        /// </summary>
        private void ResponseToOtherFormMyClickEvent()
        {
            counter++;
            lblCount.Text = counter.ToString();
        }

        private void btnNewOtherForm_Click(object sender, EventArgs e)
        {
            frmOther frm = new frmOther();
            //挂接从窗体事件响应函数
            frm.MyClick += this.ResponseToOtherFormMyClickEvent;
            frm.Show();
        }


    }
}
