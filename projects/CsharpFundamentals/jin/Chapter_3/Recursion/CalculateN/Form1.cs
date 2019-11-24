using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculateN
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 递归计算器的实现(图形化)
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        public long CalcuateFactorial(long n)
        {
            if (n == 1)
            {
                return 1;
            }
            else
            {
                return CalcuateFactorial(n - 1) * n;
            }
        }
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            long n = long.Parse(txtN.Text);
            long ret = CalcuateFactorial(n);
            lblResult.Text = txtN.Text + "!=" + ret.ToString();
        }
    }
}
