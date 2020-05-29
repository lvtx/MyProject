//#define DEBUG_LOCK

using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Diagnostics;

using DigitalPlatform;
using DigitalPlatform.Xml;
using DigitalPlatform.Text;
using DigitalPlatform.IO;
using DigitalPlatform.ResultSet;
using DigitalPlatform.Range;

namespace DigitalPlatform.rms
{
    // ���ݿ����
    public class Database
    {
        // ���ݿ���
        protected ReaderWriterLock m_lock = new ReaderWriterLock();            // �������m_lock

        // β����,��GetNewID() �� SetIf
        protected ReaderWriterLock m_TailNolock = new ReaderWriterLock();

        // ��¼��
        internal RecordLockCollection m_recordLockColl = new RecordLockCollection();

        // ����ʱ��ʱ��
        internal int m_nTimeOut = 5 * 1000; //5�� 

        internal DatabaseCollection container; // ����

        // ���ݿ���ڵ�
        internal XmlNode m_selfNode = null;

        // ���ݿ����Խڵ�
        public XmlNode m_propertyNode = null;

        //�������ݿ�ID,ǰ������@
        public string PureID = "";

        internal int KeySize = 0;

        public int m_nTimestampSeed = 0;

        private KeysCfg m_keysCfg = null;
        private BrowseCfg m_browseCfg = null;
        private bool m_bHasBrowse = true;

        internal Database(DatabaseCollection container)
        {
            this.container = container;
        }

        // ��ʼ�����ݿ���
        // parameters:
        //      node    ���ݿ����ýڵ�<database>
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        internal virtual int Initial(XmlNode node,
            out string strError)
        {
            strError = "";
            return 0;
        }


        // ������ݿ�β��
        // parameters:
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // �ߣ���ȫ��
        public int CheckTailNo(out string strError)
        {
            strError = "";

            string strRealTailNo = "";
            int nRet = 0;

            //********�����ݿ�Ӷ���**************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("CheckTailNo()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                // return:
                //		-1  ����
                //      0   δ�ҵ�
                //      1   �ҵ�
                nRet = this.GetRecordID("-1",
                    "prev",
                    out strRealTailNo,
                    out strError);
                if (nRet == -1)
                    return -1;
            }
            catch
            {
                // ???�п��ܻ�δ��ʼ�����ݿ�
            }
            finally
            {
                //***********�����ݿ�����***************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("CheckTailNo()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }

            //this.container.WriteErrorLog("�ߵ�'" + this.GetCaption("zh-cn") + "'���ݿ��CheckTailNo()������鵽����¼��Ϊ'" + strRealTailNo + "'��");

            if (nRet == 1)
            {
                //��SetIfGreaten()�������������¼�Ŵ���β��,�Զ��ƶ�β��Ϊ���
                //����������������Ǹ��ļ�¼�ų���β��ʱ
                bool bPushTailNo = false;
                this.SetIfGreaten(Convert.ToInt32(strRealTailNo),
                    false, // isExistReaderLock
                    out bPushTailNo);
            }

            return 0;
        }
        // ����strStyle���,�õ���Ӧ�ļ�¼��
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
        internal virtual int GetRecordID(string strCurrentRecordID,
            string strStyle,
            out string strOutputRecordID,
            out string strError)
        {
            strOutputRecordID = "";
            strError = "";
            return 0;
        }

        // �õ��������ڴ����
        // ������0ʱ��keysCfg����Ϊnull
        public int GetKeysCfg(out KeysCfg keysCfg,
            out string strError)
        {
            strError = "";
            keysCfg = null;

            // �Ѵ���ʱ
            if (this.m_keysCfg != null)
            {
                keysCfg = this.m_keysCfg;
                return 0;
            }

            int nRet = 0;

            string strKeysFileName = "";
     
            string strDbName = this.GetCaption("zh");
            // return:
            //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
            //		-2	û�ҵ��ڵ�
            //		-3	localname����δ�����Ϊֵ��
            //		-4	localname�ڱ��ز�����
            //		-5	���ڶ���ڵ�
            //		0	�ɹ�
            nRet = this.container.GetFileCfgItemLacalPath(strDbName + "/cfgs/keys",
                out strKeysFileName,
                out strError);

            // δ����keys���󣬰������������
            if (nRet == -2)
                return 0;

            // keys�ļ��ڱ��ز����ڣ��������������
            if (nRet == -4)
                return 0;

            if (nRet != 0)
                return -1;


            this.m_keysCfg = new KeysCfg();
            nRet = this.m_keysCfg.Initial(strKeysFileName,
                this.container.BinDir,
                out strError);
            if (nRet == -1)
            {
                this.m_keysCfg = null;
                return -1;
            }

            keysCfg = this.m_keysCfg;
            return 0;
        }

        // �õ������ʽ�ڴ����
        // ��return 0ʱ������browseCfgΪnull
        public int GetBrowseCfg(out BrowseCfg browseCfg,
            out string strError)
        {
            strError = "";
            browseCfg = null;

            if (this.m_bHasBrowse == false)
                return 0;

            // �Ѵ���ʱ
            if (this.m_browseCfg != null)
            {
                browseCfg = this.m_browseCfg;
                return 0;
            }

            string strBrowseFileName = "";
            string strDbName = this.GetCaption("zh");
            // return:
            //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
            //		-2	û�ҵ��ڵ�
            //		-3	localname����δ�����Ϊֵ��
            //		-4	localname�ڱ��ز�����
            //		-5	���ڶ���ڵ�
            //		0	�ɹ�
            int nRet = this.container.GetFileCfgItemLacalPath(strDbName + "/cfgs/browse",
                out strBrowseFileName,
                out strError);
            if (nRet == -2 || nRet == -4)
            {
                this.m_bHasBrowse = false;
                return 0;
            }
            else
            {
                this.m_bHasBrowse = true;
            }

            if (nRet != 0)
            {
                return -1;
            }

            this.m_browseCfg = new BrowseCfg();
            nRet = this.m_browseCfg.Initial(strBrowseFileName,
                this.container.BinDir,
                out strError);
            if (nRet == -1)
            {
                this.m_browseCfg = null;
                return -1;
            }

            browseCfg = this.m_browseCfg;
            return 0;
        }

        // ʱ�������
        public long GetTimestampSeed()
        {
            return this.m_nTimestampSeed++;
        }

        // �õ����ݿ�ID��ע��ǰ���"@"
        // return:
        //		���ݿ�ID,��ʽΪ:@ID
        // ��: ����ȫ
        public string FullID
        {
            get
            {
                return "@" + this.PureID;
            }
        }

