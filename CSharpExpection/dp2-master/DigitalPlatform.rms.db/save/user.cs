//#define DEBUG_LOCK

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using System.IO;
using System.Diagnostics;

using DigitalPlatform.rms;
using DigitalPlatform.Text;
using DigitalPlatform.Text.SectionPropertyString;
using DigitalPlatform.Xml;
using DigitalPlatform.ResultSet;
using DigitalPlatform.IO;

namespace DigitalPlatform.rms
{

    // �û�
    public class User
    {
        // ���û���ӵ�е����ݿ����б�
        public List<string> aOwnerDbName = null;

        public void AddOwnerDbName(string strDbName)
        {
            m_lock.AcquireWriterLock(m_nTimeOut);

            try
            {
                if (aOwnerDbName == null)
                    aOwnerDbName = new List<string>();

                aOwnerDbName.Add(strDbName);
            }
            finally
            {
                m_lock.ReleaseWriterLock();
            }
        }

        public void RemoveOwerDbName(string strDbName)
        {
            m_lock.AcquireWriterLock(m_nTimeOut);

            try
            {
                if (aOwnerDbName == null)
                    return;
                aOwnerDbName.Remove(strDbName);
            }
            finally
            {
                m_lock.ReleaseWriterLock();
            }

        }

        public UserCollection container = null;

        private string m_strRecPath = "";   // ȫ·����ʽ: ����/��¼��

        public string RecPath
        {
            get
            {
                m_lock.AcquireReaderLock(m_nTimeOut);
                try
                {
                    return m_strRecPath;
                }
                finally
                {
                    m_lock.ReleaseReaderLock();
                }
            }
            set
            {
                m_lock.AcquireWriterLock(m_nTimeOut);
                try
                {
                    this.m_strRecPath = value;
                }
                finally
                {
                    m_lock.ReleaseWriterLock();
                }
            }
        }

        private XmlDocument m_dom = new XmlDocument();
        // private Database m_db = null;

        private string m_strName = "";

        public string Name {
            get
            {
                m_lock.AcquireReaderLock(m_nTimeOut);
                try
                {
                    return m_strName;
                }
                finally
                {
                    m_lock.ReleaseReaderLock();
                }
            }
            set
            {
                m_lock.AcquireWriterLock(m_nTimeOut);
                try
                {
                    this.m_strName = value;
                }
                finally
                {
                    m_lock.ReleaseWriterLock();
                }
            }
        }

        private string m_strSHA1Password = "";   // ������Ϊ��ǿ��ΪSHA1��̬

        public string SHA1Password
        {
            get
            {
                m_lock.AcquireReaderLock(m_nTimeOut);
                try
                {
                    return m_strSHA1Password;
                }
                finally
                {
                    m_lock.ReleaseReaderLock();
                }
            }
            set
            {
                m_lock.AcquireWriterLock(m_nTimeOut);
                try
                {
                    this.m_strSHA1Password = value;
                }
                finally
                {
                    m_lock.ReleaseWriterLock();
                }
            }
        }

        // public int Count = 0;

        private XmlNode m_nodeServer = null;

        private CfgRights cfgRights = null;

        // Ŀǰ�û����������Ҫ�����޸�ʹ����������,
        // ���Ǹĳ�Interlocked.Increment��Interlocked.Decrement
        //public ReaderWriterLock m_lock = new ReaderWriterLock();
        //public int m_nTimeOut = 5000;
        internal int m_nUseCount = 0;

        internal DateTime m_timeLastUse = DateTime.Now;

        bool m_bChanged = false;

        private ReaderWriterLock m_lock = new ReaderWriterLock();
        private static int m_nTimeOut = 5000;

        // ����һ�����ʹ��ʱ��
        public void Activate()
        {
             m_timeLastUse = DateTime.Now;
        }

        public bool Changed
        {
            get
            {
                m_lock.AcquireReaderLock(m_nTimeOut);
                try
                {
                    return m_bChanged;
                }
                finally
                {
                    m_lock.ReleaseReaderLock();
                }
            }
            set
            {
                m_lock.AcquireWriterLock(m_nTimeOut);
                try
                {
                    m_bChanged = value;
                }
                finally
                {
                    m_lock.ReleaseWriterLock();
                }
            }
        }

        /*
        public int UseCount
        {
            get
            {
                return this.m_nUseCount;
            }
        }
         */

