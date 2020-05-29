using System;
using System.Xml;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Text;
using System.Text.RegularExpressions;

using DigitalPlatform;
using DigitalPlatform.Text;
using DigitalPlatform.Xml;


namespace DigitalPlatform.MarcDom
{
	/// <summary>
	/// ��һ��MARC��¼��ʾΪ��νṹ.
	/// </summary>
	public class MarcDocument
	{
		#region ��������

		public const char FLDEND  = (char)30;	// �ֶν�����
		public const char RECEND  = (char)29;	// ��¼������
		public const char SUBFLD  = (char)31;	// ���ֶ�ָʾ��

		public const int FLDNAME_LEN =        3;       // �ֶ�������
		public const int MAX_MARCREC_LEN =    100000;   // MARC��¼����󳤶�

		#endregion


		string	m_strMarc = "";	// MARC��¼��

		public MarcDocument()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string Marc
		{
			get 
			{
				return m_strMarc;
			}
			set 
			{
				m_strMarc = value;
			}

		}

		void Parse(FilterDocument filter)
		{

		}


		#region ����MARC��¼�����ڲ��ṹ�ľ�̬����

		// ���ֶ�/���ֶ����Ӽ�¼�еõ���һ�����ֶ����ݡ�
		// parameters:
		//		strMARC	���ڸ�ʽMARC��¼
		//		strFieldName	�ֶ���������Ϊ�ַ�
		//		strSubfieldName	���ֶ���������Ϊ1�ַ�
		// return:
		//		""	���ַ�������ʾû���ҵ�ָ�����ֶλ����ֶΡ�
		//		����	���ֶ����ݡ�ע�⣬�������ֶδ����ݣ����в����������ֶ�����
		static public string GetFirstSubfield(string strMARC,
			string strFieldName,
			string strSubfieldName)
		{
			string strField = "";
			string strNextFieldName = "";


		// return:
		//		-1	error
		//		0	not found
		//		1	found
			int nRet = GetField(strMARC,
                strFieldName,
				0,
				out strField,
				out strNextFieldName);

			if (nRet != 1)
				return "";

			string strSubfield = "";
			string strNextSubfieldName = "";

		// return:
		//		-1	error
		//		0	not found
		//		1	found
			nRet = GetSubfield(strField,
				ItemType.Field,
                strSubfieldName,
				0,
                out strSubfield,
                out strNextSubfieldName);
			if (strSubfield.Length < 1)
				return "";

			return strSubfield.Substring(1);
		}

		// ��Ԫ����!

