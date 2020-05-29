using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using System.Threading;
using System.Resources;
using System.Globalization;
using System.Runtime.Serialization;
using System.Diagnostics;

using DigitalPlatform.Xml;
using DigitalPlatform.rms.Client;
using DigitalPlatform.IO;
using DigitalPlatform.Text;

namespace DigitalPlatform.Message
{
    /// <summary>
    /// ��Ϣ�շ���������
    /// ����������һ����ϢӦ�á�ͨ�������ȷ�ʽ�Եײ����ݿ����ʵ�����ܰ�ȫ�Ĺ���
    /// </summary>
    public class MessageCenter
    {
        public VerifyAccountEventHandler VerifyAccount;

        public string ServerUrl = "";   // ������URL
        public string MessageDbName = "";   // ��Ϣ���ݿ���
        public List<Box> Boxes = null;

        // ����������
        public const string INBOX = "�ռ���";
        public const string TEMP = "�ݸ�";
        public const string OUTBOX = "�ѷ���";
        public const string RECYCLEBIN = "�ϼ���";

        public MessageCenter()
        {
            InitialStandardBoxes();
        }

        public MessageCenter(string strServerUrl,
            string strMessageDbName)
        {
            this.ServerUrl = strServerUrl;
            this.MessageDbName = strMessageDbName;

            InitialStandardBoxes();
        }

        ResourceManager m_rm = null;

        ResourceManager GetRm()
        {
            if (this.m_rm != null)
                return this.m_rm;

            this.m_rm = new ResourceManager("DigitalPlatform.Message.res.MessageCenter.cs",
                typeof(MessageCenter).Module.Assembly);

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
            catch // (Exception ex)
            {
                return strID + " �� " + Thread.CurrentThread.CurrentUICulture.Name + " ��û���ҵ���Ӧ����Դ��";
            }
        }

        // ��ʼ����׼�ļ�������
        public void InitialStandardBoxes()
        {
            this.Boxes = new List<Box>();

            Box box = null;

            // �ռ��� inbox
            box = new Box();
            box.Name = this.GetString("�ռ���");
            box.Type = INBOX;
            this.Boxes.Add(box);

            // �ݸ� temp
            box = new Box();
            box.Name = this.GetString("�ݸ�");
            box.Type = TEMP;
            this.Boxes.Add(box);

            // �ѷ��� outbox
            box = new Box();
            box.Name = this.GetString("�ѷ���");
            box.Type = OUTBOX;
            this.Boxes.Add(box);

            // �ϼ��� recyclebin
            box = new Box();
            box.Name = this.GetString("�ϼ���");
            box.Type = RECYCLEBIN;
            this.Boxes.Add(box);
        }

        // ����������ת��Ϊboxtypeֵ
        // 2009/7/6
        public string GetBoxType(string strName)
        {
            for (int i = 0; i < this.Boxes.Count; i++)
            {
                Box box = this.Boxes[i];

                if (strName == box.Name)
                    return box.Type;
            }

            return null;    // not found
        }

        public static bool IsInBox(string strBoxType)
        {
            if (strBoxType == INBOX/*"�ռ���"*/)
                return true;
            return false;
        }

        public static bool IsTemp(string strBoxType)
        {
            if (strBoxType == TEMP/*"�ݸ�"*/)
                return true;
            return false;
        }

        public static bool IsOutbox(string strBoxType)
        {
            if (strBoxType == OUTBOX/*"�ѷ���"*/)
                return true;
            return false;
        }

        public static bool IsRecycleBin(string strBoxType)
        {
            if (strBoxType == RECYCLEBIN/*"�ϼ���"*/)
                return true;
            return false;
        }

