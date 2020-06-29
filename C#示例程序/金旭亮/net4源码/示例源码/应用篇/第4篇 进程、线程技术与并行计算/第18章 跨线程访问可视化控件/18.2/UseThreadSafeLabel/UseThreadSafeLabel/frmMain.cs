using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace UseThreadSafeLabel
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void TheadMethod(Object info)
        {
            threadSafeLabel1.Text = info.ToString();
        }
     

        private void btnSetLabelText_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(TheadMethod);
            th.Start(txtUserInput.Text);
        }
    }
}