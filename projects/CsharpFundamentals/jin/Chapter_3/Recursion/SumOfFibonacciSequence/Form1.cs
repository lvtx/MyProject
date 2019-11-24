using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetFibonacciNumber
{
    /// <summary>
    /// 递归计算斐波那契数列
    /// 实践之后发现太卡了，
    /// 直接用for循环效果很好
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static long FibonacciUseLong(int n)
        {
            if(n == 1)
            {
                return 1;
            }
            if(n == 2)
            {
                return 1;
            }
            long ret = FibonacciUseLong(n - 1) + FibonacciUseLong(n - 2);
            if (long.MaxValue < ret)
            {
                throw new OverflowException("超过了本计算机能计算的最大整数！");
            }
            return ret;
        }
        public static BigInteger FibonacciUseBigInteger(int n)
        {
            if (n == 1)
            {
                return 1;
            }
            if (n == 2)
            {
                return 1;
            }
            BigInteger ret = FibonacciUseLong(n - 1) + FibonacciUseLong(n - 2);
            return ret;
        }
        private void ShowCalculateResult()
        {
            try
            {
                if (rdoLong.Checked)
                    lblResult.Text = FibonacciUseLong((int)numericUpDown1.Value).ToString();
                else
                    lblResult.Text = FibonacciUseBigInteger((int)numericUpDown1.Value).ToString();
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
            }
        }
        private void RdoLong_CheckedChanged(object sender, EventArgs e)
        {
            ShowCalculateResult();
        }

        private void rdoBigInteger_CheckedChanged(object sender, EventArgs e)
        {
            ShowCalculateResult();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ShowCalculateResult();
        }
    }
}
