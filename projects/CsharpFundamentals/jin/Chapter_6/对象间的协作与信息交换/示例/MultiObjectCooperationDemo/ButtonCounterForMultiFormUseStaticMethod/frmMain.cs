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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public void ShowCounter(int counter)
        {
            lblCount.Text = counter.ToString();
        }

        private void btnNewOtherForm_Click(object sender, EventArgs e)
        {
            frmOther frm = new frmOther();
            frm.Show();
        }


    }
}
