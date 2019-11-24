using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormCtrlProperty
{
    public partial class frmEnable : Form
    {
        public frmEnable()
        {
            InitializeComponent();
        }

        private void rdoEnable_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void rdoDisable_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }
    }
}
