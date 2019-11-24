using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MaxMinValueForGP2
{
    public partial class frmGPExample2 : Form
    {
        public frmGPExample2()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 三个字段，引用被处理的三种类型的数组
        /// </summary>
        private char[] CharArray = new char[10];
        private int[] IntArray = new int[10];
        private ComplexNum[] ComplexArray = new ComplexNum[10];

        private void OnButtonClick()
        {
            //填充数组
            FillArray();

            if (rdoInteger.Checked)
                ShowResult<int>(MaxMin<int>.GetMaxMinVauleFromArray(IntArray));

            if (rdoChar.Checked)
                ShowResult<char>(MaxMin<char>.GetMaxMinVauleFromArray(CharArray));

            if (rdoComplex.Checked)
                ShowResult<ComplexNum>(MaxMin<ComplexNum>.GetMaxMinVauleFromArray(ComplexArray));

        }
        /// <summary>
        /// 使用泛型方法显示数据处理结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ret"></param>
        private void ShowResult<T>(Pair<T> ret)
        {
            lblMax.Text = "最大值：" + ret.Max.ToString();
            lblMin.Text = "最小值：" + ret.Min.ToString();
        }

        //用随机数据填充数组,并显示在列表框中
        private void FillArray()
        {

            int i;
            Random ran = new Random();
            lstData.Items.Clear();
            if (rdoInteger.Checked)
                for (i = 0; i <= IntArray.GetUpperBound(0); i++)
                {
                    IntArray[i] = ran.Next(0, 100);
                    lstData.Items.Add(IntArray[i]);
                }
            if (rdoChar.Checked)
                for (i = 0; i <= CharArray.GetUpperBound(0); i++)
                {
                    int charCode;
                    charCode = ran.Next(0, 26);
                    CharArray[i] = (char)('A' + charCode);
                    lstData.Items.Add(CharArray[i].ToString() + " :ASC码值＝" + ((int)CharArray[i]).ToString());

                }

            int a, b;

            if (rdoComplex.Checked)
                for (i = 0; i <= ComplexArray.GetUpperBound(0); i++)
                {
                    a = ran.Next(0, 10);
                    b = ran.Next(0, 10);
                    ComplexArray[i] = new ComplexNum(a, b);
                    lstData.Items.Add(ComplexArray[i].ToString() + " 模＝" + ComplexArray[i].GetMod().ToString());

                }
        }

        private void btnFillArray_Click(object sender, EventArgs e)
        {
            OnButtonClick();//泛型类版本
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }



}