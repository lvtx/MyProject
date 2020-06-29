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

//定义一个可序列化的类
[Serializable]
public class CounterClass
{
    public int counter = 0;
}

public partial class ClickCounter3 : System.Web.UI.Page
{
    private CounterClass obj;  //计数器对象

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)    //如果是第一次请求
        {
            obj = new CounterClass();      
            ViewState["Counter"] = obj;   //将CounterClass对象保存到ViewState对象中
        }
        else
            obj = ViewState["Counter"] as CounterClass; //从ViewState对象中取出上次保存的CounterClass对象
    }

    
    protected void btnClickMe_Click(object sender, EventArgs e)
    {
        obj.counter++;
        ViewState["Counter"] = obj;         //将CounterClass对象保存到ViewState对象中
        lblInfo.Text = "您单击了" + obj.counter.ToString() + "次按钮。";
    }
}

