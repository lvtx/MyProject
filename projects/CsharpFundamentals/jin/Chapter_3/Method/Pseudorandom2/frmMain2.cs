using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudorandom2
{
    public partial class frmMain2 : Form
    {
        public frmMain2()
        {
            InitializeComponent();
        }
        Random ran = new Random(System.Environment.TickCount);
        private void GenerateRandomSequence(int nums)
        {
            richTextBox1.AppendText(Convert.ToString(ran.Next(1, 1000)));
            for (int i = 0; i < nums; i++)
            {
                richTextBox1.AppendText(" ," + ran.Next(1, 1000));
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            int nums = int.Parse(txtNumbers.Text);
            GenerateRandomSequence(nums);
        }
    }
}