        // �������ʽ
        // parameters:
        //      strStyle    ���ַ���, ��ʾ�������ڼ���һ��������������Ϣ�ļ���ʽ;
        //                  "untouched", ��ʾ�������ڼ���һ��������δ����Ϣ�ļ���ʽ;
        //                  "touched", ��ʾ�������ڼ���һ���������Ѷ���Ϣ�ļ���ʽ;
        public int MakeSearchQuery(
            string strUserID,
            string strBox,
            string strStyle,
            out string strQueryXml,
            out string strError)
        {
            strQueryXml = "";
            strError = "";

            if (String.IsNullOrEmpty(strUserID) == true)
            {
                // text-level: �ڲ�����
                strError = "strUserID��������Ϊ��";
                return -1;
            }

            if (String.IsNullOrEmpty(strBox) == true)
            {
                // text-level: �ڲ�����
                strError = "strBox��������Ϊ��";
                return -1;
            }

            // ��Ҫע����һ��box�����Ƿ�Ϸ�

            if (String.IsNullOrEmpty(strStyle) == true)
            {
                // 2007/4/5 ���� ������ GetXmlStringSimple()
                strQueryXml = "<target list='"
                    + StringUtil.GetXmlStringSimple(this.MessageDbName)       // 2007/9/14
                    + ":�û�������'><item><order>DESC</order><word>"
        + StringUtil.GetXmlStringSimple(strUserID + "|" + strBox + "|")
        + "</word><match>left</match><relation>=</relation><dataType>string</dataType><maxCount>-1</maxCount></item>"
        + "<lang>zh</lang></target>";
            }

            else if (String.Compare(strStyle, "untouched") == 0)
            {
                // 2007/4/5 ���� ������ GetXmlStringSimple()
                strQueryXml = "<target list='"
                    + StringUtil.GetXmlStringSimple(this.MessageDbName)       // 2007/9/14
                    + ":�û�������'><item><order>DESC</order><word>"
        + StringUtil.GetXmlStringSimple(strUserID + "|" + strBox + "|0")
        + "</word><match>left</match><relation>=</relation><dataType>string</dataType><maxCount>-1</maxCount></item>"
        + "<lang>zh</lang></target>";
            }

            else if (String.Compare(strStyle, "touched") == 0)
            {
                // 2007/4/5 ���� ������ GetXmlStringSimple()
                strQueryXml = "<target list='"
                    + StringUtil.GetXmlStringSimple(this.MessageDbName)       // 2007/9/14
                    + ":�û�������'><item><order>DESC</order><word>"
        + StringUtil.GetXmlStringSimple(strUserID + "|" + strBox + "|1")
        + "</word><match>left</match><relation>=</relation><dataType>string</dataType><maxCount>-1</maxCount></item>"
        + "<lang>zh</lang></target>";
            }
            else
            {
                // text-level: �ڲ�����
                strError = "δ֪��strStyle���� '" + strStyle + "'";
                return -1;
            }

            return 0;
        }

        // TODO: ��Ҫ����һ�����ܣ�����ʾ��ת��Ϊ�����
        // У���ռ����Ƿ����
        // parameters:
        // return:
        //      -1  error
        //      0   not exist
        //      1   exist
        public int DoVerifyRecipient(
            RmsChannelCollection channels,
            string strRecipient,
            out string strError)
        {
            strError = "";

            if (this.VerifyAccount == null)
            {
                // text-level: �ڲ�����
                strError = "MessageCenter ��δ�ҽ� VerifyRecipient �¼����޷�У���ռ��˵Ĵ������";
                return -1;
            }

            VerifyAccountEventArgs e = new VerifyAccountEventArgs();
            e.Name = strRecipient;
            e.Channels = channels;
            this.VerifyAccount(this, e);
            if (e.Error == true)
            {
                strError = e.ErrorInfo;
                return -1;
            }
            if (e.Exist == false)
            {
                if (String.IsNullOrEmpty(e.ErrorInfo) == true)
                {
                    // text-level: �û���ʾ
                    strError = string.Format(this.GetString("�ռ���s������"),   // "�ռ��� '{0}' �����ڡ�"
                        strRecipient);
                        // "�ռ��� '" + strRecipient + "' �����ڡ�";
                    return 0;
                }

                strError = e.ErrorInfo;
                return 0;
            }

            return 1;
        }

        const string EncryptKey = "dp2circulationpassword";

        // ��������
        public static string EncryptPassword(string PlainText)
        {
            return Cryptography.Encrypt(PlainText, EncryptKey);
        }

        // ���ܼ��ܹ�������
        public static string DecryptPassword(string EncryptText)
        {
            return Cryptography.Decrypt(EncryptText, EncryptKey);
        }

