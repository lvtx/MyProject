using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Swap
{
    public partial class Form1 : Form
    {
        Elephant lucinda;
        Elephant lloyd;
        public Form1()
        {
            InitializeComponent();
            lucinda = new Elephant() { EarSize = 33, Name = "Lucinda" };
            lloyd = new Elephant() { EarSize = 40, Name = "Lloyd" };
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            lloyd.WhoAmI();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            lucinda.WhoAmI();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Elephant holder;
            holder = lloyd;
            lloyd = lucinda;
            lucinda = holder;
            MessageBox.Show("Objects swapped");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            lloyd = lucinda;
            lloyd.EarSize = 4321;
            lloyd.WhoAmI();
        }
    }
}
