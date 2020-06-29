using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;

using DigitalPlatform.Xml;
using DigitalPlatform.Text;

namespace DigitalPlatform.rms
{
    public class DatabaseUtil
    {
        // �õ�newdata�ֶζ�Ӧ���ļ���
        public static string GetNewFileName(string strFilePath)
        {
            return strFilePath + ".new";
        }

        // �õ�range�ֶζ�Ӧ���ļ���
        public static string GetRangeFileName(string strFilePath)
        {
            return strFilePath + ".range";
        }

        // �õ�range�ֶζ�Ӧ���ļ���
        public static string GetTimestampFileName(string strFilePath)
        {
            return strFilePath + ".timestamp";
        }

        // �õ�metadata�ֶζ�Ӧ���ļ���
        public static string GetMetadataFileName(string strFilePath)
        {
            return strFilePath + ".metadata";
        }


        public static string GetLocalDir(XmlNode rootNode,
            XmlNode startNode)
        {
            if (startNode == null)
                return "";

            string strDir = "";
            XmlNode node = startNode;
            while (true)
            {
                if (node == null)
                    break;

                // ���startNode��root���߼�????????????


                string strOneDir = DomUtil.GetAttr(node, "localdir");
                strOneDir = strOneDir.Trim();
                if (strOneDir != "")
                {
                    if (strDir != "")
                        strDir = "\\" + strDir;

                    strDir = strOneDir + strDir;
                }

                if (node == rootNode)
                {
                    break;
                }

                node = node.ParentNode;

            }

            return strDir;
        }


        // Ϊ�����ļ�����ʱ���
        public static byte[] CreateTimestampForCfg(string strFilePath)
        {
            byte[] baTimestamp = null;
            FileInfo fileInfo = new FileInfo(strFilePath);
            if (fileInfo.Exists == false)
                return baTimestamp;

            long lTicks = fileInfo.LastWriteTimeUtc.Ticks;
            byte[] baTime = BitConverter.GetBytes(lTicks);

            byte[] baLength = BitConverter.GetBytes((long)fileInfo.Length);
            //Array.Reverse(baLength);

            baTimestamp = new byte[baTime.Length + baLength.Length];
            Array.Copy(baTime,
                0,
                baTimestamp,
                0,
                baTime.Length);
            Array.Copy(baLength,
                0,
                baTimestamp,
                baTime.Length,
                baLength.Length);

            // return ByteArray.GetHexTimeStampString(baTimestamp);
            return baTimestamp;
        }

        // ������Ŀ��"���ݿ���1:����,����"��ʽ��
        // �ֳɵ�������������table�б�
        // parameter:
        //		strdbComplete	�����������ʽ
        //		strDbName	out�������������ݿ���
        //		strTableList	out����������table�б�
        // return:
        //		-1	����
        //		0	�ɹ�
        public static int SplitToDbNameAndForm(string strTarget,
            out string strDbName,
            out string strTableList,
            out string strError)
        {
            strDbName = "";
            strTableList = "";
            strError = "";

            if (strTarget == null
                || strTarget == "")
            {
                strError = "SplitToDbNameAndForm() strTarget��������Ϊnull����ַ���";
                return -1;
            }

            int nIndex = strTarget.IndexOf(":");
            if (nIndex != -1)
            {
                strDbName = strTarget.Substring(0, nIndex);
                strTableList = strTarget.Substring(nIndex + 1);
            }
            else  //û��':'ʱ��ȫ�����ݿ�������
            {
                strDbName = strTarget;
            }

            return 0;
        }

        // �õ�10λ�ļ�¼ID
        // return:
        //      -1  ����
        //      0   �ɹ�
        public static int CheckAndGet10RecordID(ref string strRecordID,
            out string strError)
        {
            strError = "";

            if (strRecordID == null)
            {
                strError = "��¼ID����Ϊnull";
                return -1;
            }

            if (strRecordID == "")
            {
                strError = "��¼ID����Ϊ���ַ���";
                return -1;
            }

            // ����10���Ϸ�
            if (strRecordID.Length > 10)
            {
                strError = "��¼ID '" + strRecordID + "' ���Ϸ������ܴ���10λ";
                return -1;
            }


            //��'?'����'-1'  ��Ϊԭ��ϵͳ���ϵ�'-1'
            if (strRecordID == "?")
                strRecordID = "-1";

            // ����ת�������ݲ��Ϸ�
            try
            {
                long nId = Convert.ToInt64(strRecordID);
                // ��-1�⣬�������Ϸ�
                if (nId < -1)
                {
                    strError = "��¼ID '" + strRecordID + "' ���Ϸ�";
                    return -1;
                }
            }
            catch
            {
                strError = "��¼ID '" + strRecordID + "' ���Ϸ�";
                return -1;
            }

            strRecordID = DbPath.GetID10(strRecordID);

            return 0;
        }

