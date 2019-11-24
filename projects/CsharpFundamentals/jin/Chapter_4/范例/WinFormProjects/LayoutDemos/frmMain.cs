using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutDemos
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSplitContainer_Click(object sender, EventArgs e)
        {
            new frmSplitContainer().ShowDialog();
        }

        private void btnFlowLayout_Click(object sender, EventArgs e)
        {
            new frmFlowLayout().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmTableLayout().ShowDialog();
        }
    }
}
