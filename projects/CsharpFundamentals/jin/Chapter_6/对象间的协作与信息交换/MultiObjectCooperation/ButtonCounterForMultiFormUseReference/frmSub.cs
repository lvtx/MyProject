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
    public partial class frmSub : Form
    {
        public frmSub(frmMain myMainForm)
        {
            InitializeComponent();
            this.myMainForm = myMainForm;
        }

        private frmMain myMainForm = null;

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            if(myMainForm != null)
                myMainForm.ShowCounter();
        }
    }
}
