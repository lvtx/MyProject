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
    public partial class FrmDock : Form
    {
        public FrmDock()
        {
            InitializeComponent();
        }

        private void RdoLeft_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Left;
        }

        private void RdoNone_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.None;
        }

        private void RdoRight_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Right;
        }

        private void RdoTop_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Top;
        }

        private void RdoBottom_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Bottom;
        }

        private void RdoFill_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Fill;
        }
    }
}
