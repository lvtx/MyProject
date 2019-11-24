using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

namespace GetFibonacciNumber
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            ShowCalculateResult();
        }
        private long FibonacciUseLong(int NumberIndex)
        {
            long previousValue = -1;
            long currentResult = 1;
            for (var i = 0; i <= NumberIndex; ++i)
            {
                if (long.MaxValue - currentResult < previousValue)
                {
                    throw new OverflowException("超过了本计算机能计算的最大整数！");
                }
                long sum = currentResult + previousValue;
                previousValue = currentResult;
                currentResult = sum;
            }
            return currentResult;
        }

        private BigInteger FibonacciUseBigInteger(int NumberIndex)
        {
            BigInteger previousValue = -1;
            BigInteger currentResult = 1;
            for (var i = 0; i <= NumberIndex; ++i)
            {
                BigInteger sum = currentResult + previousValue;
                previousValue = currentResult;
                currentResult = sum;
            }
            return currentResult;
        }

        /// <summary>
        /// 显示计算结果
        /// </summary>
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ShowCalculateResult();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ShowCalculateResult();
        }

        private void rdoLong_CheckedChanged(object sender, EventArgs e)
        {
            ShowCalculateResult();
        }

    }
}
