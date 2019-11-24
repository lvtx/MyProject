using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _this
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int counter;
        private void BtnClick_Click(object sender, EventArgs e)
        {
            counter++;
            this.lblCount.Text = string.Format("点击{0}次",this.counter);
        }
    }
}
