using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Threading;

using DigitalPlatform.Xml;
using DigitalPlatform.rms.Client;
using DigitalPlatform.IO;
using DigitalPlatform.Text;

using DigitalPlatform.ResultSet;
using DigitalPlatform.Marc;
using DigitalPlatform.MarcDom;
using DigitalPlatform.rms;

namespace DigitalPlatform.LibraryServer
{
#if NO
    /// <summary>
    /// ���𴴽�������߳�
    /// </summary>
    public class CacheBuilder : BatchTask
    {
        public CacheBuilder(LibraryApplication app, 
            string strName)
            : base(app, strName)
        {
            this.Loop = true;
        }

        public override string DefaultName
        {
            get
            {
                return "��������";
            }
        }


        // һ�β���ѭ��
        public override void Worker()
        {
            // ϵͳ�����ʱ�򣬲����б��߳�
            if (this.App.HangupReason == HangupReason.LogRecover)
                return;

            string strError = "";
            int nRet = 0;

            bool bPerDayStart = false;  // �Ƿ�Ϊÿ��һ������ģʽ
            string strMonitorName = "cacheBuilder";
            {
                string strLastTime = "";

                nRet = ReadLastTime(
                    strMonitorName,
                    out strLastTime,
                    out strError);
                if (nRet == -1)
                {
                    string strErrorText = "���ļ��л�ȡ "+strMonitorName+" ÿ������ʱ��ʱ��������: " + strError;
                    this.AppendResultText(strErrorText + "\r\n");
                    this.App.WriteErrorLog(strErrorText);
                    return;
                }

                string strStartTimeDef = "";
                bool bRet = false;
                nRet = IsNowAfterPerDayStart(
                    strMonitorName,
                    strLastTime,
                    out bRet,
                    out strStartTimeDef,
                    out strError);
                if (nRet == -1)
                {
                    string strErrorText = "��ȡ "+strMonitorName+" ÿ������ʱ��ʱ��������: " + strError;
                    this.AppendResultText(strErrorText + "\r\n");
                    this.App.WriteErrorLog(strErrorText);
                    return;
                }

                // ���nRet == 0����ʾû��������ز����������ԭ����ϰ�ߣ�ÿ�ζ���
                if (nRet == 0)
                {

                }
                else if (nRet == 1)
                {
                    bPerDayStart = true;

                    if (bRet == false)
                    {
                        if (this.ManualStart == true)
                            this.AppendResultText("����̽�������� '" + this.Name + "'������û�е�ÿ������ʱ�� " + strStartTimeDef + " ��δ��������(�ϴ����������ʱ��Ϊ " + DateTimeUtil.LocalTime(strLastTime) + ")\r\n");

                        return; // ��û�е�ÿ��ʱ��
                    }
                }

                this.App.WriteErrorLog((bPerDayStart == true ? "(��ʱ)" : "(����ʱ)") + strMonitorName + " ������");
            }

            this.AppendResultText("��ʼ��һ��ѭ��\r\n");

            int nTotalRecCount = 0;
            for (int i = 0; ; i++)
            {
                // ϵͳ�����ʱ�򣬲����б��߳�
                if (this.App.HangupReason == HangupReason.LogRecover)
                    break;

                if (this.m_bClosed == true)
                    break;

                if (this.Stopped == true)
                    break;

                string strLine = "";
                lock (this.App.PendingCacheFiles)
                {
                    if (this.App.PendingCacheFiles.Count == 0)
                        break;
                    strLine = this.App.PendingCacheFiles[0];

                    // �Ȳ�Ҫ�Ӷ�������ɾ������У��Ա�֤�ͺ��潫Ҫ��������ų�
                }

                bool bDone = false;

                string strPart = "";
                try
                {
                    string[] parts = strLine.Split(new char[] { ':' });
                    if (parts.Length < 2)
                    {
                        strError = "parts format error";
                        goto ERROR1;
                    }

                    string strDataFile = "";
                    string strNodePath = "";

                    if (parts.Length > 0)
                        strDataFile = parts[0];
                    if (parts.Length > 1)
                        strNodePath = parts[1];
                    if (parts.Length > 2)
                        strPart = parts[2];


                    int nCount = 0;

                    this.AppendResultText("*** ��������" + " " + strLine + "\r\n");
                    nRet = BuildOneCache(
                        strDataFile,
                        strNodePath,
                        strPart,
                        out nCount,
                        out strError);
                    if (nRet == -1)
                        goto ERROR1;
                    bDone = true;
                    nTotalRecCount += nCount;
                }
                finally
                {
                    // �Ӷ�����ɾ���ղ�����˵�����
                    lock (this.App.PendingCacheFiles)
                    {
                        if (this.App.PendingCacheFiles.Count != 0)
                        {
                            nRet = this.App.PendingCacheFiles.IndexOf(strLine);
                            if (nRet != -1)
                                this.App.PendingCacheFiles.RemoveAt(nRet);
                            // ��������ȫ���ļ�������Ҫɾ�������Ŷӵĵ���rssˢ��������Ϊrss�Ѿ�������ˢ���ˡ�
                            if (bDone == true
                                && String.IsNullOrEmpty(strPart) == true)
                            {
                                nRet = this.App.PendingCacheFiles.IndexOf(strLine + ":rss");
                                if (nRet != -1)
                                    this.App.PendingCacheFiles.RemoveAt(nRet);
                            }
                        }
                    }

                    if (bDone == false)
                    {
                        string strErrorText = "�������� '"+strLine+"' ��Ϊ�������� '"+strError+"' �����ò�ɾ��(�����ʱ���ڷ�������)...";
                        this.App.WriteErrorLog(strErrorText);
                    }
                }
            }

            AppendResultText("ѭ�������������� " + nTotalRecCount.ToString() + " ����¼��\r\n");

            {
                Debug.Assert(this.App != null, "");

                // д���ļ��������Ѿ������ĵ���ʱ��
                string strLastTime = DateTimeUtil.Rfc1123DateTimeString(this.App.Clock.UtcNow);  // 2007/12/17 changed // DateTime.UtcNow
                WriteLastTime(strMonitorName,
                    strLastTime);
                string strErrorText = (bPerDayStart == true ? "(��ʱ)" : "(����ʱ)") + strMonitorName + "�������������¼ " + nTotalRecCount.ToString() + " ����";
                this.App.WriteErrorLog(strErrorText);

            }
            return;
        ERROR1:
            AppendResultText("CacheBuilder thread error : " + strError + "\r\n");
            this.App.WriteErrorLog("CacheBuilder thread error : " + strError + "\r\n");
            return;
        }

        // �Ƚ��ļ�����ʱ��͵�ǰʱ�䣬�����Ƿ񳬹��ؽ�����
        public static bool HasExpired(string strFilename,
            string strBuildStyle)
        {
            if (String.IsNullOrEmpty(strBuildStyle) == true)
                return false;

            TimeSpan delta;
            try
            {
                FileInfo fi = new FileInfo(strFilename);
                delta = DateTime.Now - fi.LastWriteTime;
            }
            catch
            {
                return false;
            }

            if (strBuildStyle.ToLower() == "perhour")
            {
                if (delta.TotalHours >= 1)
                    return true;
            }
            else if (strBuildStyle.ToLower() == "perday")
            {
                if (delta.TotalDays >= 1)
                    return true;
            }
            else if (strBuildStyle.ToLower() == "perweek")
            {
                if (delta.TotalDays >= 7)
                    return true;
            }
            else if (strBuildStyle.ToLower() == "permonth")
            {
                if (delta.TotalDays >= 30)
                    return true;
            }
            else if (strBuildStyle.ToLower() == "peryear")
            {
                if (delta.TotalDays >= 365)
                    return true;
            }
            else
            {
                return false;   // ��֧�ֵ�strBuildStyle
            }

            return false;
        }