        public static int AddInteger(string strNewValue,
            string strOldValue,
            out string strLastValue,
            out string strError)
        {
            strLastValue = "";
            strError = "";

            long lNewValue = 0;
            if (strNewValue != "")
            {
                try
                {
                    lNewValue = Convert.ToInt64(strNewValue);
                }
                catch (Exception ex)
                {
                    strError = "��actionΪAddIntegerʱ�������ֵ'" + strNewValue + "'���Ϸ���������������," + ex.Message;
                    return -1;
                }
            }

            long lOldValue = 0;
            if (strOldValue != "")
            {
                try
                {
                    lOldValue = Convert.ToInt64(strOldValue);
                }
                catch
                {
                    strError = "��actionΪAddIntegerʱ,ԭ�����е�ֵ'" + strOldValue + "'���Ϸ���������������";
                    return -1;
                }
            }
            long lLastValue = lNewValue + lOldValue;
            strLastValue = Convert.ToString(lLastValue);

            return 0;
        }

        public static string AppendString(string strNewValue,
            string strOldValue)
        {
            return strOldValue + strNewValue;
        }

        // ������Դ·���е�xpath����
        // parameters:
        //		strOrigin	��Դ·���д�����xpath·��
        //		strLocateXpath	out���������ض�λ�Ľڵ�·��
        //		strCreatePath	out���������ش����Ľڵ�·��
        //		strNewRecordTemplate	������ģ��
        //		strAction	��Ϊ
        //		strError	������Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        public static int PaseXPathParameter(string strOrigin,
            out string strLocateXPath,
            out string strCreatePath,
            out string strNewRecordTemplate,
            out string strAction,
            out string strError)
        {
            strLocateXPath = "";
            strCreatePath = "";
            strNewRecordTemplate = "";
            strAction = "";
            strError = "";

            if (strOrigin.Length == 0)
            {
                strError = "strOrigin����Ϊ���ַ���";
                return -1;
            }

            // �����ţ�xpath����ֱ���ü򵥵�ʽ��
            if (strOrigin[0] == '@')
            {
                strLocateXPath = strOrigin.Substring(1);
                return 0;
            }

            strOrigin = "<root>" + strOrigin + "</root>";
            XmlDocument tempDom = new XmlDocument();
            tempDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

            try
            {
                tempDom.LoadXml(strOrigin);
            }
            catch (Exception ex)
            {
                strError = "PaseXPathParameter() ����strOrigin��dom����,ԭ��:" + ex.Message;
                return -1;
            }

            // locate
            XmlNode node = tempDom.DocumentElement.SelectSingleNode("//locate");
            if (node != null)
                strLocateXPath = DomUtil.GetNodeText(node);

            // create
            node = tempDom.DocumentElement.SelectSingleNode("//create");
            if (node != null)
                strCreatePath = DomUtil.GetNodeText(node);

            // template		
            node = tempDom.DocumentElement.SelectSingleNode("//template");
            if (node != null)
            {
                if (node.ChildNodes.Count != 1)
                {
                    strError = "template�¼�����ֻ����һ�����ӽڵ�";
                    return -1;
                }
                if (node.ChildNodes[0].NodeType != XmlNodeType.Element)
                {
                    strError = "template���¼�������Ԫ������";
                    return -1;
                }
                strNewRecordTemplate = node.InnerXml;
            }

            // action
            node = tempDom.DocumentElement.SelectSingleNode("//action");
            if (node != null)
                strAction = DomUtil.GetNodeText(node);

            return 0;
        }

        public static byte[] StringToByteArray(string strText,
            byte[] baPreamble)
        {
            byte[] baText = Encoding.UTF8.GetBytes(strText);
            if (baPreamble != null
                && baPreamble.Length > 0)
            {
                baText = ByteArray.Add(baText, baPreamble);
            }
            return baText;
        }


