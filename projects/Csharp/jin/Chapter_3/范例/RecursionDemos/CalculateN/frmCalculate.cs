using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CalculateN
{
    public partial class frmCalculate : Form
    {
        public frmCalculate()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(txtN.Text);
            lblResult.Text = n.ToString()+"!="+Factorial(n).ToString();
            //lblResult.Text = n.ToString() + "!=" + Factorial2(n).ToString();

        }
        //����n!���õݹ�ʵ��
        private long Factorial(int n)
        {
            if (n == 1)
               return 1;
            long ret;
            ret = Factorial(n - 1) * n;
            return ret;

        }
        //����n!���õ���ʵ��
        private long Factorial2(int n)
        {
            long result = 1;
            for(int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}