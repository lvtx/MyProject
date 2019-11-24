using ProgressBarAndTimer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgressBarAndTimer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnIncrease_Click(object sender, EventArgs e)
        {
            if (progressBar1.Value + 10 >= progressBar1.Maximum)
            {
                progressBar1.Value = progressBar1.Maximum;
            }
            else
                progressBar1.Value += 10;
        }

        private void BtnDecrease_Click(object sender, EventArgs e)
        {
            if(progressBar1.Value <= progressBar1.Minimum)
            {
                progressBar1.Value = progressBar1.Minimum;
            }
            else
            {
                progressBar1.Value -= 10;
            }
        }

        private void BtnTimer_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (timer1.Enabled == true)
            {
                btnTimer.Image = Resources.EnableClock;
            }
            else
                btnTimer.Image = Resources.DisableClock;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar2.Value + 1 > progressBar1.Maximum)
            {
                progressBar2.Value = progressBar1.Minimum;
            }
            else
                progressBar2.Value += 1;
        }

        private void BtnSwitchIco_MouseHover(object sender, EventArgs e)
        {
            btnSwitchIco.Image = Resources.DisableClock;
        }

        private void BtnSwitchIco_MouseLeave(object sender, EventArgs e)
        {
            btnSwitchIco.Image = Resources.EnableClock;
        }
    }
}