        public static string BuildOneAddress(string strDisplayName, string strBarcode)
        {
            string strAddress = "";
            if (String.IsNullOrEmpty(strDisplayName) == false)
            {
                if (strDisplayName.IndexOf("[") == -1)
                    strAddress = "[" + strDisplayName + "]";
                else
                    strAddress = strDisplayName;

                string strEncryptBarcode = MessageCenter.EncryptPassword(strBarcode);

                if (String.IsNullOrEmpty(strEncryptBarcode) == false)
                    strAddress += "=encrypt_barcode:" + strEncryptBarcode;
            }
            else
                strAddress = strBarcode;

            return strAddress;
        }

        int ParseAddress(string strAddress,
            out List<string> ids,
            out List<string> origins,
            out string strError)
        {
            ids = new List<string>();
            origins = new List<string>();
            strError = "";
            int nRet = 0;

            string[] parts = strAddress.Split(new char [] {';',','});
            for (int i = 0; i < parts.Length; i++)
            {
                string strPart = parts[i].Trim();
                if (String.IsNullOrEmpty(strPart) == true)
                    continue;

                string strLeft = "";
                string strRight = "";
                nRet = strPart.IndexOf("=");
                if (nRet == -1)
                    strLeft = strPart;
                else
                {
                    strLeft = strPart.Substring(0, nRet).Trim();
                    strRight = strPart.Substring(nRet + 1).Trim();
                }

                if (String.IsNullOrEmpty(strRight) == false)
                    strLeft = strRight;

                // encrypt_barcode
                nRet = strLeft.IndexOf(":");
                if (nRet != -1)
                {
                    string strName = "";
                    string strValue = "";

                    strName = strLeft.Substring(0, nRet).Trim();
                    strValue = strRight.Substring(nRet + 1);

                    if (strName == "encrypt_barcode")
                        strLeft = DecryptPassword(strValue);
                    else
                    {
                        strError = "�޷�ʶ��Ĳ������� '"+strName+"'";
                        return -1;
                    }

                }
                else if (strLeft.IndexOf("[") != -1)
                {
                    // ����һ�� ��ʾ��
                    // TODO: ��Ҫ����Ϊ�����?

                }

                ids.Add(strLeft);
                origins.Add(strPart);
            }

            return 0;
        }

        // ������Ϣ
        // parameters:
        //      bVerifyRecipient    �Ƿ���֤�ռ��˵�ַ
        // return:
        //      -1  ����
        //      0   �ɹ�
        public int SendMessage(
            RmsChannelCollection Channels,
            string strRecipient,
            string strSender,
            string strSubject,
            string strMime,
            string strBody,
            bool bVerifyRecipient,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            if (String.IsNullOrEmpty(strRecipient) == true)
            {
                // text-level: �û���ʾ
                strError = this.GetString("�ռ��˲���Ϊ��");    // "�ռ��˲���Ϊ��"
                return -1;
            }

            List<string> userids = null;
            List<string> origins = null;
            nRet = ParseAddress(strRecipient,
            out userids,
            out origins,
            out strError);
            if (nRet == -1)
            {
                strError = "�ռ��˵�ַ��ʽ����ȷ: " + strError;
                return -1;
            }

            for (int i = 0; i < userids.Count; i++)
            {
                string strUserID = userids[i];
                string strOrigin = origins[i];

                if (bVerifyRecipient == true)
                {
                    // У���ռ����Ƿ����
                    // parameters:
                    // return:
                    //      -1  error
                    //      0   not exist
                    //      1   exist
                    nRet = DoVerifyRecipient(
                        Channels,
                        strUserID,
                        out strError);
                    if (nRet == -1 || nRet == 0)
                        return -1;
                }

                XmlDocument dom = new XmlDocument();
                dom.LoadXml("<root />");

                DomUtil.SetElementText(dom.DocumentElement,
                    "sender", strSender);
                DomUtil.SetElementText(dom.DocumentElement,
                    "recipient", strOrigin);    // fullname
                DomUtil.SetElementText(dom.DocumentElement,
                    "subject", strSubject);
                DomUtil.SetElementText(dom.DocumentElement,
                    "date", DateTimeUtil.Rfc1123DateTimeStringEx(DateTime.UtcNow.ToLocalTime()));
                DomUtil.SetElementText(dom.DocumentElement,
                    "size", Convert.ToString(strBody.Length));
                DomUtil.SetElementText(dom.DocumentElement,
                    "touched", "0");
                DomUtil.SetElementText(dom.DocumentElement,
                    "username", strUserID);
                DomUtil.SetElementText(dom.DocumentElement,
                    "box", MessageCenter.INBOX);
                DomUtil.SetElementTextPure(dom.DocumentElement,
                    "content", strBody);
                DomUtil.SetElementText(dom.DocumentElement,
                    "mime", strMime);

                RmsChannel channel = Channels.GetChannel(this.ServerUrl);

                byte[] timestamp = null;
                byte[] output_timestamp = null;
                string strOutputPath = "";

                // д���ռ���
                long lRet = channel.DoSaveTextRes(this.MessageDbName + "/?",
                    dom.OuterXml,
                    false,
                    "content,ignorechecktimestamp",
                    timestamp,
                    out output_timestamp,
                    out strOutputPath,
                    out strError);
                if (lRet == -1)
                {
                    // text-level: �ڲ�����
                    strError = "д���ռ���ʱ����: " + strError;
                    return -1;
                }

                DomUtil.SetElementText(dom.DocumentElement,
                    "username", strSender);
                DomUtil.SetElementText(dom.DocumentElement,
                    "box", MessageCenter.OUTBOX);


                // д���ѷ�������
                lRet = channel.DoSaveTextRes(this.MessageDbName + "/?",
        dom.OuterXml,
        false,
        "content,ignorechecktimestamp",
        null,
        out output_timestamp,
        out strOutputPath,
        out strError);
                if (lRet == -1)
                {
                    // text-level: �ڲ�����
                    strError = "д���ѷ�������ʱ����: " + strError;
                    return -1;
                }
            }

            return 0;
        }

