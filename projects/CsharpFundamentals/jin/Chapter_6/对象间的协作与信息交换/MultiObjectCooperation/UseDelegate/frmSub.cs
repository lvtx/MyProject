using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UseDelegate
{
    public partial class frmSub : Form
    {
        public frmSub()
        {
            InitializeComponent();
        }
        public void ShowCounts(int counts)
        {
            lblInfo.Text = counts.ToString();
        }
    }
}
