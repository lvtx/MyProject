using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using System.Threading;
using System.Resources;
using System.Globalization;

using DigitalPlatform.IO;
using DigitalPlatform.rms.Client;
using DigitalPlatform.Xml;

namespace DigitalPlatform.Message
{
    /// <summary>
    /// ���һ���������ʼ���Web�ؼ�
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MessageListControl runat=server></{0}:MessageListControl>")]
    public class MessageListControl : WebControl, INamingContainer
    {
        public override void Dispose()
        {
            if (this.Channels != null)
                this.Channels.Dispose();

            base.Dispose();
        }

        ResourceManager m_rm = null;

        ResourceManager GetRm()
        {
            if (this.m_rm != null)
                return this.m_rm;

            this.m_rm = new ResourceManager("DigitalPlatform.Message.res.MessageListControl.cs",
                typeof(MessageListControl).Module.Assembly);

            return this.m_rm;
        }

        public string GetString(string strID)
        {
            CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name);

            // TODO: ����׳��쳣����Ҫ����ȡzh-cn���ַ��������߷���һ��������ַ���
            try
            {

                string s = GetRm().GetString(strID, ci);
                if (String.IsNullOrEmpty(s) == true)
                    return strID;
                return s;
            }
            catch (Exception /*ex*/)
            {
                return strID + " �� " + Thread.CurrentThread.CurrentUICulture.Name + " ��û���ҵ���Ӧ����Դ��";
            }
        }


        public MessageCenter MessageCenter = null;

        public RmsChannelCollection Channels = null;

//        public string BoxName = "";
        public string UserID = "";

        public string Lang = "";

        #region ����

