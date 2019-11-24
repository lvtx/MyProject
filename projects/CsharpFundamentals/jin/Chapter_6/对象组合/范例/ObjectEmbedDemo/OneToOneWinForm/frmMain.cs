using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneToOneWinForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            ShowCounter(0);
        }

        private void ShowCounter(int Counter)
        {
            lblInfo.Text = String.Format("按钮被单击了{0}次", Counter);
        }

        private int Counter = 0;

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            ShowCounter(++Counter);
        }
    }
}
