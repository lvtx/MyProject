using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Menu
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Saved!!!");
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            fileToolStripMenuItem.Enabled = !fileToolStripMenuItem.Enabled;
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            menuStripFile.Visible = !menuStripFile.Visible;
            menuStripEdit.Visible = !menuStripEdit.Visible;
        }

        private void buttonShowContextMenu_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(sender as Control, 20, 25);
        }
    }
}
