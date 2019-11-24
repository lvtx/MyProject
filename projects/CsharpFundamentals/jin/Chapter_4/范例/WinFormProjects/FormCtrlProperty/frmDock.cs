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
    public partial class frmDock : Form
    {
        public frmDock()
        {
            InitializeComponent();
        }

        private void rdoNone_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.None;
        }

        private void rdoLeft_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Left;
        }

        private void rdoRight_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Right;
        }

        private void rdoTop_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Top;
        }

        private void rdoBottom_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Bottom;
        }

        private void rdoFill_CheckedChanged(object sender, EventArgs e)
        {
            button1.Dock = DockStyle.Fill;
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
