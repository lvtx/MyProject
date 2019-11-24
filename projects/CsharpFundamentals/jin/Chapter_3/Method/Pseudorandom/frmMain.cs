using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private int m = 1000;  //模
        private float c = 4;  //增量，注意，c>=0 and c<m
        private float GetNextRanNumber(int m, float a, float c, float prevNum)
        {
            return (a * prevNum + c) % m;
        }
        private void GenerateRandomSequence(int nums, float seed)
        {
            float lastNum, nextNum;
            lastNum = seed;
            //清空富文本框
            richTextBox1.Clear();
            //追加字符串到富文本框中
            richTextBox1.AppendText(seed.ToString());
            for (int i = 0; i < nums; i++)
            {
                nextNum = GetNextRanNumber(m, a, c, lastNum);
                richTextBox1.AppendText(" ," + nextNum);
                lastNum = nextNum;
            }
        }
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            float seed = float.Parse(txtSeed.Text);
            int nums = int.Parse(txtNumbers.Text);
            GenerateRandomSequence(nums, seed);
        }
    }
}
