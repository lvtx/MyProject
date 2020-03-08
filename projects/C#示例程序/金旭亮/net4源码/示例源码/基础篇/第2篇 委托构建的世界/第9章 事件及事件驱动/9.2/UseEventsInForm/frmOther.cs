using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UseEventsInForm
{
    public delegate void ValueChanged(String value);

    public partial class frmOther : Form
    {

        public event ValueChanged ButtonClicked; //自定义事件


        public frmOther()
        {
            InitializeComponent();
        }

        private int counter = 0;//计数器
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter ++;
            if (ButtonClicked != null)
            ButtonClicked(counter.ToString()); //激发事件
        }

    }
}