        // ��ʾ��ʼλ��
        public int StartIndex
        {
            get
            {
                object o = this.Page.Session[this.ID + "MessageList_StartIndex"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageList_StartIndex"] = (object)value;
            }
        }

        // ��ҳ�����ʾ����
        public int PageMaxLines
        {
            get
            {

                object o = this.Page.Session[this.ID + "MessageList_PageMaxLines"];
                return (o == null) ? 10 : (int)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageList_PageMaxLines"] = (object)value;
            }
        }


        // ������м�¼��
        public int ResultCount
        {
            get
            {
                object o = this.Page.Session[this.ID + "MessageList_ResultCount"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageList_ResultCount"] = (object)value;
            }
        }

        // �����������
        public int LineCount
        {
            get
            {

                object o = this.Page.Session[this.ID + "MessageList_LineCount"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageList_LineCount"] = (object)value;
            }
        }

        // �������еĽ������
        public string ResultSetName
        {
            get
            {
                object o = this.Page.Session[this.ID + "MessageList_ResultSetName"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageList_ResultSetName"] = (object)value;
            }
        }

        // ��ǰ����(����)��
        public string CurrentBoxType
        {
            get
            {
                object o = this.Page.Session[this.ID + "MessageList_CurrentBox"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageList_CurrentBox"] = (object)value;
            }
        }

        /*
        // ��ǰ����δ����Ϣ��
        public int CurrentBoxUntouchedCount
        {
            get
            {

                object o = this.Page.Session[this.ID + "MessageList_CurrentBoxUntouched"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageList_CurrentBoxUntouched"] = (object)value;
            }
        }

        // ����δ�����б�
        public Hashtable UntouchedCountList
        {
            get
            {
                object o = this.Page.Session[this.ID + "MessageList_UntouchedList"];
                return (o == null) ? new Hashtable() : (Hashtable)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageList_UntouchedList"] = (object)value;
            }
        }
        */
          
        // �����ҳ������
        public int PageCount
        {
            get
            {
                int nDelta = this.ResultCount % this.PageMaxLines;
                if (nDelta > 0)
                    return (this.ResultCount / this.PageMaxLines) + 1;
                return (this.ResultCount / this.PageMaxLines);
            }
        }

        // ID�б�
        public List<string> ItemIDs
        {
            get
            {
                object o = this.Page.Session[this.ID + "MessageControl_ItemIDs"];
                return (o == null) ? new List<string>() : (List<string>)o;
            }
            set
            {
                this.Page.Session[this.ID + "MessageControl_ItemIDs"] = (object)value;
            }
        }

        #endregion

        #region �����ӿؼ�

        protected override void CreateChildControls()
        {
            // �ܱ��
            this.Controls.Add(new LiteralControl(
    "<table class='messagelist_total'>" //  width='100%' cellspacing='1' cellpadding='4'
    ));
            this.Controls.Add(new LiteralControl(
                "<tr class='messagelist_total'><td class='left' valign='top'>"  // width='1%' 
            ));

            // ��������б�
            PlaceHolder boxlist = new PlaceHolder();
            boxlist.ID = "boxlist";
            this.Controls.Add(boxlist);

            CreateBoxListControls(boxlist);

            this.Controls.Add(new LiteralControl(
                "</td><td class='middle' valign='top'></td><td class='right' valign='top'>"  // width='99%'
            ));


            this.Controls.Add(new LiteralControl("<div class='" + "messagelist_wrapper" + "'>"
                + "<table class='roundbar' cellpadding='0' cellspacing='0'>"
                + "<tr class='titlebar'>"
                + "<td class='left'></td>"
                + "<td class='middle'>"));

            // ��������
            LiteralControl messagelist_titletext = new LiteralControl();
            messagelist_titletext.ID = "messagelist_titletext";
            this.Controls.Add(messagelist_titletext);

            
            this.Controls.Add(new LiteralControl("</td>"
                + "<td class='right'></td>"
                + "</tr>"
                + "</table>"));

            this.Controls.Add(new LiteralControl(
                "<table class='messagelist'>"   //  width='100%' cellspacing='1' cellpadding='4'
                ));

            // ��Ϣ��
            this.Controls.Add(new LiteralControl(
                "<tr class='info'><td colspan='6'>"
            ));

            // ��Ϣ����
            LiteralControl resultinfo = new LiteralControl();
            resultinfo.ID = "info";
            this.Controls.Add(resultinfo);

            this.Controls.Add(new LiteralControl(
                "</td></tr>"
            ));
            
            // ������
            this.Controls.Add(new LiteralControl(
                "<tr class='columntitle'>"
                // + "<td width='1%' nowrap>���</td><td class='sender' width='1%' nowrap>������</td><td class='recipient' width='10%' nowrap>�ռ���</td><td class='subject' width='50%' nowrap>����</td><td class='date' width='13%' nowrap>����</td><td class='size' width='7%' nowrap>�ߴ�</td></tr>"
                + "<td class='index'>"
                + this.GetString("���")
                + "</td><td class='sender'>"
                + this.GetString("������")
                + "</td><td class='recipient'>"
                + this.GetString("�ռ���")
                + "</td><td class='subject'>"
                + this.GetString("����")
                + "</td><td class='date'>"
                + this.GetString("����")
                + "</td><td class='size'>"
                + this.GetString("�ߴ�")
                + "</td></tr>"
            ));


            // ���ݴ���
            PlaceHolder content = new PlaceHolder();
            content.ID = "content";
            this.Controls.Add(content);


            // ������
            for (int i = 0; i < this.LineCount; i++)
            {
                PlaceHolder line = NewContentLine(content, i, null);
            }

            // �����
            PlaceHolder insertpoint = new PlaceHolder();
            insertpoint.ID = "insertpoint";
            content.Controls.Add(insertpoint);

            // ������
            CreateCmdLine();

            // ������Ϣ��
            CreateDebugLine(this);

            this.Controls.Add(new LiteralControl(
               "</table>" + this.GetPostfixString()
               ));

            // �ܱ�����
            this.Controls.Add(new LiteralControl(
                "</td></tr></table>"
            ));
        }

        void CreateDebugLine(Control parent)
        {

            parent.Controls.Add(new LiteralControl(
                "<tr class='debugline'><td colspan='6'>"
            ));


            // ������Ϣ
            LiteralControl text = new LiteralControl();
            text.ID = "debuginfo";
            parent.Controls.Add(text);

            parent.Controls.Add(new LiteralControl(
                "</td></tr>"
            ));
        }
        void SetDebugInfo(string strText)
        {
            PlaceHolder line = (PlaceHolder)FindControl("debugline");
            line.Visible = true;

            LiteralControl text = (LiteralControl)line.FindControl("debugtext");
            text.Text = strText;
        }

        void SetDebugInfo(string strSpanClass,
            string strText)
        {
            PlaceHolder line = (PlaceHolder)FindControl("debugline");
            line.Visible = true;

            LiteralControl text = (LiteralControl)line.FindControl("debugtext");
            if (strSpanClass == "errorinfo")
                text.Text = "<div class='errorinfo-frame'><div class='" + strSpanClass + "'>" + strText + "</div></div>";
            else
                text.Text = "<div class='" + strSpanClass + "'>" + strText + "</div>";
        }
        
#if NOOOOOOOOOOOOOOOOO
        void CreateBoxListControls(PlaceHolder boxlist)
        {

            bool bDetectFullCount = true;   // �Ƿ��״ξ�̽�����������δ������

            LiteralControl literal = new LiteralControl();
            literal.Text = "<table class='boxes' width='100%' cellspacing='1' cellpadding='4'>";
            boxlist.Controls.Add(literal);

            LinkButton linkbutton = null;

            Hashtable untouchedcountlist = this.UntouchedCountList;

            for (int i = 0; i < this.MessageCenter.Boxes.Count; i++)
            {
                Box box = this.MessageCenter.Boxes[i];

                string strClass = "box";

                if (box.Name == this.CurrentBox)
                    strClass = "box_active";

                literal = new LiteralControl();
                literal.Text = "<tr class='"+strClass+"'><td class='"+strClass+"' nowrap>";
                boxlist.Controls.Add(literal);

                int nCount = 0;
                if (box.Name == this.CurrentBox)
                {
                    nCount = this.CurrentBoxUntouchedCount;
                    untouchedcountlist[box.Name] = (object)nCount;
                }
                else
                {
                    object o = untouchedcountlist[box.Name];
                    if (o != null)  // ԭ�ȴ洢��
                    {
                        nCount = (int)o;
                    }
                    else
                    {
                        if (bDetectFullCount == true
                            && this.Channels != null)
                        {
                            string strError = "";
                            nCount = this.MessageCenter.GetUntouchedMessageCount(
                                this.Channels,
                                this.UserID,
                                box.Name,
                                out strError);
                            if (nCount != -1)
                            {
                                untouchedcountlist[box.Name] = (object)nCount;
                            }
                        }
                    }
                }

                linkbutton = new LinkButton();
                linkbutton.ID = box.Name;
                if (nCount != 0)
                    linkbutton.Text = box.Name + "(" + Convert.ToString(nCount) + ")";
                else
                    linkbutton.Text = box.Name;

                linkbutton.Click += new EventHandler(linkbutton_Click);
                boxlist.Controls.Add(linkbutton);

                literal = new LiteralControl();
                literal.Text = "</td></tr>";
                boxlist.Controls.Add(literal);
            }

            this.UntouchedCountList = untouchedcountlist;   // ��������


            literal = new LiteralControl();
            literal.Text = "<tr class='cmd'><td class='newmessage'>";
            boxlist.Controls.Add(literal);

            Button newmessagebutton = new Button();
            newmessagebutton.ID = "newmessage";
            newmessagebutton.Text = "׫д";
            newmessagebutton.Click += new EventHandler(newmessage_Click);
            boxlist.Controls.Add(newmessagebutton);

            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            boxlist.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "</table>";
            boxlist.Controls.Add(literal);
        }

#endif

        void CreateBoxListControls(PlaceHolder boxlist)
        {

            bool bDetectFullCount = true;   // �Ƿ��״ξ�̽�����������δ������

            LiteralControl literal = new LiteralControl();
            literal.Text = this.GetPrefixString(
                this.GetString("����"), 
                "boxes_wrapper");
            literal.Text += "<table class='boxes'>"; //  width='100%' cellspacing='1' cellpadding='4'
            boxlist.Controls.Add(literal);

            LinkButton linkbutton = null;

            int nInboxUntouchedCount = 0;   // �ռ����е�δ���ż���Ŀ

            for (int i = 0; i < this.MessageCenter.Boxes.Count; i++)
            {
                Box box = this.MessageCenter.Boxes[i];

                string strClass = "box";

                if (/*box.Name == this.CurrentBoxType*/
                    box.Type == this.CurrentBoxType)
                    strClass = "box_active";

                literal = new LiteralControl();
                literal.Text = "<tr class='" + strClass + "'><td class='" + strClass + "' nowrap>";
                boxlist.Controls.Add(literal);

                int nCount = 0;

                if (bDetectFullCount == true
                    && this.Channels != null)
                {
                    string strError = "";
                    nCount = this.MessageCenter.GetUntouchedMessageCount(
                        this.Channels,
                        this.UserID,
                        box.Name,
                        out strError);
                    if (nCount != -1)
                    {
                        // untouchedcountlist[box.Name] = (object)nCount;
                        
                    }

                    if (/*box.Name == MessageCenter.INBOX*/
                        box.Type == MessageCenter.INBOX)
                        nInboxUntouchedCount = nCount;
                }
 

                linkbutton = new LinkButton();
                linkbutton.ID = box.Name;
                string strCaption = this.MessageCenter.GetString(box.Type);   // ���������ڵ�ǰ���Ե����� 2009/7/14 changed
                if (nCount != 0)
                    linkbutton.Text = strCaption + "(" + Convert.ToString(nCount) + ")";
                else
                    linkbutton.Text = strCaption;

                linkbutton.Click += new EventHandler(linkbutton_Click);
                boxlist.Controls.Add(linkbutton);

                literal = new LiteralControl();
                literal.Text = "</td></tr>";
                boxlist.Controls.Add(literal);
            }

            // this.UntouchedCountList = untouchedcountlist;   // ��������


            literal = new LiteralControl();
            literal.Text = "<tr class='cmd'><td class='newmessage'>";
            boxlist.Controls.Add(literal);

            Button newmessagebutton = new Button();
            newmessagebutton.ID = "newmessage";
            newmessagebutton.Text = this.GetString("׫д��Ϣ");
            newmessagebutton.CssClass = "newmessage";
            newmessagebutton.Click += new EventHandler(newmessage_Click);
            boxlist.Controls.Add(newmessagebutton);

            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            boxlist.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "</table>" + this.GetPostfixString();
            boxlist.Controls.Add(literal);
        }


        void linkbutton_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            string strBoxName = button.ID;

            // TODO: ��Ҫ����ť�ϵ��������滻Ϊboxtypeֵ
            string strBoxType = this.MessageCenter.GetBoxType(strBoxName);
            if (String.IsNullOrEmpty(strBoxType) == false)
            {
                this.Page.Response.Redirect("./mymessage.aspx?box=" + HttpUtility.UrlEncode(strBoxType));
                this.Page.Response.End();
            }
            else
            {
                this.Page.Response.Write("������ '" + strBoxName + "' �޷�ת��Ϊ���������ַ���");
                this.Page.Response.End();
            }

        }

        void newmessage_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect("./message.aspx?box=" + HttpUtility.UrlEncode("�ݸ�"));
            this.Page.Response.End();
        }

        // �´���������
        PlaceHolder NewContentLine(Control content,
            int nLineNo,
            Control insertpos)
        {
            PlaceHolder line = new PlaceHolder();
            line.ID = "line" + Convert.ToString(nLineNo);

            if (insertpos != null)
            {
                int index = content.Controls.IndexOf(insertpos);
                content.Controls.AddAt(index, line);
            }
            else
            {
                content.Controls.Add(line);
            }

            LiteralControl literal = new LiteralControl();
            literal.Text = "<tr class='";
            line.Controls.Add(literal);

            literal = new LiteralControl();
            literal.ID = "line" + Convert.ToString(nLineNo) + "_classname";
            literal.Text = "content";
            line.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "'><td class='no'>"; // width='1%'
            line.Controls.Add(literal);

            // checkbox
            CheckBox checkbox = new CheckBox();
            checkbox.ID = "line" + Convert.ToString(nLineNo) + "_checkbox";
            line.Controls.Add(checkbox);

            // ���
            literal = new LiteralControl();
            literal.ID = "line" + Convert.ToString(nLineNo) + "_no";
            line.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = "</td><td class='sender'>"; //  width='10%'
            line.Controls.Add(literal);

            // ������
            literal = new LiteralControl();
            literal.ID = "line" + Convert.ToString(nLineNo) + "_sender";
            line.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "</td><td class='recipient'>"; // width='10%'
            line.Controls.Add(literal);

            // �ռ���
            literal = new LiteralControl();
            literal.ID = "line" + Convert.ToString(nLineNo) + "_recipient";
            line.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "</td><td class='subject'>"; // width='50%'
            line.Controls.Add(literal);

            // ����
            literal = new LiteralControl();
            literal.ID = "line" + Convert.ToString(nLineNo) + "_subject";
            line.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "</td><td class='date'>"; // width='13%'
            line.Controls.Add(literal);

            // ����
            literal = new LiteralControl();
            literal.ID = "line" + Convert.ToString(nLineNo) + "_date";
            line.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "</td><td class='size'>"; // width='7%'
            line.Controls.Add(literal);

            // �ߴ�
            literal = new LiteralControl();
            literal.ID = "line" + Convert.ToString(nLineNo) + "_size";
            line.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            line.Controls.Add(literal);


            return line;
        }

        void CreateCmdLine()
        {

            this.Controls.Add(new LiteralControl(
                "<tr class='cmdline'><td colspan='6'>"
            ));

            this.Controls.Add(new LiteralControl(
                "<table border='0' width='100%'><tr><td>"
            ));

            // ɾ��ѡ����ť
            Button deletebutton = new Button();
            deletebutton.ID = "delete";
            deletebutton.Text = this.GetString("ɾ��ѡ��������");
            deletebutton.CssClass = "delete";
            deletebutton.Click += new EventHandler(deletebutton_Click);
            this.Controls.Add(deletebutton);

            this.Controls.Add(new LiteralControl(
                "</td><td>"
            ));

            // ȫ��ɾ����ť 2007/7/15
            Button deleteallbutton = new Button();
            deleteallbutton.ID = "deleteall";
            deleteallbutton.Text = this.GetString("ȫ��ɾ��");
            deleteallbutton.CssClass = "deleteall";
            deleteallbutton.Click +=new EventHandler(deleteallbutton_Click);
            this.Controls.Add(deleteallbutton);

            this.Controls.Add(new LiteralControl(
                "</td><td align='right'> "
            ));

            PlaceHolder pageswitcher = new PlaceHolder();
            pageswitcher.ID = "pageswitcher";
            this.Controls.Add(pageswitcher);


            LinkButton firstpage = new LinkButton();
            firstpage.ID = "first";
            firstpage.Text = this.GetString("��ҳ");
            firstpage.CssClass = "firstpage";
            firstpage.Click += new EventHandler(firstpage_Click);
            pageswitcher.Controls.Add(firstpage);

            pageswitcher.Controls.Add(new LiteralControl(
                " "
            ));

            LinkButton prevpage = new LinkButton();
            prevpage.ID = "prev";
            prevpage.Text = this.GetString("ǰҳ");
            prevpage.CssClass = "prevpage";
            prevpage.Click += new EventHandler(prevpage_Click);
            pageswitcher.Controls.Add(prevpage);

            pageswitcher.Controls.Add(new LiteralControl(
                " "
            ));

            LiteralControl literal = new LiteralControl();
            literal.ID = "currentpageno";
            literal.Text = "";
            pageswitcher.Controls.Add(literal);

            pageswitcher.Controls.Add(new LiteralControl(
                " "
            ));


            LinkButton nextpage = new LinkButton();
            nextpage.ID = "next";
            nextpage.Text = this.GetString("��ҳ");
            nextpage.CssClass = "nextpage";
            nextpage.Click += new EventHandler(nextpage_Click);
            pageswitcher.Controls.Add(nextpage);

            pageswitcher.Controls.Add(new LiteralControl(
                " "
            ));

            LinkButton lastpage = new LinkButton();
            lastpage.ID = "last";
            lastpage.Text = this.GetString("ĩҳ");
            lastpage.CssClass = "lastpage";
            lastpage.Click += new EventHandler(lastpage_Click);
            pageswitcher.Controls.Add(lastpage);

            literal = new LiteralControl();
            literal.Text = "  ";
            pageswitcher.Controls.Add(literal);

            Button gotobutton = new Button();
            gotobutton.ID = "gotobutton";
            gotobutton.Text = this.GetString("����");
            gotobutton.CssClass = "goto";
            gotobutton.Click += new EventHandler(gotobutton_Click);
            pageswitcher.Controls.Add(gotobutton);

            literal = new LiteralControl();
            literal.Text = " " + this.GetString("��") + " ";    // " �� "
            pageswitcher.Controls.Add(literal);


            TextBox textbox = new TextBox();
            textbox.ID = "gotopageno";
            textbox.Width = new Unit("40");
            textbox.CssClass = "gotopageno";
            pageswitcher.Controls.Add(textbox);

            /*
            literal = new LiteralControl();
            literal.Text = " ҳ";
            pageswitcher.Controls.Add(literal);
             * */

            literal = new LiteralControl();
            literal.ID = "maxpagecount";
            literal.Text = " " + string.Format(this.GetString("maxpagecount"), this.PageCount.ToString());    // (�� {0} ҳ)
            pageswitcher.Controls.Add(literal);

            this.Controls.Add(new LiteralControl(
                "</td></tr></table>"
            ));

            this.Controls.Add(new LiteralControl(
                "</td></tr>"
            ));
        }

        // ɾ��ȫ������Ϣ
        void deleteallbutton_Click(object sender, EventArgs e)
        {
            string strError = "";

            int nTotalCount = 0;
            int nStart = 0;
            int nPerCount = 10;
            int nRet = 0;

            bool bMoveToRecycleBin = true;

            if (MessageCenter.IsRecycleBin(this.CurrentBoxType) == true)
                bMoveToRecycleBin = false;
            else
                bMoveToRecycleBin = true;

            for (; ; )
            {

                List<MessageData> messages = null;
                nRet = this.MessageCenter.GetMessage(
                    this.Channels,
                    this.ResultSetName,
                    "", // false,
                    this.UserID,
                    this.CurrentBoxType,
                    MessageLevel.Summary,
                    nStart,  // this.StartIndex,
                    nPerCount, // this.PageMaxLines,
                    out nTotalCount,
                    out messages,
                    out strError);
                if (nRet == -1)
                {
                    // text-level: �ڲ�����
                    this.SetDebugInfo("errorinfo", "ɾ��ȫ����Ϣʱ��������: " + strError);
                    return;
                }


                List<string> ids = new List<string>();

                for (int i = 0; i < messages.Count; i++)
                {
                    ids.Add(messages[i].strRecordID);
                }

                nRet = this.MessageCenter.DeleteMessage(bMoveToRecycleBin,
                    this.Channels,
                    ids,
                    null,
                    out strError);
                if (nRet == -1)
                {
                    this.SetDebugInfo("errorinfo", strError);
                    return;
                }

                nStart += messages.Count;
                if (nStart >= nTotalCount)
                    break;
            }

            if (bMoveToRecycleBin == true)
            {
                // text-level: �û���ʾ
                this.SetDebugInfo(
                    string.Format(this.GetString("�ѽ�s����Ϣ�ƶ����ϼ���"),    // "�ѽ� {0} ����Ϣ�ƶ����ϼ��䡣"
                    nStart.ToString()));
                // "�ѽ� " + nStart.ToString() + " ����Ϣ�ƶ����ϼ��䡣"

            }
            else
            {
                // text-level: �û���ʾ
                this.SetDebugInfo(
                    string.Format(this.GetString("�ѽ�s����Ϣ����ɾ��"),    // "�ѽ� {0} ����Ϣ����ɾ����"
                    nStart.ToString()));

                // "�ѽ� " + nStart.ToString() + " ����Ϣ����ɾ����"
            }

            this.RefreshList(); // ˢ�µ�ǰ�������ʾ


        }

        // ɾ��ѡ�����Ϣ
        void deletebutton_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();

            for (int i = 0; i < this.LineCount; i++)
            {
                CheckBox checkbox = (CheckBox)this.FindControl("line" + Convert.ToString(i) + "_checkbox");
                if (checkbox.Checked == true)
                {
                    if (this.ItemIDs.Count <= i)
                    {
                        // text-level: �ڲ�����
                        this.SetDebugInfo("errorinfo", "ItemIDsʧЧ...");
                        return;
                    }
                    ids.Add(this.ItemIDs[i]);
                    checkbox.Checked = false;
                }
            }

            if (ids.Count == 0)
            {
                // text-level: �û���ʾ
                this.SetDebugInfo(this.GetString("��δѡ���κ���Ϣ"));
                return ;
            }

            bool bMoveToRecycleBin = true;

            if (MessageCenter.IsRecycleBin(this.CurrentBoxType) == true)
                bMoveToRecycleBin = false;
            else
                bMoveToRecycleBin = true;

            string strError = "";
            int nRet = this.MessageCenter.DeleteMessage(bMoveToRecycleBin,
                this.Channels,
                ids,
                null,
                out strError);
            if (nRet == -1)
                this.SetDebugInfo("errorinfo", strError);
            else
            {
                if (bMoveToRecycleBin == true)
                {
                    // text-level: �û���ʾ
                    this.SetDebugInfo(
                                            string.Format(this.GetString("�ѽ�s����Ϣ�ƶ����ϼ���"),    // "�ѽ� {0} ����Ϣ�ƶ����ϼ��䡣"
                                            ids.Count.ToString()));

                    // "�ѽ� " + ids.Count + " ����Ϣ�ƶ����ϼ��䡣");
                }
                else
                {
                    // text-level: �û���ʾ
                    this.SetDebugInfo(
                                            string.Format(this.GetString("�ѽ�s����Ϣ����ɾ��"),    // "�ѽ� {0} ����Ϣ����ɾ����"
                                            ids.Count.ToString()));

                    // "�ѽ� " + ids.Count + " ����Ϣ����ɾ����");
                }

                this.RefreshList(); // ˢ�µ�ǰ�������ʾ
            }
        }

        // ���¼��������
        public void RefreshList()
        {
            string strError = "";
            int nTotalCount = 0;
            List<MessageData> messages = null;
            if (String.IsNullOrEmpty(this.UserID) == true)
            {
                // text-level: �ڲ�����
                throw new Exception("UserIDΪ��");
            }

            int nRet = this.MessageCenter.GetMessage(
                this.Channels,
                this.ResultSetName,
                "search",   // true,
                this.UserID,
                this.CurrentBoxType,
                MessageLevel.Summary,
                0,
                0,
                out nTotalCount,
                out messages,
                out strError);
            if (nRet == -1)
            {
                // text-level: �ڲ�����
                this.SetDebugInfo("errorinfo", "ˢ��ʱ����ʧ��: " + strError);
            }
            else
            {
                this.ResultCount = nTotalCount;
            }
        }

        // ����ָ����ҳ��
        void gotobutton_Click(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)this.FindControl("gotopageno");

            int nPageNo = 0;

            try
            {
                nPageNo = Convert.ToInt32(textbox.Text);
            }
            catch
            {
                return;
            }

            if (nPageNo < 1)
                nPageNo = 1;
            this.StartIndex = this.PageMaxLines * (nPageNo - 1);
            if (this.StartIndex >= this.ResultCount)
            {
                lastpage_Click(sender, e);
            }
        }

        void lastpage_Click(object sender, EventArgs e)
        {
            int delta = this.ResultCount % this.PageMaxLines;
            if (delta > 0)
                this.StartIndex = (this.ResultCount / this.PageMaxLines) * this.PageMaxLines;
            else
                this.StartIndex = Math.Max(0, (this.ResultCount / this.PageMaxLines) * this.PageMaxLines - 1);

        }

        void nextpage_Click(object sender, EventArgs e)
        {
            this.StartIndex += this.PageMaxLines;
            if (this.StartIndex >= this.ResultCount)
            {
                lastpage_Click(sender, e);
            }
        }

        void prevpage_Click(object sender, EventArgs e)
        {
            this.StartIndex -= this.PageMaxLines;
            if (this.StartIndex < 0)
                this.StartIndex = 0;
        }

        void firstpage_Click(object sender, EventArgs e)
        {
            this.StartIndex = 0;
        }

        #endregion


        protected override void Render(HtmlTextWriter writer)
        {
            int nRet = 0;

            int nPageNo = this.StartIndex / this.PageMaxLines;

            if (nPageNo >= this.PageCount)  // ����������һҳ
                lastpage_Click(null, null);

            SetResultInfo();

            string strError = "";

            List<string> tempids = new List<string>();

            if (this.ResultCount != 0)
            {

                int nTotalCount = 0;
                List<MessageData> messages = null;
                nRet = this.MessageCenter.GetMessage(
                    this.Channels,
                    this.ResultSetName,
                    "", // false,
                    this.UserID,
                    this.CurrentBoxType,
                    MessageLevel.Summary,
                    this.StartIndex,
                    this.PageMaxLines,
                    out nTotalCount,
                    out messages,
                    out strError);
                if (nRet == -1)
                {
                    throw new Exception(strError);
                }
                /*
                Channel channel = this.Channels.GetChannel(this.MessageCenter.ServerUrl);
                if (channel == null)
                {
                    throw new Exception("get channel error");
                }

                ArrayList aLine = null;
                long lRet = channel.DoGetSearchFullResult(
                    this.ResultSetName,
                    this.StartIndex,
                    this.PageMaxLines,
                    this.Lang,
                    null,
                    out aLine,
                    out strError);
                if (lRet == -1)
                {
                    // ��Ȼ����-1,����aLine����Ȼ��������
                    if (aLine == null)
                        throw new Exception(strError);
                }
                 */


                // ��ʾ��ҳ�е������
                for (int i = 0; i < this.PageMaxLines; i++)
                {
                    MessageData data = null;
                    if (i < messages.Count)
                        data = messages[i];

                    PlaceHolder line = (PlaceHolder)this.FindControl("line" + Convert.ToString(i));
                    if (line == null)
                    {
                        PlaceHolder insertpoint = (PlaceHolder)this.FindControl("insertpoint");
                        PlaceHolder content = (PlaceHolder)this.FindControl("content");

                        line = this.NewContentLine(content, i, insertpoint);
                    }

                    LiteralControl no = (LiteralControl)this.FindControl("line" + Convert.ToString(i) + "_no");
                    CheckBox checkbox = (CheckBox)this.FindControl("line" + Convert.ToString(i) + "_checkbox");
                    LiteralControl sender = (LiteralControl)this.FindControl("line" + Convert.ToString(i) + "_sender");
                    LiteralControl recipient = (LiteralControl)this.FindControl("line" + Convert.ToString(i) + "_recipient");
                    LiteralControl subject = (LiteralControl)this.FindControl("line" + Convert.ToString(i) + "_subject");
                    LiteralControl date = (LiteralControl)this.FindControl("line" + Convert.ToString(i) + "_date");
                    LiteralControl size = (LiteralControl)this.FindControl("line" + Convert.ToString(i) + "_size");
                    LiteralControl classname = (LiteralControl)this.FindControl("line" + Convert.ToString(i) + "_classname");


                    if (data == null)
                    {
                        checkbox.Visible = false;
                        subject.Text = "&nbsp;";
                        continue;
                    }

                    checkbox.Visible = true;

                    tempids.Add(data.strRecordID);

                    // ���
                    string strNo = "&nbsp;";
                    strNo = Convert.ToString(i + this.StartIndex + 1);

                    no.Text = strNo;


                    string strDetailUrl = "./message.aspx?id=" + data.strRecordID;
                    if (data.strSubject == "")
                        data.strSubject = this.GetString("��");   // "(��)"

                    sender.Text = data.strSender;
                    recipient.Text = data.strRecipient;
                    subject.Text = "<a href='"+strDetailUrl+"'>" + data.strSubject + "</a>"; 
                    date.Text = DateTimeUtil.LocalTime(data.strCreateTime);
                    size.Text = data.strSize;

                    if (data.Touched == true)
                        classname.Text = "content";
                    else
                        classname.Text = "content_new";

                } // end of for

                this.LineCount = Math.Max(this.LineCount, this.PageMaxLines);

            }
            else
            {
                // ��ʾ����
                for (int i = 0; i < this.PageMaxLines; i++)
                {
                    PlaceHolder line = (PlaceHolder)this.FindControl("line" + Convert.ToString(i));
                    if (line == null)
                    {
                        PlaceHolder insertpoint = (PlaceHolder)this.FindControl("insertpoint");
                        PlaceHolder content = (PlaceHolder)this.FindControl("content");

                        line = this.NewContentLine(content, i, insertpoint);
                    }

                    line.Visible = true;

                    CheckBox checkbox = (CheckBox)this.FindControl("line" + Convert.ToString(i) + "_checkbox");
                    checkbox.Visible = false;

                    LiteralControl subject = (LiteralControl)this.FindControl("line" + Convert.ToString(i) + "_subject");
                    subject.Text = "&nbsp;";

                }

            }

            this.ItemIDs = tempids;

            // ����ɾ����ť����
            Button deletebutton = (Button)this.FindControl("delete");

            if (MessageCenter.IsRecycleBin(this.CurrentBoxType) == true)
                deletebutton.Text = this.GetString("����ɾ��ѡ������Ϣ");
            else
                deletebutton.Text = this.GetString("��ѡ������Ϣ�����ϼ���");

            // ����ɾ��ȫ����ť����
            Button deleteallbutton = (Button)this.FindControl("deleteall");

            if (MessageCenter.IsRecycleBin(this.CurrentBoxType) == true)
                deleteallbutton.Text = this.GetString("����ɾ��ȫ����Ϣ");
            else
                deleteallbutton.Text = this.GetString("��ȫ����Ϣ�����ϼ���");


            base.Render(writer);
        }

        // ���ý�����й���������
        public void SetResultInfo()
        {
            LiteralControl messagelist_titletext = (LiteralControl)this.FindControl("messagelist_titletext");
            messagelist_titletext.Text = this.MessageCenter.GetString(this.CurrentBoxType);

            int nPageNo = this.StartIndex / this.PageMaxLines;

            LiteralControl resultinfo = (LiteralControl)this.FindControl("info");
            if (this.ResultCount != 0)
            {
                // text-level: ������ʾ
                resultinfo.Text =
                    string.Format(this.GetString("s�ڹ�����Ϣs��, ��sҳ��ʾ, ��ǰΪ��sҳ"),
                    // "{0} �ڹ�����Ϣ {1} ��, �� {2} ҳ��ʾ, ��ǰΪ�� {3} ҳ��"
                    this.MessageCenter.GetString(this.CurrentBoxType),
                    Convert.ToString(this.ResultCount),
                    Convert.ToString(this.PageCount),
                    Convert.ToString(nPageNo + 1));

                // this.MessageCenter.GetString(this.CurrentBoxType) + " �ڹ�����Ϣ " + Convert.ToString(this.ResultCount) + " ��, �� " + Convert.ToString(this.PageCount) + " ҳ��ʾ, ��ǰΪ�� " + Convert.ToString(nPageNo + 1) + "ҳ��";
            }
            else
            {
                // text-level: ������ʾ
                resultinfo.Text =
                    string.Format(this.GetString("sΪ��"),  // "('{0}' Ϊ��)"
                    this.MessageCenter.GetString(this.CurrentBoxType));
                    /*
                    "('"
                    + this.MessageCenter.GetString(this.CurrentBoxType)
                    + "' Ϊ��)";
                     * */
            }

            LiteralControl maxpagecount = (LiteralControl)this.FindControl("maxpagecount");
            maxpagecount.Text =
                string.Format(this.GetString("maxpagecount"),   // (�� {0} ҳ)
                Convert.ToString(this.PageCount));
                // " (�� " + Convert.ToString(this.PageCount) + " ҳ)";

            LiteralControl currentpageno = (LiteralControl)this.FindControl("currentpageno");
            currentpageno.Text = Convert.ToString(nPageNo + 1);

            PlaceHolder pageswitcher = (PlaceHolder)this.FindControl("pageswitcher");
            if (this.PageCount <= 1)
                pageswitcher.Visible = false;
            else
                pageswitcher.Visible = true;
        }

        // ���һ����Ϣ
        int GetLineInfo(
            RmsChannel channel,
            string strPath,
            out string strSender,
            out string strRecipient,
            out string strSubject,
            out string strDate,
            out string strSize,
            out bool bTouched,
            out string strError)
        {
            strSender = "";
            strRecipient = "";
            strSubject = "";
            strDate = "";
            strSize = "";
            bTouched = false;
            strError = "";


            // ���ּ�¼���ݴ�XML��ʽת��ΪHTML��ʽ
            string strMetaData = "";
            byte[] timestamp = null;
            string strXml = "";
            string strOutputPath = "";
            long lRet = channel.GetRes(strPath,
                out strXml,
                out strMetaData,
                out timestamp,
                out strOutputPath,
                out strError);
            if (lRet == -1)
            {
                // text-level: �ڲ�����
                strError = "�����Ϣ��¼ '" + strPath + "' ʱ����: " + strError;
                return -1;
            }

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                // text-level: �ڲ�����
                strError = "װ��XML��¼����DOMʱ����: " + ex.Message;
                return -1;
            }

            strSender = DomUtil.GetElementText(dom.DocumentElement,
                "sender");
            strRecipient = DomUtil.GetElementText(dom.DocumentElement,
                "recipient");
            strSubject = DomUtil.GetElementText(dom.DocumentElement,
                "subject");
            strDate = DomUtil.GetElementText(dom.DocumentElement,
                "date");
            strDate = DateTimeUtil.LocalTime(strDate);

            strSize = DomUtil.GetElementText(dom.DocumentElement,
                "size");
            string strTouched = DomUtil.GetElementText(dom.DocumentElement,
                "touched");
            if (strTouched == "1")
                bTouched = true;
            else
                bTouched = false;

            return 0;
        }

