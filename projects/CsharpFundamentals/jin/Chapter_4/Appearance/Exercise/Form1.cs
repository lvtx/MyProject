using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exercise
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static int SumRecursive()//递推
        {
            int ret = 0;
            for (int i = 1; i <= 100; i++)
            {
                ret += i;
            }
            return ret;
        }
        static int SumRecursion(int n)//递归
        {
            if(n == 1)
            {
                return 1;
            }
            return SumRecursion(n - 1) + n;
        }
        private void ShowMessage(TextBox txt, String strMessage)
        {
            txt.Text += strMessage;
        }
        private void RadioSumRecursive_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = string.Format("1+...+100 = {0}(递推)\r\n", SumRecursive());
            countRecursive = 1;
            countRecurstion = 0;
        }

        private void RadioSumRecursion_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = string.Format("1+...+100 = {0}(递进)\r\n", SumRecursion(100));
            countRecursive = 0;
            countRecurstion = 1;
        }
        int countRecursive = 0;
        int countRecurstion = 0;
        String str;
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if(countRecursive == 1)
            {
                str = textBox1.Text;
                //如果使用radioSumRecursive.Checked的话,会多次检查产生重复
                countRecursive = 2;//用于判断radio控件是否被点击
            }
            if (countRecurstion == 1)
            {
                str = textBox1.Text;
                countRecurstion = 2;//用于判断radio控件是否被点击
            }   
            ShowMessage(textBox1, str);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}
