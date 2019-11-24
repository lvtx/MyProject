using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContainerDemo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnPanel_Click(object sender, EventArgs e)
        {
            new frmPanel().ShowDialog();
        }

        private void btnTabControl_Click(object sender, EventArgs e)
        {
            new frmTabControl().ShowDialog();
        }

        private void btnGroupBox_Click(object sender, EventArgs e)
        {
            new frmGroupBox().ShowDialog();
        }
    }
}
