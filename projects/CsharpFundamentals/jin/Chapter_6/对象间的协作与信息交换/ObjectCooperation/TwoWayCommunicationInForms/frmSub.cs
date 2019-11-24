using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwoWayCommunicationInForms
{
    public partial class frmSub : Form
    {
        private frmMain mainForm1 = null;
        public frmSub(frmMain frm)
        {
            InitializeComponent();
            mainForm1 = frm;
        }

        //private int progressValue = 0;

        public void ChangeSubFormProgressValue(decimal progressBarValue)
        {
            progressBar1.Value = (int)progressBarValue;
            ShowProgressBarValue();
        }

        private void ShowProgressBarValue()
        {           
            lblValue.Text = string.Format(progressBar1.Value + "%");     
        }
        private void btnIncrease_Click(object sender, EventArgs e)
        {
            if (progressBar1.Value >= progressBar1.Maximum)
                return;
            else
                progressBar1.Value++;
            mainForm1.ChangeMainFormProgressValue(progressBar1.Value);
            ShowProgressBarValue();
        }

        private void btnDecrease_Click(object sender, EventArgs e)
        {
            if (progressBar1.Value <= progressBar1.Minimum)
                return;
            else
                progressBar1.Value--;
            mainForm1.ChangeMainFormProgressValue(progressBar1.Value);
            ShowProgressBarValue();
        }
    }
}
