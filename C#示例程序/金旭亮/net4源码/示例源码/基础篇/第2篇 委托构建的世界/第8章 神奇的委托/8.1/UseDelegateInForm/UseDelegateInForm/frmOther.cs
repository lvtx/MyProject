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

        public ShowInfoDelegate recorder;//��¼��Ϣ�ġ��ˡ�

        public frmOther()
        {
            InitializeComponent();
        }

        private int counter =0;//������
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter ++;
            if (recorder != null)
                recorder(counter.ToString());
        }


    }
}