        // TODO: ���boxtype
        // װ��һ���������Ϣ
        public int LoadBox(
            RmsChannelCollection Channels,
            string strUserID,
            string strBoxType,
            out string strError)
        {
            // ����Ĭ�Ͽؼ��ĵ�ǰ����
            if (String.IsNullOrEmpty(strBoxType) == true)
                strBoxType = this.CurrentBoxType;

            // �����ǿ�, ��Ĭ��INBOX
            if (String.IsNullOrEmpty(strBoxType) == true)
                strBoxType = MessageCenter.INBOX;

            string strResultSetName = "messagelist_" + strBoxType;

            int nTotalCount = 0;
            List<MessageData> messages = null;
            int nRet = this.MessageCenter.GetMessage(
                Channels,
                strResultSetName,
                "search", // true,
                strUserID,
                strBoxType,
                MessageLevel.Summary,
                0,
                0,
                out nTotalCount,
                out messages,
                out strError);
            if (nRet == -1)
            {
                return -1;
            }

            this.ResultSetName = strResultSetName;
            this.ResultCount = nTotalCount;
            this.CurrentBoxType = strBoxType;

            /*
            this.CurrentBoxUntouchedCount = 0;

            // �������δ����Ϣ��
            if (nTotalCount != 0)
            {
                int nUntouched = this.MessageCenter.GetUntouchedMessageCount(
                    Channels,
                    strUserID,
                    strBox,
                    out strError);
                if (nUntouched == -1)
                {
                    return -1;
                }
                this.CurrentBoxUntouchedCount = nUntouched;
            }
             * */


            return 0;
        }


        public string GetPrefixString(string strTitle,
            string strWrapperClass)
        {
            return "<div class='" + strWrapperClass + "'>"
                + "<table class='roundbar' cellpadding='0' cellspacing='0'>"
                + "<tr class='titlebar'>"
                + "<td class='left'></td>"
                + "<td class='middle'>" + strTitle + "</td>"
                + "<td class='right'></td>"
                + "</tr>"
                + "</table>";
        }

        public string GetPostfixString()
        {
            return "</div>";
        }

        // ȡ���������tag
        public override void RenderBeginTag(HtmlTextWriter writer)
        {

        }
        public override void RenderEndTag(HtmlTextWriter writer)
        {

        }
    }
}
