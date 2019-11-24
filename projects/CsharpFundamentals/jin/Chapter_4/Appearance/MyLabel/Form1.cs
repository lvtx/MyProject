using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLabel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int count = 1;
        private void Button1_Click(object sender, EventArgs e)
        {
            lblInfo.Text = string.Format("点击{0}", count);
            count++;
        }
    }
}
