using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Web;

using DigitalPlatform.Xml;

// 2013/3/16 ��� XML ע��

namespace dp2Circulation
{
    /// <summary>
    /// XmlStatisForm (XMLͳ�ƴ�) ͳ�Ʒ�����������
    /// </summary>
    public class XmlStatis : StatisHostBase
    {
        /// <summary>
        /// �����ļ���
        /// </summary>
        public string InputFilename = "";

        /// <summary>
        /// �������������� XmlStatisForm (XMLͳ�ƴ�)
        /// </summary>
        public XmlStatisForm XmlStatisForm = null;	// ����

        /// <summary>
        /// ��ǰ���ڴ���� XML ��¼ �������е��±ꡣ�� 0 ��ʼ���������Ϊ -1����ʾ��δ��ʼ����
        /// </summary>
        public long CurrentRecordIndex = -1; // ��ǰXML��¼�������е�ƫ����


        /// <summary>
        /// ��ǰ���ڴ���� XML ��¼��XmlDocument ����
        /// </summary>
        public XmlDocument RecordDom = null;    // Xmlװ��XmlDocument

        string m_strXml = "";    // XML��¼��

        /// <summary>
        /// ��ǰ���ڴ���� XML ��¼���ַ�������
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

        internal override string GetOutputFileNamePrefix()
        {
            return Path.Combine(this.XmlStatisForm.MainForm.DataDir, "~xml_statis");
        }

    }
}
