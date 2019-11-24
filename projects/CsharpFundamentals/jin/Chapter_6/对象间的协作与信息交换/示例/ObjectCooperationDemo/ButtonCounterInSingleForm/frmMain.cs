using System;
using System.Windows.Forms;

namespace ButtonCounterInSingleForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        //被应用程序处理的信息
        private int counter = 0;
        //当用户点击按钮时，此方法被调用，整个信息处理流程被启动
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            //处理信息
            counter++;
            //传输信息
            ShowInfo();
        }
        private void ShowInfo()
        {
            //标签对象通过其公有属性接收外界传入的信息
            lblCount.Text = string.Format("你单击了{0}次按钮。", counter);
        }
    }
}
