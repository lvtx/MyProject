using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseEvent
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private int counter = 0;
        private void ShowCounter()
        {
            counter++;
            lblCount.Text = counter.ToString();
        }
        private void btnNewOtherForm_Click(object sender, EventArgs e)
        {
            frmSub MySubForm = new frmSub();
            MySubForm.Show();
            MySubForm.MyClick += this.ShowCounter;
        }
    }
}
