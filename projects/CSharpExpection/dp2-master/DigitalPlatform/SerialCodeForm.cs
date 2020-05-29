using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
// using System.Management;
using System.Net.NetworkInformation;
using System.Collections;
using System.Web;
using System.IO;
using System.Threading;

namespace DigitalPlatform
{
    /// <summary>
    /// ��ʾ�û���ȡ���кŵĶԻ���
    /// </summary>
    public partial class SerialCodeForm : Form
    {
        // ��ǰѡ�������������к�
        public string CommunityVersionCode
        {
            get;
            set;
        }

        // Ԥ�õ����кš������ڿ����л�������������кš�ÿ���ַ����ĸ�ʽΪ�����к�|�汾����
        List<string> _defaultCodes = new List<string>();

        public List<string> DefaultCodes
        {
            get
            {
                return _defaultCodes;
            }
            set
            {
                SetDefaultCodes(value);
            }
        }

        public SerialCodeForm()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// ������
        /// </summary>
        public string OriginCode
        {
            get
            {
                return this.textBox_originCode.Text;
            }
            set
            {
                this.textBox_originCode.Text = value;
            }
        }

        /// <summary>
        /// ���к�
        /// </summary>
        public string SerialCode
        {
            get
            {
                return this.textBox_serialCode.Text.Trim().Trim(new char [] {'\r','\n',' '});
            }
            set
            {
                this.textBox_serialCode.Text = value;
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

#if NO
        /// <summary>
        /// Finds the MAC address of the first operation NIC found.
        /// </summary>
        /// <returns>The MAC address.</returns>
        public static string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up
                    || nic.OperationalStatus == OperationalStatus.Down)
                {
                    string strAddress = nic.GetPhysicalAddress().ToString();
                    if (string.IsNullOrEmpty(strAddress) == false)
                    {
                        macAddresses += strAddress;
                        break;
                    }
                }
            }

            return macAddresses;
        }

#endif

        // ����������Ƿ���� wireless ����
        static bool ContainWirelass(string strText)
        {
            if (string.IsNullOrEmpty(strText) == true)
                return false;

            strText = strText.ToLower();

            if (strText.IndexOf("wireless") != -1)
                return true;

            return false;
        }