        // ���Ͳݸ����е�һ����Ϣ
        // parameters:
        //      bVerifyRecipient    �Ƿ���֤�ռ��˵�ַ
        public int SendMessage(
            string strMessageID,
            string strRecipient,
            bool bVerifyRecipient,
            out string strError)
        {
            strError = "";

            return 0;
        }

        // ������Ϣ��"�ݸ�"��
        // parameters:
        //      strOldRecordID  ԭ���ڲݸ����еļ�¼id������д�id���ø��Ƿ�ʽд�룬������׷�ӷ�ʽд��
        public int SaveMessage(
            RmsChannelCollection Channels,
            string strRecipient,
            string strSender,
            string strSubject,
            string strMime,
            string strBody,
            string strOldRecordID,
            byte [] baOldTimeStamp,
            out byte[] baOutputTimeStamp,
            out string strOutputID,
            out string strError)
        {
            strError = "";
            baOutputTimeStamp = null;
            strOutputID = "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<root />");

            DomUtil.SetElementText(dom.DocumentElement,
                "sender", strSender);
            DomUtil.SetElementText(dom.DocumentElement,
                "recipient", strRecipient);
            DomUtil.SetElementText(dom.DocumentElement,
                "subject", strSubject);
            DomUtil.SetElementText(dom.DocumentElement,
                "date", DateTimeUtil.Rfc1123DateTimeStringEx(DateTime.UtcNow.ToLocalTime()));
            DomUtil.SetElementText(dom.DocumentElement,
                "size", Convert.ToString(strBody.Length));
            DomUtil.SetElementText(dom.DocumentElement,
                "touched", "0");
            DomUtil.SetElementText(dom.DocumentElement,
                "username", strSender);
            DomUtil.SetElementText(dom.DocumentElement,
                "box", MessageCenter.TEMP);
            DomUtil.SetElementTextPure(dom.DocumentElement,
                "content", strBody);
            DomUtil.SetElementText(dom.DocumentElement,
    "mime", strMime);

            RmsChannel channel = Channels.GetChannel(this.ServerUrl);

            // byte[] timestamp = null;
            // byte[] output_timestamp = null;
            string strOutputPath = "";

            string strPath = "";

            if (String.IsNullOrEmpty(strOldRecordID) == true)
            {
                strPath = this.MessageDbName + "/?";
            }
            else
            {
                strPath = this.MessageDbName + "/" + strOldRecordID;
            }

            // д�ز��¼
            long lRet = channel.DoSaveTextRes(strPath,
                dom.OuterXml,
                false,
                "content,ignorechecktimestamp",
                baOldTimeStamp,
                out baOutputTimeStamp,
                out strOutputPath,
                out strError);
            if (lRet == -1)
            {
                return -1;
            }

            strOutputID = ResPath.GetRecordId(strOutputPath);

