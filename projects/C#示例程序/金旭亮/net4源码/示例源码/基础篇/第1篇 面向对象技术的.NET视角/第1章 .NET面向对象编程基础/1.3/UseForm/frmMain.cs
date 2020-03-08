using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnShowOtherForm_Click(object sender, EventArgs e)
        {
            //创建从窗体对象
            frmOther frm = new frmOther();
            //显示从窗体
            frm.Show();
        }
    }
}
