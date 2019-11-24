using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UseEventHandler2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 借的钱
        /// </summary>
        private int LoanMoney = 0;
        /// <summary>
        /// 借钱的次数
        /// </summary>
        private int LoanCount = 0;
        /// <summary>
        /// 向黄世仁借钱
        /// </summary>
        private void LoanFromHuang()
        {
            btnSum.Enabled = true;
            LoanMoney = 0;
            //追加事件响应函数
            btnSum.Click += new EventHandler(btnSum_Click);
            lblLoanCount.Text = string.Format("{0} 次", ++LoanCount);
        }
        /// <summary>
        /// 杨白劳看看欠了黄世仁多少钱
        /// </summary>
        private void ShowLoanState()
        {
            LoanMoney += 100;
            Thread.Sleep(300);  
            lblLoanMoney.Refresh();
            lblLoanMoney.Text = string.Format("{0} 元", LoanMoney);
            btnSum.Enabled = false;
        }
        private void btnLoanFromHuang_Click(object sender, EventArgs e)
        {
            LoanFromHuang();
        }     

        void btnSum_Click(object sender, EventArgs e)
        {
            ShowLoanState();
        }
    }
}
