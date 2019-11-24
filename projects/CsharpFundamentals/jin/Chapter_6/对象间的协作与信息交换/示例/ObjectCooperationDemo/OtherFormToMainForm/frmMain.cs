using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiFormProgram2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnShowDialog_Click(object sender, EventArgs e)
        {
            frmOther frm = new frmOther();
            //依据从窗体提供的状态信息，决定不同的处理方式
            if (frm.ShowDialog() == DialogResult.OK)
                lblInfo.Text = frm.UserInput;
            else
                lblInfo.Text = "用户取消了输入。";
        }
    }
}
