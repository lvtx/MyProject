//#define DEBUG_LOCK

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

using DigitalPlatform.ResultSet;
using DigitalPlatform.IO;
using DigitalPlatform.Text;
using DigitalPlatform.Xml;
using DigitalPlatform.Range;

namespace DigitalPlatform.rms
{
    // ���ݿ⼯��
    public class DatabaseCollection : ArrayList
    {
        // �ʻ�����ָ��,�����޸��ʻ����¼ʱ��ˢ�µ�ǰ�ʻ�
        public UserCollection UserColl = null;
        public bool Changed = false;	//�����Ƿ����ı�

        public XmlNode NodeDbs = null;  //<dbs>�ڵ�
        public string DataDir = "";	// ����Ŀ¼·��
        public string InstanceName = ""; // ������ʵ����

        public string BinDir = "";//BinĿ¼��Ϊ�ű�����dll���� 2006/3/21��

        private ReaderWriterLock m_lock = new ReaderWriterLock();
        private int m_nTimeOut = 1000 * 60;	//1����

        private string m_strLogFileName = "";	//��־�ļ�����
        private string m_strDbsCfgFilePath = "";	// �������ļ���
        private XmlDocument m_dom = null;	// �������ļ�dom

        // parameter:
        //		strDataDir	dataĿ¼
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        // ��: ��ȫ��
        public int Initial(string strDataDir,
            string strBinDir,
            out string strError)
        {
            strError = "";

            //**********�Կ⼯�ϼ�д��****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("Initial()���Կ⼯�ϼ�д����");
#endif
            try
            {
                if (String.IsNullOrEmpty(strBinDir) == true)
                {
                    strError = "Initial()��strBinDir����ֵ����Ϊnull����ַ�����";
                    return -1;
                }
                this.BinDir = strBinDir;

                if (String.IsNullOrEmpty(strDataDir) == true)
                {
                    strError = "Initial()��strDataDir����ֵ����Ϊnull����ַ�����";
                    return -1;
                }

                this.DataDir = strDataDir;

                // ��־�ļ�
                string strLogDir = this.DataDir + "\\log";
                try
                {
                    PathUtil.CreateDirIfNeed(strLogDir);
                }
                catch (Exception ex)
                {
                    DirectoryInfo di = new DirectoryInfo(this.DataDir);
                    if (di.Exists == false)
                        strError = "������־Ŀ¼����: '" + ex.Message + "', ԭ�����ϼ�Ŀ¼ '" +this.DataDir+ "' ������...";
                    else
                        strError = "������־Ŀ¼����: " + ex.Message;
                    return -1;
                }
                this.m_strLogFileName = strLogDir + "\\log.txt";

                // databases.xml�����ļ�
                this.m_strDbsCfgFilePath = this.DataDir + "\\databases.xml";

                this.m_dom = new XmlDocument();
                //this.m_dom.PreserveWhitespace = true; //����հ�
                try
                {
                    this.m_dom.Load(this.m_strDbsCfgFilePath);
                }
                catch (Exception ex)
                {
                    strError = "����" + this.m_strDbsCfgFilePath + "��domʱ���� " + ex.Message;
                    return -1;
                }

                //�õ����ݿ�ڵ��б�
                this.NodeDbs = m_dom.SelectSingleNode(@"/root/dbs");
                if (this.NodeDbs == null)
                {
                    strError = "databases.xml�����ļ��в�����<dbs>�ڵ㣬�ļ����Ϸ����������ٴ��ڵ�һ���û��⡣";
                    return -1;
                }

                this.InstanceName = DomUtil.GetAttr(this.NodeDbs, "instancename");

                // �����
                this.Clear();

                // ����<database>�ڵ㴴��Database����
                int nRet = 0;
                XmlNodeList listDb = this.NodeDbs.SelectNodes("database");
                foreach (XmlNode nodeDb in listDb)
                {
                    // return:
                    //      -1  ����
                    //      0   �ɹ�
                    // �ߣ�����ȫ
                    nRet = this.AddDatabase(nodeDb,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }

                this.WriteErrorLog("�������ݿ������ϡ�");

                // ����������ݿ��¼β��
                // return:
                //      -1  ����
                //      0   �ɹ�
                // �ߣ�����ȫ
                nRet = this.CheckDbsTailNo(out strError);
                if (nRet == -1)
                    return -1;

                return 0;
            }
            finally
            {
                //***********�Կ⼯�Ͻ�д��****************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.WriteDebugInfo("Initial()���Կ⼯�Ͻ�д����");
#endif
            }
        }


        // ����node�ڵ㴴��Database���ݿ���󣬼ӵ�������
        // parameters:
        //      node    <database>�ڵ�
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // �ߣ�����ȫ
        public int AddDatabase(XmlNode node,
            out string strError)
        {
            Debug.Assert(node != null, "AddDatabase()���ô���node����ֵΪ��Ϊnull��");
            Debug.Assert(String.Compare(node.Name, "database", true) == 0, "AddDatabase()���ô���node����ֵ����Ϊ<database>�ڵ㡣");

            strError = "";

            string strType = DomUtil.GetAttr(node, "type").Trim();

            Database db = null;

            // file���ʹ���ΪFileDatabase������������ΪSqlDatabase����
            if (StringUtil.IsInList("file", strType, true) == true)
                db = new FileDatabase(this);
            else
                db = new SqlDatabase(this);

            // return:
            //		-1  ����
            //		0   �ɹ�
            int nRet = db.Initial(node,
                out strError);
            if (nRet == -1)
                return -1;

            this.Add(db);

            return 0;
        }

        // ��������
        ~DatabaseCollection()
        {
            /*
            this.Close();
            this.WriteErrorLog("����DatabaseCollection������ɡ�");
             */
        }

        public void Close()
        {
            // �����ڴ�����ļ�
            this.SaveXmlSafety();
        }


        // �Ѵ�����Ϣд����־�ļ���
        public void WriteErrorLog(string strText)
        {
            string strTime = DateTime.Now.ToString();

            try
            {
                StreamUtil.WriteText(this.m_strLogFileName,
                     strTime + " " + strText + "\r\n");
            }
            catch
            {
                // �п����ֹ����ļ�ɾ���ˣ������ļ������ڣ��׳��쳣��
            }
        }

        // �Ѵ�����Ϣд����־�ļ���
        public void WriteDebugInfo(string strText)
        {
            string strTime = DateTime.Now.ToString();

            StreamUtil.WriteText(this.DataDir + "\\debug.txt",
                 strTime + " " + strText + "\r\n");
        }

        // ����������ݿ��¼β��
        // return:
        //      -1  ����
        //      0   �ɹ�
        // �ߣ�����ȫ
        public int CheckDbsTailNo(out string strError)
        {
            strError = "";

            this.WriteErrorLog("�ߵ�CheckDbsTailNo()����ʼУ�����ݿ�β�š�");

            int nRet = 0;

            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    Database db = (Database)this[i];
                    nRet = db.CheckTailNo(out strError);
                    if (nRet == -1)
                        return -1;
                }

                // �����ڴ����
                this.SaveXml();
            }
            catch (Exception ex)
            {
                strError = "CheckDbsTailNo()����ԭ��" + ex.Message;
                return -1;
            }

            return 0;
        }


        // ���ڴ�dom���浽databases.xml�����ļ�
        // һ���ֽڵ㲻�䣬һ���ֽڵ㱻����
        // ��: ����ȫ
        public void SaveXml()
        {
            if (this.Changed == true)
            {
                XmlTextWriter w = new XmlTextWriter(this.m_strDbsCfgFilePath,
                    Encoding.UTF8);
                w.Formatting = Formatting.Indented;
                w.Indentation = 4;
                m_dom.WriteTo(w);
                w.Close();

                this.Changed = false;

                this.WriteErrorLog("��ɱ����ڴ�dom��'" + this.m_strDbsCfgFilePath + "'�ļ���");
            }
        }

        // SaveXml()�İ�ȫ�汾
        public void SaveXmlSafety()
        {
            //******************�Կ⼯�ϼ�д��******
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("SaveXmlSafety()���Կ⼯�ϼ�д����");
#endif
            try
            {
                this.SaveXml();
            }
            finally
            {
                m_lock.ReleaseWriterLock();
                //*************�Կ⼯�Ͻ�д��***********
#if DEBUG_LOCK
				this.WriteDebugInfo("SaveXmlSafety()���Կ⼯�Ͻ�д����");
#endif
            }
        }

        // ���һ���û�ӵ�е�(dbo)ȫ�����ݿ���
        public int GetOwnerDbNames(string strUserName,
            out List<string> aOwnerDbName,
            out string strError)
        {
            strError = "";

            aOwnerDbName = new List<string>();

            //******************�Կ⼯�ϼӶ���******
            this.m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("GetOwnerDbNames()���Կ⼯�ϼӶ�����");
#endif
            try
            {

                foreach (Database db in this)
                {
                    if (db.DboSafety == strUserName)
                    {
                        aOwnerDbName.Add(db.GetCaptionSafety(null));
                    }
                }

                return 0;
            }
            finally
            {
                this.m_lock.ReleaseReaderLock();
                //*****************�Կ⼯�Ͻ����*************
#if DEBUG_LOCK
				this.WriteDebugInfo("GetOwnerDbNames()���Կ⼯�Ͻ������");
#endif
            }

        }

        // �½����ݿ�
        // parameter:
        //		user	            �ʻ�����
        //		logicNames	        LogicNameItem����
        //		strType	            ���ݿ�����,�Զ��ŷָ���������file,accout
        //		strSqlDbName    	ָ����Sql���ݿ�����,����Ϊnull��ϵͳ�Զ�����һ��,��������ݿ�Ϊ��Ϊ�ļ������ݿ⣬����������ԴĿ¼������
        //		strKeysDefault  	keys������Ϣ
        //		strBrowseDefault	browse������Ϣ
        // return:
        //      -3	���½����У������Ѿ�����ͬ�����ݿ�, ���β��ܴ���
        //      -2	û���㹻��Ȩ��
        //      -1	һ���Դ�����������������Ϸ���
        //      0	�����ɹ�
        public int CreateDb(User user,
            LogicNameItem[] logicNames,
            string strType,
            string strSqlDbName,
            string strKeysDefault,
            string strBrowseDefault,
            out string strError)
        {
            strError = "";

            if (strKeysDefault == null)
                strKeysDefault = "";
            if (strBrowseDefault == null)
                strBrowseDefault = "";

            if (strKeysDefault != "")
            {
                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strKeysDefault);
                }
                catch (Exception ex)
                {
                    strError = "����keys�����ļ����ݵ�dom����ԭ��:" + ex.Message;
                    return -1;
                }
            }
            if (strBrowseDefault != "")
            {
                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strBrowseDefault);
                }
                catch (Exception ex)
                {
                    strError = "����browse�����ļ����ݵ�dom����ԭ��:" + ex.Message;
                    return -1;
                }
            }

            string strEnLoginName = "";

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

                if (this.IsExistLogicName(strLogicName, null) == true)
                {
                    strError = "���ݿ����Ѵ���'" + strLogicName + "'�߼�����";
                    return -3;  // �Ѵ�����ͬ���ݿ���
                }
                strLogicNames += "<caption lang='" + strLang + "'>" + strLogicName + "</caption>";
                if (String.Compare(logicNames[i].Lang.Substring(0, 2), "en", true) == 0)
                    strEnLoginName = strLogicName;
            }
            strLogicNames = "<logicname>" + strLogicNames + "</logicname>";

            // ��鵱ǰ�ʻ��Ƿ��д������ݿ��Ȩ��
            string strTempDbName = "test";
            if (logicNames.Length > 0)
                strTempDbName = logicNames[0].Value;
            string strExistRights = "";
            bool bHasRight = user.HasRights(strTempDbName,
                ResType.Database,
                "create",
                out strExistRights);
            if (bHasRight == false)
            {
                strError = "�����ʻ���Ϊ'" + user.Name + "'�������ݿ�û��'����(create)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                return -2;  // Ȩ�޲���
            }

            //**********�Կ⼯�ϼ�д��****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("CreateDb()���Կ⼯�ϼ�д����");
