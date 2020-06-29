using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;

using System.Threading;
using System.Resources;
using System.Globalization;

using DigitalPlatform.IO;
using DigitalPlatform.rms.Client;
using DigitalPlatform.Xml;

namespace DigitalPlatform.Message
{
    /// <summary>
    /// ��ʾ���༭һ����Ϣ
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MessageControl runat=server></{0}:MessageControl>")]
    public class MessageControl : WebControl, INamingContainer
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

            this.m_rm = new ResourceManager("DigitalPlatform.Message.res.MessageControl.cs",
                typeof(MessageControl).Module.Assembly);

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
            catch (Exception /* ex */)
            {
                return strID + " �� " + Thread.CurrentThread.CurrentUICulture.Name + " ��û���ҵ���Ӧ����Դ��";
            }
        }

        public MessageCenter MessageCenter = null;

        public MessageData MessageData = null;

        public RmsChannelCollection Channels = null;

        public string UserID = "";

        //
        public string Mode
        {
            get
            {
                object o = ViewState["Mode"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["Mode"] = (object)value;
            }
        }

        // ��Դ��¼id
        public string RecordID
        {
            get
            {
                object o = ViewState["RecordID"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["RecordID"] = (object)value;
            }
        }

        // ��Դ��¼��ʱ���
        public byte [] TimeStamp
        {
            get
            {
                object o = ViewState["TimeStamp"];
                return (o == null) ? (byte[])null : (byte[])o;
            }
            set
            {
                ViewState["TimeStamp"] = (object)value;
            }
        }

        // ��Դ��¼������������
        public string BoxName
        {
            get
            {
                object o = ViewState["BoxName"];
                return (o == null) ? String.Empty : (string)o;
            }
            set
            {
                ViewState["BoxName"] = (object)value;
            }
        }

        // ��Դ��¼id����
        public List<string> RecordIDs
        {
            get
            {
                object o = ViewState["RecordIDs"];
                return (o == null) ? (List<string>)null : (List<string>)o;
            }
            set
            {
                ViewState["RecordIDs"] = (object)value;
            }
        }

        // ���������еĵ�ǰλ��
        public int RecordIDsIndex
        {
            get
            {
                object o = ViewState["RecordIDsIndex"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                ViewState["RecordIDsIndex"] = (object)value;
            }
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

        public string Sender
        {
            get
            {
                this.EnsureChildControls();

                TextBox sender = (TextBox)this.FindControl("sender");
                return sender.Text;
            }
            set
            {
                this.EnsureChildControls();

                TextBox sender = (TextBox)this.FindControl("sender");
                sender.Text = value;
            }
        }

        public string Recipient
        {
            get
            {
                this.EnsureChildControls();

                TextBox sender = (TextBox)this.FindControl("recipient");
                return sender.Text;
            }
            set
            {
                this.EnsureChildControls();

                TextBox sender = (TextBox)this.FindControl("recipient");
                sender.Text = value;
            }
        }

        protected override void CreateChildControls()
        {
            LiteralControl literal = new LiteralControl();
            literal.Text = this.GetPrefixString(
                this.GetString("��Ϣ"),
                "content_wrapper");
            literal.Text += "<table class='message'>"    //  width='100%' cellspacing='1' cellpadding='4'
                +"";
            this.Controls.Add(literal);

            PlaceHolder edit = new PlaceHolder();
            edit.ID = "edit";
            this.Controls.Add(edit);

            // �ۺ���Ϣ
            literal = new LiteralControl();
            literal.Text = "<tr class='info'>"
                + "<td class='info' colspan='2'>";
            edit.Controls.Add(literal);

            literal = new LiteralControl();
            literal.ID = "infotext";
            edit.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            edit.Controls.Add(literal);


            // ������
            literal = new LiteralControl();
            literal.Text = "<tr class='sender'>"
                +"<td class='name' nowrap width='10%'>"
                + this.GetString("������")
                + "</td><td class='content' width='90%'>";
            edit.Controls.Add(literal);

            TextBox sender = new TextBox();
            sender.ID = "sender";
            sender.Width = new Unit("99%");
            edit.Controls.Add(sender);

            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            edit.Controls.Add(literal);

            // �ռ���
            literal = new LiteralControl();
            literal.Text = "<tr class='recipient'>"
                + "<td class='name' nowrap width='10%'>"
                + this.GetString("�ռ���")
                + "</td><td class='content' width='90%' width='90%'>";
            edit.Controls.Add(literal);

            TextBox recipient = new TextBox();
            recipient.ID = "recipient";
            recipient.Width = new Unit("99%");
            edit.Controls.Add(recipient);

            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            edit.Controls.Add(literal);

            // ����
            literal = new LiteralControl();
            literal.Text = "<tr class='subject'>"
                + "<td class='name' nowrap width='10%'>"
                + this.GetString("����")
                + "</td><td class='content' width='90%'>";
            edit.Controls.Add(literal);

            TextBox subject = new TextBox();
            subject.ID = "subject";
            subject.Width = new Unit("99%");
            edit.Controls.Add(subject);

            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            edit.Controls.Add(literal);

            // ����
            literal = new LiteralControl();
            literal.Text = "<tr class='content'>"
                + "<td class='name' nowrap width='10%'>"
                + this.GetString("����")
                + "</td><td class='content' width='90%'>";
            edit.Controls.Add(literal);


            TextBox content = new TextBox();
            content.ID = "content";
            content.Width = new Unit("99%");
            content.Rows = 10;
            content.TextMode = TextBoxMode.MultiLine;
            edit.Controls.Add(content);

            // html����
            PlaceHolder html_content_container = new PlaceHolder();
            html_content_container.ID = "html_content_container";
            edit.Controls.Add(html_content_container);


            literal = new LiteralControl();
            literal.Text = "<div>";
            html_content_container.Controls.Add(literal);


            literal = new LiteralControl();
            literal.ID = "html_content";
            literal.Text = "";
            html_content_container.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "</div>";
            html_content_container.Controls.Add(literal);

            // 


            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            edit.Controls.Add(literal);

            // ʱ��
            literal = new LiteralControl();
            literal.Text = "<tr class='date'>"
                + "<td class='name' nowrap width='10%'>"
                + this.GetString("ʱ��")
                + "</td><td class='content' width='90%'>";
            edit.Controls.Add(literal);

            TextBox date = new TextBox();
            date.ID = "date";
            date.Width = new Unit("99%");
            edit.Controls.Add(date);

            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            edit.Controls.Add(literal);


            CreateCmdLine(edit);

            CreateDebugLine(edit);

            PlaceHolder endinfoline = new PlaceHolder();
            endinfoline.ID = "endinfoline";
            this.Controls.Add(endinfoline);

            CreateEndInfo(endinfoline);


            literal = new LiteralControl();
            literal.Text = "</table>" + this.GetPostfixString();
            this.Controls.Add(literal);


            if (this.RecordIDs != null)
            {
                // װ��һ����¼
            }

            if (this.MessageData != null)
            {
                this.SetMessageData(this.MessageData);
                this.SetState(this.MessageData.strBoxType);
            }
            else
            {
                this.SetState(null);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string strMode;
            if (String.IsNullOrEmpty(this.Mode) == true)
            {
                strMode = this.Mode;
            }
            else
            {
                strMode = this.Mode;
            }

            PlaceHolder endinfoline = (PlaceHolder)this.FindControl("endinfoline");
            PlaceHolder edit = (PlaceHolder)this.FindControl("edit");

            if (strMode == "end")
            {
                endinfoline.Visible = true;
                edit.Visible = false;
            }
            else
            {
                endinfoline.Visible = false;
                edit.Visible = true;
            }

            LiteralControl infotext = (LiteralControl)this.FindControl("infotext");
            if (String.IsNullOrEmpty(this.BoxName) == true)
            {
                // text-level: ������ʾ
                infotext.Text = this.GetString("����Ϣ");
            }
            else
            {
                // text-level: ������ʾ
                infotext.Text =
                    string.Format(this.GetString("����s"),  // "���� {0}"
                    this.MessageCenter.GetString(this.BoxName)  // ������������ת��Ϊ��ǰ���Ե��������ַ���
                    );
                    // "���� " + this.BoxName;
            }

            base.Render(writer);
        }


        void CreateCmdLine(Control parent)
        {

            parent.Controls.Add(new LiteralControl(
                "<tr class='cmdline'><td colspan='2'>"
            ));


            // ���水ť
            Button savebutton = new Button();
            savebutton.ID = "save";
            savebutton.Text = this.GetString("����");
            savebutton.CssClass = "savebutton";
            savebutton.Click += new EventHandler(savebutton_Click);
            parent.Controls.Add(savebutton);

            parent.Controls.Add(new LiteralControl(
                " "
            ));

            // ���Ͱ�ť
            Button sendbutton = new Button();
            sendbutton.ID = "send";
            sendbutton.Text = this.GetString("����");
            sendbutton.CssClass = "sendbutton";
            sendbutton.Click += new EventHandler(sendbutton_Click);
            parent.Controls.Add(sendbutton);

            parent.Controls.Add(new LiteralControl(
    " "
));

            // �ظ���ť
            Button replybutton = new Button();
            replybutton.ID = "reply";
            replybutton.Text = this.GetString("�ظ�");
            replybutton.CssClass = "replybutton";
            replybutton.Click +=new EventHandler(replybutton_Click);
            parent.Controls.Add(replybutton);

            // ת����ť
            Button forwardbutton = new Button();
            forwardbutton.ID = "forward";
            forwardbutton.Text = this.GetString("ת��");
            forwardbutton.CssClass = "forwardbutton";
            forwardbutton.Click +=new EventHandler(forwardbutton_Click);
            parent.Controls.Add(forwardbutton);

            parent.Controls.Add(new LiteralControl(
    " "
));

            // ɾ����ť
            Button deletebutton = new Button();
            deletebutton.ID = "delete";
            deletebutton.Text = this.GetString("ɾ��");
            deletebutton.CssClass = "deletebutton";
            deletebutton.Click +=new EventHandler(deletebutton_Click);;
            parent.Controls.Add(deletebutton);

            parent.Controls.Add(new LiteralControl(
" "
));

            /*
            // ǰһ��Ϣ
            LinkButton prevbutton = new LinkButton();
            prevbutton.ID = "prev";
            prevbutton.Text = "ǰһ��Ϣ";
            prevbutton.Click += new EventHandler(prevbutton_Click);
            parent.Controls.Add(prevbutton);

            parent.Controls.Add(new LiteralControl(
" "
));

            // ��һ��Ϣ
            LinkButton nextbutton = new LinkButton();
            nextbutton.ID = "next";
            nextbutton.Text = "��һ��Ϣ";
            nextbutton.Click += new EventHandler(nextbutton_Click);
            parent.Controls.Add(nextbutton);
             * */

            parent.Controls.Add(new LiteralControl(
                "</td></tr>"
            ));

        }

        // ��һ��Ϣ
        void nextbutton_Click(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        // ǰһ��Ϣ
        void prevbutton_Click(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }



        // ÿ��ǰ�����>����
        string MakeQuoteContent(string strContent)
        {
            StringReader sr= new StringReader(strContent);
            string strResult = "";

            for (; ; )
            {
                string strLine = sr.ReadLine();
                if (strLine == null)
                    break;
                strResult += "> " + strLine + "\r\n";
            }

            sr.Close();

            return strResult;
        }

        void CreateDebugLine(Control parent)
        {

            parent.Controls.Add(new LiteralControl(
                "<tr class='debugline'><td colspan='2'>"
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
            LiteralControl text = (LiteralControl)this.FindControl("debuginfo");
            text.Text = strText;
        }

        void CreateEndInfo(Control parent)
        {

            parent.Controls.Add(new LiteralControl(
                "<tr class='endinfo'><td colspan='2'>"
            ));


            // ������ʾ��Ϣ
            LiteralControl text = new LiteralControl();
            text.ID = "endinfotext";
            parent.Controls.Add(text);

            text = new LiteralControl();
            text.Text = "   ";
            parent.Controls.Add(text);


            // ����ê��
            HyperLink hyperlink = new HyperLink();
            hyperlink.ID = "backlink";
            hyperlink.Text = this.GetString("����");
            hyperlink.NavigateUrl = "./mymessage.aspx";
            parent.Controls.Add(hyperlink);

            parent.Controls.Add(new LiteralControl(
                "</td></tr>"
            ));
        }

        // parameters:
        //      strBackToBox    boxtypeֵ
        void SetEndInfo(string strText,
            string strBackToBox)
        {
            LiteralControl text = (LiteralControl)this.FindControl("endinfotext");
            text.Text = strText;

            this.Mode = "end";

            // �ƶ�Ҫ�����ĸ�����
            if (String.IsNullOrEmpty(strBackToBox) == false)
            {
                HyperLink hyperlink = (HyperLink)this.FindControl("backlink");
                hyperlink.NavigateUrl = "./mymessage.aspx?box=" + HttpUtility.UrlEncode(strBackToBox);
            }

        }

        public void SetMessageData(MessageData data)
        {
            TextBox recipient = (TextBox)this.FindControl("recipient");
            TextBox sender = (TextBox)this.FindControl("sender");
            TextBox subject = (TextBox)this.FindControl("subject");
            TextBox content = (TextBox)this.FindControl("content");
            LiteralControl html_content = (LiteralControl)this.FindControl("html_content");
            PlaceHolder html_content_container = (PlaceHolder)this.FindControl("html_content_container");
            TextBox date = (TextBox)this.FindControl("date");

            recipient.Text = data.strRecipient;
            sender.Text = data.strSender;
            subject.Text = data.strSubject;
            if (data.strMime == "html")
            {
                html_content_container.Visible = true;
                content.Visible = false;

                html_content.Text = data.strBody;
            }
            else
            {
                html_content_container.Visible = false;
                content.Visible = true;

                content.Text = data.strBody;
            }
            date.Text = DateTimeUtil.LocalTime(data.strCreateTime);

            this.RecordID = data.strRecordID;
            this.TimeStamp = data.TimeStamp;
            this.BoxName = data.strBoxType;

            return;
        }

        // ���ð�ť�ͱ༭��״̬
        public void SetState(string strOriginBox)
        {
            TextBox recipient = (TextBox)this.FindControl("recipient");
            TextBox sender = (TextBox)this.FindControl("sender");
            TextBox subject = (TextBox)this.FindControl("subject");
            TextBox content = (TextBox)this.FindControl("content");
            TextBox date = (TextBox)this.FindControl("date");

            Button savebutton = (Button)this.FindControl("save");
            Button sendbutton = (Button)this.FindControl("send");
            Button replybutton = (Button)this.FindControl("reply");
            Button forwardbutton = (Button)this.FindControl("forward");
            Button deletebutton = (Button)this.FindControl("delete");

            //  ����Ǵ������¼�¼
            if (String.IsNullOrEmpty(strOriginBox) == true)
            {
                savebutton.Enabled = true;
                sendbutton.Enabled = true;
                replybutton.Enabled = false;
                forwardbutton.Enabled = false;
                deletebutton.Enabled = false;

                if (String.IsNullOrEmpty(sender.Text) == true)
                {
                    // 2006/11/25 neew add
                    sender.Text = this.UserID;
                }
                sender.ReadOnly = true;

                recipient.ReadOnly = false;
                subject.ReadOnly = false;
                content.ReadOnly = false;    // ���޸�
                date.ReadOnly = true;
            }

            //  ����������ռ���ļ�¼
            else if (MessageCenter.IsInBox(strOriginBox) == true)
            {
                savebutton.Enabled = false;
                sendbutton.Enabled = false;
                replybutton.Enabled = true;
                forwardbutton.Enabled = true;
                deletebutton.Enabled = true;

                sender.ReadOnly = true;
                recipient.ReadOnly = true;
                subject.ReadOnly = true;
                content.ReadOnly = true;    // �����޸�
                date.ReadOnly = true;
            }
            //  ��������Բݸ�ļ�¼
            else if (MessageCenter.IsTemp(strOriginBox) == true)
            {
                savebutton.Enabled = true;
                sendbutton.Enabled = true;
                replybutton.Enabled = false;
                forwardbutton.Enabled = false;
                deletebutton.Enabled = true;

                sender.Text = this.UserID;
                sender.ReadOnly = true;

                recipient.ReadOnly = false;
                subject.ReadOnly = false;

                content.ReadOnly = false;    // ���޸�

                date.ReadOnly = true;
            }
            //  ��������� �ѷ��� �ļ�¼
            else if (MessageCenter.IsOutbox(strOriginBox) == true)
            {
                savebutton.Enabled = false;
                sendbutton.Enabled = true;  // �ٴη���?
                replybutton.Enabled = false;
                forwardbutton.Enabled = true;
                deletebutton.Enabled = true;

                // ȫ�������޸�
                sender.ReadOnly = true;
                recipient.ReadOnly = true;
                subject.ReadOnly = true;
                content.ReadOnly = true;    // �����޸�
                date.ReadOnly = true;
            }
            //  ��������� �ϼ��� �ļ�¼
            else if (MessageCenter.IsRecycleBin(strOriginBox) == true)
            {
                savebutton.Enabled = false;
                sendbutton.Enabled = false;  
                replybutton.Enabled = true;
                forwardbutton.Enabled = true;
                deletebutton.Enabled = true;

                // ȫ�������޸�
                sender.ReadOnly = true;
                recipient.ReadOnly = true;
                subject.ReadOnly = true;
                content.ReadOnly = true;    // �����޸�
                date.ReadOnly = true;
            }

            // ����Ƿϼ����ڵ���Ϣ, ����ɾ��
            if (MessageCenter.IsRecycleBin(this.BoxName) == true)
            {
                deletebutton.Text = this.GetString("����ɾ��");
            }
            else
            {
                deletebutton.Text = this.GetString("�����ϼ���");
            }

        }

        // ���浽�ݸ���
        int SaveToTemp(out string strError)
        {
            strError = "";

            string strOldRecordID = "";
            byte[] baOldTimeStamp = null;

            //  ��������Բݸ�ļ�¼
            if (MessageCenter.IsTemp(this.BoxName) == true)
            {
                strOldRecordID = this.RecordID;
                baOldTimeStamp = this.TimeStamp;
            }

            TextBox recipient = (TextBox)this.FindControl("recipient");
            TextBox subject = (TextBox)this.FindControl("subject");
            TextBox sender = (TextBox)this.FindControl("sender");
            sender.Text = this.UserID;

            TextBox content = (TextBox)this.FindControl("content");

            byte[] baOutputTimeStamp = null;
            string strOutputID = "";

            int nRet = this.MessageCenter.SaveMessage(
                this.Channels,
                recipient.Text,
                sender.Text,
                subject.Text,
                "text",
                content.Text,
                strOldRecordID,   // string strOldRecordID,
                baOldTimeStamp,   // byte [] baOldTimeStamp,
                out baOutputTimeStamp,
                out strOutputID,
                out strError);
            if (nRet == -1)
            {
                return -1;
            }

            // Ϊ�˱����ٴ��޸ĺ󱣴�
            this.RecordID = strOutputID;
            this.TimeStamp = baOutputTimeStamp;

            return 0;
        }

        // ����
        int Send(out string strError)
        {
            strError = "";

            TextBox recipient = (TextBox)this.FindControl("recipient");
            if (String.IsNullOrEmpty(recipient.Text) == true)
            {
                // text-level: �û���ʾ
                strError = this.GetString("��δ��д�ռ���");    // "��δ��д�ռ���"
                return -1;
            }

            TextBox subject = (TextBox)this.FindControl("subject");
            if (String.IsNullOrEmpty(subject.Text) == true)
            {
                // text-level: �û���ʾ
                strError = this.GetString("��δ��д����");  // "��δ��д����"
                return -1;
            }

            TextBox sender = (TextBox)this.FindControl("sender");
            sender.Text = this.UserID;

            TextBox content = (TextBox)this.FindControl("content");

            int nRet = this.MessageCenter.SendMessage(this.Channels,
                recipient.Text,
                sender.Text,
                subject.Text,
                "text",
                content.Text,
                true,
                out strError);
            if (nRet == -1)
                return -1;

            // ������Բݸ���, ����Ҫ��������ɾ��
            if (MessageCenter.IsTemp(this.BoxName) == true)
            {
                nRet = this.MessageCenter.DeleteMessage(
                    false,
                    this.Channels,
                    this.RecordID,
                    this.TimeStamp,
                    out strError);
                if (nRet == -1)
                    return -1;
                this.BoxName = null;    // ���ڲ������κ�����

            }

            return 0;
        }

        // ɾ��
        int Delete(
            out string strError)
        {
            strError = "";
            int nRet = 0;

            if (String.IsNullOrEmpty(this.RecordID) == false)
            {
                // ����Ƿϼ����ڵ���Ϣ, ����ɾ��
                if (MessageCenter.IsRecycleBin(this.BoxName) == true)
                {
                    nRet = this.MessageCenter.DeleteMessage(
                        false,
                        this.Channels,
                        this.RecordID,
                        this.TimeStamp,
                        out strError);
                }
                else
                {
                    // �����ƶ����ϼ���
                    nRet = this.MessageCenter.DeleteMessage(
                        true,
                        this.Channels,
                        this.RecordID,
                        this.TimeStamp,
                        out strError);
                }
                if (nRet == -1)
                    return -1;
                this.BoxName = null;    // ���ڲ������κ�����
            }

            return 0;
        }

        #region ��ť��Ӧ

        // ɾ��
        void deletebutton_Click(object sender, EventArgs e)
        {
            string strError = "";

            bool bDelete = false;
            // ����Ƿϼ����ڵ���Ϣ, ����ɾ��
            if (MessageCenter.IsRecycleBin(this.BoxName) == true)
            {
                bDelete = true;
            }

            int nRet = Delete(out strError);
            if (nRet == -1)
            {
                // text-level: �ڲ�����
                this.SetDebugInfo("ɾ����Ϣʧ��: " + strError);
            }
            else
            {
                // text-level: �û���ʾ
                this.SetDebugInfo(this.GetString("��Ϣɾ���ɹ�"));  // "��Ϣɾ���ɹ�"
                if (bDelete == true)
                {
                    // ���ص���ǰ��������
                    this.SetEndInfo(this.GetString("��Ϣɾ���ɹ�"), null);
                }
                else
                {
                    // ���ص��ϼ���
                    this.SetEndInfo(this.GetString("��Ϣ�ѱ��Ƶ��ϼ���"), // "��Ϣ�ѱ��Ƶ��ϼ��䡣"
                        MessageCenter.TEMP
                        );
                }
            }
        }

        // ת��
        // ʵ������ˢ�´������ݵ��ʺ�ת����״̬
        void forwardbutton_Click(object senderparam, EventArgs e)
        {
            TextBox recipient = (TextBox)this.FindControl("recipient");
            TextBox sender = (TextBox)this.FindControl("sender");
            TextBox subject = (TextBox)this.FindControl("subject");
            TextBox content = (TextBox)this.FindControl("content");
            TextBox date = (TextBox)this.FindControl("date");

            string strRecipient = "";
            string strSender = this.UserID;
            string strSubject = "ת��: " + subject.Text;
            string strContent = ":\r\n���!"
            + "\r\n\r\n\r\n\r\n> === �������� " + sender.Text + " �� " + date.Text + " ���͸� " + recipient.Text + " ������ ===\r\n" + MakeQuoteContent(content.Text) + "\r\n> ======";

            recipient.Text = strRecipient;
            sender.Text = strSender;
            subject.Text = strSubject;
            content.Text = strContent;

            date.Text = "";

            content.ReadOnly = false;    // ���޸�

            this.MessageData = null;
            this.RecordID = null;
            this.TimeStamp = null;
            this.BoxName = null;
        }

        // �ظ�
        // ʵ������ˢ�´������ݵ��ʺϻظ���״̬
        void replybutton_Click(object senderparam, EventArgs e)
        {
            TextBox recipient = (TextBox)this.FindControl("recipient");
            TextBox sender = (TextBox)this.FindControl("sender");
            TextBox subject = (TextBox)this.FindControl("subject");
            TextBox content = (TextBox)this.FindControl("content");
            TextBox date = (TextBox)this.FindControl("date");

            string strRecipient = sender.Text;
            string strSender = recipient.Text;
            string strSubject = "�ظ�: " + subject.Text;
            string strContent = sender.Text + ":\r\n���!"
            + "\r\n\r\n\r\n\r\n> === �������� " + sender.Text + " �� " + date.Text + " ���͸� " + recipient.Text + " ������ ===\r\n" + MakeQuoteContent(content.Text) + "\r\n> ======";

            recipient.Text = strRecipient;
            sender.Text = strSender;
            subject.Text = strSubject;
            content.Text = strContent;

            date.Text = "";

            content.ReadOnly = false;    // ���޸�

            this.MessageData = null;
            this.RecordID = null;
            this.TimeStamp = null;
            this.BoxName = null;
        }

        // ����
        void sendbutton_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = Send(out strError);
            if (nRet == -1)
            {
                // text-level: �ڲ�����
                this.SetDebugInfo("������Ϣʧ��: " + strError);
            }
            else
            {
                this.SetDebugInfo(this.GetString("��Ϣ���ͳɹ�"));  // "��Ϣ���ͳɹ�"
                this.SetEndInfo(this.GetString("��Ϣ���ͳɹ�"),
                    MessageCenter.OUTBOX
                    );
            }

        }

        // ����
        void savebutton_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = SaveToTemp(out strError);
            if (nRet == -1)
            {
                // text-level: �û���ʾ
                this.SetDebugInfo(
                    string.Format(this.GetString("������Ϣ��sʧ�ܣ�ԭ��s"),   // "������Ϣ�� {0} ʧ�ܣ�ԭ��: {1}"
                    this.MessageCenter.GetString("�ݸ�"),
                    strError)
                    /*
                    "������Ϣ�� "
                    + this.MessageCenter.GetString("�ݸ�")
                    + " ʧ��: " + strError*/
                    );
            }
            else
            {
                // text-level: �û���ʾ
                this.SetDebugInfo(this.GetString("��Ϣ����ɹ�"));  // "��Ϣ����ɹ�"
                this.SetEndInfo(this.GetString("��Ϣ����ɹ�"),
                    MessageCenter.TEMP
                    );
            }
        }

        #endregion
    }
}
