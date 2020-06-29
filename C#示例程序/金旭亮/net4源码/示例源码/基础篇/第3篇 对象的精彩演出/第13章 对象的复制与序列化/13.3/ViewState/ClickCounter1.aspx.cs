using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class ClickCounter1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    private int counter=0;  //计数器
    protected void btnClickMe_Click(object sender, EventArgs e)
    {
        counter++;  //累加计数器
        lblInfo.Text = "您单击了" + counter.ToString() + "次按钮。";

    }
}