        // ���Build��ز���
        // parameters:
        //      strBuildStyle    ������� perday / perhour
        public static void GetBuildParam(XmlNode node,
            out string strBuildStyle)
        {
            strBuildStyle = "";

            string strBuild = DomUtil.GetAttr(node, "build");

            Hashtable param_table = StringUtil.ParseParameters(strBuild);

            // env_param_table ����������
            Hashtable env_param_table = GetBuildEnvParamTable(node);

            // �ϲ�����������
            Hashtable result = MergeTwoTable(param_table, env_param_table);

            strBuildStyle = (string)result["autoUpdate"];
        }

        // ���RSS��ز���
        // parameters:
        //      nMaxCount   -1��ʾ�����
        //      strDirection    head/tail
        public static void GetRssParam(XmlNode node,
            out bool bEnable,
            out long nMaxCount,
            out string strDirection)
        {
            bEnable = false;
            nMaxCount = -1;
            strDirection = "head";

            string strRss = DomUtil.GetAttr(node, "rss");

            Hashtable param_table = StringUtil.ParseParameters(strRss);

            // env_param_table ����������
            Hashtable env_param_table = GetRssEnvParamTable(node);

            // �ϲ�����������
            Hashtable result = MergeTwoTable(param_table, env_param_table);

            string strEnable = (string)result["enable"];
            if (strEnable != null)
                strEnable = strEnable.ToLower();
            if (strEnable == "on" || strEnable == "true")
                bEnable = true;

            string strMaxCount = (string)result["maxcount"];

            if (String.IsNullOrEmpty(strMaxCount) == true)
                return;

            if (strMaxCount[0] == '-')
            {
                strDirection = "tail";
                strMaxCount = strMaxCount.Substring(1);
            }
            else
                strDirection = "head";

            if (String.IsNullOrEmpty(strMaxCount) == true)
                nMaxCount = -1;
            else
                nMaxCount = Convert.ToInt64(strMaxCount);
        }

        // ����һ������(һ��)
        // return:
        //      -1  error
        //      0   �ɹ�
        //      1   ����ļ���ռ��
        //      2   û�б�Ҫ���
        int BuildOneCache(
            string strDataFile,
            string strNode,
            string strPart,
            out int nCount,
            out string strError)
        {
            int nRet = 0;
            strError = "";
            nCount = 0;

            string strDataFilePath = this.App.DataDir + "/browse/" + strDataFile;


            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strDataFilePath);
            }
            catch (Exception ex)
            {
                strError = "װ���ļ� '" + strDataFilePath + "' ʱ����: " + ex.Message;
                return -1;
            }

            XmlNode node = GetDataNode(dom.DocumentElement,
                strNode);

            if (node == null)
            {
                strError = "·�� '" + strNode + "' ���ļ� '" + strDataFile + "' ��û���ҵ���Ӧ�Ľڵ�";
                return -1;
            }

            bool bRss = false;
            long nMaxCount = -1;
            string strDirection = "";
            // parameters:
            //      nMaxCount   -1��ʾ�����
            //      strDirection    head/tail
            GetRssParam(node,
                out bRss,
                out nMaxCount,
                out strDirection);

            string strCommand = DomUtil.GetAttr(node, "command");
            string strPureCaption = DomUtil.GetAttr(node, "name");
            string strDescription = DomUtil.GetAttr(node, "description");

            if (strCommand == "~hidelist~")
            {
                strError = "�˽ڵ� ~hidelist~ ���ش�������";
                return 2;
            }

            if (strCommand == "~none~")
            {
                strError = "�˽ڵ� ~none~ ���ش�������";
                return 2;
            }

            // strDataFile ��Ϊ���ļ���

            string strPrefix = strNode;
            string strCacheDir = this.App.DataDir + "/browse/cache/" + strDataFile;

            PathUtil.CreateDirIfNeed(strCacheDir);
            string strResultsetFilename = strCacheDir + "/" + strPrefix;
            string strTempResultsetFilename = strCacheDir + "/_temp_" + strPrefix;

            string strRssString = "datafile=" + strDataFile + "&node=" + strPrefix;
            // this.HyperLink_rss.NavigateUrl = "browse.aspx?action=rss&" + strRssString;

            string strChannelLink = this.App.OpacServerUrl + "/browse.aspx?datafile=" + strDataFile
                + "&node=" + strNode;
            string strSelfLink = this.App.OpacServerUrl + "/browse.aspx?action=rss&datafile=" + strDataFile
                + "&node=" + strNode;

            /*
            // ����ļ��Ѿ����ڣ��Ͳ�Ҫ��dp2kernel��ȡ��
            if (File.Exists(strResultsetFilename) == true)
            {
                // �ӽ�����ļ����RSS����
                nRet = BuildRss(strResultsetFilename,
                    strPureCaption,
                    strChannelLink,
                    strSelfLink,
                    strDescription,
                    out strError);
                if (nRet == -1)
                    return -1;
                return 0;
            }*/

            bool bError = false;

