using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace VisitVisualControl2
{
    
    public partial class frmVisitControl : Form
    {
        public frmVisitControl()
        {
            InitializeComponent();
        }

     
        private void ThreadMethod()
        {
            Action del = delegate()
            {
                label1.Text = "Hello";
            };
            label1.Invoke(del);
            //或
            //label1.BeginInvoke(del);
        }

        private void btnVisitLabel_Click(object sender, EventArgs e)
        {
           Thread th = new Thread(ThreadMethod);
           th.Start();
        }
    }
}