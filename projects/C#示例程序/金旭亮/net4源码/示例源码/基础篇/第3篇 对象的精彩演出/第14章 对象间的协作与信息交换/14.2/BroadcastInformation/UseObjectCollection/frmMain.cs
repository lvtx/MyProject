using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseObjectCollection
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnNewForm_Click(object sender, EventArgs e)
        {
            NewForm();
        }

        private List<frmOther> OtherForms = new List<frmOther>();
        private  void NewForm()
        {
            frmOther frm = new frmOther();
            OtherForms.Add(frm);
            frm.Show();
        }

        private int counter = 0;//计数器
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            foreach (frmOther frm in OtherForms)
                frm.ShowCounter(counter);
        }
    }



}
