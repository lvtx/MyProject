using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainFormToOtherForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            frm = new frmSub();//创建从窗体对象
            frm.Show();//显示从窗体
        }
        private frmSub frm = null;

        private void SendMessageViaPublicMethod()
        {
            if (txtUserInput.Text == "")
            {
                MessageBox.Show("请输入一条信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtUserInput.Focus();
                return;
            }
            frm.ReceiveInfoFromMain(txtUserInput.Text);
        }

        private void SendMessageViaPublicProperty()
        {
            if(txtUserInput.Text == "")
            {
                MessageBox.Show("请输入一条信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtUserInput.Focus();
                return;
            }
            frm.Info = txtUserInput.Text;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //SendMessageViaPublicMethod();
            SendMessageViaPublicProperty();
        }
    }
}
