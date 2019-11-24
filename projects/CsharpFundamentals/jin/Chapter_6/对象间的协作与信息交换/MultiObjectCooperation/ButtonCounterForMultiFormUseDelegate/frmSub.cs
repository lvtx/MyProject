using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseDelegate
{
    public partial class frmSub : Form
    {
        private frmMain MyMainForm;
        public frmSub(frmMain MyMainForm)
        {
            InitializeComponent();
            this.MyMainForm = MyMainForm;
        }
        public delegate void MyDelegate();
        public MyDelegate MainFormCounter;
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            if(MainFormCounter != null)
                MainFormCounter();
        }
    }
}