#endif
            try
            {
                if (strType == null)
                    strType = "";

                // �õ����ID
                string strDbID = Convert.ToString(this.GetNewDbID());

                string strPureCfgsDir = "";
                string strTempSqlDbName = "";
                if (strEnLoginName != "")
                {
                    strTempSqlDbName = strEnLoginName + "_db";
                    strPureCfgsDir = strEnLoginName + "_cfgs";
                }
                else
                {
                    strTempSqlDbName = "dprms_" + strDbID + "_db";
                    strPureCfgsDir = "dprms_" + strDbID + "_cfgs";
                }

                if (strSqlDbName == null || strSqlDbName == "")
                    strSqlDbName = strTempSqlDbName;

                if (StringUtil.IsInList("file", strType, true) == false)
                {
                    strSqlDbName = this.GetFinalSqlDbName(strSqlDbName);

                    if (this.IsExistSqlName(strSqlDbName) == true)
                    {
                        strError = "�����ܵ���������ݿ����Ѵ���'" + strSqlDbName + "'Sql����";
                        return -1;
                    }

                    if (this.InstanceName != "")
                        strSqlDbName = this.InstanceName + "_" + strSqlDbName;
                }

                string strDataSource = "";
                if (StringUtil.IsInList("file", strType, true) == true)
                {
                    strDataSource = strSqlDbName;

                    strDataSource = this.GetFinalDataSource(strDataSource);

                    if (this.IsExistFileDbSource(strDataSource) == true)
                    {
                        strError = "�����ܵ���������ݿ����Ѵ���''�ļ�����Ŀ¼";
                        return -1;
                    }

                    string strDataDir = this.DataDir + "\\" + strDataSource;
                    if (Directory.Exists(strDataDir) == true)
                    {
                        strError = "�����ܵ���������ز�����������Ŀ¼��";
                        return -1;
                    }

                    Directory.CreateDirectory(strDataDir);
                }

                strPureCfgsDir = this.GetFinalCfgsDir(strPureCfgsDir);
                // �������ļ�Ŀ¼�Զ�������
                string strCfgsDir = this.DataDir + "\\" + strPureCfgsDir + "\\cfgs";
                if (Directory.Exists(strCfgsDir) == true)
                {
                    strError = "�������Ѵ���'" + strPureCfgsDir + "'�����ļ�Ŀ¼����ָ��������Ӣ���߼�������";
                    return -1;
                }

                Directory.CreateDirectory(strCfgsDir);

                string strPureKeysLocalName = "keys.xml";
                string strPureBrowseLocalName = "browse.xml";

                int nRet = 0;

                // дkeys�����ļ�
                nRet = DatabaseUtil.CreateXmlFile(strCfgsDir + "\\" + strPureKeysLocalName,
                    strKeysDefault,
                    out strError);
                if (nRet == -1)
                    return -1;


                // дbrowse�����ļ�
                nRet = DatabaseUtil.CreateXmlFile(strCfgsDir + "\\" + strPureBrowseLocalName,
                    strBrowseDefault,
                    out strError);
                if (nRet == -1)
                    return -1;

                if (StringUtil.IsInList("file", strType) == true)
                    strSqlDbName = "";

                // ���﷢��xmlƬ�Ͽ��ܻ���С���⣬Ӧ����XmlTextWriter������?
                string strDbXml = "<database type='" + strType + "' id='" + strDbID + "' localdir='" + strPureCfgsDir
                    + "' dbo='"+user.Name+"'>"  // dbo����Ϊ2006/7/4����
                    + "<property>"
                    + strLogicNames
                    + "<datasource>" + strDataSource + "</datasource>"
                    + "<seed>0</seed>"
                    + "<sqlserverdb name='" + strSqlDbName + "'/>"
                    + "</property>"
                    + "<dir name='cfgs' localdir='cfgs'>"
                    + "<file name='keys' localname='" + strPureKeysLocalName + "'/>"
                    + "<file name='browse' localname='" + strPureBrowseLocalName + "'/>"
                    + "</dir>"
                    + "</database>";

                this.NodeDbs.InnerXml = this.NodeDbs.InnerXml + strDbXml;

                XmlNodeList nodeListDb = this.NodeDbs.SelectNodes("database");
                if (nodeListDb.Count == 0)
                {
                    strError = "���½����ݿ⣬������һ�����ݿⶼ�����ڡ�";
                    return -1;
                }

                // ���һ����Ϊ�½������ݿ⣬�ӵ�������
                XmlNode nodeDb = nodeListDb[nodeListDb.Count - 1];
                // return:
                //      -1  ����
                //      0   �ɹ�
                nRet = this.AddDatabase(nodeDb,
                    out strError);
                if (nRet == -1)
                    return -1;

                // ��ʱ����dbo����
                user.AddOwnerDbName(strTempDbName);

                // ��ʱ���浽database.xml
                this.Changed = true;
                this.SaveXml();
            }
            finally
            {
                m_lock.ReleaseWriterLock();
                //***********�Կ⼯�Ͻ�д��****************
#if DEBUG_LOCK
				this.WriteDebugInfo("CreateDb()���Կ⼯�Ͻ�д����");
#endif
            }
            return 0;
        }


        // �淶sql���ݿ����ƣ�ֻ�������֣���Сд���ߣ��»��ߡ�
        // ΪGetFinalSqlDbName()����ڲ�����
        private void CanonicalizeSqlDbName(ref string strSqlDbName)
        {
            if (strSqlDbName == null)
                strSqlDbName = "";

            for (int i = 0; i < strSqlDbName.Length; i++)
            {
                char myChar = strSqlDbName[i];
                if (myChar == '_')
                    continue;

                if (myChar <= '9' && myChar >= '0')
                    continue;

                if (myChar <= 'z' && myChar >= 'a')
                    continue;

                if (myChar <= 'Z' && myChar >= 'A')
                    continue;

                strSqlDbName = strSqlDbName.Remove(i, 1);
                i--;
            }
        }

        // �õ����յ�sql���ݿ�����
        private string GetFinalSqlDbName(string strSqlDbName)
        {
            if (strSqlDbName == null)
                strSqlDbName = "";

            string strRealSqlDbName = strSqlDbName;

            // �淶��Sql���ݿ�����
            this.CanonicalizeSqlDbName(ref strRealSqlDbName);


            for (int i = 0; ; i++)
            {
                if (strRealSqlDbName == "")
                {
                    strRealSqlDbName = "dprms_db_" + Convert.ToString(i);
                }

                if (this.IsExistSqlName(strRealSqlDbName) == false)
                    return strRealSqlDbName;
                else
                    strRealSqlDbName = strRealSqlDbName + Convert.ToString(i);
            }
        }

        // �淶��DataSourceĿ¼��
        // ΪGetFinalDataSource()����ڲ�����
        private void CanonicalizeDir(ref string strDataSource)
        {
            if (strDataSource == null)
                strDataSource = "";

            for (int i = 0; i < strDataSource.Length; i++)
            {
                char myChar = strDataSource[i];

                if (myChar == '\\'
                    || myChar == '/'
                    || myChar == ':'
                    || myChar == '*'
                    || myChar == '?'
                    || myChar == '<'
                    || myChar == '>'
                    || myChar == '|')
                {
                    strDataSource = strDataSource.Remove(i, 1);
                    i--;
                }
            }
        }

        // �õ����յ��ļ���ʹ�õ�����Ŀ¼
        private string GetFinalDataSource(string strDataSource)
        {
            if (strDataSource == null)
                strDataSource = "";

            string strRealDataSource = strDataSource;

            this.CanonicalizeDir(ref strRealDataSource);

            for (int i = 0; ; i++)
            {
                if (strRealDataSource == "")
                {
                    strRealDataSource = "dprms_db_" + Convert.ToString(i);
                }

                if (this.IsExistFileDbSource(strRealDataSource) == false
                    && Directory.Exists(this.DataDir + "\\" + strRealDataSource) == false)
                {
                    return strRealDataSource;
                }
                else
                {
                    strRealDataSource = strRealDataSource + Convert.ToString(i);
                }
            }
        }

        // �õ����յ����ݿ�ʹ�õ�����Ŀ¼
        private string GetFinalCfgsDir(string strCfgsDir)
        {
            if (strCfgsDir == null)
                strCfgsDir = "";

            string strRealCfgsDir = strCfgsDir;

            this.CanonicalizeDir(ref strRealCfgsDir);

            for (int i = 0; ; i++)
            {
                if (strRealCfgsDir == "")
                {
                    strRealCfgsDir = "dprms_" + Convert.ToString(i) + "_cfgs";
                }

                if (this.IsExistCfgsDir(strRealCfgsDir, null) == false
                    && Directory.Exists(this.DataDir + "\\" + strRealCfgsDir) == false)
                {
                    return strRealCfgsDir;
                }
                else
                {
                    strRealCfgsDir = strRealCfgsDir + Convert.ToString(i);
                }
            }
        }

        // ����������Ƿ��Ѵ�����ͬ��sql������
        internal bool IsExistSqlName(string strSqlName)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Database tempDb = (Database)this[i];
                if (!(tempDb is SqlDatabase))
                    continue;

                SqlDatabase sqlDb = (SqlDatabase)tempDb;
                string strDbSqlName = sqlDb.GetSourceName();// �õ�Sql���ݿ�����
                if (String.Compare(strSqlName, strDbSqlName, true) == 0)
                    return true;
            }
            return false;
        }

        // �µ�һ�����õ����ݿ�ID
        // return:
        //		��ID
        // ˵��: �ú����ڽ��ַ���IDת������ֵIDʱ�����ת�����ɹ������׳��쳣
        private int GetNewDbID()
        {
            int nId = 0;
            for (int i = 0; i < this.Count; i++)
            {
                Database db = (Database)this[i];
                int nDbId = Convert.ToInt32(db.PureID);
                if (nId < nDbId)
                    nId = nDbId;
            }
            nId = nId + 1;
            return nId;
        }

        // ��������Ŀ��������԰汾���Ƿ������ͬ���߼���
        internal bool IsExistLogicName(string strLogicName,
            Database exceptDb)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Database db = (Database)this[i];
                if (exceptDb != null)
                {
                    if (db == exceptDb)
                        continue;
                }
                string strDbAllLogicName = db.GetAllCaption();
                if (StringUtil.IsInList(strLogicName, strDbAllLogicName, true) == true)
                    return true;
            }
            return false;
        }

        // �������ݿ��Ӧ������Ŀ¼�Ƿ��ظ�
        // parameters:
        //      strCfgsDir  Ŀ¼�������Ŀ¼
        //      exceptDb    ���ο��Ƚϵ����ݿ����
        // return:
        //      true    ���ظ�
        //      false   ���ظ�
        internal bool IsExistCfgsDir(string strCfgsDir,
            Database exceptDb)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Database db = (Database)this[i];
                if (exceptDb != null)
                {
                    if (db == exceptDb)
                        continue;
                }
                string strDbCfgsDir = DatabaseUtil.GetLocalDir(this.NodeDbs,
                    db.m_selfNode);

                if (String.Compare(strCfgsDir, strDbCfgsDir, true) == 0)
                    return true;
            }
            return false;
        }

        // ����Ƿ��Ѵ�����ͬ��sql������
        internal bool IsExistFileDbSource(string strSource)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Database db = (Database)this[i];
                if (!(db is FileDatabase))
                    continue;
                string strDbSource = ((FileDatabase)db).m_strPureSourceDir;
                if (String.Compare(strSource, strDbSource, true) == 0)
                    return true;
            }
            return false;
        }


        // ɾ�����ݿ�
        // parameters:
        //		strDbName	���ݿ����ƣ������Ǹ������԰汾���߼�����Ҳ������id��
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //		-6	���㹻��Ȩ��
        //		0	�ɹ�
        public int DeleteDb(User user,
            string strDbName,
            out string strError)
        {
            strError = "";

            if (user == null)
            {
                strError = "DeleteDb()���ô���user��������Ϊnull��";
                return -1;
            }
            if (String.IsNullOrEmpty(strDbName) == true)
            {
                strError = "DeleteDb()���ô���strDbName����ֵ����Ϊnull����ַ�����";
                return -1;
            }

            //**********�Կ⼯�ϼ�д��****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("DeleteDb()���Կ⼯�ϼ�д����");
#endif
            try
            {
                Database db = this.GetDatabase(strDbName);
                if (db == null)
                {
                    strError = "δ�ҵ���Ϊ'" + strDbName + "'�����ݿ�";
                    return -1;
                }

                // ��鵱ǰ�ʻ��Ƿ���дȨ��
                string strExistRights = "";
                bool bHasRight = user.HasRights(db.GetCaption("zh-cn"),
                    ResType.Database,
                    "delete",
                    out strExistRights);
                if (bHasRight == false)
                {
                    strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strDbName + "'���ݿ�û��'ɾ��(delete)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                    return -6;
                }

                // ��database��Delete()������ɾ���ÿ�ʹ�õ������ļ������������ݿ�
                // return:
                //      -1  ����
                //      0   �ɹ�
                int nRet = db.Delete(out strError);
                if (nRet == -1)
                    return -1;

                //this.m_nodeDbs.RemoveChild(db.m_selfNode);
                List<XmlNode> nodes = DatabaseUtil.GetNodes(this.NodeDbs,
                    strDbName);
                if (nodes.Count != 1)
                {
                    strError = "δ�ҵ���Ϊ'" + db.GetCaption("zh") + "'�����ݿ⡣";
                    return -1;
                }
                this.NodeDbs.RemoveChild(nodes[0]);

                // ɾ���ڴ����
                this.Remove(db);


                // ��ʱ��ȥdbo����
                user.RemoveOwerDbName(strDbName);


                // ��ʱ���浽database.xml
                this.Changed = true;
                this.SaveXml();

                return 0;
            }
            finally
            {
                m_lock.ReleaseWriterLock();
                //***********�Կ⼯�Ͻ�д��****************
#if DEBUG_LOCK
				this.WriteDebugInfo("DeleteDb()���Կ⼯�Ͻ�д����");
#endif
            }
        }

        // ������ݶ��巽�����Ϣ
        // parameters:
        //      strStyle            �����Щ�������? all��ʾȫ�� �ֱ�ָ������logicnames/type/sqldbname/keystext/browsetext
        // return:
        //      -1  һ���Դ���
        //      -5  δ�ҵ����ݿ����
        //      -6  û���㹻��Ȩ��
        //      0   �ɹ�
        public int GetDbInfo(User user,
            string strDbName,
            string strStyle,
            out LogicNameItem[] logicNames,
            out string strType,
            out string strSqlDbName,
            out string strKeysText,
            out string strBrowseText,
            out string strError)
        {
            strError = "";

            logicNames = null;
            strType = "";
            strSqlDbName = "";
            strKeysText = "";
            strBrowseText = "";

            Debug.Assert(user != null, "GetDbInfo()���ô���user��������Ϊnull��");

            if (String.IsNullOrEmpty(strDbName) == true)
            {
                strError = "GetDbInfo()���ò��Ϸ���strDbName����ֵ����Ϊnull����ַ�����";
                return -1;
            }

            // ��鵱ǰ�ʻ��Ƿ�����ʾȨ��
            string strExistRights = "";
            bool bHasRight = user.HasRights(strDbName,
                ResType.Database,
                "read",
                out strExistRights);

            //******************�Կ⼯�ϼӶ���******
            this.m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("GetDbInfo()���Կ⼯�ϼӶ�����");
#endif
            try
            {
                Database db = this.GetDatabase(strDbName);
                if (db == null)
                {
                    strError = "δ�ҵ���Ϊ'" + strDbName + "'�����ݿ⡣";
                    return -5;
                }

                if (bHasRight == false)
                {
                    strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strDbName + "'���ݿ�û��'��(read)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                    return -6;
                }

                // return:
                //		-1	����
                //		0	����
                return db.GetInfo(
                    strStyle,
                    out logicNames,
                    out strType,
                    out strSqlDbName,
                    out strKeysText,
                    out strBrowseText,
                    out strError);
            }
            finally
            {
                this.m_lock.ReleaseReaderLock();
                //*****************�Կ⼯�Ͻ����*************
#if DEBUG_LOCK
				this.WriteDebugInfo("GetDbInfo()���Կ⼯�Ͻ������");
#endif
            }
        }

        // �������ݿ������Ϣ
        // parameter:
        //		strDbName	        ���ݿ�����
        //		strLang	            ��Ӧ�����԰汾��������԰汾Ϊnull����Ϊ���ַ�����������е����԰汾����
        //		logicNames	        LogicNameItem����
        //		strType	            ���ݿ�����,�Զ��ŷָ���������file,accout��Ŀǰ��Ч����Ϊ�漰�����ļ��⣬����sql�������
        //		strSqlDbName	    ָ������Sql���ݿ�����,��Ŀǰ��Ч
        //		strKeysDefault	    keys������Ϣ
        //		strBrowseDefault	browse������Ϣ
        // return:
        //      -1  һ���Դ���
        //      -2  �Ѵ���ͬ�������ݿ�
        //      -5  δ�ҵ����ݿ����
        //      -6  û���㹻��Ȩ��
        //      0   �ɹ�
        public int SetDbInfo(User user,
            string strDbName,
            LogicNameItem[] logicNames,
            string strType,
            string strSqlDbName,
            string strKeysText,
            string strBrowseText,
            out string strError)
        {
            strError = "";

            Debug.Assert(user != null, "SetDbInfo()���ô���user��������Ϊnull��");

            if (String.IsNullOrEmpty(strDbName) == true)
            {
                strError = "SetDbInfo()���ô���strDbName����ֵ����Ϊnull����ַ�����";
                return -1;
            }

            // Ϊ�������������⣬���鿴Ȩ�޵ĺ�������������
            // ��鵱ǰ�ʻ��Ƿ��и������ݿ�ṹ��Ȩ��
            string strExistRights = "";
            bool bHasRight = user.HasRights(strDbName,
                ResType.Database,
                "overwrite",
                out strExistRights);

            //******************�Կ⼯�ϼӶ���******
            this.m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("SetDbInfo()���Կ⼯�ϼӶ�����");
#endif
            try
            {
                Database db = this.GetDatabase(strDbName);
                if (db == null)
                {
                    strError = "δ�ҵ���Ϊ'" + strDbName + "'�����ݿ⡣";
                    return -5;
                }

                if (bHasRight == false)
                {
                    strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strDbName + "'���ݿ�û��'����(overwrite)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                    return -6;
                }

                // return:
                //		-1	����
                //      -2  �Ѵ���ͬ�������ݿ�
                //		0	�ɹ�
                int nRet = db.SetInfo(logicNames,
                    strType,
                    strSqlDbName,
                    strKeysText,
                    strBrowseText,
                    out strError);
                if (nRet <= -1)
                    return nRet;

                // ��ʱ����databases.xml
                this.Changed = true;
                this.SaveXml();

                return 0;
            }
            finally
            {
                this.m_lock.ReleaseReaderLock();
                //*****************�Կ⼯�Ͻ����*************
#if DEBUG_LOCK
				this.WriteDebugInfo("SetDbInfo()���Կ⼯�Ͻ������");
#endif
            }

        }


        // ???�Կ⼯�ϼӶ���
        // ��ʼ�����ݿ�
        // parameters:
        //      user    �ʻ�����
        //      strDbName   ���ݿ�����
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      -5  ���ݿⲻ����
        //      -6  Ȩ�޲���
        //      0   �ɹ�
        // �ߣ���ȫ ����û����
        public int InitializePhysicalDatabase(User user,
            string strDbName,
            out string strError)
        {
            strError = "";
            Debug.Assert(user != null, "InitializeDb()���ô���user����ֵ����Ϊnull��");

            if (String.IsNullOrEmpty(strDbName) == true)
            {
                strError = "InitializeDb()���ô���strDbName����ֵ����Ϊnull����ַ�����";
                return -1;
            }

            // 1.�õ����ݿ�
            Database db = this.GetDatabaseSafety(strDbName);
            if (db == null)
            {
                strError = "û���ҵ���Ϊ'" + strDbName + "'�����ݿ�";
                return -5;
            }

            string strExistRights = "";
            bool bHasRight = user.HasRights(db.GetCaption("zh-cn"),
                ResType.Database,
                "clear",
                out strExistRights);
            if (bHasRight == false)
            {
                strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strDbName + "'���ݿ�û��'��ʼ��(clear)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                return -6;
            }

            // 3.��ʼ��
            // return:
            //		-1  ����
            //		0   �ɹ�
            return db.InitialPhysicalDatabase(out strError);
        }

        // �õ�key�ĳ���
        // parameters:
        //      nKeySize    out���������ؼ����㳤��
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // ��: ����ȫ
        public int InternalGetKeySize(
            out int nKeySize,
            out string strError)
        {
            nKeySize = 0;
            strError = "";

            Debug.Assert(this.m_dom != null, "InternalGetKeySize()�﷢��this.m_domΪnull���쳣");

            XmlNode nodeKeySize = this.m_dom.DocumentElement.SelectSingleNode("keysize");
            if (nodeKeySize == null)
            {
                strError = "�����������ļ����Ϸ�,δ�ڸ��¶���<keysize>Ԫ��";
                return -1;
            }

            string strKeySize = DomUtil.GetNodeText(nodeKeySize).Trim();
            try
            {
                nKeySize = Convert.ToInt32(strKeySize);
            }
            catch (Exception ex)
            {
                strError = "�����������ļ����Ϸ������µ�<keysize>Ԫ�ص����ݲ���Ϊ'" + strKeySize + "',����Ϊ���ָ�ʽ��" + ex.Message;
                return -1;
            }

            return 0;
        }

        // �õ������ַ���,ֻ�п�����ΪSqlDatabaseʱ��������
        // parameters:
        //      strConnection   out���������������ַ�����
        //      strError        out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // ��: ����ȫ��
        internal int InternalGetConnString(
            out string strConnection,
            out string strError)
        {
            strConnection = "";
            strError = "";

            XmlNode nodeDataSource = m_dom.DocumentElement.SelectSingleNode("datasource");
            if (nodeDataSource == null)
            {
                strError = "�����������ļ����Ϸ���δ�ڸ�Ԫ���¶���<datasource>Ԫ��";
                return -1;
            }

            string strMode = DomUtil.GetAttr(nodeDataSource, "mode");



            string strServerName = DomUtil.GetAttr(nodeDataSource, "servername").Trim();
            if (strServerName == "")
            {
                strError = "�����������ļ����Ϸ���δ����Ԫ���¼���<datasource>����'servername'���ԣ���'servername'����ֵΪ�ա�";
                return -1;
            }

            string strUserID = "";
            string strPassword = "";

            if (String.IsNullOrEmpty(strMode) == true)
            {

                strUserID = DomUtil.GetAttr(nodeDataSource, "userid").Trim();
                if (strUserID == "")
                {
                    strError = "�����������ļ����Ϸ���δ����Ԫ���¼���<datasource>����'userid'���ԣ���'userid'����ֵΪ�ա�";
                    return -1;
                }

                strPassword = DomUtil.GetAttr(nodeDataSource, "password").Trim();
                if (strPassword == "")
                {
                    strError = "�����������ļ����Ϸ���δ����Ԫ���¼���<datasource>����'password'���ԣ���'password'����ֵΪ�ա�";
                    return -1;
                }
                // password����Ϊ��
                try
                {
                    strPassword = Cryptography.Decrypt(strPassword,
                            "dp2003");
                }
                catch
                {
                    strError = "�����������ļ����Ϸ�����Ԫ���¼���<datasource>����'password'����ֵ���Ϸ���";
                    return -1;
                }

                strConnection = @"Persist Security Info=False;"
    + "User ID=" + strUserID + ";"    //�ʻ�������
    + "Password=" + strPassword + ";"
                    //+ "Integrated Security=SSPI; "      //��������
    + "Data Source=" + strServerName + ";"
    + "Connect Timeout=30";

            }
            else if (strMode == "SSPI") // 2006/3/22
            {
                strConnection = @"Persist Security Info=False;"
                    + "Integrated Security=SSPI; "      //��������
                    + "Data Source=" + strServerName + ";"
                    + "Connect Timeout=30"; // 30��
            }
            else
            {
                strError = "�����������ļ����Ϸ�����Ԫ���¼���<datasource>����mode����ֵ'"+strMode+"'���Ϸ���";
                return -1;
            }

            return 0;
        }


        // �����������Զ��������ݿ����Ƹ�ʽ���ҵ���Ӧ���ݿ�
        // strName: ���ݿ��� ��ʽΪ"����" �� "@id" �� "@id[����]"
        // ��: ��ȫ��
        public Database GetDatabaseSafety(string strDbName)
        {
            //******************�Կ⼯�ϼӶ���******
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("GetDatabaseSafety()���Կ⼯�ϼӶ�����");
#endif
            try
            {
                return this.GetDatabase(strDbName);
            }
            finally
            {
                m_lock.ReleaseReaderLock();
                //*****************�Կ⼯�Ͻ����*************
#if DEBUG_LOCK
				this.WriteDebugInfo("GetDatabaseSafety()���Կ⼯�Ͻ������");
#endif
            }
        }

        // ����ָ�������԰汾���߼��������ݿ�
        // parameters:
        //		strLogicName	�߼�����
        //		strLang	���԰汾
        // return:
        //		�ҵ�����Database����
        //		û�ҵ�����null
        // ��: ��ȫ��
        public Database GetDatabaseByLogicNameSafety(string strDbName,
            string strLang)
        {
            //******************�Կ⼯�ϼӶ���******
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("GetDatabaseByLogicNameSafety()���Կ⼯�ϼӶ�����");
#endif
            try
            {
                return this.GetDatabaseByLogicName(strDbName,
                    strLang);
            }
            finally
            {
                m_lock.ReleaseReaderLock();
                //*****************�Կ⼯�Ͻ����*********
#if DEBUG_LOCK
				this.WriteDebugInfo("GetDatabaseByLogicNameSafety()���Կ⼯�Ͻ������");
#endif
            }
        }

        // �������Ƶõ�һ�����ݿ�
        // parameters:
        //		strName	���ݿ����ƣ�Ҳ������ID(ǰ���@)
        // ��: ����ȫ
        public Database GetDatabase(string strName)
        {
            if (String.IsNullOrEmpty(strName) == true)
                throw new Exception("���ݿ�������Ϊ��");

            Debug.Assert(String.IsNullOrEmpty(strName) == false, "GetDatabase()���ô���strName����ֵ����Ϊnull����ַ�����");

            string strFirst = "";
            string strSecond = "";
            int nPosition;
            nPosition = strName.LastIndexOf("[");
            if (nPosition >= 0)
            {
                strFirst = strName.Substring(0, nPosition);
                strSecond = strName.Substring(nPosition + 1);
            }
            else
            {
                strFirst = strName;
            }
            Database db = null;
            if (strFirst != "")
            {
                if (strFirst.Substring(0, 1) == "@")
                    db = GetDatabaseByID(strFirst);
                else
                    db = GetDatabaseByLogicName(strFirst);
            }
            else if (strSecond != "")
            {
                if (strSecond.Substring(0, 1) == "@")
                    db = GetDatabaseByID(strSecond);
                else
                    db = GetDatabaseByLogicName(strSecond);
            }
            return db;
        }


        // �����߼��������ݿ⣬�κ����԰汾������
        // ��: ����ȫ
        private Database GetDatabaseByLogicName(string strLogicName)
        {
            Debug.Assert(String.IsNullOrEmpty(strLogicName) == false, "GetDatabaseByLogicName()���ô���strLogicName����ֵ����Ϊnull����ַ�����");

            foreach (Database db in this)
            {
                if (StringUtil.IsInList(strLogicName,
                    db.GetCaptionsSafety()) == true)
                {
                    return db;
                }
            }
            return null;
        }

        // ����ָ�������԰汾���߼��������ݿ�
        // parameters:
        //		strLogicName	�߼�����
        //		strLang	���԰汾
        // return:
        //		�ҵ�����Database����
        //		û�ҵ�����null
        // ��: ����ȫ
        private Database GetDatabaseByLogicName(string strLogicName,
            string strLang)
        {
            foreach (Database db in this)
            {
                if (String.Compare(strLogicName, db.GetCaptionSafety(strLang)) == 0)
                {
                    return db;
                }
            }
            return null;
        }

        // ͨ�����ݿ�ID�ҵ�ָ�������ݿ⣬ע�������ID��@
        // ��: ����ȫ
        private Database GetDatabaseByID(string strDbID)
        {
            foreach (Database db in this)
            {
                if (db.FullID == strDbID)
                {
                    return db;
                }
            }
            return null;
        }

        // ����
        // parameter:
        //		strQuery	����ʽXML�ַ���
        //		resultSet	�����,���ڴ�ż������
        //		oUser	    �ʻ�����,���ڼ������ʻ���ĳ���Ƿ��ж�Ȩ��
        //  				Ϊnull,�򲻽���Ȩ�޵ļ�飬������Ȩ����
        //		isConnected	delegate����,����ͨѶ�Ƿ���������
        //					Ϊnull���򲻵�delegate����
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //      -6  Ȩ�޲���
        //		0	�ɹ�
        // ��: ��ȫ��
        public int Search(string strQuery,
            DpResultSet resultSet,
            User oUser,
            Delegate_isConnected isConnected,
            out string strError)
        {
            strError = "";

            //�Կ⼯�ϼӶ���*********************************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("Search()���Կ⼯�ϼӶ�����");
#endif
            try
            {
                if (String.IsNullOrEmpty(strQuery) == true)
                {
                    strError = "Search()���ô���strQuery����Ϊnull����ַ���";
                    return -1;
                }

                // һ�����ȸ��������m_strQuery��Ա��ֵ��
                // �����Ƿ��ǺϷ���XML�����ý������ʱ�����ж�
                resultSet.m_strQuery = strQuery;
                XmlDocument dom = new XmlDocument();
                dom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue
                try
                {
                    dom.LoadXml(strQuery);
                }
                catch (Exception ex)
                {
                    strError += "����ʽ�ַ������ص�dom����ԭ��" + ex.Message + "\r\n"
                        + "����ʽ�ַ�������:\r\n"
                        + strQuery;
                    return -1;
                }

                //����Query����
                Query query = new Query(this,
                    oUser,
                    dom);

                //���м���
                // return:
                //		-1	����
                //		-6	��Ȩ��
                //		0	�ɹ�
                int nRet = query.DoQuery(dom.DocumentElement,
                    resultSet,
                    isConnected,
                    out strError);
                if (nRet <= -1)
                    return nRet;
            }
            finally
            {
                //****************�Կ⼯�Ͻ����**************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.WriteDebugInfo("Search()���Կ⼯�Ͻ������");
#endif
            }
            return 0;
        }



        // ����һ��Դ��¼��Ŀ���¼��Ҫ���Դ��¼�ж�Ȩ�ޣ���Ŀ���¼��дȨ��
        // �ؼ�������������
        // Parameter:
        //      user                    �û�����
        //		strOriginRecordPath	    Դ��¼·��
        //		strTargetRecordPath	    Ŀ���¼·��
        //		bDeleteOriginRecord	    �Ƿ�ɾ��Դ��¼
        //      strOutputRecordPath     ����Ŀ���¼��·��������Ŀ���¼���½�һ����¼
        //      baOutputRecordTimestamp ����Ŀ���¼��ʱ���
        //		strError	������Ϣ
        // return:
        //		-1	һ���Դ���
        //      -4  δ�ҵ���¼
        //      -5  δ�ҵ����ݿ�
        //      -6  û���㹻��Ȩ��
        //      -7  ·�����Ϸ�
        //		0	�ɹ�
        public int CopyRecord(User user,
            string strOriginRecordPath,
            string strTargetRecordPath,
            bool bDeleteOriginRecord,
            out string strTargetRecordOutputPath,
            out byte[] baOutputRecordTimestamp,
            out string strError)
        {
            Debug.Assert(user != null, "CopyRecord()���ô���user������Ϊnull��");

            this.WriteErrorLog("�ߵ�CopyRecord(),strOriginRecordPath='" + strOriginRecordPath + "' strTargetRecordPath='" + strTargetRecordPath + "'");

            strTargetRecordOutputPath = "";
            baOutputRecordTimestamp = null;
            strError = "";

            if (String.IsNullOrEmpty(strOriginRecordPath) == true)
            {
                strError = "CopyRecord()���ô���strOriginRecordPath����ֵ����Ϊnul����ַ���";
                return -1;
            }
            if (String.IsNullOrEmpty(strTargetRecordPath) == true)
            {
                strError = "CopyRecord()���ô���strTargetRecordPath����ֵ����Ϊnull����ַ���";
                return -1;
            }

            long nRet = 0;

            // �õ�Դ��¼��xml
            string strOriginRecordStyle = "data,metadata,timestamp";
            byte[] baOriginRecordData = null;
            string strOriginRecordMetadata = "";
            string strOriginRecordOutputPath = "";
            byte[] baOriginRecordOutputTimestamp = null;

            int nAdditionError = 0;
            // return:
            //		-1	һ���Դ���
            //		-4	δ�ҵ�·��ָ������Դ
            //		-5	δ�ҵ����ݿ�
            //		-6	û���㹻��Ȩ��
            //		-7	·�����Ϸ�
            //		-10	δ�ҵ���¼xpath��Ӧ�Ľڵ�  // �˴ε��ò����ܳ����������
            //		>= 0	�ɹ���������󳤶�
            nRet = this.GetRes(strOriginRecordPath,
                0,
                -1,
                strOriginRecordStyle,
                user,
                -1,
                out baOriginRecordData,
                out strOriginRecordMetadata,
                out strOriginRecordOutputPath,
                out baOriginRecordOutputTimestamp,
                out nAdditionError,
                out strError);
            if (nRet <= -1)
                return (int)nRet;


            // дĿ���¼xml
            string strTargetRecordRanges = "";
            long lTargetRecordTotalLength = baOriginRecordData.Length;
            byte[] baTargetRecordData = baOriginRecordData;
            string strTargetRecordMetadata = strOriginRecordMetadata;
            string strTargetRecordStyle = "ignorechecktimestamp";
            byte[] baTargetRecordOutputTimestamp = null;
            string strTargetRecordOutputValue = "";
            // return:
            //		-1	һ���Դ���
            //		-2	ʱ�����ƥ��    // �˴����ò����ܳ����������
            //		-4	δ�ҵ�·��ָ������Դ
            //		-5	δ�ҵ����ݿ�
            //		-6	û���㹻��Ȩ��
            //		-7	·�����Ϸ�
            //		-8	�Ѿ�����ͬ��ͬ���͵���  // �˴����ò����ܳ����������
            //		-9	�Ѿ�����ͬ������ͬ���͵���  // �˴����ò����ܳ����������
            //		0	�ɹ�
            nRet = this.WriteRes(strTargetRecordPath,
                strTargetRecordRanges,
                lTargetRecordTotalLength,
                baTargetRecordData,
                null, //streamSource
                strTargetRecordMetadata,
                strTargetRecordStyle,
                null, //baInputTimestamp
                user,
                out strTargetRecordOutputPath,
                out baTargetRecordOutputTimestamp,
                out strTargetRecordOutputValue,
                out strError);
            if (nRet <= -1)
                return (int)nRet;

            // ������Դ
            byte[] baPreamble;
            string strXml = DatabaseUtil.ByteArrayToString(baOriginRecordData,
                out baPreamble);
            XmlDocument dom = new XmlDocument();
            dom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue
            try
            {
                dom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                strError = "����'" + strOriginRecordPath + "'�ļ�¼�嵽dom����ԭ��" + ex.Message;
                return -1;
            }

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(dom.NameTable);
            nsmgr.AddNamespace("dprms", DpNs.dprms);
            XmlNodeList fileList = dom.DocumentElement.SelectNodes("//dprms:file", nsmgr);
            foreach (XmlNode fileNode in fileList)
            {

                // ��ȡԴ��Դ����
                string strObjectID = DomUtil.GetAttr(fileNode, "id");
                string strOriginObjectPath = strOriginRecordPath + "/object/" + strObjectID;
                byte[] baOriginObjectData = null;
                string strOriginObjectMetadata = "";
                string strOriginObjectOutputPath = "";
                byte[] baOriginObjectOutputTimestamp = null;

                this.WriteErrorLog("�ߵ�CopyRecord(),��ȡ��Դ��Դ·��='" + strOriginObjectPath + "'");

                // int nAdditionError = 0;
                // return:
                //		-1	һ���Դ���
                //		-4	δ�ҵ�·��ָ������Դ
                //		-5	δ�ҵ����ݿ�
                //		-6	û���㹻��Ȩ��
                //		-7	·�����Ϸ�
                //		-10	δ�ҵ���¼xpath��Ӧ�Ľڵ�
                //		>= 0	�ɹ���������󳤶�
                nRet = this.GetRes(strOriginObjectPath,
                    0,
                    -1,
                    "data,metadata",
                    user,
                    -1,
                    out baOriginObjectData,
                    out strOriginObjectMetadata,
                    out strOriginObjectOutputPath,
                    out baOriginObjectOutputTimestamp,
                    out nAdditionError,
                    out strError);
                if (nRet <= -1)
                    return (int)nRet;

                // дĿ����Դ����
                string strTargetObjectPath = strTargetRecordOutputPath + "/object/" + strObjectID;
                long lTargetObjectTotalLength = baOriginObjectData.Length;
                string strTargetObjectMetadata = strOriginObjectMetadata;
                string strTargetObjectStyle = "ignorechecktimestamp";
                string strTargetObjectOutputPath = "";
                byte[] baTargetObjectOutputTimestamp = null;
                string strTargetObjectOutputValue = "";

                //this.WriteErrorLog("�ߵ�CopyRecord(),д��Դ��Ŀ��·��='" + strTargetObjectPath + "'");

                // return:
                //		-1	һ���Դ���
                //		-2	ʱ�����ƥ��
                //		-4	δ�ҵ�·��ָ������Դ
                //		-5	δ�ҵ����ݿ�
                //		-6	û���㹻��Ȩ��
                //		-7	·�����Ϸ�
                //		-8	�Ѿ�����ͬ��ͬ���͵���
                //		-9	�Ѿ�����ͬ������ͬ���͵���
                //		0	�ɹ�
                nRet = this.WriteRes(strTargetObjectPath,
                    "",
                    lTargetObjectTotalLength,
                    baOriginObjectData,
                    null,
                    strTargetObjectMetadata,
                    strTargetObjectStyle,
                    null,
                    user,
                    out strTargetObjectOutputPath,
                    out baTargetObjectOutputTimestamp,
                    out strTargetObjectOutputValue,
                    out strError);
                if (nRet <= -1)
                    return (int)nRet;
            }

            // �ж��Ƿ�ɾ��Դ��¼
            if (bDeleteOriginRecord == true)
            {
                // return:
                //      -1	һ���Դ�����������������Ϸ���
                //      -2	ʱ�����ƥ��    // �������ʱ�������Ӧ�����������
                //      -4	δ�ҵ�·����Ӧ����Դ
                //      -5	δ�ҵ����ݿ�
                //      -6	û���㹻��Ȩ��
                //      -7	·�����Ϸ�
                //      0	�����ɹ�
                nRet = this.DeleteRes(strOriginRecordPath,
                    user,
                    baOriginRecordOutputTimestamp,
                    out baOriginRecordOutputTimestamp,
                    out strError);
                if (nRet <= -1)
                    return (int)nRet;
            }

            // ȡ��Ŀ���¼������ʱ���
            // return:
            //		-1  ����
            //		-4  δ�ҵ���¼
            //      0   �ɹ�
            nRet = this.GetTimestampFromDb(
                strTargetRecordOutputPath,
                out baOutputRecordTimestamp,
                out strError);
            if (nRet <= -1)
            {
                strError = "������¼��ɣ�����ȡĿ���¼��ʱ���ʱ����" + strError;
                return -1;
            }

            return 0;
        }

        // ��ȡ��¼��ʱ���
        // parameters:
        //      strRecordPath   ��¼·��
        //      baOutputTimestamp   out����������ʱ���
        //      strError    out���������س�����Ϣ
        // return:
        //		-1  ����
        //		-4  δ�ҵ���¼
        //      0   �ɹ�
        public int GetTimestampFromDb(string strRecordPath,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            baOutputTimestamp = null;
            strError = "";
            Debug.Assert(strRecordPath != null && strRecordPath != "", "GetTimestampFromDb()���ô���strRecordPath����ֵ����Ϊnull����ַ�����");

            DbPath dbpath = new DbPath(strRecordPath);
            Database db = this.GetDatabase(dbpath.Name);
            if (db == null)
            {
                strError = "δ�ҵ���Ϊ'" + dbpath.Name + "'�����ݿ⡣";
                return -1;
            }

            // return:
            //		-1  ����
            //		-4  δ�ҵ���¼
            //      0   �ɹ�
            int nRet = db.GetTimestampFromDb(dbpath.ID,
                out baOutputTimestamp,
                out strError);

            return nRet;
        }


        // ���Ŀ¼��������
        // parameters:
        //		strDirCfgItemPath	����Ŀ¼��·��
        //		nodeDir	            dir�ڵ㣬���Ϊnull�������·������
        //		strError        	out���������س�����Ϣ
        // return:
        //		-1	����
        //      -4  δָ��·����Ӧ�Ķ���
        //		0	�ɹ�
        // ���dir����������������¼������ԣ�Ҳɾ���¼���Ӧ�������ļ�
        public int ClearDirCfgItem(string strDirCfgItemPath,
            XmlNode nodeDir,
            out string strError)
        {
            strError = "";
            if (nodeDir == null)
            {
                if (String.IsNullOrEmpty(strDirCfgItemPath) == true)
                {
                    strError = "ClearDirCfgItem()���ô���strDirCfgItemPath��������Ϊnull���߿��ַ�����";
                    return -1;
                }

                List<XmlNode> nodes = DatabaseUtil.GetNodes(this.NodeDbs,
                    strDirCfgItemPath);
                if (nodes.Count == 0)
                {
                    strError = "ClearDirCfgItem()��δ�ҵ�·��Ϊ'" + strDirCfgItemPath + "'���������";
                    return -4;
                }

                if (nodes.Count > 1)
                {
                    strError = "ClearDirCfgItem()��·��Ϊ'" + strDirCfgItemPath + "'������������'" + Convert.ToString(nodes.Count) + "'����databases.xml�����ļ����Ϸ���";
                    return -1;
                }

                nodeDir = nodes[0];
            }

            // ɾ������ı���Ŀ¼
            string strLocalDir = "";
            strLocalDir = DatabaseUtil.GetLocalDir(this.NodeDbs,
                nodeDir).Trim();

            string strDir = "";
            if (strLocalDir != "")
                strDir = this.DataDir + "\\" + strLocalDir + "\\";
            else
                strDir = this.DataDir + "\\";

            DirectoryInfo di = new DirectoryInfo(strDir);

            // ɾ�����е��¼�Ŀ¼
            DirectoryInfo[] dirs = di.GetDirectories();
            foreach (DirectoryInfo childDir in dirs)
            {
                Directory.Delete(childDir.FullName, true);
            }

            // ɾ�����е��¼��ļ�
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo childFile in files)
            {
                File.Delete(childFile.FullName);
            }

            // �Ƴ��ڴ����
            nodeDir.RemoveAll();

            this.Changed = true;

            return 0;
        }


        // ���ڴ��������һ����������
        // parameters:
        //		strParentPath	����·�� ���Ϊnull����ַ�������ֱ����objects�¼��½�
        //		strName	�Լ������ƣ�����Ϊnull����ַ���
        //		bDir	�Ƿ���·��
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        public int SetFileCfgItem(string strParentPath,
            XmlNode nodeParent,
            string strName,
            out string strError)
        {
            strError = "";
            //**********�����ݿ⼯�ϼ�д��**************
            this.m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("SetCfgItem()�������ݼ��ϼ�д����");
#endif
            try
            {
                if (String.IsNullOrEmpty(strName) == true)
                {
                    strError = "SetCfgItem()���ô���strName����ֵ����Ϊnull����ַ�����";
                    return -1;
                }

                if (nodeParent == null)
                {
                    if (strParentPath == "" || strParentPath == null)
                    {
                        nodeParent = this.NodeDbs;
                    }
                    else
                    {
                        List<XmlNode> parentNodes = DatabaseUtil.GetNodes(this.NodeDbs,
                            strParentPath);
                        if (parentNodes.Count > 1)
                        {
                            strError = "��<objects>�¼�·��Ϊ'" + strParentPath + "'����������'" + Convert.ToString(parentNodes.Count) + "'���������ļ����Ϸ�����";
                            return -1;
                        }
                        if (parentNodes.Count == 0)
                        {
                            strError = "��<objects>�¼�δ�ҵ�·��Ϊ'" + strParentPath + "'�������";
                            return -1;
                        }

                        nodeParent = parentNodes[0];
                    }
                }

                string strCfgItemOuterXml = "";
                string strLocalName = strName + ".xml";
                strCfgItemOuterXml = "<file name='" + strName + "' localname='" + strLocalName + "'/>";

                nodeParent.InnerXml = nodeParent.InnerXml + strCfgItemOuterXml;

                this.Changed = true;

                return 0;
            }
            finally
            {
                //***********�����ݿ⼯�Ͻ�д��***************
                this.m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("SetCfgItem()�������ݿ⼯�Ͻ�д����");
#endif
            }
        }


        // �Զ�����Ŀ¼��������
        // parameters:
        //		strParentPath	����·�� ���Ϊnull����ַ�������ֱ����objects�¼��½�
        //		strName	�Լ������ƣ�����Ϊnull����ַ���
        //		bDir	�Ƿ���·��
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        public int AutoCreateDirCfgItem(string strDirCfgItemPath,
            out string strError)
        {
            strError = "";

            //**********�����ݿ⼯�ϼ�д��**************
            this.m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("AutoCreateDirCfgItem()����'" + this.GetCaption("zh-cn") + "'���ݿ⼯�ϼ�д����");
#endif
            try
            {
                if (String.IsNullOrEmpty(strDirCfgItemPath) == true)
                {
                    strError = "AutoCreateDirCfgItem()���ô���strDirCfgItemPath����ֵ����Ϊnull����ַ�����";
                    return -1;
                }

                List<XmlNode> nodes = DatabaseUtil.GetNodes(this.NodeDbs,
                    strDirCfgItemPath);
                if (nodes.Count > 1)
                {
                    strError = "·��Ϊ'" + strDirCfgItemPath + "'������������'" + Convert.ToString(nodes.Count) + "'���������������ļ����Ϸ���";
                    return -1;
                }
                if (nodes.Count == 1)
                {
                    strError = "AutoCreateDirCfgItem()���ô����Ѵ���·��Ϊ'" + strDirCfgItemPath + "'������Ŀ¼��";
                    return -1;
                }

                XmlDocument dom = this.NodeDbs.OwnerDocument;
                if (dom == null)
                {
                    strError = "AutoCreateDirCfgItem()�ﲻ�����Ҳ���dom��";
                    return -1;
                }

                //��strpath��'/'�ֿ�
                string[] paths = strDirCfgItemPath.Split(new char[] { '/' });
                if (paths.Length == 0)
                {
                    strError = "AutoCreateDirCfgItem()��paths���Ȳ�����Ϊ0��";
                    return -1;
                }

                int i = 0;
                if (paths[0] == "")
                    i = 1;
                XmlNode nodeCurrent = this.NodeDbs;
                XmlNode temp = null;
                for (; i < paths.Length; i++)
                {
                    string strDirName = paths[i];

                    if (nodeCurrent == this.NodeDbs)
                    {
                        //XmlNode temp = null;
                        foreach (XmlNode tempChild in nodeCurrent.ChildNodes)
                        {
                            if (tempChild.Name == "database")
                            {
                                string strAllCaption = DatabaseUtil.GetAllCaption(tempChild);
                                if (StringUtil.IsInList(strDirName, strAllCaption, true) == true)
                                {
                                    temp = tempChild;
                                    break;
                                }
                            }
                            else
                            {
                                string strTempName = DomUtil.GetAttr(tempChild, "name");
                                if (String.Compare(strTempName, strDirName, true) == 0)
                                {
                                    temp = tempChild;
                                    break;
                                }
                            }
                        }

                        if (temp == null)
                        {
                            temp = dom.CreateElement("dir");
                            DomUtil.SetAttr(temp, "name", strDirName);
                            DomUtil.SetAttr(temp, "localdir", strDirName);
                            nodeCurrent.AppendChild(temp);
                        }

                        nodeCurrent = temp;
                    }
                    else
                    {
                        string strTempXpath = "dir[@name='" + strDirName + "']";
                        temp = nodeCurrent.SelectSingleNode(strTempXpath);
                        if (temp == null)
                        {
                            temp = dom.CreateElement("dir");
                            DomUtil.SetAttr(temp, "name", strDirName);
                            DomUtil.SetAttr(temp, "localdir", strDirName);
                            nodeCurrent.AppendChild(temp);
                        }
                        nodeCurrent = temp;
                    }
                }

                nodes = DatabaseUtil.GetNodes(this.NodeDbs,
                    strDirCfgItemPath);
                if (nodes.Count > 1)
                {
                    strError = "�����Զ�������·��Ϊ'" + strDirCfgItemPath + "'������������'" + Convert.ToString(nodes.Count) + "'�������Բ����ܵ������";
                    return -1;
                }
                if (nodes.Count == 0)
                {
                    strError = "AutoCreateDirCfgItem()���Զ�����'" + strDirCfgItemPath + "'����Ŀ¼�ڴ������ϣ������ܻ��ǲ����ڡ�";
                    return -1;
                }
                XmlNode node = nodes[0];

                string strDir = DatabaseUtil.GetLocalDir(this.NodeDbs,
                    node);
                strDir = this.DataDir + "\\" + strDir;
                PathUtil.CreateDirIfNeed(strDir);

                this.Changed = true;

                return 0;
            }
            finally
            {
                //***************�����ݿ⼯�Ͻ�д��************
                this.m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.container.WriteDebugInfo("AutoCreateDirCfgItem()����'" + this.GetCaption("zh-cn") + "'���ݿ⼯�Ͻ�д����");
#endif
            }
        }


        // д��Դ
        // parameter:
        //		strResPath		��Դ·��,����Ϊnull����ַ���
        //						��Դ���Ϳ��������ݿ���������(Ŀ¼���ļ�)����¼�壬������Դ�����ּ�¼��
        //						��������: ����/��������·��
        //						��¼��: ����/��¼��
        //						������Դ: ����/��¼��/object/��ԴID
        //						���ּ�¼��: ����/��¼/xpath/<locate>hitcount</locate><action>AddInteger</action> ���� ����/��¼/xpath/@hitcount
        //		strRanges		Ŀ���λ��,���range�ö��ŷָ�,null��Ϊ�ǿ��ַ��������ַ�����Ϊ��0-(lTotalLength-1)
        //		lTotalLength	��Դ�ܳ���,����Ϊ0
        //		baContent		��byte[]���ݴ��͵���Դ���ݣ����Ϊnull���ʾ��0�ֽڵ�����
        //		streamContent	������
        //		strMetadata		Ԫ�������ݣ�null��Ϊ�ǿ��ַ�����ע:��ЩԪ������Ȼ�������������������ϣ����糤��
        //		strStyle		���,null��Ϊ�ǿ��ַ���
        //						ignorechecktimestamp ����ʱ���;
        //						createdir,����Ŀ¼,·����ʾ��������Ŀ¼·��
        //						autocreatedir	�Զ������м���Ŀ¼
        //						content	���ݷ���baContent������
        //						attachment	���ݷ��ڸ�����
        //		baInputTimestamp	�����ʱ���,���ڴ���Ŀ¼�������ʱ���
        //		user	�ʻ����󣬲���Ϊnull
        //		strOutputResPath	���ص���Դ·��
        //							����׷�Ӽ�¼ʱ������ʵ�ʵ�·��
        //							������Դ���ص�·���������·����ͬ
        //		baOutputTimestamp	����ʱ���
        //							��ΪĿ¼ʱ�����ص�ʱ���Ϊnull
        //		strOutputValue	���ص�ֵ���������ۼӼ���ʱ
        //		strError	������Ϣ
        // ˵����
        //		������ʵ�ʴ���������������½���Դ��������Դ
        //		baContent��strAttachmentIDֻ��ʹ��һ������strStyle����ʹ��
        // return:
        //		-1	һ���Դ���
        //		-2	ʱ�����ƥ��
        //		-4	δ�ҵ�·��ָ������Դ
        //		-5	δ�ҵ����ݿ�
        //		-6	û���㹻��Ȩ��
        //		-7	·�����Ϸ�
        //		-8	�Ѿ�����ͬ��ͬ���͵���
        //		-9	�Ѿ�����ͬ������ͬ���͵���
        //		0	�ɹ�
        // �ߣ���ȫ
        public int WriteRes(string strResPath,
            string strRanges,
            long lTotalLength,
            byte[] baSource,
            Stream streamSource,
            string strMetadata,
            string strStyle,
            byte[] baInputTimestamp,
            User user,
            out string strOutputResPath,
            out byte[] baOutputTimestamp,
            out string strOutputValue,
            out string strError)
        {
            baOutputTimestamp = null;
            strOutputResPath = strResPath;
            strOutputValue = "";
            strError = "";

            //**********�Կ⼯�ϼ�д��****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("WriteRes()���Կ⼯�ϼ�д����");
#endif
            try
            {

                //------------------------------------------------
                //�����������Ƿ�Ϸ������淶�������
                //---------------------------------------------------
                if (user == null)
                {
                    strError = "WriteRes()���ô���user������Ϊnull";
                    return -1;
                }
                if (String.IsNullOrEmpty(strResPath) == true)
                {
                    strError = "��Դ·��'" + strResPath + "'���Ϸ�������Ϊnull����ַ�����";
                    return -7;
                }
                if (lTotalLength < 0)
                {
                    strError = "WriteRes()��lTotalLength����Ϊ'" + Convert.ToString(lTotalLength) + "'������>=0��";
                    return -1;
                }
                if (strRanges == null) //����ĺ������ᴦ��ɴ���ķ�Χ
                    strRanges = "";
                if (strMetadata == null)
                    strMetadata = "";
                if (strStyle == null)
                    strStyle = "";

                if (baSource == null && streamSource == null)
                {
                    strError = "WriteRes()���ô���baSource������streamSource��������ͬʱΪnull��";
                    return -1;
                }
                if (baSource != null && streamSource != null)
                {
                    strError = "WriteRes()���ô���baSource������streamSource����ֻ����һ������ֵ��";
                    return -1;
                }


                //------------------------------------------------
                //��������Դ������
                //---------------------------------------------------

                int nRet = 0;

                bool bRecordPath = this.IsRecordPath(strResPath);
                if (bRecordPath == false)
                {
                    // ��������Ŀ¼
                    if (StringUtil.IsInList("createdir", strStyle, true) == true)
                    {
                        // return:
                        //      -1  һ���Դ���
                        //		-4	δָ��·����Ӧ�Ķ���
                        //		-6	Ȩ�޲���
                        //		-8	Ŀ¼�Ѵ���
                        //		-9	�����������͵�����
                        //		0	�ɹ�
                        nRet = this.WriteDirCfgItem(strResPath,
                            strStyle,
                            user,
                            out strError);
                    }
                    else
                    {
                        // return:
                        //      -1  һ���Դ���
                        //      -2  ʱ�����ƥ��
                        //      -4  �Զ�����Ŀ¼ʱ��δ�ҵ��ϼ�
                        //		-6	Ȩ�޲���
                        //		-9	�����������͵�����
                        //		0	�ɹ�
                        nRet = this.WriteFileCfgItem(strResPath,
                            strRanges,
                            lTotalLength,
                            baSource,
                            streamSource,
                            strMetadata,
                            strStyle,
                            baInputTimestamp,
                            user,
                            out baOutputTimestamp,
                            out strError);
                    }

                    strOutputResPath = strResPath;

                    // ����database.xml�ļ�
                    if (this.Changed == true)
                        this.SaveXmlSafety();
                }
                else
                {
                    bool bObject = false;
                    string strRecordID = "";
                    string strObjectID = "";
                    string strXPath = "";

                    string strPath = strResPath;
                    string strDbName = StringUtil.GetFirstPartPath(ref strPath);
                    //***********�Ե���1��*************
                    // ����Ϊֹ��strPath�������ݿ�����,�����·�����������:cfgs;���඼��������¼id
                    if (strPath == "")
                    {
                        strError = "��Դ·��'" + strResPath + "'·�����Ϸ���δָ������¼���";
                        return -7;
                    }
                    // �ҵ����ݿ����
                    Database db = this.GetDatabaseSafety(strDbName);
                    if (db == null)
                    {
                        strError = "��'" + strDbName + "'�����ݿⲻ���ڡ�";
                        return -5;
                    }

                    string strFirstPart = StringUtil.GetFirstPartPath(ref strPath);
                    //***********�Ե���2��*************
                    // ����Ϊֹ��strPath������¼�Ų��ˣ��¼�������ж�


                    strRecordID = strFirstPart;
                    // ֻ����¼�Ų��·��
                    if (strPath == "")
                    {
                        bObject = false;
                        goto DOWRITE;
                    }

                    strFirstPart = StringUtil.GetFirstPartPath(ref strPath);
                    //***********�Ե���2��*************
                    // ����Ϊֹ��strPath����object��xpath�� strFirstPart������object �� xpath

                    if (strFirstPart != "object"
                        && strFirstPart != "xpath")
                    {
                        strError = "��Դ·�� '" + strResPath + "' ���Ϸ�,��3��������'object'��'xpath'";
                        return -7;
                    }
                    if (strPath == "")  //object��xpath�¼�������ֵ
                    {
                        strError = "��Դ·�� '" + strResPath + "' ���Ϸ�,����3����'object'��'xpath'����4�����������ݡ�";
                        return -7;
                    }

                    if (strFirstPart == "object")
                    {
                        strObjectID = strPath;
                        bObject = true;
                    }
                    else
                    {
                        strXPath = strPath;
                        bObject = false;
                    }


                    //------------------------------------------------
                //��ʼ������Դ
                //---------------------------------------------------

                DOWRITE:

                    // ****************************************


                    string strOutputRecordID = "";
                    nRet = db.CanonicalizeRecordID(strRecordID,
                        out strOutputRecordID,
                        out strError);
                    if (nRet == -1)
                    {
                        strError = "��Դ·�� '" + strResPath + "' ���Ϸ���ԭ�򣺼�¼�Ų���Ϊ'" + strRecordID + "'";
                        return -1;
                    }


                    // ************************************
                    // �����¼�ͼ�¼��Ķ���
                    if (bObject == true)  //����
                    {
                        if (strOutputRecordID == "-1")
                        {
                            strError = "��Դ·�� '" + strResPath + "' ���Ϸ�,ԭ�򣺱��������Դʱ,��¼�Ų���Ϊ'" + strRecordID + "'��";
                            return -1;
                        }
                        strRecordID = strOutputRecordID;

                        // return:
                        //		-1  ����
                        //		-2  ʱ�����ƥ��
                        //      -4  ��¼�������Դ������
                        //      -6  Ȩ�޲���
                        //		0   �ɹ�
                        nRet = db.WriteObject(user,
                            strRecordID,
                            strObjectID,
                            strRanges,
                            lTotalLength,
                            baSource,
                            streamSource,
                            strMetadata,
                            strStyle,
                            baInputTimestamp,
                            out baOutputTimestamp,
                            out strError);

                        strOutputResPath = strDbName + "/" + strRecordID + "/object/" + strObjectID;

                    }
                    else  // ��¼��
                    {
                        strRecordID = strOutputRecordID;

                        string strOutputID = "";
                        // return:
                        //		-1  ����
                        //		-2  ʱ�����ƥ��
                        //      -4  ��¼������
                        //      -6  Ȩ�޲���
                        //		0   �ɹ�
                        nRet = db.WriteXml(user,
                            strRecordID,
                            strXPath,
                            strRanges,
                            lTotalLength,
                            baSource,
                            streamSource,
                            strMetadata,
                            strStyle,
                            baInputTimestamp,
                            out baOutputTimestamp,
                            out strOutputID,
                            out strOutputValue,
                            true,
                            out strError);

                        strRecordID = strOutputID;

                        if (strXPath == "")
                            strOutputResPath = strDbName + "/" + strRecordID;
                        else
                            strOutputResPath = strDbName + "/" + strRecordID + "/xpath/" + strXPath;

                    }
                }

                return nRet;
            }
            finally
            {
                //**********�Կ⼯�Ͻ�д��****************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
			this.WriteDebugInfo("WriteRes()���Կ⼯��дд����");
#endif
            }
        }

        // дĿ¼��������
        // parameters:
        //		strResPath	��Դ·��������
        //					ԭ����û�����������Ϊʲô�����أ�
        //					��Ϊ����ʱ����ԭ·�������Ϊnull����ַ��������Ϊ:����·��/strCfgItemPath
        //		strStyle	��� null��Ϊ�ǿ��ַ���
        //					clear	��ʾ����¼�
        //					autocreatedir	��ʾ�Զ�����ȱʡ��Ŀ¼
        //		user	User���������ж��Ƿ���Ȩ�ޣ�����Ϊnull
        //		strCfgItemPath	��������·������������������Ϊnull����ַ�����???������strResPathһ���ã�������
        //		strError	out���������س�����Ϣ
        // return:
        //      -1  һ���Դ���
        //		-4	δָ��·����Ӧ�Ķ���
        //		-6	Ȩ�޲���
        //		-8	Ŀ¼�Ѵ���
        //		-9	�����������͵�����
        //		0	�ɹ�
        // �ߣ�����ȫ
        public int WriteDirCfgItem(string strCfgItemPath,
            string strStyle,
            User user,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            if (String.IsNullOrEmpty(strCfgItemPath) == true)
            {
                strError = "WriteDirCfgItem()�������strCfgItemPath����Ϊnull����ַ�����";
                return -1;
            }

            List<XmlNode> list = DatabaseUtil.GetNodes(this.NodeDbs,
                strCfgItemPath);
            if (list.Count > 1)
            {
                strError = "�������������ļ����Ϸ���·��Ϊ'" + strCfgItemPath + "'�����������Ӧ�Ľڵ���'" + Convert.ToString(list.Count) + "'����";
                return -1;
            }

            string strExistRights = "";
            bool bHasRight = false;

            // �Ѵ���ͬ��������������
            if (list.Count == 1)
            {
                XmlNode node = list[0];
                if (node.Name == "file")
                {
                    strError = "�������Ѵ���·��Ϊ'" + strCfgItemPath + "'�������ļ���������Ŀ¼�����ļ���";
                    return -9;
                }
                if (node.Name == "database")
                {
                    strError = "�������Ѵ�����Ϊ'" + strCfgItemPath + "'�����ݿ⣬������Ŀ¼�������ݿ⡣";
                    return -9;
                }

                if (StringUtil.IsInList("clear", strStyle) == true)
                {
                    // ������������Ѵ��ڣ�������Ƿ���clearȨ��
                    string strPathForRights = strCfgItemPath;
                    bHasRight = user.HasRights(strPathForRights,
                        ResType.Directory,
                        "clear",
                        out strExistRights);
                    if (bHasRight == false)
                    {
                        strError = "�����ʻ���Ϊ'" + user.Name + "'����·��Ϊ'" + strCfgItemPath + "'����������û��'����¼�(clear)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                        return -6;
                    }

                    // ���Ŀ¼
                    // return:
                    //		-1	����
                    //      -4  δָ��·����Ӧ�Ķ���
                    //		0	�ɹ�
                    return this.ClearDirCfgItem(strCfgItemPath,
                        node,
                        out strError);
                }
                else
                {
                    strError = "�������Ѵ���·��Ϊ'" + strCfgItemPath + "'������Ŀ¼��";
                    return -8;
                }
            }


            //***************************************

            bHasRight = user.HasRights(strCfgItemPath,
                ResType.Directory,
                "create",
                out strExistRights);
            if (bHasRight == false)
            {
                strError = "�����ʻ���Ϊ'" + user.Name + "'����·��Ϊ'" + strCfgItemPath + "'����������û��'����¼�(clear)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                return -6;
            }

            // return:
            //		-1	����
            //		0	�ɹ�
            nRet = this.AutoCreateDirCfgItem(strCfgItemPath,
                out strError);
            if (nRet == -1)
                return -1;

            return 0;
        }


        // д�ļ���������
        // return:
        //      -1  һ���Դ���
        //      -2  ʱ�����ƥ��
        //      -4  �Զ�����Ŀ¼ʱ��δ�ҵ��ϼ�
        //		-6	Ȩ�޲���
        //		-9	�����������͵�����
        //		0	�ɹ�
        // �̣߳�����ȫ��
        internal int WriteFileCfgItem(string strCfgItemPath,
            string strRanges,
            long lTotalLength,
            byte[] baSource,
            Stream streamSource,
            string strMetadata,
            string strStyle,
            byte[] baInputTimestamp,
            User user,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            baOutputTimestamp = null;
            strError = "";
            int nRet = 0;

            Debug.Assert(user != null, "WriteFileCfgItem()���ô���user������Ϊnull");

            //------------------------------------------------
            // ���������������淶���������
            //--------------------------------------------------
            if (lTotalLength <= -1)
            {
                strError = "WriteFileCfgItem()���ô���lTotalLengthֵΪ'" + Convert.ToString(lTotalLength) + "'���Ϸ���������ڵ���0��";
                return -1;
            }
            if (strStyle == null)
                strStyle = "";
            if (strRanges == null)
                strRanges = null;
            if (strMetadata == null)
                strMetadata = "";

            if (baSource == null && streamSource == null)
            {
                strError = "WriteFileCfgItem()���ô���baSource������streamSource��������ͬʱΪnull��";
                return -1;
            }
            if (baSource != null && streamSource != null)
            {
                strError = "WriteFileCfgItem()���ô���baSource������streamSource����ֻ����һ������ֵ��";
                return -1;
            }

            if (strCfgItemPath == null || strCfgItemPath == "")
            {
                strError = "WriteFileCfgItem()���ô���strResPath����Ϊnull����ַ�����";
                return -1;
            }

            //------------------------------------------------
            // ��ʼ������
            //--------------------------------------------------

            List<XmlNode> list = DatabaseUtil.GetNodes(this.NodeDbs,
                strCfgItemPath);
            if (list.Count > 1)
            {
                strError = "�������������ļ����Ϸ���·��Ϊ'" + strCfgItemPath + "'�����������Ӧ�Ľڵ���'" + Convert.ToString(list.Count) + "'����";
                return -1;
            }

            string strExistRights = "";
            bool bHasRight = false;


            //------------------------------------------------
            // �Ѵ���ͬ��������������
            //--------------------------------------------------

            if (list.Count == 1)
            {
                XmlNode node = list[0];
                if (node.Name == "dir")
                {
                    strError = "�������Ѵ���·��Ϊ'" + strCfgItemPath + "'������Ŀ¼���������ļ�����Ŀ¼��";
                    return -9;
                }
                if (node.Name == "database")
                {
                    strError = "�������Ѵ�����Ϊ'" + strCfgItemPath + "'�����ݿ⣬�������ļ��������ݿ⡣";
                    return -9;
                }

                // ������������Ѵ��ڣ�������Ƿ���overwriteȨ��
                string strPathForRights = strCfgItemPath;
                bHasRight = user.HasRights(strPathForRights,
                    ResType.File,
                    "overwrite",
                    out strExistRights);
                if (bHasRight == false)
                {
                    strError = "�����ʻ���Ϊ'" + user.Name + "'����·��Ϊ'" + strCfgItemPath + "'����������û��'����(overwrite)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                    return -6;
                }

                // �����������������������ļ���
                // ���ڴ�������Ѵ��ڣ���ô�����ļ���һ�����ڣ��������ļ�һ������
                string strLocalPath = "";
                // return:
                //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
                //		-2	û�ҵ��ڵ�
                //		-3	localname����δ�����Ϊֵ��
                //		-4	localname�ڱ��ز�����
                //		-5	���ڶ���ڵ�
                //		0	�ɹ�
                nRet = this.GetFileCfgItemLacalPath(strCfgItemPath,
                    out strLocalPath,
                    out strError);
                if (nRet != 0)
                {
                    if (nRet != -4)
                        return -1;
                }

                goto DOWRITE;
            }


            //------------------------------------------------
            // ������������������
            //--------------------------------------------------


            string strParentCfgItemPath = ""; //���׵�·��
            string strThisCfgItemName = ""; //���������������
            int nIndex = strCfgItemPath.LastIndexOf('/');
            if (nIndex != -1)
            {
                strParentCfgItemPath = strCfgItemPath.Substring(0, nIndex);
                strThisCfgItemName = strCfgItemPath.Substring(nIndex + 1);
            }
            else
            {
                strThisCfgItemName = strCfgItemPath;
            }

            XmlNode nodeParent = null;
            // ���ϼ�·�����м��
            if (strParentCfgItemPath != "")
            {
                List<XmlNode> parentNodes = DatabaseUtil.GetNodes(this.NodeDbs,
                    strParentCfgItemPath);
                if (parentNodes.Count > 1)
                {
                    nIndex = strCfgItemPath.LastIndexOf("/");
                    string strTempParentPath = strCfgItemPath.Substring(0, nIndex);
                    strError = "��������·��Ϊ'" + strTempParentPath + "'������������'" + Convert.ToString(parentNodes.Count) + "'���������ļ����Ϸ���";
                    return -1;
                }

                if (parentNodes.Count == 1)
                {
                    nodeParent = parentNodes[0];
                }
                else
                {

                    if (StringUtil.IsInList("autocreatedir", strStyle, true) == false)
                    {
                        nIndex = strCfgItemPath.LastIndexOf("/");
                        string strTempParentPath = strCfgItemPath.Substring(0, nIndex);
                        strError = "δ�ҵ�·��Ϊ'" + strTempParentPath + "'��������޷������¼��ļ���";
                        return -4;
                    }

                    // return:
                    //		-1	����
                    //		0	�ɹ�
                    nRet = this.AutoCreateDirCfgItem(strParentCfgItemPath,
                        out strError);
                    if (nRet == -1)
                        return -1;

                    parentNodes = DatabaseUtil.GetNodes(this.NodeDbs,
                        strParentCfgItemPath);
                    if (parentNodes.Count != 1)
                    {
                        strError = "WriteFileCfgItem()���Զ��������ϼ�Ŀ¼�ˣ���ʱ�������Ҳ���·��Ϊ'" + strParentCfgItemPath + "'�����������ˡ�";
                        return -1;
                    }

                    nodeParent = parentNodes[0];
                }
            }
            else
            {
                nodeParent = this.NodeDbs;
            }


            // ����ϼ��Ƿ���ָ��Ȩ��
            bHasRight = user.HasRights(strCfgItemPath,
                ResType.File,
                "create",
                out strExistRights);
            if (bHasRight == false)
            {
                strError = "�����ʻ���Ϊ'" + user.Name + "',��'" + strCfgItemPath + "',û��'����(create)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                return -6;
            }


            // return:
            //		-1	����
            //		0	�ɹ�
            nRet = this.SetFileCfgItem(strParentCfgItemPath,
                nodeParent,
                strThisCfgItemName,
                out strError);
            if (nRet == -1)
                return -1;


        DOWRITE:

            string strFilePath = "";//GetCfgItemLacalPath(strCfgItemPath);
            // return:
            //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
            //		-2	û�ҵ��ڵ�
            //		-3	localname����δ�����Ϊֵ��
            //		-4	localname�ڱ��ز�����
            //		-5	���ڶ���ڵ�
            //		0	�ɹ�
            nRet = this.GetFileCfgItemLacalPath(strCfgItemPath,
                out strFilePath,
                out strError);
            if (nRet != 0)
            {
                if (nRet != -4)
                    return -1;
            }

            string strTempPath = strCfgItemPath;
            string strFirstPart = StringUtil.GetFirstPartPath(ref strTempPath);
            Database db = this.GetDatabase(strFirstPart);
            if (db != null)
            {

                // return:
                //		-1  һ���Դ���
                //      -2  ʱ�����ƥ��
                //		0	�ɹ�
                return db.WriteFileForCfgItem(strCfgItemPath,
                    strFilePath,
                     strRanges,
                     lTotalLength,
                     baSource,
                     streamSource,
                     strMetadata,
                     strStyle,
                     baInputTimestamp,
                     out baOutputTimestamp,
                     out strError);
            }
            else
            {
                // ��������ĳһ�����ݿ�������ļ�
                // return:
                //		-1	һ���Դ���
                //		-2	ʱ�����ƥ��
                //		0	�ɹ�
                return this.WriteFileForCfgItem(strFilePath,
                    strRanges,
                    lTotalLength,
                    baSource,
                    streamSource,
                    strMetadata,
                    strStyle,
                    baInputTimestamp,
                    out baOutputTimestamp,
                    out strError);
            }
        }

        // Ϊ�ļ���������д�ļ�
        // parameters:
        //		strFilePath Ŀ���ļ�·��������Ϊnull����ַ���
        //		strRanges	������򣬿���Ϊnull��""��ʾ0-sourceBuffer.Length-1������
        //		nTotalLength	�ܳ��ȣ�����Ϊ0
        //		baSource	�����ֽ����飬����Ϊnull
        //		streamSource	������������Ϊnull
        //		strMetadata	Ԫ������Ϣ������Ϊnull��""
        //		inputTimestamp	�����ʱ���������Ϊnull
        //		outputTimestamp	out����������ʵ�ʵ�ʱ���
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	һ���Դ���
        //		-2	ʱ�����ƥ��
        //		0	�ɹ�
        // ��: ����ȫ
        // ˵��: ���ֺ�����ִ�й��̻����ȼ��һ�±����ǲ���һ�η���
        // ȫ�������ݣ�����ǣ���ֱ��дĿ���ļ�������ʹ����ʱ�ļ�
        // ������ǲ�ʹ����ʱ�ļ��������ж�ranges�Ƿ�������������Ӧ�Ĵ���
        // Ҳ�п������½�һ���ļ�
        internal int WriteFileForCfgItem(string strFilePath,
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
            baOutputTimestamp = null;
            strError = "";

            // --------------------------------------------------------
            // ���������������淶���������
            // --------------------------------------------------------
            if (String.IsNullOrEmpty(strFilePath) == true)
            {
                strError = "WriteFileForCfgItem()���ô���strFilePath�������ܼ�null����ַ�����";
                return -1;
            }
            if (lTotalLength <= -1)
            {
                strError = "WriteFileForCfgItem()���ô���lTotalLength������ֵ����Ϊ'" + Convert.ToString(lTotalLength) + "',������ڵ���0��";
                return -1;
            }

            if (strStyle == null)
                strStyle = "";
            if (strRanges == null)
                strRanges = null;
            if (strMetadata == null)
                strMetadata = "";

            if (baSource == null && streamSource == null)
            {
                strError = "WriteFileForCfgItem()���ô���baSource������streamSource��������ͬʱΪnull��";
                return -1;
            }
            if (baSource != null && streamSource != null)
            {
                strError = "WriteFileForCfgItem()���ô���baSource������streamSource����ֻ����һ������ֵ��";
                return -1;
            }


            // --------------------------------------------------------
            // ���������������淶���������
            // --------------------------------------------------------

            string strNewFilePath = DatabaseUtil.GetNewFileName(strFilePath);

            //*************************************************
            // ���ʱ���,���е������ļ�����ʱ
            if (File.Exists(strFilePath) == true
                || File.Exists(strNewFilePath) == true)
            {
                if (StringUtil.IsInList("ignorechecktimestamp", strStyle) == false)
                {
                    if (File.Exists(strNewFilePath) == true)
                        baOutputTimestamp = DatabaseUtil.CreateTimestampForCfg(strNewFilePath);
                    else
                        baOutputTimestamp = DatabaseUtil.CreateTimestampForCfg(strFilePath);
                    if (ByteArray.Compare(baOutputTimestamp, baInputTimestamp) != 0)
                    {
                        strError = "ʱ�����ƥ��";
                        return -2;
                    }
                }
            }
            else
            {
                FileStream s = File.Create(strFilePath);
                s.Close();
                baOutputTimestamp = DatabaseUtil.CreateTimestampForCfg(strFilePath);
            }


            //**************************************************
            long lCurrentLength = 0;

            //if (lTotalLength == 0)
            //	goto END1;

            if (baSource != null)
            {
                if (baSource.Length == 0)
                {
                    if (strRanges != "")
                    {
                        strError = "WriteCfgFileByRange()����baSource�����ĳ���Ϊ0ʱ��strRanges��ֵȴΪ'" + strRanges + "'����ƥ�䣬ӦΪ���ַ�����";
                        return -1;
                    }
                    //��д��metadata��ĳߴ����
                    FileInfo fi = new FileInfo(strFilePath);
                    lCurrentLength = fi.Length;
                    fi = null;

                    //goto END1;
                }
            }
            else
            {
                if (streamSource.Length == 0)
                {
                    if (strRanges != "")
                    {
                        strError = "WriteCfgFileByRange()����streamSource��������Ϊ0ʱ��strRanges��ֵȴΪ'" + strRanges + "'����ƥ�䣬ӦΪ���ַ�����";
                        return -1;
                    }
                    //��д��metadata��ĳߴ����
                    FileInfo fi = new FileInfo(strFilePath);
                    lCurrentLength = fi.Length;
                    fi = null;

                    //goto END1;
                }
            }

            //******************************************
            // д����
            if (strRanges == null || strRanges == "")
            {
                if (lTotalLength > 0)
                    strRanges = "0-" + Convert.ToString(lTotalLength - 1);
                else
                    strRanges = "";
            }
            string strRealRanges = strRanges;

            // ��鱾�δ����ķ�Χ�Ƿ����������ļ���
            bool bIsComplete = false;
            if (lTotalLength == 0)
                bIsComplete = true;
            else
            {
                //		-1	���� 
                //		0	����δ���ǵĲ��� 
                //		1	�����Ѿ���ȫ����
                int nState = RangeList.MergContentRangeString(strRanges,
                    "",
                    lTotalLength,
                    out strRealRanges);
                if (nState == 1)
                    bIsComplete = true;
            }


            if (bIsComplete == true)
            {
                if (baSource != null)
                {
                    if (baSource.Length != lTotalLength)
                    {
                        strError = "��Χ'" + strRanges + "'�������ֽ����鳤��'" + Convert.ToString(baSource.Length) + "'�����ϡ�";
                        return -1;
                    }
                }
                else
                {
                    if (streamSource.Length != lTotalLength)
                    {
                        strError = "��Χ'" + strRanges + "'��������'" + Convert.ToString(streamSource.Length) + "'�����ϡ�";
                        return -1;
                    }
                }
            }


            RangeList rangeList = new RangeList(strRealRanges);

            // ��ʼд����
            Stream target = null;
            if (bIsComplete == true)
                target = File.Create(strFilePath);  //һ���Է��ֱ꣬��д���ļ�
            else
                target = File.Open(strNewFilePath, FileMode.OpenOrCreate);
            try
            {
                int nStartOfBuffer = 0;
                for (int i = 0; i < rangeList.Count; i++)
                {
                    RangeItem range = (RangeItem)rangeList[i];
                    int nStartOfTarget = (int)range.lStart;
                    int nLength = (int)range.lLength;
                    if (nLength == 0)
                        continue;

                    // �ƶ�Ŀ������ָ�뵽ָ��λ��
                    target.Seek(nStartOfTarget,
                        SeekOrigin.Begin);

                    if (baSource != null)
                    {
                        target.Write(baSource,
                            nStartOfBuffer,
                            nLength);


                        nStartOfBuffer += nLength; //2005.11.11��
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

            string strRangeFileName = DatabaseUtil.GetRangeFileName(strFilePath);

            // ���һ����д�����������Ҫ�����м�������:
            // 1.ʱ�����Ŀ���ļ�����
            // 2.д��metadata�ĳ���ΪĿ���ļ��ܳ���
            // 3.���������ʱ�����ļ�����ɾ����Щ�ļ���
            if (bIsComplete == true)
            {
                baOutputTimestamp = DatabaseUtil.CreateTimestampForCfg(strFilePath);
                lCurrentLength = lTotalLength;

                // ɾ�������ļ�
                if (File.Exists(strNewFilePath) == true)
                    File.Delete(strNewFilePath);
                if (File.Exists(strRangeFileName) == true)
                    File.Delete(strRangeFileName);

                goto END1;
            }


            //****************************************
            //�������ļ�
            bool bFull = false;
            string strResultRange = "";
            if (strRanges == "" || strRanges == null)
            {
                bFull = true;
            }
            else
            {
                string strOldRanges = "";
                if (File.Exists(strRangeFileName) == true)
                    strOldRanges = FileUtil.File2StringE(strRangeFileName);
                int nState1 = RangeList.MergContentRangeString(strRanges,
                    strOldRanges,
                    lTotalLength,
                    out strResultRange);
                if (nState1 == 1)
                    bFull = true;
            }

            // ����ļ���������Ҫ�����м�������:
            // 1.����󳤶Ƚ���ʱ�ļ� 
            // 2.����ʱ�ļ�����Ŀ���ļ�
            // 3.ɾ��new,range�����ļ�
            // 4.ʱ�����Ŀ���ļ�����
            // 5.metadata�ĳ���ΪĿ���ļ����ܳ���
            if (bFull == true)
            {
                Stream s = new FileStream(strNewFilePath,
                    FileMode.OpenOrCreate);
                try
                {
                    s.SetLength(lTotalLength);
                }
                finally
                {
                    s.Close();
                }

                // ��.new��ʱ�ļ��滻ֱ���ļ�
                File.Copy(strNewFilePath,
                    strFilePath,
                    true);

                File.Delete(strNewFilePath);

                if (File.Exists(strRangeFileName) == true)
                    File.Delete(strRangeFileName);
                baOutputTimestamp = DatabaseUtil.CreateTimestampForCfg(strFilePath);

                lCurrentLength = lTotalLength;
            }
            else
            {

                //����ļ�δ������Ҫ�����м������飺
                // 1.��Ŀǰ��rangeд��range�����ļ�
                // 2.ʱ�������ʱ�ļ�����
                // 3.metadata�ĳ���Ϊ-1����δ֪�����

                FileUtil.String2File(strResultRange,
                    strRangeFileName);

                lCurrentLength = -1;

                baOutputTimestamp = DatabaseUtil.CreateTimestampForCfg(strNewFilePath);
            }

        END1:

            // дmetadata
            if (strMetadata != "")
            {
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
                int nRet = DatabaseUtil.MergeMetadata(strOldMetadata,
                    strMetadata,
                    lCurrentLength,
                    out strResultMetadata,
                    out strError);
                if (nRet == -1)
                    return -1;

                // �Ѻϲ���������д���ļ���
                FileUtil.String2File(strResultMetadata,
                    strMetadataFileName);
            }
            return 0;
        }


        // GetRes()��range��̫��ʵ��,��Ϊԭ��������ĳ��ȳ�������ĳ���ʱ,���Ȼ��Զ�Ϊ��ȡ
        // �������range����ʾ,��֪�ýض��Ĳ��ֺá�
        // parameter:
        //		strResPath		��Դ·��,����Ϊnull����ַ���
        //						��Դ���Ϳ��������ݿ���������(Ŀ¼���ļ�)����¼�壬������Դ�����ּ�¼��
        //						��������: ����/��������·��
        //						��¼��: ����/��¼��
        //						������Դ: ����/��¼��/object/��ԴID
        //						���ּ�¼��: ����/��¼/xpath/<locate>hitcount</locate><action>AddInteger</action> ���� ����/��¼/xpath/@hitcount
        //		lStart	��ʼ����
        //		lLength	�ܳ���,-1:��start�����
        //		strStyle	ȡ��Դ�ķ���Զ���������ַ���
        /*
        strStyle�÷�

        1.�������ݴ�ŵ�λ��
        content		�ѷ��ص����ݷŵ��ֽ����������
        attachment	�ѷ��ص����ݷŵ�������,�����ظ�����id

        2.���Ʒ��ص�����
        metadata	����metadata��Ϣ
        timestamp	����timestamp
        length		�����ܳ��ȣ�ʼ�ն���ֵ
        data		����������
        respath		���ؼ�¼·��,Ŀǰʼ�ն���ֵ
        all			��������ֵ

        3.���Ƽ�¼��
        prev		ǰһ��
        prev,self	�Լ���ǰһ��
        next		��һ��
        next,self	�Լ�����һ��
        �ŵ�strOutputResPath������

        */
        //		baContent	��content�ֽ����鷵����Դ����
        //		strAttachmentID	�ø���������Դ����
        //		strMetadata	���ص�metadata����
        //		strOutputResPath	���ص���Դ·��
        //		baTimestamp	���ص���Դʱ���
        // return:
        //		-1	һ���Դ���
        //		-4	δ�ҵ�·��ָ������Դ
        //		-5	δ�ҵ����ݿ�
        //		-6	û���㹻��Ȩ��
        //		-7	·�����Ϸ�
        //		-10	δ�ҵ���¼xpath��Ӧ�Ľڵ�
        //		>= 0	�ɹ���������󳤶�
        //      nAdditionError -50 ��һ�������¼���Դ��¼������
        // �ߣ���ȫ
        public long GetRes(string strResPath,
            int nStart,
            int nLength,
            string strStyle,
            User user,
            int nMaxLength,
            out byte[] baData,
            out string strMetadata,
            out string strOutputResPath,
            out byte[] baOutputTimestamp,
            out int nAdditionError, // ���ӵĴ�����
            out string strError)
        {
            baData = null;
            strMetadata = "";
            strOutputResPath = "";
            baOutputTimestamp = null;
            strError = "";
            nAdditionError = 0;

            //------------------------------------------------
            //�����������Ƿ�Ϸ������淶�������
            //---------------------------------------------------

            Debug.Assert(user != null, "GetRes()���ô���user������Ϊnull��");

            if (user == null)
            {
                strError = "GetRes()���ô���user������Ϊnull��";
                return -1;
            }
            if (String.IsNullOrEmpty(strResPath) == true)
            {
                strError = "��Դ·��'" + strResPath + "'���Ϸ�������Ϊnull����ַ�����";
                return -7;
            }
            if (nStart < 0)
            {
                strError = "GetRes()���ô���nStart����С��0��";
                return -1;
            }
            if (strStyle == null)
                strStyle = "";


            //------------------------------------------------
            // ��ʼ������
            //---------------------------------------------------

            //******************�ӿ⼯�ϼӶ���******
            this.m_lock.AcquireReaderLock(m_nTimeOut);

#if DEBUG_LOCK
			this.WriteDebugInfo("GetRes()���Կ⼯�ϼӶ�����");
#endif
            try
            {
                long nRet = 0;

                bool bRecordPath = this.IsRecordPath(strResPath);
                if (bRecordPath == false)
                {
                    //�����������
                    // return:
                    //		-1  һ���Դ���
                    //		-4	δ�ҵ�·����Ӧ�Ķ���
                    //		-6	û���㹻��Ȩ��
                    //		>= 0    �ɹ� ������󳤶�
                    nRet = this.GetFileCfgItem(strResPath,
                        nStart,
                        nLength,
                        nMaxLength,
                        strStyle,
                        user,
                        out baData,
                        out strMetadata,
                        out baOutputTimestamp,
                        out strError);


                    if (StringUtil.IsInList("outputpath", strStyle) == true)
                    {
                        strOutputResPath = strResPath;
                    }
                }
                else
                {

                    // �ж���Դ����
                    string strPath = strResPath;
                    string strDbName = StringUtil.GetFirstPartPath(ref strPath);
                    //***********�Ե���1��*************
                    // ����Ϊֹ��strPath�������ݿ�����,�����·�����������:cfgs;���඼��������¼id
                    if (strPath == "")
                    {
                        strError = "��Դ·��'" + strResPath + "'·�����Ϸ���δָ������¼���";
                        return -7;
                    }

                    // ���������������ݿ⻹�Ƿ������������ļ�

                    // ������Դ���ͣ�д��Դ
                    Database db = this.GetDatabase(strDbName);
                    if (db == null)
                    {
                        strError = "δ�ҵ�'" + strDbName + "'��";
                        return -5;
                    }

                    bool bObject = false;
                    string strRecordID = "";
                    string strObjectID = "";
                    string strXPath = "";

                    string strFirstPart = StringUtil.GetFirstPartPath(ref strPath);
                    //***********�Ե���2��*************
                    // ����Ϊֹ��strPath��¼�Ų��ˣ��¼�������ж�

                    strRecordID = strFirstPart;
                    // ֻ����¼�Ų��·��
                    if (strPath == "")
                    {
                        bObject = false;
                        goto DOGET;
                    }

                    strFirstPart = StringUtil.GetFirstPartPath(ref strPath);
                    //***********�Ե���2��*************
                    // ����Ϊֹ��strPath����object��xpath�� strFirstPart������object �� xpath
                    if (strFirstPart != "object"
                        && strFirstPart != "xpath")
                    {
                        strError = "��Դ·�� '" + strResPath + "' ���Ϸ�,��3��������'object'��'xpath'";
                        return -7;
                    }
                    if (strPath == "")  //object��xpath�¼�������ֵ
                    {
                        strError = "��Դ·�� '" + strResPath + "' ���Ϸ�,����3����'object'��'xpath'����4�����������ݡ�";
                        return -7;
                    }

                    if (strFirstPart == "object")
                    {
                        strObjectID = strPath;
                        bObject = true;
                    }
                    else
                    {
                        strXPath = strPath;
                        bObject = false;
                    }

                    ///////////////////////////////////
                ///��ʼ������
                //////////////////////////////////////////

                DOGET:


                    // �������ݿ��м�¼��Ȩ��
                    string strExistRights = "";
                    bool bHasRight = user.HasRights(strDbName + "/" + strRecordID,
                        ResType.Record,
                        "read",
                        out strExistRights);
                    if (bHasRight == false)
                    {
                        strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strDbName + "'��û��'����¼(read)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                        return -6;
                    }

                    if (bObject == true)  //����
                    {
                        //		-1  ����
                        //		-4  ��¼������
                        //		>=0 ��Դ�ܳ���
                        nRet = db.GetObject(strRecordID,
                            strObjectID,
                            nStart,
                            nLength,
                            nMaxLength,
                            strStyle,
                            out baData,
                            out strMetadata,
                            out baOutputTimestamp,
                            out strError);

                        if (StringUtil.IsInList("outputpath", strStyle) == true)
                        {
                            strOutputResPath = strDbName + "/" + strRecordID + "/object/" + strObjectID;

                        }
                    }
                    else
                    {
                        string strOutputID;
                        // return:
                        //		-1  ����
                        //		-4  δ�ҵ���¼
                        //      -10 ��¼�ֲ�δ�ҵ�
                        //		>=0 ��Դ�ܳ���
                        //      nAdditionError -50 ��һ�������¼���Դ��¼������
                        nRet = db.GetXml(strRecordID,
                            strXPath,
                            nStart,
                            nLength,
                            nMaxLength,
                            strStyle,
                            out baData,
                            out strMetadata,
                            out strOutputID,
                            out baOutputTimestamp,
                            true,
                            out nAdditionError,
                            out strError);
                        if (StringUtil.IsInList("outputpath", strStyle) == true)
                        {
                            strRecordID = strOutputID;
                        }

                        if (StringUtil.IsInList("outputpath", strStyle) == true)
                        {
                            if (strXPath == "")
                                strOutputResPath = strDbName + "/" + strRecordID;
                            else
                                strOutputResPath = strDbName + "/" + strRecordID + "/xpath/" + strXPath;

                        }
                    }
                }

                return nRet;

            }
            finally
            {
                //******************�Կ⼯�Ͻ����******
                this.m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
			this.WriteDebugInfo("GetRes()���Կ⼯�Ͻ������");
#endif
            }
        }

        // ���һ��·���Ƿ������ݿ��¼·��
        private bool IsRecordPath(string strResPath)
        {
            string[] paths = strResPath.Split(new char[] { '/' });
            if (paths.Length >= 2)
            {
                if (StringUtil.IsPureNumber(paths[1]) == true
                    || paths[1] == "?"
                    || paths[1] == "-1")
                {
                    return true;
                }
            }
            return false;
        }


        // ��ָ����Χ�������ļ�
        // strRoleName:  ��ɫ��,��Сд����
        // ��������ͬGetXml(),��strOutputResPath����
        // ��: ��ȫ��
        // return:
        //		-1  һ���Դ���
        //		-4	δ�ҵ�·����Ӧ�Ķ���
        //		-6	û���㹻��Ȩ��
        //		>= 0    �ɹ� ������󳤶�
        // �ߣ���ȫ
        public int GetFileCfgItem(string strCfgItemPath,
            int nStart,
            int nLength,
            int nMaxLength,
            string strStyle,
            User user,
            out byte[] destBuffer,
            out string strMetadata,
            out byte[] outputTimestamp,
            out string strError)
        {
            strMetadata = "";
            destBuffer = null;
            outputTimestamp = null;
            strError = "";

            // ��鵱ǰ�ʻ������������Ȩ�ޣ���ʱ����Ȩ�޵Ĵ����������Ƿ���ڣ��ٱ���
            string strExistRights = "";
            bool bHasRight = user.HasRights(strCfgItemPath,
                ResType.File,
                "read",
                out strExistRights);


            //**********�����ݿ⼯�ϼӶ���**************
            this.m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.container.WriteDebugInfo("GetCfgFile()����'" + this.GetCaption("zh-cn") + "'���ݿ⼯�ϼӶ�����");
#endif
            try
            {

                string strFilePath = "";//this.GetCfgItemLacalPath(strCfgItemPath);
                // return:
                //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
                //		-2	û�ҵ��ڵ�
                //		-3	localname����δ�����Ϊֵ��
                //		-4	localname�ڱ��ز�����
                //		-5	���ڶ���ڵ�
                //		0	�ɹ�
                int nRet = this.GetFileCfgItemLacalPath(strCfgItemPath,
                    out strFilePath,
                    out strError);
                if (nRet != 0)
                {
                    if (nRet == -2)
                        return -4;
                    return -1;
                }

                // ��ʱ�ٱ�Ȩ�޵Ĵ�
                if (bHasRight == false)
                {
                    strError = "�����ʻ���Ϊ'" + user.Name + "'����·��Ϊ'" + strCfgItemPath + "'����������û��'��(read)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                    return -6;
                }

                // return:
                //		-1      ����
                //		>= 0	�ɹ���������󳤶�
                return DatabaseCollection.GetFileForCfgItem(strFilePath,
                    nStart,
                    nLength,
                    nMaxLength,
                    strStyle,
                    out destBuffer,
                    out strMetadata,
                    out outputTimestamp,
                    out strError);
            }
            finally
            {
                //****************�����ݿ⼯�Ͻ����**************
                this.m_lock.ReleaseReaderLock();
#if DEBUG_LOCK	
				this.container.WriteDebugInfo("GetCfgFile()����'" + this.GetCaption("zh-cn") + "'���ݿ⼯�Ͻ������");
#endif
            }
        }

        // ΪGetCfgItem���������ڲ�����
        // return:
        //		-1      ����
        //		>= 0	�ɹ���������󳤶�
        public static int GetFileForCfgItem(string strFilePath,
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
                strError = "����������������·��Ϊ'" + strFilePath + "'���ļ���";
                return -1;
            }

            // 1.ȡʱ���
            if (StringUtil.IsInList("timestamp", strStyle) == true)
            {
                string strNewFileName = DatabaseUtil.GetNewFileName(strFilePath);
                if (File.Exists(strNewFileName) == true)
                {
                    outputTimestamp = DatabaseUtil.CreateTimestampForCfg(strNewFileName);
                }
                else
                {
                    outputTimestamp = DatabaseUtil.CreateTimestampForCfg(strFilePath);
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

        // �õ�һ���ļ���������ı����ļ�����·��
        // parameters:
        //		strFileCfgItemPath	�ļ����������·������ʽΪ'dir1/dir2/file'
        //		strLocalPath	out���������ض�Ӧ�ı����ļ�����·��	
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
        //		-2	û�ҵ��ڵ�
        //		-3	localname����δ�����Ϊֵ��
        //		-4	localname�ڱ��ز�����
        //		-5	���ڶ���ڵ�
        //		0	�ɹ�
        // �ߣ�����ȫ
        public int GetFileCfgItemLacalPath(string strFileCfgItemPath,
            out string strLocalPath,
            out string strError)
        {
            strLocalPath = "";
            strError = "";

            if (strFileCfgItemPath == ""
                || strFileCfgItemPath == null)
            {
                strError = "GetCfgItemLacalPath()��strPath����ֵ����Ϊnull����ַ���";
                return -1;
            }
            List<XmlNode> nodes = DatabaseUtil.GetNodes(this.NodeDbs,
                strFileCfgItemPath);
            if (nodes.Count == 0)
            {
                strError = "��������δ����·��Ϊ'" + strFileCfgItemPath + "'�������ļ���";
                return -2;
            }
            if (nodes.Count > 1)
            {
                strError = "��������·��Ϊ'" + strFileCfgItemPath + "'������������'" + Convert.ToString(nodes.Count) + "'���������ļ����Ϸ���";
                return -5;
            }

            XmlNode nodeFile = nodes[0];

            string strPureFileName = DomUtil.GetAttr(nodeFile, "localname");
            if (strPureFileName == "")
            {
                strError = "��������·��Ϊ'" + strFileCfgItemPath + "'���ļ���������δ�����Ӧ�������ļ���";
                return -3;
            }

            string strLocalDir = DatabaseUtil.GetLocalDir(this.NodeDbs,
                nodeFile.ParentNode);

            string strRealPath = "";
            if (strLocalDir == "")
                strRealPath = this.DataDir + "\\" + strPureFileName;
            else
                strRealPath = this.DataDir + "\\" + strLocalDir + "\\" + strPureFileName;

            strLocalPath = strRealPath;
            if (File.Exists(strRealPath) == false)
            {
                strError = "��������·��Ϊ'" + strFileCfgItemPath + "'���ļ����������Ӧ�������ļ��ڱ��ز����ڡ�";
                return -4;
            }
            return 0;
        }


        // ɾ����Դ�������Ǽ�¼ �� ���������֧�ֶ�����Դ�򲿷ּ�¼��
        // parameter:
        //		strResPath		��Դ·��,����Ϊnull����ַ���
        //						��Դ���Ϳ��������ݿ���������(Ŀ¼���ļ�)����¼
        //						��������: ����/��������·��
        //						��¼: ����/��¼��
        //		user	��ǰ�ʻ����󣬲���Ϊnull
        //		baInputTimestamp	�����ʱ���
        //		baOutputTimestamp	out����������ʱ���
        //		strError	out���������س�����Ϣ
        // return:
        //      -1	һ���Դ�����������������Ϸ���
        //      -2	ʱ�����ƥ��
        //      -4	δ�ҵ�·����Ӧ����Դ
        //      -5	δ�ҵ����ݿ�
        //      -6	û���㹻��Ȩ��
        //      -7	·�����Ϸ�
        //      0	�����ɹ�
        // ˵��: 
        // 1)ɾ����Ҫ��ǰ�ʻ��Խ���ɾ���ļ�¼����deleteȨ��		
        // 2)ɾ����¼����ȷ������ɾ����¼�壬����ɾ���ü�¼���������ж�����Դ
        // 3)ɾ������Ŀ¼��Ҫ��ʱ���,ͬʱbaOutputTimestampҲ��null
        public int DeleteRes(string strResPath,
            User user,
            byte[] baInputTimestamp,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            baOutputTimestamp = null;
            strError = "";

            //-----------------------------------------
            //��������������м��
            //---------------------------------------
            if (strResPath == null || strResPath == "")
            {
                strError = "DeleteRes()���ô���strResPath��������Ϊnull����ַ�����";
                return -1;
            }
            if (user == null)
            {
                strError = "DeleteRes()���ô���user��������Ϊnull��";
                return -1;
            }


            //-----------------------------------------
            //��ʼ������ 
            //---------------------------------------

            //******************�ӿ⼯�ϼӶ���******
            this.m_lock.AcquireReaderLock(m_nTimeOut);

#if DEBUG_LOCK
			this.WriteDebugInfo("CheckDbsTailNoSafety()���Կ⼯�ϼӶ�����");
#endif
            try
            {
                int nRet = 0;

                bool bRecordPath = this.IsRecordPath(strResPath);
                if (bRecordPath == false)
                {
                    // Ҳ���������ݿ����


                    // ɾ��ʵ�ʵ������ļ�
                    //      -1  һ���Դ���
                    //      -2  ʱ�����ƥ��
                    //      -4  δ�ҵ�·����Ӧ����Դ
                    //      -6  û���㹻��Ȩ��
                    //      0   �ɹ�
                    nRet = this.DeleteCfgItem(user,
                        strResPath,
                        baInputTimestamp,
                        out baOutputTimestamp,
                        out strError);
                    if (nRet <= -1)
                        return nRet;
                }
                else
                {

                    string strPath = strResPath;
                    string strDbName = StringUtil.GetFirstPartPath(ref strPath);
                    if (strPath == "")
                    {
                        strError = "��Դ·��'" + strResPath + "'���Ϸ���δָ������¼���";
                        return -7;
                    }

                    // ������Դ���ͣ�д��Դ
                    Database db = this.GetDatabase(strDbName);
                    if (db == null)
                    {
                        strError = "û�ҵ���Ϊ'" + strDbName + "'�����ݿ⡣";
                        return -5;
                    }

                    string strFirstPart = StringUtil.GetFirstPartPath(ref strPath);
                    //***********�Ե���2��*************
                    // ����Ϊֹ��strPath����cfgs���¼�Ų��ˣ��¼�������ж�
                    // strFirstPart������Ϊcfg���¼��

                    string strRecordID = strFirstPart;

                    // ��鵱ǰ�ʻ��Ƿ���ɾ����¼
                    string strExistRights = "";
                    bool bHasRight = user.HasRights(strResPath,//db.GetCaption("zh-cn"),
                        ResType.Record,
                        "delete",
                        out strExistRights);
                    if (bHasRight == false)
                    {
                        strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strDbName + "'���ݿ�û��'ɾ����¼(delete)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                        return -6;
                    }

                    // return:
                    //		-1  һ���Դ���
                    //		-2  ʱ�����ƥ��
                    //      -4  δ�ҵ���¼
                    //		0   �ɹ�
                    nRet = db.DeleteRecord(strRecordID,
                        baInputTimestamp,
                        out baOutputTimestamp,
                        out strError);
                    if (nRet <= -1)
                        return nRet;
                }
            }
            finally
            {
                m_lock.ReleaseReaderLock();
                //*************�Կ⼯�Ͻ����***********
#if DEBUG_LOCK
				this.WriteDebugInfo("CheckDbsTailNoSafety()���Կ⼯�Ͻ������");
#endif
            }

            //��ʱ����database.xml // ���ü����ĺ�����
            if (this.Changed == true)
                this.SaveXmlSafety();

            return 0;

        }

        // ɾ��һ���������������Ŀ¼��Ҳ�������ļ�
        // return:
        //      -1  һ���Դ���
        //      -2  ʱ�����ƥ��
        //      -4  δ�ҵ�·����Ӧ����Դ
        //      -6  û���㹻��Ȩ��
        //      0   �ɹ�
        public int DeleteCfgItem(User user,
            string strCfgItemPath,
            byte[] intputTimestamp,
            out byte[] outputTimestamp,
            out string strError)
        {
            outputTimestamp = null;
            strError = "";

            if (strCfgItemPath == null
                || strCfgItemPath == "")
            {
                strError = "DeleteCfgItem()���ô���strCfgItemPath����ֵ����Ϊnull����ַ�����";
                return -1;
            }

            List<XmlNode> nodes = DatabaseUtil.GetNodes(this.NodeDbs,
                strCfgItemPath);
            if (nodes.Count == 0)
            {
                strError = "������������·��Ϊ'" + strCfgItemPath + "'���������";
                return -4;
            }
            if (nodes.Count != 1)
            {
                strError = "��������·��Ϊ'" + strCfgItemPath + "'�������������Ϊ'" + Convert.ToString(nodes.Count) + "'��database.xml�����ļ��쳣��";
                return -1;
            }


            string strExistRights = "";
            bool bHasRight = false;

            XmlNode node = nodes[0];

            if (node.Name == "dir")
            {
                // ��鵱ǰ�ʻ��Ƿ���ɾ����¼'
                bHasRight = user.HasRights(strCfgItemPath,
                    ResType.Directory,
                    "delete",
                    out strExistRights);
                if (bHasRight == false)
                {
                    strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strCfgItemPath + "'��������û��'ɾ��(delete)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                    return -6;
                }
                string strDir = DatabaseUtil.GetLocalDir(this.NodeDbs, node).Trim();
                Directory.Delete(this.DataDir + "\\" + strDir, true);
                node.ParentNode.RemoveChild(node);
                return 0;
            }
            else if (String.Compare(node.Name, "database", true) == 0)
            {

            }


            // ��鵱ǰ�ʻ��Ƿ���ɾ����¼'
            bHasRight = user.HasRights(strCfgItemPath,
                ResType.File,
                "delete",
                out strExistRights);
            if (bHasRight == false)
            {
                strError = "�����ʻ���Ϊ'" + user.Name + "'����'" + strCfgItemPath + "'��������û��'ɾ��(delete)'Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                return -6;
            }

            string strFilePath = "";//GetCfgItemLacalPath(strCfgItemPath);
            // return:
            //		-1	һ���Դ��󣬱�����ô��󣬲������Ϸ���
            //		-2	û�ҵ��ڵ�
            //		-3	localname����δ�����Ϊֵ��
            //		-4	localname�ڱ��ز�����
            //		-5	���ڶ���ڵ�
            //		0	�ɹ�
            int nRet = this.GetFileCfgItemLacalPath(strCfgItemPath,
                out strFilePath,
                out strError);
            if (nRet != 0)
            {
                if (nRet == -1 || nRet == -5)
                    return -1;

            }
            if (strFilePath != "")
            {
                string strNewFileName = DatabaseUtil.GetNewFileName(strFilePath);

                if (File.Exists(strFilePath) == true)
                {

                    byte[] oldTimestamp = null;
                    if (File.Exists(strNewFileName) == true)
                        oldTimestamp = DatabaseUtil.CreateTimestampForCfg(strNewFileName);
                    else
                        oldTimestamp = DatabaseUtil.CreateTimestampForCfg(strFilePath);

                    outputTimestamp = oldTimestamp;
                    if (ByteArray.Compare(oldTimestamp, intputTimestamp) != 0)
                    {
                        strError = "ʱ�����ƥ��";
                        return -2;
                    }
                }

                File.Delete(strNewFileName);
                File.Delete(strFilePath);

                string strRangeFileName = DatabaseUtil.GetRangeFileName(strFilePath);
                if (File.Exists(strRangeFileName) == false)
                    File.Delete(strRangeFileName);

                string strMetadataFileName = DatabaseUtil.GetMetadataFileName(strFilePath);
                if (File.Exists(strMetadataFileName) == false)
                    File.Delete(strMetadataFileName);
            }
            node.ParentNode.RemoveChild(node);

            this.Changed = true;
            this.SaveXml();

            return 0;
        }



        // ���ݷ������ϵ�ָ��·���г����¼�������
        // parameters:
        //		strPath	·��,�������������֣�
        //				��ʽΪ: "���ݿ���/�¼���/�¼���",
        //				��Ϊnull����Ϊ""ʱ����ʾ�г��÷����������е����ݿ�
        //		lStart	��ʼλ��,��0��ʼ ,����С��0
        //		lLength	���� -1��ʾ��lStart�����
        //		strLang	���԰汾 �ñ�׼��ĸ��ʾ������zh-cn
        //      strStyle    �Ƿ�Ҫ�г��������Ե�����? "alllang"��ʾҪ�г�ȫ������
        //		items	 out�����������¼���������
        // return:
        //		-1  ����
        //      -6  Ȩ�޲���
        //		0   ����
        // ˵��	ֻ�е�ǰ�ʻ���������"list"Ȩ��ʱ�������г�����
        //		����б������������ݿ�ʱ�������е����ݿⶼû��listȨ�ޣ�������������û�����ݿ��������ֿ���
        public int Dir(string strResPath,
            long lStart,
            long lLength,
            long lMaxLength,
            string strLang,
            string strStyle,
            User user,
            out ResInfoItem[] items,
            out int nTotalLength,
            out string strError)
        {
            items = new ResInfoItem[0];
            nTotalLength = 0;

            ArrayList aItem = new ArrayList();
            strError = "";
            int nRet = 0;
            //******************�ӿ⼯�ϼӶ���******
            this.m_lock.AcquireReaderLock(m_nTimeOut);

#if DEBUG_LOCK
			this.WriteDebugInfo("Dir()���Կ⼯�ϼӶ�����");
#endif
            try
            {

                if (strResPath == "" || strResPath == null)
                {
                    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    // 1.ȡ�������µ����ݿ�

                    nRet = this.GetDirableChildren(user,
                        strLang,
                        strStyle,
                        out aItem,
                        out strError);
                    if (this.Count > 0 && aItem.Count == 0)
                    {
                        strError = "�����ʻ���Ϊ'" + user.Name + "'�������е����ݿⶼû��'��ʾ(list)'Ȩ�ޡ�";
                        return -6;
                    }
                }
                else
                {
                    string strPath = strResPath;
                    string strDbName = StringUtil.GetFirstPartPath(ref strPath);

                    // ���������ݿ�Ҳ��������������
                    if (strPath == "")
                    {
                        Database db = this.GetDatabase(strDbName);
                        if (db != null)
                        {
                            // return:
                            //		-1	����
                            //		0	�ɹ�
                            nRet = db.GetDirableChildren(user,
                                strLang,
                                strStyle,
                                out aItem,
                                out strError);
                            if (nRet == -1)
                                return -1;
                            goto END1;
                        }
                    }

                    // return:
                    //		-1	����
                    //		0	�ɹ�
                    nRet = this.DirCfgItem(user,
                        strResPath,
                        out aItem,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }

            }
            finally
            {
                m_lock.ReleaseReaderLock();
                //*************�Կ⼯�Ͻ����***********
#if DEBUG_LOCK
				this.WriteDebugInfo("Dir()���Կ⼯�Ͻ������");
#endif
            }


        END1:
            // �г�ʵ����Ҫ����
            nTotalLength = aItem.Count;
            int nOutputLength;
            // return:
            //		-1  ����
            //		0   �ɹ�
            nRet = DatabaseUtil.GetRealLength((int)lStart,
                (int)lLength,
                nTotalLength,
                (int)lMaxLength,
                out nOutputLength,
                out strError);
            if (nRet == -1)
                return -1;

            items = new ResInfoItem[(int)nOutputLength];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = (ResInfoItem)(aItem[i + (int)lStart]);
            }

            return 0;
        }


        // �õ�ĳһָ��·��strPath�Ŀ�����ʾ���¼�
        // parameters:
        //		oUser	��ǰ�ʻ�
        //		db	��ǰ���ݿ�
        //		strPath	���������·��
        //		strLang	���԰汾
        //		aItem	out���������ؿ�����ʾ���¼�
        //		strError	out������������Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        private int DirCfgItem(User user,
            string strCfgItemPath,
            out ArrayList aItem,
            out string strError)
        {
            strError = "";
            aItem = new ArrayList();

            if (this.NodeDbs == null)
            {
                strError = "�����������ļ�δ����<dbs>Ԫ��";
                return -1;
            }
            List<XmlNode> list = DatabaseUtil.GetNodes(this.NodeDbs,
                strCfgItemPath);
            if (list.Count == 0)
            {
                strError = "δ�ҵ�·��Ϊ'" + strCfgItemPath + "'��Ӧ�����";
                return -1;
            }

            if (list.Count > 1)
            {
                strError = "���������������ļ����Ϸ�����鵽·��Ϊ'" + strCfgItemPath + "'��Ӧ�Ľڵ���'" + Convert.ToString(list.Count) + "'��������ֻ����һ����";
                return -1;
            }
            XmlNode node = list[0];

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode child = node.ChildNodes[i];
                string strChildName = DomUtil.GetAttr(child, "name");
                if (strChildName == "")
                    continue;

                string strTempPath = strCfgItemPath + "/" + strChildName;
                string strExistRights;
                bool bHasRight = false;


                ResInfoItem resInfoItem = new ResInfoItem();
                resInfoItem.Name = strChildName;
                if (child.Name == "dir")
                {
                    bHasRight = user.HasRights(strTempPath,
                     ResType.Directory,
                     "list",
                     out strExistRights);
                    if (bHasRight == false)
                        continue;

                    resInfoItem.HasChildren = true;
                    resInfoItem.Type = 4;

                    resInfoItem.TypeString = DomUtil.GetAttr(child, "type");    // xietao 2006/6/5 add
                }
                else
                {
                    bHasRight = user.HasRights(strTempPath,
                        ResType.File,
                        "list",
                        out strExistRights);
                    if (bHasRight == false)
                        continue;
                    resInfoItem.HasChildren = false;
                    resInfoItem.Type = 5;

                    resInfoItem.TypeString = DomUtil.GetAttr(child, "type");    // xietao 2006/6/5 add

                }
                aItem.Add(resInfoItem);
            }
            return 0;
        }

        // �г��������µ�ǰ�ʻ�����ʾȨ�޵����ݿ�
        // �ߣ�����ȫ��
        // parameters:
        //      strStyle    �Ƿ�Ҫ�г��������Ե�����? "alllang"��ʾҪ�г��������Ե�����
        public int GetDirableChildren(User user,
            string strLang,
            string strStyle,
            out ArrayList aItem,
            out string strError)
        {
            aItem = new ArrayList();
            strError = "";

            if (this.NodeDbs == null)
            {
                strError = "��װ�������ļ����Ϸ���δ����<dbs>Ԫ��";
                return -1;
            }

            foreach (XmlNode child in this.NodeDbs.ChildNodes)
            {
                string strChildName = DomUtil.GetAttr(child, "name");
                if (String.Compare(child.Name, "database", true) != 0
                    && strChildName == "")
                    continue;

                if (String.Compare(child.Name, "database", true) != 0
                    && String.Compare(child.Name, "dir", true) != 0
                    && String.Compare(child.Name, "file", true) != 0)
                {
                    continue;
                }

                string strExistRights;
                bool bHasRight = false;

                ResInfoItem resInfoItem = new ResInfoItem();
                if (String.Compare(child.Name, "database", true) == 0)
                {
                    string strID = DomUtil.GetAttr(child, "id");
                    Database db = this.GetDatabaseByID("@" + strID);
                    if (db == null)
                    {
                        strError = "δ�ҵ�idΪ'" + strID + "'�����ݿ�";
                        return -1;
                    }

                    bHasRight = user.HasRights(db.GetCaption("zh"),
                        ResType.Database,
                        "list",
                        out strExistRights);
                    if (bHasRight == false)
                        continue;

                    if (StringUtil.IsInList("account", db.GetDbType(), true) == true)
                        resInfoItem.Style = 1;
                    else
                        resInfoItem.Style = 0;

                    resInfoItem.TypeString = db.GetDbType();

                    resInfoItem.Name = db.GetCaptionSafety(strLang);
                    resInfoItem.Type = 0;   // ���ݿ�
                    resInfoItem.HasChildren = true;

                    // ���Ҫ���ȫ�����Ե�����
                    if (StringUtil.IsInList("alllang", strStyle) == true)
                    {
                        List<string> results = db.GetAllLangCaptionSafety();
                        string [] names = new string[results.Count];
                        results.CopyTo(names);
                        resInfoItem.Names = names;
                    }
                }
                else if (String.Compare(child.Name, "dir", true) == 0)
                {
                    bHasRight = user.HasRights(strChildName,
                        ResType.Directory,
                        "list",
                        out strExistRights);
                    if (bHasRight == false)
                        continue;
                    resInfoItem.HasChildren = true;
                    resInfoItem.Type = 4;   // Ŀ¼
                    resInfoItem.Name = strChildName;

                    resInfoItem.TypeString = DomUtil.GetAttr(child, "type");   // xietao 2006/6/5 add
                }
                else
                {
                    bHasRight = user.HasRights(strChildName,
                        ResType.File,
                        "list",
                        out strExistRights);
                    if (bHasRight == false)
                        continue;
                    resInfoItem.HasChildren = false;
                    resInfoItem.Name = strChildName;
                    resInfoItem.Type = 5;   // �ļ�?

                    resInfoItem.TypeString = DomUtil.GetAttr(child, "type");   // xietao 2006/6/5 add
                }
                aItem.Add(resInfoItem);
            }
            return 0;
        }

        // �����û����ӿ��в����û���¼���õ��û�����
        // ������δ���뼯��, �������Ϊ�������
        // parameters:
        //		strBelongDb	�û����������ݿ�,��������
        //      user        out�����������ʻ�����
        //      strError    out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	δ�ҵ��ʻ�
        //		1	�ҵ���
        // �ߣ���ȫ
        internal int ShearchUser(string strUserName,
            out User user,
            out string strError)
        {
            user = null;
            strError = "";

            int nRet = 0;

            DpResultSet resultSet = new DpResultSet();


            //*********���ʻ��⼯�ϼӶ���***********
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK
			this.WriteDebugInfo("ShearchUser()�����ʻ��⼯�ϼӶ�����");
#endif
            try
            {
                // return:
                //		-1	����
                //		0	�ɹ�
                nRet = this.SearchUserInternal(strUserName,
                    resultSet,
                    out strError);
                if (nRet == -1)
                    return -1;
            }
            finally
            {
                //*********���ʻ��⼯�Ͻ����*************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
				this.m_dbColl.WriteDebugInfo("ShearchUser()�����ʻ��⼯�Ͻ������");
#endif
            }

            // �����û���û�ҵ���Ӧ���ʻ���¼
            long lCount = resultSet.Count;
            if (lCount == 0)
                return 0;

            if (lCount > 1)
            {
                strError = "�û���'" + strUserName + "'��Ӧ������¼";
                return -1;
            }

            // ����һ���ʻ���
            DpRecord record = (DpRecord)resultSet[0];

            // ����һ��DpPsthʵ��
            DbPath path = new DbPath(record.ID);

            // �ҵ�ָ���ʻ����ݿ�
            Database db = this.GetDatabaseSafety(path.Name);
            if (db == null)
            {
                strError = "δ�ҵ�'" + strUserName + "'�ʻ���Ӧ����Ϊ'" + path.Name + "'�����ݿ����";
                return -1;
            }

            // ���ʻ������ҵ���¼
            string strXml = "";
            // return:
            //      -1  ����
            //      -4  ��¼������
            //      0   ��ȷ
            nRet = db.GetXmlDataSafety(path.ID,
                out strXml,
                out strError);
            if (nRet <= -1)  // ��-4��-1����Ϊ-1����
                return -1;

            //���ص�dom
            XmlDocument dom = new XmlDocument();
            //dom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue
            try
            {
                dom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                strError = "�����û� '" + strUserName + "' ���ʻ���¼��domʱ����,ԭ��:" + ex.Message;
                return -1;
            }

            user = new User();
            // return:
            //      -1  ����
            //      0   �ɹ�
            nRet = user.Initial(
                record.ID,
                dom,
                db,
                this,
                out strError);
            if (nRet == -1)
                return -1;

            return 1;
        }

        // ���ݼ�¼·���õ����ݿ����
        public Database GetDatabaseFromRecPath(string strRecPath)
        {
            // ����һ��DpPsthʵ��
            DbPath path = new DbPath(strRecPath);

            // �ҵ�ָ���ʻ����ݿ�
            return this.GetDatabaseSafety(path.Name);
        }

                // �������ʻ�������б��в����ʻ�
        // parameter
        //		strUserName �û���
        //		resultSet   �����,���ڴ�Ų��ҵ����û�
        //      strError    out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        // �ߣ�����ȫ
        private int SearchUserInternal(string strUserName,
            DpResultSet resultSet,
            out string strError)
        {
            strError = "";

            foreach (Database db in this)
            {
                if (StringUtil.IsInList("account", db.GetDbType()) == false)
                    continue;

                if (strUserName.Length > db.KeySize)
                    continue;

                string strWarning = "";
                SearchItem searchItem = new SearchItem();
                searchItem.TargetTables = "";
                searchItem.Word = strUserName;
                searchItem.Match = "exact";
                searchItem.Relation = "=";
                searchItem.DataType = "string";
                searchItem.MaxCount = -1;
                searchItem.OrderBy = "";

                // �ʻ��ⲻ��ȥ������
                // return:
                //		-1	����
                //		0	�ɹ�
                int nRet = db.SearchByUnion(searchItem,
                    null,       //�����ж� , deleget
                    resultSet,
                    0,
                    out strError,
                    out strWarning);
                if (nRet == -1)
                    return -1;
            }
            return 0;
        }

    } // end of class DatabaseCollection


    //*****************************************************

    // string���͵�ArrayList������IComparer�ӿ�
    public class ComparerClass : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            if (!(x is String))
                throw new Exception("object x is not a String");
            if (!(y is String))
                throw new Exception("object y is not a String");

            string strText1 = (string)x;
            string strText2 = (string)y;

            return String.Compare(strText1, strText2, true);
        }
    }


    // ���ͨѶ�Ƿ������ŵ�delegate
    public delegate bool Delegate_isConnected();

    #region ר�����ڼ�������
    public class DatabaseCommandTask
    {
        public SqlCommand m_command = null;
        public AutoResetEvent m_event = new AutoResetEvent(false);

        public bool bError = false;
        public string ErrorString = "";
        //���ⲿʹ��
        public SqlDataReader DataReader = null;

        public DatabaseCommandTask(SqlCommand command)
        {
            m_command = command;
        }
        public void Cancel()
        {
            m_command.Cancel();
        }
        // ������
        public void ThreadMain()
        {
            try
            {
                DataReader = m_command.ExecuteReader();
            }
            catch (SqlException sqlEx)
            {
                this.bError = true;
                if (sqlEx.Errors is SqlErrorCollection)
                    this.ErrorString = "���ݿ���δ��ʼ����";
                else
                    this.ErrorString = "�����߳�:" + sqlEx.Message;
            }
            catch (Exception ex)
            {
                this.bError = true;
                this.ErrorString = "�����߳�:" + ex.Message;
            }
			finally  // һ��Ҫ�����ź�
            {
                m_event.Set();
            }
        }
    }
    #endregion

    // ��Դ����Ϣ
    // ��ʱ����DigitalPlatform.rms.Service�����Ҫ��Database.xml��ʹ�ã������ƶ������
    public class ResInfoItem
    {
        public int Type;	// ����,0 �⣬1 ;��,4 cfgs,5 file
        public string Name;	// ������;����
        public bool HasChildren = true;  //�Ƿ��ж���
        public int Style = 0;   // 0x01:�ʻ���  // ԭ��Style

        public string TypeString = "";  // ����
        public string[] Names;    // ���� ���������µ����֡�ÿ��Ԫ�صĸ�ʽ ���Դ���:����
    }
}

