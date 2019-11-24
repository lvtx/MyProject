using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseEvent
{
    public delegate void MyClickDelegate(int counter);

    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        //主窗体定义一个MyClick事件
        public event MyClickDelegate MyClick;
        private void btnNewForm_Click(object sender, EventArgs e)
        {
            NewForm();
        }


        private void NewForm()
        {
            frmOther frm = new frmOther();
            //从窗体对象将ShowCounter方法挂接到MyClick事件上，响应这个事件
            MyClick += frm.ShowCounter;
            frm.Show();
        }

        private int counter = 0;//计数器
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            //触发事件
            if (MyClick != null)
            {
                MyClick(counter);
            }

        }
    }
}
