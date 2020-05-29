using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#if NOOOOOOOOOOOOOO

namespace DigitalPlatform.LibraryServer
{
    /// <summary>
    /// ����������
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:HeadBarControl runat=server></{0}:HeadBarControl>")]
    public class HeadBarControl : WebControl, INamingContainer
    {
        public event GetInboxUnreadCountEventHandler GetInboxUnreadCount;

        // ��ǰ���ڵ���Ŀ
        public HeaderColumn CurrentColumn = HeaderColumn.None;

        // �Ƿ�Ϊͼ�������ַ��
        public bool ImageStyle
        {
            get
            {
                object s = ViewState["ImageStyle"];
                return ((s == null) ? true : (bool)s);
            }

            set
            {
                ViewState["ImageStyle"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        string GetLeftString()
        {
            return

        "<table width='780px' border='0' cellpadding='0' cellspacing='0'>" +
        "	<tr style=\"height: 90px; background-image: url(./shadow_top.jpg); background-repeat: no-repeat;\">" +
//        "		<td class='margin' width='10'></td>" +
        "		<td colspan='3'>";

 //       "			<table width='780px' border='0' cellpadding='0' cellspacing='0'>";
        }

        string GetRightString()
        {
            return 


//		"			</table>" +
//        "       </td><td class='margin' width='10'>" +
		"		</td>" +
		"	</tr>" +


		"	<tr style='height: 26px;' height='26'>" +
		"		<td width='10' style=\"background-image: url(./shadow_left.gif); background-repeat: repeat-y;\"></td>" +
		"		<td width='760' bgcolor='#FFFFFF'>";
        }

        protected override void CreateChildControls()
        {
            SessionInfo sessioninfo = (SessionInfo)this.Page.Session["sessioninfo"];

            bool bPublic = false;
            // public
            if (sessioninfo != null && sessioninfo.Account != null)
            {
                if (sessioninfo.Account.UserID == "public")
                    bPublic = true;
            }

            string strClass = "content";

            // ��ͷ
            LiteralControl literal = new LiteralControl();
            literal.Text = // "<table class='head' cellspacing='1' cellpadding='4'>";
                "<table class='head' width='768' border='0' cellpadding='0' cellspacing='0'>";

            //  width='780px' 
            this.Controls.Add(literal);

            // ͼ��ݱ�־
            literal = new LiteralControl();
            literal.Text = "<tr height='14'></tr><tr class='logo'>"
                + "<td width='12' rowspan='2'></td><td class='logo' nowrap>";
            this.Controls.Add(literal);

            Image image = new Image();
            image.ID = "logo";
            image.ImageUrl = "./arttext.aspx?text="
                + HttpUtility.UrlEncode("����ƽ̨���߷���")
                + "&face="
                + HttpUtility.UrlEncode("��������")
                + "&size="
                + Convert.ToString(30.0F)
                + "&effect=shadow&fontcolor=aaaaaa&backcolor=ffffff";
            // image.ImageUrl = "./logo.jpg";
            this.Controls.Add(image);

            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            this.Controls.Add(literal);


            // LinkButton linkbutton = null;
            // HyperLink hyperlink = null;


            literal = new LiteralControl();
            literal.Text = "<tr class='content'>";
            this.Controls.Add(literal);

            // ��¼�͵ǳ�
            if (this.CurrentColumn == HeaderColumn.Login
                || this.CurrentColumn == HeaderColumn.Login)
                strClass = "content_active";
            else
                strClass = "content";


            literal = new LiteralControl();
            literal.Text = "<td class='"+strClass+"' nowrap>";

            this.Controls.Add(literal);


            /*

            // ��¼
            linkbutton = new LinkButton();
            linkbutton.ID = "login";
            linkbutton.Text = "��¼";
            linkbutton.Click +=new EventHandler(loginButton_Click);
            this.Controls.Add(linkbutton);

            // �ǳ�
            linkbutton = new LinkButton();
            linkbutton.ID = "logout";
            linkbutton.Text = "�ǳ�";
            linkbutton.Click += new EventHandler(logoutButton_Click);
            this.Controls.Add(linkbutton);
             * 
             */

            CreateLink(
                false,
                "login",
    "��¼",
    "./login.aspx");

            CreateLink(
                false,
                "logout",
"�ǳ�",
"./login.aspx?action=logout");

            literal = new LiteralControl();
            literal.Text = "</td>";
            this.Controls.Add(literal);

            // �ҵ�ͼ���

            if (this.CurrentColumn == HeaderColumn.MyLibrary)
                strClass = "content_active";
            else
                strClass = "content";

            literal = new LiteralControl();
            literal.Text = "<td class='" + strClass + "' nowrap>";
            this.Controls.Add(literal);


            /*
            if (this.ImageStyle == false)
            {
                hyperlink = new HyperLink();
                hyperlink.ID = "mylibrary";
                hyperlink.Text = "�ҵ�ͼ���";
                hyperlink.NavigateUrl = "./mylibrary.aspx";
                this.Controls.Add(hyperlink);
            }
            else
            {
                literal = new LiteralControl();
                literal.Text = "<a href='./mylibrary.aspx'>";
                this.Controls.Add(literal);

                Image image = new Image();
                image.ID = "mylibrary";
                image.ImageUrl = "./arttext.aspx?text="
                + HttpUtility.UrlEncode("�ҵ�ͼ���")
                + "&face=Tahoma&size="
                + Convert.ToString(12.0F)
                + "&effect=none";
                image.CssClass = "mylibrary";
                this.Controls.Add(image);

                literal = new LiteralControl();
                literal.Text = "</a>";
                this.Controls.Add(literal);
            }
             */

            if (bPublic == false)
            {
                CreateLink(
                    this.CurrentColumn == HeaderColumn.MyLibrary ? true : false,
                    "mylibrary",
                    "�ҵ�ͼ���",
                    "./mylibrary.aspx");
            }

            literal = new LiteralControl();
            literal.Text = "</td>";
            this.Controls.Add(literal);

            // ����

            if (this.CurrentColumn == HeaderColumn.Search)
                strClass = "content_active";
            else
                strClass = "content";


            literal = new LiteralControl();
            literal.Text = "<td class='" + strClass + "' nowrap>";
            this.Controls.Add(literal);

            /*

            hyperlink = new HyperLink();
            hyperlink.ID = "search";
            hyperlink.Text = "����";
            hyperlink.NavigateUrl = "./search.aspx";
            this.Controls.Add(hyperlink);
             */

            CreateLink(
                this.CurrentColumn == HeaderColumn.Search ? true : false,
                "search",
    "����",
    "./search.aspx");

            literal = new LiteralControl();
            literal.Text = "</td>";
            this.Controls.Add(literal);

            // �ҵ���Ϣ


            if (this.CurrentColumn == HeaderColumn.MyMessage)
                strClass = "content_active";
            else
                strClass = "content";

            literal = new LiteralControl();
            literal.Text = "<td class='" + strClass + "' nowrap>";
            this.Controls.Add(literal);

            /*


            hyperlink = new HyperLink();
            hyperlink.ID = "mymessage";
            hyperlink.Text = "�ҵ���Ϣ";
            hyperlink.NavigateUrl = "./mymessage.aspx";
            this.Controls.Add(hyperlink);
             */
            if (bPublic == false)
            {
                int nUnreadCount = 0;
                if (this.GetInboxUnreadCount != null)
                {
                    GetInboxUnreadCountEventArgs e = new GetInboxUnreadCountEventArgs();

                    this.GetInboxUnreadCount(this, e);
                    nUnreadCount = e.UnreadCount;
                }

                string strText = "�ҵ���Ϣ";

                if (nUnreadCount != 0)
                    strText += "(" + nUnreadCount.ToString() + ")";
                CreateLink(
                    this.CurrentColumn == HeaderColumn.MyMessage ? true : false,
                    "mymessage",
                    strText,
                    "./mymessage.aspx");
            }

            literal = new LiteralControl();
            literal.Text = "</td>";
            this.Controls.Add(literal);

            // ˢ��


            strClass = "content";

            literal = new LiteralControl();
            literal.Text = "<td class='" + strClass + "' nowrap>";
            this.Controls.Add(literal);

            /*

            linkbutton = new LinkButton();
            linkbutton.ID = "refresh";
            linkbutton.Text = "ˢ��";
            linkbutton.Click += new EventHandler(refreshButton_Click);
            this.Controls.Add(linkbutton);
             */

            ImageButton refresh = CreateImageButton(
                false,
                "refresh",
    "ˢ��");
            refresh.Click +=new ImageClickEventHandler(refresh_Click);

            literal = new LiteralControl();
            literal.Text = "</td>";
            this.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = "</tr></table>";
            this.Controls.Add(literal);
        }

        void refresh_Click(object sender, ImageClickEventArgs e)
        {
            // ʲôҲ������
            
        }

        HyperLink CreateLink(
            bool bActive,
            string id, 
            string strText,
            string strUrl)
        {
            HyperLink hyperlink = null;

            hyperlink = new HyperLink();
            hyperlink.ID = id;
            hyperlink.CssClass = id;
            hyperlink.Text = strText;
            hyperlink.NavigateUrl = strUrl;

            this.Controls.Add(hyperlink);

            if (this.ImageStyle == false)
            {
            }
            else
            {
                string strImageUrl = "./arttext.aspx?text="
                + HttpUtility.UrlEncode(strText)
                + "&face="
                + HttpUtility.UrlEncode("����")
                + "&size="
                + Convert.ToString(12.0F)
                + "&effect=none&fontcolor=0000ff";

                if (bActive == true)
                    strImageUrl += "&style=Underline";


                hyperlink.ImageUrl = strImageUrl;

            }

            return hyperlink;
        }

        ImageButton CreateImageButton(
            bool bActive,
            string id,
    string strAlternateText)
        {
            ImageButton imagebutton = null;

            imagebutton = new ImageButton();
            imagebutton.ID = id;
            imagebutton.CssClass = id;
            imagebutton.AlternateText = strAlternateText;

            this.Controls.Add(imagebutton);

            if (this.ImageStyle == false)
            {
            }
            else
            {
                string strImageUrl = "./arttext.aspx?text="
                + HttpUtility.UrlEncode(strAlternateText)
                + "&face="
                + HttpUtility.UrlEncode("����")
                + "&size="
                + Convert.ToString(12.0F)   // 14
                + "&effect=none&fontcolor=0000ff";

                if (bActive == true)
                    strImageUrl += "&style=Underline";

                imagebutton.ImageUrl = strImageUrl;

            }

            return imagebutton;
        }


        protected override void Render(HtmlTextWriter output)
        {
            // CirculationApplication app = (CirculationApplication)this.Page.Application["app"];
            SessionInfo sessioninfo = (SessionInfo)this.Page.Session["sessioninfo"];

            HyperLink login = (HyperLink)this.FindControl("login");
            HyperLink logout = (HyperLink)this.FindControl("logout");
            HyperLink mymessage = (HyperLink)this.FindControl("mymessage");

            if (sessioninfo != null
                && sessioninfo.UserID != "")
            {
                login.Visible = false;
                logout.Visible = true;
            }
            else
            {
                login.Visible = true;
                logout.Visible = false;
            }

            if (this.ImageStyle == true)
                goto END1;

            HyperLink mylibrary = (HyperLink)this.FindControl("mylibrary");


            if (sessioninfo != null
                && sessioninfo.Account != null
                && sessioninfo.Account.Name != "")
            {
                mylibrary.Text = sessioninfo.Account.Name + "��ͼ���";
                mymessage.Text = sessioninfo.Account.Name + "����Ϣ";
            }
            else
            {
                mylibrary.Text = "�ҵ�ͼ���";
                mymessage.Text = "�ҵ���Ϣ";
            }

            if (sessioninfo != null
                && sessioninfo.Account != null
                && sessioninfo.Account.Type != "reader")
            {
                mylibrary.Enabled = false;
            }


        END1:
           
            output.Write(GetLeftString());

            base.Render(output);

            output.Write(GetRightString());

        }

        void logoutButton_Click(object sender, EventArgs e)
        {
            SessionInfo sessioninfo = (SessionInfo)this.Page.Session["sessioninfo"];

            if (sessioninfo != null && sessioninfo.Account != null)
            {
                sessioninfo.Account = null;
            }

            this.Page.Session.Abandon();
        }

        void refreshButton_Click(object sender, EventArgs e)
        {
        }

        void loginButton_Click(object sender, EventArgs e)
        {
            LibraryApplication app = null;
            SessionInfo sessioninfo = null;

            if (WebUtil.PrepareEnvironment(this.Page,
                ref app,
                ref sessioninfo) == false)
                return;

            sessioninfo.LoginCallStack.Push(this.Page.Request.RawUrl);
            this.Page.Response.Redirect("login.aspx", true);
        }
    }


    public enum HeaderColumn
    {
        None = 0,
        Login = 1,
        Logout = 2,
        MyLibrary = 3,
        Search = 4,
        MyMessage = 5,

    }

    // ����ռ�����δ���ż������¼�
    public delegate void GetInboxUnreadCountEventHandler(object sender,
    GetInboxUnreadCountEventArgs e);

    public class GetInboxUnreadCountEventArgs : EventArgs
    {
        public bool First = false;
        public int UnreadCount = 0;
    }

}

#endif