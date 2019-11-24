using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuDemo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            FileMenuItem.Enabled = !FileMenuItem.Enabled;
        }

        private int SaveCount = 0;
        private void mnuSave_Click(object sender, EventArgs e)
        {
            SaveCount++;
            mnuSave.Text = string.Format("保存({0})", SaveCount);
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            menuStripEdit.Visible = !menuStripEdit.Visible;
            menuStripFile.Visible = !menuStripFile.Visible;
        }

        private void buttonShowContextMenu_Click(object sender, EventArgs e)
        {
            contextMenuStripExample.Show(sender as Control,20,25);
        }
    }
}
