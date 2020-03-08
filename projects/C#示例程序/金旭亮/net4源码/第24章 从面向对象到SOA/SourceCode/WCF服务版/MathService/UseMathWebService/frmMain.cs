using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UseMathWebService.localhost;

namespace UseMathWebService
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnCalculator_Click(object sender, EventArgs e)
        {
            try
            {
                MyCalculatorServiceClient client = new MyCalculatorServiceClient();
                lblInfo.Text = "结果为："+client.Calculator(txtExpr.Text).ToString();

            }
            catch (Exception ex )
            {

                lblInfo.Text = ex.Message;
            }
           

        }
    }
}
