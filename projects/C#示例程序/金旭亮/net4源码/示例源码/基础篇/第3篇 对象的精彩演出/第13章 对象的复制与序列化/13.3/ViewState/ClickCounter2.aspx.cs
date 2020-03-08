using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ClickCounter2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)    //如果是第一次请求
        {
            counter = 0;    
            ViewState["Counter"]=counter;   //将counter变量默认值保存到ViewState对象中
        }
        else
            counter = (int)ViewState["Counter"]; //从ViewState对象中取出上次保存的counter变量值
    }

    private int counter=0;  //计数器
    protected void btnClickMe_Click(object sender, EventArgs e)
    {
        counter++;  //累加计数器
        ViewState["Counter"] = counter;         //将counter变量值保存到ViewState对象中
        lblInfo.Text = "您单击了" + counter.ToString() + "次按钮。";
    }
}
