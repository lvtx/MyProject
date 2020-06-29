//#define DEBUG_LOCK_SQLDATABASE

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

using DigitalPlatform.ResultSet;
using DigitalPlatform.Text;
using DigitalPlatform.Range;
using DigitalPlatform.Xml;
using DigitalPlatform.IO;

namespace DigitalPlatform.rms
{
    // SQL��������
    public class SqlDatabase : Database
    {
        // �����ַ���
        private string m_strConnString = "";

        // Sql���ݿ�����
        private string m_strSqlDbName = "";

        public SqlDatabase(DatabaseCollection container)
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
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("Initial()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif

                //      -1  ����
                //      0   �ɹ�
                // ��: ����ȫ��
                int nRet = this.container.InternalGetConnString(
                    out this.m_strConnString,
                    out strError);
                if (nRet == -1)
                    return -1;

                // �����㳤��
                // return:
                //      -1  ����
                //      0   �ɹ�
                // ��: ����ȫ
                nRet = this.container.InternalGetKeySize(
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

                // <sqlserverdb>�ڵ�
                XmlNode nodeSqlServerDb = this.m_propertyNode.SelectSingleNode("sqlserverdb");
                if (nodeSqlServerDb == null)
                {
                    strError = "�����ļ����Ϸ�����nameΪ'" + this.GetCaption("zh-cn") + "'��database/property�¼�δ����<sqlserverdb>Ԫ��";
                    return -1;
                }

                // ���SqlServer������ֻ��Sql���Ϳ����Ҫ
                this.m_strSqlDbName = DomUtil.GetAttr(nodeSqlServerDb, "name").Trim();
                if (this.m_strSqlDbName == "")
                {
                    strError = "�����ļ����Ϸ�����nameΪ'" + this.GetCaption("zh-cn") + "'��database/property/sqlserverdb�Ľڵ�δ����'name'���ԣ���'name'����ֵΪ��";
                    return -1;
                }
            }
            finally
            {
                m_lock.ReleaseWriterLock();
                //***********�����ݿ��д��*************
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("Initial()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            }

            return 0;
        }

        // �õ�����Դ���ƣ�����Sql���ݿ⣬����Sql���ݿ�����
        public override string GetSourceName()
        {
            return this.m_strSqlDbName;
        }

        // ��ʼ�����ݿ⣬ע���麯������Ϊprivate
        // parameter:
        //		strError    out���������س�����Ϣ
        // return:
        //		-1  ����
        //		0   �ɹ�
        // ��: ��ȫ��
        // ��д����ԭ���޸ļ�¼β�ţ������SQL�Ĳ������ص�����
        public override int InitialPhysicalDatabase(out string strError)
        {
            strError = "";

            //************�����ݿ��д��********************
            m_lock.AcquireWriterLock(m_nTimeOut);

#if DEBUG_LOCK_SQLDATABASE
			this.container.WriteDebugInfo("Initialize()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            try
            {
                SqlConnection connection = new SqlConnection(this.m_strConnString);
                connection.Open();
                try //����
                {
                    string strCommand = "";
                    SqlCommand command = null;
                    // 1.����
                    strCommand = this.GetCreateDbComdString();
                    command = new SqlCommand(strCommand,
                        connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        strError = "�������.\r\n"
                            + ex.Message + "\r\n"
                            + "SQL����:\r\n"
                            + strCommand;
                        return -1;
                    }

                    // 2.����
                    int nRet = this.GetCreateTablesString(out strCommand,
                        out strError);
                    if (nRet == -1)
                        return -1;
                    command = new SqlCommand(strCommand,
                        connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        strError = "�������.\r\n"
                            + ex.Message + "\r\n"
                            + "SQL����:\r\n"
                            + strCommand;
                        return -1;
                    }

                    // 3.������
                    nRet = this.GetCreateIndexString(out strCommand,
                        out strError);
                    if (nRet == -1)
                        return -1;
                    command = new SqlCommand(strCommand,
                        connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        strError = "����������.\r\n"
                            + ex.Message + "\r\n"
                            + "SQL����:\r\n"
                            + strCommand;
                        return -1;
                    }

                    // 4.����¼����Ϊ0
                    this.SetTailNo(0);

                    this.container.Changed = true;   //���ݸı�
                }
                finally
                {
                    connection.Close();
                }
            }
            finally
            {
                //*********************�����ݿ��д��******
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("Initialize()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            }
            return 0;
        }

        // �õ����������ַ���
        public string GetCreateDbComdString()
        {
            string strCommand = "use master " + "\n"
                + " if exists (select * from dbo.sysdatabases where name = N'" + this.m_strSqlDbName + "')" + "\n"
                + " drop database " + this.m_strSqlDbName + "\n"
                + " CREATE database " + this.m_strSqlDbName + "\n";

            strCommand += " use master " + "\n";

            return strCommand;
        }

        // �õ����������ַ���
        // return
        //		-1	����
        //		0	�ɹ�
        private int GetCreateTablesString(out string strCommand,
            out string strError)
        {
            strCommand = "";
            strError = "";

            // ����reocrds��
            strCommand = "use " + this.m_strSqlDbName + "\n"
                + "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[records]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)" + "\n"
                + "drop table [dbo].[records]" + "\n"
                + "CREATE TABLE [dbo].[records]" + "\n"
                + "(" + "\n"
                + "[id] [nvarchar] (255) NULL ," + "\n"
                + "[data] [image] NULL ," + "\n"
                + "[newdata] [image] NULL ," + "\n"
                + "[range] [nvarchar] (4000) NULL," + "\n"
                + "[dptimestamp] [nvarchar] (100) NULL ," + "\n"
                + "[metadata] [nvarchar] (4000) NULL ," + "\n"
                + ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]" + "\n" + "\n";


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


                // ���������
                for (int i = 0; i < aTableInfo.Count; i++)
                {
                    TableInfo tableInfo = aTableInfo[i];

                    strCommand += "\n" +
                        "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + tableInfo.SqlTableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)" + "\n" +
                        "drop table [dbo].[" + tableInfo.SqlTableName + "]" + "\n" +
                        "CREATE TABLE [dbo].[" + tableInfo.SqlTableName + "]" + "\n" +
                        "(" + "\n" +
                        "[keystring] [nvarchar] (" + Convert.ToString(this.KeySize) + ") Null," + "\n" +         //keystring�ĳ����������ļ���
                        "[fromstring] [nvarchar] (255) NULL ," + "\n" +
                        "[idstring] [nvarchar] (255)  NULL ," + "\n" +
                        "[keystringnum] [bigint] NULL " + "\n" +
                        ")" + "\n" + "\n";
                }
            }

            strCommand += " use master " + "\n";

            return 0;
        }

        // �����������ַ���
        // return
        //		-1	����
        //		0	�ɹ�
        public int GetCreateIndexString(out string strCommand,
            out string strError)
        {
            strCommand = "";
            strError = "";

            strCommand = "use " + this.m_strSqlDbName + "\n"
                + " CREATE INDEX records_id_index " + "\n"
                + " ON records (id) \n";

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

                for (int i = 0; i < aTableInfo.Count; i++)
                {
                    TableInfo tableInfo = (TableInfo)aTableInfo[i];

                    strCommand += " CREATE INDEX " + tableInfo.SqlTableName + "_keystring_index \n"
                        + " ON " + tableInfo.SqlTableName + " (keystring) \n";
                    strCommand += " CREATE INDEX " + tableInfo.SqlTableName + "_keystringnum_index \n"
                        + " ON " + tableInfo.SqlTableName + " (keystringnum) \n";
                }
            }

            strCommand += " use master " + "\n";
            return 0;
        }

        // ɾ�����ݿ�
        // return:
        //      -1  ����
        //      0   �ɹ�
        public override int Delete(out string strError)
        {
            strError = "";

            //************�����ݿ��д��********************
            this.m_lock.AcquireWriterLock(m_nTimeOut);

#if DEBUG_LOCK_SQLDATABASE
			this.container.WriteDebugInfo("Delete()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            try //��
            {
                string strCommand = "";

                SqlConnection connection = new SqlConnection(this.m_strConnString);
                connection.Open();
                try //����
                {
                    // 1.ɾ���sql���ݿ�
                    SqlCommand command = null;
                    strCommand = "use master " + "\n"
                        + " if exists (select * from dbo.sysdatabases where name = N'" + this.m_strSqlDbName + "')" + "\n"
                        + " drop database " + this.m_strSqlDbName + "\n";
                    strCommand += " use master " + "\n";
                    command = new SqlCommand(strCommand,
                        connection);

                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    // ����������������ݿ⣬�򲻱���

                    if (!(sqlEx.Errors is SqlErrorCollection))
                    {
                        strError = "ɾ��sql�����.\r\n"
                           + sqlEx.Message + "\r\n"
                           + "SQL����:\r\n"
                           + strCommand;
                        return -1;
                    }
                }
                catch (Exception ex)
                {
                    strError = "ɾ��sql�����.\r\n"
                        + ex.Message + "\r\n"
                        + "SQL����:\r\n"
                        + strCommand;
                    return -1;
                }
                finally  //����
                {
                    connection.Close();
                }


                // ɾ������Ŀ¼
                string strCfgsDir = DatabaseUtil.GetLocalDir(this.container.NodeDbs,
                    this.m_selfNode);
                if (strCfgsDir != "")
                {
                    // Ӧ��Ŀ¼���أ������������ʹ�����Ŀ¼������ɾ����������Ϣ
                    if (this.container.IsExistCfgsDir(strCfgsDir, this) == true)
                    {
                        // ��������־дһ����Ϣ
                        this.container.WriteErrorLog("���ֳ���'" + this.GetCaption("zh-cn") + "'��ʹ��'" + strCfgsDir + "'Ŀ¼�⣬�����������ʹ�����Ŀ¼�����Բ�����ɾ����ʱɾ��Ŀ¼");
                    }
                    else
                    {
                        string strRealDir = this.container.DataDir + "\\" + strCfgsDir;
                        if (Directory.Exists(strRealDir) == true)
                        {
                            Directory.Delete(strRealDir, true);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                strError = "ɾ��'" + this.GetCaption("zh") + "'���ݿ����ԭ��:" + ex.Message;
                return -1;
            }
            finally
            {

                //*********************�����ݿ��д��**********
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("Delete()����'" + this.GetCaption("zh-cn") + "'���ݿ��д����");
#endif
            }

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

            Debug.Assert(searchItem != null, "SearchByID()���ô���searchItem����ֵ����Ϊnull��");
            Debug.Assert(isConnected != null, "SearchByID()���ô���isConnected����ֵ����Ϊnull��");
            Debug.Assert(resultSet != null, "SearchByID()���ô���resultSet����ֵ����Ϊnull��");

            SqlConnection connection = new SqlConnection(this.m_strConnString);
            connection.Open();
            try
            {
                List<SqlParameter> aSqlParameter = new List<SqlParameter>();
                string strWhere = "";
                if (searchItem.Match == "left"
                    || searchItem.Match == "")
                {
                    strWhere = " WHERE id LIKE @id and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    SqlParameter temp = new SqlParameter("@id", SqlDbType.NVarChar);
                    temp.Value = searchItem.Word + "%";
                    aSqlParameter.Add(temp);
                }
                else if (searchItem.Match == "middle")
                {
                    strWhere = " WHERE id LIKE @id and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    SqlParameter temp = new SqlParameter("@id", SqlDbType.NVarChar);
                    temp.Value = "%" + searchItem.Word + "%";
                    aSqlParameter.Add(temp);
                }
                else if (searchItem.Match == "right")
                {
                    strWhere = " WHERE id LIKE @id and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    SqlParameter temp = new SqlParameter("@id", SqlDbType.NVarChar);
                    temp.Value = "%" + searchItem.Word;
                    aSqlParameter.Add(temp);
                }
                else if (searchItem.Match == "exact")
                {
                    if (searchItem.DataType == "string")
                        searchItem.Word = DbPath.GetID10(searchItem.Word);

                    if (searchItem.Relation == "draw")
                    {
                        int nPosition;
                        nPosition = searchItem.Word.IndexOf("-");
                        if (nPosition >= 0)
                        {
                            string strStartID;
                            string strEndID;
                            StringUtil.SplitRange(searchItem.Word,
                                out strStartID,
                                out strEndID);
                            strStartID = DbPath.GetID10(strStartID);
                            strEndID = DbPath.GetID10(strEndID);

                            strWhere = " WHERE @idMin <=id and id<= @idMax and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";

                            SqlParameter temp = new SqlParameter("@idMin", SqlDbType.NVarChar);
                            temp.Value = strStartID;
                            aSqlParameter.Add(temp);

                            temp = new SqlParameter("@idMax", SqlDbType.NVarChar);
                            temp.Value = strEndID;
                            aSqlParameter.Add(temp);
                        }
                        else
                        {
                            string strOperator;
                            string strRealText;
                            StringUtil.GetPartCondition(searchItem.Word,
                                out strOperator,
                                out strRealText);

                            strRealText = DbPath.GetID10(strRealText);
                            strWhere = " WHERE id " + strOperator + " @id and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";

                            SqlParameter temp = new SqlParameter("@id", SqlDbType.NVarChar);
                            temp.Value = strRealText;
                            aSqlParameter.Add(temp);
                        }
                    }
                    else
                    {
                        searchItem.Word = DbPath.GetID10(searchItem.Word);
                        strWhere = " WHERE id " + searchItem.Relation + " @id and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";

                        SqlParameter temp = new SqlParameter("@id", SqlDbType.NVarChar);
                        temp.Value = searchItem.Word;
                        aSqlParameter.Add(temp);
                    }
                }

                string strTop = "";
                if (searchItem.MaxCount != -1)  // ֻ����ָ��������
                    strTop = " TOP " + Convert.ToString(searchItem.MaxCount) + " ";

                string strOrderBy = "";
                if (searchItem.IdOrder != "")
                    strOrderBy = "ORDER BY id " + searchItem.IdOrder + " ";

                string strCommand = "use " + this.m_strSqlDbName
                    + " SELECT "
                    + " DISTINCT "
                    + strTop
                    + " id "
                    + " FROM records "
                    + strWhere
                    + " " + strOrderBy + "\n";

                strCommand += " use master " + "\n";

                SqlCommand command = new SqlCommand(strCommand, connection);
                command.CommandTimeout = 20 * 60;  // �Ѽ���ʱ����
                foreach (SqlParameter sqlParameter in aSqlParameter)
                {
                    command.Parameters.Add(sqlParameter);
                }

                DatabaseCommandTask task =
                    new DatabaseCommandTask(command);
                try
                {
                    Thread t1 = new Thread(new ThreadStart(task.ThreadMain));
                    t1.Start();
                    bool bRet;
                    while (true)
                    {
                        if (isConnected != null)  //ֻ�ǲ��ټ�����
                        {
                            if (isConnected() == false)
                            {
                                strError = "�û��ж�";
                                return -1;
                            }
                        }
                        bRet = task.m_event.WaitOne(100, false);  //millisecondsTimeout
                        if (bRet == true)
                            break;
                    }
                    if (task.bError == true)
                    {
                        strError = task.ErrorString;
                        return -1;
                    }

                    if (task.DataReader == null)
                        return 0;

                    if (task.DataReader.HasRows == false)
                    {
                        return 0;
                    }


                    int nLoopCount = 0;
                    while (task.DataReader.Read())
                    {
                        if (nLoopCount % 10000 == 0)
                        {
                            if (isConnected != null)
                            {
                                if (isConnected() == false)
                                {
                                    strError = "�û��ж�";
                                    return -1;
                                }
                            }
                        }

                        string strID = ((string)task.DataReader[0]);
                        if (strID.Length != 10)
                        {
                            strError = "������г����˳��Ȳ���10λ�ļ�¼�ţ�������";
                            return -1;
                        }


                        string strId = this.FullID + "/" + strID;   //��¼·����ʽ����ID/��¼��
                        resultSet.Add(new DpRecord(strId));

                        nLoopCount++;

                        Thread.Sleep(0);
                    }
                }
                finally
                {
                    if (task != null && task.DataReader != null)
                        task.DataReader.Close();
                }

            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Errors is SqlErrorCollection)
                    strError = "���ݿ�'" + this.GetCaption("zh") + "'��δ��ʼ����";
                else
                    strError = sqlEx.Message;
                return -1;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return -1;
            }
            finally // ����
            {
                connection.Close();
            }
            return 0;
        }


        // �õ�����������˽�к�������SearchByUnion()������
        // ���ܻ��׳����쳣:NoMatchException(������ʽ����������)
        // parameters:
        //      searchItem              SearchItem����
        //      nodeConvertQueryString  �ַ����ͼ����ʵĴ�����Ϣ�ڵ�
        //      nodeConvertQueryNumber  ��ֵ�ͼ����ʵĴ�����Ϣ�ڵ�
        //      strPostfix              Sql����������ƺ�׺���Ա�����������һ��ʱ����
        //      aParameter              ��������
        //      strKeyCondition         out����������Sql����ʽ��������
        //      strError                out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // �ߣ�����ȫ
        // ???�ú����׳��쳣�Ĵ���̫˳
        private int GetKeyCondition(SearchItem searchItem,
            XmlNode nodeConvertQueryString,
            XmlNode nodeConvertQueryNumber,
            string strPostfix,
            ref List<SqlParameter> aSqlParameter,
            out string strKeyCondition,
            out string strError)
        {
            strKeyCondition = "";
            strError = "";

            //���������Ƿ���ì�ܣ��ú������ܻ��׳�NoMatchException�쳣
            QueryUtil.VerifyRelation(ref searchItem.Match,
                ref searchItem.Relation,
                ref searchItem.DataType);


            int nRet = 0;
            KeysCfg keysCfg = null;
            nRet = this.GetKeysCfg(out keysCfg,
                out strError);
            if (nRet == -1)
                return -1;


            //3.�����������ͣ��Լ����ʽ��мӹ�
            string strKeyValue = searchItem.Word.Trim();
            if (searchItem.DataType == "string")    //�ַ����͵��ַ������ã��Լ����ʽ��мӹ�
            {
                if (nodeConvertQueryString != null && keysCfg != null)
                {
                    List<string> keys = null;
                    nRet = keysCfg.ConvertKeyWithStringNode(
                        null,//dataDom
                        strKeyValue,
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
                    strKeyValue = keys[0];
                }
            }
            else if (searchItem.DataType == "number")   //�����͵����ָ�ʽ�����ã��Լ����ʽ��мӹ�
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

            string strParameterName;
            //4.����match��ֵ���ֱ�õ���ͬ�ļ������ʽ
            if (searchItem.Match == "left"
                || searchItem.Match == "")  //���strMatchΪ�գ���"��һ��"
            {
                //��ʵһ��ʼ���Ѿ�����������Ƿ�ì�ܣ������ì���׳����죬�����ظ�����޺������ϸ�
                if (searchItem.DataType != "string")
                {
                    NoMatchException ex =
                        new NoMatchException("��ƥ�䷽ʽֵΪleft���ʱ������������ֵ" + searchItem.DataType + "ì�ܣ���������Ӧ��Ϊstring");
                    throw (ex);
                }
                strParameterName = "@keyValue" + strPostfix;
                strKeyCondition = "keystring LIKE "
                    + strParameterName + " ";

                SqlParameter temp = new SqlParameter(strParameterName, SqlDbType.NVarChar);
                temp.Value = strKeyValue + "%";
                aSqlParameter.Add(temp);
            }
            else if (searchItem.Match == "middle")
            {
                //��ʵһ��ʼ���Ѿ�����������Ƿ�ì�ܣ������ì���׳����죬�����ظ�����޺������ϸ�
                if (searchItem.DataType != "string")
                {
                    NoMatchException ex = new NoMatchException("��ƥ�䷽ʽֵΪmiddle���ʱ������������ֵ" + searchItem.DataType + "ì�ܣ���������Ӧ��Ϊstring");
                    throw (ex);
                }
                strParameterName = "@keyValue" + strPostfix;
                strKeyCondition = "keystring LIKE "
                    + strParameterName + " "; //N'%" + strKeyValue + "'";

                SqlParameter temp = new SqlParameter(strParameterName, SqlDbType.NVarChar);
                temp.Value = "%" + strKeyValue + "%";
                aSqlParameter.Add(temp);
            }
            else if (searchItem.Match == "right")
            {
                //��ʵһ��ʼ���Ѿ�����������Ƿ�ì�ܣ������ì���׳����죬�����ظ�����޺������ϸ�
                if (searchItem.DataType != "string")
                {
                    NoMatchException ex = new NoMatchException("��ƥ�䷽ʽֵΪleft���ʱ������������ֵ" + searchItem.DataType + "ì�ܣ���������Ӧ��Ϊstring");
                    throw (ex);
                }
                strParameterName = "@keyValue" + strPostfix;
                strKeyCondition = "keystring LIKE "
                    + strParameterName + " "; //N'%" + strKeyValue + "'";

                SqlParameter temp = new SqlParameter(strParameterName, SqlDbType.NVarChar);
                temp.Value = "%" + strKeyValue;
                aSqlParameter.Add(temp);
            }
            else if (searchItem.Match == "exact") //�ȿ�match���ٿ�relation,���dataType
            {
                //�Ӵ��м�ȡ,�ϸ��ӣ�ע��
                if (searchItem.Relation == "draw")
                {
                    int nPosition;
                    nPosition = searchItem.Word.IndexOf("-");
                    //Ӧ��"-"��
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
                                nRet = keysCfg.ConvertKeyWithStringNode(
                                    null,//dataDom
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
                                nRet = keysCfg.ConvertKeyWithStringNode(
                                    null,//dataDom
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
                            string strParameterMinName = "@keyValueMin" + strPostfix;
                            string strParameterManName = "@keyValueMan" + strPostfix;

                            strKeyCondition = " " + strParameterMinName
                                + " <=keystring and keystring<= "
                                + strParameterManName + " ";

                            SqlParameter temp = new SqlParameter(strParameterMinName, SqlDbType.NVarChar);
                            temp.Value = strStartText;
                            aSqlParameter.Add(temp);

                            temp = new SqlParameter(strParameterManName, SqlDbType.NVarChar);
                            temp.Value = strEndText;
                            aSqlParameter.Add(temp);
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
                            strKeyCondition = strStartText
                                + " <= keystringnum and keystringnum <= "
                                + strEndText +
                                " and keystringnum <> -1";
                        }
                    }
                    else
                    {
                        string strOperator;
                        string strRealText;

                        //�������û�а�����ϵ������=����
                        StringUtil.GetPartCondition(searchItem.Word,
                            out strOperator,
                            out strRealText);

                        if (strOperator == "!=")
                            strOperator = "<>";

                        if (searchItem.DataType == "string")
                        {
                            if (nodeConvertQueryString != null
                                && keysCfg != null)
                            {
                                List<string> keys = null;
                                nRet = keysCfg.ConvertKeyWithStringNode(
                                    null,//dataDom
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

                            strParameterName = "@keyValue" + strPostfix;
                            strKeyCondition = " keystring"
                                + strOperator
                                + " " + strParameterName + " ";

                            SqlParameter temp = new SqlParameter(strParameterName, SqlDbType.NVarChar);
                            temp.Value = strRealText;
                            aSqlParameter.Add(temp);
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

                            strKeyCondition = " keystringnum"
                                + strOperator
                                + strRealText
                                + " and keystringnum <> -1";
                        }
                    }
                }
                else   //��ͨ�Ĺ�ϵ������
                {
                    //����ϵ������Ϊ��Ϊ����������
                    if (searchItem.Relation == "")
                        searchItem.Relation = "=";
                    if (searchItem.Relation == "!=")
                        searchItem.Relation = "<>";

                    if (searchItem.DataType == "string")
                    {
                        strParameterName = "@keyValue" + strPostfix;

                        strKeyCondition = " keystring "
                            + searchItem.Relation
                            + " " + strParameterName + " ";

                        SqlParameter temp = new SqlParameter(strParameterName, SqlDbType.NVarChar);
                        temp.Value = strKeyValue;
                        aSqlParameter.Add(temp);
                    }
                    else if (searchItem.DataType == "number")
                    {
                        strKeyCondition = " keystringnum "
                            + searchItem.Relation
                            + strKeyValue
                            + " and keystringnum <> -1";
                    }
                }
            }

            return 0;
        }

        // ����
        // parameters:
        //      searchItem  SearchItem���󣬴�ż����ʵ���Ϣ
        //      isConnected ���Ӷ���
        //      resultSet   ��������󣬴�����м�¼
        //      strLang     ���԰汾��
        // return:
        //		-1	����
        //		0	�ɹ�
        internal override int SearchByUnion(SearchItem searchItem,
            Delegate_isConnected isConnected,
            DpResultSet resultSet,
            int nWarningLevel,
            out string strError,
            out string strWarning)
        {
            strError = "";
            strWarning = "";

            //**********�����ݿ�Ӷ���**************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK_SQLDATABASE
			this.container.WriteDebugInfo("SearchByUnion()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                bool bHasID;
                List<TableInfo> aTableInfo = null;
                int nRet = this.TableNames2aTableInfo(searchItem.TargetTables,
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

                // ��sql����˵,ͨ��ID�����󣬼�¼������ȥ��
                if (aTableInfo == null || aTableInfo.Count == 0)
                    return 0;


                string strCommand = "";

                // Sql�����������
                List<SqlParameter> aSqlParameter = new List<SqlParameter>();

                string strSelectKeystring = "";
                if (searchItem.KeyOrder != "")
                {
                    if (aTableInfo.Count > 1)
                        strSelectKeystring = ",keystring";
                }

                // ѭ��ÿһ������;��
                for (int i = 0; i < aTableInfo.Count; i++)
                {
                    TableInfo tableInfo = aTableInfo[i];

                    // �������ĺ�׺
                    string strPostfix = Convert.ToString(i);

                    string strConditionAboutKey = "";
                    try
                    {
                        nRet = GetKeyCondition(
                            searchItem,
                            tableInfo.nodeConvertQueryString,
                            tableInfo.nodeConvertQueryNumber,
                            strPostfix,
                            ref aSqlParameter,
                            out strConditionAboutKey,
                            out strError);
                        if (nRet == -1)
                            return -1;
                    }
                    catch (NoMatchException ex)
                    {
                        strWarning = ex.Message;
                        strError = strWarning;
                        return -1;
                    }

                    // ���������һ�����������ÿ��;����������������
                    string strTop = "";
                    if (searchItem.MaxCount != -1)  //���Ƶ������
                        strTop = " TOP " + Convert.ToString(searchItem.MaxCount) + " ";

                    string strWhere = "";
                    if (strConditionAboutKey != "")
                        strWhere = " WHERE " + strConditionAboutKey;

                    string strOneCommand = "";
                    if (i == 0)// ��һ����
                    {
                        strOneCommand = "use " + this.m_strSqlDbName + " "
                            + " SELECT "
                            + " DISTINCT "
                            + strTop
                            + " idstring" + strSelectKeystring + " "
                            + " FROM " + tableInfo.SqlTableName + " "
                            + strWhere;
                    }
                    else
                    {
                        strOneCommand = " union SELECT "
                            + " DISTINCT "
                            + strTop
                            + " idstring" + strSelectKeystring + " "  //DISTINCT ȥ��
                            + " FROM " + tableInfo.SqlTableName + " "
                            + strWhere;
                    }
                    strCommand += strOneCommand;
                }

                string strOrderBy = "";
                if (searchItem.OrderBy != "")
                    strOrderBy = "ORDER BY " + searchItem.OrderBy + " ";

                strCommand += strOrderBy;
                strCommand += " use master " + "\n";

                if (aSqlParameter == null)
                {
                    strError = "һ������Ҳû�ǲ����ܵ����";
                    return -1;
                }

                SqlCommand command = null;
                SqlConnection connection = new SqlConnection(this.m_strConnString);
                connection.Open();
                try
                {
                    command = new SqlCommand(strCommand,
                        connection);
                    foreach (SqlParameter sqlParameter in aSqlParameter)
                    {
                        command.Parameters.Add(sqlParameter);
                    }
                    command.CommandTimeout = 20 * 60;  // �Ѽ���ʱ����
                    // �����̴߳���
                    DatabaseCommandTask task = new DatabaseCommandTask(command);
                    try
                    {
                        if (task == null)
                        {
                            strError = "testΪnull";
                            return -1;
                        }
                        Thread t1 = new Thread(new ThreadStart(task.ThreadMain));
                        t1.Start();
                        bool bRet;
                        while (true)
                        {
                            if (isConnected != null)
                            {
                                if (isConnected() == false)
                                {
                                    strError = "�û��ж�";
                                    return -1;
                                }
                            }
                            bRet = task.m_event.WaitOne(100, false);  //1/10�뿴һ��
                            if (bRet == true)
                                break;
                        }

                        if (task.DataReader == null
                            || task.DataReader.HasRows == false)
                        {
                            return 0;
                        }

                        int nGetedCount = 0;
                        while (task.DataReader.Read())
                        {
                            if (isConnected != null
                                && (nGetedCount % 10000) == 0)
                            {
                                if (isConnected() == false)
                                {
                                    strError = "�û��ж�";
                                    return -1;
                                }
                            }

                            string strId = this.FullID + "/" + (string)task.DataReader[0]; // ��¼��ʽΪ����id/��¼��
                            resultSet.Add(new DpRecord(strId));

                            nGetedCount++;

                            // �����������
                            if (searchItem.MaxCount != -1
                                && nGetedCount >= searchItem.MaxCount)
                                break;

                            Thread.Sleep(0);
                        }
                    }
                    finally
                    {
                        if (task.DataReader != null)
                            task.DataReader.Close();
                    }

                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Errors is SqlErrorCollection)
                        strError = "���ݿ�'" + this.GetCaption("zh") + "'��δ��ʼ����";
                    else
                        strError = sqlEx.Message;
                    return -1;
                }
                catch (Exception ex)
                {
                    strError = ex.Message;
                    return -1;
                }
                finally // ����
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return -1;
            }
            finally
            {
                //*****************�����ݿ�����***************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("SearchByUnion()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }

            return 0;
        }



        // ����strStyle���,�õ���͵ļ�¼��
        // prev:ǰһ��,next:��һ��,���strID == ? ��prevΪ��һ��,nextΪ���һ��
        // ���������prev��next���ܵ��˺���
        // parameter:
        //		connection	        ���Ӷ���
        //		strCurrentRecordID	��ǰ��¼ID
        //		strStyle	        ���
        //      strOutputRecordID   out�����������ҵ��ļ�¼��
        //      strError            out���������س�����Ϣ
        // return:
        //		-1  ����
        //      0   δ�ҵ�
        //      1   �ҵ�
        // �ߣ�����ȫ
        private int GetRecordID(SqlConnection connection,
            string strCurrentRecordID,
            string strStyle,
            out string strOutputRecordID,
            out string strError)
        {
            strOutputRecordID = "";
            strError = "";

            Debug.Assert(connection != null, "GetRecordID()���ô���connection����ֵ����Ϊnull��");

            if ((StringUtil.IsInList("prev", strStyle) == false)
                && (StringUtil.IsInList("next", strStyle) == false))
            {
                Debug.Assert(false, "GetRecordID()���ô������strStyle����������prev��nextֵ��Ӧ�ߵ����");
                throw new Exception("GetRecordID()���ô������strStyle����������prev��nextֵ��Ӧ�ߵ����");
            }

            strCurrentRecordID = DbPath.GetID10(strCurrentRecordID);

            string strWhere = "";
            string strOrder = "";
            if ((StringUtil.IsInList("prev", strStyle) == true))
            {
                if (DbPath.GetCompressedID(strCurrentRecordID) == "-1")
                {
                    strWhere = " where id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    strOrder = " ORDER BY id DESC ";
                }
                else if (StringUtil.IsInList("myself", strStyle) == true)
                {
                    strWhere = " where id<='" + strCurrentRecordID + "' and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    strOrder = " ORDER BY id DESC ";
                }
                else
                {
                    strWhere = " where id<'" + strCurrentRecordID + "' and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    strOrder = " ORDER BY id DESC ";
                }
            }
            else if (StringUtil.IsInList("next", strStyle) == true)
            {
                if (DbPath.GetCompressedID(strCurrentRecordID) == "-1")
                {
                    strWhere = " where id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    strOrder = " ORDER BY id ASC ";
                }
                else if (StringUtil.IsInList("myself", strStyle) == true)
                {
                    strWhere = " where id>='" + strCurrentRecordID + "' and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    strOrder = " ORDER BY id ASC ";
                }
                else
                {
                    strWhere = " where id>'" + strCurrentRecordID + "' and id like N'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' ";
                    strOrder = " ORDER BY id ASC ";
                }
            }
            string strCommand = "use " + this.m_strSqlDbName + " "
                + " SELECT Top 1 id "
                + " FROM records "
                + strWhere
                + strOrder;
            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);

            SqlDataReader dr =
                command.ExecuteReader(CommandBehavior.SingleResult);
            try
            {
                if (dr == null || dr.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    dr.Read();
                    strOutputRecordID = (string)dr[0];
                    return 1;
                }
            }
            finally
            {
                dr.Close();
            }
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

            SqlConnection connection = new SqlConnection(this.m_strConnString);
            connection.Open();
            try
            {
                // return:
                //		-1  ����
                //      0   δ�ҵ�
                //      1   �ҵ�
                return this.GetRecordID(connection,
                    strCurrentRecordID,
                    strStyle,
                    out strOutputRecordID,
                    out strError);
            }
            catch (SqlException ex)
            {
                if (ex.Errors is SqlErrorCollection)
                    return 0;

                strError = ex.Message;
                return -1;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return -1;
            }
            finally // ����
            {
                connection.Close();
            }
        }

        // ��ָ����Χ��Xml
        // parameter:
        //		strRecordID			��¼ID
        //		strXPath			������λ�ڵ��xpath
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
        public override long GetXml(string strRecordID,
            string strXPath,
            int nStart,
            int nLength,
            int nMaxLength,
            string strStyle,
            out byte[] destBuffer,
            out string strMetadata,
            out string strOutputRecordID,
            out byte[] outputTimestamp,
            bool bCheckAccount,
            out int nAdditionError,
            out string strError)
        {
            destBuffer = null;
            strMetadata = "";
            strOutputRecordID = "";
            outputTimestamp = null;
            strError = "";
            nAdditionError = 0;

            int nRet = 0;

            int nNotFoundSubRes = 0;    // �¼�û���ҵ�����Դ����
            string strNotFoundSubResIds = "";

            // ���ID
            // return:
            //      -1  ����
            //      0   �ɹ�
            nRet = DatabaseUtil.CheckAndGet10RecordID(ref strRecordID,
                out strError);
            if (nRet == -1)
                return -1;

            // ����ʽȥ�հ�
            strStyle = strStyle.Trim();

            // ȡ��ʵ�ʵļ�¼��
            if (StringUtil.IsInList("prev", strStyle) == true
                || StringUtil.IsInList("next", strStyle) == true)
            {
                string strTempOutputID = "";
                SqlConnection connection = new SqlConnection(this.m_strConnString);
                connection.Open();
                try
                {
                    // return:
                    //		-1  ����
                    //      0   δ�ҵ�
                    //      1   �ҵ�
                    nRet = this.GetRecordID(connection,
                        strRecordID,
                        strStyle,
                        out strTempOutputID,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }
                finally
                {
                    connection.Close();
                }
                if (nRet == 0 || strTempOutputID == "")
                {
                    strError = "δ�ҵ���¼ID '" + strRecordID + "' �ķ��Ϊ'" + strStyle + "'�ļ�¼";
                    return -4;
                }
                strRecordID = strTempOutputID;

                // �ٴμ��һ�·��ص�ID
                // return:
                //      -1  ����
                //      0   �ɹ�
                nRet = DatabaseUtil.CheckAndGet10RecordID(ref strRecordID,
                    out strError);
                if (nRet == -1)
                    return -1;
            }

            // ���ݷ��Ҫ�󣬷�����Դ·��
            if (StringUtil.IsInList("outputpath", strStyle) == true)
            {
                strOutputRecordID = DbPath.GetCompressedID(strRecordID);
            }


            // ���ʻ��⿪�ĺ��ţ����ڸ����ʻ�,RefreshUser�ǻ��WriteXml()�Ǽ����ĺ���
            // �����ڿ�ͷ��һ��connection����
            if (bCheckAccount == true &&
                StringUtil.IsInList("account", this.TypeSafety) == true)
            {
                // ���Ҫ��ü�¼�������˻����¼��������
                // UserCollection�У��ǾͰ���ص�User��¼
                // ��������ݿ⣬�Ա��Ժ�����ݿ�����ȡ��
                // �����ش��ڴ�����ȡ��
                string strAccountPath = this.FullID + "/" + strRecordID;

                // return:
                //		-1  ����
                //      -4  ��¼������
                //		0   �ɹ�
                nRet = this.container.UserColl.SaveUserIfNeed(
                    strAccountPath,
                    out strError);
                if (nRet <= -1)
                    return nRet;
            }


            //********����Ӷ���**************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK_SQLDATABASE
			this.container.WriteDebugInfo("GetXml()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                //*******************�Լ�¼�Ӷ���************************
                m_recordLockColl.LockForRead(strRecordID, m_nTimeOut);

#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("GetXml()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼�Ӷ�����");
#endif
                try //��
                {

                    SqlConnection connection = new SqlConnection(this.m_strConnString);
                    connection.Open();
                    try  //����
                    {
                        // return:
                        //		-1  ����
                        //      0   ������
                        //      1   ����
                        nRet = this.RecordIsExist(connection,
                            strRecordID,
                            out strError);
                        if (nRet == -1)
                            return -1;

                        if (nRet == 0)
                        {
                            strError = "��¼'" + strRecordID + "'�ڿ��в�����";
                            return -4;
                        }

                        byte[] baWholeXml = null;
                        byte[] baPreamble = null;
                        string strXml = null;
                        XmlDocument dom = null;
                        // ����ԴԪ���ݵ������Ҫ�������xml���ݵ�
                        if (StringUtil.IsInList("withresmetadata", strStyle) == true)
                        {
                            // ������һ���򵥵ĺ�����һ��
                            // return:
                            //		-1  ����
                            //		-4  ��¼������
                            //		>=0 ��Դ�ܳ���
                            nRet = this.GetImage(connection,
                                strRecordID,
                                "data",
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

                            strXml = DatabaseUtil.ByteArrayToString(baWholeXml,
                                out baPreamble);

                            if (strXml != "")
                            {
                                dom = new XmlDocument();
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


                                // �ҵ����е�dprms:fileԪ��
                                XmlNamespaceManager nsmgr = new XmlNamespaceManager(dom.NameTable);
                                nsmgr.AddNamespace("dprms", DpNs.dprms);
                                XmlNodeList fileList = dom.DocumentElement.SelectNodes("//dprms:file", nsmgr);
                                foreach (XmlNode fileNode in fileList)
                                {
                                    string strObjectID = DomUtil.GetAttr(fileNode, "id");
                                    if (strObjectID == "")
                                        continue;

                                    byte[] baObjectDestBuffer;
                                    string strObjectMetadata;
                                    byte[] baObjectOutputTimestamp;

                                    string strObjectFullID = strRecordID + "_" + strObjectID;
                                    // return:
                                    //		-1  ����
                                    //		-4  ��¼������
                                    //		>=0 ��Դ�ܳ���
                                    nRet = this.GetImage(connection,
                                        strObjectFullID,
                                        "data",
                                        nStart,
                                        nLength,
                                        nMaxLength,
                                        "metadata,timestamp",//strStyle,
                                        out baObjectDestBuffer,
                                        out strObjectMetadata,
                                        out baObjectOutputTimestamp,
                                        out strError);
                                    if (nRet <= -1)
                                    {
                                        // ��Դ��¼������
                                        if (nRet == -4)
                                        {
                                            nNotFoundSubRes++;

                                            if (strNotFoundSubResIds != "")
                                                strNotFoundSubResIds += ",";
                                            strNotFoundSubResIds += strObjectID;
                                        }
                                    }

                                    // ����metadata
                                    if (strObjectMetadata != "")
                                    {
                                        Hashtable values = rmsUtil.ParseMedaDataXml(strObjectMetadata,
                                            out strError);
                                        if (values == null)
                                            return -1;

                                        string strObjectTimestamp = ByteArray.GetHexTimeStampString(baObjectOutputTimestamp);

                                        DomUtil.SetAttr(fileNode, "__mime", (string)values["mimetype"]);
                                        DomUtil.SetAttr(fileNode, "__localpath", (string)values["localpath"]);
                                        DomUtil.SetAttr(fileNode, "__size", (string)values["size"]);

                                        DomUtil.SetAttr(fileNode, "__timestamp", strObjectTimestamp);
                                    }
                                }
                            } // end if (strXml != "")

                        } // if (StringUtil.IsInList("withresmetadata", strStyle) == true)

                        // ͨ��xpath��Ƭ�ϵ����
                        if (strXPath != null && strXPath != "")
                        {
                            if (baWholeXml == null)
                            {
                                // return:
                                //		-1  ����
                                //		-4  ��¼������
                                //		>=0 ��Դ�ܳ���
                                nRet = this.GetImage(connection,
                                    strRecordID,
                                    "data",
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

                                if (baWholeXml == null)
                                {
                                    strError = "����Ȼʹ����xpath����δȡ�����ݣ�����������style�����ȷ����ǰstyle��ֵΪ'" + strStyle + "'��";
                                    return -1;

                                }

                                strXml = DatabaseUtil.ByteArrayToString(baWholeXml,
                                    out baPreamble);

                                if (strXml != "")
                                {

                                    dom = new XmlDocument();
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
                                }
                            } 

                            if (dom != null)
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

                                if (strLocateXPath == "")
                                {
                                    strError = "xpath���ʽ�е�locate��������Ϊ��ֵ";
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
                            }
                            else
                            {
                                destBuffer = new byte[0];
                            }

                            return 0;
                        } // end if (strXPath != null && strXPath != "")

                        if (dom != null)
                        {
                            // ����ԴԪ���ݵ������Ҫ�������xml���ݵ�
                            if (StringUtil.IsInList("withresmetadata", strStyle) == true)
                            {
                                // ʹ��XmlTextWriter�����utf8�ı��뷽ʽ
                                MemoryStream ms = new MemoryStream();
                                XmlTextWriter textWriter = new XmlTextWriter(ms, Encoding.UTF8);
                                dom.Save(textWriter);
                                //dom.Save(ms);

                                int nRealLength;
                                // return:
                                //		-1  ����
                                //		0   �ɹ�
                                nRet = DatabaseUtil.GetRealLength(nStart,
                                    nLength,
                                    (int)ms.Length,
                                    nMaxLength,
                                    out nRealLength,
                                    out strError);
                                if (nRet == -1)
                                    return -1;

                                destBuffer = new byte[nRealLength];

                                // ��Ԫ�ص���Ϣ����ܳ���
                                long nWithMetedataTotalLength = ms.Length;

                                ms.Seek(nStart, SeekOrigin.Begin);
                                ms.Read(destBuffer,
                                    0,
                                    destBuffer.Length);
                                ms.Close();

                                if (nNotFoundSubRes > 0)
                                {
                                    strError = "��¼" + strRecordID + "��idΪ " + strNotFoundSubResIds + " ���¼���Դ��¼������";
                                    nAdditionError = -50; // ��һ�������¼���Դ��¼������
                                }

                                return nWithMetedataTotalLength;
                            }
                        } // end if (dom != null)

                        // ��ʹ��xpath�����
                        // return:
                        //		-1  ����
                        //		-4  ��¼������
                        //		>=0 ��Դ�ܳ���
                        nRet = this.GetImage(connection,
                            strRecordID,
                            "data",
                            nStart,
                            nLength,
                            nMaxLength,
                            strStyle,
                            out destBuffer,
                            out strMetadata,
                            out outputTimestamp,
                            out strError);

                        if (nRet >= 0 && nNotFoundSubRes > 1)
                        {
                            strError = "��¼" + strRecordID + "��idΪ " + strNotFoundSubResIds + " ���¼���Դ��¼������";
                            nAdditionError = -50; // ��һ�������¼���Դ��¼������
                        }

                        return nRet;
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Errors is SqlErrorCollection)
                            strError = "���ݿ�'" + this.GetCaption("zh") + "'��δ��ʼ����";
                        else
                            strError = "ȡ��¼'" + strRecordID + "'�����ˣ�ԭ��:" + sqlEx.Message;
                        return -1;
                    }
                    catch (Exception ex)
                    {
                        strError = "ȡ��¼'" + strRecordID + "'�����ˣ�ԭ��:" + ex.Message;
                        return -1;
                    }
                    finally //����
                    {
                        connection.Close();
                    }
                }
                finally //��
                {

                    //*********�Լ�¼�����******
                    m_recordLockColl.UnlockForRead(strRecordID);
#if DEBUG_LOCK_SQLDATABASE
					this.container.WriteDebugInfo("GetXml()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼�������");
#endif
                }
            }
            catch (Exception ex)
            {
                strError = "ȡ��¼'" + strRecordID + "'�����ˣ�ԭ��:" + ex.Message;
                return -1;
            }
            finally
            {
                //***********�����ݿ�����*****************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("GetXml()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif

            }
        }


        // �õ�xml����
        // ��:��ȫ��,���ⲿ��
        // return:
        //      -1  ����
        //      -4  ��¼������
        //      0   ��ȷ
        public override int GetXmlData(string strID,
            out string strXml,
            out string strError)
        {
            strXml = "";
            strError = "";

            strID = DbPath.GetID10(strID);

            SqlConnection connection = new SqlConnection(this.m_strConnString);
            connection.Open();
            try
            {
                // return:
                //      -1  ����
                //      -4  ��¼������
                //      0   ��ȷ
                return this.GetXmlString(connection,
                    strID,
                    out strXml,
                    out strError);
            }
            finally
            {
                connection.Close();
            }
        }


        // ȡxml���ݵ��ַ���,��װGetXmlData()
        // ��:����ȫ
        // return:
        //      -1  ����
        //      -4  ��¼������
        //      0   ��ȷ
        private int GetXmlString(SqlConnection connection,
            string strID,
            out string strXml,
            out string strError)
        {
            byte[] baPreamble;
            // return:
            //      -1  ����
            //      -4  ��¼������
            //      0   ��ȷ
            return this.GetXmlData(connection,
                strID,
                "data",
                out strXml,
                out baPreamble,
                out strError);
        }

        // �õ�xml�ַ���,��װGetImage()
        // ��: ����ȫ
        // return:
        //      -1  ����
        //      -4  ��¼������
        //      0   ��ȷ
        private int GetXmlData(SqlConnection connection,
            string strID,
            string strFieldName,
            out string strXml,
            out byte[] baPreamble,
            out string strError)
        {
            baPreamble = new byte[0];
            strXml = "";
            strError = "";

            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            byte[] newXmlBuffer;
            byte[] outputTimestamp;
            string strMetadata;
            // return:
            //		-1  ����
            //		-4  ��¼������
            //		>=0 ��Դ�ܳ���
            nRet = this.GetImage(connection,
                strID,
                strFieldName,
                0,
                -1,
                -1,
                "data",
                out newXmlBuffer,
                out strMetadata,
                out outputTimestamp,
                out strError);
            if (nRet <= -1)
                return nRet;

            strXml = DatabaseUtil.ByteArrayToString(newXmlBuffer,
                out baPreamble);
            return 0;
        }

        // ��ָ����Χ����Դ
        // parameter:
        //		strID       ��¼ID
        //		nStart      ��ʼλ��
        //		nLength     ���� -1:��ʼ������
        //		destBuffer  out�����������ֽ�����
        //		timestamp   out����������ʱ���
        //		strError    out���������س�����Ϣ
        // return:
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
            out byte[] outputTimestamp,
            out string strError)
        {
            destBuffer = null;
            outputTimestamp = null;
            strMetadata = "";
            strError = "";

            strRecordID = DbPath.GetID10(strRecordID);
            //********�����ݿ�Ӷ���**************
            m_lock.AcquireReaderLock(m_nTimeOut);

#if DEBUG_LOCK_SQLDATABASE
			this.container.WriteDebugInfo("GetObject()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                //*******************�Լ�¼�Ӷ���************************
                m_recordLockColl.LockForRead(strRecordID, m_nTimeOut);
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("GetObject()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼�Ӷ�����");
#endif
                try  // ��¼��
                {

                    SqlConnection connection = new SqlConnection(this.m_strConnString);
                    connection.Open();
                    try // ����
                    {

                        string strObjectFullID = strRecordID + "_" + strObjectID;
                        // return:
                        //		-1  ����
                        //		-4  ��¼������
                        //		>=0 ��Դ�ܳ���
                        return this.GetImage(connection,
                            strObjectFullID,
                            "data",
                            nStart,
                            nLength,
                            nMaxLength,
                            strStyle,
                            out destBuffer,
                            out strMetadata,
                            out outputTimestamp,
                            out strError);
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Errors is SqlErrorCollection)
                            strError = "���ݿ�'" + this.GetCaption("zh") + "'��δ��ʼ����";
                        else
                            strError = sqlEx.Message;
                        return -1;
                    }
                    catch (Exception ex)
                    {
                        strError = ex.Message;
                        return -1;
                    }
                    finally // ����
                    {
                        connection.Close();
                    }
                }
                finally // ��¼��
                {
                    //*************�Լ�¼�����***********
                    m_recordLockColl.UnlockForRead(strRecordID);
#if DEBUG_LOCK_SQLDATABASE
					this.container.WriteDebugInfo("GetObject()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼�������");
#endif
                }
            }
            finally //����
            {
                //******�����ݿ�����*********
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("GetObject()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }
        }

        // ��ָ����Χ����Դ
        // parameter:
        //		strID       ��¼ID
        //		nStart      ��ʼλ��
        //		nLength     ���� -1:��ʼ������
        //		nMaxLength  ��󳤶�,��Ϊ-1ʱ,��ʾ����
        //		destBuffer  out�����������ֽ�����
        //		timestamp   out����������ʱ���
        //		strError    out���������س�����Ϣ
        // return:
        //		-1  ����
        //		-4  ��¼������
        //		>=0 ��Դ�ܳ���
        private int GetImage(SqlConnection connection,
            string strID,
            string strImageFieldName,
            int nStart,
            int nLength1,
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

            // ������Ӷ���
            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            strID = DbPath.GetID10(strID);

            int nTotalLength = 0;


            // 1.textPtr
            string strTextPtrComm = "";
            if (StringUtil.IsInList("data", strStyle) == true)
            {
                strTextPtrComm = " @textPtr=TEXTPTR(" + strImageFieldName + ")";
            }

            // 2.length,һ��Ҫ��
            string strLengthComm = "";
            strLengthComm = " @Length=DataLength(" + strImageFieldName + ")";

            // 3.timestamp
            string strTimestampComm = "";
            if (StringUtil.IsInList("timestamp", strStyle) == true)
            {
                strTimestampComm = " @dptimestamp=dptimestamp";
            }
            // 4.metadata
            string strMetadataComm = "";
            if (StringUtil.IsInList("metadata", strStyle) == true)
            {
                strMetadataComm = " @metadata=metadata";
            }
            // 5.range
            string strRangeComm = "";
            if (StringUtil.IsInList("range", strStyle) == true)
            {
                strRangeComm = " @range=range";
            }

            // ���������ַ���
            string strPartComm = "";

            if (strTextPtrComm != "")
            {
                if (strPartComm != "")
                    strPartComm += ",";
                strPartComm += strTextPtrComm;
            }

            if (strLengthComm != "")
            {
                if (strPartComm != "")
                    strPartComm += ",";
                strPartComm += strLengthComm;
            }

            if (strTimestampComm != "")
            {
                if (strPartComm != "")
                    strPartComm += ",";
                strPartComm += strTimestampComm;
            }

            if (strMetadataComm != "")
            {
                if (strPartComm != "")
                    strPartComm += ",";
                strPartComm += strMetadataComm;
            }

            if (strRangeComm != "")
            {
                if (strPartComm != "")
                    strPartComm += ",";
                strPartComm += strRangeComm;
            }

            if (strPartComm != "")
                strPartComm += ",";
            strPartComm += " @testid=id";

            string strCommand = "";
            // DataLength()����int����
            strCommand = "use " + this.m_strSqlDbName + " "
                + " SELECT "
                + strPartComm + " "
                + " FROM records WHERE id=@id";

            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);

            SqlParameter idParam =
                command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            idParam.Value = strID;


            SqlParameter testidParam =
                    command.Parameters.Add("@testid",
                    SqlDbType.NVarChar,
                    255);
            testidParam.Direction = ParameterDirection.Output;

            // 1.textPtr
            SqlParameter textPtrParam = null;
            if (StringUtil.IsInList("data", strStyle) == true)
            {
                textPtrParam =
                    command.Parameters.Add("@textPtr",
                    SqlDbType.VarBinary,
                    16);
                textPtrParam.Direction = ParameterDirection.Output;

            }
            // 2.length,һ��Ҫ����
            SqlParameter lengthParam =
                command.Parameters.Add("@length",
                SqlDbType.Int);
            lengthParam.Direction = ParameterDirection.Output;

            // 3.timestamp
            SqlParameter timestampParam = null;
            if (StringUtil.IsInList("timestamp", strStyle) == true)
            {
                timestampParam =
                    command.Parameters.Add("@dptimestamp",
                    SqlDbType.NVarChar,
                    100);
                timestampParam.Direction = ParameterDirection.Output;
            }
            // 4.metadata
            SqlParameter metadataParam = null;
            if (StringUtil.IsInList("metadata", strStyle) == true)
            {
                metadataParam =
                    command.Parameters.Add("@metadata",
                    SqlDbType.NVarChar,
                    4000);
                metadataParam.Direction = ParameterDirection.Output;

            }
            // 5.range
            SqlParameter rangeParam = null;
            if (StringUtil.IsInList("range", strStyle) == true)
            {
                rangeParam =
                    command.Parameters.Add("@range",
                    SqlDbType.NVarChar,
                    4000);
                rangeParam.Direction = ParameterDirection.Output;
            }



            // ִ������
            command.ExecuteNonQuery();


            if (testidParam == null
                || (testidParam.Value is System.DBNull))
            {
                strError = "��¼'" + strID + "'�ڿ��в�����";
                return -4;
            }


            // 2.length,һ���᷵��
            if (lengthParam != null
                && (!(lengthParam.Value is System.DBNull)))
            {
                nTotalLength = (int)lengthParam.Value;
            }

            // 3.timestamp
            if (StringUtil.IsInList("timestamp", strStyle) == true)
            {
                if (timestampParam != null
                    && (!(timestampParam.Value is System.DBNull)))
                {
                    string strOutputTimestamp = (string)timestampParam.Value;
                    outputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTimestamp);//Encoding.UTF8.GetBytes(strOutputTimestamp);
                }
            }
            // 4.metadata
            if (StringUtil.IsInList("metadata", strStyle) == true)
            {
                if (metadataParam != null
                    && (!(metadataParam.Value is System.DBNull)))
                {
                    strMetadata = (string)metadataParam.Value;
                }
            }
            // 5.range
            if (StringUtil.IsInList("range", strStyle) == true)
            {
                if (rangeParam != null
                    && (!(rangeParam.Value is System.DBNull)))
                {
                    string strRange = (string)rangeParam.Value;
                }
            }


            // 1.textPtr
            byte[] textPtr = null;
            if (StringUtil.IsInList("data", strStyle) == true)
            {
                if (textPtrParam != null
                    && (!(textPtrParam.Value is System.DBNull)))
                {
                    textPtr = (byte[])textPtrParam.Value;
                }
                else
                {
                    destBuffer = new byte[0];
                    return 0;

                    // ����˵��Image�ֶ�Ϊ��

                    //strError = strID + "�ǿռ�¼";
                    //return -3;
                }
            }



            // ��Ҫ��ȡ����ʱ,�Ż�ȡ����
            if (StringUtil.IsInList("data", strStyle) == true)
            {
                if (nLength1 == 0)  // ȡ0����
                {
                    destBuffer = new byte[0];
                    return nTotalLength;    // >= 0
                }

                if (textPtr == null)
                {
                    strError = "textPtrΪnull";
                    return -1;
                }

                int nOutputLength = 0;
                // �õ�ʵ�ʶ��ĳ���
                // return:
                //		-1  ����
                //		0   �ɹ�
                nRet = DatabaseUtil.GetRealLength(nStart,
                    nLength1,
                    nTotalLength,
                    nMaxLength,
                    out nOutputLength,
                    out strError);
                if (nRet == -1)
                    return -1;

                // READTEXT����:
                // text_ptr: ��Ч�ı�ָ�롣text_ptr ������ binary(16)��
                // offset:   ��ʼ��ȡimage����֮ǰ�������ֽ�����ʹ�� text �� image ��������ʱ�����ַ�����ʹ�� ntext ��������ʱ����
                //			 ʹ�� ntext ��������ʱ��offset ���ڿ�ʼ��ȡ����ǰ�������ַ�����
                //			 ʹ�� text �� image ��������ʱ��offset ���ڿ�ʼ��ȡ����ǰ�������ֽ�����
                // size:     ��Ҫ��ȡ���ݵ��ֽ�����ʹ�� text �� image ��������ʱ�����ַ�����ʹ�� ntext ��������ʱ������� size �� 0�����ʾ��ȡ�� 4 KB �ֽڵ����ݡ�
                // HOLDLOCK: ʹ�ı�ֵһֱ��������������������û����Զ�ȡ��ֵ�����ǲ��ܶ�������޸ġ�

                strCommand = "use " + this.m_strSqlDbName + " "
                    + " READTEXT records." + strImageFieldName
                    + " @text_ptr"
                    + " @offset"
                    + " @size"
                    + " HOLDLOCK";

                strCommand += " use master " + "\n";

                command = new SqlCommand(strCommand,
                    connection);

                SqlParameter text_ptrParam =
                    command.Parameters.Add("@text_ptr",
                    SqlDbType.VarBinary,
                    16);
                text_ptrParam.Value = textPtr;

                SqlParameter offsetParam =
                    command.Parameters.Add("@offset",
                    SqlDbType.Int);
                offsetParam.Value = nStart;

                SqlParameter sizeParam =
                    command.Parameters.Add("@size",
                    SqlDbType.Int);
                sizeParam.Value = nOutputLength;

                destBuffer = new Byte[nOutputLength];

                SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult);
                dr.Read();
                dr.GetBytes(0,
                    0,
                    destBuffer,
                    0,
                    System.Convert.ToInt32(sizeParam.Value));
                dr.Close();
            }

            return nTotalLength;
        }


        // дxml����
        // parameter:
        //		strID           ��¼ID -1:��ʾ׷��һ����¼
        //		strRanges       Ŀ���λ��,���range�ö��ŷָ�
        //		nTotalLength    �ܳ���
        //		inputTimestamp  �����ʱ���
        //		outputTimestamp ���ص�ʱ���
        //		strOutputID     ���صļ�¼ID,��strID == -1ʱ,�õ�ʵ�ʵ�ID
        //		strError        
        // return:
        //		-1  ����
        //		-2  ʱ�����ƥ��
        //      -4  ��¼������
        //      -6  Ȩ�޲���
        //		0   �ɹ�
        public override int WriteXml(User oUser,  //null���򲻼���Ȩ��
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
            out string strOutputValue,   //��AddInteger �� AppendStringʱ ����ֵ����ֵ
            bool bCheckAccount,
            out string strError)
        {
            strOutputValue = "";
            outputTimestamp = null;
            strOutputID = "";
            strError = "";

            if (strID == "?")
                strID = "-1";

            bool bPushTailNo = false;
            strID = this.EnsureID(strID,
                out bPushTailNo);  //�Ӻ�д��
            if (oUser != null)
            {
                string strTempRecordPath = this.GetCaption("zh-cn") + "/" + strID;
                if (bPushTailNo == true)
                {
                    string strExistRights = "";
                    bool bHasRight = oUser.HasRights(strTempRecordPath,
                        ResType.Record,
                        "create",//"append",
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
            int nRet = 0;

            bool bFull = false;
            //*********�����ݿ�Ӷ���*************
            m_lock.AcquireReaderLock(m_nTimeOut);

#if DEBUG_LOCK_SQLDATABASE
			this.container.WriteDebugInfo("WriteXml()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif
            try
            {
                strID = DbPath.GetID10(strID);
                //**********�Լ�¼��д��***************
                this.m_recordLockColl.LockForWrite(strID, m_nTimeOut);
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("WriteXml()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼��д����");
#endif
                try // ��¼��
                {

                    SqlConnection connection = new SqlConnection(this.m_strConnString);
                    connection.Open();
                    try // ����
                    {
                        // 1.�����¼������,����һ���ֽڵļ�¼,��ȷ���õ�textPtr
                        // return:
                        //		-1  ����
                        //      0   ������
                        //      1   ����
                        nRet = this.RecordIsExist(connection,
                            strID,
                            out strError);
                        if (nRet == -1)
                            return -1;

                        bool bExist = false;
                        if (nRet == 1)
                            bExist = true;

                        // �¼�¼ʱ������һ���ֽڣ���������ʱ���
                        if (bExist == false)
                        {
                            byte[] tempInputTimestamp = inputTimestamp;
                            // ע���¼�¼��ʱ���,��inputTimestamp����
                            nRet = this.InsertRecord(connection,
                                strID,
                                out inputTimestamp,//tempTimestamp,//
                                out strError);

                            if (nRet == -1)
                                return -1;
                        }


                        // д����
                        // return:
                        //		-1	һ���Դ���
                        //		-2	ʱ�����ƥ��
                        //		0	�ɹ�
                        nRet = this.WriteSqlRecord(connection,
                            strID,
                            strRanges,
                            (int)lTotalLength,
                            baSource,
                            streamSource,
                            strMetadata,
                            strStyle,
                            inputTimestamp,
                            out outputTimestamp,
                            out bFull,
                            out strError);
                        if (nRet <= -1)
                            return nRet;

                        // ��鷶Χ
                        //string strCurrentRange = this.GetRange(connection,
                        //	strID);
                        if (bFull == true)  //��������
                        {
                            byte[] baOldPreamble = new byte[0];
                            byte[] baNewPreamble = new byte[0];

                            // 1.�õ��¾ɼ�����
                            string strOldXml = "";
                            if (bExist == true)
                            {
                                // return:
                                //      -1  ����
                                //      -4  ��¼������
                                //      0   ��ȷ
                                nRet = this.GetXmlData(
                                    connection,
                                    strID,
                                    "data",
                                    out strOldXml,
                                    out baOldPreamble,
                                    out strError);
                                if (nRet <= -1 && nRet != -3)
                                    return nRet;
                            }

                            string strNewXml = "";
                            // return:
                            //      -1  ����
                            //      -4  ��¼������
                            //      0   ��ȷ
                            nRet = this.GetXmlData(
                                connection,
                                strID,
                                "newdata",
                                out strNewXml,
                                out baNewPreamble,
                                out strError);
                            if (nRet == -1)
                                return -1;

                            // �޸Ĳ���
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
                                    if (strCreatePath == "")
                                    {
                                        strError = "��'" + this.GetCaption("zh-cn") + "'��д���¼'" + strID + "'ʱ��XPathʽ��'" + strXPath + "'ָ���Ľڵ�δ�ҵ�����ʱxpath���ʽ�е�create��������Ϊ��ֵ";
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

                                if (node.NodeType == XmlNodeType.Attribute)
                                {

                                    if (strAction == "AddInteger"
                                        || strAction == "+AddInteger"
                                        || strAction == "AddInteger+")
                                    {
                                        int nNumber = 0;
                                        try
                                        {
                                            nNumber = Convert.ToInt32(strNewXml);
                                        }
                                        catch (Exception ex)
                                        {
                                            strError = "���������'" + strNewXml + "'�������ָ�ʽ��" + ex.Message;
                                            return -1;
                                        }

                                        string strOldValue = node.Value;
                                        string strLastValue;
                                        nRet = StringUtil.IncreaseNumber(strOldValue,
                                            nNumber,
                                            out strLastValue,
                                            out strError);
                                        if (nRet == -1)
                                            return -1;

                                        if (strAction == "AddInteger+")
                                        {
                                            strOutputValue = node.Value;
                                        }
                                        else
                                        {
                                            strOutputValue = strLastValue;
                                        }

                                        node.Value = strLastValue;
                                        //strOutputValue = node.Value;
                                    }
                                    else if (strAction == "AppendString")
                                    {

                                        node.Value = node.Value + strNewXml;
                                        strOutputValue = node.Value;
                                    }
                                    else if (strAction == "Push")
                                    {
                                        string strLastValue;
                                        nRet = StringUtil.GetBiggerLedNumber(node.Value,
                                            strNewXml,
                                            out strLastValue,
                                            out strError);
                                        if (nRet == -1)
                                            return -1;

                                        node.Value = strLastValue;
                                        strOutputValue = node.Value;
                                    }
                                    else
                                    {
                                        node.Value = strNewXml;
                                        strOutputValue = node.Value;
                                    }
                                }
                                else if (node.NodeType == XmlNodeType.Element)
                                {

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
                                                strError = "���������'" + strNewValue + "'�������ָ�ʽ��" + ex.Message;
                                                return -1;
                                            }


                                            string strLastValue;
                                            nRet = StringUtil.IncreaseNumber(strOldValue,
                                                nNumber,
                                                out strLastValue,
                                                out strError);
                                            if (nRet == -1)
                                                return -1;
                                            /*
                                                                                    string strLastValue;
                                                                                    nRet = Database.AddInteger(strNewValue,
                                                                                        strOldValue,
                                                                                        out strLastValue,
                                                                                        out strError);
                                                                                    if (nRet == -1)
                                                                                        return -1;
                                            */
                                            newNode.InnerText = strLastValue;
                                            strOutputValue = newNode.OuterXml;
                                        }
                                        else if (strAction == "AppendString")
                                        {
                                            newNode.InnerText = strOldValue + strNewValue;
                                            strOutputValue = newNode.OuterXml;
                                        }
                                        else if (strAction == "Push")
                                        {
                                            string strLastValue;
                                            nRet = StringUtil.GetBiggerLedNumber(strOldValue,
                                                strNewValue,
                                                out strLastValue,
                                                out strError);
                                            if (nRet == -1)
                                                return -1;

                                            newNode.InnerText = strLastValue;
                                            strOutputValue = newNode.OuterXml;
                                        }
                                    }

                                    node.ParentNode.RemoveChild(node);

                                }

                                strNewXml = tempDom.OuterXml;

                                byte[] baRealXml =
                                    DatabaseUtil.StringToByteArray(
                                    strNewXml,
                                    baNewPreamble);

                                string strMyRange = "";
                                strMyRange = "0-" + Convert.ToString(baRealXml.Length - 1);
                                lTotalLength = baRealXml.Length;

                                // return:
                                //		-1	һ���Դ���
                                //		-2	ʱ�����ƥ��
                                //		0	�ɹ�
                                nRet = this.WriteSqlRecord(connection,
                                    strID,
                                    strMyRange,
                                    (int)lTotalLength,
                                    baRealXml,
                                    null,
                                    strMetadata,
                                    strStyle,
                                    outputTimestamp,   //ע�����
                                    out outputTimestamp,
                                    out bFull,
                                    out strError);
                                if (nRet <= -1)
                                    return nRet;
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

                            // ���������
                            // return:
                            //      -1  ����
                            //      0   �ɹ�
                            nRet = this.ModifyKeys(connection,
                                newKeys,
                                oldKeys,
                                out strError);
                            if (nRet == -1)
                                return -1;

                            // �������ļ�
                            // return:
                            //      -1  ����
                            //      0   �ɹ�
                            nRet = this.ModifyFiles(connection,
                                strID,
                                newDom,
                                oldDom,
                                out strError);
                            if (nRet == -1)
                                return -1;

                            // 4.��new����data
                            // return:
                            //      -1  ����
                            //      >=0   �ɹ� ����Ӱ��ļ�¼��
                            nRet = this.UpdateDataField(connection,
                                strID,
                                out strError);
                            if (nRet == -1)
                                return -1;

                            // 5.ɾ��newdata�ֶ�
                            // return:
                            //		-1  ����
                            //		0   �ɹ�
                            nRet = this.DeleteDuoYuImage(connection,
                                strID,
                                "newdata",
                                0,
                                out strError);
                            if (nRet == -1)
                                return -1;
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Errors is SqlErrorCollection)
                            strError = "���ݿ�'" + this.GetCaption("zh") + "'��δ��ʼ����";
                        else
                            strError = "WriteXml() �ڸ�'" + this.GetCaption("zh-cn") + "'��д���¼'" + strID + "'ʱ����,ԭ��:" + sqlEx.Message;
                        return -1;
                    }
                    catch (Exception ex)
                    {
                        strError = "WriteXml() �ڸ�'" + this.GetCaption("zh-cn") + "'��д���¼'" + strID + "'ʱ����,ԭ��:" + ex.Message;
                        return -1;
                    }
                    finally // ����
                    {
                        connection.Close();
                    }
                }
                finally  // ��¼��
                {
                    //******�Լ�¼��д��****************************
                    m_recordLockColl.UnlockForWrite(strID);
#if DEBUG_LOCK_SQLDATABASE
					this.container.WriteDebugInfo("WriteXml()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼��д����");
#endif
                }
            }
            finally
            {
                //********�����ݿ�����****************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("WriteXml()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif
            }


            // ������������֪Ϊ�˻����д��������ʱ, һ��Ҫ��bCheckAccount==false
            // �����ã������������𲻱�Ҫ�ĵݹ�
            if (bFull == true
                && bCheckAccount == true
                && StringUtil.IsInList("account", this.TypeSafety) == true)
            {
                string strResPath = this.FullID + "/" + strID;

                this.container.UserColl.RefreshUserSafety(strResPath);
            }

            return 0;
        }

        // parameters:
        //      strRecorID   ��¼ID
        //      strObjectID  ����ID
        //      ��������ͬWriteXml,��strOutputID����
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
#if DEBUG_LOCK_SQLDATABASE
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
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("WriteObject()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼��д����");
#endif
                try // ��¼��
                {
                    // �����Ӷ���
                    SqlConnection connection = new SqlConnection(this.m_strConnString);
                    connection.Open();
                    try // ����
                    {
                        // 1.�ڶ�Ӧ��xml���ݣ��ö���·���ҵ�����ID
                        string strXml;
                        // return:
                        //      -1  ����
                        //      -4  ��¼������
                        //      0   ��ȷ
                        nRet = this.GetXmlString(connection,
                            strRecordID,
                            out strXml,
                            out strError);
                        if (nRet <= -1)
                        {
                            strError = "����'" + strRecordID + "/" + strObjectID + "'��Դʧ�ܣ�ԭ��:" + strError;
                            return nRet;
                        }
                        XmlDocument xmlDom = new XmlDocument();
                        xmlDom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue

                        xmlDom.LoadXml(strXml);

                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDom.NameTable);
                        nsmgr.AddNamespace("dprms", DpNs.dprms);
                        XmlNode fileNode = xmlDom.DocumentElement.SelectSingleNode("//dprms:file[@id='" + strObjectID + "']", nsmgr);
                        if (fileNode == null)
                        {
                            strError = "������xml��û���ҵ���ID��Ӧ��dprms:file�ڵ�";
                            return -1;
                        }

                        strObjectID = strRecordID + "_" + strObjectID;

                        // 2. ����¼Ϊ�ռ�¼ʱ,��updata�����ı�ָ��
                        if (this.IsEmptyObject(connection, strObjectID) == true)
                        {
                            // return
                            //		-1  ����
                            //		0   �ɹ�
                            nRet = this.UpdateObject(connection,
                                strObjectID,
                                out inputTimestamp,
                                out strError);
                            if (nRet == -1)
                                return -1;
                        }

                        // 3.������д��rangeָ���ķ�Χ
                        bool bFull = false;
                        // return:
                        //		-1	һ���Դ���
                        //		-2	ʱ�����ƥ��
                        //		0	�ɹ�
                        nRet = this.WriteSqlRecord(connection,
                            strObjectID,
                            strRanges,
                            (int)lTotalLength,
                            baSource,
                            streamSource,
                            strMetadata,
                            strStyle,
                            inputTimestamp,
                            out outputTimestamp,
                            out bFull,
                            out strError);
                        if (nRet <= -1)
                            return nRet;



                        //string strCurrentRange = this.GetRange(connection,strObjectID);
                        if (bFull == true)  //��������
                        {
                            // 1. ��newdata�滻data�ֶ�
                            // return:
                            //      -1  ����
                            //      >=0   �ɹ� ����Ӱ��ļ�¼��
                            nRet = this.UpdateDataField(connection,
                                strObjectID,
                                out strError);
                            if (nRet == -1)
                                return -1;

                            // 2. ɾ��newdata�ֶ�
                            // return:
                            //		-1  ����
                            //		0   �ɹ�
                            nRet = this.DeleteDuoYuImage(connection,
                                strObjectID,
                                "newdata",
                                0,
                                out strError);
                            if (nRet == -1)
                                return -1;
                        }

                        // �����޸�һ�¼�¼��ʱ���
                        string strNewTimestamp = this.CreateTimestampForDb();
                        // return:
                        //      -1  ����
                        //      >=0   �ɹ� ���ر�Ӱ��ļ�¼��
                        nRet = this.SetTimestampForDb(connection,
                            strRecordID,
                            strNewTimestamp,
                            out strError);
                        if (nRet == -1)
                            return -1;
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Errors is SqlErrorCollection)
                            strError = "���ݿ�'" + this.GetCaption("zh") + "'��δ��ʼ����";
                        else
                            strError = sqlEx.Message;
                        return -1;
                    }
                    catch (Exception ex)
                    {
                        strError = "WriteXml() �ڸ�'" + this.GetCaption("zh-cn") + "'��д����Դ'" + strObjectID + "'ʱ����,ԭ��:" + ex.Message;
                        return -1;
                    }
                    finally // ����
                    {
                        connection.Close();
                    }
                }
                finally // ��¼��
                {
                    //*********�Լ�¼��д��****************************
                    m_recordLockColl.UnlockForWrite(strRecordID);
#if DEBUG_LOCK_SQLDATABASE
					this.container.WriteDebugInfo("WriteObject()����'" + this.GetCaption("zh-cn") + "/" + strRecordID + "'��¼��д����");
#endif

                }
            }
            finally
            {
                //************�����ݿ�����************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("WriteObject()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif

            }

            return 0;
        }
        // ��sql��дһ����¼
        // ��baContent��streamContentд��image�ֶ���rangeָ��Ŀ��λ��,
        // ˵����sql�еļ�¼������Xml���¼Ҳ���Զ�����Դ��¼
        // parameters:
        //		connection	    ���Ӷ���	����Ϊnull
        //		strID	        ��¼ID	����Ϊnull����ַ���
        //		strRanges	    Ŀ�귶Χ�������Χ�ö��ŷָ�
        //		nTotalLength	��¼�����ܳ���
        //						����Sql ServerĿǰֻ֧��int������nTotalLength��Ϊint���ͣ�������ӿ���long
        //		baContent	    �����ֽ�����	����Ϊnull
        //		streamContent	������	����Ϊnull
        //		strStyle	    ���
        //					    ignorechecktimestamp	����ʱ���
        //		baInputTimestamp    �����ʱ���	����Ϊnull
        //		baOutputTimestamp	out���������ص�ʱ���
        //		bFull	        out��������¼�Ƿ�����
        //		strError	    out���������س�����Ϣ
        // return:
        //		-1	һ���Դ���
        //		-2	ʱ�����ƥ��
        //		0	�ɹ�
        // ˵��	baContent��streamContent��˭��ֵ����˭
        private int WriteSqlRecord(SqlConnection connection,
            string strID,
            string strRanges,
            int nTotalLength,
            byte[] baSource,
            Stream streamSource,
            string strMetadata,
            string strStyle,
            byte[] baInputTimestamp,
            out byte[] baOutputTimestamp,
            out bool bFull,
            out string strError)
        {
            baOutputTimestamp = null;
            strError = "";
            bFull = false;

            int nRet = 0;

            //-------------------------------------------
            //��������������м��
            //-------------------------------------------

            // return:
            //      -1  ����
            //      0   ����
            nRet = this.CheckConnection(connection, out strError);
            if (nRet == -1)
            {
                strError = "WriteSqlRecord()���ô���" + strError;
                return -1;
            }
            Debug.Assert(nRet == 0, "");

            if (strID == null || strID == "")
            {
                strError = "WriteSqlRecord()���ô���strID��������Ϊnull����ַ�����";
                return -1;
            }
            if (nTotalLength < 0)
            {
                strError = "WriteSqlRecord()���ô���nTotalLength����ֵ����Ϊ'" + Convert.ToString(nTotalLength) + "'��������ڵ���0��";
                return -1;
            }
            if (baSource == null && streamSource == null)
            {
                strError = "WriteSqlRecord()���ô���baSource������streamSource��������ͬʱΪnull��";
                return -1;
            }
            if (baSource != null && streamSource != null)
            {
                strError = "WriteSqlRecord()���ô���baSource������streamSource����ֻ����һ������ֵ��";
                return -1;
            }
            if (strStyle == null)
                strStyle = "";
            if (strRanges == null)
                strRanges = "";
            if (strMetadata == null)
                strMetadata = "";




            //-------------------------------------------
            //��ʼ������
            //-------------------------------------------

            ////////////////////////////////////////////////////
            // ����¼�Ƿ����,ʱ���Ƿ�ƥ��,���õ�����,range��textPtr
            /////////////////////////////////////////////////////
            string strCommand = "use " + this.m_strSqlDbName + " "
                + " SELECT TEXTPTR(newdata),"
                + " DataLength(newdata),"
                + " range,"
                + " dptimestamp,"
                + " metadata "
                + " FROM records "
                + " WHERE id=@id";

            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);
            SqlParameter idParam =
                command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            idParam.Value = strID;

            byte[] textPtr = null;
            string strOldMetadata = "";
            string strCurrentRange = "";
            int nCurrentLength = 0;
            string strOutputTimestamp = "";

            SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult);
            try
            {
                // 1.��¼�����ڱ���
                if (dr == null
                    || dr.HasRows == false)
                {
                    strError = "��¼'" + strID + "'�ڿ��в����ڣ��ǲ����ܵ����";
                    return -1;
                }

                dr.Read();

                // 2.textPtrΪnull����
                if (dr[0] is System.DBNull)
                {
                    strError = "TextPtr������Ϊnull";
                    return -1;
                }
                textPtr = (byte[])dr[0];

                // 3.ʱ���������Ϊnull,ʱ�����ƥ�䱨��
                if ((dr[4] is System.DBNull))
                {
                    strError = "ʱ���������Ϊnull";
                    return -1;
                }

                // ��strStyle���� ignorechecktimestampʱ�����ж�ʱ���
                strOutputTimestamp = dr.GetString(3);
                baOutputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTimestamp);

                if (StringUtil.IsInList("ignorechecktimestamp", strStyle) == false)
                {
                    if (ByteArray.Compare(baInputTimestamp,
                        baOutputTimestamp) != 0)
                    {
                        strError = "ʱ�����ƥ��";
                        return -2;
                    }
                }
                // 4.metadataΪnull����
                if ((dr[4] is System.DBNull))
                {
                    strError = "Metadata������Ϊnull";
                    return -1;
                }
                strOldMetadata = dr.GetString(4);


                // 5.rangeΪnull�ı���
                if ((dr[2] is System.DBNull))
                {
                    strError = "range��ʱҲ������Ϊnull";
                    return -1;
                }
                strCurrentRange = dr.GetString(2);

                // 6.ȡ������
                nCurrentLength = dr.GetInt32(1);
            }
            finally
            {
                dr.Close();
            }

            bFull = false;
            bool bDeleted = false;

            long nSourceTotalLength = 0;
            if (baSource != null)
                nSourceTotalLength = baSource.Length;
            else
                nSourceTotalLength = streamSource.Length;

            // ����rangeд����
            RangeList rangeList = null;
            if (strRanges == "")
            {
                RangeItem rangeItem = new RangeItem();
                rangeItem.lStart = 0;
                rangeItem.lLength = nSourceTotalLength;
                rangeList = new RangeList();
                rangeList.Add(rangeItem);
            }
            else
            {
                rangeList = new RangeList(strRanges);
            }


            int nStartOfBuffer = 0;    // ��������λ��
            int nState = 0;
            for (int i = 0; i < rangeList.Count; i++)
            {
                bool bCanDeleteDuoYu = false;  // ȱʡ������ɾ������ĳ���

                RangeItem range = (RangeItem)rangeList[i];
                int nStartOfTarget = (int)range.lStart;     // �ָ���image�ֶε�λ��  
                int nNeedReadLength = (int)range.lLength;   // ��Ҫ���������ĳ���
                if (rangeList.Count == 1 && nNeedReadLength == 0)
                {
                    bFull = true;
                    break;
                }

                string strThisEnd = Convert.ToString(nStartOfTarget + nNeedReadLength - 1);

                string strThisRange = Convert.ToString(nStartOfTarget)
                    + "-" + strThisEnd;

                string strNewRange;
                nState = RangeList.MergContentRangeString(strThisRange,
                    strCurrentRange,
                    nTotalLength,
                    out strNewRange);
                if (nState == -1)
                {
                    strError = "MergContentRangeString() error";
                    return -1;
                }
                if (nState == 1)  //��Χ����
                {
                    bFull = true;
                    string strFullEnd = "";
                    int nPosition = strNewRange.IndexOf('-');
                    if (nPosition >= 0)
                        strFullEnd = strNewRange.Substring(nPosition + 1);

                    // ��Ϊ��Χ�����һ��,�ұ��η�Χ��ĩβ�����ܷ�Χ��ĩβ,�һ�û��ɾ��ʱ
                    if (i == rangeList.Count - 1
                        && (strFullEnd == strThisEnd)
                        && bDeleted == false)
                    {
                        bCanDeleteDuoYu = true;
                        bDeleted = true;
                    }
                }
                strCurrentRange = strNewRange;

                // return:	
                //		-1  ����
                //		0   �ɹ�
                nRet = this.WriteImage(connection,
                    textPtr,
                    ref nCurrentLength,   // ��ǰimage�ĳ����ڲ��ϵı仯��
                    bCanDeleteDuoYu,
                    strID,
                    "newdata",
                    nStartOfTarget,
                    baSource,
                    streamSource,
                    nStartOfBuffer,
                    nNeedReadLength,
                    out strError);
                if (nRet == -1)
                    return -1;
                nStartOfBuffer += nNeedReadLength;
            }

            if (bFull == true)
            {
                if (bDeleted == false)
                {
                    // ����¼������ʱ��ɾ�������ֵ
                    // return:
                    //		-1  ����
                    //		0   �ɹ�
                    nRet = this.DeleteDuoYuImage(connection,
                        strID,
                        "newdata",
                        nTotalLength,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }
                strCurrentRange = "";
                nCurrentLength = nTotalLength;
            }
            else
            {
                nCurrentLength = -1;
            }

            // ���,����range,metadata,dptimestamp;

            // �õ���Ϻ��Metadata;
            string strResultMetadata;
            // return:
            //		-1	����
            //		0	�ɹ�
            nRet = DatabaseUtil.MergeMetadata(strOldMetadata,
                strMetadata,
                nCurrentLength,
                out strResultMetadata,
                out strError);
            if (nRet == -1)
                return -1;

            // �����µ�ʱ���,���浽���ݿ���
            strOutputTimestamp = this.CreateTimestampForDb();

            strCommand = "use " + this.m_strSqlDbName + "\n"
                + " UPDATE records "
                + " SET dptimestamp=@dptimestamp,"
                + " range=@range,"
                + " metadata=@metadata "
                + " WHERE id=@id";

            strCommand += " use master " + "\n";

            command = new SqlCommand(strCommand,
                connection);

            idParam = command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            idParam.Value = strID;

            SqlParameter dptimestampParam =
                command.Parameters.Add("@dptimestamp",
                SqlDbType.NVarChar,
                100);
            dptimestampParam.Value = strOutputTimestamp;

            SqlParameter rangeParam =
                command.Parameters.Add("@range",
                SqlDbType.NVarChar,
                4000);
            rangeParam.Value = strCurrentRange;

            SqlParameter metadataParam =
                command.Parameters.Add("@metadata",
                SqlDbType.NVarChar,
                4000);
            metadataParam.Value = strResultMetadata;

            int nCount = command.ExecuteNonQuery();
            if (nCount == 0)
            {
                strError = "û�и��µ���¼��Ϊ'" + strID + "'��ʱ���,range,metadata";
                return -1;
            }
            baOutputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTimestamp);//Encoding.UTF8.GetBytes(strOutputTimestamp);
            return 0;
        }

        // дimage�ֶε�����
        // ����ָ��һ��textprtָ��
        // parameter:
        //		connection  ���Ӷ���
        //		textPtr     imageָ��
        //		nOldLength  ԭ����
        //		nDeleteDuoYu    �Ƿ�ɾ������
        //		strID           ��¼id
        //		strImageFieldName   image�ֶ�
        //		nStartOfTarget      Ŀ�����ʼλ��
        //		sourceBuffer    Դ���ֽ�����
        //		streamSource    Դ����
        //		nStartOfBuffer  Դ������ʼλ��
        //		nNeedReadLength ��Ҫд�ĳ���
        //		strError        out���������س�����Ϣ
        // return:	
        //		-1  ����
        //		0   �ɹ�
        private int WriteImage(SqlConnection connection,
            byte[] textPtr,
            ref int nCurrentLength,           // ԭ���ĳ���     
            bool bDeleteDuoYu,
            string strID,
            string strImageFieldName,
            int nStartOfTarget,       // Ŀ�����ʼλ��
            byte[] baSource,
            Stream streamSource,
            int nStartOfSource,     // ��������ʵ��λ�� ���� >=0 
            int nNeedReadLength,    // ��Ҫ���������ĳ��ȿ�����-1,��ʾ��Դ��nSourceStartλ�õ�ĩβ
            out string strError)
        {
            strError = "";
            int nRet = 0;

            //---------------------------------------
            //���м���������
            //-----------------------------------------
            if (baSource == null && streamSource == null)
            {
                strError = "WriteImage()���ô���baSource������streamSource��������ͬʱΪnull��";
                return -1;
            }
            if (baSource != null && streamSource != null)
            {
                strError = "WriteImage()���ô���baSource������streamSource����ֻ����һ������ֵ��";
                return -1;
            }

            int nSourceTotalLength = 0;
            if (baSource != null)
                nSourceTotalLength = baSource.Length;
            else
                nSourceTotalLength = (int)streamSource.Length;

            int nOutputLength = 0;
            // return:
            //		-1  ����
            //		0   �ɹ�
            nRet = DatabaseUtil.GetRealLength(nStartOfSource,
                nNeedReadLength,
                nSourceTotalLength,
                -1,//nMaxLength
                out nOutputLength,
                out strError);
            if (nRet == -1)
                return -1;


            //---------------------------------------
            //��ʼ������
            //-----------------------------------------


            int chucksize = 32 * 1024;  //д��ʱÿ��Ϊ32K


            // ִ�и��²���,ʹ��UPDATETEXT���

            // UPDATETEXT����˵��:
            // dest_text_ptr: ָ��Ҫ���µ�image ���ݵ��ı�ָ���ֵ���� TEXTPTR �������أ�����Ϊ binary(16)
            // insert_offset: ����Ϊ���ĸ�����ʼλ��,
            //				  ����image �У�insert_offset ���ڲ���������ǰ�������е���㿪ʼҪ�������ֽ���
            //				  ��ʼ���������Ϊ������ʼ������� image ���������ƣ�Ϊ�������ڳ��ռ䡣
            //				  ֵΪ 0 ��ʾ�������ݲ��뵽����λ�õĿ�ʼ����ֵΪ NULL ��������׷�ӵ���������ֵ�С�
            // delete_length: �Ǵ� insert_offset λ�ÿ�ʼ�ġ�Ҫ������ image ����ɾ�������ݳ��ȡ�
            //				  delete_length ֵ���� text �� image �����ֽ�ָ�������� ntext �����ַ�ָ����ÿ�� ntext �ַ�ռ�� 2 ���ֽڡ�
            //				  ֵΪ 0 ��ʾ��ɾ�����ݡ�ֵΪ NULL ��ɾ������ text �� image ���д� insert_offset λ�ÿ�ʼ��ĩβ���������ݡ�
            // WITH LOG:      �� Microsoft? SQL Server? 2000 �б����ԡ��ڸð汾�У���־��¼�����ݿ����Ч�ָ�ģ�;�����
            // inserted_data: ��Ҫ���뵽���� text��ntext �� image �� insert_offset λ�õ����ݡ�
            //				  ���ǵ��� char��nchar��varchar��nvarchar��binary��varbinary��text��ntext �� image ֵ��
            //				  inserted_data ���������ֻ������
            // ���ʹ��UPDATETEXT����?
            // �滻��������:  ָ��һ���ǿ� insert_offset ֵ������ delete_length ֵ��Ҫ����������ݡ�
            // ɾ����������:  ָ��һ���ǿ� insert_offset ֵ������ delete_length ֵ����ָ��Ҫ����������ݡ�
            // ����������:    ָ�� insert_offset ֵ��Ϊ��� delete_length ֵ��Ҫ����������ݡ�
            string strCommand = "use " + this.m_strSqlDbName + " "
                + " UPDATETEXT records." + strImageFieldName
                + " @dest_text_ptr"
                + " @insert_offset"
                + " @delete_length"
                + " WITH LOG"
                + " @inserted_data";   //���ܼ�where���

            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);

            // ��������ֵ
            SqlParameter dest_text_ptrParam =
                command.Parameters.Add("@dest_text_ptr",
                SqlDbType.Binary,
                16);

            SqlParameter insert_offsetParam =
                command.Parameters.Add("@insert_offset",
                SqlDbType.Int);

            SqlParameter delete_lengthParam =
                command.Parameters.Add("@delete_length",
                SqlDbType.Int);

            SqlParameter inserted_dataParam =
                command.Parameters.Add("@inserted_data",
                SqlDbType.Binary,
                0);

            int insert_offset = nStartOfTarget; // ����image�ֶε�λ��
            int nReadStartOfBuffer = nStartOfSource;         // ��Դ�������еĶ�����ʼλ��
            Byte[] chuckBuffer = null; // �黺����
            int nCount = 0;             // Ӱ��ļ�¼����

            dest_text_ptrParam.Value = textPtr;

            while (true)
            {
                // �Ѵӻ����������ĳ���
                int nReadedLength = nReadStartOfBuffer - nStartOfSource;
                if (nReadedLength >= nNeedReadLength)
                    break;

                // ����Ҫ���ĳ���
                int nContinueLength = nNeedReadLength - nReadedLength;
                if (nContinueLength > chucksize)  // ��Դ���ж��ĳ���
                    nContinueLength = chucksize;

                inserted_dataParam.Size = nContinueLength;
                chuckBuffer = new byte[nContinueLength];

                if (baSource != null)
                {
                    // ����Դ�����һ�ε�ÿ������д��chuckbuffer
                    Array.Copy(baSource,
                        nReadStartOfBuffer,
                        chuckBuffer,
                        0,
                        nContinueLength);
                }
                else
                {
                    streamSource.Read(chuckBuffer,
                        0,
                        nContinueLength);
                }


                if (chuckBuffer.Length <= 0)
                    break;

                insert_offsetParam.Value = insert_offset;

                // ɾ���ֶεĳ���
                int nDeleteLength = 0;
                if (bDeleteDuoYu == true)  //���һ��
                {
                    nDeleteLength = nCurrentLength - insert_offset;  // ��ǰ���ȱ�ʾimage�ĳ���
                    if (nDeleteLength < 0)
                        nDeleteLength = 0;
                }
                else
                {
                    // д��ĳ��ȳ�����ǰ��󳤶�ʱ,Ҫɾ���ĳ���Ϊ��ǰ����-start
                    if (insert_offset + chuckBuffer.Length > nCurrentLength)
                    {
                        nDeleteLength = nCurrentLength - insert_offset;
                        if (nDeleteLength < 0)
                            nDeleteLength = nCurrentLength;
                    }
                    else
                    {
                        nDeleteLength = chuckBuffer.Length;
                    }
                }
                delete_lengthParam.Value = nDeleteLength;
                inserted_dataParam.Value = chuckBuffer;

                nCount = command.ExecuteNonQuery();
                if (nCount == 0)
                {
                    strError = "û�и��µ���¼��";
                    return -1;
                }

                // д���,��ǰ���ȷ����ı仯
                nCurrentLength = nCurrentLength + chuckBuffer.Length - nDeleteLength;

                // ��������λ�ñ仯
                nReadStartOfBuffer += chuckBuffer.Length;

                // Ŀ���λ�ñ仯
                insert_offset += chuckBuffer.Length;   //�ָ�ʱҪ�ָ���ԭ����λ��

                if (chuckBuffer.Length < chucksize)
                    break;
            }

            return 0;
        }

        // return:
        //      -1  ����
        //      0   �ɹ�
        public int ModifyKeys(SqlConnection connection,
            KeyCollection keysAdd,
            KeyCollection keysDelete,
            out string strError)
        {
            strError = "";
            string strCommand = "";

            string strRecordID = "";
            if (keysAdd != null && keysAdd.Count > 0)
                strRecordID = ((KeyItem)keysAdd[0]).RecordID;
            else if (keysDelete != null && keysDelete.Count > 0)
                strRecordID = ((KeyItem)keysDelete[0]).RecordID;

            SqlCommand command = new SqlCommand("", connection);

            int i = 0;
            // int nCount = 0;
            int nNameIndex = 0;

            // 2006/12/8 ��ɾ����ǰ��������ǰ
            if (keysDelete != null)
            {
                // ɾ��keys
                for (i = 0; i < keysDelete.Count; i++)
                {
                    KeyItem oneKey = (KeyItem)keysDelete[i];

                    string strKeysTableName = oneKey.SqlTableName;

                    string strIndex = Convert.ToString(nNameIndex++);

                    string strKeyParamName = "@key" + strIndex;
                    string strFromParamName = "@from" + strIndex;
                    string strIdParamName = "@id" + strIndex;
                    string strKeynumParamName = "@keynum" + strIndex;

                    strCommand += " DELETE FROM " + strKeysTableName
                        + " WHERE keystring = " + strKeyParamName + " AND fromstring= " + strFromParamName + " AND idstring= " + strIdParamName + " AND keystringnum= " + strKeynumParamName;

                    SqlParameter keyParam =
                        command.Parameters.Add(strKeyParamName,
                        SqlDbType.NVarChar);
                    keyParam.Value = oneKey.Key;

                    SqlParameter fromParam =
                        command.Parameters.Add(strFromParamName,
                        SqlDbType.NVarChar);
                    fromParam.Value = oneKey.FromValue;

                    SqlParameter idParam =
                        command.Parameters.Add(strIdParamName,
                        SqlDbType.NVarChar);
                    idParam.Value = oneKey.RecordID;

                    SqlParameter keynumParam =
                        command.Parameters.Add(strKeynumParamName,
                        SqlDbType.NVarChar);
                    keynumParam.Value = oneKey.Num;
                }
            }

            if (keysAdd != null)
            {
                // nCount = keysAdd.Count;

                // ����keys
                for (i = 0; i < keysAdd.Count; i++)
                {
                    KeyItem oneKey = (KeyItem)keysAdd[i];

                    string strKeysTableName = oneKey.SqlTableName;

                    // string strIndex = Convert.ToString(i);
                    string strIndex = Convert.ToString(nNameIndex++);

                    string strKeyParamName = "@key" + strIndex;
                    string strFromParamName = "@from" + strIndex;
                    string strIdParamName = "@id" + strIndex;
                    string strKeynumParamName = "@keynum" + strIndex;


                    //��keynum
                    strCommand += " INSERT INTO " + strKeysTableName
                        + " (keystring,fromstring,idstring,keystringnum) "
                        + " VALUES(" + strKeyParamName + ","
                        + strFromParamName + ","
                        + strIdParamName + ","
                        + strKeynumParamName + ")";
                    //+ " VALUES(@key,@from,@id,@keynum)";

                    SqlParameter keyParam =
                        command.Parameters.Add(strKeyParamName,
                        SqlDbType.NVarChar);
                    keyParam.Value = oneKey.Key;

                    SqlParameter fromParam =
                        command.Parameters.Add(strFromParamName,
                        SqlDbType.NVarChar);
                    fromParam.Value = oneKey.FromValue;

                    SqlParameter idParam =
                        command.Parameters.Add(strIdParamName,
                        SqlDbType.NVarChar);
                    idParam.Value = oneKey.RecordID;

                    SqlParameter keynumParam =
                        command.Parameters.Add(strKeynumParamName,
                        SqlDbType.NVarChar);
                    keynumParam.Value = oneKey.Num;

                }
            }




            if (strCommand != "")
            {
                strCommand = "use " + this.m_strSqlDbName + " \n"
                    + strCommand
                    + " use master " + "\n";
                command.CommandText = strCommand;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    strError = "�������������,��¼·��'" + this.GetCaption("zh-cn") + "/" + strRecordID + "��ԭ��" + ex.Message;
                    return -1;
                }
            }
            return 0;
        }


