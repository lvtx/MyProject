using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwoWayCommunicationInForms
{
    public partial class frmMain : Form
    {
        private frmSub subForm1 = null;
        public frmMain()
        {
            InitializeComponent();
            subForm1 = new frmSub(this);
            subForm1.Show();
        }
        //将子窗体的进度传入
        public void ChangeMainFormProgressValue(int progressBarValue)
        {
            numProjectProgress.Value = progressBarValue;
        }

        private void numProjectProgress_ValueChanged(object sender, EventArgs e)
        {
            subForm1.ChangeSubFormProgressValue(numProjectProgress.Value);
        }
    }
}
