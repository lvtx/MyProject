using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseDelegate
{
    public partial class frmOther : Form
    {
        public frmOther()
        {
            InitializeComponent();
        }
        //用于“回调”主窗体挂接的方法
        public Action CallBackMethod = null;

        private void btnClickMe_Click(object sender, EventArgs e)
        {

            //回调主窗体的方法，显示按钮计数
            if (CallBackMethod != null)
            {
                CallBackMethod();
            }

        }

    }
}
