using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseStaticMethod
{
    
    public partial class frmOther : Form
    {
        public frmOther()
        {
            InitializeComponent();
        }

       


       
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            Program.NotifyClicked();
        }

    }
}
