using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddTwoNumber2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        public void Add()
        {
            try
            {
                int num1 = Convert.ToInt32(txtNumber1.Text);
                int num2 = Convert.ToInt32(txtNumber2.Text);
                lblResult.Text = Convert.ToString(num1 + num2);
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

        private void txtNumber1_TextChanged(object sender, EventArgs e)
        {
            Add();
        }

        private void txtNumber2_TextChanged(object sender, EventArgs e)
        {
            Add();
        }
    }
}
