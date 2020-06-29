using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;

using DigitalPlatform.ResultSet;
using DigitalPlatform.Xml;
using DigitalPlatform.IO;
using DigitalPlatform.Text;
using DigitalPlatform.Range;

namespace DigitalPlatform.rms
{
    // �ļ���������
    public class FileDatabase : Database
    {
        // ���������ݿ�Ŀ¼
        internal string m_strPureSourceDir = "";

        // ���ݿ�Ŀ¼ȫ·����ĩβ����\
        internal string m_strSourceFullPath = "";

        public FileDatabase(DatabaseCollection container)
            : base(container)
        { }

        // ��ʼ�����ݿ���
        // parameters:
        //      node    ���ݿ����ýڵ�<database>
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        internal override int Initial(XmlNode node,
            out string strError)
        {
            strError = "";
            Debug.Assert(node != null, "Initial()���ô���node����ֵ����Ϊnull��");

            //****************�����ݿ��д��**** �ڹ���ʱ,�����ܶ�Ҳ����д
            this.m_lock.AcquireWriterLock(m_nTimeOut);
            try
            {
                this.m_selfNode = node;

                // ֻ�������д�ˣ�Ҫ������δ��ʼ���ء�
#if DEBUG_LOCK
				this.container.WriteDebugInfo("Initial()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
                // �����㳤��
                // return:
                //      -1  ����
                //      0   �ɹ�
                // ��: ����ȫ
                int nRet = this.container.InternalGetKeySize(
                    out this.KeySize,
                    out strError);
                if (nRet == -1)
                    return -1;

                // ��ID
                this.PureID = DomUtil.GetAttr(this.m_selfNode, "id").Trim();
                if (this.PureID == "")
                {
                    strError = "�����ļ����Ϸ�����nameΪ'" + this.GetCaption("zh-cn") + "'��<database>�¼�δ����'id'���ԣ���'id'����Ϊ��";
                    return -1;
                }

                // ���Խڵ�
                this.m_propertyNode = this.m_selfNode.SelectSingleNode("property");
                if (this.m_propertyNode == null)
                {
                    strError = "�����ļ����Ϸ�����nameΪ'" + this.GetCaption("zh-cn") + "'��<database>�¼�δ����<property>Ԫ��";
                    return -1;
                }

                XmlNode nodeDatasource = this.m_propertyNode.SelectSingleNode("datasource");
                if (nodeDatasource == null)
                {
                    strError = "�����������ļ����Ϸ�����nameΪ'" + this.GetCaption("zh-cn") + "'��database/property�¼�δ����<datasource>Ԫ��";
                    return -1;
                }

                // ����������ԴĿ¼
                this.m_strPureSourceDir = DomUtil.GetNodeText(nodeDatasource).Trim();
                if (this.m_strPureSourceDir == "")
                {
                    strError = "�����ļ����Ϸ�����nameΪ'" + this.GetCaption("zh-cn") + "'��database/property/datasource�Ľڵ������Ϊ��";
                    return -1;
                }

                // ����ԴĿ¼ȫ·��
                this.m_strSourceFullPath = this.container.DataDir + "\\" + this.m_strPureSourceDir;

            }
            finally
            {
                m_lock.ReleaseWriterLock();
                //***********�����ݿ��д��*************
#if DEBUG_LOCK
				this.container.WriteDebugInfo("Initial()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            }

            return 0;
        }

        // �õ�����ԴĿ¼�������ļ������ݿ⣬��������ԴĿ¼����
        public override string GetSourceName()
        {
            return this.m_strPureSourceDir;
        }

        // ��ʼ����������ԴĿ¼
        // parameter:
        //		strError    out���������س�����Ϣ
        // return:
        //		-1  ����
        //		0   �ɹ�
        // ��: ��ȫ��
        // ΪʲôҪ������:��ΪDeleteDir(),seed
        public override int InitialPhysicalDatabase(out string strError)
        {
            strError = "";
            //************�����ݿ��д��*********
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("Initialize()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            try
            {
                // ��������Ŀ¼
                // ����Ѵ���Դ����Ŀ¼������ɾ���������´���
                if (Directory.Exists(this.m_strSourceFullPath))
                    Directory.Delete(this.m_strSourceFullPath, true);
                Directory.CreateDirectory(this.m_strSourceFullPath);


                // �����������ļ�
                KeysCfg keysCfg = null;
                int nRet = this.GetKeysCfg(out keysCfg,
                    out strError);
                if (nRet == -1)
                    return -1;
                if (keysCfg != null)
                {
                    List<TableInfo> aTableInfo = null;
                    nRet = keysCfg.GetTableInfosRemoveDup(out aTableInfo,
                        out strError);
                    if (nRet == -1)
                        return -1;

                    // 2.���������ļ�
                    string strText = @"<?xml version='1.0' encoding='utf-8' ?><root></root>";
                    for (int i = 0; i < aTableInfo.Count; i++)
                    {
                        TableInfo tableInfo = aTableInfo[i];
                        string strKeyFileName = this.TableName2TableFileName(tableInfo.SqlTableName);
                        FileUtil.String2File(strText, strKeyFileName);
                    }
                }

                // 3.������ֵ
                this.SetTailNo(0);
                this.container.Changed = true;
            }
            finally
            {
                //**************�����ݿ��д��************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("Initialize()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            }
            return 0;
        }

        // �õ�xml����
        // ��:��ȫ��,���ⲿ��
        public override int GetXmlData(string strID,
            out string strXml,
            out string strError)
        {
            strXml = "";
            strError = "";

            strID = DbPath.GetID10(strID);

            string strXmlFilePath = this.GetXmlFilePath(strID);
            strXml = FileUtil.File2StringE(strXmlFilePath);
            return 0;
        }

        // ����strStyle���,�õ���͵ļ�¼��
        // prev:ǰһ��,next:��һ��,���strID == ? ��prevΪ��һ��,nextΪ���һ��
        // ���������prev��next���ܵ��˺���
        // parameter:
        //		strCurrentRecordID	��ǰ��¼ID
        //		strStyle	        ���
        //      strOutputRecordID   out�����������ҵ��ļ�¼��
        //      strError            out���������س�����Ϣ
        // return:
        //		-1  ����
        //      0   δ�ҵ�
        //      1   �ҵ�
        // �ߣ�����ȫ
        internal override int GetRecordID(string strCurrentRecordID,
            string strStyle,
            out string strOutputRecordID,
            out string strError)
        {
            strOutputRecordID = "";
            strError = "";

            if (strCurrentRecordID.Length != 10)
                strCurrentRecordID = DbPath.GetID10(strCurrentRecordID);// ȷ��һ��strIDΪ10λ��

            // ��ͨ��ʱ�򷵻�ԭ��¼��
            if ((StringUtil.IsInList("prev", strStyle) == false)
                && (StringUtil.IsInList("next", strStyle) == false))
            {
                Debug.Assert(false, "GetRecordID()���ô������strStyle����������prev��nextֵ��Ӧ�ߵ����");
                throw new Exception("GetRecordID()���ô������strStyle����������prev��nextֵ��Ӧ�ߵ����");
            }

            //��Ŀ¼�еõ����б�ʾ��¼�ļ�
            string[] files = Directory.GetFiles(
                this.m_strSourceFullPath,
                "??????????.xml");
            ArrayList records = new ArrayList();
            foreach (string filename in files)
            {
                FileInfo fileInfo = new FileInfo(filename);
                string strFileName = fileInfo.Name;
                if (this.IsRecord(strFileName) == false)
                    continue;
                records.Add(this.XmlFileName2RecordID(strFileName));
            }
            // û�м�¼����ȻҲ�����ڼ�¼�ˡ�
            if (records.Count == 0)
            {
                return 0;
            }

            // �Լ�¼��������
            records.Sort(new ComparerClass());


            // ��ǰ��
            if ((StringUtil.IsInList("prev", strStyle) == true))
            {
                if (DbPath.GetCompressedID(strCurrentRecordID) == "-1")
                {
                    strOutputRecordID = (string)records[records.Count - 1];
                    return 1;
                }
                else if (StringUtil.IsInList("myself", strStyle) == true)
                {
                    int nIndex = records.IndexOf(strCurrentRecordID);
                    if (nIndex != -1)
                    {
                        strOutputRecordID = strCurrentRecordID;
                        return 1;
                    }

                    for (int i = records.Count - 1; i >= 0; i--)
                    {
                        if (String.Compare((string)records[i], strCurrentRecordID) < 0)
                        {
                            strOutputRecordID = (string)records[i];
                            return 1;
                        }
                    }
                }
                else
                {
                    for (int i = records.Count - 1; i >= 0; i--)
                    {
                        if (String.Compare((string)records[i], strCurrentRecordID) < 0)
                        {
                            strOutputRecordID = (string)records[i];
                            return 1;
                        }
                    }
                }
            }
            else if (StringUtil.IsInList("next", strStyle) == true)
            {
                if (DbPath.GetCompressedID(strCurrentRecordID) == "-1")
                {
                    strOutputRecordID = (string)records[0];
                    return 1;
                }
                else if (StringUtil.IsInList("myself", strStyle) == true)
                {
                    int nIndex = records.IndexOf(strCurrentRecordID);
                    if (nIndex != -1)
                    {
                        strOutputRecordID = strCurrentRecordID;
                        return 1;
                    }
                    for (int i = 0; i < records.Count; i++)
                    {
                        if (String.Compare((string)records[i], strCurrentRecordID) > 0)
                        {
                            strOutputRecordID = (string)records[i];
                            return 1;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < records.Count; i++)
                    {
                        if (String.Compare((string)records[i], strCurrentRecordID) > 0)
                        {
                            strOutputRecordID = (string)records[i];
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }

        // ��ָ����Χ����Դ
        // parameter:
        //		strID       ��¼ID
        //		nStart      ��ʼλ��
        //		nLength     ���� -1:��ʼ������
        //		nMaxLength  ��󳤶�
        //		destBuffer  out�����������ֽ�����
        //		timestamp   out����������ʱ���
        //		strError    out���������س�����Ϣ
        // return:
        //		-1  ����
        //		-4  δ�ҵ���¼
        //      -10 ��¼�ֲ�δ�ҵ�
        //		>=0 ��Դ�ܳ���
        //      nAdditionError -50 ��һ�������¼���Դ��¼������(TODO:��δʵ�� 2006/7/3)
        public override long GetXml(string strID,
            string strXPath,
            int nStart,
            int nLength,
            int nMaxLength,
            string strStyle,
            out byte[] destBuffer,
            out string strMetadata,
            out string strOutputResID,
            out byte[] outputTimestamp,
            bool bCheckAccount,
            out int nAdditionError,
            out string strError)
        {
            destBuffer = null;
            strMetadata = "";
            strOutputResID = "";
            outputTimestamp = null;
            strError = "";
            nAdditionError = 0;

            int nRet = 0;

            if (strID == "?")
                strID = "-1";

            try
            {
                long nId = Convert.ToInt64(strID);
                if (nId < -1)
                {
                    strError = "��¼��'" + strID + "'���Ϸ�";
                    return -1;
                }
            }
            catch
            {
                strError = "��¼��'" + strID + "'���Ϸ�";
                return -1;
            }


            // ���ݷ��ȡ��¼��
            strStyle = strStyle.Trim();
            if (StringUtil.IsInList("prev", strStyle) == true
                || StringUtil.IsInList("next", strStyle) == true)
            {

                // �õ�ָ���ļ�¼��
                // return:
                //		-1  ����
                //      0   δ�ҵ�
                //      1   �ҵ�
                // �ߣ�����ȫ
                nRet = this.GetRecordID(strID,
                    strStyle,
                    out strOutputResID,
                    out strError);
                if (nRet == -1)
                    return -1;

                if (nRet == 0)
                {
                    strError = "û�ҵ���¼��'" + strID + "'�ķ��Ϊ'" + strStyle + "'�ļ�¼";
                    return -4;
                }
                strID = strOutputResID;
            }
            strID = DbPath.GetID10(strID);

            // ������Դ·��
            if (StringUtil.IsInList("outputpath", strStyle) == true)
            {
                strOutputResID = DbPath.GetCompressedID(strID);
            }


            // ���ʻ��⿪�ĺ��ţ����ڸ����ʻ�
            if (bCheckAccount == true &&
                StringUtil.IsInList("account", this.TypeSafety) == true)
            {
                // ���Ҫ��ü�¼�������˻����¼��������
                // UserCollection�У��ǾͰ���ص�User��¼
                // ��������ݿ⣬�Ա��Ժ�����ݿ�����ȡ��
                // �����ش��ڴ�����ȡ��
                string strResPath = this.FullID + "/" + strID;

                // return:
                //		-1  ����
                //      -4  ��¼������
                //		0   �ɹ�
                nRet = this.container.UserColl.SaveUserIfNeed(
                    strResPath,
                    out strError);
                if (nRet <= -1)
                    return nRet;
            }


            //**********�����ݿ�Ӷ���***************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetXml()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                strID = DbPath.GetID10(strID);
                //********�Լ�¼�Ӷ���*************
                m_recordLockColl.LockForRead(strID, m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetXml()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼�Ӷ�����");
#endif
                try
                {
                    string strXmlFilePath = this.GetXmlFilePath(strID);

                    if (strXPath != null
                        && strXPath != "")
                    {
                        byte[] baWholeXml;
                        nRet = this.GetFileDbRecord(strXmlFilePath,
                            0,
                            -1,
                            -1,
                            strStyle,
                            out baWholeXml,
                            out strMetadata,
                            out outputTimestamp,
                            out strError);
                        if (nRet <= -1)
                            return nRet;

                        byte[] baPreamble = new byte[0];
                        string strXml = DatabaseUtil.ByteArrayToString(baWholeXml,
                            out baPreamble);

                        string strLocateXPath = "";
                        string strCreatePath = "";
                        string strNewRecordTemplate = "";
                        string strAction = "";
                        nRet = DatabaseUtil.PaseXPathParameter(strXPath,
                            out strLocateXPath,
                            out strCreatePath,
                            out strNewRecordTemplate,
                            out strAction,
                            out strError);
                        if (nRet == -1)
                            return -1;

                        XmlDocument dom = new XmlDocument();
                        dom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                        try
                        {
                            dom.LoadXml(strXml);
                        }
                        catch (Exception ex)
                        {
                            strError = "GetXml() �������ݵ�dom����ԭ��" + ex.Message;
                            return -1;
                        }

                        XmlNode node = dom.DocumentElement.SelectSingleNode(strLocateXPath);
                        if (node == null)
                        {
                            strError = "��dom��δ�ҵ�XPathΪ'" + strLocateXPath + "'�Ľڵ�";
                            return -10;
                        }


                        string strOutputText = "";
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            strOutputText = node.OuterXml;
                        }
                        else if (node.NodeType == XmlNodeType.Attribute)
                        {
                            strOutputText = node.Value;
                        }
                        else
                        {
                            strError = "ͨ��xpath '" + strXPath + "' �ҵ��Ľڵ�����Ͳ�֧�֡�";
                            return -1;
                        }
                        //string strOutputText = node.OuterXml;

                        byte[] baOutputText = DatabaseUtil.StringToByteArray(strOutputText,
                            baPreamble);

                        int nRealLength;
                        // return:
                        //		-1  ����
                        //		0   �ɹ�
                        nRet = DatabaseUtil.GetRealLength(nStart,
                            nLength,
                            baOutputText.Length,
                            nMaxLength,
                            out nRealLength,
                            out strError);
                        if (nRet == -1)
                            return -1;

                        destBuffer = new byte[nRealLength];

                        Array.Copy(baOutputText,
                            nStart,
                            destBuffer,
                            0,
                            nRealLength);

                        return 0;
                    }
                    else
                    {
                        return this.GetFileDbRecord(strXmlFilePath,
                            nStart,
                            nLength,
                            nMaxLength,
                            strStyle,
                            out destBuffer,
                            out strMetadata,
                            out outputTimestamp,
                            out strError);
                    }
                }
                finally
                {
                    //*********�Լ�¼�����************
                    m_recordLockColl.UnlockForRead(strID);
#if DEBUG_LOCK
					this.container.WriteDebugInfo("GetXml()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼�������");
#endif
                }
            }
            finally
            {
                //***********�����ݿ�����************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetXml()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        // ��ָ����Χ����Դ
        // parameter:
        //		strID	ƴ�õ���ԴID,�Ƿ��Ǹ���Դ��һ��res��׺
        //		nStart	��ʼλ��
        //		nLength	���� -1:��ʼ������
        //		destBuffer	out�����������ֽ�����
        //		timestamp	out����������ʱ���
        //		strError	out���������س�����Ϣ
        // return:
        //		-1  ����
        //		-4  ��¼������
        //		>=0 ��Դ�ܳ���
        public override int GetObject(string strRecordID,
            string strObjectID,
            int nStart,
            int nLength,
            int nMaxLength,
            string strStyle,
            out byte[] destBuffer,
            out string strMetadata,
            out byte[] timestamp,
            out string strError)
        {
            destBuffer = null;
            strMetadata = "";
            timestamp = null;
            strError = "";

            strRecordID = DbPath.GetID10(strRecordID);

            //**********�����ݿ�Ӷ���***************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetObject()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                //********�Լ�¼�Ӷ���*************
                m_recordLockColl.LockForRead(strRecordID, m_nTimeOut);
#if DEBUG_LOCK			
				this.container.WriteDebugInfo("GetObject()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼�Ӷ�����");
#endif
                try
                {

                    string strObjectFilePath = this.m_strSourceFullPath + "\\" + strRecordID + "_" + strObjectID;

                    return this.GetFileDbRecord(strObjectFilePath,
                        nStart,
                        nLength,
                        nMaxLength,
                        strStyle,
                        out destBuffer,
                        out strMetadata,
                        out timestamp,
                        out strError);
                }
                finally
                {
                    //*********�Լ�¼�����************
                    m_recordLockColl.UnlockForRead(strRecordID);
#if DEBUG_LOCK
					this.container.WriteDebugInfo("GetObject()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼�������");
#endif
                }
            }
            finally
            {
                //***********�����ݿ�����************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetObject()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }


        // ���ļ��ж�ȡ����
        // �������ļ����ļ��ⶼ�õ��ú���
        // paramter
        //		strFilePath �ļ�·��
        //		nStart      ��ʼλ��
        //		nLength     ����
        //		nMaxLength  ���Ƶ���󳤶�
        //		strStyle    ���,��data������������,��length,metadata,ʱ���,rangeȱʡ��
        //		destBuffer  out���������ص������ֽ�����
        //		strMetadata out���������ص�metadata����
        //		outputTimestamp out����������ʱ���
        //		strError    out���������س�����Ϣ
        // return:
        //		-1      ����
        //		>= 0    �ɹ�,����ʵ���ļ����ܳ���
        // ��: ����ȫ
        private int GetFileDbRecord(string strFilePath,
            int nStart,
            int nLength,
            int nMaxLength,
            string strStyle,
            out byte[] destBuffer,
            out string strMetadata,
            out byte[] outputTimestamp,
            out string strError)
        {
            destBuffer = null;
            strMetadata = "";
            outputTimestamp = null;
            strError = "";

            int nTotalLength = 0;
            FileInfo file = new FileInfo(strFilePath);
            if (file.Exists == false)
            {
                strError = "�ļ�'" + strFilePath + "'������";
                return -1;
            }

            // 1.ȡʱ���
            if (StringUtil.IsInList("timestamp", strStyle) == true)
            {
                string strTimestampFileName = DatabaseUtil.GetTimestampFileName(strFilePath);
                if (File.Exists(strTimestampFileName) == true)
                {
                    string strOutputTimestamp = FileUtil.File2StringE(strTimestampFileName);
                    outputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTimestamp);
                }
            }

            // 2.ȡԪ����
            if (StringUtil.IsInList("metadata", strStyle) == true)
            {
                string strMetadataFileName = DatabaseUtil.GetMetadataFileName(strFilePath);
                if (File.Exists(strMetadataFileName) == true)
                {
                    strMetadata = FileUtil.File2StringE(strMetadataFileName);
                }
            }

            // 3.ȡrange
            if (StringUtil.IsInList("range", strStyle) == true)
            {
                string strRangeFileName = DatabaseUtil.GetRangeFileName(strFilePath);
                if (File.Exists(strRangeFileName) == true)
                {
                    string strRange = FileUtil.File2StringE(strRangeFileName);
                }
            }

            // 4.����
            nTotalLength = (int)file.Length;

            // 5.��data���ʱ,�Ż�ȡ����
            if (StringUtil.IsInList("data", strStyle) == true)
            {
                if (nLength == 0)  // ȡ0����
                {
                    destBuffer = new byte[0];
                    return nTotalLength;
                }
                // ��鷶Χ�Ƿ�Ϸ�
                int nOutputLength;
                // return:
                //		-1  ����
                //		0   �ɹ�
                int nRet = DatabaseUtil.GetRealLength(nStart,
                    nLength,
                    nTotalLength,
                    nMaxLength,
                    out nOutputLength,
                    out strError);
                if (nRet == -1)
                    return -1;

                FileStream s = new FileStream(strFilePath,
                    FileMode.Open);
                try
                {
                    destBuffer = new byte[nOutputLength];
                    s.Seek(nStart, SeekOrigin.Begin);
                    s.Read(destBuffer,
                        0,
                        nOutputLength);
                }
                finally
                {
                    s.Close();
                }
            }
            return nTotalLength;
        }


        // дxml����
        // parameter:
        //		strID	        ��¼ID -1:��ʾ׷��һ����¼
        //		strRanges	    Ŀ���λ��,���range�ö��ŷָ�
        //		nTotalLength	�ܳ���
        //		inputTimestamp	�����ʱ���
        //		outputTimestamp	out���������ص�ʱ���
        //		strOutputID	    out���������صļ�¼ID,��strID == -1ʱ,�õ�ʵ�ʵ�ID
        //		strError	    out���������س�����Ϣ
        // return:
        // return:
        //		-1  ����
        //		-2  ʱ�����ƥ��
        //      -4  ��¼������
        //      -6  Ȩ�޲���
        //		0   �ɹ�
        // ??? AddInteger+,+AddInteger,Push����û��ʵ��
        public override int WriteXml(User oUser,
            string strID,
            string strXPath,
            string strRanges,
            long lTotalLength,
            byte[] baSource,
            Stream streamSource,
            string strMetadata,
            string strStyle,
            byte[] inputTimestamp,
            out byte[] outputTimestamp,
            out string strOutputID,
            out string strOutputValue,
            bool bCheckAccount,
            out string strError)
        {
            outputTimestamp = null;
            strOutputID = "";
            strOutputValue = "";
            strError = "";

            if (strID == "?")
                strID = "-1";

            // ȷ��ID,���Ҹ�����ֵ��ֵ
            bool bPushTailNo = false;
            strID = this.EnsureID(strID,
                out bPushTailNo);
            if (oUser != null)
            {
                string strTempRecordPath = this.GetCaption("zh-cn") + "/" + strID;
                if (bPushTailNo == true)
                {
                    string strExistRights = "";
                    bool bHasRight = oUser.HasRights(strTempRecordPath,
                        ResType.Record,
                        "create",
                        out strExistRights);
                    if (bHasRight == false)
                    {
                        strError = "�����ʻ���Ϊ'" + oUser.Name + "'����'" + strTempRecordPath + "'��¼û��'����(create)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                        return -6;
                    }
                }
                else
                {
                    string strExistRights = "";
                    bool bHasRight = oUser.HasRights(strTempRecordPath,
                        ResType.Record,
                        "overwrite",
                        out strExistRights);
                    if (bHasRight == false)
                    {
                        strError = "�����ʻ���Ϊ'" + oUser.Name + "'����'" + strTempRecordPath + "'��¼û��'����(overwrite)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                        return -6;
                    }
                }
            }

            strOutputID = DbPath.GetCompressedID(strID);
            int nRet;
            int nFull = -1;

            //***********�����ݿ�Ӷ���***********
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("WriteXml()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                strID = DbPath.GetID10(strID);
                //**********�Լ�¼��д��***************
                m_recordLockColl.LockForWrite(strID, m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("WriteXml()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼��д����");
#endif
                try
                {



                    string strXmlFilePath = this.GetXmlFilePath(strID);
                    bool bExist = File.Exists(strXmlFilePath);
                    if (bExist == false)
                    {
                        //�������ļ�,���Ѹ�����Ϣ������
                        this.InsertRecord(strID,
                            out inputTimestamp);
                        // ���������һ���ֽڣ�������Ϣ������
                    }

                    nRet = this.WriteFileDbTempRecord(strXmlFilePath,
                        strRanges,
                        lTotalLength,
                        baSource,
                        streamSource,
                        strMetadata,
                        strStyle,
                        inputTimestamp,
                        out outputTimestamp,
                        out nFull,
                        out strError);
                    if (nRet <= -1)
                        return nRet;

                    if (nFull == 1)  // �ļ�����
                    {
                        // 1.�õ��¾ɼ�����
                        string strNewFileName = DatabaseUtil.GetNewFileName(strXmlFilePath);
                        string strNewXml = FileUtil.File2StringE(strNewFileName);
                        string strOldXml = FileUtil.File2StringE(strXmlFilePath);

                        if (strXPath != null
                            && strXPath != "")
                        {
                            string strLocateXPath = "";
                            string strCreatePath = "";
                            string strNewRecordTemplate = "";
                            string strAction = "";
                            nRet = DatabaseUtil.PaseXPathParameter(strXPath,
                                out strLocateXPath,
                                out strCreatePath,
                                out strNewRecordTemplate,
                                out strAction,
                                out strError);
                            if (nRet == -1)
                                return -1;

                            XmlDocument tempDom = new XmlDocument();
                            tempDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue
                            try
                            {
                                if (strOldXml == "")
                                {
                                    if (strNewRecordTemplate == "")
                                        tempDom.LoadXml("<root/>");
                                    else
                                        tempDom.LoadXml(strNewRecordTemplate);
                                }
                                else
                                    tempDom.LoadXml(strOldXml);
                            }
                            catch (Exception ex)
                            {
                                strError = "WriteXml() �ڸ�'" + this.GetCaption("zh-cn") + "'��д���¼'" + strID + "'ʱ��װ�ؾɼ�¼��dom����,ԭ��:" + ex.Message;
                                return -1;
                            }

                            if (strLocateXPath == "")
                            {
                                strError = "xpath���ʽ�е�locate��������Ϊ��ֵ";
                                return -1;
                            }

                            // ͨ��strLocateXPath��λ��ָ���Ľڵ�
                            XmlNode node = null;
                            try
                            {
                                node = tempDom.DocumentElement.SelectSingleNode(strLocateXPath);
                            }
                            catch (Exception ex)
                            {
                                strError = "WriteXml() �ڸ�'" + this.GetCaption("zh-cn") + "'��д���¼'" + strID + "'ʱ��XPathʽ��'" + strXPath + "'ѡ��Ԫ��ʱ����,ԭ��:" + ex.Message;
                                return -1;
                            }

                            if (node == null)
                            {
                                if (strLocateXPath == "")
                                {
                                    strError = "xpath���ʽ�е�create��������Ϊ��ֵ";
                                    return -1;
                                }
                                node = DomUtil.CreateNodeByPath(tempDom.DocumentElement,
                                    strCreatePath);
                                if (node == null)
                                {
                                    strError = "�ڲ�����!";
                                    return -1;
                                }
                            }

                            //Create a document fragment.
                            XmlDocumentFragment docFrag = tempDom.CreateDocumentFragment();

                            //Set the contents of the document fragment.
                            docFrag.InnerXml = strNewXml;

                            //Add the children of the document fragment to the
                            //original document.
                            node.ParentNode.InsertBefore(docFrag, node);


                            if (strAction == "AddInteger"
                                || strAction == "AppendString")
                            {
                                XmlNode newNode = node.PreviousSibling;
                                if (newNode == null)
                                {
                                    strError = "newNode������Ϊnull";
                                    return -1;
                                }

                                string strNewValue = newNode.InnerText;
                                string strOldValue = DomUtil.GetNodeText(node);
                                if (strAction == "AddInteger")
                                {

                                    int nNumber = 0;
                                    try
                                    {
                                        nNumber = Convert.ToInt32(strNewValue);
                                    }
                                    catch (Exception ex)
                                    {
                                        strError = "���������'" + strNewXml + "'�������ָ�ʽ��" + ex.Message;
                                        return -1;
                                    }

                                    string strLastValue;
                                    nRet = StringUtil.IncreaseNumber(strOldValue,
                                        nNumber,
                                        out strLastValue,
                                        out strError);
                                    if (nRet == -1)
                                        return -1;

                                    newNode.InnerText = strLastValue;
                                    strOutputValue = newNode.OuterXml;
                                }
                                else if (strAction == "AppendString")
                                {
                                    newNode.InnerText = strOldValue + strNewValue;
                                    strOutputValue = newNode.OuterXml;
                                }
                            }

                            node.ParentNode.RemoveChild(node);

                            strNewXml = tempDom.OuterXml;
                        }


                        KeyCollection newKeys = null;
                        KeyCollection oldKeys = null;
                        XmlDocument newDom = null;
                        XmlDocument oldDom = null;

                        // return:
                        //      -1  ����
                        //      0   �ɹ�
                        nRet = this.MergeKeys(strID,
                            strNewXml,
                            strOldXml,
                            true,
                            out newKeys,
                            out oldKeys,
                            out newDom,
                            out oldDom,
                            out strError);
                        if (nRet == -1)
                            return -1;

                        this.AddKeys(newKeys);
                        this.DeleteKeys(oldKeys);

                        // 3.�������ļ�
                        nRet = this.ProcessFiles(strID,
                            newDom,
                            oldDom,
                            out strError);
                        if (nRet <= -1)
                            return nRet;

                        // 4.��newdata�滻data
                        // �Ȱ�xml���ݸ����ˣ��ٸ��¼�����
                        if (strXPath != null
                            && strXPath != "")
                        {
                            FileUtil.String2File(strNewXml,
                                strXmlFilePath);
                        }
                        else
                        {
                            File.Copy(strNewFileName,
                                strXmlFilePath,
                                true);
                        }

                        // 5.ɾ��newdata�ֶ�
                        File.Delete(strNewFileName);

                    }
                }
                catch (Exception ex)
                {
                    strError = "WriteXml() �ڸ�'" + this.GetCaption("zh-cn") + "'��д���¼'" + strID + "'ʱ����,ԭ��:" + ex.Message;
                    return -1;
                }
                finally
                {
                    //*********�Լ�¼��д��****************************
                    m_recordLockColl.UnlockForWrite(strID);
#if DEBUG_LOCK
					this.container.WriteDebugInfo("WriteXml()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼��д����");
#endif
                }
            }
            finally
            {
                //**********�����ݿ�����**************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("WriteXml()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }

            // ������������֪Ϊ�˻����д��������ʱ, һ��Ҫ��bCheckAccount==false
            // �����ã������������𲻱�Ҫ�ĵݹ�
            if (nFull == 1
                && bCheckAccount == true
                && StringUtil.IsInList("account", this.TypeSafety) == true)
            {
                string strResPath = this.FullID + "/" + strID;
                this.container.UserColl.RefreshUserSafety(
                    strResPath);
            }
            return 0;
        }

        // д����
        //		strRecordID	��¼ID
        //		strObjectID	��ԴID
        //		strRanges	��Χ
        //		nTotalLength	�ܳ���
        //		sourceBuffer	Դ����
        //		strMetadata	Ԫ����
        //		strStyle	��ʽ
        //		inputTimestamp	�����ʱ���
        //		outputTimestamp	out���������ص�ʱ���
        // return:
        //		-1  ����
        //		-2  ʱ�����ƥ��
        //      -4  ��¼�������Դ������
        //      -6  Ȩ�޲���
        //		0   �ɹ�
        public override int WriteObject(User user,
            string strRecordID,
            string strObjectID,
            string strRanges,
            long lTotalLength,
            byte[] baSource,
            Stream streamSource,
            string strMetadata,
            string strStyle,
            byte[] inputTimestamp,
            out byte[] outputTimestamp,
            out string strError)
        {
            outputTimestamp = null;
            strError = "";
            int nRet = 0;

            if (user != null)
            {
                string strTempRecordPath = this.GetCaption("zh-cn") + "/" + strRecordID;
                string strExistRights = "";
                bool bHasRight = user.HasRights(strTempRecordPath,
                    ResType.Record,
                    "overwrite",
                    out strExistRights);
                if (bHasRight == false)
                {
                    strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strTempRecordPath + "'��¼û��'����(overwrite)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                    return -6;
                }
            }

            //**********�����ݿ�Ӷ���************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("WriteObject()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                string strOutputRecordID = "";
                // return:
                //      -1  ����
                //      0   �ɹ�
                nRet = this.CanonicalizeRecordID(strRecordID,
                    out strOutputRecordID,
                    out strError);
                if (nRet == -1)
                    return -1;
                if (strOutputRecordID == "-1")
                {
                    strError = "���������Դ��֧�ּ�¼�Ų���ֵΪ'" + strRecordID + "'��";
                    return -1;
                }
                strRecordID = strOutputRecordID;

                //**********�Լ�¼��д��***************
                m_recordLockColl.LockForWrite(strRecordID, m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("WriteObject()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼��д����");
#endif
                try
                {
                    //////////////////////////////////////////////
                    // 1.�ڶ�Ӧ��xml���ݣ��ö���·���ҵ�����ID
                    ///////////////////////////////////////////////
                    string strXmlFilePath = this.GetXmlFilePath(strRecordID);

                    XmlDocument xmlDom = new XmlDocument();
                    xmlDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                    xmlDom.Load(strXmlFilePath);

                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDom.NameTable);
                    nsmgr.AddNamespace("dprms", DpNs.dprms);
                    XmlNode fileNode = xmlDom.DocumentElement.SelectSingleNode("//dprms:file[@id='" + strObjectID + "']", nsmgr);
                    if (fileNode == null)
                    {
                        strError = "������xml��û���ҵ���ID��Ӧ��dprms:file�ڵ�";
                        return -1;
                    }

                    string strObjectFilePath = this.GetObjectFileName(strRecordID,
                        strObjectID);
                    if (File.Exists(strObjectFilePath) == false)
                    {
                        strError = "����������:��Դ��¼'" + strObjectFilePath + "'������,�����ܵ����";
                        return -1;
                    }
                    string strNewObjectFileName = DatabaseUtil.GetNewFileName(strObjectFilePath);
                    if (File.Exists(strNewObjectFileName) == false)
                    {
                        this.UpdateObject(strObjectFilePath,
                            out inputTimestamp);
                        // Updata��,��¼��ʱ�ļ���һ���ֽ�,������Ϣ��������.
                    }

                    int nFull;
                    nRet = this.WriteFileDbTempRecord(strObjectFilePath,
                        strRanges,
                        lTotalLength,
                        baSource,
                        streamSource,
                        strMetadata,
                        strStyle,
                        inputTimestamp,
                        out outputTimestamp,
                        out nFull,
                        out strError);
                    if (nRet <= -1)
                        return nRet;

                    if (nFull == 1)  //��������
                    {
                        // 1. �滻data�ֶ�
                        File.Copy(strNewObjectFileName,
                            strObjectFilePath,
                            true);

                        // 2. ɾ��newdata�ֶ�
                        File.Delete(strNewObjectFileName);

                    }
                }
                catch (Exception ex)
                {
                    strError = "WriteXml() �ڸ�'" + this.GetCaption("zh-cn") + "'��д����Դ'" + strRecordID + "_" + strObjectID + "'ʱ����,ԭ��:" + ex.Message;
                    return -1;
                }
                finally
                {
                    //********�Լ�¼��д��****************************
                    m_recordLockColl.UnlockForWrite(strRecordID);
#if DEBUG_LOCK
					this.container.WriteDebugInfo("WriteObject()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼��д����");
#endif
                }
            }
            finally
            {
                //*******�����ݿ�����****************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK

				this.container.WriteDebugInfo("WriteObject()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }

            return 0;
        }



        // �������ڳ����뷶Χ�������������Ƿ�Ϸ�
        // parameters:
        //		lTotalLength	��Դ�ܳ���
        //		strRanges		��Χ�������淶��
        //		lSourceLength	���δ������ݵĳ���
        //		strError		out���������س�����Ϣ
        // reutrn:
        //		-1	����
        //		-2	Ŀǰ���������Դ��״̬
        //		1	��Դ�����ÿ�
        //		0	��Դ�������κ��޸�
        public int CheckParamsAboutLengthAndRanges(long lTotalLength,
            ref string strRanges,
            long lSourceLength,
            out string strError)
        {
            strError = "";

            if (strRanges == null)
                strRanges = "";

            if (lTotalLength < 0)
            {
                strError = "CheckParamsAboutLengthAndRanges()���ô���lTotalLength��������С��0��";
                return -1;
            }
            if (lSourceLength < 0)
            {
                strError = "CheckParamsAboutLengthAndRanges()���ô���lSourceLength��������С��0��";
                return -1;
            }



            if (lTotalLength == 0)
            {
                if (strRanges != "")
                {
                    strError = "CheckParamsAboutLengthAndRanges()���ô��󣬵�lTotalLength == 0ʱ��strRanges����ֻ��Ϊnull����ַ�����";
                    return -1;
                }
                if (lSourceLength != 0)
                {
                    strError = "CheckParamsAboutLengthAndRanges()���ô��󣬵�lTotalLength == 0ʱ��lSourceLength����ֻ��Ϊ0��";
                    return -1;
                }

                // ��Դ�����ÿա�
                return 1;
            }

            if (lTotalLength > 0)
            {
                if (lSourceLength == 0)
                {
                    if (strRanges != "")
                    {
                        strError = "CheckParamsAboutLengthAndRanges()���ô��󣬵�lTotalLength == 0ʱ ��lSourceLength == 0����ôstrRanges����ֻ��Ϊnull����ַ�����";
                        return -1;
                    }
                    else
                    {
                        //���ԭ��Դ�����κ��޸ģ�����0
                        return 0;
                    }
                }
                else
                {
                    if (strRanges == "")
                        strRanges = "0-" + Convert.ToString(lSourceLength - 1);

                    return -2;
                }
            }

            return -2;
        }

        // ���ļ����еļ�¼��ʱ��ɾ���������ļ���
        public void DeleteFuZhuFilesWhenFull(string strFilePath)
        {
            string strNewFilePath = DatabaseUtil.GetNewFileName(strFilePath);
            if (File.Exists(strNewFilePath) == true)
                File.Delete(strNewFilePath);

            string strRangeFilePath = DatabaseUtil.GetRangeFileName(strFilePath);
            if (File.Exists(strRangeFilePath) == true)
                File.Delete(strRangeFilePath);
        }


        // д�ļ������ʱ��¼�ļ��ͼ�¼��Ϣ��������xml��¼�壬Ҳ�����Ƕ�����Դ�ļ�
        // parameters:
        //		strFilePath	��¼�ļ�����·��
        //		strRanges	Ŀ�귶Χ
        //		nTotalLength	��Դ�ܳ���
        //		baSource	�����ֽ�����
        //		streamSource	������
        //		strStyle	���
        //		baInputTimestamp	�����ʱ���
        //		baOutputTimestamp	out���������ص�ʱ���
        //		bFull	out���������ؼ�¼�Ƿ�����
        //		strError	out���������س�����Ϣ
        // return:
        //		<=-1	����
        //		-2	ʱ�����ƥ��
        //		0	�ɹ�
        // ��: ����ȫ
        //��ס����д���ݿ�ļ�¼��������д��ʱ�ֶΣ�����ʱ�����滻��ʵ�ʵ��ֶ�
        private int WriteFileDbTempRecord(string strFilePath,
            string strRanges,
            long lTotalLength,
            byte[] baSource,
            Stream streamSource,
            string strMetadata,
            string strStyle,
            byte[] baInputTimestamp,
            out byte[] baOutputTimestamp,
            out int nFull,
            out string strError)
        {
            nFull = -1;
            baOutputTimestamp = null;
            strError = "";

            // --------------------------------------------------
            // ������һ���������
            // --------------------------------------------------
            if (strFilePath == null || strFilePath == "")
            {
                strError = "WriteFileDbRecord()���ô���strFilePath��������Ϊnull����ַ�����";
                return -1;
            }
            if (baSource == null && streamSource == null)
            {
                strError = "WriteFileDbRecord()���ô���baSource������streamSource��������ͬʱΪnull��";
                return -1;
            }
            if (baSource != null && streamSource != null)
            {
                strError = "WriteFileDbRecord()���ô���baSource������streamSource����ֻ����һ������ֵ��";
                return -1;
            }
            if (lTotalLength < 0)
            {
                strError = "WriteFileDbRecord()���ô���nTotalLength����������ڵ���0��";
                return -1;
            }


            // --------------------------------------------------
            // ��ʼ������
            // --------------------------------------------------

            if (File.Exists(strFilePath) == false)
            {
                strError = "�ļ���'" + this.GetCaption("zh-cn") + "'�ļ�¼�ļ�'" + strFilePath + "'�����ڣ������ܵ������";
                return -1;
            }
            string strTimestampFileName = DatabaseUtil.GetTimestampFileName(strFilePath);
            if (File.Exists(strTimestampFileName) == false)
            {
                strError = "�ļ���'" + this.GetCaption("zh-cn") + "'�ļ�¼�ļ�'" + strFilePath + "'��Ӧ��ʱ����ļ�'" + strTimestampFileName + "'�����ڣ������ܵ������";
                return -1;
            }

            if (StringUtil.IsInList("ignorechecktimestamp", strStyle) == false)
            {
                string strOldTimestamp = FileUtil.File2StringE(strTimestampFileName);
                baOutputTimestamp = ByteArray.GetTimeStampByteArray(strOldTimestamp);
                if (ByteArray.Compare(baOutputTimestamp, baInputTimestamp) != 0)
                {
                    strError = "ʱ�����ƥ��";
                    return -2;
                }
            }



            // д����

            int nRet = 0;

            // ��ǰ��Դ�ĳߴ�
            //	-1	��ʾδ֪
            //	-2	��ʾ����
            long lCurrentLength = 0;

            string strNewFileName = DatabaseUtil.GetNewFileName(strFilePath);
            string strRangeFileName = DatabaseUtil.GetRangeFileName(strFilePath);


            long lSourceTotalLength = 0;
            if (baSource != null)
                lSourceTotalLength = baSource.Length;
            else
                lSourceTotalLength = streamSource.Length;

            // reutrn:
            //		-1	����
            //		-2	Ŀǰ���������Դ��״̬
            //		1	��Դ�����ÿ�
            //		0	��Դ�������κ��޸�
            nRet = this.CheckParamsAboutLengthAndRanges(lTotalLength,
                ref strRanges,
                lSourceTotalLength,
                out strError);
            if (nRet == -1)
                return -1;
            if (nRet == 1)
            {
                // ��Դ�����ÿ�
                Stream s = File.Create(strNewFileName);
                s.Close();

                nFull = 1;
                lCurrentLength = 0;

                if (File.Exists(strRangeFileName) == true)
                    File.Delete(strRangeFileName);

                goto END1;
            }
            if (nRet == 0)
            {
                nFull = -1;
                lCurrentLength = -2;
                goto END1;
            }






            RangeList rangeList = new RangeList(strRanges);

            Stream target = File.Open(strNewFileName,
                FileMode.OpenOrCreate);
            try
            {
                int nStartOfSource = 0;
                for (int i = 0; i < rangeList.Count; i++)
                {
                    RangeItem range = (RangeItem)rangeList[i];
                    int nStartOfTarget = (int)range.lStart;
                    int nLength = (int)range.lLength;

                    // �ƶ�Ŀ������ָ�뵽ָ��λ��
                    target.Seek(nStartOfTarget, SeekOrigin.Begin);
                    if (baSource != null)
                    {
                        target.Write(baSource,
                            nStartOfSource,
                            nLength);

                        nStartOfSource += nLength; //2005.11.14 add
                    }
                    else
                    {
                        StreamUtil.DumpStream(streamSource,
                            target,
                            nLength);
                    }
                }
            }
            finally
            {
                target.Close();
            }


            string strOldRanges = "";
            if (File.Exists(strRangeFileName) == true)
                strOldRanges = FileUtil.File2StringE(strRangeFileName);

            string strResultRanges = "";
            int nState = RangeList.MergContentRangeString(strRanges,
                strOldRanges,
                lTotalLength,
                out strResultRanges);
            if (nState == -1)
            {
                strError = "MergContentRangeString() error";
                return -1;
            }
            if (nState == 1)
            {
                nFull = 1;
                lCurrentLength = lTotalLength;

                Stream s = File.Open(strNewFileName, FileMode.Open);
                s.SetLength(lTotalLength);
                s.Close();

                if (File.Exists(strRangeFileName) == true)
                    File.Delete(strRangeFileName);
            }
            else
            {
                nFull = 0;
                lCurrentLength = -1;  //��ǰ�ߴ�δ֪�����ǿ���֪����

                FileUtil.String2File(strResultRanges,
                    strRangeFileName);
            }


        END1:

            // дmetadata
            if (strMetadata == null || strMetadata == "")
                strMetadata = "<file/>";

            string strMetadataFileName = DatabaseUtil.GetMetadataFileName(strFilePath);

            // ȡ���ɵ����ݽ��кϲ�
            string strOldMetadata = "";
            if (File.Exists(strMetadataFileName) == true)
                strOldMetadata = FileUtil.File2StringE(strMetadataFileName);

            if (strOldMetadata == "")
                strOldMetadata = "<file/>";

            string strResultMetadata;
            // return:
            //		-1	����
            //		0	�ɹ�
            nRet = DatabaseUtil.MergeMetadata(strOldMetadata,
                strMetadata,
                lCurrentLength,
                out strResultMetadata,
                out strError);
            if (nRet == -1)
                return -1;

            // �Ѻϲ���������д���ļ���
            FileUtil.String2File(strResultMetadata,
                strMetadataFileName);

            string strOutputTime = this.CreateTimestampForDb();
            FileUtil.String2File(strOutputTime,
                strTimestampFileName);
            baOutputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTime);


            return 0;
        }



        // ɾ������ļ���Ϣ�ĸ����ļ�
        // ��: ����ȫ
        public void DeleteFuZhuFiles(string strFilePath)
        {
            // 1. ɾ��range�ֶ��ļ�
            string strRangeFileName = DatabaseUtil.GetRangeFileName(strFilePath);
            if (File.Exists(strRangeFileName) == true)
                File.Delete(strRangeFileName);

            // 2. ɾ��metadata�ֶ��ļ�
            string strMetadataFileName = DatabaseUtil.GetMetadataFileName(strFilePath);
            if (File.Exists(strMetadataFileName) == true)
                File.Delete(strMetadataFileName);

            // 3. ɾ��timestamp�ֶ��ļ�
            string strTimestampFileName = DatabaseUtil.GetTimestampFileName(strFilePath);
            if (File.Exists(strTimestampFileName) == true)
                File.Delete(strTimestampFileName);

            // 4. ɾ����ʱ�����ļ�
            string strNewFileName = DatabaseUtil.GetNewFileName(strFilePath);
            if (File.Exists(strNewFileName) == true)
                File.Delete(strNewFileName);

        }

        public void InsertRecord(string strRecordID,
            out byte[] outputTimestamp)
        {
            outputTimestamp = null;
            string strXmlFilePath = this.GetXmlFilePath(strRecordID);

            // �˴�����дfinally
            Stream file = File.Create(strXmlFilePath);
            file.Close();

            // new�ֶ�
            string strNewFileName = DatabaseUtil.GetNewFileName(strXmlFilePath);
            Stream s = File.Create(strNewFileName);
            try
            {
                s.Write(new byte[] { 0x0 },
                    0,
                    1);
            }
            finally
            {
                s.Close();
            }

            // timeatamp
            string strTimestampFileName = DatabaseUtil.GetTimestampFileName(strXmlFilePath);
            string strTimestamp = this.CreateTimestampForDb();
            FileUtil.String2File(strTimestamp, strTimestampFileName);
            outputTimestamp = ByteArray.GetTimeStampByteArray(strTimestamp);

            // range
            string strRangeFileName = DatabaseUtil.GetRangeFileName(strXmlFilePath);
            FileUtil.String2File("0-0", strRangeFileName);

            // metadata
            string strMetadataFileName = DatabaseUtil.GetMetadataFileName(strXmlFilePath);
            FileUtil.String2File("<file size='0'/>", strMetadataFileName);
        }

        public void UpdateObject(string strObjectPath,
            out byte[] outputTimestamp)
        {
            outputTimestamp = null;

            // new�ֶ�
            string strNewFileName = DatabaseUtil.GetNewFileName(strObjectPath);
            Stream s = File.Create(strNewFileName);
            try
            {
                s.Write(new byte[] { 0x0 },
                    0,
                    1);
            }
            finally
            {
                s.Close();
            }

            // timeatamp
            string strTimestampFileName = DatabaseUtil.GetTimestampFileName(strObjectPath);
            string strTimestamp = this.CreateTimestampForDb();
            FileUtil.String2File(strTimestamp, strTimestampFileName);
            outputTimestamp = ByteArray.GetTimeStampByteArray(strTimestamp);

            // range
            string strRangeFileName = DatabaseUtil.GetRangeFileName(strObjectPath);
            FileUtil.String2File("0-0", strRangeFileName);

            // metadata
            string strMetadataFileName = DatabaseUtil.GetMetadataFileName(strObjectPath);
            FileUtil.String2File("<file size='0'/>", strMetadataFileName);
        }

        // �õ���Դ����
        public string GetObjectFileName(string strRecordID,
            string strObjectID)
        {
            return this.m_strSourceFullPath + "\\"
                + strRecordID
                + "_" + strObjectID;
        }

        // �õ���¼ID��Ӧ���ļ���
        // parameters:
        //      strRecordID ��¼��
        public string GetXmlFilePath(string strRecordID)
        {
            return this.m_strSourceFullPath + "\\"
                + this.RecordID2XmlFileName(strRecordID);
        }

        // �����¼�����
        public void AddKeys(KeyCollection keys)
        {
            foreach (KeyItem oneKey in keys)
            {
                string strTablePath;
                strTablePath = this.TableName2TableFileName(oneKey.SqlTableName);
                XmlDocument domTable = new XmlDocument();
                domTable.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                domTable.Load(strTablePath);

                //�½�key�ڵ�
                XmlNode nodeKey = domTable.CreateElement("key");

                XmlNode nodeKeystring = domTable.CreateElement("keystring");
                DomUtil.SetNodeText(nodeKeystring,
                    oneKey.Key);
                nodeKey.AppendChild(nodeKeystring);

                XmlNode nodeFromstring = domTable.CreateElement("fromstring");
                DomUtil.SetNodeText(nodeFromstring,
                    oneKey.FromValue);
                nodeKey.AppendChild(nodeFromstring);

                XmlNode nodeIdstring = domTable.CreateElement("idstring");
                DomUtil.SetNodeText(nodeIdstring,
                    oneKey.RecordID);
                nodeKey.AppendChild(nodeIdstring);

                XmlNode nodeKeystringnum = domTable.CreateElement("keystringnum");
                DomUtil.SetNodeText(nodeKeystringnum,
                    oneKey.Num);
                nodeKey.AppendChild(nodeKeystringnum);

                domTable.DocumentElement.AppendChild(nodeKey);
                domTable.Save(strTablePath);
            }
        }

        // ɾ���ɼ�����
        public void DeleteKeys(KeyCollection keys)
        {
            foreach (KeyItem oneKey in keys)
            {
                string strTablePath;
                strTablePath = this.TableName2TableFileName(oneKey.SqlTableName);
                XmlDocument domTable = new XmlDocument();
                domTable.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                domTable.Load(strTablePath);

                string strXpath = "/root/key[keystring='" + oneKey.Key + "' and fromstring='" + oneKey.FromValue + "' and idstring='" + oneKey.RecordID + "']";
                XmlNode nodeKey = domTable.SelectSingleNode(strXpath);
                if (nodeKey != null)
                {
                    domTable.DocumentElement.RemoveChild(nodeKey);
                }
                else
                {
                    throw (new Exception("����xpath'" + strXpath + "'û�ҵ��ڵ�,�����ܵ����!"));
                }
                domTable.Save(strTablePath);
            }
        }

        // �������ļ�
        public int ProcessFiles(string strRecordID,
            XmlDocument newDom,
            XmlDocument oldDom,
            out string strError)
        {
            strError = "";

            // �������ļ�
            ArrayList aNewFileID = new ArrayList();
            if (newDom != null)
            {
                XmlNamespaceManager newNsmgr = new XmlNamespaceManager(newDom.NameTable);
                newNsmgr.AddNamespace("dprms", DpNs.dprms);
                XmlNodeList newFileList = newDom.SelectNodes("//dprms:file", newNsmgr);
                foreach (XmlNode newFileNode in newFileList)
                {
                    string strNewFileID = DomUtil.GetAttr(newFileNode,
                        "id");
                    if (strNewFileID != "")
                        aNewFileID.Add(strNewFileID);
                }
            }

            ArrayList aOldFileID = new ArrayList();
            if (oldDom != null)
            {
                XmlNamespaceManager oldNsmgr = new XmlNamespaceManager(oldDom.NameTable);
                oldNsmgr.AddNamespace("dprms", DpNs.dprms);
                XmlNodeList oldFileList = oldDom.SelectNodes("//dprms:file", oldNsmgr);
                foreach (XmlNode oldFileNode in oldFileList)
                {
                    string strOldFileID = DomUtil.GetAttr(oldFileNode,
                        "id");
                    if (strOldFileID != "")
                        aOldFileID.Add(strOldFileID);
                }
            }
            //���ݱ���������
            aNewFileID.Sort(new ComparerClass());
            aOldFileID.Sort(new ComparerClass());

            List<string> targetLeft = new List<string>();
            List<string> targetMiddle = new List<string>();
            List<string> targetRight = new List<string>();

            //�¾�����File������
            ArrayListUtil.MergeStringArray(aNewFileID,
                aOldFileID,
                targetLeft,
                targetMiddle,
                targetRight);

            //ɾ������ľ��ļ�
            if (targetRight.Count > 0)
            {
                foreach (string strNeedDeleteFileID in targetRight)
                {
                    string strFilePath = this.GetObjectFileName(strRecordID,
                        strNeedDeleteFileID);
                    File.Delete(strFilePath);
                    this.DeleteFuZhuFiles(strFilePath);
                }
            }

            // �������ļ��ļ�
            if (targetLeft.Count > 0)
            {
                foreach (string strNewFileID in targetLeft)
                {
                    string strObjectPath = this.GetObjectFileName(strRecordID,
                        strNewFileID);
                    int nRet = this.InsertEmptyObject(strObjectPath,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }
            }
            return 0;
        }



        // �����в���һ����Դ����
        // return
        //		-1  ����
        //		0   �ɹ�
        private int InsertEmptyObject(string strObjectPath,
            out string strError)
        {
            strError = "";
            Stream s = File.Create(strObjectPath);
            s.Close();
            return 0;
        }



        // ��������������ļ���
        public string TableName2TableFileName(string strKeyName)
        {
            strKeyName = strKeyName.Trim();
            return this.m_strSourceFullPath + "\\" + strKeyName + ".xml";
        }
        // ���ݼ�¼ID�õ�xml�ļ���
        // �����¼�Ų���10,�Ὣ��¼�ű��10λ
        // parameter:
        //		strID   ��¼ID
        // return:
        //		xml�ļ���
        private string RecordID2XmlFileName(string strID)
        {
            strID = strID.Trim();
            strID = DbPath.GetID10(strID);
            return strID + ".xml";
        }

        // ��Xml�ļ��� �� ��¼ID
        // parameter:
        //		strFileName �ļ���
        // return:
        //		��¼ID,����10���10��
        private string XmlFileName2RecordID(string strFileName)
        {
            string strEx = Path.GetExtension(strFileName);
            if (strEx.Length > 1)
                strEx = strEx.Substring(1);
            strEx = strEx.ToUpper();
            if (strEx != "XML")
            {
                throw (new Exception("���ļ����Ǽ�¼����"));
            }

            int nPosition = strFileName.LastIndexOf(".");
            string strRecordID = "";
            if (nPosition >= 0)
                strRecordID = strFileName.Substring(0, nPosition);
            else
                strRecordID = strFileName;
            strRecordID = DbPath.GetID10(strRecordID);
            return strRecordID;
        }


        // ��ͨɾ����¼
        // paramter:
        //		strID       ��¼ID
        //		strError    out���������س�����Ϣ
        // return:
        //		-1  һ���Դ���
        //		-2  ʱ�����ƥ��
        //      -4  δ�ҵ���¼
        //		0   �ɹ�
        //��: ��ȫ��
        //ΪʲôҪ��д������Ϊ��ɾ����¼�Լ���Ӧ�ļ����㣬�����ʱ�ڣ��ü�¼�����ܶ�Ҳ��д�����Լ�д��
        public override int DeleteRecord(string strID,
            byte[] inputTimestamp,
            out byte[] outputTimestamp,
            out string strError)
        {
            outputTimestamp = null;
            strError = "";

            strID = DbPath.GetID10(strID);

            int nRet;

            //*********�����ݿ�Ӷ���**************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("DeleteRecord()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                //***********�Ӽ�¼д��**********
                m_recordLockColl.LockForWrite(strID, m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("DeleteRecord()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼��д����");
#endif
                try
                {
                    strID = DbPath.GetID10(strID);

                    string strXmlFilePath = this.GetXmlFilePath(strID);

                    //�Ƚ�ʱ���
                    //outputTimestamp = this.GetTimestampByFile(strXmlFilePath);
                    string strTimestampFileName = DatabaseUtil.GetTimestampFileName(strXmlFilePath);
                    if (File.Exists(strTimestampFileName) == true)
                    {
                        string strOutputTimestamp = FileUtil.File2StringE(strTimestampFileName);
                        outputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTimestamp);
                    }
                    else
                    {
                        strError = "������û��ʱ����ļ�";
                        return -1;
                    }
                    if (ByteArray.Compare(inputTimestamp,
                        outputTimestamp) != 0)
                    {
                        strError = "ʱ�����ƥ��";
                        return -2;
                    }
                    bool bLoadXmlSuccessed = true;
                    XmlDocument oldDataDom = new XmlDocument();
                    oldDataDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                    try
                    {
                        oldDataDom.Load(strXmlFilePath);
                    }
                    catch
                    {
                        bLoadXmlSuccessed = false;
                    }

                    // 1.ɾ�����ļ�
                    if (bLoadXmlSuccessed == true)
                    {
                        nRet = this.ProcessFiles(strID,
                            null,
                            oldDataDom,
                            out strError);
                        if (nRet <= -1)
                            return nRet;
                    }
                    else
                    {
                        this.ForceDeleteFiles(strID);
                    }

                    // 2.ɾ��������
                    if (bLoadXmlSuccessed == true)
                    {

                        KeysCfg keysCfg = null;
                        nRet = this.GetKeysCfg(out keysCfg,
                            out strError);

                        if (nRet == -1)
                            return -1;

                        if (keysCfg != null)
                        {

                            //���ɼ����㼯��
                            KeyCollection oldKeys = null;
                            nRet = keysCfg.BuildKeys(oldDataDom,
                                strID,
                                "zh",
                                "",//strStyle
                                this.KeySize,
                                out oldKeys,
                                out strError);
                            if (nRet == -1)
                                return -1;

                            oldKeys.Sort();
                            oldKeys.RemoveDup();

                            this.DeleteKeys(oldKeys);
                        }
                    }
                    else
                    {

                        // return:
                        //      -1  ����
                        //      0   �ɹ�
                        nRet = this.ForceDeleteKeys(strID,
                            out strError);
                        if (nRet == -1)
                            return -1;
                    }

                    // 3.ɾ������¼
                    this.DeleteRecordByID(strXmlFilePath);

                    // 4.��Sql���,ɾ����ʾ�ֶ���Ϣ�ļ�
                    this.DeleteFuZhuFiles(strXmlFilePath);

                }
                finally
                {
                    //***********�Լ�¼��д��**************
                    m_recordLockColl.UnlockForWrite(strID);
#if DEBUG_LOCK
					this.container.WriteDebugInfo("DeleteRecord()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼��д����");
#endif
                }
            }
            finally
            {
                //**************�����ݿ�����*************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("DeleteRecord()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
            return 0;
        }


        // ���ݼ�¼��֮��Ĺ�ϵ,ǿ��ɾ���ļ�
        public void ForceDeleteFiles(string strRecordID)
        {
            DirectoryInfo dir = new DirectoryInfo(this.m_strSourceFullPath);
            FileInfo[] files = dir.GetFiles(strRecordID + "_*.*");
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i].FullName);
            }
        }

        // ����ɾ��һ����¼��Ӧ�ļ�����,������еı�
        // ��:����ȫ
        // return:
        //      -1  ����
        //      0   �ɹ�
        public int ForceDeleteKeys(string strRecordID,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            KeysCfg keysCfg = null;
            nRet = this.GetKeysCfg(out keysCfg,
                 out strError);
            if (nRet == -1)
                return -1;

            if (keysCfg == null)
                return 0;

            List<TableInfo> aTableInfo = null;
            nRet = keysCfg.GetTableInfosRemoveDup(out aTableInfo,
                out strError);
            if (nRet == -1)
                return -1;


            for (int i = 0; i < aTableInfo.Count; i++)
            {
                TableInfo tableInfo = aTableInfo[i];

                string strTablePath = this.TableName2TableFileName(tableInfo.SqlTableName);

                XmlDocument domTable = new XmlDocument();
                domTable.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                domTable.Load(strTablePath);

                string strXpath = "/root/key[idstring='" + strRecordID + "']";

                XmlNodeList listKey = domTable.SelectNodes(strXpath);
                foreach (XmlNode nodeKey in listKey)
                {
                    domTable.DocumentElement.RemoveChild(nodeKey);
                }
                domTable.Save(strTablePath);
            }
            return 0;
        }

        // ����ID�ӿ���ɾ��һ����¼��,�����Ǽ�¼Ҳ��������Դ
        public void DeleteRecordByID(string strFilePath)
        {
            File.Delete(strFilePath);
        }

        // ��ID������¼
        // parameter:
        //		searchItem  SearchItem���󣬰���������Ϣ
        //		isConnected ���Ӷ����delegate
        //		resultSet   ���������,������м�¼
        // return:
        //		-1  ����
        //		0   �ɹ�
        // �ߣ�����ȫ
        private int SearchByID(SearchItem searchItem,
            Delegate_isConnected isConnected,
            DpResultSet resultSet,
            out string strError)
        {
            strError = "";
            // �ӿ�Ŀ¼��õ������Ƽ�¼�ļ�
            string[] files = Directory.GetFiles(this.m_strSourceFullPath, "??????????.xml");
            ArrayList records = new ArrayList();
            foreach (string fileName in files)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                string strFileName = fileInfo.Name;
                if (this.IsRecord(strFileName) == false)
                    continue;
                records.Add(this.XmlFileName2RecordID(strFileName));
            }

            //ǰ��һ��
            if (searchItem.Match == "left"
                || searchItem.Match == "")
            {
                foreach (string recordID in records)
                {
                    if (recordID.Length < searchItem.Word.Length)
                        continue;

                    string strFirstPart = recordID.Substring(0,
                        searchItem.Word.Length);

                    if (strFirstPart == searchItem.Word)
                    {
                        string strRecPath = this.FullID + "/" + recordID;
                        resultSet.Add(new DpRecord(strRecPath));
                    }
                }
            }
            else if (searchItem.Match == "exact")
            {
                // �Ӽ�����ʱ����������ϵ
                if (searchItem.Relation == "draw")
                {
                    foreach (string recordID in records)
                    {
                        int nPosition = searchItem.Word.IndexOf("-");
                        if (nPosition >= 0)
                        {
                            string strStartID;
                            string strEndID;
                            StringUtil.SplitRange(searchItem.Word,
                                out strStartID,
                                out strEndID);

                            strStartID = DbPath.GetID10(strStartID);
                            strEndID = DbPath.GetID10(strEndID);

                            if (String.Compare(recordID, strStartID, true) >= 0
                                && String.Compare(recordID, strEndID, true) <= 0)
                            {
                                string strRecPath = this.FullID + "/" + recordID;
                                resultSet.Add(new DpRecord(strRecPath));
                                continue;
                            }
                        }
                        else
                        {
                            string strOperator;
                            string strCanKaoID;
                            StringUtil.GetPartCondition(searchItem.Word,
                                out strOperator,
                                out strCanKaoID);

                            strCanKaoID = DbPath.GetID10(strCanKaoID);
                            if (StringUtil.CompareByOperator(recordID,
                                strOperator,
                                strCanKaoID) == true)
                            {
                                string strRecPath = this.FullID + "/" + recordID;
                                resultSet.Add(new DpRecord(strRecPath));
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    foreach (string recordID in records)
                    {
                        searchItem.Word = DbPath.GetID10(searchItem.Word);
                        if (StringUtil.CompareByOperator(recordID,
                            searchItem.Relation,
                            searchItem.Word) == true)
                        {
                            string strRecPath = this.FullID + "/" + recordID;
                            resultSet.Add(new DpRecord(strRecPath));
                            continue;
                        }
                    }
                }
            }
            return 0;
        }

        // �õ�����key������
        // parameter:
        //		searchItem      ������Ϣ����
        //		nodeCovertQuery ��������ʵ����ýڵ�
        // ���ܻ��׳����쳣:NoMatchException(������ʽ����������)
        private int GetKeyCondition(SearchItem searchItem,
            XmlNode nodeConvertQueryString,
            XmlNode nodeConvertQueryNumber,
            out string strKeyCondition,
            out string strError)
        {
            strError = "";
            strKeyCondition = "";

            QueryUtil.VerifyRelation(ref searchItem.Match,//strMatch,
                ref searchItem.Relation,//strRelation,
                ref searchItem.DataType); //strDataType);

            int nRet;
            KeysCfg keysCfg = null;
            nRet = this.GetKeysCfg(out keysCfg,
                out strError);
            if (nRet == -1)
                return -1;

            string strKeyValue = searchItem.Word.Trim();
            if (searchItem.DataType == "string")
            {
                if (nodeConvertQueryString != null
                    && keysCfg != null)
                {
                    List<string> keys = null;
                    nRet = keysCfg.ConvertKeyWithStringNode(
                        null, //dataDom
                        strKeyValue,
                        nodeConvertQueryString,
                        out keys,
                        out strError);
                    if (nRet == -1)
                        return -1;
                    if (keys.Count != 1)
                    {
                        // strError = "���������ò��Ϸ�����Ӧ��ɶ����";
                        strError = "��֧�ְѼ�����ͨ��'split'��ʽ�ӹ��ɶ��.";
                        return -1;
                    }
                    strKeyValue = keys[0];
                }
            }
            else if (searchItem.DataType == "number")
            {
                if (nodeConvertQueryNumber != null
                    && keysCfg != null)
                {
                    string strMyKey;
                    nRet = KeysCfg.ConvertKeyWithNumberNode(
                        strKeyValue,
                        nodeConvertQueryNumber,
                        out strMyKey,
                        out strError);
                    if (nRet == -1)
                        return -1;
                    strKeyValue = strMyKey;
                }
            }
            strKeyValue = strKeyValue.Trim();


            // 4. ���strMatchΪ�գ���"��һ��"
            if (searchItem.Match == "left"
                || searchItem.Match == "")
            {
                //�ж�ѡ�����������
                if (searchItem.DataType != "string")
                {
                    NoMatchException ex = new NoMatchException("��ƥ�䷽ʽΪleftʱ��Ϊ��ʱ���������Ͳ�ƥ�䣬Ӧ��Ϊstring");
                    throw (ex);
                }
                //����Ǳ��յģ���Ϊ�������׳��쳣
                int nLength = searchItem.Word.Trim().Length;
                strKeyCondition = " (substring(keystring,1," + Convert.ToString(nLength) + ")='" + strKeyValue + "') ";
            }
            //�ҷ�һ��
            if (searchItem.Match == "right")
            {
                if (searchItem.DataType != "string")
                {
                    NoMatchException ex = new NoMatchException("��ƥ�䷽ʽΪrightʱ���������Ͳ�ƥ�䣬Ӧ��Ϊstring");
                    throw (ex);
                }
                //ע������Ҫ�ĳ��ҷ�һ��
                int nLength = searchItem.Word.Trim().Length;
                strKeyCondition = " (substring(keystring,1," + Convert.ToString(nLength) + ")='" + strKeyValue + "') ";
            }
            //��ȷһ��
            if (searchItem.Match == "exact")
            {
                //�Ӵ��м�ȡ,�ϸ��ӣ�ע��
                if (searchItem.Relation == "draw")
                {
                    int nPosition;
                    nPosition = searchItem.Word.IndexOf("-");

                    //�Ȱ�"-"��
                    if (nPosition >= 0)
                    {
                        string strStartText;
                        string strEndText;
                        StringUtil.SplitRange(searchItem.Word,
                            out strStartText,
                            out strEndText);

                        if (searchItem.DataType == "string")
                        {
                            if (nodeConvertQueryString != null
                                && keysCfg != null)
                            {
                                // �ӹ���
                                List<string> keys = null;
                                nRet = keysCfg.ConvertKeyWithStringNode(null,//dataDom
                                    strStartText,
                                    nodeConvertQueryString,
                                    out keys,
                                    out strError);
                                if (nRet == -1)
                                    return -1;
                                if (keys.Count != 1)
                                {
                                    strError = "��֧�ְѼ�����ͨ��'split'��ʽ�ӹ��ɶ��.";
                                    return -1;
                                }
                                strStartText = keys[0];


                                // �ӹ�β
                                nRet = keysCfg.ConvertKeyWithStringNode(null,//dataDom
                                    strEndText,
                                    nodeConvertQueryString,
                                    out keys,
                                    out strError);
                                if (nRet == -1)
                                    return -1;
                                if (keys.Count != 1)
                                {
                                    strError = "��֧�ְѼ�����ͨ��'split'��ʽ�ӹ��ɶ��.";
                                    return -1;
                                }
                                strEndText = keys[0];
                            }
                            strKeyCondition = " keystring >= '"
                                + strStartText
                                + "' and keystring<= '"
                                + strEndText + "'";
                        }
                        else if (searchItem.DataType == "number")
                        {
                            if (nodeConvertQueryNumber != null
                                && keysCfg != null)
                            {
                                // ��
                                string strMyKey;
                                nRet = KeysCfg.ConvertKeyWithNumberNode(
                                    strStartText,
                                    nodeConvertQueryNumber,
                                    out strMyKey,
                                    out strError);
                                if (nRet == -1)
                                    return -1;
                                strStartText = strMyKey;

                                // β
                                nRet = KeysCfg.ConvertKeyWithNumberNode(
                                    strEndText,
                                    nodeConvertQueryNumber,
                                    out strMyKey,
                                    out strError);
                                if (nRet == -1)
                                    return -1;
                                strEndText = strMyKey;

                            }
                            strKeyCondition = " keystringnum >= "
                                + strStartText
                                + " and keystringnum<= "
                                + strEndText + "";
                        }
                    }
                    else  // ���� �ȽϷ���
                    {
                        string strOperator;
                        string strRealText;
                        StringUtil.GetPartCondition(searchItem.Word,
                            out strOperator,
                            out strRealText);

                        //SQL��Xpath�Ƚ�������Ĳ��
                        if (strOperator == "<>")
                            strOperator = "!=";
                        if (searchItem.DataType == "string")
                        {
                            if (nodeConvertQueryString != null
                                && keysCfg != null)
                            {
                                List<string> keys = null;
                                nRet = keysCfg.ConvertKeyWithStringNode(null,//dataDom
                                    strRealText,
                                    nodeConvertQueryString,
                                    out keys,
                                    out strError);
                                if (nRet == -1)
                                    return -1;
                                if (keys.Count != 1)
                                {
                                    strError = "��֧�ְѼ�����ͨ��'split'��ʽ�ӹ��ɶ��.";
                                    return -1;
                                }
                                strRealText = keys[0];
                            }

                            strKeyCondition = "keystring" +
                                strOperator +
                                "'" + strRealText + "'";
                        }
                        else if (searchItem.DataType == "number")
                        {
                            if (nodeConvertQueryNumber != null
                                && keysCfg != null)
                            {
                                string strMyKey;
                                nRet = KeysCfg.ConvertKeyWithNumberNode(
                                    strRealText,
                                    nodeConvertQueryNumber,
                                    out strMyKey,
                                    out strError);
                                if (nRet == -1)
                                    return -1;
                                strRealText = strMyKey;

                            }
                            strKeyCondition = "keystringnum" +
                                strOperator +
                                strRealText + " and keystringnum!=-1";
                        }
                    }
                }
                else
                {
                    // ����ϵ������Ϊ��Ϊ����������
                    if (searchItem.Relation == "")
                        searchItem.Relation = "=";
                    if (searchItem.Relation == "<>")
                        searchItem.Relation = "!=";

                    if (searchItem.DataType == "string")
                    {
                        strKeyCondition = " keystring "
                            + searchItem.Relation
                            + "'" + strKeyValue + "'";
                    }
                    else if (searchItem.DataType == "number")
                    {
                        strKeyCondition = "keystringnum"
                            + searchItem.Relation
                            + "" + strKeyValue + "";
                    }
                }
            }
            return 0;
        }

        // ����
        internal override int SearchByUnion(SearchItem searchItem,
            Delegate_isConnected isConnected,
            DpResultSet resultSet,
            int nWarningLevel,
            out string strError,
            out string strWarning)
        {
            strError = "";
            strWarning = "";

            //************�����ݿ�Ӷ���**************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("SearchByUnion()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                int nRet;
                bool bHasID;
                List<TableInfo> aTableInfo = null;
                nRet = this.TableNames2aTableInfo(searchItem.TargetTables,
                    out bHasID,
                    out aTableInfo,
                    out strError);
                if (nRet == -1)
                    return -1;

                if (bHasID == true)
                {
                    nRet = SearchByID(searchItem,
                        isConnected,
                        resultSet,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }

                if (aTableInfo == null || aTableInfo.Count == 0)
                {
                    return 0;
                }

                for (int i = 0; i < aTableInfo.Count; i++)
                {
                    TableInfo tableInfo = aTableInfo[i]; ;
                    string strTiaoJian = "";
                    try
                    {
                        nRet = GetKeyCondition(
                            searchItem,
                            tableInfo.nodeConvertQueryString,
                            tableInfo.nodeConvertQueryNumber,
                            out strTiaoJian,
                            out strError);
                        if (nRet == -1)
                            return -1;
                    }
                    catch (NoMatchException ex)
                    {
                        strWarning += ex.Message;
                        if (nWarningLevel == 0)
                            return -1;
                    }

                    XmlDocument dom = new XmlDocument();
                    dom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                    string strTablePath = this.TableName2TableFileName(tableInfo.SqlTableName);
                    try
                    {
                        dom.Load(strTablePath);
                    }
                    catch (Exception ex)
                    {
                        strError = "���ؼ������'" + tableInfo.SqlTableName + "'��dom����" + ex.Message;
                        return -1;
                    }

                    string strXpath = "/root/key[" + strTiaoJian + "]/idstring";
                    XmlNodeList listIdstring;
                    try
                    {
                        listIdstring = dom.SelectNodes(strXpath);
                    }
                    catch (System.Xml.XPath.XPathException ex)
                    {
                        strError += "Xpath����:" + strXpath + "-------" + ex.Message + "<br/>";
                        return -1;
                    }

                    for (int j = 0; j < listIdstring.Count; j++)
                    {
                        string strIdstring = DomUtil.GetNodeText(listIdstring[j]).Trim();
                        string strId = this.FullID + "/" + strIdstring;
                        resultSet.Add(new DpRecord(strId));
                    }
                }

                //����
                resultSet.Sort();

                //ȥ��
                resultSet.RemoveDup();

                return 0;
            }
            finally
            {
                //*********�����ݿ�����************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("SearchByUnion()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        // ���һ���ļ��ǲ��Ǽ�¼
        private bool IsRecord(string strFileName)
        {
            if (strFileName.Length != 14)
                return false;

            // ����ļ���չ��
            string strEx = Path.GetExtension(strFileName);
            if (strEx.Length > 1)
                strEx = strEx.Substring(1);
            strEx = strEx.ToUpper();
            if (strEx != "XML")
                return false;

            // ����ǲ���10�еļ�¼��
            string strRecordID = Path.GetFileNameWithoutExtension(strFileName);
            if (strRecordID.Length != 10)
                return false;
            if (StringUtil.RegexCompare(@"\d[10]", strRecordID) == false)
                return false;

            // ����xml��չ�������ǳ���10λʱ����������¼
            if (strEx == "XML"
                && strRecordID.Length == 10)
                return true;

            return false;
        }

        // ɾ�����ݿ�
        // return:
        //      -1  ����
        //      0   �ɹ�
        // ��: ��ȫ
        public override int Delete(out string strError)
        {
            strError = "";

            //************�����ݿ��д��********************
            this.m_lock.AcquireWriterLock(m_nTimeOut);

#if DEBUG_LOCK
			this.container.WriteDebugInfo("Delete()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            try
            {
                // ɾ������ԴĿ¼
                if (Directory.Exists(this.m_strSourceFullPath) == true)
                    Directory.Delete(this.m_strSourceFullPath, true);

                // ɾ������Ŀ¼
                string strCfgsDir = DatabaseUtil.GetLocalDir(this.container.NodeDbs,
                    this.m_selfNode);
                if (strCfgsDir != "")
                {
                    // Ӧ��Ŀ¼���أ������������ʹ�����Ŀ¼������ɾ����������Ϣ
                    if (this.container.IsExistCfgsDir(strCfgsDir, this) == false)
                    {
                        Directory.Delete(this.container.DataDir + "\\" + strCfgsDir, true);
                    }
                    else
                    {
                        this.container.WriteErrorLog("���ֳ���'" + this.GetCaption("zh-cn") + "'��ʹ��'" + strCfgsDir + "'Ŀ¼�⣬�����������ʹ�����Ŀ¼�����Բ�����ɾ����ʱɾ��Ŀ¼");
                    }
                }

                return 0;
            }
            finally
            {
                //*********************�����ݿ��д��**********
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("Delete()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            }
        }
    }
}
