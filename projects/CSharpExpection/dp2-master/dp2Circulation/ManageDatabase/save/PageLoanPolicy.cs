using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Web;

using DigitalPlatform;
using DigitalPlatform.GUI;
using DigitalPlatform.CirculationClient;
using DigitalPlatform.Xml;
using DigitalPlatform.IO;
using DigitalPlatform.Text;

using DigitalPlatform.CirculationClient.localhost;

namespace dp2Circulation
{
    /// <summary>
    /// ����Ȩ�� ����ҳ
    /// </summary>
    public partial class ManagerForm
    {
        int m_nRightsTableXmlVersion = 0;
        int m_nRightsTableHtmlVersion = 0;
        int m_nRightsTableTypesVersion = 0;

#if NO
        // types�༭���� --> DOM�е�<readerTypes>��<bookTypes>����
        // ����ǰdom��Ӧ���Ѿ�װ����Ȩ��XML����
        void TypesToRightsXml(ref XmlDocument dom)
        {
            string strReaderTypesXml = BuildTypesXml(this.textBox_loanPolicy_readerTypes);
            string strBookTypesXml = BuildTypesXml(this.textBox_loanPolicy_bookTypes);

            {
                XmlNode nodeReaderTypes = dom.DocumentElement.SelectSingleNode("readerTypes");
                if (nodeReaderTypes == null)
                {
                    nodeReaderTypes = dom.CreateElement("readerTypes");
                    dom.DocumentElement.AppendChild(nodeReaderTypes);
                }

                nodeReaderTypes.InnerXml = strReaderTypesXml;
            }

            {
                XmlNode nodeBookTypes = dom.DocumentElement.SelectSingleNode("bookTypes");
                if (nodeBookTypes == null)
                {
                    nodeBookTypes = dom.CreateElement("bookTypes");
                    dom.DocumentElement.AppendChild(nodeBookTypes);
                }

                nodeBookTypes.InnerXml = strBookTypesXml;
            }
        }
#endif

        /*public*/ void FinishLibraryCodeTextbox()
        {
                            // �ѵ�ǰtextbox�е���β
                if (m_currentLibraryCodeItem != null)
                {
                    LibraryCodeInfo info = (LibraryCodeInfo)m_currentLibraryCodeItem.Tag;
                    if (info == null)
                    {
                        info = new LibraryCodeInfo();
                        m_currentLibraryCodeItem.Tag = info;
                    }

                    if (info.BookTypeList != this.textBox_loanPolicy_bookTypes.Text)
                    {
                        info.BookTypeList = this.textBox_loanPolicy_bookTypes.Text;
                        info.Changed = true;
                    }
                    if (info.ReaderTypeList != this.textBox_loanPolicy_readerTypes.Text)
                    {
                        info.ReaderTypeList = this.textBox_loanPolicy_readerTypes.Text;
                        info.Changed = true;
                    }
                }

        }

