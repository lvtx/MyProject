using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiObjectCooperation
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private List<frmSub> frmSubs = new List<frmSub>();
        private void NewAndShowSubForm()
        {
            frmSub frm = new frmSub();
            frmSubs.Add(frm);
            frm.Show();
        }

        private void btnNewForm_Click(object sender, EventArgs e)
        {
            NewAndShowSubForm();
        }
        private int counts = 0;
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counts++;
            foreach (var item in frmSubs)
            {
                item.MyLblShow(counts);
            }
        }
    }
}
