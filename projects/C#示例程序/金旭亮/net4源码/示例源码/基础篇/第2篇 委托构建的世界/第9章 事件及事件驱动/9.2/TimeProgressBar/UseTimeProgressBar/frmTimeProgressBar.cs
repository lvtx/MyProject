using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UseTimeProgressBar
{
    public partial class frmTimeProgressBar : Form
    {
        public frmTimeProgressBar()
        {
            InitializeComponent();
        }

        private void timeProgressBar1_TimeIsUp()
        {
            MessageBox.Show("Time Is Up!");
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            timeProgressBar1.SetTimeSpan((int)updnHour.Value, (int)updnMinute.Value, (int)updnSecond.Value);

        }

        
    }
}