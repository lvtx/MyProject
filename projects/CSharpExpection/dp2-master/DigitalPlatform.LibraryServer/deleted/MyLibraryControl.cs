using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using DigitalPlatform.rms.Client;
using DigitalPlatform.Xml;
namespace DigitalPlatform.LibraryServer
{
#if NOOOOOOOOOOOOOOO
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyLibraryControl runat=server></{0}:MyLibraryControl>")]
    public class MyLibraryControl : WebControl, INamingContainer
    {
        /*
        public SessionInfo SessionInfo = null;
        public CirculationApplication App = null;
         */

        public string Barcode = "";

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

        /*
        // ΥԼ��Ϣ��������
        public int OverdueLineCount
        {
            get
            {
                object o = this.Page.Session[this.ID + "MyLibraryControl_OverdueLineCount"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                this.Page.Session[this.ID + "MyLibraryControl_OverdueLineCount"] = (object)value;
            }
        }
         */

        // ������Ϣ��������
        public int BorrowLineCount
        {
            get
            {
                object o = this.Page.Session[this.ID + "MyLibraryControl_BorrowLineCount"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                this.Page.Session[this.ID + "MyLibraryControl_BorrowLineCount"] = (object)value;
            }
        }

        // ԤԼ��Ϣ��������
        public int ReservationLineCount
        {
            get
            {
                object o = this.Page.Session["MyLibraryControl_ReservationLineCount"];
                return (o == null) ? 0 : (int)o;
            }
            set
            {
                this.Page.Session["MyLibraryControl_ReservationLineCount"] = (object)value;
            }
        }

        // �ѽ���������б�
        public List<string> BorrowBarcodes
        {
            get
            {
                object o = this.Page.Session["MyLibraryControl_BorrowBarcodes"];
                return (o == null) ? new List<string>() : (List<string>)o;
            }
            set
            {
                this.Page.Session["MyLibraryControl_BorrowBarcodes"] = (object)value;
            }
        }

        // ��ԤԼ������б�
        public List<string> ReservationBarcodes
        {
            get
            {
                object o = this.Page.Session["MyLibraryControl_ReservationBarcodes"];
                return (o == null) ? new List<string>() : (List<string>)o;
            }
            set
            {
                this.Page.Session["MyLibraryControl_ReservationBarcodes"] = (object)value;
            }
        }

        // ���ֿؼ�
        protected override void CreateChildControls()
        {
            // ����Բ�νǵĿ���

            // ��Ҫ��ʶ: ֤����š�������֤�䷢����Ч��
            PlaceHolder identity = new PlaceHolder();
            identity.ID = "identity";
            this.Controls.Add(identity);

            CreateIdentityControls(identity);

            LiteralControl sep = new LiteralControl();
            sep.Text = "<br/>";
            this.Controls.Add(sep);


            // һ����Ϣ���Ա�סַ�ȵ� ���Գ�ʼ��Ϊ����״̬
            PlaceHolder generalinfo = new PlaceHolder();
            generalinfo.ID = "generalinfo";
            this.Controls.Add(generalinfo);

            CreateGeneralInfoControls(generalinfo);

            sep = new LiteralControl();
            sep.Text = "<br/>";
            this.Controls.Add(sep);


            // ΥԼ��Ϣ
            PlaceHolder overdueinfo = new PlaceHolder();
            overdueinfo.ID = "overdueinfo";
            this.Controls.Add(overdueinfo);

            CreateOverdueInfoControls(overdueinfo);

            sep = new LiteralControl();
            sep.Text = "<br/>";
            this.Controls.Add(sep);


            // ������Ϣ
            PlaceHolder borrowinfo = new PlaceHolder();
            borrowinfo.ID = "borrowinfo";
            this.Controls.Add(borrowinfo);

            CreateBorrowInfoControls(borrowinfo);

            sep = new LiteralControl();
            sep.Text = "<br/>";
            this.Controls.Add(sep);

            // ԤԼ����
            PlaceHolder reservation = new PlaceHolder();
            reservation.ID = "reservation";
            this.Controls.Add(reservation);

            CreateReservationControls(reservation);

            sep = new LiteralControl();
            sep.Text = "<br/>";
            this.Controls.Add(sep);

            // ��������
            PlaceHolder commands = new PlaceHolder();
            commands.ID = "commands";
            this.Controls.Add(commands);

            CreateCommandsControls(commands);


            // �Լ����ƵĽ�������ף����������
            PlaceHolder prefer = new PlaceHolder();
            prefer.ID = "prefer";
            this.Controls.Add(prefer);

        }

        void CreateRoundTableStart(Control parent,
            string strID,
            string strTitleText,
            out Button button)
        {
            button = null;

            string strResult = "";
            strResult += "<table class='roundtable' cellpadding='0' cellspacing='0' border='0' width='100%'>";
            strResult += "  <tr>";
            strResult += "      <td>";
            strResult += "          <div id='unRestricted'>";
            strResult += "              <div class='topCorners'>";
            strResult += "                  <div class='l1'></div>";
            strResult += "                  <div class='l2'></div>";
            strResult += "                  <div class='l3'></div>";
            strResult += "                  <div class='l4'></div>";
            strResult += "              </div>";

            strResult += "              <div id='quickInfoHeader'>";
            strResult += "                  <div style='text-align:left;'>";

            LiteralControl literal = new LiteralControl();
            literal.Text = strResult;
            parent.Controls.Add(literal);


            button = new Button();
            button.ID = strID + "_expandbutton";
            button.Text = "-";
            parent.Controls.Add(button);

            literal = new LiteralControl();
            literal.ID = strID + "_titletext";
            literal.Text = strTitleText;
            parent.Controls.Add(literal);

            strResult =  "                  </div>";
            strResult += "              </div>";

            literal = new LiteralControl();
            literal.Text = strResult;
            parent.Controls.Add(literal);

           
        }

        /*
        string GetRoundTableStart(string strTableTitle)
        {
            string strResult = "";
            strResult += "<table class='roundtable' cellpadding='0' cellspacing='0' border='0' width='100%'>";
            strResult += "  <tr>";
            strResult += "      <td>";
            strResult += "          <div id='unRestricted'>";
            strResult += "              <div class='topCorners'>";
            strResult += "                  <div class='l1'></div>";
            strResult += "                  <div class='l2'></div>";
            strResult += "                  <div class='l3'></div>";
            strResult += "                  <div class='l4'></div>";
            strResult += "              </div>";

            strResult += "              <div id='quickInfoHeader'>";
            strResult += "                  <div style='text-align:left;'>"+strTableTitle+"</div>";
            strResult += "              </div>";

            return strResult;
        }
         */

        string GetRoundTableEnd()
        {
            string strResult = "";

            strResult += "              <div class='bottomCorners'>";
            strResult += "			        <div class='l4'></div>";
            strResult += "                  <div class='l3'></div>";
            strResult += "                  <div class='l2'></div>";
            strResult += "                  <div class='l1'></div>";
            strResult += "              </div>";

		    strResult += "          </div>";
	        strResult += "      </td>";
            strResult += "  </tr>";
            strResult += "</table>";

            return strResult;
        }

        // ������ʼ��ʶ����Ϣ��Ϣ�ڲ��в���
        void CreateIdentityControls(PlaceHolder parent)
        {
            Button identityExpandButton = null;
            CreateRoundTableStart(parent, "identity", "ʶ����Ϣ", out identityExpandButton);
            identityExpandButton.Click += new EventHandler(identityExpandButton_Click);

            PlaceHolder identity = new PlaceHolder();
            identity.ID = "identity_content";
            parent.Controls.Add(identity);

            LiteralControl titleline = new LiteralControl();
            titleline.ID = "identity_titleline";
            titleline.Text = "<table cellpadding='0' cellspacing='0' border='0' width='100%'>";
            // titleline.Text += "<tr class='roundtitle'><td nowrap>����</td><td nowrap>ֵ</td></tr>";
            identity.Controls.Add(titleline);

            // ÿһ��һ��ռλ�ؼ�
            NewIdentityLine(identity, "����", "identity_name", true);
            NewIdentityLine(identity, "����֤�����", "identity_barcode", true);
            NewIdentityLine(identity, "״̬", "identity_state", true);
            NewIdentityLine(identity, "�������", "identity_readertype", false);
            NewIdentityLine(identity, "��֤����", "identity_createdate", false);
            NewIdentityLine(identity, "ʧЧ����", "identity_expiredate", false);


            // ���һ���հ���
            LiteralControl literal = new LiteralControl();
            literal.ID = "identity_blank";
            literal.Text = "<tr class='roundcontent'><td colspan='9'>&nbsp;</td></tr>";
            identity.Controls.Add(literal);


            literal = new LiteralControl();
            literal.ID = "identity_tableend";
            literal.Text = "</table>";
            identity.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text =  GetRoundTableEnd();
            parent.Controls.Add(literal);
        }

        void identityExpandButton_Click(object sender, EventArgs e)
        {
            ExpandContent("identity");
        }

        void ExpandContent(string strID)
        {
            Button button = (Button)this.FindControl(strID + "_expandbutton");
            Control control = this.FindControl(strID + "_content");
            if (control.Visible == true)
            {
                control.Visible = false;
                button.Text = "+";
            }
            else
            {
                control.Visible = true;
                button.Text = "-";
            }
        }

        PlaceHolder NewIdentityLine(Control parent,
            string strName,
            string strValueControlID,
            bool bLarge)
        {

            string strClass = "";
            if (bLarge == true)
                strClass = " class='largecontent' ";

            PlaceHolder line = new PlaceHolder();
            line.ID = "line" + strValueControlID;

            if (FindControl(line.ID) != null)
                throw new Exception("id='" + line.ID + "' already existed");


            parent.Controls.Add(line);

            // �������
            LiteralControl literal = new LiteralControl();
            literal.Text = "<tr class='roundcontentdark'><td>" + strName + "</td><td "+strClass+" >";
            line.Controls.Add(literal);

            // ֵ
            literal = new LiteralControl();
            literal.ID = strValueControlID;
            literal.Text = "(��)";
            line.Controls.Add(literal);

            // �Ҳ�����
            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            line.Controls.Add(literal);


            return line;
        }

        // һ������
        void CreateCommandsControls(PlaceHolder parent)
        {
            // �޸�����
            HyperLink changepassword = new HyperLink();
            changepassword.Text = "�޸�����";
            changepassword.NavigateUrl = "./changereaderpassword.aspx";
            parent.Controls.Add(changepassword);

            // �޸ĸ�����Ϣ

        }

        // ������ʼ��һ����Ϣ�ڲ��в���
        void CreateGeneralInfoControls(PlaceHolder parent)
        {
            /*
            LiteralControl tablebegin = new LiteralControl();
            tablebegin.ID = "generalinfo_tablebegin";
            tablebegin.Text = GetRoundTableStart("һ����Ϣ") + "<table cellpadding='0' cellspacing='0' border='0' width='100%'>";
            generalinfo.Controls.Add(tablebegin);
             */
            Button generalinfoExpandButton = null;
            CreateRoundTableStart(parent, "generalinfo", "һ����Ϣ", out generalinfoExpandButton);
            generalinfoExpandButton.Click += new EventHandler(generalinfoExpandButton_Click);


            PlaceHolder generalinfo = new PlaceHolder();
            generalinfo.ID = "generalinfo_content";
            parent.Controls.Add(generalinfo);


            LiteralControl titleline = new LiteralControl();
            titleline.ID = "generalinfo_titleline";
            titleline.Text = "<table cellpadding='0' cellspacing='0' border='0' width='100%'>";
            // titleline.Text += "<tr class='roundtitle'><td nowrap>����</td><td nowrap>ֵ</td></tr>";
            generalinfo.Controls.Add(titleline);

            // ÿһ��һ��ռλ�ؼ�
            NewGeneralInfoLine(generalinfo, "�Ա�", "generalinfo_gender");
            NewGeneralInfoLine(generalinfo, "����", "generalinfo_birthday");
            NewGeneralInfoLine(generalinfo, "���֤��", "generalinfo_idcardnumber");
            NewGeneralInfoLine(generalinfo, "��λ", "generalinfo_department");
            NewGeneralInfoLine(generalinfo, "��ַ", "generalinfo_address");
            NewGeneralInfoLine(generalinfo, "�绰", "generalinfo_tel");
            NewGeneralInfoLine(generalinfo, "Email��ַ", "generalinfo_email");


            // ���һ���հ���
            LiteralControl literal = new LiteralControl();
            literal.ID = "generalinfo_blank";
            literal.Text = "<tr class='roundcontent'><td colspan='9'>&nbsp;</td></tr>";
            generalinfo.Controls.Add(literal);


            literal = new LiteralControl();
            literal.ID = "generalinfo_tableend";
            literal.Text = "</table>";
            generalinfo.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = GetRoundTableEnd();
            parent.Controls.Add(literal);


        }

        void generalinfoExpandButton_Click(object sender, EventArgs e)
        {
            ExpandContent("generalinfo");
        }

        PlaceHolder NewGeneralInfoLine(Control parent,
    string strName,
    string strValueControlID)
        {
            PlaceHolder line = new PlaceHolder();
            line.ID = "line" + strValueControlID;

            if (FindControl(line.ID) != null)
                throw new Exception("id='" + line.ID + "' already existed");

            parent.Controls.Add(line);

            // �������
            LiteralControl literal = new LiteralControl();
            literal.Text = "<tr class='roundcontentdark'><td>" + strName + "</td><td>";
            line.Controls.Add(literal);

            // ֵ
            literal = new LiteralControl();
            literal.ID = strValueControlID;
            literal.Text = "(��)";
            line.Controls.Add(literal);

            // �Ҳ�����
            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            line.Controls.Add(literal);


            return line;
        }

        // ������ʼ��ΥԼ��Ϣ�ڲ��в���
        void CreateOverdueInfoControls(PlaceHolder parent)
        {
            Button overdueinfoExpandButton = null;
            CreateRoundTableStart(parent, "overdueinfo", "ΥԼ��Ϣ", out overdueinfoExpandButton);
            overdueinfoExpandButton.Click += new EventHandler(overdueinfoExpandButton_Click);

            PlaceHolder overdueinfo = new PlaceHolder();
            overdueinfo.ID = "overdueinfo_content";
            parent.Controls.Add(overdueinfo);


            LiteralControl titleline = new LiteralControl();
            titleline.ID = "overdueinfo_titleline";
            titleline.Text = "<table cellpadding='0' cellspacing='0' border='0' width='100%'>";
            titleline.Text += "<tr class='roundtitle'><td nowrap>�������</td><td nowrap width='50%'>ժҪ</td><td nowrap>ΥԼ���</td><td nowrap>��������</td><td nowrap>����</td><td nowrap>��������</td></tr>";
            overdueinfo.Controls.Add(titleline);

            /*
            // ÿһ��һ��ռλ�ؼ�
            for (int i = 0; i < this.OverdueLineCount; i++)
            {
                PlaceHolder line = NewOverdueLine(overdueinfo, i, null);
                line.Visible = true;
            }
             */

            // ����в����
            PlaceHolder insertpos = new PlaceHolder();
            insertpos.ID = "overdueinfo_insertpos";
            overdueinfo.Controls.Add(insertpos);

            // ���һ���հ���
            LiteralControl literal = new LiteralControl();
            literal.ID = "overdueinfo_blank";
            literal.Text = "<tr class='roundcontent'><td colspan='9'>&nbsp;</td></tr>";
            overdueinfo.Controls.Add(literal);


            literal = new LiteralControl();
            literal.ID = "overdueinfo_tableend";
            literal.Text = "</table>";
            overdueinfo.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = GetRoundTableEnd();
            parent.Controls.Add(literal);


        }

        void overdueinfoExpandButton_Click(object sender, EventArgs e)
        {
            // off��Ͻ�ڵ�checkbox
            OffCheckBoxes("overdueinfo");

            ExpandContent("overdueinfo");
        }

        //

        // ������ʼ�Ľ�����Ϣ�ڲ��в���
        void CreateBorrowInfoControls(PlaceHolder parent)
        {
            Button borrowinfoExpandButton = null;
            CreateRoundTableStart(parent, "borrowinfo", "������Ϣ", out borrowinfoExpandButton);
            borrowinfoExpandButton.Click += new EventHandler(borrowinfoExpandButton_Click);

            PlaceHolder borrowinfo = new PlaceHolder();
            borrowinfo.ID = "borrowinfo_content";
            parent.Controls.Add(borrowinfo);


            LiteralControl titleline = new LiteralControl();
            titleline.ID = "borrowinfo_titleline";
            titleline.Text = "<table cellpadding='0' cellspacing='0' border='0' width='100%'>";
            titleline.Text += "<tr class='roundtitle'><td nowrap>�������</td><td nowrap width='50%'>ժҪ</td><td nowrap>�����</td><td nowrap>��������</td><td nowrap>����</td><td nowrap>������</td><td nowrap>�Ƿ���</td><td nowrap>��ע</td></tr>";
            borrowinfo.Controls.Add(titleline);

            // ÿһ��һ��ռλ�ؼ�
            for (int i = 0; i < this.BorrowLineCount; i++)
            {
                PlaceHolder line = NewBorrowLine(borrowinfo, i, null);
                line.Visible = true;
            }

            // ����в����
            PlaceHolder insertpos = new PlaceHolder();
            insertpos.ID = "borrowinfo_insertpos";
            borrowinfo.Controls.Add(insertpos);


            //
            // ������
            PlaceHolder cmdline = new PlaceHolder();
            cmdline.ID = "borrowinfo_cmdline";
            borrowinfo.Controls.Add(cmdline);

            LiteralControl literal = new LiteralControl();
            literal.Text = "<tr class='roundcontent'><td colspan='8'>";
            cmdline.Controls.Add(literal);

            Button renewButton = new Button();
            renewButton.ID = "borrowinfo_renewbutton";
            renewButton.Text = "����";
            renewButton.Click +=new EventHandler(renewButton_Click);
            cmdline.Controls.Add(renewButton);
            renewButton = null;

            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            cmdline.Controls.Add(literal);

            cmdline = null;

            // ������Ϣ��
            PlaceHolder debugline = new PlaceHolder();
            debugline.ID = "borrowinfo_debugline";
            borrowinfo.Controls.Add(debugline);

            literal = new LiteralControl();
            literal.Text = "<tr class='roundcontent'><td colspan='8'>";
            debugline.Controls.Add(literal);

            literal = new LiteralControl();
            literal.ID = "borrowinfo_debugtext";
            literal.Text = "";
            debugline.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            debugline.Controls.Add(literal);

            debugline = null;

            //
            /*

            // ���һ���հ���
            LiteralControl literal = new LiteralControl();
            literal.ID = "borrowinfo_blank";
            literal.Text = "<tr class='roundcontent'><td colspan='9'>&nbsp;</td></tr>";
            borrowinfo.Controls.Add(literal);
             */


            literal = new LiteralControl();
            literal.ID = "borrowinfo_tableend";
            literal.Text = "</table>";
            borrowinfo.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = GetRoundTableEnd();
            parent.Controls.Add(literal);


        }

        void renewButton_Click(object sender, EventArgs e)
        {
            List<string> barcodes = this.GetCheckedBorrowBarcodes();

            if (barcodes.Count == 0)
            {
                SetBorrowDebugInfo("��δѡ��Ҫ��������");
                return;
            }

            LibraryApplication app = (LibraryApplication)this.Page.Application["app"];
            SessionInfo sessioninfo = (SessionInfo)this.Page.Session["sessioninfo"];

            for (int i = 0; i < barcodes.Count; i++)
            {
                string strItemBarcode = barcodes[i];
                //string strItemRecord = "";
                //string strReaderRecord = "";

                string strReaderBarcode = "";
                if (String.IsNullOrEmpty(this.Barcode) == false)
                    strReaderBarcode = this.Barcode;
                else
                    strReaderBarcode = sessioninfo.Account.Barcode;

                if (String.IsNullOrEmpty(strReaderBarcode) == true)
                {
                    SetBorrowDebugInfo("��δָ������֤����š�����ʧ�ܡ�");
                    return;
                }

                string[] aDupPath = null;
                string[] item_records = null;
                string[] reader_records = null;
                string[] biblio_records = null;
                BorrowInfo borrow_info = null;

                LibraryServerResult result = app.Borrow(
                    sessioninfo,
                    true,
                    strReaderBarcode,
                    strItemBarcode,
                    null,
                    false,
                    null,
                    "", // style
                    "",
                    out item_records,
                    "",
                    out reader_records,
                    "",
                    out biblio_records,
                    out aDupPath,
                    out borrow_info);

                if (result.Value == -1)
                {
                    SetBorrowDebugInfo(result.ErrorInfo);
                    return;
                }
            }


            SetBorrowDebugInfo("����ɹ���");
        }

        void SetBorrowDebugInfo(string strText)
        {
            LiteralControl text = (LiteralControl)this.FindControl("borrowinfo_debugtext");

            text.Text = strText;
        }

        void borrowinfoExpandButton_Click(object sender, EventArgs e)
        {
            // off��Ͻ�ڵ�checkbox
            OffCheckBoxes("borrowinfo");

            ExpandContent("borrowinfo");
        }

        // ������ʼ��ԤԼ��Ϣ�ڲ��в���
        void CreateReservationControls(PlaceHolder parent)
        {
            parent.Controls.Clear();

            /*
            LiteralControl tablebegin = new LiteralControl();
            tablebegin.ID = "reservation_tablebegin";
            tablebegin.Text = GetRoundTableStart("ԤԼ��Ϣ") + "<table cellpadding='0' cellspacing='0' border='0' width='100%'>";
            reservation.Controls.Add(tablebegin);
             */
            Button reservationExpandButton = null;
            CreateRoundTableStart(parent, "reservation", "ԤԼ��Ϣ", out reservationExpandButton);
            reservationExpandButton.Click += new EventHandler(reservationExpandButton_Click);

            PlaceHolder reservation = new PlaceHolder();
            reservation.ID = "reservation_content";
            parent.Controls.Add(reservation);


            LiteralControl titleline = new LiteralControl();
            titleline.ID = "reservation_titleline";
            titleline.Text = "<table cellpadding='0' cellspacing='0' border='0' width='100%'>";
            titleline.Text += "<tr  class='roundtitle'><td nowrap>�������</td><td nowrap>�������</td><td nowrap width='50%'>ժҪ</td><td nowrap>��������</td><td nowrap>������</td></tr>";
            reservation.Controls.Add(titleline);

            // ÿһ��һ��ռλ�ؼ�
            for (int i = 0; i < this.ReservationLineCount; i++)
            {
                PlaceHolder line = NewReservationLine(reservation, i, null);
                line.Visible = true;
            }

            // ����в����
            PlaceHolder insertpos = new PlaceHolder();
            insertpos.ID = "reservation_insertpos";
            reservation.Controls.Add(insertpos);

            // ������
            PlaceHolder cmdline = new PlaceHolder();
            cmdline.ID = "reservation_cmdline";
            reservation.Controls.Add(cmdline);

            LiteralControl literal = new LiteralControl();
            literal.Text = "<tr class='roundcontent'><td colspan='5'>";
            cmdline.Controls.Add(literal);

            literal = new LiteralControl();
            literal.Text = "&nbsp;(ע��ɾ��״̬Ϊ���ѵ��顱���б�ʾ���߷���ȡ�顣���Ҫȥͼ�������ȡ�飬��һ����Ҫɾ���������У���ȡ����ɺ�������Զ�ɾ��)<br/>";
            cmdline.Controls.Add(literal);

            Button reservationDeleteButton = new Button();
            reservationDeleteButton.ID = "reservation_deletebutton";
            reservationDeleteButton.Text = "ɾ��";
            reservationDeleteButton.Click -= new EventHandler(reservationDeleteButton_Click);
            reservationDeleteButton.Click += new EventHandler(reservationDeleteButton_Click);
            cmdline.Controls.Add(reservationDeleteButton);
            reservationDeleteButton = null;


            literal = new LiteralControl();
            literal.Text = "&nbsp;";
            cmdline.Controls.Add(literal);


            Button reservationMergeButton = new Button();
            reservationMergeButton.ID = "reservation_mergebutton";
            reservationMergeButton.Text = "�ϲ�";
            reservationMergeButton.Click -= new EventHandler(reservationMergeButton_Click);
            reservationMergeButton.Click += new EventHandler(reservationMergeButton_Click);
            cmdline.Controls.Add(reservationMergeButton);
            reservationMergeButton = null;

            literal = new LiteralControl();
            literal.Text = "&nbsp;";
            cmdline.Controls.Add(literal);


            Button reservationSplitButton = new Button();
            reservationSplitButton.ID = "reservation_splitbutton";
            reservationSplitButton.Text = "��ɢ";
            reservationSplitButton.Click -= new EventHandler(reservationSplitButton_Click);
            reservationSplitButton.Click += new EventHandler(reservationSplitButton_Click);
            cmdline.Controls.Add(reservationSplitButton);
            reservationSplitButton = null;


            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            cmdline.Controls.Add(literal);

            cmdline = null;

            // ������Ϣ��
            PlaceHolder debugline = new PlaceHolder();
            debugline.ID = "reservation_debugline";
            reservation.Controls.Add(debugline);

            literal = new LiteralControl();
            literal.Text = "<tr class='roundcontent'><td colspan='5'>";
            debugline.Controls.Add(literal);

            literal = new LiteralControl();
            literal.ID = "reservation_debugtext";
            literal.Text = "";
            debugline.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = "</td></tr>";
            debugline.Controls.Add(literal);

            debugline = null;


            // ����β
            literal = new LiteralControl();
            literal.ID = "reservation_tableend";
            literal.Text = "</table>";
            reservation.Controls.Add(literal);


            literal = new LiteralControl();
            literal.Text = GetRoundTableEnd();
            parent.Controls.Add(literal);
        }

        void reservationExpandButton_Click(object sender, EventArgs e)
        {
            // off��Ͻ�ڵ�checkbox
            OffCheckBoxes("reservation");

            ExpandContent("reservation");
        }

        void OffCheckBoxes(string strID)
        {

            for (int i = 0; ; i++)
            {
                CheckBox checkbox = (CheckBox)this.FindControl(strID + "_line" + Convert.ToString(i) + "checkbox");
                if (checkbox == null)
                    break;
                if (checkbox.Checked == true)
                    checkbox.Checked = false;
            }
        }

        string GetChekcedReservationBarcodes()
        {
            string strBarcodeList = "";

            PlaceHolder reservation = (PlaceHolder)this.FindControl("reservation");

            for (int i = 0; i < this.ReservationLineCount; i++)
            {
                CheckBox checkbox = (CheckBox)reservation.FindControl("reservation_line" + Convert.ToString(i) + "checkbox");
                if (checkbox.Checked == true)
                {
                    if (this.ReservationBarcodes.Count <= i)
                    {
                        //this.SetReservationDebugInfo("ReservationBarcodesʧЧ...");
                        //return null;
                        throw new Exception("ReservationBarcodesʧЧ...");
                    }
                    string strBarcode = this.ReservationBarcodes[i];

                    if (strBarcodeList != "")
                        strBarcodeList += ",";
                    strBarcodeList += strBarcode;
                    checkbox.Checked = false;
                }
            }

            return strBarcodeList;
        }

        List<string> GetCheckedBorrowBarcodes()
        {
            List<string> barcodes = new List<string>();

            for (int i = 0; i < this.BorrowLineCount; i++)
            {
                CheckBox checkbox = (CheckBox)this.FindControl("borrowinfo_line" + Convert.ToString(i) + "checkbox");
                if (checkbox.Checked == true)
                {
                    if (this.BorrowBarcodes.Count <= i)
                    {
                        throw new Exception("BorrowBarcodesʧЧ...");
                    }
                    string strBarcode = this.BorrowBarcodes[i];

                    barcodes.Add(strBarcode);

                    checkbox.Checked = false;
                }
            }

            return barcodes;
        }

        // ԤԼ����ɢ����
        void reservationSplitButton_Click(object sender, EventArgs e)
        {
            string strBarcodeList = GetChekcedReservationBarcodes();

            if (String.IsNullOrEmpty(strBarcodeList) == true)
            {
                SetReservationDebugInfo("��δѡ��Ҫ��ɢ��ԤԼ���");
                return;
            }

            LibraryApplication app = (LibraryApplication)this.Page.Application["app"];
            SessionInfo sessioninfo = (SessionInfo)this.Page.Session["sessioninfo"];

            string strReaderBarcode = "";
            if (String.IsNullOrEmpty(this.Barcode) == false)
                strReaderBarcode = this.Barcode;
            else
                strReaderBarcode = sessioninfo.Account.Barcode;

            if (String.IsNullOrEmpty(strReaderBarcode) == true)
            {
                SetBorrowDebugInfo("��δָ������֤����š�����ʧ�ܡ�");
                return;
            }

            LibraryServerResult result = app.Reservation(sessioninfo,
                "split",
                strReaderBarcode,
                strBarcodeList);
            if (result.Value == -1)
                SetReservationDebugInfo(result.ErrorInfo);
            else
                SetReservationDebugInfo("��ɢԤԼ��Ϣ�ɹ����뿴ԤԼ�б�");
        }

        // ԤԼ���ϲ�����
        void reservationMergeButton_Click(object sender, EventArgs e)
        {
            string strBarcodeList = GetChekcedReservationBarcodes();

            if (String.IsNullOrEmpty(strBarcodeList) == true)
            {
                SetReservationDebugInfo("��δѡ��Ҫ�ϲ���ԤԼ���");
                return;
            }

            LibraryApplication app = (LibraryApplication)this.Page.Application["app"];
            SessionInfo sessioninfo = (SessionInfo)this.Page.Session["sessioninfo"];

            string strReaderBarcode = "";
            if (String.IsNullOrEmpty(this.Barcode) == false)
                strReaderBarcode = this.Barcode;
            else
                strReaderBarcode = sessioninfo.Account.Barcode;

            if (String.IsNullOrEmpty(strReaderBarcode) == true)
            {
                SetBorrowDebugInfo("��δָ������֤����š�����ʧ�ܡ�");
                return;
            }

            LibraryServerResult result = app.Reservation(sessioninfo,
                "merge",
                strReaderBarcode,
                strBarcodeList);
            if (result.Value == -1)
                SetReservationDebugInfo(result.ErrorInfo);
            else
                SetReservationDebugInfo("�ϲ�ԤԼ��Ϣ�ɹ����뿴ԤԼ�б�");


        }

        // ԤԼ��ɾ������
        void reservationDeleteButton_Click(object sender, EventArgs e)
        {
            string strBarcodeList = GetChekcedReservationBarcodes();

            if (String.IsNullOrEmpty(strBarcodeList) == true)
            {
                SetReservationDebugInfo("��δѡ��Ҫɾ����ԤԼ���");
                return;
            }

            LibraryApplication app = (LibraryApplication)this.Page.Application["app"];
            SessionInfo sessioninfo = (SessionInfo)this.Page.Session["sessioninfo"];

            string strReaderBarcode = "";
            if (String.IsNullOrEmpty(this.Barcode) == false)
                strReaderBarcode = this.Barcode;
            else
                strReaderBarcode = sessioninfo.Account.Barcode;

            if (String.IsNullOrEmpty(strReaderBarcode) == true)
            {
                SetBorrowDebugInfo("��δָ������֤����š�����ʧ�ܡ�");
                return;
            }

            LibraryServerResult result = app.Reservation(sessioninfo,
                "delete",
                strReaderBarcode,
                strBarcodeList);
            if (result.Value == -1)
                SetReservationDebugInfo(result.ErrorInfo);
            else
                SetReservationDebugInfo("ɾ��ԤԼ��Ϣ�ɹ����뿴ԤԼ�б�");

            // Button button = (Button)FindControl("reservation_deletebutton");
        }

        PlaceHolder NewOverdueLine(Control parent,
            int index,
            Control insertbefore)
        {
            if (parent != null && insertbefore != null)
            {
                if (insertbefore.Parent != parent)
                    throw new Exception("����ο�λ�ú͸�Control֮��, ���ӹ�ϵ����ȷ");
            }


            PlaceHolder line = new PlaceHolder();
            line.ID = "overdueinfo_line" + Convert.ToString(index);

            if (FindControl(line.ID) != null)
                throw new Exception("id='" + line.ID + "' already existed");

            if (insertbefore == null)
                parent.Controls.Add(line);
            else
            {
                int pos = parent.Controls.IndexOf(insertbefore);
                if (pos == -1)
                    throw new Exception("������ն���û���ҵ�");
                parent.Controls.AddAt(pos, line);
            }

            // �������
            LiteralControl literal = new LiteralControl();
            literal.ID = "overdueinfo_line" + Convert.ToString(index) + "left";
            literal.Text = "<tr><td></td></tr>";
            line.Controls.Add(literal);

            /*
            // checkbox
            CheckBox checkbox = new CheckBox();
            checkbox.ID = "overdueinfo_line" + Convert.ToString(index) + "checkbox";
            line.Controls.Add(checkbox);

            // �Ҳ�����
            literal = new LiteralControl();
            literal.ID = "overdueinfo_line" + Convert.ToString(index) + "right";
            literal.Text = "</td></tr>";
            line.Controls.Add(literal);
             */


            return line;
        }

        PlaceHolder NewBorrowLine(Control parent,
            int index,
            Control insertbefore)
        {
            if (parent != null && insertbefore != null)
            {
                if (insertbefore.Parent != parent)
                    throw new Exception("����ο�λ�ú͸�Control֮��, ���ӹ�ϵ����ȷ");
            }


            PlaceHolder line = new PlaceHolder();
            line.ID = "borrowinfo_line" + Convert.ToString(index);

            if (FindControl(line.ID) != null)
                throw new Exception("id='" + line.ID + "' already existed");

            if (insertbefore == null)
                parent.Controls.Add(line);
            else
            {
                int pos = parent.Controls.IndexOf(insertbefore);
                if (pos == -1)
                    throw new Exception("������ն���û���ҵ�");
                parent.Controls.AddAt(pos, line);
            }

            // �������
            LiteralControl literal = new LiteralControl();
            literal.ID = "borrowinfo_line" + Convert.ToString(index) + "left";
            literal.Text = "<tr><td>";
            line.Controls.Add(literal);

            // checkbox
            CheckBox checkbox = new CheckBox();
            checkbox.ID = "borrowinfo_line" + Convert.ToString(index) + "checkbox";
            line.Controls.Add(checkbox);

            // �Ҳ�����
            literal = new LiteralControl();
            literal.ID = "borrowinfo_line" + Convert.ToString(index) + "right";
            literal.Text = "</td></tr>";
            line.Controls.Add(literal);


            return line;
        }

        PlaceHolder NewReservationLine(Control parent,
    int index,
    Control insertbefore)
        {
            PlaceHolder line = new PlaceHolder();
            line.ID = "reservation_line" + Convert.ToString(index);

            if (FindControl(line.ID) != null)
                throw new Exception("id='" + line.ID + "' already existed");

            if (insertbefore == null)
                parent.Controls.Add(line);
            else
            {
                int pos = parent.Controls.IndexOf(insertbefore);
                parent.Controls.AddAt(pos, line);
            }

            // �������
            LiteralControl literal = new LiteralControl();
            literal.ID = "reservation_line" + Convert.ToString(index) + "left";
            literal.Text = "<tr><td>";
            line.Controls.Add(literal);

            // checkbox
            CheckBox checkbox = new CheckBox();
            checkbox.ID = "reservation_line" + Convert.ToString(index) + "checkbox";
            line.Controls.Add(checkbox);

            // �Ҳ�����
            literal = new LiteralControl();
            literal.ID = "reservation_line" + Convert.ToString(index) + "right";
            literal.Text = "</td></tr>";
            line.Controls.Add(literal);


            return line;
        }

        void SetReservationDebugInfo(string strText)
        {
            PlaceHolder reservation = (PlaceHolder)this.FindControl("reservation");

            PlaceHolder line = (PlaceHolder)reservation.FindControl("reservation_debugline");
            LiteralControl text = (LiteralControl)line.FindControl("reservation_debugtext");

            text.Text = strText;
        }

        void RenderIdentity(XmlDocument dom)
        {
            // ʶ����Ϣ
            PlaceHolder identity = (PlaceHolder)this.FindControl("identity");

            LiteralControl literal = (LiteralControl)identity.FindControl("identity_name");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "name");

            literal = (LiteralControl)identity.FindControl("identity_barcode");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "barcode");

