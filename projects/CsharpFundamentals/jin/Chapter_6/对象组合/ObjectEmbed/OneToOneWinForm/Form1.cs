using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneToOneWinForm
{
    public partial class Form1 : Form
    {
        private int count;
        public Form1()
        {
            InitializeComponent();
            count = 0;
        }
        
        private void btnClick_Click(object sender, EventArgs e)
        {
            count++;
            lblText.Text = string.Format("单击{0}次",count);
        }
    }
}
