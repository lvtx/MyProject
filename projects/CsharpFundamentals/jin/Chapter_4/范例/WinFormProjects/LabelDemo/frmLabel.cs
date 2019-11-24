using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabelDemo
{
    public partial class frmLabel : Form
    {
        public frmLabel()
        {
            InitializeComponent();
        }

        private int counter = 0;
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            lblInfo.Text = "我被单击了" + counter + "次";
        }
    }
}