        // ��ʼ���û�����
        // �̲߳���ȫ����Ϊ������ʱ��δ���뼯�ϣ�Ҳû�б�Ҫ�̰߳�ȫ
        // parameters:
        //      dom         �û���¼dom
        //      strResPath  ��¼·�� ȫ·�� ����/��¼��
        //      db          �����������ݿ�
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        internal int Initial(string strRecPath,
            XmlDocument dom,
            Database db,
            DatabaseCollection dbs,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            this.RecPath = strRecPath;
            this.m_dom = dom;
            // this.m_db = db;

            XmlNode root = this.m_dom.DocumentElement;
            XmlNode nodeName = root.SelectSingleNode("name");
            if (nodeName != null)
                this.Name = DomUtil.GetNodeText(nodeName).Trim();

            // �û���ӵ�е����ݿ�
            this.aOwnerDbName = null;

            if (String.IsNullOrEmpty(this.Name) == false)
            {
                List<string> aOwnerDbName = null;

                nRet = dbs.GetOwnerDbNames(this.Name,
                    out aOwnerDbName,
                    out strError);
                if (nRet == -1)
                    return -1;

                this.aOwnerDbName = aOwnerDbName;
            }




            XmlNode nodePassword = root.SelectSingleNode("password");
            if (nodePassword != null)
                SHA1Password = DomUtil.GetNodeText(nodePassword).Trim();

            XmlNode nodeRightsItem = root.SelectSingleNode("rightsItem");
            if (nodeRightsItem != null)
            {
                strError = "�ʻ���¼Ϊ�ɰ汾����Ԫ�����Ѿ���֧��<rightsItem>Ԫ�ء�";
                return -1;
            }

            // û��<server>Ԫ���Ƿ񰴳�����
            this.m_nodeServer = root.SelectSingleNode("server");
            if (this.m_nodeServer == null)
            {
                strError = "�ʻ���¼δ����<server>Ԫ�ء�";
                return -1;
            }

            this.cfgRights = new CfgRights();
            // return:
            //      -1  ����
            //      0   �ɹ�
            nRet = this.cfgRights.Initial(this.m_nodeServer,
                out strError);
            if (nRet == -1)
                return -1;

            return 0;
        }


        // �õ�Ƭ�ϵĶ�����Ϣ
        // parameters:
        //      strRights   ��Ȩ��
        //      strCategory ����
        // return:
        //      �ҵ��ֶ����Ȩ�ޣ�������������
        private string GetSectionRights(string strRights,
            string strCategory)
        {
            DigitalPlatform.Text.SectionPropertyString.PropertyCollection propertyColl =
                new DigitalPlatform.Text.SectionPropertyString.PropertyCollection("this",
                strRights,
                DelimiterFormat.Semicolon);
            Section section = propertyColl[strCategory];
            if (section == null)
                return "";

            return section.Value;
        }


        // �������ʻ��Ƿ��ָ��������������ָ����Ȩ��
        // �̰߳�ȫ
        // parameters:
        //		strPath	Ҫ������Ȩ�޵Ķ������Դ·��
        //      resType ��Դ����
        //      strOneRight ��ȷ����Ȩ��
        //		strExistRights	���ظö����Ѿ����ڵĵ�Ȩ���б�
        // return:
        //		true	��
        //		false	��
        public bool HasRights(string strPath,
            ResType resType,
            string strQueryOneRight,
            out string strExistRights)
        {
            strExistRights = "";

            m_lock.AcquireReaderLock(m_nTimeOut);

            try
            {
                ResultType resultType = new ResultType();
                string strError = "";
                int nRet = this.cfgRights.CheckRights(strPath,
                    this.aOwnerDbName,
                    this.Name,
                    resType,
                    strQueryOneRight,
                    out strExistRights,
                    out resultType,
                    out strError);
                if (nRet == -1)
                {
                    throw new Exception("CheckRights()����ԭ��" + strError);
                }

                if (resultType == ResultType.Plus)
                    return true;

                return false;
            }
            finally
            {
                m_lock.ReleaseReaderLock();
            }
        }

        // ȱʡ��Ϊ�����޸��Լ�������
        private bool CheckChangePasswordRights()
        {
            if (this.m_dom != null)
            {
                XmlNode nodePassword = this.m_dom.DocumentElement.SelectSingleNode("password");
                string strStyle = DomUtil.GetAttr(nodePassword, "style");
                if (StringUtil.IsInList("changepassworddenied", strStyle, true) == true)
                    return false;
            }
            return true;
        }

        // �޸��Լ�������
        // �̰߳�ȫ
        // parameters:
        //      strNewPassword   ����
        // return:
        //      -1  ����
        //      -4  ��¼������
        //		0   �ɹ�
        public int ChangePassword(
            string strNewPassword,
            out string strError)
        {
            strError = "";

            m_lock.AcquireWriterLock(m_nTimeOut);
            try
            {


                // �����Ƿ����޸��Լ������Ȩ��
                bool bHasChangePasswordRights = false;
                bHasChangePasswordRights = this.CheckChangePasswordRights();
                if (bHasChangePasswordRights == false)
                {
                    strError = "�����û���Ϊ '" + this.Name + "'��û���޸��Լ������Ȩ�ޡ�";
                    return -6;
                }

                this.SHA1Password = Cryptography.GetSHA1(strNewPassword);

                // �����µ�����
                XmlNode root = this.m_dom.DocumentElement;
                XmlNode nodePassword = root.SelectSingleNode("password");
                DomUtil.SetNodeText(nodePassword, this.SHA1Password);

                // �������浽�û���¼��
                // return:
                //		-1  ����
                //      -4  ��¼������
                //		0   �ɹ�
                int nRet = this.InternalSave(out strError);
                this.m_bChanged = false;
                return nRet;
            }
            finally
            {
                m_lock.ReleaseWriterLock();
            }
        }

