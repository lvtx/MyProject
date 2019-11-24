using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseReference
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private int counter = 0;
        public void ShowCounter()
        {
            counter++;
            lblInfo.Text = counter.ToString();
        }

        private void btnShowOtherForm_Click(object sender, EventArgs e)
        {
            frmSub MySubForm = new frmSub(this);
            MySubForm.Show();
        }
    }
}