            // ����ļ��Ѿ����ڣ��Ͳ�Ҫ��dp2kernel��ȡ��
            if (File.Exists(strResultsetFilename) == true
                && strPart == "rss")
            {
                if (bRss == false)
                {
                    // TODO: ��������ì��
                }

                // �����ļ�������ʱ�ļ�Ϊ����RSS�ļ���Դ��
                // �������Լ���������ͻ
                File.Copy(strResultsetFilename,
                    strTempResultsetFilename,
                    true);
                File.Copy(
                    strResultsetFilename + ".index",
                    strTempResultsetFilename + ".index",
                    true);
                bool bDone = false;
                try
                {
                    SetProgressText("����RSS�ļ�" + " " + strPureCaption + " -- "+ strResultsetFilename);
                    this.AppendResultText("����RSS�ļ�" + " " + strPureCaption + " -- " + strResultsetFilename + "\r\n");

                    // �ӽ�����ļ����RSS����
                    nRet = BuildRssFile(strTempResultsetFilename,
                        nMaxCount,
                        strDirection,
                        strPureCaption,
                        strChannelLink,
                        strSelfLink,
                        strDescription,
                        strTempResultsetFilename + ".rss",
                        out nCount,
                        out strError);
                    if (nRet == -1)
                    {
                        return -1;
                    }
                    bDone = true;
                    this.AppendResultText("  ������¼ " + nCount.ToString() + " ��\r\n");
                }
                finally
                {
                    // �滻
                    try
                    {
                        this.App.ResultsetLocks.LockForWrite(strResultsetFilename + ".rss",
                            500);
                        try
                        {
                            if (bDone == true)
                            {
                                File.Delete(strResultsetFilename + ".rss");
                                File.Move(strTempResultsetFilename + ".rss", strResultsetFilename + ".rss");
                            }
                            // ɾ����ʱ�ļ�
                            File.Delete(strTempResultsetFilename);
                            File.Delete(strTempResultsetFilename + ".index");
                        }
                        finally
                        {
                            this.App.ResultsetLocks.UnlockForWrite(strResultsetFilename + ".rss");
                        }
                    }
                    catch (System.ApplicationException)
                    {
                        bError = true;
                        strError = "����ļ���ռ��";
                        // TODO: ��ô�ƺ�?
                    }
                }

                if (bError == true)
                    return 1;
            }
            else
            {
                int nResultSetCount = 0;
                int nRssCount = 0;

                /*
                nRet = this.App.InitialVdbs(this.RmsChannels,
            out strError);
                if (nRet == -1)
                {
                    strError = "InitialVdbs error : " + strError;
                    return -1;
                }
                 * */
                if (this.App.vdbs == null)
                {
                    strError = "vdbs == null";
                    return -1;
                }

                string strXml = "";
                nRet = BuildXmlQuery(node,
                    out strXml,
                    out strError);
                if (nRet == -1)
                    return -1;

                RmsChannel channel = this.RmsChannels.GetChannel(this.App.WsUrl);
                if (channel == null)
                {
                    strError = "get channel error";
                    return -1;
                }

                string strResultSetName = "opac_browse_" + strPureCaption;

                channel.Idle += new IdleEventHandler(channel_Idle);
                try
                {
                    long lRet = 0;
                    int nRedoCount = 0;

                REDO_SEARCH:

                    SetProgressText("����" + " " + strPureCaption);
                    this.AppendResultText("����" + " " + strPureCaption + "\r\n");

                    lRet = channel.DoSearch(strXml,
                        strResultSetName,
                        "", // strOutputStyle
                        out strError);
                    if (lRet == -1)
                    {
                        // ��ʱ����
                        if (channel.ErrorCode == ChannelErrorCode.RequestTimeOut
                            && nRedoCount < 5)
                        {
                            channel.Abort();
                            // channel.Timeout *= 10;
                            // TODO: �ҵ�����취
                            nRedoCount++;
                            this.AppendResultText("���棺����������ʱ" + "\r\n");
                            goto REDO_SEARCH;
                        }


                        strError = "DoSearch() error : " + strError;
                        return -1;
                    }

                    this.AppendResultText("  ���м�¼ "+lRet.ToString()+" ��" + "\r\n");

                    SetProgressText("��ý�����ļ�" + " " + strPureCaption + " -- " + strResultsetFilename);
                    this.AppendResultText("��ý�����ļ�" + " " + strPureCaption + " -- " + strResultsetFilename + "\r\n");

                    nRet = GetResultset(channel,
                        strResultSetName,
                        strTempResultsetFilename,
                        out nResultSetCount,
                        out strError);
                    if (nRet == -1)
                        return -1;

                    if (this.m_bClosed == true || this.Stopped == true)
                    {
                        strError = "�ж�";
                        return -1;
                    }

                    /*
                    // not found
                    if (lRet == 0)
                    {
                        return 0;
                    }*/
                }
                finally
                {
                    channel.Idle -= new IdleEventHandler(channel_Idle);
                }

                // �滻�����ļ�
                try
                {
                    this.App.ResultsetLocks.LockForWrite(strResultsetFilename,
                        500);
                    try
                    {
                        File.Delete(strResultsetFilename);
                        File.Delete(strResultsetFilename + ".index");

                        if (bRss == true)
                        {
                            // �����ļ���������ʱ�ļ�Ϊ����RSS�ļ���ΪԴ��
                            // �������Լ���������ͻ
                            File.Copy(strTempResultsetFilename, 
                                strResultsetFilename, 
                                true);
                            File.Copy(strTempResultsetFilename + ".index", 
                                strResultsetFilename + ".index",
                                true);
                        }
                        else
                        {
                            File.Move(strTempResultsetFilename, strResultsetFilename);
                            File.Move(strTempResultsetFilename + ".index", strResultsetFilename + ".index");
                        }
                    }
                    finally
                    {
                        this.App.ResultsetLocks.UnlockForWrite(strResultsetFilename);
                    }
                }
                catch (System.ApplicationException)
                {
                    strError = "����ļ���ռ��1";
                    // TODO: ��ô�ƺ�?
                    return 1;
                }

                if (bRss == true)
                {
                    bool bDone = false;
                    try
                    {
                        SetProgressText("����RSS�ļ�" + " " + strPureCaption + " -- " + strResultsetFilename);
                        this.AppendResultText("����RSS�ļ�" + " " + strPureCaption + " -- " + strResultsetFilename + "\r\n");

                        // �ӽ�����ļ����RSS����
                        nRet = BuildRssFile(strTempResultsetFilename,
                        nMaxCount,
                        strDirection,
                            strPureCaption,
                            strChannelLink,
                            strSelfLink,
                            strDescription,
                            null,
                            out nRssCount,
                            out strError);
                        if (nRet == -1)
                        {
                            return -1;
                        }

                        bDone = true;
                        this.AppendResultText("  ������¼ "+nRssCount.ToString()+" ��\r\n");

                    }
                    finally
                    {
                        // �滻RSS�ļ�
                        try
                        {
                            this.App.ResultsetLocks.LockForWrite(strResultsetFilename + ".rss",
                                500);
                            try
                            {
                                if (bDone == true)
                                {
                                    File.Delete(strResultsetFilename + ".rss");
                                    File.Move(strTempResultsetFilename + ".rss",
                                        strResultsetFilename + ".rss");
                                }

                                // ɾ����ʱ�ļ�
                                File.Delete(strTempResultsetFilename);
                                File.Delete(strTempResultsetFilename + ".index");
                            }
                            finally
                            {
                                this.App.ResultsetLocks.UnlockForWrite(strResultsetFilename + ".rss");
                            }
                        }
                        catch (System.ApplicationException)
                        {
                            bError = true;
                            strError = "����ļ���ռ��2";
                            // TODO: ��ô�ƺ�?
                        }
                    }

                    if (bError == true)
                        return 1;

                }


                nCount = nResultSetCount + nRssCount;
            }


