using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormCtrlProperties
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void BtnEnable_Click(object sender, EventArgs e)
        {
            new FrmEnable().ShowDialog();
        }

        private void BtnVisible_Click(object sender, EventArgs e)
        {
            new FrmVisiable().ShowDialog();
        }

        private void BtnDock_Click(object sender, EventArgs e)
        {
            new FrmDock().ShowDialog();
        }

        private void BtnAnchor_Click(object sender, EventArgs e)
        {
            new FrmAnchor().ShowDialog();
        }
    }
}
