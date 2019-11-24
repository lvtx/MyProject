using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResponseToEvents
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            foreach (Control ctl in groupBox1.Controls)
            {
                if (ctl is TextBox)
                    (ctl as TextBox).KeyDown += this.EnterToTab;
            }
        }
        private void EnterToTab(object sender,KeyEventArgs e)
        {
            TextBox txt = null;
            if(e.KeyCode == Keys.Enter)
            {
                groupBox1.SelectNextControl(sender as Control, true, true, true, true);
                txt = (sender as TextBox);
                if (txt != null)
                    txt.SelectAll();
            }
        }
    }
}
