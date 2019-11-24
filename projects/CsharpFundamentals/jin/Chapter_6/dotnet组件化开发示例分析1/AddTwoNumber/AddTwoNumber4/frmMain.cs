using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathLibrary;

namespace AddTwoNumber4
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        MathOpt calculate = new MathOpt();
        private void Add()
        {
            int num1 = Convert.ToInt32(txtNumber1.Text);
            int num2 = Convert.ToInt32(txtNumber2.Text);
            lblResult.Text = Convert.ToString(calculate.Add(num1, num2));
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void txtNumber2_TextChanged(object sender, EventArgs e)
        {
            Add();
        }

        private void txtNumber1_TextChanged(object sender, EventArgs e)
        {
            Add();
        }
    }
}