		// �Ӽ�¼�еõ�һ���ֶ�
		// parameters:
		//		strMARC		���ڸ�ʽMARC��¼
		//		strFieldName	�ֶ��������������==null����ʾ���ȡ�����ֶ��еĵ�nIndex��
		//		nIndex		ͬ���ֶ��еĵڼ�������0��ʼ����(�����ֶ��У����0�����ʾͷ����)
		//		strField	[out]����ֶΡ������ֶ�������Ҫ���ֶ�ָʾ�����ֶ����ݡ��������ֶν�������
		//					ע��ͷ��������һ���ֶη��أ���ʱstrField�в������ֶ���������һ��ʼ����ͷ��������
		//		strNextFieldName	[out]˳�㷵�����ҵ����ֶ�����һ���ֶε�����
		// return:
		//		-1	����
		//		0	��ָ�����ֶ�û���ҵ�
		//		1	�ҵ����ҵ����ֶη�����strField������
		public static int GetField(string strMARC,
			string strFieldName,
			int nIndex,
			out string strField,
			out string strNextFieldName)
		{
//			LPTSTR p;
			int nFoundCount = 0;
			int nChars = 0;
			string strCurFldName;


			strField = null;
			strNextFieldName = null;


			if (strMARC == null) 
			{
				Debug.Assert(false, "strMARC��������Ϊnull");
				return -1;
			}


			if (strMARC.Length < 24)
				return -1;

			if (strFieldName != null) 
			{
				if (strFieldName.Length != 3) 
				{
					Debug.Assert(false, "�ֶ������ȱ���Ϊ3");	// �ֶ�������Ϊ3�ַ�
					return -1;
				}
			}
			else 
			{
				// ��ʾ�������ֶ���������nIndex����λ�ֶ�
			}

			strField = "";

			char ch;

			// ѭ�������ض����ֶ�

			// p = (LPTSTR)pszMARC;
			for(int i=0; i<strMARC.Length;) 
			{
				ch = strMARC[i];

				if (ch == RECEND)
					break;

				// ����m_strItemName
				if (i == 0) 
				{

					if ( (nIndex == 0 && strFieldName == null)	// ͷ����
						|| 
						(strFieldName != null 
						&& String.Compare("hdr",strFieldName, true) == 0) // ���ֶ���Ҫ�󣬲���Ҫ��ͷ����
						)
					{
						strField = strMARC.Substring(0, 24);

						// ȡstrNextFieldName
						strNextFieldName = GetNextFldName(strMARC,
							24);
						return 1;	// found
					}
					nChars = 24;

					if (strFieldName == null
						|| (strFieldName != null 
						&& "hdr" == strFieldName)
						)
					{
						nFoundCount ++;
					}

				}
				else 
				{
					nChars = DetectFldLens(strMARC, i);
					if (nChars < 3+1) 
					{
						strCurFldName = "???";	// ???
						goto SKIP;
					}
					Debug.Assert(nChars>=3+1, "");
					strCurFldName = strMARC.Substring(i, 3);
					if (strFieldName == null
						|| (strFieldName != null 
						&& strCurFldName == strFieldName)
						)
					{
						if (nIndex == nFoundCount) 
						{
							strField = strMARC.Substring(i, nChars-1);	// �������ֶν�����

							// ȡstrNextFieldName
							strNextFieldName = GetNextFldName(strMARC,
								i + nChars);

							/*
							if (i+nChars < strMARC.Length
								&& strMARC[i+nChars] != RECEND
								&& DetectFldLens(strMARC, i+nChars) >= 3 ) 
							{
								strNextFieldName = strMARC.Substring(i+nChars, 3);
								for(int j=0;j<strNextFieldName.Length;j++) 
								{
									char ch0 = strNextFieldName[j];
									if (ch0 == RECEND
										|| ch0 == SUBFLD 
										|| ch0 == FLDEND)
										strNextFieldName = strNextFieldName.Insert(j, "?").Remove(j+1, 1);

								}
							}
							else
								strNextFieldName = "";
							*/

							return 1;	// found
						}
						nFoundCount ++;
					}
				}

			SKIP:
				i += nChars;
			}
			return 0;	// not found
		}

		// nStart������Ϊ�ֶ����ַ�λ��
		// ������Ϊ����GetField()����
		static string GetNextFldName(string strMARC,
			int nStart)
		{
			string strNextFieldName = "";

			if (nStart < strMARC.Length
				&& strMARC[nStart] != RECEND
				&& DetectFldLens(strMARC, nStart) >= 3 ) 
			{
				strNextFieldName = strMARC.Substring(nStart, 3);
				for(int j=0;j<strNextFieldName.Length;j++) 
				{
					char ch0 = strNextFieldName[j];
					if (ch0 == RECEND
						|| ch0 == SUBFLD 
						|| ch0 == FLDEND)
						strNextFieldName = strNextFieldName.Insert(j, "?").Remove(j+1, 1);

				}
			}
			else
				strNextFieldName = "";

			return strNextFieldName;
		}

		// �����ֶο�ʼλ��̽�������ֶγ���
		// �����ֶεĳ���
		// ���ǰ�����¼�����������ڵĳ���
		public static int DetectFldLens(string strText,
			int nStart)
		{
			Debug.Assert(strText != null, "strText��������Ϊnull");
			int nChars = 0;
			// LPTSTR p = (LPTSTR)pFldStart;
			for(;nStart<strText.Length;nStart++) 
			{
				if (strText[nStart] == FLDEND) 
				{
					nChars ++;
					break;
				}
				nChars ++;
			}

			return nChars;
		}