        public static List<string> GetMacAddress()
        {
            List<string> results = new List<string>();
            List<string> ethernet_results = new List<string>(); // Ethernet ���͵�,�������� wireless 2014/12/26
            List<string> wireless_ethernet_results = new List<string>(); // Ethernet ���͵ģ�wireless �� 2015/1/2

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {
                    string strAddress = nic.GetPhysicalAddress().ToString();
                    if (string.IsNullOrEmpty(strAddress) == false)
                    {
                        if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            if (ContainWirelass(nic.Description) == true)
                                wireless_ethernet_results.Add(strAddress);
                            else
                                ethernet_results.Add(strAddress);
                        }
                        else
                            results.Add(strAddress);
                    }
                }
            }
            // NetworkInterfaceType.Ethernet ������ǰ�棬���� wireless �������Ժ�
            ethernet_results.AddRange(wireless_ethernet_results);
            ethernet_results.AddRange(results);

            return ethernet_results;
        }


        static string ToString(NetworkInterface nic)
        {
            return
                "Name=" + nic.Name
                + "; NetworkInterfaceType=" + nic.NetworkInterfaceType.ToString()
                + "; Id=" + nic.Id
                + "; Description=" + nic.Description
                + "; OperationalStatus=" + nic.OperationalStatus.ToString()
                + "; GetPhysicalAddress()=" + nic.GetPhysicalAddress().ToString()
                + "; Speed=" + nic.Speed.ToString()
                + "; SupportsMulticast=" + nic.SupportsMulticast.ToString()
                // + "; LoopbackInterfaceIndex=" + nic.LoopbackInterfaceIndex.ToString()
                + "; IsReceiveOnly=" + nic.IsReceiveOnly.ToString()
                // + "; GetIsNetworkAvailable=" + nic.GetIsNetworkAvailable().ToString()

                
                ;
        }

        public static string GetNicInformation()
        {
            StringBuilder text = new StringBuilder();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                text.Append(ToString(nic).Replace("; ", "\r\n") + "\r\n***\r\n");
            }

            return text.ToString();
        }

        // �� strSerialCode �е���չ�����趨�� table ��
        public static void SetExtParams(ref Hashtable table,
            string strSerialCode)
        {
            if (string.IsNullOrEmpty(strSerialCode) == true)
                return;

            string strExtParam = GetExtParams(strSerialCode);
            if (string.IsNullOrEmpty(strExtParam) == false)
            {
                Hashtable ext_table = ParseParameters(strExtParam);
                foreach (string key in ext_table.Keys)
                {
                    string value = (string)ext_table[key];
                    if (string.IsNullOrEmpty(value) == false)
                        table[key] = value;
                }
            }
        }

        // ��� xxx|||xxxx ����߲���
        public static string GetCheckCode(string strSerialCode)
        {
            string strSN = "";
            string strExtParam = "";
            ParseTwoPart(strSerialCode,
                "|||",
                out strSN,
                out strExtParam);

            return strSN;
        }

        // ��� xxx|||xxxx ���ұ߲���
        public static string GetExtParams(string strSerialCode)
        {
            string strSN = "";
            string strExtParam = "";
            ParseTwoPart(strSerialCode,
                "|||",
                out strSN,
                out strExtParam);

            return strExtParam;
        }

        public static string GetTimeRange()
        {
            DateTime now = DateTime.Now;
            return now.Year.ToString().PadLeft(4, '0');
        }

        public static string GetNextYearTimeRange()
        {
            DateTime now = DateTime.Now;
            return (now.Year+1).ToString().PadLeft(4, '0');
        }

        /// <summary>
        /// ͨ�õģ��и��������ֵĺ���
        /// </summary>
        /// <param name="strText">Ҫ������ַ���</param>
        /// <param name="strSep">�ָ�����</param>
        /// <param name="strPart1">���ص�һ����</param>
        /// <param name="strPart2">���صڶ�����</param>
        public static void ParseTwoPart(string strText,
            string strSep,
            out string strPart1,
            out string strPart2)
        {
            strPart1 = "";
            strPart2 = "";

            if (string.IsNullOrEmpty(strText) == true)
                return;

            int nRet = strText.IndexOf(strSep);
            if (nRet == -1)
            {
                strPart1 = strText;
                return;
            }

            strPart1 = strText.Substring(0, nRet).Trim();
            strPart2 = strText.Substring(nRet + strSep.Length).Trim();
        }

        // �����ż���Ĳ����������Hashtable��
        // parameters:
        //      strText �ַ�������̬�� "��1=ֵ1,��2=ֵ2"
        public static Hashtable ParseParameters(string strText)
        {
            return ParseParameters(strText, ',', '=');
        }

        // �����ż���Ĳ����������Hashtable��
        // parameters:
        //      strText �ַ�������̬�� "��1=ֵ1,��2=ֵ2"
        public static Hashtable ParseParameters(string strText,
            char chSegChar,
            char chEqualChar,
            string strDecodeStyle = "")
        {
            Hashtable results = new Hashtable();

            if (string.IsNullOrEmpty(strText) == true)
                return results;

            string[] parts = strText.Split(new char[] { chSegChar });   // ','
            for (int i = 0; i < parts.Length; i++)
            {
                string strPart = parts[i].Trim();
                if (String.IsNullOrEmpty(strPart) == true)
                    continue;
                string strName = "";
                string strValue = "";
                int nRet = strPart.IndexOf(chEqualChar);    // '='
                if (nRet == -1)
                {
                    strName = strPart;
                    strValue = "";
                }
                else
                {
                    strName = strPart.Substring(0, nRet).Trim();
                    strValue = strPart.Substring(nRet + 1).Trim();
                }

                if (String.IsNullOrEmpty(strName) == true
                    && String.IsNullOrEmpty(strValue) == true)
                    continue;

                if (strDecodeStyle == "url")
                    strValue = HttpUtility.UrlDecode(strValue);

                results[strName] = strValue;
            }

            return results;
        }

        private void button_copyNicInfomation_Click(object sender, EventArgs e)
        {
            string strContent = GetNicInformation();
            // Clipboard.SetDataObject(strContent, true);
            CopyToClipboard(strContent);

            // string strFileName = Path.GetTempFileName();
            // System.Diagnostics.Process.Start("notepad.exe", strFileName);

        }

        string _content = "";

        void CopyToClipboard(string strContent)
        {
            try
            {
                Clipboard.SetDataObject(strContent, true);
            }
            catch {
                this._content = strContent;
                Thread thread = new Thread(threadAction);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

        void threadAction()
        {
            Clipboard.SetDataObject(this._content, true);
        }

#if NO
        private void toolStripSplitButton_useCommunityVersion_ButtonClick(object sender, EventArgs e)
        {
            this.textBox_serialCode.Text = "community";
            this.CommunityVersionCode = this.textBox_serialCode.Text;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
#endif

        private void toolStripButton_copyNicInfomation_Click(object sender, EventArgs e)
        {
            string strContent = GetNicInformation();
            CopyToClipboard(strContent);
        }

        // �����������б�
        void SetDefaultCodes(List<string> list)
        {
            this._defaultCodes = list;

            if (list.Count == 0)
            {
                this.toolStripSplitButton_useCommunityVersion.Visible = false;
                return;
            }

            this.toolStripSplitButton_useCommunityVersion.Visible = true;

            this.toolStripSplitButton_useCommunityVersion.DropDownItems.Clear();
            int i = 0;
            foreach(string line in list)
            {
                string[] parts = line.Split(new char[] { '|' });
                string strValue = "";
                string strCaption = "";
                if (parts.Length > 0)
                    strValue = parts[0];
                if (parts.Length > 1)
                    strCaption = parts[1];

                ToolStripItem item = null;
                if (i == 0)
                {
                    item = this.toolStripSplitButton_useCommunityVersion;
                    if (string.IsNullOrEmpty(strCaption) == false)
                    {
                        item.Text = "�л�Ϊ" + strCaption;
                        item.ToolTipText = item.Text;
                    }
                    this.toolStripSplitButton_useCommunityVersion.ButtonClick += item_Click;
                }
                else
                {
                    item = new ToolStripButton("�л�Ϊ" + (string.IsNullOrEmpty(strCaption) == false ? strCaption : strValue));
                    this.toolStripSplitButton_useCommunityVersion.DropDownItems.Add(item);
                    item.Click += item_Click;
                    item.ToolTipText = item.Text;
                }

                {
                    item.Tag = strValue;
                }


                i++;
            }

#if NO
            if (this.toolStripSplitButton_useCommunityVersion.DropDownItems.Count == 0)
                this.toolStripSplitButton_useCommunityVersion.HideDropDown();
#endif
        }

        void item_Click(object sender, EventArgs e)
        {
            this.textBox_serialCode.Text = (string)((ToolStripItem)sender).Tag;
            this.CommunityVersionCode = this.textBox_serialCode.Text;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}