            literal = (LiteralControl)identity.FindControl("identity_state");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "state");


            literal = (LiteralControl)identity.FindControl("identity_readertype");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "readerType");

            literal = (LiteralControl)identity.FindControl("identity_createdate");
            literal.Text = ItemConverter.LocalDate(DomUtil.GetElementText(dom.DocumentElement, "createDate"));

            literal = (LiteralControl)identity.FindControl("identity_expiredate");
            literal.Text = ItemConverter.LocalDate(DomUtil.GetElementText(dom.DocumentElement, "expireDate"));
        }

        void RenderGeneralInfo(XmlDocument dom)
        {
            // һ����Ϣ
            PlaceHolder generalinfo = (PlaceHolder)this.FindControl("generalinfo");

            LiteralControl literal = (LiteralControl)generalinfo.FindControl("generalinfo_gender");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "gender");

            literal = (LiteralControl)generalinfo.FindControl("generalinfo_birthday");
            literal.Text = ItemConverter.LocalDate(DomUtil.GetElementText(dom.DocumentElement, "birthday"));

            literal = (LiteralControl)generalinfo.FindControl("generalinfo_idcardnumber");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "idCardNumber");

            literal = (LiteralControl)generalinfo.FindControl("generalinfo_department");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "department");

            literal = (LiteralControl)generalinfo.FindControl("generalinfo_address");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "address");

            literal = (LiteralControl)generalinfo.FindControl("generalinfo_tel");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "tel");

            literal = (LiteralControl)generalinfo.FindControl("generalinfo_email");
            literal.Text = DomUtil.GetElementText(dom.DocumentElement, "email");

        }

        void RenderOverdue(
            LibraryApplication app,
            SessionInfo sessioninfo,
            XmlDocument dom)
        {
            
            PlaceHolder overdueinfo = (PlaceHolder)this.FindControl("overdueinfo");

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("overdues/overdue");
            for (int i = 0; i < nodes.Count; i++)
            {
                PlaceHolder line = (PlaceHolder)overdueinfo.FindControl("overdueinfo_line" + Convert.ToString(i));
                if (line == null)
                {
                    Control insertpos = overdueinfo.FindControl("overdueinfo_insertpos");
                    line = NewOverdueLine(insertpos.Parent, i, insertpos);
                }
                line.Visible = true;

                LiteralControl left = (LiteralControl)line.FindControl("overdueinfo_line" + Convert.ToString(i) + "left");


                XmlNode node = nodes[i];

                string strBarcode = DomUtil.GetAttr(node, "barcode");
                string strItemRecPath = DomUtil.GetAttr(node, "recPath");

                string strReason = DomUtil.GetAttr(node, "reason");
                string strBorrowDate = DomUtil.GetAttr(node, "borrowDate");
                string strPeriod = DomUtil.GetAttr(node, "borrowPeriod");
                string strReturnDate = DomUtil.GetAttr(node, "returnDate");

                string strClass = " class='roundcontentdark' ";

                if ((i % 2) == 1)
                    strClass = " class='roundcontentlight' ";

                string strResult = "<tr " + strClass + " nowrap><td nowrap>";

                strResult += "&nbsp;";

                strResult += "<a href='book.aspx?barcode=" + strBarcode + "&itemrecpath=" + strItemRecPath + "'>"
                    + strBarcode + "</a></td>";

                // ���ժҪ
                string strSummary = "";
                string strBiblioRecPath = "";
                LibraryServerResult result = app.GetBiblioSummary(
                    sessioninfo,
                    strBarcode,
                    strItemRecPath,
                    null,
                    out strBiblioRecPath,
                    out strSummary);
                if (result.Value == -1 || result.Value == 0)
                    strSummary = result.ErrorInfo;

                strResult += "<td width='50%'>" + strSummary + "</td>";
                strResult += "<td >" + strReason + "</td>";
                strResult += "<td nowrap>" + ItemConverter.LocalTime(strBorrowDate) + "</td>";
                strResult += "<td nowrap>" + strPeriod + "</td>";
                strResult += "<td>" + strReturnDate + "</td>";
                strResult += "</tr>";

                left.Text = strResult;
            }

            // �Ѷ��������������
            for (int i = nodes.Count; ; i++)
            {

                PlaceHolder line = (PlaceHolder)overdueinfo.FindControl("overdueinfo_line" + Convert.ToString(i));
                if (line == null)
                    break;

                line.Visible = false;
            }

        }


        void RenderBorrow(
            LibraryApplication app,
            SessionInfo sessioninfo,
            XmlDocument dom)
        {
            string strReaderType = DomUtil.GetElementText(dom.DocumentElement,
                "readerType");

            // �������
            string strError = "";
            Calendar calendar = null;
            int nRet = app.GetReaderCalendar(strReaderType, out calendar, out strError);
            if (nRet == -1)
            {
                this.SetBorrowDebugInfo(strError);
                calendar = null;
            }

            // ���ĵĲ�
            PlaceHolder borrowinfo = (PlaceHolder)this.FindControl("borrowinfo");

            // ��ռ���
            this.BorrowBarcodes = new List<string>();

            string strReaderBarcode = DomUtil.GetElementText(dom.DocumentElement,
                "barcode");

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("borrows/borrow");
            this.BorrowLineCount = nodes.Count;
            for (int i = 0; i < nodes.Count; i++)
            {

                PlaceHolder line = (PlaceHolder)borrowinfo.FindControl("borrowinfo_line" + Convert.ToString(i));
                if (line == null)
                {
                    Control insertpos = borrowinfo.FindControl("borrowinfo_insertpos");
                    line = NewBorrowLine(insertpos.Parent, i, insertpos);
                    // this.BorrowLineCount++;
                }
                line.Visible = true;

                LiteralControl left = (LiteralControl)line.FindControl("borrowinfo_line" + Convert.ToString(i) + "left");
                CheckBox checkbox = (CheckBox)line.FindControl("borrowinfo_line" + Convert.ToString(i) + "checkbox");
                LiteralControl right = (LiteralControl)line.FindControl("borrowinfo_line" + Convert.ToString(i) + "right");


                XmlNode node = nodes[i];

                string strBarcode = DomUtil.GetAttr(node, "barcode");

                // ��ӵ�����
                this.BorrowBarcodes.Add(strBarcode);

                string strNo = DomUtil.GetAttr(node, "no");
                string strBorrowDate = DomUtil.GetAttr(node, "borrowDate");
                string strPeriod = DomUtil.GetAttr(node, "borrowPeriod");
                string strOperator = DomUtil.GetAttr(node, "operator");
                string strRenewComment = DomUtil.GetAttr(node, "renewComment");

                string strColor = "bgcolor=#ffffff";

                string strOverDue = "";

                // string strError = "";
                // ��鳬�������
                // return:
                //      -1  ���ݸ�ʽ����
                //      0   û�з��ֳ���
                //      1   ���ֳ���   strError������ʾ��Ϣ
                //      2   �Ѿ��ڿ������ڣ������׳��� 2009/3/13
                nRet = app.CheckPeriod(
                    calendar, 
                    strBorrowDate,
                    strPeriod,
                    out strError);
                if (nRet == -1)
                    strOverDue = strError;
                else
                {
                   strOverDue = strError;	// ��������ʲô�������ʾ����
                }

                string strResult = "";

                string strClass = " class='roundcontentdark' ";

                if ((i % 2) == 1)
                    strClass = " class='roundcontentlight' ";

                strResult += "<tr " + strClass + strColor + "  nowrap><td nowrap>";
                // ��
                left.Text = strResult;

                // checkbox
                // checkbox.Text = Convert.ToString(i + 1);

                // �ҿ�ʼ
                strResult = "&nbsp;";

                strResult += "<a href='book.aspx?barcode=" + strBarcode + "&borrower=" + strReaderBarcode + "'>"
                    + strBarcode + "</a></td>";

                // ���ժҪ
                string strSummary = "";
                string strBiblioRecPath = "";
                LibraryServerResult result = app.GetBiblioSummary(
                    sessioninfo,
                    strBarcode,
                    null,
                    null,
                    out strBiblioRecPath,
                    out strSummary);
                if (result.Value == -1 || result.Value == 0)
                    strSummary = result.ErrorInfo;

                strResult += "<td width='50%'>" + strSummary + "</td>";
                strResult += "<td nowrap align='right'>" + strNo + "</td>";
                strResult += "<td nowrap>" + ItemConverter.LocalTime(strBorrowDate) + "</td>";
                strResult += "<td nowrap>" + strPeriod + "</td>";
                strResult += "<td nowrap>" + strOperator + "</td>";
                strResult += "<td>" + strOverDue + "</td>";
                strResult += "<td>" + strRenewComment.Replace(";", "<br/>") +

    "</td>";
                strResult += "</tr>";

                right.Text = strResult;

            }

            // �Ѷ��������������
            for (int i = nodes.Count; ; i++)
            {

                PlaceHolder line = (PlaceHolder)borrowinfo.FindControl("borrowinfo_line" + Convert.ToString(i));
                if (line == null)
                    break;

                line.Visible = false;
            }

        }

        void RenderReservation(LibraryApplication app,
            SessionInfo sessioninfo,
            XmlDocument dom)
        {
            // ԤԼ����
            PlaceHolder reservation = (PlaceHolder)this.FindControl("reservation");
            this.ReservationBarcodes = new List<string>();

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("reservations/request");
            this.ReservationLineCount = nodes.Count;
            for (int i = 0; i < nodes.Count; i++)
            {
                PlaceHolder line = (PlaceHolder)reservation.FindControl("reservation_line" + Convert.ToString(i));
                if (line == null)
                {
                    Control insertpos = reservation.FindControl("reservation_insertpos");
                    line = NewReservationLine(insertpos.Parent, i, insertpos);
                    //this.ReservationLineCount++;
                }
                line.Visible = true;

                LiteralControl left = (LiteralControl)line.FindControl("reservation_line" + Convert.ToString(i) + "left");
                CheckBox checkbox = (CheckBox)line.FindControl("reservation_line" + Convert.ToString(i) + "checkbox");
                LiteralControl right = (LiteralControl)line.FindControl("reservation_line" + Convert.ToString(i) + "right");


                XmlNode node = nodes[i];
                string strBarcodes = DomUtil.GetAttr(node, "items");

                this.ReservationBarcodes.Add(strBarcodes);

                string strRequestDate = ItemConverter.LocalTime(DomUtil.GetAttr(node, "requestDate"));

                string strResult = "";

                string strClass = " class='roundcontentdark' ";

                if ((i % 2) == 1)
                    strClass = " class='roundcontentlight' ";


                strResult += "<tr " + strClass + "><td nowrap>";

                // ��
                left.Text = strResult;

                // �ҿ�ʼ
                strResult = "&nbsp;";

                //strResult += "" + strBarcodes + "</td>";

                strResult += "" + MakeBarcodeListHyperLink(strBarcodes, ",") + "</td>";

                // ������
                string strOperator = DomUtil.GetAttr(node, "operator");
                // ״̬
                string strArrivedDate = DomUtil.GetAttr(node, "arrivedDate");
                string strState = DomUtil.GetAttr(node, "state");
                // 2007/1/18
                string strArrivedItemBarcode = DomUtil.GetAttr(node, "arrivedItemBarcode");
                if (strState == "arrived")
                {
                    strArrivedDate = ItemConverter.LocalTime(strArrivedDate);
                    strState = "�� "+strArrivedItemBarcode+" ���� " + strArrivedDate + " ����";
                }
                strResult += "<td>" + strState + "</td>";

                string strSummary = app.GetBarcodesSummary(
                    sessioninfo,
                    strBarcodes,
                    "html",
                    "");

                /*
                string strSummary = "";

                string strPrevBiblioRecPath = "";
                string[] barcodes = strBarcodes.Split(new char[] {','});
                for (int j = 0; j < barcodes.Length; j++)
                {
                    string strBarcode = barcodes[j];
                    if (String.IsNullOrEmpty(strBarcode) == true)
                        continue;

                    // ���ժҪ
                    string strOneSummary = "";
                    string strBiblioRecPath = "";

                    Result result = app.GetBiblioSummary(sessioninfo,
        strBarcode,
        strPrevBiblioRecPath,   // ǰһ��path
        out strBiblioRecPath,
        out strOneSummary);
                    if (result.Value == -1 || result.Value == 0)
                        strOneSummary = result.ErrorInfo;

                    if (strOneSummary == "" 
                        && strPrevBiblioRecPath == strBiblioRecPath)
                        strOneSummary = "(ͬ��)";

                    strSummary += strBarcode + " : " + strOneSummary + "<br/>";

                    strPrevBiblioRecPath = strBiblioRecPath;
                }
                 */


                strResult += "<td width='50%'>" + strSummary + "</td>";
                strResult += "<td nowrap>" + strRequestDate + "</td>";
                strResult += "<td nowrap>" + strOperator + "</td>";
                strResult += "</tr>";

                right.Text = strResult;

            }

            // �Ѷ��������������
            for (int i = nodes.Count; ; i++)
            {
                PlaceHolder line = (PlaceHolder)reservation.FindControl("reservation_line" + Convert.ToString(i));
                if (line == null)
                    break;

                line.Visible = false;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            string strError = "";
            LibraryApplication app = (LibraryApplication)this.Page.Application["app"];
            if (app == null)
            {
                strError = "app == null";
                goto ERROR1;
            }
            SessionInfo sessioninfo = (SessionInfo)this.Page.Session["sessioninfo"];
            if (sessioninfo == null)
            {
                strError = "sessioninfo == null";
                goto ERROR1;
            }

            if (sessioninfo.Account == null)
            {
                // output.Write("��δ��¼");
                sessioninfo.LoginCallStack.Push(this.Page.Request.RawUrl);
                this.Page.Response.Redirect("login.aspx", true);
                return;
            }

            string strBarcode = "";
            if (this.Barcode == "")
                strBarcode = sessioninfo.Account.Barcode;
            else
                strBarcode = this.Barcode;

            if (strBarcode == "")
            {
                strError = "����֤�����Ϊ�գ��޷���λ���߼�¼��";
                goto ERROR1;
            }

            string strXml = "";
            string strOutputPath = "";
            // ��ö��߼�¼
            int nRet = app.GetReaderRecXml(
                sessioninfo.Channels,
                strBarcode,
                out strXml,
                out strOutputPath,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            if (nRet == 0)
                goto ERROR1;

            XmlDocument dom = new XmlDocument();

            try
            {
                dom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                strError = "װ�ض���XML��¼����DOMʱ����: " + ex.Message;
                goto ERROR1;
            }

            // ����ʶ����Ϣ
            RenderIdentity(dom);

            // ����һ����Ϣ
            RenderGeneralInfo(dom);

            // ���� ΥԼ��Ϣ
            RenderOverdue(app,
                sessioninfo,
                dom);

            // ���� ������Ϣ
            RenderBorrow(app,
                sessioninfo,
                dom);

            // ���� ԤԼ����
            RenderReservation(app,
                sessioninfo,
                dom);



            base.Render(output);
            return;

        ERROR1:
            output.Write(strError);
        }




        static string MakeBarcodeListHyperLink(string strBarcodes,
            string strSep)
        {
            string strResult = "";
            string[] barcodes = strBarcodes.Split(new char[] { ',' });
            for (int i = 0; i < barcodes.Length; i++)
            {
                string strBarcode = barcodes[i];
                if (String.IsNullOrEmpty(strBarcode) == true)
                    continue;

                if (strResult != "")
                    strResult += strSep;
                strResult += "<a href='book.aspx?barcode=" + strBarcode + "&forcelogin=on'>"
                    + strBarcode + "</a>";
            }

            return strResult;
        }

        /*
        protected override void RenderContents(HtmlTextWriter output)
        {
            // output.Write(Text);

            string strError = "";
            string strResult = "";
            int nRet = GetReaderHtml(
                this.App,
                this.SessionInfo,
                this.Barcode,
                out strResult,
                out strError);
            if (nRet == -1 || nRet == 0)
            {
                output.Write(strError);
                return;
            }

            output.Write(strResult);
        }

        static int GetReaderHtml(
            CirculationApplication app,
            SessionInfo sessioninfo,
            string strBarcode,
            out string strResult,
            out string strError)
        {
            strResult = "";
            strError = "";

            string strXml = "";
            string strOutputPath = "";

            int nRet = app.GetReaderXml(
                sessioninfo,
                strBarcode,
                out strXml,
                out strOutputPath,
                out strError);
            if (nRet == 0)
            {
                strError = "����Ϊ '"+strBarcode+"' �Ķ��߼�¼û���ҵ�";
                return 0;
            }
            if (nRet == -1)
                goto ERROR1;

            // �����߼�¼���ݴ�XML��ʽת��ΪHTML��ʽ
            nRet = app.ConvertReaderXmlToHtml(
                app.CfgDir + "\\readeropac.cs",
                app.CfgDir + "\\readeropac.cs.ref",
                strXml,
                OperType.None,
                null,
                "",
                out strResult,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            return 1;
        ERROR1:
            return -1;
        }
         */

        public override void RenderBeginTag(HtmlTextWriter writer)
        {

        }
        public override void RenderEndTag(HtmlTextWriter writer)
        {

        }
    }
#endif
}
