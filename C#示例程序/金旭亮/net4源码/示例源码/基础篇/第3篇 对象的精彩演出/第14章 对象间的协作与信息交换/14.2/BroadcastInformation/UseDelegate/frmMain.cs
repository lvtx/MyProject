using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseDelegate
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private Action<int> ReceiverMethods;
        private void btnNewForm_Click(object sender, EventArgs e)
        {
            NewForm();
        }

        
        private  void NewForm()
        {
            frmOther frm = new frmOther();
            ReceiverMethods += frm.ShowCounter;
            frm.Show();
        }

        private int counter = 0;//计数器
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            if (ReceiverMethods != null)
                ReceiverMethods(counter);
        }
    }
}
