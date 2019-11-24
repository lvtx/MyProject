using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MathLibrary;

namespace AddTwoNumForWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private MathOpt calculate = new MathOpt();
        private void Add()
        {
            int num1 = Convert.ToInt32(txtNumber1.Text);
            int num2 = Convert.ToInt32(txtNumber2.Text);
            lblResult.Text = Convert.ToString(calculate.Add(num1, num2));
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }
    }
}