        // �õ����ݿ�������߼������ŵ�һ���ַ���������
        public LogicNameItem[] GetLogicNames()
        {
            ArrayList aLogicName = new ArrayList();
            XmlNodeList captionList = this.m_propertyNode.SelectNodes("logicname/caption");
            for (int i = 0; i < captionList.Count; i++)
            {
                XmlNode captionNode = captionList[i];
                string strLang = DomUtil.GetAttr(captionNode, "lang");
                string strValue = DomUtil.GetNodeText(captionNode);

                // �п���δ�������ԣ���δ����ֵ������ô��������
                LogicNameItem item = new LogicNameItem();
                item.Lang = strLang;
                item.Value = strValue;
                aLogicName.Add(item);
            }

            LogicNameItem[] logicNames = new LogicNameItem[aLogicName.Count];

            for (int i = 0; i < aLogicName.Count; i++)
            {
                LogicNameItem item = (LogicNameItem)aLogicName[i];
                logicNames[i] = item;
            }
            return logicNames;
        }

        // �õ�ĳ���Ե����ݿ���
        // parameters:
        //      strLang ���==null����ʾʹ�����ݿ����ʵ�ʶ���ĵ�һ������
        public string GetCaption(string strLang)
        {
            XmlNode nodeCaption = null;
            string strCaption = "";

            if (String.IsNullOrEmpty(strLang) == true)
                goto END1;

            strLang = strLang.Trim();
            if (strLang == "")
                goto END1;

            // 1.�����԰汾��ȷ��
            nodeCaption = this.m_propertyNode.SelectSingleNode("logicname/caption[@lang='" + strLang + "']");
            if (nodeCaption != null)
            {
                strCaption = DomUtil.GetNodeText(nodeCaption).Trim();
                if (String.IsNullOrEmpty(strCaption) == false)
                    return strCaption;
            }

            // �����԰汾�س����ַ���
            if (strLang.Length >= 2)
            {
                string strShortLang = strLang.Substring(0, 2);//

                // 2. ��ȷ��2�ַ���
                nodeCaption = this.m_propertyNode.SelectSingleNode("logicname/caption[@lang='" + strShortLang + "']");
                if (nodeCaption != null)
                {
                    strCaption = DomUtil.GetNodeText(nodeCaption).Trim();
                    if (String.IsNullOrEmpty(strCaption) == false)
                        return strCaption;
                }

                // 3. ��ֻ��ǰ�����ַ���ͬ��
                // xpathʽ�Ӿ�������֤��xpath�±�ϰ�ߴ�1��ʼ
                nodeCaption = this.m_propertyNode.SelectSingleNode("logicname/caption[(substring(@lang,1,2)='" + strShortLang + "')]");
                if (nodeCaption != null)
                {
                    strCaption = DomUtil.GetNodeText(nodeCaption).Trim();
                    if (String.IsNullOrEmpty(strCaption) == false)
                        return strCaption;
                }

            }

        END1:

            // 4.��������ڵ�һλ��caption
            nodeCaption = this.m_propertyNode.SelectSingleNode("logicname/caption");
            if (nodeCaption != null)
                strCaption = DomUtil.GetNodeText(nodeCaption).Trim();
            if (strCaption != "")
                return strCaption;


            // 5.���һ�����԰汾��Ϣ��û��ʱ���������ݿ��id
            return this.FullID;

        }

        // �������ԣ��õ����ݵı�ǩ��
        // parameter:
        //		strLang ���԰汾
        // return:
        //		�ҵ��������ַ���
        //		û�ҵ�,�᷵�ؿ��ַ���""
        // ��: ��ȫ��
        public string GetCaptionSafety(string strLang)
        {
            //***********�����ݿ�Ӷ���******GetCaption���ܻ��׳��쳣��������try,catch
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetCaptionSafety(strLang)����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                return this.GetCaption(strLang);

            }
            finally
            {
                m_lock.ReleaseReaderLock();
                //*****************�����ݿ�����*********
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetCaptionSafety(strLang)����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }


        // �õ�ȫ�����Ե�caption��ÿ��Ԫ���ڵĸ�ʽ ���Դ���:����
        public List<string> GetAllLangCaption()
        {
            List<string> result = new List<string>();

            XmlNodeList listCaption =
                this.m_propertyNode.SelectNodes("logicname/caption");
            foreach (XmlNode nodeCaption in listCaption)
            {
                string strLang = DomUtil.GetAttr(nodeCaption, "lang");
                string strText = DomUtil.GetNodeText(nodeCaption).Trim();

                result.Add(strLang + ":" + strText);
            }

            return result;
        }

