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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();           
        }
            
        private void btnShowDialog_Click(object sender, EventArgs e)
        {
            frmSub frm = new frmSub(); 
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
                lblInfo.Text = frm.frmTxtUserInput;
            else
                lblInfo.Text = "用户取消了输入";
        }
    }
}
