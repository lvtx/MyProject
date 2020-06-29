using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UseDelegateInForm
{
    public partial class frmOther : Form
    {

        public ShowInfoDelegate recorder;//记录信息的“人”

        public frmOther()
        {
            InitializeComponent();
        }

        private int counter =0;//计数器
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter ++;
            if (recorder != null)
                recorder(counter.ToString());
        }


    }
}