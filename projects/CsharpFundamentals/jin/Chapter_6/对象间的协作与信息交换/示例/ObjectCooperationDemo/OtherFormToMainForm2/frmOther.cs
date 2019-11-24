using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtherFormToMainForm2
{
    public partial class frmOther : Form
    {
        //用于保存主窗体对象引用
        private frmMain mainForm = null;
        //从外部将主窗体对象“注入”进来
        public frmOther(frmMain main)
        {
            InitializeComponent();
            mainForm = main;
        }
        //在事件响应方法中“回调”主窗体对象的Report()方法
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUserInput.Text.Trim()))
            {
                mainForm.Report("用户没有输入文本");
            }
            else
            {
                mainForm.Report(txtUserInput.Text);
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mainForm.Report("用户取消了操作");
            Close();
        }
    }
}