		// �����ֶο�ʼλ��̽�������ֶγ���
		// �����ֶεĳ���(�ַ���������byte��)
		public static int DetectSubFldLens(string strField,
			int nStart)
		{
			Debug.Assert(strField != null, "strField��������Ϊnull");
			Debug.Assert(strField[nStart] == SUBFLD, "nStartλ�ñ�����һ�����ֶη���");
			Debug.Assert(nStart < strField.Length, "nStart����Խ��strField����");
			int nChars = 0;
			nStart ++;
			nChars ++;
			for(;nStart < strField.Length; nStart++) 
			{
				if (strField[nStart] == SUBFLD) 
				{
					break;
				}
				nChars ++;
			}

			return nChars;
		}


		// �õ�һ��Ƕ�׼�¼�е��ֶ�
		// parameters:
		//		strMARC		�ֶ���Ƕ�׵�MARC��¼����ʵ���Ȿ��һ��MARC�ֶΣ�������$1��������Ƕ���ֶΡ�����UNIMARC��410�ֶξ���������
		//		strFieldName	�ֶ��������������==null����ʾ���ȡ�����ֶ��еĵ�nIndex��
		//		nIndex		ͬ���ֶ��еĵڼ�������0��ʼ����(�����ֶ��У����0������ʾͷ�����ˣ���ΪMARCǶ�׼�¼���޷�����ͷ����)
		//		strField	[out]����ֶΡ������ֶ�������Ҫ���ֶ�ָʾ�����ֶ����ݡ��������ֶν�������
		//					ע��ͷ��������һ���ֶη��أ���ʱstrField�в������ֶ���������һ��ʼ����ͷ��������
		//		strNextFieldName	[out]˳�㷵�����ҵ����ֶ�����һ���ֶε�����
		// return:
		//		-1	����
		//		0	��ָ�����ֶ�û���ҵ�
		//		1	�ҵ����ҵ����ֶη�����strField������
		public static int GetNestedField(string strMARC,
			string strFieldName,
			int nIndex,
			out string strField,
			out string strNextFieldName)
		{
			// LPTSTR p;
			int nFoundCount = 0;
			int nChars = 0;
			string strFldName = "";

			strField = "";
			strNextFieldName = "";

			if (strMARC == null) 
			{
				Debug.Assert(false, "strMARC��������Ϊnull");
				return -1;
			}

			if (strMARC.Length < 5)
				return -1;

			if (strFieldName != null) 
			{
				if (strFieldName.Length != 3) 
				{
					Debug.Assert(false, "�ֶ������ȱ���Ϊ3");	// �ֶ�������Ϊ3�ַ�
					return -1;
				}
			}
			else 
			{
				// ��ʾ�������ֶ���������nIndex����λ�ֶ�
			}

			strField = "";

			// ѭ�������ض����ֶ�
			//p = (LPTSTR)pszMARC + 5;
			int nStart = 5;

			// �ҵ���һ��'$1'����
			for(;nStart<strMARC.Length;)
				// *p&&*p!=FLDEND;) 
			{
				if (strMARC[nStart] == FLDEND)
					break;
				if (strMARC[nStart] == SUBFLD
					&& strMARC[nStart+1] == '1' )
					goto FOUND;
				nStart ++;
			}
			return 0;

			FOUND:

				for(int i=nStart;i<strMARC.Length;) 
				{
					if (strMARC[i] == FLDEND)
						break;

					nChars = DetectNestedFldLens(strMARC, i);
					if (nChars < 2+3+1) 
					{
						strFldName = "???";
						goto SKIP;
					}
					Debug.Assert(nChars>=2+3+1, "error");
					strFldName = strMARC.Substring(i + 2, 3);

					if (strFieldName == null
						|| (strFieldName != null
						&& strFldName == strFieldName) )
					{
						if (nIndex == nFoundCount) 
						{
							strField = strMARC.Substring(i + 2, nChars - 2);

							if (i+nChars < strMARC.Length
								&& strMARC[i+nChars] != RECEND
								&& DetectFldLens(strMARC, i+nChars) >= 2+3)
								strNextFieldName = strMARC.Substring(i+nChars+2, 3);
							else
								strNextFieldName = "";

							return 1;	// found
						}
						nFoundCount ++;
					}

				SKIP:
					i += nChars;
				}
			return 0;	// not found
		}