            return 0;
        }

        // ���������ļ���
        public static string MakeResultsetFilename(XmlNode node)
        {
            string strResult = "";
            while (node != null)
            {
                XmlNode parent = node.ParentNode;
                if (parent == null
                    || node == node.OwnerDocument.DocumentElement)
                {
                    if (String.IsNullOrEmpty(strResult) == true)
                        strResult = "0";
                    else
                        strResult = "0" + "_" + strResult;
                    return strResult;
                }

                for (int i = 0; i < parent.ChildNodes.Count; i++)
                {
                    if (parent.ChildNodes[i] == node)
                    {
                        if (String.IsNullOrEmpty(strResult) == true)
                            strResult = i.ToString();
                        else
                            strResult = i.ToString() + "_" + strResult;
                        break;
                    }
                }

                node = parent;
            }

            return strResult;
        }

        // parameters:
        //      bOnlyAppend ==true��ʾ����׷��û�е����⣬�Ѿ��еĲ����ظ���==false����ʾȫ����ˢ��
        public static int RefreshAll(LibraryApplication app,
            string strDataFile,
            bool bOnlyAppend,
            out string strError)
        {
            strError = "";

            string strDataFilePath = app.DataDir + "/browse/" + strDataFile;

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strDataFilePath);
            }
            catch (Exception ex)
            {
                strError = "װ���ļ� '" + strDataFilePath + "' ʱ����: " + ex.Message;
                return -1;
            }

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("//*");
            List<string> lines = new List<string>();
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];
                if (node.NodeType != XmlNodeType.Element)
                    continue;
                if (node.Name == "_title")
                    continue;
                if (node.Name == "caption")
                    continue;

                bool bRss = false;
                long nMaxCount = -1;
                string strDirection = "";
                // parameters:
                //      nMaxCount   -1��ʾ�����
                //      strDirection    head/tail
                GetRssParam(node,
                    out bRss,
                    out nMaxCount,
                    out strDirection);
                string strCommand = DomUtil.GetAttr(node, "command");
                if (strCommand == "~hidelist~")
                {
                    // strError = "�˽ڵ� ~hidelist~ ���ش�������";
                    continue;
                }

                if (strCommand == "~none~")
                {
                    // strError = "�˽ڵ� ~none~ ���ش�������";
                    continue;
                }

                /*
                string strPureCaption = DomUtil.GetAttr(node, "name");
                string strDescription = DomUtil.GetAttr(node, "description");
                */

                string strPrefix = MakeResultsetFilename(node);
                if (bOnlyAppend == true)
                {
                    // strDataFile ��Ϊ���ļ���
                    string strCacheDir = app.DataDir + "/browse/cache/" + strDataFile;

                    PathUtil.CreateDirIfNeed(strCacheDir);
                    string strResultsetFilename = strCacheDir + "/" + strPrefix;

                    string strRssString = "datafile=" + strDataFile + "&node=" + strPrefix;

                    if (File.Exists(strResultsetFilename) == true)
                    {
                        if (File.Exists(strResultsetFilename + ".index") == false)
                            goto DO_ADD;

                        if (File.Exists(strResultsetFilename + ".rss") == true)
                            continue;
                    }

                    string strLine = strDataFile + ":" + strPrefix + ":rss";
                    lines.Add(strLine.ToLower());
                    continue;
                }

            DO_ADD:
                {
                    string strLine = strDataFile + ":" + strPrefix;
                    lines.Add(strLine.ToLower());
                }
            }

                int nCount = 0;
            if (lines.Count > 0)
            {
                lock (app.PendingCacheFiles)
                {
                    foreach (string strLine in lines)
                    {
                        if (app.PendingCacheFiles.IndexOf(strLine) == -1)
                        {
                            nCount++;
                            app.PendingCacheFiles.Add(strLine);
                        }
                    }
                }
                /*
                if (nCount > 0)
                    app.ActivateCacheBuilder();
                 * */
            }

            // ������ζ�����
            app.ActivateCacheBuilder();
            return nCount;
        }

        // return:
        //      0   û�д����µĶ�������
        //      1   �������µĶ�������
        public static int AddToPendingList(LibraryApplication app,
            string strDataFile,
            string strNodePath,
            string strPart)
        {
            // �����б�
            lock (app.PendingCacheFiles)
            {
                string strLine = strDataFile + ":" + strNodePath;
                if (String.IsNullOrEmpty(strPart) == false)
                    strLine += ":" + strPart;
                strLine = strLine.ToLower();
                if (app.PendingCacheFiles.IndexOf(strLine) == -1)
                {
                    app.PendingCacheFiles.Add(strLine);
                    app.ActivateCacheBuilder();
                    return 1;
                }
            }
            app.ActivateCacheBuilder();
            return 0;
        }

        public static XmlNode GetDataNode(XmlNode root,
    string strNodePath)
        {
            string[] path = strNodePath.Split(new char[] { '_' });
            XmlNode current_node = root;
            for (int i = 1; i < path.Length; i++)
            {
                string strNumber = path[i];
                int nNumber = Convert.ToInt32(strNumber);
                current_node = current_node.ChildNodes[nNumber];
            }

            return current_node;
        }

        public static long GetCount(LibraryApplication app,
            string strResultsetFilename)
        {
            try
            {
                app.ResultsetLocks.LockForRead(strResultsetFilename, 500);
                try
                {
                    return DpResultSet.GetCount(strResultsetFilename + ".index");
                }
                finally
                {
                    app.ResultsetLocks.UnlockForRead(strResultsetFilename);
                }
            }
            catch (System.ApplicationException)
            {
                return -1;
            }
        }

        // ����XML����ʽ
        // parameters:
        int BuildXmlQuery(XmlNode node,
            out string strXml,
            out string strError)
        {
            strError = "";
            strXml = "";

            string strCommand = DomUtil.GetAttr(node, "command");


            if (String.IsNullOrEmpty(strCommand) == true)
            {
                string strName = DomUtil.GetAttr(node, "name");

                // �س���һ���ո��Ժ�Ĳ���
                int nRet = strName.IndexOf(" ");
                if (nRet != -1)
                    strName = strName.Substring(0, nRet);

                if (String.IsNullOrEmpty(strName) == false)
                    strCommand = "word=" + strName;
            }

            string[] parts = strCommand.Split(new char[] { ';' });

            int nCount = 0;

            for (int i = 0; i < parts.Length; i++)
            {
                string strPart = parts[i].Trim();

                if (String.IsNullOrEmpty(strPart) == true)
                    continue;

                if (strPart == "(")
                {
                    strXml += "<group>";
                    continue;
                }

                if (strPart == ")")
                {
                    strXml += "</group>";
                    continue;
                }
                if (strPart.ToUpper() == "AND")
                {
                    strXml += "<operator value='AND' />";
                    continue;
                }
                if (strPart.ToUpper() == "OR")
                {
                    strXml += "<operator value='OR' />";
                    continue;
                }
                if (strPart.ToUpper() == "SUB")
                {
                    strXml += "<operator value='SUB' />";
                    continue;
                }

                Hashtable param_table = StringUtil.ParseParameters(strPart);

                //      env_param_table ����������
                Hashtable env_param_table = GetCommandEnvParamTable(node);


                // �ϲ�����������
                Hashtable result = MergeTwoTable(param_table, env_param_table);

                // 2010/10/8 ����Ҳ����ʹ��'|'����
                string strDbName = (string)result["dbname"];
                strDbName = strDbName.Replace("|", ",");

                // ���쵥������ʽ
                string strSingle = "";
                // ���ݼ�����������XML����ʽ
                // return:
                //      -1  ����
                //      0   ��������ָ�������ݿ���߼���;����һ����û��
                //      1   �ɹ�
                int nRet = LibraryApplication.BuildQueryXml(
                    this.App,
                    strDbName,
                    (string)result["word"],
                    (string)result["from"],
                    (string)result["matchstyle"],
                    (string)result["relation"],
                    (string)result["datatype"],
                    result["maxcount"] == null ? -1 : Convert.ToInt32((string)result["maxcount"]),
                    out strSingle,
                    out strError);
                if (nRet == -1)
                    return -1;
                if (nRet == 0)
                    return -1;  // TODO: OR����¿��Լ������� AND�����ֻ�ܵ����ս������

                strXml += strSingle;

                nCount++;
            }

            // TODO: �������Χ�Ѿ�����<group>Ԫ�ؾͲ�Ҫ�ظ�����
            if (nCount > 1)
                strXml = "<group>" + strXml + "</group>";

            return 0;
        }

        void channel_Idle(object sender, IdleEventArgs e)
        {
            if (this.m_bClosed == true || this.Stopped == true)
            {
                RmsChannel channel = (RmsChannel)sender;
                channel.Abort();
            }

            e.bDoEvents = false;

            System.Threading.Thread.Sleep(100);	// ����CPU��Դ���Ⱥķ�

        }

        public static string GetMyBookshelfFilename(
            LibraryApplication app,
            SessionInfo sessioninfo)
        {
            if (sessioninfo.Account == null)
                return null;

            string strUserID = sessioninfo.UserID;
            if (String.IsNullOrEmpty(strUserID) == true)
                return null;
            string strType = sessioninfo.Account.Type;
            if (String.IsNullOrEmpty(strType) == true)
                strType = "worker";
            string strDir = PathUtil.MergePath(app.DataDir + "/personaldata/" + strType, strUserID);
            PathUtil.CreateDirIfNeed(strDir);
            return PathUtil.MergePath(strDir, "mybookshelf.resultset");
        }

        // �ⲿ����
        public static int RemoveFromResultset(List<string> aPath,
            string strResultsetFilename,
            out string strError)
        {
            strError = "";
            if (File.Exists(strResultsetFilename) == false)
            {
                strError = "������ļ� '" + strResultsetFilename + "' ������";
                return -1;
            }
            DpResultSet resultset = null;


            try
            {

                resultset = new DpResultSet(false, false);
                resultset.Attach(strResultsetFilename,
        strResultsetFilename + ".index");

            }
            catch (Exception ex)
            {
                strError = "�򿪽����(�ļ�Ϊ'" + strResultsetFilename + "')��������: " + ex.Message;
                return -1;
            }

            try
            {
                for (int i = 0; i < resultset.Count; i++)
                {
                    DpRecord record = resultset[i];
                    int index = aPath.IndexOf(record.ID);
                    if (index != -1)
                    {
                        resultset.RemoveAt(i);
                        i--;
                    }
                }
            }
            finally
            {
                string strTemp1 = "";
                string strTemp2 = "";
                resultset.Detach(out strTemp1,
                    out strTemp2);
            }

            return 0;
        }

        // �ⲿ����
        public static int AddToResultset(List<string> aPath,
    string strResultsetFilename,
            bool bInsertAtFirst,
    out string strError)
        {
            strError = "";
            DpResultSet resultset = null;
            bool bCreate = false;
            try
            {
                if (File.Exists(strResultsetFilename) == true)
                {
                    resultset = new DpResultSet(false, false);
                    resultset.Attach(strResultsetFilename,
            strResultsetFilename + ".index");
                }
                else
                {
                    bCreate = true;
                    resultset = new DpResultSet(false, false);
                    resultset.Create(strResultsetFilename,
                        strResultsetFilename + ".index");

                }
            }
            catch (Exception ex)
            {
                strError = (bCreate == true ? "����" : "��")
                + "�����(�ļ�Ϊ'" + strResultsetFilename + "')��������: " + ex.Message;
                return -1;
            }

            bool bDone = false;
            try
            {
                for (int j = 0; j < aPath.Count; j++)
                {
                    Thread.Sleep(1);
                    DpRecord record = new DpRecord(aPath[j]);
                    if (bInsertAtFirst == true)
                        resultset.Insert(0, record);
                    else
                        resultset.Add(record);
                }

                bDone = true;
            }
            finally
            {
                if (bDone == true || bCreate == false)
                {
                    string strTemp1 = "";
                    string strTemp2 = "";
                    resultset.Detach(out strTemp1,
                        out strTemp2);
                }
                else
                {
                    // �����ļ��ᱻɾ��
                    resultset.Close();
                }
            }

            return 0;
        }

        // ��dp2kernel�˻�ȡ�����
        int GetResultset(RmsChannel channel,
            string strResultsetName,
            string strResultsetFilename,
            out int nCount,
            out string strError)
        {
            strError = "";
            nCount = 0;

            long lStart = 0;

            DpResultSet resultset = new DpResultSet(false, false);
            resultset.Create(strResultsetFilename,
                strResultsetFilename + ".index");

            bool bDone = false;
            try
            {

                for (; ; )
                {
                    if (this.m_bClosed == true || this.Stopped == true)
                    {
                        strError = "�ж�";
                        return -1;
                    }

                    Thread.Sleep(1);

                    List<string> aPath = null;

                    long lRet = channel.DoGetSearchResultEx(
                        strResultsetName,
                        lStart,
                        100,
                        "zh",
                        null,
                        out aPath,
                        out strError);
                    if (lRet == -1)
                        return -1;

                    long lHitCount = lRet;

                    for (int j = 0; j < aPath.Count; j++)
                    {
                        DpRecord record = new DpRecord(aPath[j]);
                        resultset.Add(record);
                    }

                    if (aPath.Count <= 0  // < 100
                        )
                        break;

                    lStart += aPath.Count;
                    nCount += aPath.Count;

                    if (lStart >= lHitCount)
                        break;
                }

                bDone = true;
            }
            finally
            {
                if (bDone == true)
                {
                    string strTemp1 = "";
                    string strTemp2 = "";
                    resultset.Detach(out strTemp1,
                        out strTemp2);
                }
                else
                {
                    // �����ļ��ᱻɾ��
                    resultset.Close();
                }
            }

            return 0;
        }

        int BuildRssFile(string strResultsetFilename,
            long nMaxCount,
            string strDirection,
            string strChannelTitle,
            string strChannelLink,
            string strSelfLink,
            string strChannelDescription,
            string strOutputFilename,
            out int nOutputCount,
            out string strError)
        {
            strError = "";
            nOutputCount = 0;
            int nRet = 0;

            if (String.IsNullOrEmpty(strOutputFilename) == true)
                strOutputFilename = strResultsetFilename + ".rss";
            bool bDone = false;

            /*
            if (File.Exists(strOutputFilename) == true)
                return 0;
             * */


            XmlTextWriter writer = new XmlTextWriter(strOutputFilename,
                Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;

            DpResultSet resultset = null;
            this.App.ResultsetLocks.LockForRead(strResultsetFilename + ".rss", 500);

            try
            {
                resultset = new DpResultSet(false, false);
                try
                {
                    resultset.Attach(strResultsetFilename,
                        strResultsetFilename + ".index");
                }
                catch (Exception ex)
                {
                    strError = "�򿪽�����ļ�ʱ����: " + ex.Message;
                    return -1;
                }

                try
                {

                    writer.WriteStartDocument();
                    writer.WriteStartElement("rss");
                    writer.WriteAttributeString("version", "2.0");

                    writer.WriteAttributeString("xmlns", "dc", null,
                        "http://purl.org/dc/elements/1.1/");
                    writer.WriteAttributeString("xmlns", "atom", null,
                        "http://www.w3.org/2005/Atom");
                    writer.WriteAttributeString("xmlns", "content", null,
    "http://purl.org/rss/1.0/modules/content/");

                    writer.WriteStartElement("channel");

                    writer.WriteStartElement("title");
                    writer.WriteString(strChannelTitle);
                    writer.WriteEndElement();

                    writer.WriteStartElement("link");
                    writer.WriteString(strChannelLink);
                    writer.WriteEndElement();

                    writer.WriteStartElement("atom", "link",
                        "http://www.w3.org/2005/Atom");
                    writer.WriteAttributeString("href", strSelfLink);
                    writer.WriteAttributeString("rel", "self");
                    writer.WriteAttributeString("type", "application/rss+xml");
                    writer.WriteEndElement();

                    writer.WriteStartElement("description");
                    writer.WriteString(strChannelDescription);
                    writer.WriteEndElement();

                    writer.WriteStartElement("lastBuildDate");
                    writer.WriteString(DateTimeUtil.Rfc1123DateTimeString(DateTime.Now.ToUniversalTime()));
                    writer.WriteEndElement();

                    /*
                    // �����δ��¼
                    if (sessioninfo.Account == null)
                    {
                        // ģ��һ������getbibliosummaryȨ�޵��û�
                        sessioninfo.Account = new Account();
                        sessioninfo.Account.Rights = "getbibliosummary";
                    }
                     * */

                    // ��ʱ��SessionInfo����
                    SessionInfo sessioninfo = new SessionInfo(this.App);

                    // ģ��һ���˻�
                    Account account = new Account();
                    account.LoginName = "CacheBuilder";
                    account.Password = "";
                    account.Rights = "getbibliosummary";

                    account.Type = "";
                    account.Barcode = "";
                    account.Name = "CacheBuilder";
                    account.UserID = "CacheBuilder";
                    account.RmsUserName = this.App.ManagerUserName;
                    account.RmsPassword = this.App.ManagerPassword;

                    sessioninfo.Account = account;

                    long nCount = resultset.Count;
                    long nStart = 0;

                    if (strDirection == "head" && nMaxCount != -1)
                        nCount = Math.Min(nMaxCount, nCount);
                    else if (strDirection == "tail" && nMaxCount != -1)
                    {
                        nStart = nCount - nMaxCount;
                        if (nStart < 0)
                            nStart = 0;
                    }

                    for (long i = nStart; i < nCount; i++)
                    {
                        if (this.m_bClosed == true || this.Stopped == true)
                        {
                            strError = "�ж�";
                            return -1;
                        }

                        DpRecord record = resultset[i];

                        string strPath = record.ID;

                        // TODO: ����ʵ����¼������ע���¼�����Լ������������Ŀ��¼��ȡ�������ȣ�
                        // ������ע���¼�������������
                        // ʵ����¼����link�� book.aspx?itemrecpath=???


                        string strBiblioDbName = "";
                        string strDbName = ResPath.GetDbName(strPath);
                        string strDbType = this.App.GetDbType(strDbName,
                            out strBiblioDbName);
                        if (strDbType == null)
                        {
                            strError = "���ݿ� '"+strDbName+"' �������޷�ʶ��";
                            return -1;
                        }


                        string strItemMetadata = "";
                        XmlDocument itemdom = null;
                        string strBiblioRecPath = "";
                        if (strDbType == "item" || strDbType == "comment")
                        {
                            RmsChannel channel = this.RmsChannels.GetChannel(this.App.WsUrl);
                            if (channel == null)
                            {
                                strError = "get channel error";
                                return -1;
                            }
                            string strItemXml = "";
                            byte[] item_timestamp = null;
                            string strItemOutputPath = "";
                            long lRet = channel.GetRes(strPath,
                                out strItemXml,
                                out strItemMetadata,
                                out item_timestamp,
                                out strItemOutputPath,
                                out strError);
                            if (lRet == -1)
                            {
                                strError = "��ȡ��¼ '"+strPath+"' ʱ��������: " + strError;
                                return -1;
                            }

                            nRet = LibraryApplication.LoadToDom(strItemXml,
                                out itemdom,
                                out strError);
                            if (nRet == -1)
                            {
                                strError = "װ�ؼ�¼ '"+strPath+"' ����XML DOMʱ��������: " + strError;
                                return -1;
                            }

                            string strParentID = DomUtil.GetElementText(itemdom.DocumentElement,
                                "parent");
                            strBiblioRecPath = strBiblioDbName + "/" + strParentID;
                        }
                        else if (strDbType == "biblio")
                        {
                            strBiblioRecPath = strPath;
                        }
                        
                        // �����ݿ��л�ȡ
                        string strBiblioXml = "";
                        byte[] timestamp = null;


                        string[] formats = new string[3];
                        formats[0] = "xml";
                        formats[1] = "summary";
                        formats[2] = "metadata";

                        string[] results = null;

                        LibraryServerResult result = this.App.GetBiblioInfos(
                            sessioninfo,
                            strBiblioRecPath,
                            formats,
                            out results,
                            out timestamp);
                        if (result.Value == -1)
                        {
                            strError = result.ErrorInfo;
                            return -1;
                        }

                        if (result.Value == 0)
                            continue;   // TODO: ����һ��ռλ��¼?

                        if (results == null || results.Length != 3)
                        {
                            strError = "results error";
                            return -1;
                        }

                        strBiblioXml = results[0];
                        string strSummary = results[1];
                        string strBiblioMetaData = results[2];

                        string strMetaData = "";

                        if (strDbType == "biblio")
                            strMetaData = strBiblioMetaData;
                        else
                            strMetaData = strItemMetadata;

                        // ȡmetadata
                        Hashtable values = rmsUtil.ParseMedaDataXml(strMetaData,
                            out strError);
                        if (values == null)
                        {
                            strError = "parse metadata error: " + strError;
                            return -1;
                        }

                        string strPubDate = "";

                        string strLastModified = (string)values["lastmodified"];
                        if (String.IsNullOrEmpty(strLastModified) == false)
                        {
                            DateTime time = DateTime.Parse(strLastModified);
                            strPubDate = DateTimeUtil.Rfc1123DateTimeString(time.ToUniversalTime());
                        }

                        string strTitle = "";
                        string strLink = "";
                        List<string> authors = null;

                        if (strDbType == "biblio")
                            strLink = this.App.OpacServerUrl + "/book.aspx?BiblioRecPath=" + HttpUtility.UrlEncode(strBiblioRecPath);
                        else if (strDbType == "item")
                            strLink = this.App.OpacServerUrl + "/book.aspx?ItemRecPath=" + HttpUtility.UrlEncode(strPath) + "#active";
                        else if (strDbType == "comment")
                            strLink = this.App.OpacServerUrl + "/book.aspx?CommentRecPath=" + HttpUtility.UrlEncode(strPath) + "#active";

                        if (strDbType == "biblio"
                            || strDbType == "item")
                        {
                            nRet = GetBiblioInfos(
                                strBiblioXml,
                    out strTitle,
                    out authors,
                    out strError);
                        }
                        else if (strDbType == "comment")
                        {
                            nRet = GetCommentInfos(
    itemdom,
out strTitle,
out authors,
out strError);
                        }

                        string strItemSummary = "";
                        string strItemSummaryHtml = "";
                        if (strDbType == "item" || strDbType == "comment")
                        {
                                    // ���ʵ���¼������ע��¼��ժҪ
                            nRet = GetSummary(strDbType,
            itemdom,
            "text",
            out strItemSummary,
            out strError);
                            if (nRet == -1)
                            {
                                strError = "������¼ '"+strPath+"' ��ժҪ��Ϣʱ��������: " + strError;
                                return -1;
                            }
                            // ���ʵ���¼������ע��¼��ժҪ
                            nRet = GetSummary(strDbType,
            itemdom,
            "html",
            out strItemSummaryHtml,
            out strError);
                            if (nRet == -1)
                            {
                                strError = "������¼ '" + strPath + "' ��ժҪ��Ϣʱ��������: " + strError;
                                return -1;
                            }
                        }

                        writer.WriteStartElement("item");
                        writer.WriteAttributeString("id", (i + 1).ToString());

                        writer.WriteStartElement("title");
                        writer.WriteString(strTitle);
                        writer.WriteEndElement();

                        foreach (string strAuthor in authors)
                        {
                            // writer.WriteStartElement("author");
                            writer.WriteStartElement("dc", "creator",
                                "http://purl.org/dc/elements/1.1/");
                            writer.WriteString(strAuthor);
                            writer.WriteEndElement();
                        }

                        if (String.IsNullOrEmpty(strSummary) == false
                            || String.IsNullOrEmpty(strItemSummary) == false)
                        {
                            writer.WriteStartElement("description");
                            if (String.IsNullOrEmpty(strItemSummary) == false)
                            {
                                if (strDbType == "comment")
                                    writer.WriteString(strItemSummary + "\r\n\r\n�����ڣ� " + strSummary);
                                else
                                    writer.WriteString(strItemSummary + "\r\n\r\n" + strSummary);
                            }
                            else
                                writer.WriteString(strSummary);
                            writer.WriteEndElement();
                        }

                        // <content:encoded>
                        if (String.IsNullOrEmpty(strItemSummaryHtml) == false)
                        {
                            writer.WriteStartElement("content", "encoded", "http://purl.org/rss/1.0/modules/content/");
                            if (strDbType == "comment")
                                writer.WriteCData(strItemSummaryHtml + "<br/><br/>�����ڣ� " + strSummary);
                            else
                                writer.WriteCData(strItemSummaryHtml + "<br/><br/>" + strSummary);
                            writer.WriteEndElement();
                        }

                        writer.WriteStartElement("link");
                        writer.WriteString(strLink);
                        writer.WriteEndElement();

                        writer.WriteStartElement("pubDate");
                        writer.WriteString(strPubDate);
                        writer.WriteEndElement();

                        writer.WriteEndElement();   // </item>

                        nOutputCount++;
                    }

                }
                finally
                {
                    string strTemp1 = "";
                    string strTemp2 = "";
                    resultset.Detach(out strTemp1, out strTemp2);
                }


                writer.WriteEndElement();   // </channel>
                writer.WriteEndElement();   // </rss>
                writer.WriteEndDocument();
                bDone = true;
            }
            finally
            {
                this.App.EndLoop(strOutputFilename, true);

                this.App.ResultsetLocks.UnlockForRead(strResultsetFilename + ".rss");
                writer.Close();

                if (bDone == false)
                    File.Delete(strOutputFilename); // ���������ļ�Ҫɾ��������
            }

            return 0;
        }

        // ���ʵ���¼������ע��¼��ժҪ
        static int GetSummary(string strDbType,
            XmlDocument itemdom,
            string strFormat,
            out string strSummary,
            out string strError)
        {
            strError = "";
            strSummary = "";

            if (strDbType == "item")
            {
                string strBarcode = DomUtil.GetElementText(itemdom.DocumentElement,
                    "barcode");
                string strLocation = DomUtil.GetElementText(itemdom.DocumentElement,
                    "location");
                string strAccessNo = DomUtil.GetElementText(itemdom.DocumentElement,
    "accessNo");
                if (strFormat == "html")
                    strSummary = "�������: " + strBarcode + "<br/>�ݲص�: " + strLocation + "<br/>��ȡ��: " + strAccessNo;
                else
                    strSummary = "�������: " + strBarcode + " \r\n�ݲص�: " + strLocation + " \r\n��ȡ��: " + strAccessNo;
                return 0;
            }

            if (strDbType == "comment")
            {
                string strType = DomUtil.GetElementText(itemdom.DocumentElement,
                    "type"); // ����/������ѯ
                string strOrderSuggestion = DomUtil.GetElementText(itemdom.DocumentElement,
                    "orderSuggestion");
                
                string strTitle = DomUtil.GetElementText(itemdom.DocumentElement,
                    "title");

                string strContent = DomUtil.GetElementText(itemdom.DocumentElement,
    "content");

                string strDisplayName = "";
                string strCreator = "";
                XmlNode nodeCreator = itemdom.DocumentElement.SelectSingleNode("creator");
                if (nodeCreator != null)
                {
                    strDisplayName = DomUtil.GetAttr(nodeCreator, "displayName");
                    strCreator = nodeCreator.InnerText;
                }

                if (String.IsNullOrEmpty(strDisplayName) == false)
                    strCreator = "[ " + strDisplayName +" ]";

                StringBuilder text = new StringBuilder(4096);

                if (strType == "������ѯ")
                {
                    if (strOrderSuggestion == "yes")
                        text.Append("���� ���� ����\r\n");
                    else
                        text.Append("���� ��Ҫ���� ����\r\n");
                }

                if (String.IsNullOrEmpty(strTitle) == false)
                    text.Append("����: " + strTitle + "\r\n");
                if (String.IsNullOrEmpty(strTitle) == false)
                    text.Append("����: " + strCreator + "\r\n");
                if (String.IsNullOrEmpty(strContent) == false)
                    text.Append("����: \r\n" + strContent.Replace("\\r", "\r\n") + "\r\n");

                string strOperInfo = "";

                string strFirstOperator = "";
                string strTime = "";

                XmlNode node = itemdom.DocumentElement.SelectSingleNode("operations/operation[@name='create']");
                if (node != null)
                {
                    strFirstOperator = DomUtil.GetAttr(node, "operator");
                    strTime = DomUtil.GetAttr(node, "time");
                    strOperInfo += " " + "����" + ": "
                        + GetUTimeString(strTime);
                }

                node = itemdom.DocumentElement.SelectSingleNode("operations/operation[@name='lastContentModified']");
                if (node != null)
                {
                    string strLastOperator = DomUtil.GetAttr(node, "operator");
                    strTime = DomUtil.GetAttr(node, "time");
                    strOperInfo += " " + "����޸�" + ": "
                        + GetUTimeString(strTime);
                    if (strLastOperator != strFirstOperator)
                        strOperInfo += " (" + strLastOperator + ")";
                }

                if (String.IsNullOrEmpty(strOperInfo) == false)
                    text.Append(strOperInfo + "\r\n");

                if (strFormat == "html")
                    strSummary = text.ToString().Replace("\r\n", "<br/>");
                else
                    strSummary = text.ToString();
                return 0;
            }

            return 0;
        }

        public static string GetUTimeString(string strRfc1123TimeString)
        {
            if (String.IsNullOrEmpty(strRfc1123TimeString) == true)
                return "";

            DateTime time = new DateTime(0);
            try
            {
                time = DateTimeUtil.FromRfc1123DateTimeString(strRfc1123TimeString);
            }
            catch
            {
            }

            return time.ToLocalTime().ToString("u");
        }

        // �����ע��¼�ı��������
        static int GetCommentInfos(
    XmlDocument itemdom,
    out string strTitle,
    out List<string> authors,
    out string strError)
        {
            strError = "";
            strTitle = "";
            authors = new List<string>();

            strTitle = DomUtil.GetElementText(itemdom.DocumentElement,
    "title");
            string strDisplayName = "";
            string strCreator = "";
            XmlNode nodeCreator = itemdom.DocumentElement.SelectSingleNode("creator");
            if (nodeCreator != null)
            {
                strDisplayName = DomUtil.GetAttr(nodeCreator, "displayName");
                strCreator = nodeCreator.InnerText;
            }

            if (String.IsNullOrEmpty(strDisplayName) == false)
                strCreator = "[ " + strDisplayName + " ]";

            authors.Add(strCreator);

            return 0;
        }

        // �����Ŀ��¼�ı��������
        static int GetBiblioInfos(
            string strBiblioXml,
            out string strTitle,
            out List<string> authors,
            out string strError)
        {
            strError = "";

            strTitle = "";
            authors = new List<string>();

            string strOutMarcSyntax = "";
            string strMarc = "";
            // ��MARCXML��ʽ��xml��¼ת��Ϊmarc���ڸ�ʽ�ַ���
            // parameters:
            //		bWarning	==true, ��������ת��,���ϸ�Դ�����; = false, �ǳ��ϸ�Դ�����,��������󲻼���ת��
            //		strMarcSyntax	ָʾmarc�﷨,���==""�����Զ�ʶ��
            //		strOutMarcSyntax	out����������marc�����strMarcSyntax == ""�������ҵ�marc�﷨�����򷵻����������strMarcSyntax��ͬ��ֵ
            int nRet = MarcUtil.Xml2Marc(strBiblioXml,
                true,
                "", // this.CurMarcSyntax,
                out strOutMarcSyntax,
                out strMarc,
                out strError);
            if (nRet == -1)
                return -1;

            if (strOutMarcSyntax.ToLower() == "unimarc")
            {
                strTitle = MarcDocument.GetFirstSubfield(strMarc,
                    "200",
                    "a");

                for (int i = 0; ; i++)
                {
                    string strField = "";
                    string strNextFieldName = "";
                    // return:
                    //		-1	����
                    //		0	��ָ�����ֶ�û���ҵ�
                    //		1	�ҵ����ҵ����ֶη�����strField������
                    nRet = MarcDocument.GetField(strMarc,
                        null,
                        i,
                        out strField,
                        out strNextFieldName);
                    if (nRet != 1)
                        break;

                    if (StringUtil.HasHead(strField, "7") == true)
                    {
                        string strSubfield = "";
                        string strNextSubfieldName = "";
                        // return:
                        //		-1	����
                        //		0	��ָ�������ֶ�û���ҵ�
                        //		1	�ҵ����ҵ������ֶη�����strSubfield������
                        nRet = MarcDocument.GetSubfield(strField,
                            DigitalPlatform.MarcDom.ItemType.Field,
                            "a",
                            0,
                            out strSubfield,
                            out strNextSubfieldName);
                        if (String.IsNullOrEmpty(strSubfield) == true)
                            continue;

                        authors.Add(strSubfield.Substring(1));
                    }

                }
            }
            else if (strOutMarcSyntax.ToLower() == "usmarc")
            {
                strTitle = MarcDocument.GetFirstSubfield(strMarc,
        "245",
        "a");
                for (int i = 0; ; i++)
                {
                    string strField = "";
                    string strNextFieldName = "";
                    // return:
                    //		-1	����
                    //		0	��ָ�����ֶ�û���ҵ�
                    //		1	�ҵ����ҵ����ֶη�����strField������
                    nRet = MarcDocument.GetField(strMarc,
                        null,
                        i,
                        out strField,
                        out strNextFieldName);
                    if (nRet != 1)
                        break;

                    if (StringUtil.HasHead(strField, "7") == true)
                    {
                        string strSubfield = "";
                        string strNextSubfieldName = "";
                        // return:
                        //		-1	����
                        //		0	��ָ�������ֶ�û���ҵ�
                        //		1	�ҵ����ҵ������ֶη�����strSubfield������
                        nRet = MarcDocument.GetSubfield(strField,
                            DigitalPlatform.MarcDom.ItemType.Field,
                            "a",
                            0,
                            out strSubfield,
                            out strNextSubfieldName);
                        if (String.IsNullOrEmpty(strSubfield) == true)
                            continue;

                        authors.Add(strSubfield.Substring(1));
                    }

                }
            }

            return 0;
        }

        static Hashtable MergeTwoTable(Hashtable cmd, Hashtable env)
        {
            Hashtable result = new Hashtable();
            foreach (string key in cmd.Keys)
            {
                env.Remove(key);

                result.Add(key, cmd[key]);
            }

            foreach (string key in env.Keys)
            {
                result.Add(key, env[key]);
            }

            return result;
        }

        // ����һ�����������default����ֵ
        static string GetEnvParamValue(XmlNode node,
            string strDefaultParam,
            string strName)
        {
            node = node.ParentNode; // �Ӹ��ڵ��default���Կ�ʼ��

            while (node != null)
            {
                string strDefault = DomUtil.GetAttr(node, strDefaultParam);
                Hashtable param_table = StringUtil.ParseParameters(strDefault);
                if (param_table.Contains(strName) == true)
                    return (string)param_table[strName];

                node = node.ParentNode;
            }

            return null;
        }

        static Hashtable GetBuildEnvParamTable(XmlNode node)
        {
            Hashtable param_table = new Hashtable();

            // build
            string strBuild = GetEnvParamValue(node,
                "build",
                "");
            if (strBuild != null)
                param_table["build"] = strBuild;

            return param_table;
        }

        static Hashtable GetRssEnvParamTable(XmlNode node)
        {
            Hashtable param_table = new Hashtable();

            // enable
            string strEnable = GetEnvParamValue(node,
                "rssDefault",
                "enable");
            if (strEnable != null)
                param_table["enable"] = strEnable;

            // maxcount
            string strMaxCount = GetEnvParamValue(node,
                "rssDefault",
                 "maxcount");
            if (strMaxCount != null)
                param_table["maxcount"] = strMaxCount;


            return param_table;
        }

        static Hashtable GetCommandEnvParamTable(XmlNode node)
        {
            Hashtable param_table = new Hashtable();

            // dbname
            string strDbName = GetEnvParamValue(node,
                "default",
                "dbname");
            if (strDbName != null)
                param_table["dbname"] = strDbName;


            // word
            string strWord = GetEnvParamValue(node,
                "default",
                 "word");
            if (strWord != null)
                param_table["word"] = strWord;

            // from
            string strFrom = GetEnvParamValue(node,
                "default",
                 "from");
            if (strFrom != null)
                param_table["from"] = strFrom;

            // matchstyle
            string strMatchStyle = GetEnvParamValue(node,
                "default",
                 "matchstyle");
            if (strMatchStyle != null)
                param_table["matchstyle"] = strMatchStyle;


            // relation
            string strRelation = GetEnvParamValue(node,
                "default",
                "relation");
            if (strRelation != null)
                param_table["relation"] = strRelation;

            // datatype
            string strDataType = GetEnvParamValue(node,
                "default",
                "datatype");
            if (strDataType != null)
                param_table["datatype"] = strDataType;

            // maxcount
            string strMaxCount = GetEnvParamValue(node,
                "default",
                "maxcount");
            if (strMaxCount != null)
                param_table["maxcount"] = strMaxCount;

            return param_table;
        }
    }
#endif
}