        // �������ļ�
        // return:
        //      -1  ����
        //      0   �ɹ�
        public int ModifyFiles(SqlConnection connection,
            string strID,
            XmlDocument newDom,
            XmlDocument oldDom,
            out string strError)
        {
            strError = "";
            strID = DbPath.GetID10(strID);

            // ���ļ�
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

            // ���ļ�
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

            string strCommand = "";
            SqlCommand command = new SqlCommand("", connection);

            int nCount = 0;
            //ɾ�����ļ�
            if (targetRight.Count > 0)
            {
                nCount = targetRight.Count;

                for (int i = 0; i < targetRight.Count; i++)
                {
                    string strPureObjectID = targetRight[i];
                    string strObjectID = strID + "_" + strPureObjectID;

                    string strParamIDName = "@id" + Convert.ToString(i);
                    strCommand += " DELETE FROM records WHERE id = " + strParamIDName + " \n";
                    SqlParameter idParam =
                        command.Parameters.Add(strParamIDName,
                        SqlDbType.NVarChar);
                    idParam.Value = strObjectID;
                }
            }

            // �������ļ�
            if (targetLeft.Count > 0)
            {
                for (int i = 0; i < targetLeft.Count; i++)
                {
                    string strPureObjectID = targetLeft[i];
                    string strObjectID = strID + "_" + strPureObjectID;

                    string strParamIDName = "@id" + Convert.ToString(i) + nCount;
                    strCommand += " INSERT INTO records(id) "
                        + " VALUES(" + strParamIDName + "); \n";
                    SqlParameter idParam =
                        command.Parameters.Add(strParamIDName,
                        SqlDbType.NVarChar);
                    idParam.Value = strObjectID;
                }
            }

            if (strCommand != "")
            {
                strCommand = "use " + this.m_strSqlDbName + " \n"
                    + strCommand
                    + " use master " + "\n";

                command.CommandText = strCommand;
                command.CommandTimeout = 10 * 60; // 10����

                int nResultCount = 0;
                try
                {
                    nResultCount = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    strError = "�����¼·��Ϊ'" + this.GetCaption("zh") + "/" + strID + "'�����ļ���������:" + ex.Message + ",sql����:\r\n" + strCommand;
                    return -1;
                }

                if (nResultCount != targetRight.Count + targetLeft.Count)
                {
                    this.container.WriteErrorLog("ϣ��������ļ���'" + Convert.ToString(targetRight.Count + targetLeft.Count) + "'����ʵ��ɾ�����ļ���'" + Convert.ToString(nResultCount) + "'��");
                }
            }

            return 0;
        }