		// Ƕ���ֶΣ������ֶο�ʼλ��̽�������ֶγ���
		// �����ֶεĳ���(�ַ���������byte��)
		static int DetectNestedFldLens(string strMARC, int nStart)
		{
			Debug.Assert(strMARC != null , "strMARC��������Ϊnull");

			if (nStart >= strMARC.Length) 
			{
				Debug.Assert(false, "nStart����ֵ����strMARC���ݳ��ȷ�Χ");
				return 0;
			}

			if (strMARC[nStart] != SUBFLD || strMARC[nStart+1] != '1') 
			{
				Debug.Assert(false, "������$1��ͷ��λ�õ��ñ�����");
			}

			int i = nStart + 1;
			for(;i<strMARC.Length;i++)
			{
				if (strMARC[i] == SUBFLD
					&& strMARC[i + 1] == '1')
					break;
			}

			return i-nStart;
		}


		// ���ֶ��еõ����ֶ���
		// parameters:
		//		strField	�ֶΡ����а����ֶ�������Ҫ��ָʾ�����ֶ����ݡ�Ҳ���ǵ���GetField()�������õ����ֶΡ�
		//		nIndex	���ֶ�����š���0��ʼ������
		//		strGroup	[out]�õ������ֶ��顣���а����������ֶ����ݡ�
		// return:
		//		-1	����
		//		0	��ָ�������ֶ���û���ҵ�
		//		1	�ҵ����ҵ������ֶ��鷵����strGroup������
		public static int GetGroup(string strField,
			int nIndex,
			out string strGroup)
		{
			Debug.Assert(strField != null ,"strField��������Ϊnull");

			Debug.Assert(nIndex>=0, "nIndex��������>=0");

			strGroup = "";

			int nLen = strField.Length;
			if (nLen <= 5) 
			{
				return 0;
			}

			// LPCTSTR lpStart,lpStartSave;
			// LPTSTR pp;
			//int l;
			string strZzd = "a";

			// char zzd[3];

			/*
			zzd[0]=SUBFLD;
			zzd[1]='a';
			zzd[2]=0;
			*/
			strZzd = strZzd.Insert(0, new String(SUBFLD, 1));
	
			// lpStart = pszField;

			// �ҵ����
			int nStart = 5;
			int nPos;
			for(int i=0;;i++) 
			{
				nPos = strField.IndexOf(strZzd, nStart);
				if (nPos == -1)
					return 0;

				/*
				pp = _tcsstr(lpStart,zzd);
				if (pp==NULL) 
				{
					return 0; // not found
				}
				*/

				if (i>=nIndex) 
				{
					nStart = nPos;
					break;
				}
				nStart = nPos + 1;
				// lpStart = pp + 1;
			}

			//lpStart = pp;
			//lpStartSave = pp;
			//lpStart ++;
			int nStartSave = nStart;
			nStart ++;

			nPos = strField.IndexOf(strZzd, nStart);
			if (nPos == -1) 
			{
				// ��û�����ֶ���
				strGroup = strField.Substring(nStartSave);
				return 1;
			}
			else 
			{
				strGroup = strField.Substring(nStartSave, nPos - nStartSave);
				return 1;
			}
			/*
			pp = _tcsstr(lpStart,zzd);
			if (pp == NULL) 
			{	// ��û�����ֶ���
				l = _tcslen(lpStartSave);
				MemCpyToString(lpStartSave, l, strGroup);
				return strGroup.GetLength();
			}
			else 
			{
				l = pp - lpStartSave;	// ע�⣬���ַ���
				MemCpyToString(lpStartSave, l, strGroup);
				return strGroup.GetLength();
			}
			*/

			//	ASSERT(0);
			//   return 0;
		}

