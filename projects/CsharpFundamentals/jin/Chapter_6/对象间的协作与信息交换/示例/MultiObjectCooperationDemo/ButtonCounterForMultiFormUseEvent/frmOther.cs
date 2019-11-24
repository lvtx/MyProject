using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseEvent
{
    /// <summary>
    /// 定义事件委托
    /// </summary>
    /// <param name="?"></param>
    public delegate void MyClickDelegate();

    public partial class frmOther : Form
    {
        public frmOther()
        {
            InitializeComponent();
        }

        //自定义一个事件
        public event MyClickDelegate MyClick;


        //在按钮单击事件中激发MyClick事件
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            if (MyClick != null)
            {
                MyClick();
            }
        }

    }
}
