using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatusStripDemo
{
    public partial class frmStatusStrip : Form
    {
        public frmStatusStrip()
        {
            InitializeComponent();
        }

        private void btnShowTime_Click(object sender, EventArgs e)
        {
            timerForCurrentTime.Enabled = !timerForCurrentTime.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToLongTimeString();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("菜单项一");
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("菜单项二");
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("菜单项三");
        }

        private void btnShowProgress_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Value = 0;
            timerForProgress.Enabled = true;
            btnShowProgressBar.Enabled = false;
        }

        private void timerForProgress_Tick(object sender, EventArgs e)
        {
            if (toolStripProgressBar1.Value < 100)
                toolStripProgressBar1.Value += 5;
            else
            {
                toolStripProgressBar1.Visible = false;
                timerForProgress.Enabled = false;
                btnShowProgressBar.Enabled = true;
            }


        }
    }
}