        // ��һ���ֽ�����ת�����ַ������ڲ����Զ����preamble
        //		bHasPreamble	����bytes�Ƿ��preamble
        public static string ByteArrayToString(byte[] bytes,
            out byte[] baOutputPreamble)
        {
            baOutputPreamble = new byte[0];

            int nIndex = 0;
            int nCount = bytes.Length;

            byte[] baPreamble = Encoding.UTF8.GetPreamble();
            if (bytes.Length > baPreamble.Length)
            {
                if (baPreamble != null
                    && baPreamble.Length != 0)
                {
                    byte[] temp = new byte[baPreamble.Length];
                    Array.Copy(bytes,
                        0,
                        temp,
                        0,
                        temp.Length);

                    bool bEqual = true;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i] != baPreamble[i])
                        {
                            bEqual = false;
                            break;
                        }
                    }

                    if (bEqual == true)
                    {
                        baOutputPreamble = baPreamble;
                        nIndex = temp.Length;
                        nCount = bytes.Length - temp.Length;
                    }
                }
            }
            return Encoding.UTF8.GetString(bytes,
                nIndex,
                nCount);
        }


        // �ϲ�Ԫ������Ϣ
        // parameters:
        //		strOldMetadata	��Ԫ����
        //		strNewMetadata	��Ԫ����
        //		nLength	���� -1��ʾ����δ֪ -2��ʾ���Ȳ���
        //		strResult	out���������غϲ����Ԫ����
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        public static int MergeMetadata(string strOldMetadata,
            string strNewMetadata,
            long lLength,
            out string strResult,
            out string strError)
        {
            strResult = "";
            strError = "";

            XmlDocument oldDom = new XmlDocument();
            oldDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue
            if (strOldMetadata == "")
                strOldMetadata = "<file/>";
            try
            {
                oldDom.LoadXml(strOldMetadata);
            }
            catch (Exception ex)
            {
                strError = "���е�Ԫ���ݲ��Ϸ�\r\n" + ex.Message;
                return -1;
            }
            XmlNode oldRoot = oldDom.DocumentElement;

            XmlDocument newDom = new XmlDocument();
            newDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue
            if (strNewMetadata == "")
                strNewMetadata = "<file/>";
            try
            {
                newDom.LoadXml(strNewMetadata);
            }
            catch (Exception ex)
            {
                strError = "��������ԴԪ���ݲ��Ϸ�\r\n" + ex.Message;
                return -1;
            }
            XmlNode newRoot = newDom.DocumentElement;

            string strMimetype = DomUtil.GetAttr(newRoot, "mimetype");
            if (strMimetype != "")
                DomUtil.SetAttr(oldRoot, "mimetype", strMimetype);

            string strLocalPath = DomUtil.GetAttr(newRoot, "localpath");
            if (strLocalPath != "")
                DomUtil.SetAttr(oldRoot, "localpath", strLocalPath);

            // -1��ʾ����δ֪
            // -2��ʾ���ֲ���
            if (lLength != -2)
                DomUtil.SetAttr(oldRoot, "size", Convert.ToString(lLength));

            DomUtil.SetAttr(oldRoot, "lastmodified", System.DateTime.Now.ToString());

            strResult = oldRoot.OuterXml;
            return 0;
        }


        // ������Χ�Ƿ�Ϸ�,�����������ܹ�ȡ�ĳ���
        // parameter:
        //		nStart          ��ʼλ�� ����С��0
        //		nNeedLength     ��Ҫ�ĳ���	����С��-1��-1��ʾ��nStart-(nTotalLength-1)
        //		nTotalLength    ����ʵ���ܳ��� ����С��0
        //		nMaxLength      ���Ƶ���󳤶�	����-1����ʾ������
        //		nOutputLength   out���������صĿ����õĳ���
        //		strError        out���������س�����Ϣ
        // return:
        //		-1  ����
        //		0   �ɹ�
        public static int GetRealLength(int nStart,
            int nNeedLength,
            int nTotalLength,
            int nMaxLength,
            out int nOutputLength,
            out string strError)
        {
            nOutputLength = 0;
            strError = "";

            // ��ʼֵ,�����ܳ��Ȳ��Ϸ�
            if (nStart < 0
                || nTotalLength < 0)
            {
                strError = "��Χ����:nStart < 0 �� nTotalLength <0 \r\n";
                return -1;
            }
            if (nStart != 0
                && nStart >= nTotalLength)
            {
                strError = "��Χ����:��ʼֵ�����ܳ���\r\n";
                return -1;
            }

            nOutputLength = nNeedLength;
            if (nOutputLength == 0)
            {
                return 0;
            }

            if (nOutputLength == -1)  // �ӿ�ʼ��ȫ��
                nOutputLength = nTotalLength - nStart;

            if (nStart + nOutputLength > nTotalLength)
                nOutputLength = nTotalLength - nStart;

            // ��������󳤶�
            if (nMaxLength != -1 && nMaxLength >= 0)
            {
                if (nOutputLength > nMaxLength)
                    nOutputLength = nMaxLength;
            }
            return 0;
        }




        // ����xml�ļ�
        // parameters:
        //      strFileName �ļ���
        //      strXml  xml�ַ���
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // strXml����ֵֻ��Ϊ�գ�null����ַ�����,���ߺϷ�xml
        // ���Ϊ�գ��򴴽�һ�����ļ�
        public static int CreateXmlFile(string strFileName,
            string strXml,
            out string strError)
        {
            strError = "";
            if (String.IsNullOrEmpty(strXml) == true)
            {
                Stream s = File.Create(strFileName);
                s.Close();
                return 0;
            }

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                strError = "CreateXmlFile()�������ַ�����domʱ����" + ex.Message;
                return -1;
            }

            XmlTextWriter w = new XmlTextWriter(strFileName,
                System.Text.Encoding.UTF8);
            dom.Save(w);
            w.Close();

            return 0;
        }


        // �õ�
        public static string GetAllCaption(XmlNode node)
        {
            string strXpath = "property/logicname/caption";
            XmlNodeList list = node.SelectNodes(strXpath);

            string strAllCaption = "";
            foreach (XmlNode oneNode in list)
            {
                string strCaption = DomUtil.GetNodeText(oneNode);
                if (strCaption == "")
                    continue;

                if (strAllCaption != "")
                    strAllCaption += ",";

                strAllCaption += strCaption;

            }

            return strAllCaption;
        }



        public static List<XmlNode> GetNodes(XmlNode root,
            string strCfgItemPath)
        {
            Debug.Assert(root != null, "GetNodes()���ô���root����ֵ����Ϊnull��");
            Debug.Assert(strCfgItemPath != null && strCfgItemPath != "", "GetNodes()���ô���strCfgItemPath����Ϊnull��");


            List<XmlNode> nodes = new List<XmlNode>();

            //��strpath��'/'�ֿ�
            string[] paths = strCfgItemPath.Split(new char[] { '/' });
            if (paths.Length == 0)
                return nodes;

            int i = 0;
            if (paths[0] == "")
                i = 1;
            XmlNode nodeCurrent = root;
            for (; i < paths.Length; i++)
            {
                string strName = paths[i];

                bool bFound = false;
                foreach (XmlNode child in nodeCurrent.ChildNodes)
                {
                    if (child.NodeType != XmlNodeType.Element)
                        continue;

                    bool bThisFound = false;

                    if (String.Compare(child.Name, "database", true) == 0)
                    {
                        string strAllCaption = DatabaseUtil.GetAllCaption(child);
                        if (StringUtil.IsInList(strName, strAllCaption) == true)
                        {
                            bFound = true;
                            bThisFound = true;
                        }
                        else
                        {
                            bThisFound = false;
                        }
                    }
                    else
                    {
                        string strChildName = DomUtil.GetAttr(child, "name");
                        if (String.Compare(strName, strChildName, true) == 0)
                        {
                            bFound = true;
                            bThisFound = true;
                        }
                        else
                        {
                            bThisFound = false;
                        }
                    }

                    if (bThisFound == true)
                    {
                        if (i == paths.Length - 1)
                        {
                            nodes.Add(child);
                        }
                        else
                        {
                            nodeCurrent = child;
                            break;
                        }
                    }
                }

                // ����δ�ҵ�������ѭ��
                if (bFound == false)
                    break;
            }

            return nodes;

        }

    }
}
