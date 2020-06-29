using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace VisitVisualControl3
{
    

    public partial class frmMain : Form
    {
        
        public frmMain()
        {
            InitializeComponent();
            
        }

     

        private void ThreadMethod(Object info)
        {
            Action<string> del = delegate(string infovalue)
            {
                lblInfo.Text = infovalue;
            };
            lblInfo.Invoke(del, info);
        }

        private void btnVisitLabel_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(ThreadMethod);
            th.Start(txtUserInput.Text);

        }


    }
}