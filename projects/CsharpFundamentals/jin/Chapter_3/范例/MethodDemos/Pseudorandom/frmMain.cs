using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pseudorandom
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private float a = 3; //乘数,注意，a>=2 and a<m
        private int m = 100;  //模
        private float c = 4;  //增量，注意，c>=0 and c<m

        //依据公式计算出下一个随机数
        private float GetNextRanNumber(int m, float a, float c, float prevNum)
        {
            return (a * prevNum + c) % m;
        }

        //按指定的种子Seed生成nums个随机数
        private void GenerateRandomSequence(int nums, float seed)
        {
            float lastNum, nextNum;
            lastNum = seed;
            //清空富文本框
            RichTextBox1.Clear();
            //追加字符串到富文本框中
            RichTextBox1.AppendText(seed.ToString());
            for (int i = 0; i < nums; i++)
            {
                nextNum = GetNextRanNumber(m, a, c, lastNum);
                RichTextBox1.AppendText(" ," + nextNum);
                lastNum = nextNum;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int nums = Convert.ToInt32(txtNumbers.Text);
            float seed = Convert.ToSingle(txtSeed.Text);
            GenerateRandomSequence(nums, seed);
        }

    }
}