            return 0;
        }

        // �۲��ռ���״̬
        public int CheckInbox(string strUserName,
            out int nMessageCount,
            out string strError)
        {
            nMessageCount = 0;
            strError = "";

            return 0;
        }

        // һ���Ի�������Ϣ
        // parameters:
        //      message_ids ��ϢID�����顣����ַ����а���'/'������·�����������id
        public int GetMessage(
    RmsChannelCollection Channels,
    string strUserID,
    string[] message_ids,
    MessageLevel messagelevel,
    out List<MessageData> messages,
    out string strError)
        {
            strError = "";
            messages = new List<MessageData>();

            RmsChannel channel = Channels.GetChannel(this.ServerUrl);
            if (channel == null)
            {
                strError = "get channel error";
                return -1;
            }

            for (int i = 0; i < message_ids.Length; i++)
            {
                string strID = message_ids[i];

                string strPath = strID;
                if (strID.IndexOf("/") == -1)
                    strPath = this.MessageDbName + "/" + strID;
                MessageData message = null;

                int nRet = 0;

                nRet = GetMessageByPath(
                    channel,
                    strPath,
                    messagelevel,
                    out message,
                    out strError);
                if (nRet == -1)
                    return -1;

                if (strUserID != null && message.strUserName != strUserID)
                {
                    // text-level: �û���ʾ
                    strError = string.Format(this.GetString("������Ϣ�������û�s, ������쿴"),  // "������Ϣ�������û� '{0}', ������쿴��"
                        strUserID);
                    // "������Ϣ�������û� '" +strUserID+ "', ������쿴��";
                    return -1;
                }

                messages.Add(message);
            }

            return 1;
        }

        // ������Ϣ��¼id�����Ϣ��ϸ����
        // ���������������Ϣ�Ƿ�����strUserIDָ�����û�
        // parameters:
        //      strUserID   ���==null����ʾ�������Ϣ���ں��û�
        public int GetMessage(
            RmsChannelCollection Channels,
            string strUserID,
            string strMessageID,
            MessageLevel messagelevel,
            out MessageData message,
            out string strError)
        {
            strError = "";
            message = null;

            int nRet = 0;

            RmsChannel channel = Channels.GetChannel(this.ServerUrl);
            if (channel == null)
            {
                strError = "get channel error";
                return -1;
            }

            string strPath = this.MessageDbName + "/" + strMessageID;

            nRet = GetMessageByPath(
                channel,
                strPath,
                messagelevel,
                out message,
                out strError);
            if (nRet == -1)
                return -1;

            if (strUserID != null && message.strUserName != strUserID)
            {
                // text-level: �û���ʾ
                strError = string.Format(this.GetString("������Ϣ�������û�s, ������쿴"),  // "������Ϣ�������û� '{0}', ������쿴��"
                    strUserID);
                    // "������Ϣ�������û� '" +strUserID+ "', ������쿴��";
                return -1;
            }

            return 1;
        }

        // ������Ϣ��¼·�������Ϣ
        // �������Ϣ�Ƿ������ض��û�
        int GetMessageByPath(
            RmsChannel channel,
            string strPath,
            MessageLevel messagelevel,
            out MessageData data,
            out string strError)
        {
            data = new MessageData();

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


            data.strSender = DomUtil.GetElementText(dom.DocumentElement,
                "sender");
            data.strRecipient = DomUtil.GetElementText(dom.DocumentElement,
                "recipient");
            data.strSubject = DomUtil.GetElementText(dom.DocumentElement,
                "subject");
            data.strCreateTime = DomUtil.GetElementText(dom.DocumentElement,
                "date");
            data.strMime = DomUtil.GetElementText(dom.DocumentElement,
                "mime");

            data.strSize = DomUtil.GetElementText(dom.DocumentElement,
                "size");

            string strTouched = DomUtil.GetElementText(dom.DocumentElement,
                "touched");
            if (strTouched == "1")
                data.Touched = true;
            else
                data.Touched = false;

            data.strRecordID = ResPath.GetRecordId(strOutputPath);

            if (messagelevel == MessageLevel.Full)
            {
                data.strBody = DomUtil.GetElementText(dom.DocumentElement,
                    "content");
            }

            data.strUserName = DomUtil.GetElementText(dom.DocumentElement,
                "username");

            // �㶨Ϊ��������
            data.strBoxType = DomUtil.GetElementText(dom.DocumentElement,
                "box");

            data.TimeStamp = timestamp;

            // �޸�touchedԪ��ֵ
            if (messagelevel == MessageLevel.Full
                && data.Touched == false)
            {
                DomUtil.SetElementText(dom.DocumentElement,
                    "touched", "1");

                byte[] output_timestamp = null;
                //string strOutputPath = "";

                lRet = channel.DoSaveTextRes(strPath,
                    dom.OuterXml,
                    false,
                    "content,ignorechecktimestamp",
                    timestamp,
                    out output_timestamp,
                    out strOutputPath,
                    out strError);
                if (lRet == -1)
                {
                    // text-level: �ڲ�����
                    strError = "д�ؼ�¼ '"+strPath+"' ʱ����: " + strError;
                    return -1;
                }
                data.Touched = true;
                data.TimeStamp = output_timestamp;

            }

            return 1;
        }


