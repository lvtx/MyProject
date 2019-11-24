using System;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseReference
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        //从窗体点击计数器
        private int counter = 0;
        /// <summary>
        /// 显示累计按钮单击次数的结果
        /// </summary>
        public void ShowCounter()
        {
            counter++;
            lblInfo.Text = counter.ToString();
        }

        private void btnShowOtherForm_Click(object sender, EventArgs e)
        {
            frmOther frm = new frmOther(this);
            //在屏幕上显示窗体
            frm.Show();
        }
    }
}
