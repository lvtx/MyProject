using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObjectCooperation
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private int count = 0;

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            ShowInfo();
        }
        private void ShowInfo()
        {
            count++;
            lblCount.Text = string.Format("您单击了{0}次按钮。", count);
        }
    }
}
