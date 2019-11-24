using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDemo
{
    public partial class frmButton : Form
    {
        public frmButton()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button 1 is pressed!");
        }
        private int count = 0;
        //展示如何修改按钮的文本
        private void button2_Click(object sender, EventArgs e)
        {
            count++;
            button2.Text = string.Format("我被扁了{0}次", count);
        }
    }
}
