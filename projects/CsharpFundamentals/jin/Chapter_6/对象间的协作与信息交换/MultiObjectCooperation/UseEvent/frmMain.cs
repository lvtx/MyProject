using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UseEvent
{
    public delegate void MyClickDelegate(int counter);
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        public event MyClickDelegate MyClick;
        private int counter = 0;
        public void NewAndShowSubForm()
        {
            frmSub mySubForm = new frmSub();
            MyClick += mySubForm.ShowCounter;
            mySubForm.Show();
        }
        private void btnNewForm_Click(object sender, EventArgs e)
        {
            NewAndShowSubForm();
        }

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            if (MyClick != null)
            {
                MyClick(counter);
            }
        }
    }
}
