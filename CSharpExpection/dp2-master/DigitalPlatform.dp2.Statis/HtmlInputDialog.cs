using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web;

using System.Collections.Specialized;

// using mshtml;

namespace DigitalPlatform.dp2.Statis
{
    /// <summary>
    /// �����ռ��û���������ĶԻ��򡣲���Html�ļ�����������棬��PostData����û��������
    /// </summary>
    public partial class HtmlInputDialog : Form
    {
        string m_strUrl = "";
        int m_nNavigating = 0;

        public string SubmitUrl = "";
        public NameValueCollection SubmitResult = new NameValueCollection();


        public HtmlInputDialog()
        {
            InitializeComponent();
        }

        private void extendedWebBrowser1_BeforeNavigate(object sender, BeforeNavigateArgs e)
        {
            if (m_nNavigating > 0)
                return;

            SubmitResult.Clear();

            // ׼��SubmitResult��Ϣ
            // string strEncoding = ((mshtml.HTMLDocumentClass)this.extendedWebBrowser1.Document).charset;


            string strEncoding = this.extendedWebBrowser1.Document.Encoding;

            GetFormData(e.postData, strEncoding);


            // MessageBox.Show(this, "url=[" +url+ "]");

            /*
            // Ԥ���������
            if (String.Compare(url, "action://print/", true) == 0)
            {
                this.Print();
                Cancel = true;
                return;
            }*/

            SubmitUrl = e.url;
            e.Cancel = true;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        void GetFormData(byte[] data,
            string strEncoding)
        {
            if (data == null)
                return; // 2011/4/18

            Encoding encoding = null;

            try
            {
                encoding = Encoding.GetEncoding(strEncoding);
            }
            catch (NotSupportedException)
            {
                encoding = Encoding.ASCII;
            }

            byte[] data1 = null;

            // ȥ��ĩβ��0
            if (data.Length > 1 && data[data.Length - 1] == 0)
            {
                data1 = new byte[data.Length - 1];
                Array.Copy(data, 0, data1, 0, data.Length - 1);
            }
            else
                data1 = data;

            string strData = encoding.GetString(data1);

            // �и� &
            string[] saItem = strData.Split(new Char[] { '&' });

            for (int i = 0; i < saItem.Length; i++)
            {
                // �и�'='
                int nRet = saItem[i].IndexOf('=', 0);
                if (nRet == -1)
                    continue;	// invalid item
                string strName = saItem[i].Substring(0, nRet);
                string strValue = saItem[i].Substring(nRet + 1);

                // ����
                strName = HttpUtility.UrlDecode(strName,
                    encoding);
                strValue = HttpUtility.UrlDecode(strValue,
                    encoding);

                SubmitResult.Add(strName, strValue);
            }

        }

        // Url����
        public string Url
        {
            get
            {
                return m_strUrl;
            }
            set
            {
                m_strUrl = value;

                // eventDocumentComplete.Reset();
                if (this.Visible == true)   // 2011/4/18 (IE 9)
                {
                    this.m_nNavigating++;
                    this.extendedWebBrowser1.Navigate(value);
                    this.m_nNavigating--;
                }

            }
        }

        private void HtmlInputDialog_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(m_strUrl) == false)
            {
                // eventDocumentComplete.Reset();

                this.m_nNavigating++;
                this.extendedWebBrowser1.Navigate(m_strUrl);
                this.m_nNavigating--;

            }
        }
    }
}