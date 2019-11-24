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
    //delegate void DealWithSubFormMethod(int counts);
    public partial class frmMain : Form
    {
        
        public frmMain()
        {
            InitializeComponent();
        }
        private int counts = 0;
        Action<int> DealWithSubFormMethod;
        private void btnNewForm_Click(object sender, EventArgs e)
        {
            frmSub MyFrmSub = new frmSub();
            DealWithSubFormMethod += MyFrmSub.ShowCounts;
            MyFrmSub.Show();
        }

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counts++;
            if(DealWithSubFormMethod != null)
            {
                DealWithSubFormMethod(counts);
            }
        }
    }
}
