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
    public partial class frmSub : Form
    {
        public frmSub()
        {
            InitializeComponent();
        }
        public void ShowCounter(int counter)
        {
            lblInfo.Text = counter.ToString();
        }
    }
}