		// ���ֶλ����ֶ����еõ�һ�����ֶ�
		// parameters:
		//		strText		�ֶ����ݣ��������ֶ������ݡ�
		//		textType	��ʾstrText�а��������ֶ����ݻ��������ݡ���ΪItemType.Field����ʾstrText������Ϊ�ֶΣ���ΪItemType.Group����ʾstrText������Ϊ���ֶ��顣
		//		strSubfieldName	���ֶ���������Ϊ1λ�ַ������==null����ʾ�������ֶ�
		//					��ʽΪ'a'�����ġ�
		//		nIndex			��Ҫ���ͬ�����ֶ��еĵڼ�������0��ʼ���㡣
		//		strSubfield		[out]������ֶΡ����ֶ���(1�ַ�)�����ֶ����ݡ�
		//		strNextSubfieldName	[out]��һ�����ֶε����֣�����һ���ַ�
		// return:
		//		-1	����
		//		0	��ָ�������ֶ�û���ҵ�
		//		1	�ҵ����ҵ������ֶη�����strSubfield������
		public static int GetSubfield(string strText,
			ItemType textType,
			string strSubfieldName,
			int nIndex,
			out string strSubfield,
			out string strNextSubfieldName)
		{

			Debug.Assert(textType == ItemType.Field 
				|| textType == ItemType.Group,
				"textTypeֻ��Ϊ ItemType.Field/ItemType.Group֮һ");
			// LPCTSTR p;
			// int nTextLen;
			int nFoundCount = 0;
			int i;

			strSubfield = "";
			strNextSubfieldName = "";

			Debug.Assert(strText != null, "strText��������Ϊnull");
			//nFieldLen = strText.Length;

			if (textType == ItemType.Field)
			{
				if (strText.Length < 3)
					return -1;	// �ֶ����ݳ��Ȳ���3�ַ�

				if (strText.Length == 3) 
					return 0;	// �������κ����ֶ�����
			}
			if (textType == ItemType.Group)
			{
				if (strText.Length < 2)
					return -1;	// �ֶ����ݳ��Ȳ���3�ַ�

				if (strText.Length == 1) 
					return 0;	// �������κ����ֶ�����
			}


			if (strSubfieldName != null) 
			{
				if (strSubfieldName.Length != 1) 
				{
					Debug.Assert(false, "strSubfieldName������ֵ������null�������Ϊ1�ַ�");
					return -1;
				}
			}

			// p = pszField + 3;
			// �ҵ���һ�����ֶη���
			for(i=0;i<strText.Length;i++) 
			{
				if (strText[i] == SUBFLD)
					goto FOUND;
			}
			return 0;

			FOUND:
				// ƥ��
				for(;i<strText.Length;i++) 
				{
					if (strText[i] == SUBFLD) 
					{
						if (i + 1 >= strText.Length)
							return 0;	// not found

						if ( strSubfieldName == null
							|| 
							(strSubfieldName != null
							&& strText[i + 1] == strSubfieldName[0]) 
							)
						{
							if ( nFoundCount == nIndex) 
							{
								int nChars = DetectSubFldLens(strText, i);
								strSubfield = strText.Substring(i+1, nChars-1);

								// ȡ��һ�����ֶ���
									if (i + nChars < strText.Length
										&& strText[i + nChars] == SUBFLD
										&& DetectFldLens(strText, i+nChars) >= 2)
									{
										strNextSubfieldName = strText.Substring(i+nChars+1, 1);
									}
									else
										strNextSubfieldName = "";

								return 1;
							}

							nFoundCount ++;
						}

					}

				}

			return 0;
		}


