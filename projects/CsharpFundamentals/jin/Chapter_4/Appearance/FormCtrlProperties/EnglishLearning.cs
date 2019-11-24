using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormCtrlProperties
{
    public partial class EnglishLearning : Form
    {
        public EnglishLearning()
        {
            InitializeComponent();
        }
        int count = 0;
        private void Button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            count++;
            lblCount.Text = string.Format("口头复读{0}次", count);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            count = 0;
            lblCount.Text = string.Format("口头复读{0}次", count);
        }
    }
}
