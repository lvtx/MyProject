using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace VisitVisualControl1
{
   

    public partial class frmVisitControl : Form
    {
        public frmVisitControl()
        {
            InitializeComponent();
         
        }

        

        private void SetLabelText()
        {
            label1.Text = "Hello";
        }

       
        private void btnVisitLabel_Click(object sender, EventArgs e)
        {
            //以下这句将引发InvalidOperationException
            Thread th = new Thread(SetLabelText);
           
            th.Start();
        }
    }
}