		// �滻��¼�е��ֶ����ݡ�
		// ���ڼ�¼����ͬ���ֶ�(��nIndex��)������ҵ������滻�����û���ҵ���
		// ����˳��λ�ò���һ�����ֶΡ�
		// parameters:
		//		strMARC		[in][out]MARC��¼��
		//		strFieldName	Ҫ�滻���ֶε��������Ϊnull����""�����ʾ�����ֶ������ΪnIndex�е��Ǹ����滻
		//		nIndex		Ҫ�滻���ֶε�������š����Ϊ-1����ʼ��Ϊ�ڼ�¼��׷�����ֶ����ݡ�
		//		strField	Ҫ�滻�ɵ����ֶ����ݡ������ֶ�������Ҫ���ֶ�ָʾ�����ֶ����ݡ�����ζ�ţ����������滻һ���ֶε����ݣ�Ҳ�����滻�����ֶ�����ָʾ�����֡�
		// return:
		//		-1	����
		//		0	û���ҵ�ָ�����ֶΣ���˽�strField���ݲ��뵽�ʵ�λ���ˡ�
		//		1	�ҵ���ָ�����ֶΣ�����Ҳ�ɹ���strField�滻���ˡ�
		public static int ReplaceField(
			ref string strMARC,
			string strFieldName,
			int nIndex,
			string strField)
		{
			int nInsertOffs = 24;
			int nStartOffs = 24;
			int nLen = 0;
			int nChars = 0;
			string strFldName;
			int nFoundCount = 0;
			bool bFound = false;

			if (strMARC.Length < 24)
				return -1;

			Debug.Assert(strFieldName != null, "");

			/*
			if (strFieldName.Length != 3) 
			{
				Debug.Assert(false, "strFieldName�������ݱ���Ϊ3�ַ�");
				return -1;
			}
			*/
			if (strFieldName != null) 
			{
				if (strFieldName.Length != 3) 
				{
					Debug.Assert(false, "�ֶ������ȱ���Ϊ3");	// �ֶ�������Ϊ3�ַ�
					return -1;
				}
			}
			else 
			{
				// ��ʾ�������ֶ���������nIndex����λ�ֶ�
			}

			bool bIsHeader = false;

			if (strFieldName == null || strFieldName == "") 
			{
				if (nIndex == 0)
					bIsHeader = true;
			}
			else 
			{
				if (strFieldName == "hdr")
					bIsHeader = true;
			}

			// ���strField������ȷ��
			if (bIsHeader == true) 
			{
				if (strField == null
					|| strField == "")
				{
					Debug.Assert(false,"ͷ��������ֻ���滻������ɾ��");
					return -1;
				}

				if (strField.Length != 24) 
				{
					Debug.Assert(false,"strField�������滻ͷ���������ݣ�����Ϊ24�ַ�");
					return -1;
				}
			}


			// �����������ֶ���������Ƿ����ֶν����������û�У���׷��һ����
			// ͷ�������ݲ����˴���
			if (strField != null && strField.Length > 0
				&& bIsHeader == false) 
			{
				if (strField[strField.Length - 1] != FLDEND)
					strField += FLDEND;
			}

			bool bInsertOffsOK = false;

			// ѭ�������ض����ֶ�
			//p = (LPTSTR)(LPCTSTR)strMARC;
			for(int i=0; i<strMARC.Length;) 
			{
				if (strMARC[i] == RECEND)
					break;


				if (i== 0) 
				{
					if ( (nIndex == 0 && strFieldName == null)	// ͷ����
						|| 
						(strFieldName != null 
						&& String.Compare("hdr",strFieldName, true) == 0) // ���ֶ���Ҫ�󣬲���Ҫ��ͷ����
						)
					{
						if (strField == null
							|| strField == "")
						{
							Debug.Assert(false,"ͷ��������ֻ���滻������ɾ��");
							return -1;
						}

						if (strField.Length != 24) 
						{
							Debug.Assert(false,"strField�������滻ͷ���������ݣ�����Ϊ24�ַ�");
							return -1;
						}

						strMARC = strMARC.Remove(0, 24);	// ɾ��ԭ������

						strMARC = strMARC.Insert(0, strField);	// �����µ�����

						return 1;	// found
					}

					nChars = 24;
					strFldName = "hdr";


					if (strFieldName == null
						|| (strFieldName != null 
						&& "hdr" == strFieldName)
						)
					{
						nFoundCount ++;
					}

				}
				else 
				{
					nChars = DetectFldLens(strMARC, i);
					if (nChars < 3+1) 
					{
						strFldName = "???";
						goto SKIP;
					}
					Debug.Assert(nChars>=3+1, "̽�⵽�ֶγ��Ȳ���С��3+1");
					strFldName = strMARC.Substring(i, 3);
					// MemCpyToString(p, 3, strFldName);
				}

			SKIP:

				if (strFieldName == null
					|| (strFieldName != null 
					&& strFldName == strFieldName)
					)
				{
					if (nIndex == nFoundCount) 
					{
						nStartOffs = i;
						nLen = nChars;
						bFound = true;
						goto FOUND;
					}
					nFoundCount ++;
				}

				// ��취���潫��Ҫ�õĲ���λ��
				if (strFieldName != null && strFieldName != ""
					&& strFldName != "hdr"
					&& bInsertOffsOK == false) 
				{
					if (String.Compare(strFldName,strFieldName, false) > 0) 
					{
						nInsertOffs = Math.Max(i, 24);
						bInsertOffsOK = true;	// �Ժ�������
					}
					else 
					{
						nInsertOffs = Math.Max(i+nChars, 24);
					}
				}

				i += nChars;
			}

			nStartOffs = nInsertOffs;
			nLen = 0;

			if (strField == null || strField.Length == 0)	// ʵ��Ϊɾ��Ҫ��
				return 0;
			FOUND:
				if (nLen > 0)
					strMARC = strMARC.Remove(nStartOffs, nLen);	// ɾ��ԭ������

			strMARC = strMARC.Insert(nStartOffs, strField);	// �����µ�����

			if (bFound == true)
				return 1;

			return 0;
		}


