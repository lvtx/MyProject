using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiFormProgram2
{
    public partial class frmOther : Form
    {
        public frmOther()
        {
            InitializeComponent();
        }
        //供主窗体对象提取信息的公有属性
        public string UserInput
        {
            get
            {
                return txtUserInput.Text;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //设置本窗体关闭的“原因”，以供主窗体进行查询
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //设置本窗体关闭的“原因”，以供主窗体进行查询
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
