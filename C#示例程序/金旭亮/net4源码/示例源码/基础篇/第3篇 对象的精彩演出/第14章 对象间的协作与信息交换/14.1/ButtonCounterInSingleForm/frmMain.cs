using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ButtonCounterInSingleForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private int counter = 0;
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            ShowInfo();
        }
        private void ShowInfo()
        {
            lblCount.Text = string.Format("你单击了{0}次按钮。", counter);
        }
    }
}
