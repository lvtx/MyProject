using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CovarianceExample
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            btnTestCovariance.MouseClick += MultiHandler;
            btnTestCovariance.KeyDown += MultiHandler;

        }

        private void MultiHandler(object sender, System.EventArgs e) 
        {
            if (e is KeyEventArgs)
                lblInfo.Text = string.Format("您敲了{0}键", (e as KeyEventArgs).KeyCode.ToString());
            if(e is MouseEventArgs)
                lblInfo.Text = "您按了鼠标上的键";
        }

    }
}
