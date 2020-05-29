using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Web;

using DigitalPlatform.Xml;
using DigitalPlatform.dp2.Statis;
using DigitalPlatform.Script;

namespace dp2Circulation
{
    /// <summary>
    /// ReaderStatisForm (����ͳ�ƴ�) ͳ�Ʒ�����������
    /// </summary>
    public class ReaderStatis : StatisHostBase
    {
        /// <summary>
        /// �������������� ReaderStatisForm (����ͳ�ƴ�)
        /// </summary>
        public ReaderStatisForm ReaderStatisForm = null;	// ����

        /// <summary>
        /// �ӷ���������λ�ȡ����XML�ĸ�ʽ�� ���Ϊ "advancexml"��������ḻ��ҵ����Ϣ���������ٶȻ�������ȱʡΪ "xml"
        /// </summary>
        public string XmlFormat = "xml";    // �ӷ������˻�ȡ����XML�ĸ�ʽ�����Ϊadvancexml��������ḻ��ҵ����Ϣ���������ٶȻ�����

#if NO
        private bool disposed = false;

                public WebBrowser Console = null;

                public string ProjectDir = "";  // ����Դ�ļ�����Ŀ¼
        public string InstanceDir = ""; // ��ǰʵ����ռ��Ŀ¼�����ڴ洢��ʱ�ļ�

        public List<string> OutputFileNames = new List<string>(); // ��������html�ļ�

        int m_nFileNameSeed = 1;

#endif

        /// <summary>
        /// ��֤���ڷ�Χ������ɸѡ����ͬ���Ķ��߼�¼
        /// </summary>
        public string CreateTimeRange = "";

        /// <summary>
        /// (����֤)ʧЧ���ڷ�Χ������ɸѡ����ͬ���Ķ��߼�¼
        /// </summary>
        public string ExpireTimeRange = "";

        /// <summary>
        /// ��֤���ڷ�Χ֮��ʼʱ��
        /// </summary>
        public DateTime CreateStartDate = new DateTime(0);
        /// <summary>
        /// ��֤���ڷ�Χ֮����ʱ��
        /// </summary>
        public DateTime CreateEndDate = new DateTime(0);

        /// <summary>
        /// ʧЧ���ڷ�Χ֮��ʼʱ��
        /// </summary>
        public DateTime ExpireStartDate = new DateTime(0);
        /// <summary>
        /// ʧЧ���ڷ�Χ֮����ʱ��
        /// </summary>
        public DateTime ExpireEndDate = new DateTime(0);

        /// <summary>
        /// ��λ���б�����ɸѡ����ͬ���Ķ��߼�¼
        /// </summary>
        public string DepartmentNames = "";

        /// <summary>
        /// ���������б�����ɸѡ����ͬ���Ķ��߼�¼
        /// </summary>
        public string ReaderTypes = "";

        /// <summary>
        /// ��ǰ���߼�¼·��
        /// </summary>
        public string CurrentRecPath = "";    // ��ǰ���߼�¼·��

        /// <summary>
        /// ��ǰ���߼�¼�������е��±�
        /// </summary>
        public long CurrentRecordIndex = -1; // ��ǰ���߼�¼�������е�ƫ����

        /// <summary>
        /// ��ǰ���߼�¼�� XmlDocument ����
        /// </summary>
        public XmlDocument ReaderDom = null;    // Xmlװ��XmlDocument

        string m_strXml = "";    // ���߼�¼��
        /// <summary>
        /// ��ǰ���ڴ���Ķ��� XML ��¼���ַ�������
        /// </summary>
        public string Xml
        {
            get
            {
                return this.m_strXml;
            }
            set
            {
                this.m_strXml = value;
            }
        }

        /// <summary>
        /// ��ǰ���߼�¼��ʱ���
        /// </summary>
        public byte[] Timestamp = null; // ��ǰ���߼�¼��ʱ���

