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
        //将用于引用多个从窗体对象所挂接的“回调”方法
        private Action<int> ReceiverMethods;
        private void btnNewForm_Click(object sender, EventArgs e)
        {
            NewForm();
        }


        private void NewForm()
        {
            frmOther frm = new frmOther();
            //从窗体将自己的ShowCounter方法挂接到主窗体的委托变量ReceiverMethods上
            ReceiverMethods += frm.ShowCounter;
            frm.Show();
        }

        private int counter = 0;//计数器
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            if (ReceiverMethods != null)
            {
                //调用委托调用链中的所有方法，传入当前的计数器值
                ReceiverMethods(counter);
            }

        }
    }
}
