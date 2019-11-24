using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtherFormToMainForm
{
    public partial class frmSub : Form
    {
        public frmSub()
        {
            InitializeComponent();
        }

        public string frmTxtUserInput
        {
            get { return txtUserInput.Text; }
        }


        public string SendValueToMainForm()
        {
            return txtUserInput.Text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
