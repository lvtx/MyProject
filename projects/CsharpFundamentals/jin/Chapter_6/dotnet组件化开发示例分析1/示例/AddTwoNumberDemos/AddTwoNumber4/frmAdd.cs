using MathLibrary;
using System;
using System.Windows.Forms;

namespace AddTwoNumber4
{
    public partial class frmAdd : Form
    {
        public frmAdd()
        {
            InitializeComponent();
        }
        private MathOpt calculator = new MathOpt();

        private void Add()
        {
            try
            {
                int num1 = Convert.ToInt32(txtNumber1.Text);
                int num2 = Convert.ToInt32(txtNumber2.Text);
                lblResult.Text = calculator.Add(num1, num2).ToString();
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
            }
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