        /// <summary>
        /// ���캯��
        /// </summary>
        public ReaderStatis()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        internal override string GetOutputFileNamePrefix()
        {
            return Path.Combine(this.ReaderStatisForm.MainForm.DataDir, "~reader_statis");
        }

#if NO
        // TODO: ������� mem leak�������� Dispose() ��д
        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~ReaderStatis()      
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }


        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }


        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // ɾ����������ļ�
                if (this.OutputFileNames != null)
                {
                    Global.DeleteFiles(this.OutputFileNames);
                    this.OutputFileNames = null;
                }

                /*
                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;
                 * */
                try // 2008/6/26
                {
                    this.FreeResources();
                }
                catch
                {
                }

            }
            disposed = true;
        }

        public virtual void FreeResources()
        {

        }

        // ��ʼ��
        public virtual void OnInitial(object sender, StatisEventArgs e)
        {

        }

        // ��ʼ
        public virtual void OnBegin(object sender, StatisEventArgs e)
        {

        }

        // ÿһ��¼����
        public virtual void OnRecord(object sender, StatisEventArgs e)
        {

        }

        // ����
        public virtual void OnEnd(object sender, StatisEventArgs e)
        {

        }

        // ��ӡ���
        public virtual void OnPrint(object sender, StatisEventArgs e)
        {

        }

        public void ClearConsoleForPureTextOutputing()
        {
            Global.ClearForPureTextOutputing(this.Console);
        }

        public void WriteToConsole(string strText)
        {
            Global.WriteHtml(this.Console, strText);
        }

        public void WriteTextToConsole(string strText)
        {
            Global.WriteHtml(this.Console, HttpUtility.HtmlEncode(strText));
        }

        // ���һ���µ�����ļ���
        public string NewOutputFileName()
        {
            string strFileNamePrefix = this.ReaderStatisForm.MainForm.DataDir + "\\~reader_statis";

            string strFileName = strFileNamePrefix + "_" + this.m_nFileNameSeed.ToString() + ".html";

            this.m_nFileNameSeed++;

            this.OutputFileNames.Add(strFileName);

            return strFileName;
        }

        // ���ַ�������д���ı��ļ�
        public void WriteToOutputFile(string strFileName,
            string strText,
            Encoding encoding)
        {
            StreamWriter sw = new StreamWriter(strFileName,
                false,	// append
                encoding);
            sw.Write(strText);
            sw.Close();
        }

        // ɾ��һ������ļ�
        public void DeleteOutputFile(string strFileName)
        {
            int nIndex = this.OutputFileNames.IndexOf(strFileName);
            if (nIndex != -1)
                this.OutputFileNames.RemoveAt(nIndex);

            try
            {
                File.Delete(strFileName);
            }
            catch
            {
            }
        }
#endif
    }

    // 
    /// <summary>
    /// ���������ֱ�񡱵ļ���
    /// </summary>
    public class NamedStatisTableCollection : List<NamedStatisTable>
    {
        int m_nColumnsHint = 0;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="nColumnHint">��Ŀ����ʾ</param>
        public NamedStatisTableCollection(int nColumnHint)
        {
            this.m_nColumnsHint = nColumnHint;
        }

        /// <summary>
        /// ����һ����Ԫ������ֵ
        /// </summary>
        /// <param name="strTableName">�������</param>
        /// <param name="strEntry">������</param>
        /// <param name="nColumn">�к�</param>
        /// <param name="createValue">����ֵ</param>
        /// <param name="incValue">����ֵ</param>
        public void IncValue(
            string strTableName,
            string strEntry,
            int nColumn,
            Int64 createValue,
            Int64 incValue)
        {
            NamedStatisTable table = GetTable(strTableName, this.m_nColumnsHint);
            table.Table.IncValue(strEntry,
                nColumn, createValue, incValue);
        }

        /// <summary>
        /// ����һ����Ԫ���ַ���ֵ
        /// </summary>
        /// <param name="strTableName">�������</param>
        /// <param name="strEntry">������</param>
        /// <param name="nColumn">�к�</param>
        /// <param name="createValue">����ֵ</param>
        /// <param name="incValue">����ֵ</param>
        public void IncValue(
            string strTableName,
            string strEntry,
            int nColumn,
            string createValue,
            string incValue)
        {
            NamedStatisTable table = GetTable(strTableName, this.m_nColumnsHint);
            table.Table.IncValue(strEntry,
                nColumn, createValue, incValue);
        }

        // ���һ���ʵ��ı�����û���ҵ������Զ�����
        /// <summary>
        /// ���һ���ʵ��ı�������ǰ�����ڣ����Զ�����
        /// </summary>
        /// <param name="strTableName">�������</param>
        /// <param name="nColumnsHint">��Ŀ����ʾ</param>
        /// <returns>NamedStatisTable ���͵ı�����</returns>
        public NamedStatisTable GetTable(string strTableName,
            int nColumnsHint)
        {
            for (int i = 0; i < this.Count; i++)
            {
                NamedStatisTable table = this[i];


                if (table.Name == strTableName)
                    return table;
            }

            // û���ҵ�������һ���µı�
            NamedStatisTable newTable = new NamedStatisTable();
            newTable.Name = strTableName;
            newTable.Table = new Table(nColumnsHint);

            this.Add(newTable);
            return newTable;
        }

    }

    // 
    /// <summary>
    /// �������ֵı��
    /// </summary>
    public class NamedStatisTable
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Name = "";   // ����

        /// <summary>
        /// ���
        /// </summary>
        public Table Table = null;
    }

}
