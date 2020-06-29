
using System;
using System.Xml;
using System.Diagnostics;

using DigitalPlatform.Text;

namespace DigitalPlatform.Xml
{
	// ���Խڵ�
	public class AttrItem : ElementAttrBase
	{
		internal AttrItem(XmlEditor document)
		{
			this.m_document = document;
		}


		// ��ʼ�����ݣ���node�л�ȡ
		public virtual void InitialData(XmlNode node)
		{
			this.Name = node.Name;
			this.SetValue(node.Value);

			// Prefix��	NamespaceURI ������Ա���Ժ��ʼ��
			Prefix = null;
			this.m_strTempURI = node.NamespaceURI;
		}

		// parameters:
		//		style	��ʼ�������δʹ�á�
		public override void InitialVisualSpecial(Box boxTotal)
		{
			XmlText text = new XmlText ();
			text.Name = "TextOfAttrItem";
			text.container = boxTotal;
			Debug.Assert(this.m_paraValue1 != null,"m_paraValue���������ݲ���������Ϊnull");
			text.Text = this.GetValue();//this.m_paraValue;
			boxTotal.AddChildVisual(text);

			// �Ƿ������SetValue()
			this.m_paraValue1 = null;

			if (this.IsNamespace == true)
				text.Editable = false;

			this.m_strTempURI = null;
		}




		// ��node��ʼ����������¼�
		// parameters:
		//		style	�ݲ�ʹ��
		// return:
		//		-1  ����
		//		-2  ��;cancel
		//		0   �ɹ�
		public override int Initial(XmlNode node, 
			ItemAllocator allocator,
			object style,
            bool bConnect)
		{
			// ��ʼ�����ݣ���node�л�ȡ
			this.InitialData(node);

			// ������ǲ���namespace���ͽڵ�
			string strName = node.Name;
			if (strName.Length >= 5)
			{
				if (strName.Substring(0,5) == "xmlns")	// ��Сд����
				{
					if (strName.Length == 5)	// �������ԣ���������ǰ׺�ַ��������ֿռ�
					{
						this.IsNamespace = true;
						this.Prefix = "xmlns";
						this.LocalName = "";

						// ���Ҫ��.net dom���ݵĻ�
						//this.Prefix = "";	// ��Ӧ��"xmlns"������.net��domΪ"";
						//this.LocalName = "xmlns";	// ��Ӧ��"",����.net��domΪ"xmlns";
					}
					else if (strName[5] == ':')	// �������ԣ���������ǰ׺�ַ��������ֿռ�
					{
						this.IsNamespace = true;
						this.Prefix = "xmlns";
						this.LocalName = strName.Substring(6);
						if (this.LocalName == "")
						{
							throw(new Exception("ð�ź���Ӧ��������"));
							// return -1;
						}
					}
					else // ��ͨ����
					{
						this.IsNamespace = false;
					}
				}
			}

			// ������ͨ����
			if (this.IsNamespace == false)
			{
				int nRet = strName.IndexOf(":");
				if (nRet == -1) 
				{
					this.Prefix = "";
					this.LocalName = strName;
				}
				else 
				{
					this.Prefix = strName.Substring(0, nRet);
					this.LocalName = strName.Substring(nRet + 1);
				}
			}

			return 0;
		}


		// Ҫ��ǰreader������һ����������
		public int Initial(XmlReader reader)
		{
			this.Name = reader.Name;
			this.SetValue(reader.Value);

			// Prefix��	NamespaceURI ������Ա���Ժ��ʼ��
			Prefix = null;

			// ������ǲ���namespace���ͽڵ�
			string strName = reader.Name;
			if (strName.Length >= 5)
			{
				if (strName.Substring(0,5) == "xmlns")	// ��Сд����
				{
					if (strName.Length == 5)	// �������ԣ���������ǰ׺�ַ��������ֿռ�
					{
						this.IsNamespace = true;
						this.Prefix = "xmlns";
						this.LocalName = "";

						// ���Ҫ��.net dom���ݵĻ�
						//this.Prefix = "";	// ��Ӧ��"xmlns"������.net��domΪ"";
						//this.LocalName = "xmlns";	// ��Ӧ��"",����.net��domΪ"xmlns";
					}
					else if (strName[5] == ':')	// �������ԣ���������ǰ׺�ַ��������ֿռ�
					{
						this.IsNamespace = true;
						this.Prefix = "xmlns";
						this.LocalName = strName.Substring(6);
						if (this.LocalName == "")
						{
							throw(new Exception("ð�ź���Ӧ��������"));
							// return -1;
						}
					}
					else // ��ͨ����
					{
						this.IsNamespace = false;
					}
				}
			}

			// ������ͨ����
			if (this.IsNamespace == false)
			{
				int nRet = strName.IndexOf(":");
				if (nRet == -1) 
				{
					this.Prefix = "";
					this.LocalName = strName;
				}
				else 
				{
					this.Prefix = strName.Substring(0, nRet);
					this.LocalName = strName.Substring(nRet + 1);
				}
			}

			return 0;
		}


		internal override string GetOuterXml(ElementItem FragmentRoot)
		{
			return this.Name + "='" + StringUtil.GetXmlStringSimple(this.GetValue()) + "'";
		}

		public override string NamespaceURI 
		{
			get 
			{
				if (m_strTempURI != null)
					return m_strTempURI;	// �ڲ���ǰ��ժ����������

				string strURI = "";
				AttrItem namespaceAttr = null;

				// ����������ȱʡ�����ֿռ�
				if (this.Prefix == "")
					return "";

				if (this.Prefix == "xml")
					return "http://www.w3.org/XML/1998/namespace";

				// ����һ��ǰ׺�ַ���, �����Ԫ�ؿ�ʼ����, �����ǰ׺�ַ����������ﶨ���URI��
				// Ҳ����Ҫ�ҵ�xmlns:???=???���������Զ��󣬷�����namespaceAttr�����С�
				// �����ӷ��ص�namespaceAttr�����п����ҵ�����URI��Ϣ������Ϊ��ʹ���������㣬
				// ������Ҳֱ����strURI�����з��������е�URI
				// parameters:
				//		startItem	���element����
				//		strPrefix	Ҫ���ҵ�ǰ׺�ַ���
				//		strURI		[out]���ص�URI
				//		namespaceAttr	[out]���ص�AttrItem�ڵ����
				// return:
				//		ture	�ҵ�(strURI��namespaceAttr���з���ֵ)
				//		false	û���ҵ�
				bool bRet = ItemUtil.LocateNamespaceByPrefix(
					(ElementItem)this.parent,
					this.Prefix,
					out strURI,
					out namespaceAttr);
				if (bRet == false) 
				{
					if (this.Prefix == "")
						return "";
					return null;
				}
				return strURI;
			}
		}

	}


}