        // types�༭���� --> DOM�е�<readerTypes>��<bookTypes>����
        // ����ǰdom��Ӧ���Ѿ�װ����Ȩ��XML����
        bool TypesToRightsXml(ref XmlDocument dom)
        {
            // ����һ�µ�ǰtextbox
            FinishLibraryCodeTextbox();

            bool bChanged = false;

            XmlNode root = dom.DocumentElement;

            // ��ÿ���ݴ���ѭ��
            foreach (ListViewItem item in this.listView_loanPolicy_libraryCodes.Items)
            {
                // string strLibraryCode = item.Text;
                LibraryCodeInfo info = (LibraryCodeInfo)item.Tag;

                /*
                if (info.Changed == false)
                    continue;
                 * */
                string strLibraryCode = info.LibraryCode;
                string strReaderTypesXml = BuildTypesXml(info.ReaderTypeList);
                string strBookTypesXml = BuildTypesXml(info.BookTypeList);

                string strFilter = "";
                if (string.IsNullOrEmpty(strLibraryCode) == false)
                {
                    XmlNode temp = root.SelectSingleNode("//library[@code='" + strLibraryCode + "']");
                    if (temp == null)
                    {
                        temp = dom.CreateElement("library");
                        root.AppendChild(temp);
                        DomUtil.SetAttr(temp, "code", strLibraryCode);
                    }
                    root = temp;
                }
                else
                {
                    strFilter = "[count(ancestor::library) = 0]";
                }

                {
                XmlNode nodeReaderTypes = root.SelectSingleNode("descendant::readerTypes" + strFilter);

                    if (nodeReaderTypes == null)
                    {
                        nodeReaderTypes = dom.CreateElement("readerTypes");
                        root.AppendChild(nodeReaderTypes);
                    }

                    nodeReaderTypes.InnerXml = strReaderTypesXml;
                }

                {
                    XmlNode nodeBookTypes = root.SelectSingleNode("descendant::bookTypes" + strFilter);


                    if (nodeBookTypes == null)
                    {
                        nodeBookTypes = dom.CreateElement("bookTypes");
                        root.AppendChild(nodeBookTypes);
                    }

                    nodeBookTypes.InnerXml = strBookTypesXml;
                }

                if (info.Changed == true)
                    bChanged = true;

                info.Changed = false;
            }

            return bChanged;
        }

#if NO
        // DOM�е�<readerTypes>��<bookTypes>���� --> types�༭����
        void RightsXmlToTypes(XmlDocument dom,
            string strLibraryCode)
        {
            string strFilter = "";

            XmlNode root = dom.DocumentElement;
            if (string.IsNullOrEmpty(strLibraryCode) == false)
            {
                XmlNode temp = root.SelectSingleNode("//library[@code='" + strLibraryCode + "']");
                if (temp == null)
                    return;
                root = temp;
            }
            else
            {
                strFilter = "[count(ancestor::library) = 0]";
            }


            // readertypes
            
            {
                XmlNodeList nodes = root.SelectNodes("descendant::readerTypes/item" + strFilter);
                string strText = "";
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (String.IsNullOrEmpty(strText) == false)
                        strText += "\r\n";
                    strText += nodes[i].InnerText;
                }

                if (this.textBox_loanPolicy_readerTypes.Text != strText)
                {
                    //this.textBox_loanPolicy_readerTypes.Text = strText;
                    SetText(this.textBox_loanPolicy_readerTypes,
                        strText);
                }
            }

            // booktypes
            {
                XmlNodeList nodes = root.SelectNodes("descendant::bookTypes/item" + strFilter);
                string strText = "";
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (String.IsNullOrEmpty(strText) == false)
                        strText += "\r\n";
                    strText += nodes[i].InnerText;
                }
                if (this.textBox_loanPolicy_bookTypes.Text != strText)
                {
                    // this.textBox_loanPolicy_bookTypes.Text = strText;
                    SetText(this.textBox_loanPolicy_bookTypes,
                        strText);
                }
            }
        }