        // ������޸��򱣴�
        // �̰߳�ȫ��InternalSave()�У��������ݿ�Ĳ�����Ȼ���̰߳�ȫ��
        // return:
        //		-1  ����
        //      -4  ��¼������
        //		0   �ɹ�
        public int SaveChanges(out string strError)
        {
            strError = "";
            if (this.Changed == false)
                return 0;
            // return:
            //		-1  ����
            //      -4  ��¼������
            //		0   �ɹ�
            int nRet = InternalSave(out strError);
            this.Changed = false;
            return nRet;
        }

        // �����ڴ�������ݿ��¼
        // ��m_bChanged״̬�޹�
        // return:
        //		-1  ����
        //      -4  ��¼������
        //		0   �ɹ�
        private int InternalSave(out string strError)
        {
            strError = "";

            if (this.container == null)
                throw new Exception("User�����container��Ա����Ϊnull");

            if (String.IsNullOrEmpty(this.m_strRecPath) == true)
            {
                strError = "InternalSaveʧ�ܣ���Ϊm_strRecPathΪ��";
                return -1;
            }

            Database db = this.container.m_dbs.GetDatabaseFromRecPath(this.m_strRecPath);
            if (db == null)
            {
                strError = "GetDatabaseFromRecPath()û���ҵ���¼·��'"+this.m_strRecPath+"'��Ӧ�����ݿ����";
                return -1;
            }

            DbPath path = new DbPath(this.RecPath);

            // ���ʻ���¼�����ݶ���һ���ֽ�����
            MemoryStream fs = new MemoryStream();
            this.m_dom.Save(fs);
            fs.Seek(0, SeekOrigin.Begin);
            byte[] baSource = new byte[fs.Length];
            fs.Read(baSource,
                0,
                baSource.Length);
            fs.Close();

            string strRange = "0-" + Convert.ToString(baSource.Length - 1);
            byte[] baInputTimestamp = null;
            byte[] baOutputTimestamp = null;
            string strOutputID = "";
            string strOutputValue = "";
            string strStyle = "ignorechecktimestamp";
            // return:
            //		-1  ����
            //		-2  ʱ�����ƥ�� // ��Ϊ�������ignorechecktimestamp,���Դ˴ε��ò����ܳ���-2�����
            //      -4  ��¼������
            //      -6  Ȩ�޲���    // �˴ε��ò����ܳ���Ȩ�޲��������
            //		0   �ɹ�
            // ��Ϊ����user����Ϊnull�����Բ����ܳ���Ȩ�޲��������
            int nRet = db.WriteXml(null, //oUser
                path.ID,
                null,
                strRange,
                baSource.Length,
                baSource,
                null,
                "",  //metadata
                strStyle,
                baInputTimestamp,
                out baOutputTimestamp,
                out strOutputID,
                out strOutputValue,
                false,  //bCheckAccount
                out strError);

            Debug.Assert(nRet != -1 && nRet != -4, "�����ܵ������");

            return nRet;
        }


        // ����һ��ʹ����
        // ��login()ʱ����
        // �����̰߳�ȫ
        public void PlusOneUse()
        {
            // out string strError
            // strError = "";

            Interlocked.Increment(ref this.m_nUseCount);

            /*
            //*********���û���д��***********
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.m_dbColl.WriteDebugInfo("PlusOneUse()�����û���д����");
#endif
            try
            {
                this.m_nUseCount++;
                return 0;
            }
            finally
            {
                //*********���û���д��*************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.container.m_dbColl.WriteDebugInfo("PlusOneUse()�����û���д����");
#endif
            }
             */
        }

        // ����һ��ʹ����
        // ��SessionʧЧ����Logout()ʱ����
        // �����̰߳�ȫ
        public void MinusOneUse()
        {   
            // out string strError
            // strError = "";

            Interlocked.Decrement(ref this.m_nUseCount);

            /*
            //*********���û���д��***********
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.m_dbColl.WriteDebugInfo("MinusOneUse()�����û���д����");
#endif
            try
            {
                this.m_nUseCount--;
                return 0;
            }
            finally
            {
                //*********���û���д��*************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.container.m_dbColl.WriteDebugInfo("MinusOneUse()�����û���д����");
#endif
            }
             */
        }
    }


    // test1
}


