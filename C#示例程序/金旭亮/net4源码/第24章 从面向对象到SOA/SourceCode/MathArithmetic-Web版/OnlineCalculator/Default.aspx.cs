using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MathArithmetic;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCalculator_Click(object sender, EventArgs e)
    {
        string expr = PreProcess.ClearAllSpace(txtExpression.Text);
        try
        {
            PreProcess.CheckExprValidate(expr);
            InfixAlgorithm obj = new InfixAlgorithm();
            lblResult.Text = "½á¹ûÎª£º"+obj.Calculate(expr).ToString();
          }
        catch (Exception ex)
        {
            lblResult.Text = ex.ToString();
        }

    }
}