#endif

        // DOM�е�<readerTypes>��<bookTypes>���� --> types�༭����
        LibraryCodeInfo GetLibraryCodeInfo(XmlDocument dom,
            string strLibraryCode)
        {
            LibraryCodeInfo info = new LibraryCodeInfo();

            info.LibraryCode = strLibraryCode;

            string strFilter = "";
            XmlNode root = dom.DocumentElement;
            if (string.IsNullOrEmpty(strLibraryCode) == false)
            {
                XmlNode temp = root.SelectSingleNode("//library[@code='" + strLibraryCode + "']");
                if (temp == null)
                    return info;
                root = temp;
            }
            else
            {
                strFilter = "[count(ancestor::library) = 0]";
            }


            // readertypes

            {
                XmlNodeList nodes = root.SelectNodes("descendant::readerTypes/item" + strFilter);
                string strText = "";
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (String.IsNullOrEmpty(strText) == false)
                        strText += "\r\n";
                    strText += nodes[i].InnerText;
                }

                info.ReaderTypeList = strText;
            }

            // booktypes
            {
                XmlNodeList nodes = root.SelectNodes("descendant::bookTypes/item" + strFilter);
                string strText = "";
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (String.IsNullOrEmpty(strText) == false)
                        strText += "\r\n";
                    strText += nodes[i].InnerText;
                }

                info.BookTypeList = strText;
            }

            return info;
        }


        // ��listview���г��ݴ���
        void ListLibraryCodes(XmlDocument dom)
        {
            List<string> librarycodes = new List<string>();
            XmlNodeList nodes = dom.DocumentElement.SelectNodes("library");
            foreach (XmlNode node in nodes)
            {
                string strCode = DomUtil.GetAttr(node, "code");
                librarycodes.Add(strCode);
            }

            // �����Ƿ��в������κ�<library>Ԫ�ص�
            nodes = dom.DocumentElement.SelectNodes("//param[count(ancestor::library) = 0]");
            if (nodes.Count > 0)
            {
                if (librarycodes.IndexOf("") == -1)
                    librarycodes.Insert(0, "");
            }

            // 
            this.m_currentLibraryCodeItem = null;
            this.listView_loanPolicy_libraryCodes.Items.Clear();
            foreach (string strLibraryCode in librarycodes)
            {
                ListViewItem item = new ListViewItem(string.IsNullOrEmpty(strLibraryCode) == true ? "<ȱʡ>" : strLibraryCode);
                LibraryCodeInfo info = GetLibraryCodeInfo(dom,
                    strLibraryCode);
                item.Tag = info;

                this.listView_loanPolicy_libraryCodes.Items.Add(item);
            }

            this.textBox_loanPolicy_readerTypes.Text = "";
            this.textBox_loanPolicy_bookTypes.Text = "";

            // ѡ����һ��
            if (this.listView_loanPolicy_libraryCodes.Items.Count > 0)
                this.listView_loanPolicy_libraryCodes.Items[0].Selected = true;
        }

        void SetRightsTableHtml(string strRightsTableHtml)
        {
            Global.SetHtmlString(this.webBrowser_rightsTableHtml,
    "<html><body style='font-family:\"Microsoft YaHei\", Times, serif;'>" + strRightsTableHtml + "</body></html>");
        }

        int ListRightsTables(out string strError)
        {
            strError = "";

            if (this.LoanPolicyDefChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�����ڶ�����ͨȨ�޶��屻�޸ĺ���δ���档����ʱˢ�´������ݣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪˢ��? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return 0;
                }
            }

            /*
            // 2008/10/12 new add
            if (this.LoanPolicyDefChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������ж�����ͨȨ�޶��屻�޸ĺ���δ���档����ʱ����װ�ض�����ͨȨ�޶��壬����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ����װ��? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return 0;
            }*/

            string strRightsTableXml = "";
            string strRightsTableHtml = "";
            //string strReaderTypesXml = "";
            //string strBookTypesXml = "";

            // �����ͨ����Ȩ����ض���
            int nRet = GetRightsTableInfo(out strRightsTableXml,
                out strRightsTableHtml,
                //out strReaderTypesXml,
                //out strBookTypesXml,
                out strError);
            if (nRet == -1)
                return -1;

            strRightsTableXml = "<rightsTable>" + strRightsTableXml + "</rightsTable>";

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strRightsTableXml);
            }
            catch (Exception ex)
            {
                strError = "strRightsTableXmlװ��XMLDOMʱ��������" + ex.Message;
                return -1;
            }

            /*
            // readertypes
            this.textBox_loanPolicy_readerTypes.Text = "";
            {
                XmlNodeList nodes = dom.DocumentElement.SelectNodes("readerTypes/item");
                string strText = "";
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (String.IsNullOrEmpty(strText) == false)
                        strText += "\r\n";
                    strText += nodes[i].InnerText;
                }
                this.textBox_loanPolicy_readerTypes.Text = strText;
            }

            // booktypes
            this.textBox_loanPolicy_bookTypes.Text = "";
            {
                XmlNodeList nodes = dom.DocumentElement.SelectNodes("bookTypes/item");
                string strText = "";
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (String.IsNullOrEmpty(strText) == false)
                        strText += "\r\n";
                    strText += nodes[i].InnerText;
                }
                this.textBox_loanPolicy_bookTypes.Text = strText;
            }
             * */

            // ��listview���г��ݴ���
            ListLibraryCodes(dom);

            // RightsXmlToTypes(dom);

            /*
            // TODO: Ϊ����XMLԴ���벻�����������(��Ҫȥ�༭<readerTypes>��<bookTypes>)���Ƿ�Ҫ��������Ԫ��ȥ��?
            {
                XmlNodeList nodes = dom.DocumentElement.SelectNodes("readerTypes | bookTypes");
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];
                    node.ParentNode.RemoveChild(node);
                }
            }*/

            string strXml = "";
            nRet = DomUtil.GetIndentXml(dom.DocumentElement.OuterXml,
                out strXml,
                out strError);
            if (nRet == -1)
                return -1;

            this.textBox_loanPolicy_rightsTableDef.Text = strXml;
            SetRightsTableHtml(strRightsTableHtml);


            this.LoanPolicyDefChanged = false;

            this.m_nRightsTableHtmlVersion = 0;
            this.m_nRightsTableXmlVersion = 0;
            this.m_nRightsTableTypesVersion = 0;

            return 1;
        }

        // �����ͨ����Ȩ����ض���
        int GetRightsTableInfo(out string strRightsTableXml,
            out string strRightsTableHtml,
            //out string strReaderTypesXml,
            //out string strBookTypesXml,
            out string strError)
        {
            strError = "";
            strRightsTableXml = "";
            strRightsTableHtml = "";
            //strReaderTypesXml = "";
            //strBookTypesXml = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡ������ͨȨ�޶��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "rightsTable",
                    out strRightsTableXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "rightsTableHtml",
                    out strRightsTableHtml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                /*
                lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "readerTypes",
                    out strReaderTypesXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "bookTypes",
                    out strBookTypesXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                 * */

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ������ͨ����Ȩ����ض���
        // parameters:
        //      strRightsTableXml   ��ͨ����Ȩ�޶���XML��ע�⣬û�и�Ԫ��
        int SetRightsTableDef(string strRightsTableXml,
            //string strReaderTypesXml,
            //string strBookTypesXml,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڱ��������ͨȨ�޶��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "rightsTable",
                    strRightsTableXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                /*
                lRet = Channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "readerTypes",
                    strReaderTypesXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                lRet = Channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "bookTypes",
                    strBookTypesXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                 * */

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        static void SetText(TextBox textbox, string strText)
        {
            int nStart = 0;
            int nLength = 0;

            nStart = textbox.SelectionStart;
            nLength = textbox.SelectionLength;

            textbox.Text = strText;

            textbox.SelectionStart = nStart;
            textbox.SelectionLength = nLength;
        }

        void SynchronizeLoanPolicy()
        {
            SynchronizeRightsTableAndTypes();
            SynchronizeRightsTableAndHtml();
        }

        // ͬ������Ȩ��XML����Ͷ���/ͼ�����ͱ༭����
        int SynchronizeRightsTableAndTypes()
        {
            string strError = "";

            if (this.m_nRightsTableXmlVersion == this.m_nRightsTableTypesVersion)
                return 0;

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(this.textBox_loanPolicy_rightsTableDef.Text);
            }
            catch (Exception ex)
            {
                strError = "����Ȩ��XML�����ʽ����: " + ex.Message;
                goto ERROR1;
            }


            // XML�������
            if (this.m_nRightsTableXmlVersion > this.m_nRightsTableTypesVersion)
            {
                // RightsXmlToTypes(dom);
                ListLibraryCodes(dom);
                this.m_nRightsTableTypesVersion = this.m_nRightsTableXmlVersion;
                return 0;
            }

            // types�༭�������
            if (this.m_nRightsTableXmlVersion < this.m_nRightsTableTypesVersion)
            {
                // types�༭���� --> DOM�е�<readerTypes>��<bookTypes>����
                // ����ǰdom��Ӧ���Ѿ�װ����Ȩ��XML����
                TypesToRightsXml(ref dom);

                // ˢ��XML�ı���
                string strXml = "";
                int nRet = DomUtil.GetIndentXml(dom.DocumentElement.OuterXml,
                    out strXml,
                    out strError);
                if (nRet == -1)
                    return -1;

                if (this.textBox_loanPolicy_rightsTableDef.Text != strXml)
                {
                    // this.textBox_loanPolicy_rightsTableDef.Text = strXml;
                    SetText(this.textBox_loanPolicy_rightsTableDef,
                        strXml);
                }

                this.m_nRightsTableXmlVersion = this.m_nRightsTableTypesVersion;
                return 0;
            }

            return 0;
        ERROR1:
            MessageBox.Show(this, strError);
            return -1;
        }

        // ͬ������Ȩ��XML�������ͨȨ�ޱ�HTML��ʾ
        int SynchronizeRightsTableAndHtml()
        {
            string strError = "";

            if (this.m_nRightsTableXmlVersion == this.m_nRightsTableHtmlVersion)
                return 0;


            string strRightsTableXml = this.textBox_loanPolicy_rightsTableDef.Text;
            string strRightsTableHtml = "";

            if (String.IsNullOrEmpty(strRightsTableXml) == true)
            {
                Global.ClearHtmlPage(this.webBrowser_rightsTableHtml,
                    this.MainForm.DataDir);
                this.m_nRightsTableHtmlVersion = this.m_nRightsTableXmlVersion;
                return 0;
            }

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strRightsTableXml);
            }
            catch (Exception ex)
            {
                strError = "XML�ַ���װ��XMLDOMʱ��������: " + ex.Message;
                goto ERROR1;
            }

            if (dom.DocumentElement == null)
            {
                Global.ClearHtmlPage(this.webBrowser_rightsTableHtml,
                    this.MainForm.DataDir);
                this.m_nRightsTableHtmlVersion = this.m_nRightsTableXmlVersion;
                return 0;
            }