        // ���δ����Ϣ��
        public int GetUntouchedMessageCount(
            RmsChannelCollection Channels,
            string strUserID,
            string strBoxType,
            out string strError)
        {
            RmsChannel channel = Channels.GetChannel(this.ServerUrl);
            if (channel == null)
            {
                strError = "get channel error";
                return -1;
            }

            string strQueryXml = "";

            // �������ʽ
            int nRet = MakeSearchQuery(
                strUserID,
                strBoxType,
                "untouched",   // ������ȫ���ʼ�
                out strQueryXml,
                out strError);
            if (nRet == -1)
            {
                // text-level: �ڲ�����
                strError = "�������ʽ����: " + strError;
                return -1;
            }


            long lRet = channel.DoSearch(strQueryXml,
                "null",
                "", // strOuputStyle
                out strError);
            if (lRet == -1)
            {
                // text-level: �ڲ�����
                strError = "����ʧ��: " + strError;
                return -1;
            }

            return (int)lRet;
        }


        // ���������Ϣ, ���ߴӽ�����л����Ϣ
        // parameters:
        //      strStyle    search / untouched / touched
        //                  ��search��ʾ���м����ͻ�ȡ��û��search�ͱ�ʾ����������ȡ��ǰ�����Ľ������
        //                  untoched��touchedӦ����search���á�����ֻ�ܻ�ȡ��ǰ�Ľ����
        public int GetMessage(
            RmsChannelCollection Channels,
            string strResultsetName,
            string strStyle,
            string strUserID,
            string strBoxType,
            MessageLevel messagelevel,
            int nStart,
            int nCount,
            out int nTotalCount,
            out List<MessageData> messages,
            out string strError)
        {
            nTotalCount = 0;
            messages = new List<MessageData>();
            strError = "";

            if (String.IsNullOrEmpty(this.MessageDbName) == true)
            {
                strError = "��Ϣ����δ����";
                return -1;
            }

            int nRet = 0;
            long lRet = 0;

            if (String.IsNullOrEmpty(strBoxType) == true)
            {
                strBoxType = MessageCenter.INBOX;
            }

            RmsChannel channel = Channels.GetChannel(this.ServerUrl);
            if (channel == null)
            {
                strError = "get channel error";
                return -1;
            }


            if (String.IsNullOrEmpty(strResultsetName) == true)
                strResultsetName = "messages_of_" + strBoxType;

            bool bSearch = true;
            if (StringUtil.IsInList("search", strStyle) == true)
                bSearch = true;
            else
                bSearch = false;

            string strQueryStyle = "";
            if (StringUtil.IsInList("touched", strStyle) == true)
                strQueryStyle = "touched";
            else if (StringUtil.IsInList("untouched", strStyle) == true)
                strQueryStyle = "untouched";

            // ����
            if (bSearch == true)
            {

                string strQueryXml = "";

                // �������ʽ
                nRet = MakeSearchQuery(
                    strUserID,
                    strBoxType,
                    strQueryStyle,
                    out strQueryXml,
                    out strError);
                if (nRet == -1)
                {
                    // text-level: �ڲ�����
                    strError = "�������ʽ����: " + strError;
                    return -1;
                }


                lRet = channel.DoSearch(strQueryXml,
                    strResultsetName,
                    "", // strOuputStyle
                    out strError);
                if (lRet == -1)
                {
                    // text-level: �ڲ�����
                    strError = "����ʧ��: " + strError;
                    return -1;
                }

                // not found
                if (lRet == 0)
                {
                    // text-level: �û���ʾ
                    strError = this.GetString("û���κ���Ϣ");  // "û���κ���Ϣ"
                    return 0;
                }

                nTotalCount = (int)lRet;
            }


            if (nCount == 0)
                return nTotalCount;   // �������Ҫ��ý����

            Debug.Assert(nStart >= 0, "");


            // ��ý������ָ����Χ�ļ�¼·��
            ArrayList aLine = null;
            lRet = channel.DoGetSearchFullResult(
                strResultsetName,
                nStart,
                nCount,
                "zh",
                null,
                out aLine,
                out strError);
            if (lRet == -1)
            {
                // ��Ȼ����-1,����aLine����Ȼ��������
                if (aLine == null)
                {
                    // text-level: �ڲ�����
                    strError = "��ȡ�����ʽʧ��: " + strError;
                    return -1;
                }
            }

            // ��������
            for (int i = 0; i < aLine.Count; i++)
            {
                string[] cols = null;

                cols = (string[])aLine[i];

                string strPath = cols[0];

                MessageData data = null;

                // TODO: level == none ֻ����·��
                nRet = GetMessageByPath(
                    channel,
                    strPath,
                    messagelevel,
                    out data,
                    out strError);
                if (nRet == -1)
                    return -1;

                messages.Add(data);

            }

            return aLine.Count;
        }