        // �������Ӷ����Ƿ���ȷ
        // return:
        //      -1  ����
        //      0   ����
        private int CheckConnection(SqlConnection connection,
            out string strError)
        {
            strError = "";
            if (connection == null)
            {
                strError = "connectionΪnull";
                return -1;
            }
            if (connection.State != ConnectionState.Open)
            {
                strError = "connectionû�д�";
                return -1;
            }
            return 0;
        }


        // �õ���Χ
        private string GetRange(SqlConnection connection,
            string strID)
        {
            string strRange = "";

            string strCommand = "use " + this.m_strSqlDbName + " "
                + "select range from records where id='" + strID + "'";

            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult);
            try
            {
                if (dr != null && dr.HasRows == true)
                {
                    dr.Read();
                    strRange = dr.GetString(0);
                    if (strRange == null)
                        strRange = "";
                }
            }
            finally
            {
                dr.Close();
            }

            return strRange;
        }


        // ���¶���,ʹimage�ֶλ����Ч��TextPrtָ��
        // return
        //		-1  ����
        //		0   �ɹ�
        private int UpdateObject(SqlConnection connection,
            string strObjectID,
            out byte[] outputTimestamp,
            out string strError)
        {
            outputTimestamp = null;
            strError = "";

            // ������Ӷ���
            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            string strCommand = "";
            SqlCommand command = null;

            string strOutputTimestamp = this.CreateTimestampForDb();

            strCommand = "use " + this.m_strSqlDbName + " "
                + " UPDATE records "
                + " set newdata=0x0,range='0-0',dptimestamp=@dptimestamp,metadata=@metadata "
                + " where id='" + strObjectID + "'";

            strCommand += " use master " + "\n";

            command = new SqlCommand(strCommand,
                connection);

            string strMetadata = "<file size='0'/>";
            SqlParameter metadataParam =
                command.Parameters.Add("@metadata",
                SqlDbType.NVarChar);
            metadataParam.Value = strMetadata;


            SqlParameter dptimestampParam =
                command.Parameters.Add("@dptimestamp",
                SqlDbType.NVarChar,
                100);
            dptimestampParam.Value = strOutputTimestamp;

            int nCount = command.ExecuteNonQuery();
            if (nCount <= 0)
            {
                strError = "û�и���'" + strObjectID + "'��¼";
                return -1;
            }
            // ���ص�ʱ���
            outputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTimestamp);//Encoding.UTF8.GetBytes(strOutputTimestamp);
            return 0;
        }

        // �ж�һ���Զ�����Դ���ǿն���
        private bool IsEmptyObject(SqlConnection connection,
            string strID)
        {
            return this.IsEmptyObject(connection,
                "newdata",
                strID);
        }

        // �ж�һ���Զ�����Դ���ǿն���
        private bool IsEmptyObject(SqlConnection connection,
            string strImageFieldName,
            string strID)
        {
            string strError = "";
            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                throw (new Exception(strError));

            string strCommand = "";
            SqlCommand command = null;
            strCommand = "use " + this.m_strSqlDbName + " "
                + " SELECT @Pointer=TEXTPTR(" + strImageFieldName + ") "
                + " FROM records "
                + " WHERE id=@id";

            strCommand += " use master " + "\n";

            command = new SqlCommand(strCommand,
                connection);
            SqlParameter idParam =
                command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            idParam.Value = strID;

            SqlParameter PointerOutParam =
                command.Parameters.Add("@Pointer",
                SqlDbType.VarBinary,
                100);
            PointerOutParam.Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            if (PointerOutParam == null
                || PointerOutParam.Value is System.DBNull)
            {
                return true;
            }
            return false;
        }

        // ����һ���¼�¼,ʹ������Ч��textptr,��װInsertRecord
        private int InsertRecord(SqlConnection connection,
            string strID,
            out byte[] outputTimestamp,
            out string strError)
        {
            return this.InsertRecord(connection,
                strID,
                "newdata",
                new byte[] { 0x0 },
                out outputTimestamp,
                out strError);
        }

        // �����в���һ����¼
        private int InsertRecord(SqlConnection connection,
            string strID,
            string strImageFieldName,
            byte[] sourceBuffer,
            out byte[] outputTimestamp,
            out string strError)
        {
            outputTimestamp = null;
            strError = "";

            // ������Ӷ���
            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            string strCommand = "";
            SqlCommand command = null;

            string strRange = "0-" + Convert.ToString(sourceBuffer.Length - 1);
            string strOutputTimestamp = this.CreateTimestampForDb();

            strCommand = "use " + this.m_strSqlDbName + " "
                + " INSERT INTO records(id," + strImageFieldName + ",range,metadata,dptimestamp) "
                + " VALUES(@id,@data,@range,@metadata,@dptimestamp);";

            strCommand += " use master " + "\n";

            command = new SqlCommand(strCommand,
                connection);

            SqlParameter idParam =
                command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            idParam.Value = strID;

            SqlParameter dataParam =
                command.Parameters.Add("@data",
                SqlDbType.Binary,
                sourceBuffer.Length);
            dataParam.Value = sourceBuffer;

            SqlParameter rangeParam =
                command.Parameters.Add("@range",
                SqlDbType.NVarChar);
            rangeParam.Value = strRange;

            string strMetadata = "<file size='0'/>";
            SqlParameter metadataParam =
                command.Parameters.Add("@metadata",
                SqlDbType.NVarChar);
            metadataParam.Value = strMetadata;

            SqlParameter dptimestampParam =
                command.Parameters.Add("@dptimestamp",
                SqlDbType.NVarChar,
                100);
            dptimestampParam.Value = strOutputTimestamp;

            int nCount = command.ExecuteNonQuery();
            if (nCount <= 0)
            {
                strError = "InsertImage() SQL����ִ��Ӱ�������Ϊ" + Convert.ToString(nCount);
                return -1;
            }

            // ���ص�ʱ���
            outputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTimestamp);//Encoding.UTF8.GetBytes(strOutputTimestamp);
            return 0;
        }


        // ��newdata�ֶ��滻data�ֶ�
        // parameters:
        //      connection  SqlConnection����
        //      strID       ��¼id
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      >=0   �ɹ� ����Ӱ��ļ�¼��
        // ��: ����ȫ
        private int UpdateDataField(SqlConnection connection,
            string strID,
            out string strError)
        {
            strError = "";
            // ������Ӷ���
            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            string strCommand = "use " + this.m_strSqlDbName + " "
                + " UPDATE records \n"
                + " SET data=newdata \n"
                + " WHERE id='" + strID + "'";
            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);
            command.CommandTimeout = 5 * 60;  // 30����

            int nCount = command.ExecuteNonQuery();
            if (nCount == -1)
            {
                strError = "û���滻���ü�¼'" + strID + "'��data�ֶ�";
                return -1;
            }

            return nCount;
        }

        // ɾ��image����Ĳ���
        // parameter:
        //		connection  ���Ӷ���
        //		strID       ��¼ID
        //		strImageFieldName   image�ֶ���
        //		nStart      ��ʼλ��
        //		strError    out���������س�����Ϣ
        // return:
        //		-1  ����
        //		0   �ɹ�
        // ��: ����ȫ
        private int DeleteDuoYuImage(SqlConnection connection,
            string strID,
            string strImageFieldName,
            int nStart,
            out string strError)
        {
            strError = "";

            // ������Ӷ���
            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            string strCommand = "";
            SqlCommand command = null;

            // 1.�õ�imageָ�� �� ����
            strCommand = "use " + this.m_strSqlDbName + " "
                + " SELECT @Pointer=TEXTPTR(" + strImageFieldName + "),"
                + " @Length=DataLength(" + strImageFieldName + ") "
                + " FROM records "
                + " WHERE id=@id";

            strCommand += " use master " + "\n";

            command = new SqlCommand(strCommand,
                connection);
            command.CommandTimeout = 20 * 60;  // 20����

            SqlParameter idParam =
                command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            idParam.Value = strID;

            SqlParameter PointerOutParam =
                command.Parameters.Add("@Pointer",
                SqlDbType.VarBinary,
                100);
            PointerOutParam.Direction = ParameterDirection.Output;

            SqlParameter LengthOutParam =
                command.Parameters.Add("@Length",
                SqlDbType.Int);
            LengthOutParam.Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();
            if (PointerOutParam == null)
            {
                strError = "û�ҵ�imageָ��";
                return -1;
            }

            int nTotalLength = (int)LengthOutParam.Value;
            if (nStart >= nTotalLength)
                return 0;


            // 2.����ɾ��
            strCommand = "use " + this.m_strSqlDbName + " "
                + " UPDATETEXT records." + strImageFieldName
                + " @dest_text_ptr"
                + " @insert_offset"
                + " NULL"  //@delete_length"
                + " WITH LOG";
            //+ " @inserted_data";   //���ܼ�where���

            strCommand += " use master " + "\n";

            command = new SqlCommand(strCommand,
                connection);

            // ��������ֵ
            SqlParameter dest_text_ptrParam =
                command.Parameters.Add("@dest_text_ptr",
                SqlDbType.Binary,
                16);

            SqlParameter insert_offsetParam =
                command.Parameters.Add("@insert_offset",
                SqlDbType.Int);


            dest_text_ptrParam.Value = PointerOutParam.Value;
            insert_offsetParam.Value = nStart;

            command.ExecuteNonQuery();

            return 0;
        }

        // ����¼�ڿ����Ƿ����
        // return:
        //		-1  ����
        //      0   ������
        //      1   ����
        private int RecordIsExist(SqlConnection connection,
            string strID,
            out string strError)
        {
            strError = "";

            // ������Ӷ���
            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            string strCommand = "use " + this.m_strSqlDbName + " "
                + " SET NOCOUNT OFF;"
                + "select id from records where id='" + strID + "'";
            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult);
            try
            {
                if (dr != null && dr.HasRows == true)
                    return 1;
            }
            finally
            {
                dr.Close();
            }
            return 0;
        }

        // �ӿ��еõ�һ����¼��ʱ���
        // return:
        //		-1  ����
        //		-4  δ�ҵ���¼
        //      0   �ɹ�
        private int GetTimestampFromDb(SqlConnection connection,
            string strID,
            out byte[] outputTimestamp,
            out string strError)
        {
            strError = "";
            outputTimestamp = null;
            int nRet = 0;

            string strOutputRecordID = "";
            // return:
            //      -1  ����
            //      0   �ɹ�
            nRet = this.CanonicalizeRecordID(strID,
                out strOutputRecordID,
                out strError);
            if (nRet == -1)
            {
                strError = "GetTimestampFormDb()���ô���strID����ֵ'" + strID + "'���Ϸ���";
                return -1;
            }
            if (strOutputRecordID == "-1")
            {
                strError = "GetTimestampFormDb()���ô���strID����ֵ'" + strID + "'���Ϸ���";
                return -1;
            }
            strID = strOutputRecordID;


            // return:
            //      -1  ����
            //      0   ����
            nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            string strCommand = "use " + this.m_strSqlDbName + " "
                + "select dptimestamp "
                + " from records "
                + " where id='" + strID + "'";

            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult);
            try
            {
                if (dr == null
                    || dr.HasRows == false)
                {
                    strError = "GetTimestampFromDb() ��¼'" + strID + "'�ڿ��в�����";
                    return -4;
                }
                dr.Read();
                string strOutputTimestamp = dr.GetString(0);
                outputTimestamp = ByteArray.GetTimeStampByteArray(strOutputTimestamp);//Encoding.UTF8.GetBytes(strOutputTimestamp);
            }
            finally
            {
                dr.Close();
            }
            return 0;
        }

        // ��ȡ��¼��ʱ���
        // parameters0:
        //      strID   ��¼id
        //      baOutputTimestamp
        // return:
        //		-1  ����
        //		-4  δ�ҵ���¼
        //      0   �ɹ�
        public override int GetTimestampFromDb(
            string strID,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            baOutputTimestamp = null;
            strError = "";

            // �����Ӷ���
            SqlConnection connection = new SqlConnection(this.m_strConnString);
            connection.Open();
            try
            {
                // return:
                //		-1  ����
                //		-4  δ�ҵ���¼
                //      0   �ɹ�
                return this.GetTimestampFromDb(connection,
                    strID,
                    out baOutputTimestamp,
                    out strError);
            }
            finally
            {
                connection.Close();
            }
        }


        // ����ָ����¼��ʱ���
        // parameters:
        //      connection  SqlConnection����
        //      strID       ��¼id�������Ǽ�¼Ҳ��������Դ
        //      strInputTimestamp   �����ʱ���
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      >=0   �ɹ� ���ر�Ӱ��ļ�¼��
        private int SetTimestampForDb(SqlConnection connection,
            string strID,
            string strInputTimestamp,
            out string strError)
        {
            strError = "";

            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            string strCommand = "use " + this.m_strSqlDbName + "\n"
                + " UPDATE records "
                + " SET dptimestamp=@dptimestamp"
                + " WHERE id=@id";
            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);

            SqlParameter idParam = command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            idParam.Value = strID;

            SqlParameter dptimestampParam =
                command.Parameters.Add("@dptimestamp",
                SqlDbType.NVarChar,
                100);
            dptimestampParam.Value = strInputTimestamp;

            int nCount = command.ExecuteNonQuery();
            if (nCount == 0)
            {
                strError = "û�и��µ���¼��Ϊ'" + strID + "'��ʱ���";
                return -1;
            }
            return nCount;
        }

        // ɾ����¼,�������ļ�,������,�ͱ���¼
        // parameter:
        //		strRecordID           ��¼ID
        //		inputTimestamp  �����ʱ���
        //		outputTimestamp out����,���ص�ʵ�ʵ�ʱ���
        //		strError        out����,���س�����Ϣ
        // return:
        //		-1  һ���Դ���
        //		-2  ʱ�����ƥ��
        //      -4  δ�ҵ���¼
        //		0   �ɹ�
        // ��: ��ȫ
        public override int DeleteRecord(string strRecordID,
            byte[] baInputTimestamp,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            strError = "";
            baOutputTimestamp = null;

            strRecordID = DbPath.GetID10(strRecordID);

            //********�����ݿ�Ӷ���*********************
            m_lock.AcquireReaderLock(m_nTimeOut);
#if DEBUG_LOCK_SQLDATABASE		
			this.container.WriteDebugInfo("DeleteRecordForce()����'" + this.GetCaption("zh-cn") + "'���ݿ�Ӷ�����");
#endif

            int nRet = 0;
            try
            {
                //*********�Լ�¼��д��**********
                m_recordLockColl.LockForWrite(strRecordID, m_nTimeOut);
#if DEBUG_LOCK_SQLDATABASE
				this.container.WriteDebugInfo("DeleteRecordForce()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼��д����");
#endif
                try
                {
                    SqlConnection connection = new SqlConnection(this.m_strConnString);
                    connection.Open();
                    try
                    {
                        // �Ƚ�ʱ���
                        // return:
                        //		-1  ����
                        //		-4  δ�ҵ���¼
                        //      0   �ɹ�
                        nRet = this.GetTimestampFromDb(connection,
                            strRecordID,
                            out baOutputTimestamp,
                            out strError);
                        if (nRet <= -1)
                            return nRet;

                        if (baOutputTimestamp == null)
                        {
                            strError = "������ȡ����ʱ���Ϊnull";
                            return -1;
                        }

                        if (ByteArray.Compare(baInputTimestamp,
                            baOutputTimestamp) != 0)
                        {
                            strError = "ʱ�����ƥ��";
                            return -2;
                        }

                        //bool bXmlError = false;
                        string strXml;
                        // return:
                        //      -1  ����
                        //      -4  ��¼������
                        //      0   ��ȷ
                        nRet = this.GetXmlString(connection,
                            strRecordID,
                            out strXml,
                            out strError);
                        if (nRet <= -1)
                            return nRet;

                        XmlDocument newDom = null;
                        XmlDocument oldDom = null;

                        KeyCollection newKeys = null;
                        KeyCollection oldKeys = null;
                        // 1.ɾ��������

                        // return:
                        //      -1  ����
                        //      0   �ɹ�
                        nRet = this.MergeKeys(strRecordID,
                            "",
                            strXml,
                            true,
                            out newKeys,
                            out oldKeys,
                            out newDom,
                            out oldDom,
                            out strError);
                        if (nRet == -1)
                            return -1;

                        if (oldDom != null)
                        {
                            // return:
                            //      -1  ����
                            //      0   �ɹ�
                            nRet = this.ModifyKeys(connection,
                                null,
                                oldKeys,
                                out strError);
                            if (nRet == -1)
                                return -1;
                        }
                        else
                        {
                            // return:
                            //      -1  ����
                            //      0   �ɹ�
                            nRet = this.ForceDeleteKeys(connection,
                                strRecordID,
                                out strError);
                            if (nRet == -1)
                                return -1;
                        }

                        // 2.ɾ�����ļ�
                        if (oldDom != null)
                        {
                            // return:
                            //      -1  ����
                            //      0   �ɹ�
                            nRet = this.ModifyFiles(connection,
                                strRecordID,
                                null,
                                oldDom,
                                out strError);
                            if (nRet == -1)
                                return -1;
                        }
                        else
                        {

                            // ͨ����¼��֮��Ĺ�ϵǿ��ɾ��
                            // return:
                            //      -1  ����
                            //      0   �ɹ�
                            nRet = this.ForceDeleteFiles(connection,
                                strRecordID,
                                out strError);
                            if (nRet == -1)
                                return -1;
                        }

                        // 3.ɾ���Լ�,����ɾ���ļ�¼��
                        // return:
                        //      -1  ����
                        //      >=0   �ɹ� ����ɾ���ļ�¼��
                        nRet = DeleteRecordByID(connection,
                            strRecordID,
                            out strError);
                        if (nRet == -1)
                            return -1;

                        if (nRet == 0)
                        {
                            strError = "ɾ����¼ʱ,�ӿ���û�ҵ���¼��Ϊ'" + strRecordID + "'�ļ�¼";
                            return -1;
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Errors is SqlErrorCollection)
                            strError = "���ݿ�'" + this.GetCaption("zh") + "'��δ��ʼ����";
                        else
                            strError = sqlEx.Message;
                        return -1;
                    }
                    catch (Exception ex)
                    {
                        strError = "ɾ��'" + this.GetCaption("zh-cn") + "'����idΪ'" + strRecordID + "'�ļ�¼ʱ����,ԭ��:" + ex.Message;
                        return -1;
                    }
                    finally // ����
                    {
                        connection.Close();
                    }
                }
                finally // ��¼��
                {
                    //**************�Լ�¼��д��**********
                    m_recordLockColl.UnlockForWrite(strRecordID);
#if DEBUG_LOCK_SQLDATABASE			
					this.container.WriteDebugInfo("DeleteRecordForce()����'" + this.GetCaption("zh-cn") + "/" + strID + "'��¼��д����");
#endif

                }
            }
            finally
            {
                //***************�����ݿ�����*****************
                m_lock.ReleaseReaderLock();
#if DEBUG_LOCK_SQLDATABASE		
				this.container.WriteDebugInfo("DeleteRecordForce()����'" + this.GetCaption("zh-cn") + "'���ݿ�������");
#endif

            }
            return 0;
        }


        // ���ݼ�¼��֮��Ĺ�ϵ(��¼��~~��¼��_0),ǿ��ɾ����Դ�ļ�
        // parameters:
        //      connection  SqlConnection����
        //      strRecordID ��¼id  ������10λ
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        private int ForceDeleteFiles(SqlConnection connection,
            string strRecordID,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            // ��������
            // return:
            //      -1  ����
            //      0   ����
            nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            Debug.Assert(strRecordID != null && strRecordID.Length == 10, "ForceDeleteFiles()���ô���strRecordID����ֵ����Ϊnull�ҳ��ȱ������10λ��");

            string strCommand = "use " + this.m_strSqlDbName + " "
                + " DELETE FROM records WHERE id like @id";
            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);
            SqlParameter param = command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            param.Value = strRecordID + "_%";

            //???�������ɾ������
            int nDeletedCount = command.ExecuteNonQuery();

            return 0;
        }

        // ǿ��ɾ����¼��Ӧ�ļ�����,������еı�
        // parameters:
        //      connection  SqlConnection���Ӷ���
        //      strRecordID ��¼id,��֮ǰ������Ϊ10
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // ��: ����ȫ
        public int ForceDeleteKeys(SqlConnection connection,
            string strRecordID,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            // return:
            //      -1  ����
            //      0   ����
            nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;

            Debug.Assert((strRecordID != null) && (strRecordID.Length == 10), "ForceDeleteKeys()���ô���strRecordID����ֵ����Ϊnull�ҳ��ȱ������10λ��");

            KeysCfg keysCfg = null;
            nRet = this.GetKeysCfg(out keysCfg,
                out strError);
            if (nRet == -1)
                return -1;
            if (keysCfg == null)
                return 0;

            List<TableInfo> aTableInfo = null;
            nRet = keysCfg.GetTableInfosRemoveDup(
                out aTableInfo,
                out strError);
            if (nRet == -1)
                return -1;

            string strCommand = "";
            for (int i = 0; i < aTableInfo.Count; i++)
            {
                TableInfo tableInfo = aTableInfo[i];

                string strTempID = "@id" + Convert.ToString(i);
                strCommand += "DELETE FROM " + tableInfo.SqlTableName
                    + " WHERE idstring=" + strTempID + "\r\n";
            }

            if (strCommand != "")
            {
                strCommand = "use " + this.m_strSqlDbName + " \r\n"
                    + strCommand
                    + "use master " + "\r\n";

                SqlCommand command = new SqlCommand(strCommand,
                    connection);

                for (int i = 0; i < aTableInfo.Count; i++)
                {
                    string strTempID = "@id" + Convert.ToString(i);
                    SqlParameter idParam = command.Parameters.Add(strTempID,
                        SqlDbType.NVarChar);
                    idParam.Value = strRecordID;
                }

                // ????�������ɾ������
                int nDeletedCount = command.ExecuteNonQuery();
            }
            return 0;
        }

        // �ӿ���ɾ��ָ���ļ�¼,�����Ǽ�¼Ҳ��������Դ
        // parameters:
        //      connection  ���Ӷ���
        //      strID       ��¼id
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      >=0   �ɹ� ����ɾ���ļ�¼��
        private int DeleteRecordByID(
            SqlConnection connection,
            string strID,
            out string strError)
        {
            strError = "";

            Debug.Assert(connection != null, "DeleteRecordById()���ô���connection����ֵ����Ϊnull��");
            Debug.Assert(strID != null, "DeleteRecordById()���ô���strID����ֵ����Ϊnull��");
            Debug.Assert(strID.Length >= 10, "DeleteRecordByID()���ô��� strID����ֵ�ĳ��ȱ�����ڵ���10��");

            // return:
            //      -1  ����
            //      0   ����
            int nRet = this.CheckConnection(connection,
                out strError);
            if (nRet == -1)
                return -1;


            string strCommand = "use " + this.m_strSqlDbName + " "
                + " DELETE FROM records WHERE id = @id";
            strCommand += " use master " + "\n";

            SqlCommand command = new SqlCommand(strCommand,
                connection);
            command.CommandTimeout = 10 * 60;// 10����

            SqlParameter param = command.Parameters.Add("@id",
                SqlDbType.NVarChar);
            param.Value = strID;

            int nDeletedCount = command.ExecuteNonQuery();

            if (nDeletedCount != 1)
            {
                this.container.WriteErrorLog("ϣ��ɾ��" + strID + " '1'����ʵ��ɾ��'" + Convert.ToString(nDeletedCount) + "'��");
            }

            return nDeletedCount;
        }

    }
}
