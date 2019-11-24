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
    public delegate void MyClickDelegate();
    public partial class frmSub : Form
    {
        public frmSub()
        {
            InitializeComponent();
        }

        public event MyClickDelegate MyClick;
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            MyClick();
        }
    }
}