        // ɾ��һ��box�е�ȫ����Ϣ
        public int DeleteMessage(
            RmsChannelCollection Channels,
            string strUserID,
            bool bMoveToRecycleBin,
            string strBoxType,
            out string strError)
        {
            strError = "";

            int nStart = 0;
            int nCount = -1;
                int nTotalCount = 0;
            for (; ; )
            {
                List<MessageData> messages = null;

                int nRet = GetMessage(
                    Channels,
                    "message_deleteall",
                    nStart == 0 ? "search" : "",
                    strUserID,
                    strBoxType,
                    MessageLevel.ID,
                    nStart,
                    nCount,
                out nTotalCount,
                out messages,
                out strError);
                if (nRet == -1)
                    return -1;

                if (nCount == -1)
                    nCount = nTotalCount - nStart;

                for (int j = 0; j < messages.Count; j++)
                {
                    MessageData data = messages[j];

                    nRet = DeleteMessage(
                        bMoveToRecycleBin,
                        Channels,
                        data.strRecordID,
                        data.TimeStamp,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }


                nStart += messages.Count;
                nCount -= messages.Count;
                
                if (nStart >= nTotalCount)
                    break;
                if (nCount <= 0)
                    break;
            }


            return nTotalCount;
        }

        // ɾ����Ϣ
        public int DeleteMessage(
            bool bMoveToRecycleBin,
            RmsChannelCollection Channels,
            List<string> ids,
            List<byte[]> timestamps,
            out string strError)
        {
            strError = "";

            string strError1 = "";

//            Channel channel = Channels.GetChannel(this.ServerUrl);

            for (int i = 0; i<ids.Count; i++)
            {
                string strID = ids[i];

                // string strPath = this.MessageDbName + "/" + strID;

                byte [] timestamp = null;
                // byte[] output_timestamp = null;

                if (timestamps != null)
                    timestamp = timestamps[i];

                int nRet = DeleteMessage(
                    bMoveToRecycleBin,
                    Channels,
                    strID,
                    timestamp,
                    out strError);
                if (nRet == -1)
                {
                    // text-level: �ڲ�����
                    strError1 += "ɾ����¼ '" +strID+ "' ʱ��������: "+ strError + ";";
                }

            }

            if (strError1 != "")
            {
                strError = strError1;
                return -1;
            }

            return 0;
        }

        // ɾ��һ����Ϣ
        public int DeleteMessage(
            bool bMoveToRecycleBin,
            RmsChannelCollection Channels,
            string strID,
            byte [] timestamp,
            out string strError)
        {
            RmsChannel channel = Channels.GetChannel(this.ServerUrl);

            string strPath = this.MessageDbName + "/" + strID;

            long lRet = 0;
            byte[] output_timestamp = null;


            // Ҫ�ƶ����ϼ���
            if (bMoveToRecycleBin == true)
            {
                string strXml = "";
                string strMetaData = "";
                string strOutputPath = "";

                // ������¼
                lRet = channel.GetRes(strPath,
                    out strXml,
                    out strMetaData,
                    out output_timestamp,
                    out strOutputPath,
                    out strError);
                if (lRet == -1)
                {
                    // text-level: �ڲ�����
                    strError = "������¼ '" + strPath + "' ʱ����: " + strError;
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

                // �޸�box����
                DomUtil.SetElementText(dom.DocumentElement,
                    "box", MessageCenter.RECYCLEBIN);

                timestamp = output_timestamp;
                // д��
                lRet = channel.DoSaveTextRes(strPath,
    dom.OuterXml,
    false,
    "content,ignorechecktimestamp",
    timestamp,
    out output_timestamp,
    out strOutputPath,
    out strError);
                if (lRet == -1)
                {
                    // text-level: �ڲ�����
                    strError = "д�ؼ�¼ '" + strPath + "' ʱ����: " + strError;
                    return -1;
                }

                return 0;
            }

            bool bNullTimeStamp = false;
            if (timestamp == null)
                bNullTimeStamp = true;


        REDO:
            lRet = channel.DoDeleteRes(strPath,
                timestamp,
                out output_timestamp,
                out strError);
            if (lRet == -1)
            {
                if (bNullTimeStamp == true
                    && channel.ErrorCode == ChannelErrorCode.TimestampMismatch)
                {
                    timestamp = output_timestamp;
                    goto REDO;
                }

                // text-level: �ڲ�����
                strError = "ɾ����¼ '" + strPath + "' ʱ��������: " + strError;
                return -1;
            }

            return 0;
        }
    }

    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public enum MessageLevel
    {
        [EnumMember]
        ID = 0,    // ֻ����ID
        [EnumMember]
        Summary = 1,    // ժҪ����������body
        [EnumMember]
        Full = 2,   // ȫ����������ȫ����Ϣ
    }

    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public class MessageData
    {
        [DataMember]
        public string strUserName = ""; // ��Ϣ���������û�ID
        [DataMember]
        public string strBoxType = "";

        [DataMember]
        public string strSender = "";   // ������
        [DataMember]
        public string strRecipient = "";    // ������
        [DataMember]
        public string strSubject = "";  // ����
        [DataMember]
        public string strMime = ""; // ý������
        [DataMember]
        public string strBody = "";
        [DataMember]
        public string strCreateTime = "";   // �ʼ�����(�յ�)ʱ��
        [DataMember]
        public string strSize = "";     // �ߴ�
        [DataMember]
        public bool Touched = false;    // �Ƿ��Ķ���
        [DataMember]
        public string strRecordID = ""; // ��¼ID������Ψһ��λһ����Ϣ

        [DataMember]
        public byte[] TimeStamp = null;

        public MessageData()
        {
        }

        public MessageData(MessageData origin)
        {
            this.strUserName = origin.strUserName;
            this.strBoxType = origin.strBoxType;

            this.strSender = origin.strSender;
            this.strRecipient = origin.strRecipient;
            this.strSubject = origin.strSubject;
            this.strMime = origin.strMime;
            this.strBody = origin.strBody;
            this.strCreateTime = origin.strCreateTime;
            this.strSize = origin.strSize;
            this.Touched = origin.Touched;

            this.strRecordID = origin.strRecordID;
            this.TimeStamp = origin.TimeStamp;
        }
    }

    public class Box
    {
        public string Name = "";
        public string Type = "";    // ����

        /*
        INBOX = "�ռ���";
        TEMP = "�ݸ�";
        OUTBOX = "�ѷ���";
        RECYCLEBIN = "�ϼ���";
         * */

    }

    // У��һ���ʻ����Ƿ����
    public delegate void VerifyAccountEventHandler(object sender,
    VerifyAccountEventArgs e);

    public class VerifyAccountEventArgs : EventArgs
    {
        public RmsChannelCollection Channels = null;
        public string Name = "";    // [in]
        public bool Exist = false;  // [out]
        public bool Error = false;  // [out] ֻ�е�Exist == false��ʱ��Error���� == true
        public string ErrorInfo = "";   // [out] ��Exist==false������Error == true��ʱ�򣬶�Ӧ�����س�����Ϣ��
    }
}