		// �滻�ֶ��е����ֶΡ�
		// parameters:
		//		strField	[in,out]���滻���ֶ�
		//		strSubfieldName	Ҫ�滻�����ֶε���������Ϊ1�ַ������==null����ʾ�������ֶ�
		//					��ʽΪ'a'�����ġ�
		//		nIndex		Ҫ�滻�����ֶ�������š����Ϊ-1����ʼ��Ϊ���ֶ���׷�������ֶ����ݡ�
		//		strSubfield	Ҫ�滻�ɵ������ֶΡ�ע�⣬���е�һ�ַ�Ϊ���ֶ���������Ϊ���ֶ�����
		// return:
		//		-1	����
		//		0	ָ�������ֶ�û���ҵ�����˽�strSubfieldzhogn�����ݲ��뵽�ʵ��ط��ˡ�
		//		1	�ҵ���ָ�����ֶΣ�����Ҳ�ɹ���strSubfield�����滻���ˡ�
		public static int ReplaceSubfield(
			ref string strField,
			string strSubfieldName,
			int nIndex,
			string strSubfield)
		{
			if (strField.Length <= 1)
				return -1;

			if (strSubfieldName != null) 
			{
				if (strSubfieldName.Length != 1) 
				{
					Debug.Assert(false, "strSubfieldName������ֵ������null�������Ϊ1�ַ�");
					return -1;
				}
			}

			if (nIndex < 0)
				goto APPEND;	// ׷�������ֶ�

			int i = 0;
			int nFoundCount = 0;

			// �ҵ���һ�����ֶη���
			for(i=0;i<strField.Length;i++) 
			{
				if (strField[i] == SUBFLD)
					goto FOUNDHEAD;
			}
			goto APPEND;
			FOUNDHEAD:
				// ƥ��
				for(;i<strField.Length;i++) 
				{
					if (strField[i] == SUBFLD) 
					{
						if (i + 1 >= strField.Length)
							goto APPEND;	// not found

						if ( strSubfieldName == null
							|| 
							(strSubfieldName != null
							&& strField[i + 1] == strSubfieldName[0]) 
							)
						{
							if ( nFoundCount == nIndex) 
							{
								int nChars = DetectSubFldLens(strField, i);

								// ȥ��ԭ��������
								strField = strField.Remove(i, nChars);
								if (strSubfield != null
									&& strSubfield != "") 
								{
									// ����������
									strField = strField.Insert(i, new string(SUBFLD,1) + strSubfield);
								}
								return 1;
							}

							nFoundCount ++;
						}

					}

				} // end



			APPEND:
				strField +=  new string(SUBFLD,1) + strSubfield;

			return 0;	// inserted

		}

		#endregion



	}



}
