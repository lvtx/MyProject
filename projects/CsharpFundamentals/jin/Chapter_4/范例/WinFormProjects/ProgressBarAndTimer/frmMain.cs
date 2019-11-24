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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            ShowProgressBarValue(AutoProgressBar.Value);
        }

        private void ShowProgressBarValue(int value)
        {
            lblInfo.Text = string.Format("{0}%", value);
        }
        #region "事件响应"
        private void btnIncrease_Click(object sender, EventArgs e)
        {
            if (ManualProgressBar.Value + 2 > ManualProgressBar.Maximum)
            {
                ManualProgressBar.Value = ManualProgressBar.Maximum;
            }
            else
            {
                ManualProgressBar.Value += 2;
            }

        }

        private void btnDecrease_Click(object sender, EventArgs e)
        {
            if (ManualProgressBar.Value - 2 < ManualProgressBar.Minimum)
            {
                ManualProgressBar.Value = ManualProgressBar.Minimum;
            }
            else
            {
                ManualProgressBar.Value -= 2;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (AutoProgressBar.Value + 2 > AutoProgressBar.Maximum)
            {
                //自动回头
                AutoProgressBar.Value = 0;
            }
            else
            {
                AutoProgressBar.Value += 2;
            }
            ShowProgressBarValue(AutoProgressBar.Value);
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (timer1.Enabled == true)
            {
                btnTimer.Image = Resources.DisableClock;

            }
            else
            {
                btnTimer.Image = Resources.EnableClock;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
