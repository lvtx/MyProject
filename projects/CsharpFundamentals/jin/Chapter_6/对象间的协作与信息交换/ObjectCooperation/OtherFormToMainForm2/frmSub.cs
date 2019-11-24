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
    public partial class frmSub : Form
    {
        private frmMain mainFrm = null;
        public frmSub(frmMain mainFrm)
        {
            InitializeComponent();
            this.mainFrm = mainFrm;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserInput.Text.Trim()))
            {
                mainFrm.Report("用户输入为空");
                this.Close();
                return;
            }
            mainFrm.Report(txtUserInput.Text.Trim());
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mainFrm.Report("用户取消了输入");
            this.Close();
        }
    }
}