#if NO
            // �ڷ���ȥ��XML������<readerTypes>��<bookTypes>����
            string strReaderTypesXml = BuildTypesXml(this.textBox_loanPolicy_readerTypes);
            string strBookTypesXml = BuildTypesXml(this.textBox_loanPolicy_bookTypes);

            XmlNode node_readertypes = dom.CreateElement("readerTypes");
            XmlNode node_booktypes = dom.CreateElement("bookTypes");
            dom.DocumentElement.AppendChild(node_readertypes);
            dom.DocumentElement.AppendChild(node_booktypes);
            if (String.IsNullOrEmpty(strReaderTypesXml) == false)
                node_readertypes.InnerXml = strReaderTypesXml;
            if (String.IsNullOrEmpty(strBookTypesXml) == false)
                node_booktypes.InnerXml = strBookTypesXml;

#endif
            // ��ΪSynchronizeRightsTableAndHtml() �������Ժ���ã����Բ��ò���types xml���µ�����

            strRightsTableXml = dom.DocumentElement.InnerXml;

            // EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡ������ͨȨ�޶���HTML ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "instance_rightstable_html",
                    strRightsTableXml,
                    out strRightsTableHtml,
                    out strError);
                if (lRet == -1)
                {
                    goto ERROR1;
                }

                SetRightsTableHtml(strRightsTableHtml);
                this.m_nRightsTableHtmlVersion = this.m_nRightsTableXmlVersion;

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                // EnableControls(true);
            }

        ERROR1:
            Global.SetHtmlString(this.webBrowser_rightsTableHtml,
                HttpUtility.HtmlEncode(strError));
            return -1;
        }


        bool m_bLoanPolicyDefChanged = false;

        /// <summary>
        /// ������ͨȨ�޶����Ƿ��޸�
        /// </summary>
        public bool LoanPolicyDefChanged
        {
            get
            {
                return this.m_bLoanPolicyDefChanged;
            }
            set
            {
                this.m_bLoanPolicyDefChanged = value;
                if (value == true)
                    this.toolStripButton_loanPolicy_save.Enabled = true;
                else
                    this.toolStripButton_loanPolicy_save.Enabled = false;
            }
        }

        // ��<rightsTable>��Ȩ�޶��������(�����Ǵ�<readerTypes>��<bookTypes>Ԫ����)��ö��ߺ�ͼ�������б�
        int GetReaderAndBookTypes(out List<string> readertypes,
            out List<string> booktypes,
            out string strError)
        {
            strError = "";
            booktypes = new List<string>();
            readertypes = new List<string>();

            string strRightsTableXml = this.textBox_loanPolicy_rightsTableDef.Text;

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strRightsTableXml);
            }
            catch (Exception ex)
            {
                strError = "����Ȩ��XML����װ��DOMʱ����: " + ex.Message;
                return -1;
            }

            // ѡ������<type>Ԫ��
            XmlNodeList nodes = dom.DocumentElement.SelectNodes("//type");

            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];
                string strReaderType = DomUtil.GetAttr(node, "reader");
                string strBookType = DomUtil.GetAttr(node, "book");

                if (String.IsNullOrEmpty(strReaderType) == false
                    && strReaderType != "*")
                {
                    readertypes.Add(strReaderType);
                    continue;
                }

                if (String.IsNullOrEmpty(strBookType) == false
                    && strBookType != "*")
                {
                    booktypes.Add(strBookType);
                    continue;
                }
            }

            StringUtil.RemoveDupNoSort(ref readertypes);

            StringUtil.RemoveDupNoSort(ref booktypes);

            return 0;
        }

        static List<string> MakeStringList(string strLines)
        {
            strLines = strLines.Replace("\r\n", "\r");
            string[] lines = strLines.Split(new char[] {'\r'});
            List<string> results = new List<string>();
            results.AddRange(lines);

            return results;
        }

        // �����ı��д���types XML Ƭ��
        static string BuildTypesXml(string strText)
        {
            if (String.IsNullOrEmpty(strText) == true)
                return "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<types/>");

            strText = strText.Replace("\r\n", "\n");
            string[] lines = strText.Split(new char[] {'\n'});
            foreach(string s in lines)
            {
                string strLine = s.Trim();
                if (String.IsNullOrEmpty(strLine) == true)
                    continue;

                XmlNode node = dom.CreateElement("item");
                dom.DocumentElement.AppendChild(node);
                node.InnerText = strLine;
            }

            return dom.DocumentElement.InnerXml;
        }

#if NO
        static string BuildTypesXml(TextBox textbox)
        {
            if (String.IsNullOrEmpty(textbox.Text) == true)
                return "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<types/>");

            for (int i = 0; i < textbox.Lines.Length; i++)
            {
                string strLine = textbox.Lines[i].Trim();
                if (String.IsNullOrEmpty(strLine) == true)
                    continue;

                XmlNode node = dom.CreateElement("item");
                dom.DocumentElement.AppendChild(node);
                node.InnerText = strLine;
            }

            return dom.DocumentElement.InnerXml;
        }
#endif
    }

    internal class LibraryCodeInfo
    {
        /// <summary>
        /// ͼ��ݴ���
        /// </summary>
        public string LibraryCode = "";

        /// <summary>
        /// ���������б�
        /// </summary>
        public string ReaderTypeList = "";

        /// <summary>
        /// ͼ�������б�
        /// </summary>
        public string BookTypeList = "";

        /// <summary>
        /// �����Ƿ������޸�
        /// </summary>
        public bool Changed = false;    // readertype �� booktypes ���ݷ����˱仯
    }
}
