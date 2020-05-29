using System;

using DigitalPlatform.Library;
using DigitalPlatform.rms.Client;
using DigitalPlatform.Xml;

namespace dp2Batch
{
	/// <summary>
	/// Summary description for Batch.
	/// </summary>
	public class Batch
	{
		public ApplicationInfo ap = null;
		public MainForm MainForm = null;	// ����

        public SearchPanel SearchPanel = null;

		public string ServerUrl = "";

		private string	m_strXmlRecord = "";
		private bool	m_bXmlRecordChanged = false;	// XML��¼�Ƿ񱻽ű��ı�


		public string	MarcSyntax = "";

		private string	m_strMarcRecord = "";	// MARC��¼�塣�ⲿ�ӿ���MarcRecord {get;set}
		private bool	m_bMarcRecordChanged = false;	// MARC��¼�Ƿ񱻽ű��ı�

		/*
		private string	m_strMarcOutputRecord = "";	// ���������MARC��¼�塣�ⲿ�ӿ���MarcOutputRecord {get;set}
		private bool	m_bMarcOutputRecordChanged = false;	// ���������MARC��¼�Ƿ񱻽ű��ı�
		*/


		public byte[] TimeStamp = null;	// ʱ���
		public string RecPath = "";	// ��¼·��
		public int RecIndex = 0;	// ��ǰ��¼��һ���е���š���0��ʼ����
		public string ProjectDir = "";
		public string DbPath = "";	// ���ݿ�·��

		public bool SkipInput = false;

		public Batch()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public virtual void OnInitial(object sender, BatchEventArgs e)
		{

		}

		public virtual void OnBegin(object sender, BatchEventArgs e)
		{

		}

		public virtual void Outputing(object sender, BatchEventArgs e)
		{

		}

		public virtual void Inputing(object sender, BatchEventArgs e)
		{

		}

		public virtual void Inputed(object sender, BatchEventArgs e)
		{

		}

		public virtual void OnEnd(object sender, BatchEventArgs e)
		{

		}

		public virtual void OnPrint(object sender, BatchEventArgs e)
		{

		}


		// MARC��¼��
		public string MarcRecord 
		{
			get 
			{
				return m_strMarcRecord;
			}
			set 
			{
				m_strMarcRecord = value;
				m_bMarcRecordChanged = true;
			}
		}

		// MARC��¼���Ƿ񱻸ı��
		public bool MarcRecordChanged
		{
			get 
			{
				return m_bMarcRecordChanged;
			}
			set 
			{
				m_bMarcRecordChanged = value;
			}
		}

		// XML��¼��
		public string XmlRecord 
		{
			get 
			{
				return m_strXmlRecord;
			}
			set 
			{
				m_strXmlRecord = value;
				m_bXmlRecordChanged = true;
			}
		}

		// Xml��¼���Ƿ񱻸ı��
		public bool XmlRecordChanged
		{
			get 
			{
				return m_bXmlRecordChanged;
			}
			set 
			{
				m_bXmlRecordChanged = value;
			}
		}

		/*
		// ���������MARC��¼��
		public string MarcOutputRecord 
		{
			get 
			{
				return m_strMarcOutputRecord;
			}
			set 
			{
				m_strMarcOutputRecord = value;
				m_bMarcOutputRecordChanged = true;
			}
		}

		// ���������MARC��¼���Ƿ񱻸ı��
		public bool MarcOutputRecordChanged
		{
			get 
			{
				return m_bMarcOutputRecordChanged;
			}
			set 
			{
				m_bMarcOutputRecordChanged = value;
			}
		}
		*/

		public string RecFullPath
		{
			get 
			{
				return this.ServerUrl + "?" + this.RecPath;
			}
			set 
			{
				ResPath respath = new ResPath(value);

				this.ServerUrl = respath.Url;
				this.RecPath = respath.Path;
			}
		}

	}

	public enum ContinueType
	{
		Yes = 0,
		/*
		SkipBegin = 1,
		SkipMiddle = 2,
		SkipBeginMiddle = 3,
		*/
		SkipAll = 4,
	}

	public class BatchEventArgs : EventArgs
	{
		public ContinueType	Continue = ContinueType.Yes;	// �Ƿ����ѭ��
	}

}
