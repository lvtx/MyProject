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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            new frmEnable().ShowDialog();
        }

        private void btnVisible_Click(object sender, EventArgs e)
        {
            new frmVisiable().ShowDialog();
        }

        private void btnAnchor_Click(object sender, EventArgs e)
        {
            new frmAnchor().ShowDialog();
        }

        private void btnDock_Click(object sender, EventArgs e)
        {
            new frmDock().ShowDialog();
        }
    }
}
