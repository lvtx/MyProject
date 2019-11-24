using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormCtrlProperty
{
    public partial class frmVisiable : Form
    {
        public frmVisiable()
        {
            InitializeComponent();
        }

        private void btnShowOrHide_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = !pictureBox1.Visible;
            if (pictureBox1.Visible)
            {
                btnShowOrHide.Text = "藏起来!";
            }
            else
            {
                btnShowOrHide.Text = "现身！";
            }
        }
    }
}
