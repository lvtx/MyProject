using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UseEventHandler2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnLoanFromHuang_Click(object sender, EventArgs e)
        {
            LoanFromHuang();
        }

        private void btnSum_Click(object sender, EventArgs e)
        {
            ShowLoan();
        }

        private int LoanMoney = 0;
        private int LoanCount = 0;

        public void LoanFromHuang()
        {
            btnSum.Enabled = true;
            LoanMoney = 0;
            btnSum.Click += new EventHandler(btnSum_Click);
            lblLoanCount.Text = string.Format("{0}次",++LoanCount);
        }
        public void ShowLoan()
        {
            LoanMoney += 100;         
            Thread.Sleep(300);
            lblLoanMoney.Refresh();
            lblLoanMoney.Text = string.Format("{0}元", LoanMoney);
            btnSum.Enabled = false;
        }
    }
}