        // ��: ��ȫ��
        public List<string> GetAllLangCaptionSafety()
        {
            //***********�����ݿ�Ӷ���******GetCaption���ܻ��׳��쳣��������try,catch
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK		
			this.container.WriteDebugInfo("GetAllLangCaptionSafety()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                return GetAllLangCaption();
            }
            finally
            {
                m_lock.ReleaseReaderLock();
                //*****************�����ݿ�����*********
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetAllLangCaptionSafety()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        // �õ�ȫ����caption�����ֵ֮���÷ֺŷָ�
        public string GetAllCaption()
        {
            string strResult = "";
            XmlNodeList listCaption =
                this.m_propertyNode.SelectNodes("logicname/caption");
            foreach (XmlNode nodeCaption in listCaption)
            {
                if (strResult != "")
                    strResult += ",";
                strResult += DomUtil.GetNodeText(nodeCaption).Trim();
            }
            return strResult;
        }

        // �õ�����caption��ֵ,�Զ��ŷָ�
        // ��: ��ȫ��
        public string GetCaptionsSafety()
        {
            //***********�����ݿ�Ӷ���******GetCaption���ܻ��׳��쳣��������try,catch
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK		
			this.container.WriteDebugInfo("GetCaptionSafety()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                return GetAllCaption();
            }
            finally
            {
                m_lock.ReleaseReaderLock();
                //*****************�����ݿ�����*********
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetCaptionSafety()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        // �õ�����caption��ֵ��ID,�Զ��ŷָ�
        // ��: ��ȫ��
        public string GetAllNameSafety()
        {
            //***********�����ݿ�Ӷ���******GetCaption���ܻ��׳��쳣��������try,catch
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetAllNameSafety()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                string strAllName = GetAllCaption();
                if (strAllName != "")
                    strAllName += ",";
                strAllName += this.FullID;
                return strAllName;
            }
            finally
            {
                m_lock.ReleaseReaderLock();
                //*****************�����ݿ�����********
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetAllNameSafety()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        // �������ݿ�dbo
        // return:
        //		string���ͣ�dbo�û���
        // ��: ����ȫ��
        internal string GetDbo()
        {
            string strDboValue = "";
            if (this.m_selfNode != null)
                strDboValue = DomUtil.GetAttr(this.m_selfNode, "dbo").Trim();
            return strDboValue;
        }

        // ��: ��ȫ��  ����ԭ��:�����ݿ����ø��ڵ������
        public string DboSafety
        {
            get
            {
                //***********�����ݿ�Ӷ���******GetDbo�������죬���Բ��ü�try,catch
                m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("TypeSafety���ԣ���'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
                try
                {
                    return GetDbo();
                }
                finally
                {
                    //**********�����ݿ�����************
                    m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
					this.container.WriteDebugInfo("TypeSafety���ԣ���'" + this.GetCaption("zh-cn") + "'���ݿ�������");

#endif
                }
            }
        }

        // ˽��GetType����: �������ݿ�����
        // return:
        //		string���ͣ����ݿ�����
        // ��: ����ȫ��
        internal string GetDbType()
        {
            string strType = "";
            if (this.m_selfNode != null)
                strType = DomUtil.GetAttr(this.m_selfNode, "type").Trim();
            return strType;
        }



        // ��: ��ȫ��  ����ԭ��:�����ݿ����ø��ڵ������
        public string TypeSafety
        {
            get
            {
                //***********�����ݿ�Ӷ���******GetDbType�������죬���Բ��ü�try,catch
                m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("TypeSafety���ԣ���'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
                try
                {
                    return GetDbType();
                }
                finally
                {
                    //**********�����ݿ�����************
                    m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
					this.container.WriteDebugInfo("TypeSafety���ԣ���'" + this.GetCaption("zh-cn") + "'���ݿ�������");

#endif
                }
            }
        }

        // �õ�����ֵ
        // return
        //      ��¼β��
        // ��: ����ȫ
        private int GetTailNo()
        {
            XmlNode nodeSeed =
                this.m_propertyNode.SelectSingleNode("seed");
            if (nodeSeed == null)
            {
                throw new Exception("�����������ļ�����,δ�ҵ�<seed>Ԫ�ء�");
            }

            return System.Convert.ToInt32(DomUtil.GetNodeText(nodeSeed));
        }

        // set���ݿ��β��
        // parameter:
        //		 nSeed  �����β����
        // ��: ����ȫ
        protected void SetTailNo(int nSeed)  //��Ϊprotected����Ϊ�ڳ�ʼ��ʱ������
        {
            XmlNode nodeSeed =
                this.m_propertyNode.SelectSingleNode("seed");

            DomUtil.SetNodeText(nodeSeed,
                Convert.ToString(nSeed));

            this.container.Changed = true;
        }

        // �õ���¼��IDβ�ţ��ȼ�1�ٷ��أ�,
        // ��������ID
        // ��: ��ȫ
        // ����ԭ�򣬼�д�����޸���nodeSeed���ݣ���ʼ�ձ������ӣ����Դ�ʱ���������ٶ���д
        protected int GetNewTailNo()
        {
            //****************�����ݿ�β�ż�д��***********
            this.m_TailNolock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetNewTailNo()����'" + this.GetCaption("zh-cn") + "'���ݿ�β�ż�д����");
#endif
            try
            {
                int nTemp = GetTailNo();   //����������ַ���������ֱ����Seed++
                nTemp++;
                SetTailNo(nTemp);
                return nTemp; //GetTailNo();
            }
            finally
            {
                //***************�����ݿ�β�Ž�д��************
                this.m_TailNolock.ReleaseWriterLock();

#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetNewTailNo()����'" + this.GetCaption("zh-cn") + "'���ݿ�β�Ž�д����");
#endif
            }
        }

        // ����û��ּ�����ļ�¼�Ŵ��ڵ����ݿ��β�ţ�
        // ��Ҫ�޸�β�ţ���ʱ����������Ϊд����
        // �޸�����ٽ���Ϊ����
        // parameter:
        //		nID         ����ID
        //		isExistReaderLock   �Ƿ��Ѵ��ڶ���
        //      bPushTailNo �Ƿ��ƶ�β��
        // return:
        //		���ص�ǰ��¼��
        // ��: ��ȫ��
        protected void SetIfGreaten(int nID,
            bool isExistReaderLock,
            out bool bPushTailNo)
        {
            bPushTailNo = false;

            if (isExistReaderLock == false)
            {
                //*********�����ݿ�β�żӶ���*************
                this.m_TailNolock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("SetIfGreaten()����'" + this.GetCaption("zh-cn") + "'���ݿ�β�żӶ�����");
#endif
            }
            try
            {
                int nSavedNo = GetTailNo();
                if (nID > nSavedNo)
                {
                    // д��־
                    this.container.WriteErrorLog("�������ݿ�'" + this.GetCaption("zh-cn") + "'��ʵ��β��'" + Convert.ToString(nID) + "'���ڱ����β��'" + Convert.ToString(nSavedNo) + "'���ƶ�β�š�");
                    bPushTailNo = true;

                    //*********��������Ϊд��************
                    LockCookie lc = m_TailNolock.UpgradeToWriterLock(m_nTimeOut);
#if DEBUG_LOCK
					this.container.WriteDebugInfo("SetIfGreaten()����'" + this.GetCaption("zh-cn") + "'���ݿ�β�Ŷ�������Ϊд����");
#endif
                    try
                    {
                        SetTailNo(nID);
                    }
                    finally
                    {
                        //*************��д��Ϊ����*************
                        m_TailNolock.DowngradeFromWriterLock(ref lc);
#if DEBUG_LOCK
						this.container.WriteDebugInfo("SetIfGreaten()����'" + this.GetCaption("zh-cn") + "'���ݿ�β��д���½�Ϊ������");
#endif
                    }
                }
            }
            finally
            {
                if (isExistReaderLock == false)
                {
                    //*******�����ݿ�β�Ž����********
                    this.m_TailNolock.ReleaseReaderLock();
#if DEBUG_LOCK					
					this.container.WriteDebugInfo("SetIfGreaten()����'" + this.GetCaption("zh-cn") + "'���ݿ�β�Ž������");
#endif
                }
            }
        }

        // �����ݿ�����ı�ǩ��Ϣ��ת����TableInfo��������
        // ��������;�����Ƿ����id���������;��Ϊ�գ���ʾ��ȫ����������м���(������id)
        // parameter:
        //		strTableNames   ����;�����ƣ�֮���ö��ŷָ�
        //      bHasID          out����������;�����Ƿ���id
        //      aTableInfo      out����������TableInfo����
        //      strError        out���������س�����Ϣ
        // returns:
        //      -1  ����
        //      0   �ɹ�
        // ��: ����ȫ
        protected int TableNames2aTableInfo(string strTableNames,
            out bool bHasID,
            out List<TableInfo> aTableInfo,
            out string strError)
        {
            strError = "";
            bHasID = false;
            aTableInfo = new List<TableInfo>();

            strTableNames = strTableNames.Trim();

            int nRet = 0;

            KeysCfg keysCfg = null;
            nRet = this.GetKeysCfg(out keysCfg,
                out strError);
            if (nRet == -1)
                return -1;


            //���strTableListΪ��,�򷵻����еı�,��������ͨ��id����
            if (strTableNames == "")
            {
                if (keysCfg != null)
                {
                    nRet = keysCfg.GetTableInfosRemoveDup(out aTableInfo,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }
                return 0;
            }

            string[] aTableName = strTableNames.Split(new Char[] { ',' });
            for (int i = 0; i < aTableName.Length; i++)
            {
                string strTableName = aTableName[i].Trim();

                if (strTableName == "")
                    continue;

                if (strTableName == "__id")
                {
                    bHasID = true;
                    continue;
                }

                if (keysCfg != null)
                {
                    TableInfo tableInfo = null;
                    nRet = keysCfg.GetTableInfo(strTableName,
                        out tableInfo,
                        out strError);
                    if (nRet == 0)
                        continue;
                    if (nRet != 1)
                        return -1;
                    aTableInfo.Add(tableInfo);
                }
            }
            return 0;
        }

        // ����
        // parameter:
        //		searchItem  SearchItem����
        //		isConnected IsConnection���������ж�ͨѶ�Ƿ�����
        //		resultSet   �����������Ľ����
        //		nWarningLevel   �����漶�� 0����ʾ�ر�ǿ�ң����־���Ҳ��������1����ʾ��ǿ�ң������س�������ִ��
        //		strError    out���������س�����Ϣ
        //		strWarning  out���������ؾ�����Ϣ
        // return:
        //		-1  ����
        //		>=0 ���м�¼��
        // ��: ��ȫ��
        internal virtual int SearchByUnion(SearchItem searchItem,
            Delegate_isConnected isConnected,
            DpResultSet resultSet,
            int nWarningLevel,
            out string strError,
            out string strWarning)
        {
            strError = "";
            strWarning = "";
            return 0;
        }

        // ��ʼ�����ݿ⣬ע���麯������Ϊprivate
        // parameter:
        //		strError    out���������س�����Ϣ
        // return:
        //		-1  ����
        //		0   �ɹ�
        // ��: ��ȫ��,�������������
        public virtual int InitialPhysicalDatabase(out string strError)
        {
            strError = "";
            return 0;
        }

        // ɾ�����ݿ�
        // return:
        //      -1  ����
        //      0   �ɹ�
        public virtual int Delete(out string strError)
        {
            strError = "";
            return 0;
        }

        // ��ָ����Χ��Xml
        // parameter:
        //		strID				��¼ID
        //		nStart				��Ŀ����Ŀ�ʼλ��
        //		nLength				���� -1:��ʼ������
        //		nMaxLength			���Ƶ���󳤶�
        //		strStyle			���,data:ȡ���� prev:ǰһ����¼ next:��һ����¼
        //							withresmetadata���Ա�ʾ����Դ��Ԫ�����body���
        //							ͬʱע��ʱ��������ߺϲ����ʱ���
        //		destBuffer			out�����������ֽ�����
        //		strMetadata			out����������Ԫ����
        //		strOutputResPath	out������������ؼ�¼��·��
        //		outputTimestamp		out����������ʱ���
        //		strError			out���������س�����Ϣ
        // return:
        //		-1  ����
        //		-4  δ�ҵ���¼
        //      -10 ��¼�ֲ�δ�ҵ�
        //		>=0 ��Դ�ܳ���
        //      nAdditionError -50 ��һ�������¼���Դ��¼������
        // ��: ��ȫ��
        public virtual long GetXml(string strID,
            string strXPath,
            int nStart,
            int nLength,
            int nMaxLength,
            string strStyle,
            out byte[] destBuffer,
            out string strMetadata,
            out string strOutputResPath,
            out byte[] outputTimestamp,
            bool bCheckAccount,
            out int nAdditionError,
            out string strError)
        {
            destBuffer = null;
            strMetadata = "";
            strOutputResPath = "";
            outputTimestamp = null;
            strError = "";
            nAdditionError = 0;

            return 0;
        }


        // ��ָ����Χ������
        // strRecordID  �����ļ�¼ID
        // strObjectID  ��Դ����ID
        // ��������GetXml(),��strOutputResPath����
        // ��: ��ȫ��
        public virtual int GetObject(string strRecordID,
            string strObjectID,
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
            return 0;
        }

        // �õ�xml����
        // ��:��ȫ��,���ⲿ��
        // return:
        //      -1  ����
        //      -4  ��¼������
        //      0   ��ȷ
        public int GetXmlDataSafety(string strRecordID,
            out string strXml,
            out string strError)
        {
            strXml = "";
            strError = "";

            //********�����ݿ�Ӷ���**************
            this.m_lock.AcquireReaderLock(m_nTimeOut);

#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetXmlDataSafety()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                strRecordID = DbPath.GetID10(strRecordID);
                //*******************�Լ�¼�Ӷ���************************
                m_recordLockColl.LockForRead(strRecordID,
                    m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetXmlDataSafety()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼�Ӷ�����");
#endif

                try
                {
                    // return:
                    //      -1  ����
                    //      -4  ��¼������
                    //      0   ��ȷ
                    return this.GetXmlData(strRecordID,
                        out strXml,
                        out strError);
                }
                finally
                {
                    //************�Լ�¼�����*************
                    m_recordLockColl.UnlockForRead(strRecordID);
#if DEBUG_LOCK
					this.container.WriteDebugInfo("GetXmlDataSafety()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼�������");
#endif
                }
            }

            finally
            {
                //*********�����ݿ�����****************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetXmlDataSafety()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        // ��ȡ��¼��xml����
        // return:
        //      -1  ����
        //      -4  ��¼������
        //      0   ��ȷ
        public virtual int GetXmlData(string strRecordID,
            out string strXml,
            out string strError)
        {
            strXml = "";
            strError = "";

            return 0;
        }


        // дxml����
        // parameter:
        //		strRecordID     ��¼ID
        //		strRanges       д���Ƭ�Ϸ�Χ
        //		nTotalLength    �����ܳ���
        //		sourceBuffer    д��������ֽ�����
        //		strMetadata     Ԫ����
        //		intputTimestamp �����ʱ���
        //		outputTimestamp out���������ص�ʱ���,����ʱ,Ҳ����ʱ���
        //		strOutputID     out���������صļ�¼ID,׷�Ӽ�¼ʱ,����
        //		strError        out���������س�����Ϣ
        // return:
        // return:
        //		-1  ����
        //		-2  ʱ�����ƥ��
        //      -4  ��¼������
        //      -6  Ȩ�޲���
        //		0   �ɹ�
        // ˵��,�ܳ�����Դ����� != null,��д������,Ƭ�Ϻϲ�������Ƭ�ϼǵ�����,�������,����Ƭ��Ϊ���ַ���
        // ��: ��ȫ��
        public virtual int WriteXml(User oUser,
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

            return 0;
        }


        // parameters:
        //      strRecordID  ��¼ID
        //      strObjectID  ����ID
        //      ����ͬWriteXml,��strOutputID����
        // return:
        //		-1  ����
        //		-2  ʱ�����ƥ��
        //      -4  ��¼�������Դ������
        //      -6  Ȩ�޲���
        //		0   �ɹ�
        // ��: ��ȫ��
        public virtual int WriteObject(User user,
            string strRecordID,
            string strObjectID,
            string strRanges,
            long lTotalLength,
            byte[] baSource,
            Stream streamSource,
            string strMetadata,
            string strStyle,
            byte[] intputTimestamp,
            out byte[] outputTimestamp,
            out string strError)
        {
            outputTimestamp = intputTimestamp;
            strError = "";
            return 0;
        }

        // ɾ����¼
        // ������,����ͨ�޷�ɾ��ʱ,ʹ��ǿ�Ʒ���
        // parameter:
        //		strRecordID     ��¼ID
        //		inputTimestamp  �����ʱ���
        //		outputTimestamp out����������ʱ���,������ȷʱ��Ч
        //		strError        out���������س�����Ϣ
        // return:
        //		-1  һ���Դ���
        //		-2  ʱ�����ƥ��
        //      -4  δ�ҵ���¼
        //		0   �ɹ�
        // ��: ��ȫ��,��д��,�������������
        public virtual int DeleteRecord(string strID,
            byte[] timestamp,
            out byte[] outputTimestamp,
            out string strError)
        {
            outputTimestamp = null;
            strError = "";
            return 0;
        }

        // �õ�һ����¼�������ʽ��һ���ַ�������
        // parameter:
        //		strRecordID	һ������λ���ļ�¼�ţ���10λ���ֵļ�¼��
        //		cols	out���������������ʽ����
        // ������ʱ,������ϢҲ����������
        public void GetCols(string strRecordID,
            out string[] cols)
        {
            cols = null;

            //**********�����ݿ�Ӷ���**************
            this.m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetCols()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                BrowseCfg browseCfg = null;
                string strError = "";
                int nRet = this.GetBrowseCfg(out browseCfg,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                if (browseCfg == null)
                    return;

                // ���������ļ�Ϊdom
                string strXml;
                strRecordID = DbPath.GetID10(strRecordID);
                //*******************�Լ�¼�Ӷ���************************
                m_recordLockColl.LockForRead(strRecordID, m_nTimeOut);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetCols()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼�Ӷ�����");
#endif
                try
                {
                    // return:
                    //      -1  ����
                    //      -4  ��¼������
                    //      0   ��ȷ
                    nRet = this.GetXmlData(strRecordID,
                        out strXml,
                        out strError);
                    if (nRet <= -1)
                        goto ERROR1;
                }
                finally
                {
                    //*******************�Լ�¼�����************************
                    m_recordLockColl.UnlockForRead(strRecordID);
#if DEBUG_LOCK
				this.container.WriteDebugInfo("GetCols()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼�������");
#endif
                }

                XmlDocument domData = new XmlDocument();
                try
                {
                    domData.LoadXml(strXml);
                }
                catch (Exception ex)
                {
                    strError = "����'" + this.GetCaption("zh-cn") + "'���'" + strRecordID + "'��¼��domʱ����,ԭ��: " + ex.Message;
                    goto ERROR1;
                }

                nRet = browseCfg.BuildCols(domData,
                    out cols,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                return;

            ERROR1:
                cols = new string[1];
                cols[0] = strError;
            }
            finally
            {
                //****************�����ݿ�����**************
                this.m_lock.ReleaseReaderLock();
#if DEBUG_LOCK		
				this.container.WriteDebugInfo("GetCols()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }


        // �õ���¼��ʱ���
        // return:
        //		-1  ����
        //		-4  δ�ҵ���¼
        //      0   �ɹ�
        public virtual int GetTimestampFromDb(string strID,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            baOutputTimestamp = null;
            strError = "";


            return 0;
        }


        // ��дxml���ݣ��õ������㼯��
        // parameter:
        //		strXml	xml����
        //		strID	��¼ID,�����������
        //		strLang	���԰汾
        //		strStyle	��񣬿��Ʒ���ֵ
        //		keyColl	    out����,���ؼ����㼯�ϵ�
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        // ��: ��ȫ��
        public int PretendWrite(string strXml,
            string strRecordID,
            string strLang,
            string strStyle,
            out KeyCollection keys,
            out string strError)
        {
            keys = null;
            strError = "";
            //**********�����ݿ�Ӷ���**************
            this.m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK

			this.container.WriteDebugInfo("PretendWrite()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");

#endif
            try
            {
                //�������ݵ�DOM
                XmlDocument domData = new XmlDocument();
                domData.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue
                try
                {
                    domData.LoadXml(strXml);
                }
                catch (Exception ex)
                {
                    strError = "PretendWrite()����ز����е�xml���ݳ���ԭ��:" + ex.Message;
                    return -1;
                }

                KeysCfg keysCfg = null;
                int nRet = this.GetKeysCfg(out keysCfg,
                    out strError);
                if (nRet == -1)
                    return -1;

                if (keysCfg != null)
                {
                    //����������
                    keys = new KeyCollection();
                    nRet = keysCfg.BuildKeys(domData,
                        strRecordID,
                        strLang,
                        strStyle,
                        this.KeySize,
                        out keys,
                        out strError);
                    if (nRet == -1)
                        return -1;

                    //����ȥ��
                    keys.Sort();
                    keys.RemoveDup();
                }
                return 0;
            }
            finally
            {
                //****************�����ݿ�����**************
                this.m_lock.ReleaseReaderLock();

#if DEBUG_LOCK		
				this.container.WriteDebugInfo("PretendWrite()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        // �ϲ�������
        // return:
        //      -1  ����
        //      0   �ɹ�
        public int MergeKeys(string strID,
            string strNewXml,
            string strOldXml,
            bool bOutputDom,
            out KeyCollection newKeys,
            out KeyCollection oldKeys,
            out XmlDocument newDom,
            out XmlDocument oldDom,
            out string strError)
        {
            newKeys = null;
            oldKeys = null;
            newDom = null;
            oldDom = null;
            strError = "";

            int nRet;

            KeysCfg keysCfg = null;

            nRet = this.GetKeysCfg(out keysCfg,
                out strError);
            if (nRet == -1)
                return -1;


            // ������xml����������
            newKeys = new KeyCollection();

            if (strNewXml != "" && strNewXml != null)
            {
                newDom = new XmlDocument();
                newDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                try
                {
                    newDom.LoadXml(strNewXml);
                }
                catch (Exception ex)
                {
                    strError = "���������ݵ�domʱ����" + ex.Message;
                    return -1;
                }

                if (keysCfg != null)
                {
                    nRet = keysCfg.BuildKeys(newDom,
                        strID,
                        "zh",//strLang,
                        "",//strStyle,
                        this.KeySize,
                        out newKeys,
                        out strError);
                    if (nRet == -1)
                        return -1;
                    newKeys.Sort();
                    newKeys.RemoveDup();
                }
            }

            oldKeys = new KeyCollection();

            if (strOldXml != null && strOldXml != "")
            {
                oldDom = new XmlDocument();
                oldDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                try
                {
                    oldDom.LoadXml(strOldXml);
                }
                catch (Exception ex)
                {
                    strError = "���ؾ����ݵ�domʱ����" + ex.Message;
                    return -1;
                }

                if (keysCfg != null)
                {
                    nRet = keysCfg.BuildKeys(oldDom,
                        strID,
                        "zh",//strLang,
                        "",//strStyle,
                        this.KeySize,
                        out oldKeys,
                        out strError);
                    if (nRet == -1)
                        return -1;
                    oldKeys.Sort();
                    oldKeys.RemoveDup();
                }
            }

            // �¾ɼ�������
            KeyCollection dupKeys = new KeyCollection();
            dupKeys = KeyCollection.Merge(newKeys,
                oldKeys);

            if (bOutputDom == false)
            {
                newDom = null;
                oldDom = null;
            }
            return 0;
        }

        // Ϊ���ݿ��еļ�¼����ʱ���
        public string CreateTimestampForDb()
        {
            long lTicks = System.DateTime.UtcNow.Ticks;
            byte[] baTime = BitConverter.GetBytes(lTicks);

            byte[] baSeed = BitConverter.GetBytes(this.GetTimestampSeed());
            Array.Reverse(baSeed);

            byte[] baTimestamp = new byte[baTime.Length + baSeed.Length];
            Array.Copy(baTime,
                0,
                baTimestamp,
                0,
                baTime.Length);
            Array.Copy(baSeed,
                0,
                baTimestamp,
                baTime.Length,
                baSeed.Length);

            return ByteArray.GetHexTimeStampString(baTimestamp);
        }


        // ȷ��ID
        public string EnsureID(string strID,
            out bool bPushTailNo)
        {
            bPushTailNo = false;

            if (strID == "-1") // ׷�Ӽ�¼,GetNewTailNo()�ǰ�ȫ��
            {
                strID = Convert.ToString(GetNewTailNo());// �ӵÿ�д��
                bPushTailNo = true;
                return DbPath.GetID10(strID);
            }


            //*******�����ݿ�Ӷ���**********************
            m_TailNolock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("EnsureID()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                strID = DbPath.GetID10(strID);
                if (StringUtil.RegexCompare(@"\B[0123456789]+", strID) == false)
                {
                    throw (new Exception("��¼��:'" + strID + "'���Ϸ���"));
                }

                //��SetIfGreaten()�������������¼�Ŵ���β��,�Զ��ƶ�β��Ϊ���
                //����������������Ǹ��ļ�¼�ų���β��ʱ
                SetIfGreaten(Convert.ToInt32(strID), true,
                    out bPushTailNo);
            }
            finally
            {
                //***********�����ݿ�����**********
                this.m_TailNolock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("EnsureID()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
            return strID;
        }


        // �õ�һ�������Ϣ
        // parameters:
        //      strStyle            �����Щ�������? all��ʾȫ�� �ֱ�ָ������logicnames/type/sqldbname/keystext/browsetext
        //		logicNames	    �߼���������
        //		strType	        ���ݿ����� �Զ��ŷָ����ַ���
        //		strSqlDbName	���ݿ��������ƣ�������ݿ�Ϊ��Ϊ�ļ������ݿ⣬�򷵻�����ԴĿ¼������
        //		strKeyText	    �������ļ�����
        //		strBrowseText	�������ļ�����
        //		strError	    ������Ϣ
        // return:
        //		-1	����
        //		0	����
        public int GetInfo(
            string strStyle,
            out LogicNameItem[] logicNames,
            out string strType,
            out string strSqlDbName,
            out string strKeysText,
            out string strBrowseText,
            out string strError)
        {
            logicNames = null;
            strType = "";
            strSqlDbName = "";
            strKeysText = "";
            strBrowseText = "";
            strError = "";

            // ���滯strStyle������,���ں��洦��
            if (String.IsNullOrEmpty(strStyle) == true
                || StringUtil.IsInList("all", strStyle) == true)
            {
                strStyle = "logicnames,type,sqldbname,keystext,browsetext";
            }

            //**********�����ݿ�Ӷ���**************
            this.m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetInfo()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                logicNames = this.GetLogicNames();

                // ���type
                if (StringUtil.IsInList("type", strStyle) == true)
                    strType = this.GetDbType();

                // ���sqldbname
                if (StringUtil.IsInList("sqldbname", strStyle) == true)
                {
                    // �������ĺ������õ�����Դ��Ϣ��������ʵ������
                    strSqlDbName = this.GetSourceName();

                    if (this.container.InstanceName != "" && strSqlDbName.Length > this.container.InstanceName.Length)
                    {
                        string strPart = strSqlDbName.Substring(0, this.container.InstanceName.Length);
                        if (strPart == this.container.InstanceName)
                        {
                            strSqlDbName = strSqlDbName.Substring(this.container.InstanceName.Length + 1); //rmsService_Guestbook
                        }
                    }
                }

                string strDbName = "";
                int nRet = 0;

                // ���keystext
                if (StringUtil.IsInList("keystext", strStyle) == true)
                {
                    string strKeysFileName = "";
                    strDbName = this.GetCaption("zh");

                    // return:
                    //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
                    //		-2	û�ҵ��ڵ�
                    //		-3	localname����δ�����Ϊֵ��
                    //		-4	localname�ڱ��ز�����
                    //		-5	���ڶ���ڵ�
                    //		0	�ɹ�
                    nRet = this.container.GetFileCfgItemLacalPath(strDbName + "/cfgs/keys",
                        out strKeysFileName,
                        out strError);
                    if (nRet != 0)
                    {
                        if (nRet != -4)
                            return -1;
                    }

                    if (File.Exists(strKeysFileName) == true)
                    {
                        StreamReader sr = new StreamReader(strKeysFileName,
                            Encoding.UTF8);
                        strKeysText = sr.ReadToEnd();
                        sr.Close();
                    }

                    /*
                                    // keys�ļ�
                                    KeysCfg keysCfg = null;
                                    int nRet = this.GetKeysCfg(out keysCfg,
                                        out strError);
                                    if (nRet == -1)
                                        return -1;
                                    if (keysCfg != null)
                                    {
                                        if (keysCfg.dom != null)
                                        {
                                            TextWriter tw = new StringWriter();
                                            keysCfg.dom.Save(tw);
                                            tw.Close();
                                            strKeysText = tw.ToString();
                                        }
                                    }
                    */

                }

                // ���browsetext
                if (StringUtil.IsInList("browsetext", strStyle) == true)
                {


                    string strBrowseFileName = "";
                    strDbName = this.GetCaption("zh");
                    // return:
                    //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
                    //		-2	û�ҵ��ڵ�
                    //		-3	localname����δ�����Ϊֵ��
                    //		-4	localname�ڱ��ز�����
                    //		-5	���ڶ���ڵ�
                    //		0	�ɹ�
                    nRet = this.container.GetFileCfgItemLacalPath(strDbName + "/cfgs/browse",
                        out strBrowseFileName,
                        out strError);
                    if (nRet != 0)
                    {
                        if (nRet != -4)
                            return -1;
                    }

                    if (File.Exists(strBrowseFileName) == true)
                    {
                        StreamReader sr = new StreamReader(strBrowseFileName,
                            Encoding.UTF8);
                        strBrowseText = sr.ReadToEnd();
                        sr.Close();
                    }
                    /*
                                    // browse�ļ�
                                    BrowseCfg browseCfg = null;
                                    nRet = this.GetBrowseCfg(out browseCfg,
                                        out strError);
                                    if (nRet == -1)
                                        return -1;
                                    if (browseCfg != null)
                                    {
                                        if (browseCfg.dom != null)
                                        {
                                            TextWriter tw = new StringWriter();
                                            browseCfg.dom.Save(tw);
                                            tw.Close();
                                            strBrowseText = tw.ToString();
                                        }
                                    }
                    */
                }

                return 0;
            }
            finally
            {
                //****************�����ݿ�����**************
                this.m_lock.ReleaseReaderLock();
#if DEBUG_LOCK		
				this.container.WriteDebugInfo("GetInfo()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        public virtual string GetSourceName()
        {
            return "";
        }


        // �������ݿ�Ļ�����Ϣ
        // parameters:
        //		logicNames	        LogicNameItem���飬���µ��߼����������滻ԭ�����߼���������
        //		strType	            ���ݿ�����,�Զ��ŷָ���������file,accout��Ŀǰ��Ч����Ϊ�漰�����ļ��⣬����sql�������
        //		strSqlDbName	    ָ������Sql���ݿ����ƣ�Ŀǰ��Ч����������ݿ�Ϊ��Ϊ�ļ������ݿ⣬�򷵻�����ԴĿ¼������
        //		strkeysDefault	    keys������Ϣ
        //		strBrowseDefault	browse������Ϣ
        // return:
        //		-1	����
        //      -2  �Ѵ���ͬ�������ݿ�
        //		0	�ɹ�
        public int SetInfo(LogicNameItem[] logicNames,
            string strType,
            string strSqlDbName,
            string strKeysText,
            string strBrowseText,
            out string strError)
        {
            strError = "";

            //****************�����ݿ��д��***********
            m_TailNolock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("SetInfo()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");

#endif
            try
            {
                if (strKeysText == null)
                    strKeysText = "";
                if (strBrowseText == null)
                    strBrowseText = "";

                if (strKeysText != "")
                {
                    XmlDocument dom = new XmlDocument();
                    try
                    {
                        dom.LoadXml(strKeysText);
                    }
                    catch (Exception ex)
                    {
                        strError = "����keys�����ļ����ݵ�dom����ԭ��:" + ex.Message;
                        return -1;
                    }
                }
                if (strBrowseText != "")
                {
                    XmlDocument dom = new XmlDocument();
                    try
                    {
                        dom.LoadXml(strBrowseText);
                    }
                    catch (Exception ex)
                    {
                        strError = "����browse�����ļ����ݵ�dom����ԭ��:" + ex.Message;
                        return -1;
                    }
                }

                // ����һ���߼�����Ҳû�У�������
                string strLogicNames = "";
                for (int i = 0; i < logicNames.Length; i++)
                {
                    string strLang = logicNames[i].Lang;
                    string strLogicName = logicNames[i].Value;

                    if (strLang.Length != 2
                        && strLang.Length != 5)
                    {
                        strError = "���԰汾�ַ�������ֻ����2λ����5λ,'" + strLang + "'���԰汾���Ϸ�";
                        return -1;
                    }

                    if (this.container.IsExistLogicName(strLogicName, this) == true)
                    {
                        strError = "���ݿ����Ѵ���'" + strLogicName + "'�߼�����";
                        return -2;
                    }
                    strLogicNames += "<caption lang='" + strLang + "'>" + strLogicName + "</caption>";
                }

                // �޸�LogicName��ʹ��ȫ���滻�ķ�ʽ
                XmlNode nodeLogicName = this.m_propertyNode.SelectSingleNode("logicname");
                nodeLogicName.InnerXml = strLogicNames;


                // Ŀǰ��֧���޸�strType,strSqlDbName

                string strKeysFileName = "";//this.GetFixedCfgFileName("keys");
                string strDbName = this.GetCaption("zh");
                // return:
                //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
                //		-2	û�ҵ��ڵ�
                //		-3	localname����δ�����Ϊֵ��
                //		-4	localname�ڱ��ز�����
                //		-5	���ڶ���ڵ�
                //		0	�ɹ�
                int nRet = this.container.GetFileCfgItemLacalPath(strDbName + "/cfgs/keys",
                    out strKeysFileName,
                    out strError);
                if (nRet != 0)
                {
                    if (nRet != -2 && nRet != -4)
                        return -1;
                    else if (nRet == -2)
                    {
                        // return:
                        //		-1	����
                        //		0	�ɹ�
                        nRet = this.container.SetFileCfgItem(this.GetCaption("zh") + "/cfgs",
                            null,
                            "keys",
                            out strError);
                        if (nRet == -1)
                            return -1;

                        // return:
                        //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
                        //		-2	û�ҵ��ڵ�
                        //		-3	localname����δ�����Ϊֵ��
                        //		-4	localname�ڱ��ز�����
                        //		-5	���ڶ���ڵ�
                        //		0	�ɹ�
                        nRet = this.container.GetFileCfgItemLacalPath(this.GetCaption("zh") + "/cfgs/keys",
                            out strKeysFileName,
                            out strError);
                        if (nRet != 0)
                        {
                            if (nRet != -4)
                                return -1;
                        }
                    }
                }

                if (File.Exists(strKeysFileName) == false)
                {
                    Stream s = File.Create(strKeysFileName);
                    s.Close();
                }

                nRet = DatabaseUtil.CreateXmlFile(strKeysFileName,
                    strKeysText,
                    out strError);
                if (nRet == -1)
                    return -1;

                // �ѻ������
                this.m_keysCfg = null;



                string strBrowseFileName = "";

                // return:
                //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
                //		-2	û�ҵ��ڵ�
                //		-3	localname����δ�����Ϊֵ��
                //		-4	localname�ڱ��ز�����
                //		-5	���ڶ���ڵ�
                //		0	�ɹ�
                nRet = this.container.GetFileCfgItemLacalPath(strDbName + "/cfgs/browse",
                    out strBrowseFileName,
                    out strError);
                if (nRet != 0)
                {
                    if (nRet != -2 && nRet != -4)
                        return -1;
                    else if (nRet == -2)
                    {
                        // return:
                        //		-1	����
                        //		0	�ɹ�
                        nRet = this.container.SetFileCfgItem(this.GetCaption("zh") + "/cfgs",
                            null,
                            "browse",
                            out strError);
                        if (nRet == -1)
                            return -1;

                        // return:
                        //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
                        //		-2	û�ҵ��ڵ�
                        //		-3	localname����δ�����Ϊֵ��
                        //		-4	localname�ڱ��ز�����
                        //		-5	���ڶ���ڵ�
                        //		0	�ɹ�
                        nRet = this.container.GetFileCfgItemLacalPath(this.GetCaption("zh") + "/cfgs/browse",
                            out strBrowseFileName,
                            out strError);
                        if (nRet != 0)
                        {
                            if (nRet != -4)
                                return -1;
                        }
                    }
                }

                if (File.Exists(strBrowseFileName) == false)
                {
                    Stream s = File.Create(strBrowseFileName);
                    s.Close();
                }

                nRet = DatabaseUtil.CreateXmlFile(strBrowseFileName,
                    strBrowseText,
                    out strError);
                if (nRet == -1)
                    return -1;

                // �ѻ������
                this.m_browseCfg = null;
                this.m_bHasBrowse = true; // ȱʡֵ


                return 0;
            }
            finally
            {
                //***************�����ݿ��д��************
                m_TailNolock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("SetInfo()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            }
        }

        // �õ����ݿ������ʾ���¼�
        // parameters:
        //		oUser	�ʻ�����
        //		db	���ݿ����
        //		strLang	���԰汾
        //		aItem	out�������������ݿ������ʾ���¼�
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        public int GetDirableChildren(User user,
            string strLang,
            string strStyle,
            out ArrayList aItem,
            out string strError)
        {
            aItem = new ArrayList();
            strError = "";

            //**********�����ݿ�Ӷ���**************
            this.m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("Dir()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                // 1.��������

                foreach (XmlNode child in this.m_selfNode.ChildNodes)
                {
                    string strElementName = child.Name;
                    if (String.Compare(strElementName, "dir", true) != 0
                        && String.Compare(strElementName, "file", true) != 0)
                    {
                        continue;
                    }


                    string strChildName = DomUtil.GetAttr(child, "name");
                    if (strChildName == "")
                        continue;
                    string strCfgPath = this.GetCaption("zh-cn") + "/" + strChildName;
                    string strExistRights;
                    bool bHasRight = false;

                    ResInfoItem resInfoItem = new ResInfoItem();
                    resInfoItem.Name = strChildName;
                    if (child.Name == "dir")
                    {
                        bHasRight = user.HasRights(strCfgPath,
                            ResType.Directory,
                            "list",
                            out strExistRights);
                        if (bHasRight == false)
                            continue;

                        resInfoItem.HasChildren = true;
                        resInfoItem.Type = 4;   // Ŀ¼

                        resInfoItem.TypeString = DomUtil.GetAttr(child, "type");    // xietao 2006/6/5
                    }
                    else
                    {
                        bHasRight = user.HasRights(strCfgPath,
                            ResType.File,
                            "list",
                            out strExistRights);
                        if (bHasRight == false)
                            continue;
                        resInfoItem.HasChildren = false;
                        resInfoItem.Type = 5;   // �ļ�

                        resInfoItem.TypeString = DomUtil.GetAttr(child, "type"); // xietao 2006/6/5 add

                    }
                    aItem.Add(resInfoItem);
                }


                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                // 2.����;��

                KeysCfg keysCfg = null;
                int nRet = this.GetKeysCfg(out keysCfg,
                    out strError);
                if (nRet == -1)
                    return -1;

                if (keysCfg != null)
                {
                    List<TableInfo> aTableInfo = null;
                    nRet = keysCfg.GetTableInfosRemoveDup(
                        out aTableInfo,
                        out strError);
                    if (nRet == -1)
                        return -1;

                    // ���ڼ���;����ȫ������Ȩ��
                    for (int i = 0; i < aTableInfo.Count; i++)
                    {
                        TableInfo tableInfo = aTableInfo[i];
                        Debug.Assert(tableInfo.Dup == false, "�����������ظ����ˡ�");

                        ResInfoItem resInfoItem = new ResInfoItem();
                        resInfoItem.Type = 1;
                        resInfoItem.Name = tableInfo.GetCaption(strLang);
                        resInfoItem.HasChildren = false;

                        resInfoItem.TypeString = tableInfo.TypeString;  // xietao 2006/6/5 add

                        // �����Ҫ, �г����������µ�����
                        if (StringUtil.IsInList("alllang", strStyle) == true)
                        {
                            List<string> results = tableInfo.GetAllLangCaption();
                            string[] names = new string[results.Count];
                            results.CopyTo(names);
                            resInfoItem.Names = names;
                        }

                        aItem.Add(resInfoItem);
                    }
                }

                // ��__id
                ResInfoItem resInfoItemID = new ResInfoItem();
                resInfoItemID.Type = 1;
                resInfoItemID.Name = "__id";
                resInfoItemID.HasChildren = false;
                resInfoItemID.TypeString = "recid";

                aItem.Add(resInfoItemID);

                return 0;
            }
            finally
            {
                //***************�����ݿ�����************
                this.m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("Dir()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }

        }


        // д�����ļ�
        // parameters:
        //      strCfgItemPath  ȫ·����������
        // return:
        //		-1  һ���Դ���
        //      -2  ʱ�����ƥ��
        //		0	�ɹ�
        // �̣߳��Կ⼯���ǲ���ȫ��
        internal int WriteFileForCfgItem(string strCfgItemPath,
            string strFilePath,
            string strRanges,
            long lTotalLength,
            byte[] baSource,
            Stream streamSource,
            string strMetadata,
            string strStyle,
            byte[] baInputTimestamp,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            //**********�����ݿ��д��**************
            this.m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("Dir()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            try
            {
                // return:
                //		-1	һ���Դ���
                //		-2	ʱ�����ƥ��
                //		0	�ɹ�
                int nRet = this.container.WriteFileForCfgItem(strFilePath,
                    strRanges,
                    lTotalLength,
                    baSource,
                    streamSource,
                    strMetadata,
                    strStyle,
                    baInputTimestamp,
                    out baOutputTimestamp,
                    out strError);
                if (nRet <= -1)
                    return nRet;

                int nPosition = strCfgItemPath.IndexOf("/");
                if (nPosition == -1)
                {
                    strError = "'" + strCfgItemPath + "'·��������'/'�����Ϸ���";
                    return -1;
                }
                if (nPosition == strCfgItemPath.Length - 1)
                {
                    strError = "'" + strCfgItemPath + "'·�������'/'�����Ϸ���";
                    return -1;
                }
                string strPathWithoutDbName = strCfgItemPath.Substring(nPosition + 1);
                // ���Ϊkeys������Ѹÿ��KeysCfg�е�dom���
                if (strPathWithoutDbName == "cfgs/keys")
                {
                    this.m_keysCfg = null;
                }

                // ���Ϊbrowse����
                if (strPathWithoutDbName == "cfgs/browse")
                {
                    this.m_browseCfg = null;
                    this.m_bHasBrowse = true; // ȱʡֵ
                }

                return 0;
            }
            finally
            {
                //**********�����ݿ��д��**************
                this.m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
			this.container.WriteDebugInfo("Dir()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            }

        }


        // �淶����¼��
        // parameters:
        //      strInputRecordID    ����ļ�¼�ţ�ֻ��Ϊ '-1','?'���ߴ�����(��С�ڵ���10λ)
        //      strOututRecordID    out���������ع淶����ļ�¼��
        // return:
        //      -1  ����
        //      0   �ɹ�
        public int CanonicalizeRecordID(string strInputRecordID,
            out string strOutputRecordID,
            out string strError)
        {
            strOutputRecordID = "";
            strError = "";

            if (strInputRecordID.Length > 10)
            {
                strError = "��¼�Ų��Ϸ������ȳ���10λ��";
                return -1;
            }

            if (strInputRecordID == "?" || strInputRecordID == "-1")
            {
                strOutputRecordID = "-1";
                return 0;
            }

            if (StringUtil.IsPureNumber(strInputRecordID) == false)
            {
                strError = "��¼��'" + strInputRecordID + "'���Ϸ���";
                return -1;
            }

            strOutputRecordID = DbPath.GetID10(strInputRecordID);
            return 0;
        }
    }
}