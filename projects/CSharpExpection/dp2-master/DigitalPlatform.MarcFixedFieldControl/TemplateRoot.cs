using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

using DigitalPlatform.Xml;

namespace DigitalPlatform.Marc
{

    // ģ�������
	public class TemplateRoot
	{
		public MarcFixedFieldControl control = null;

		private XmlNode m_fieldNode = null;
		private string m_strLang = null;

		private string m_strName = null;
		private string m_strLabel = null;

		public ArrayList Lines = new ArrayList();//null;


		public TemplateRoot(MarcFixedFieldControl ctrl)
		{
			this.control = ctrl;
		
		}


/*
	<Field name='###' length='24' mandatory='yes' repeatable='no'>
		<Property>
			<Label xml:lang='en'>RECORD IDENTIFIER</Label>
			<Label xml:lang='cn'>ͷ����</Label>
			<Help xml:lang='cn'>������Ϣ</Help>
		</Property>
		<Char name='0/5'>
		</Char>
		....
	</Field>
*/
		// ��ʼ��TemplateRoot����
		// parameters:
		//		fieldNode	Field�ڵ�
		//		strLang	���԰汾
		//		strError	������Ϣ
		// return:
		//		-1	ʧ��
		//		0	���Ƕ����ֶ�
		//		1	�ɹ�
		public int Initial(XmlNode node,
			string strLang,
			out string strError)
		{
			strError = "";

			Debug.Assert(node!=null,"���ô���node����Ϊnull");

			this.m_fieldNode  = node;
			this.m_strLang = strLang;


			this.m_strName = DomUtil.GetAttr(node,"name");
			if (this.m_strName == "")
			{
				strError = "<" + node.Name + ">Ԫ�ص�name���Կ��ܲ����ڻ���ֵΪ�գ������ļ����Ϸ���";
				Debug.Assert(false,strError);
				return -1;					
			}

			XmlNode propertyNode = node.SelectSingleNode("Property");
			if (propertyNode == null)
			{
                // TODO������Ҫ��ʾ����ϸ����Ϣ
                XmlNode temp = node.Clone();
                while(temp.ChildNodes.Count > 0)
                    temp.RemoveChild(temp.ChildNodes[0]);
                strError = "�� " + temp.OuterXml + " Ԫ���¼�δ����<Property>Ԫ��,�����ļ����Ϸ�.";
                // strError = "��<" + node.Name + ">Ԫ���¼�δ����<Property>Ԫ��,�����ļ����Ϸ�.";
				// Debug.Assert(false,strError);
				return -1;
			}

			XmlNodeList charList = node.SelectNodes("Char");
			// û��<Char>Ԫ�أ������ֳ��ֶλ����ֶ�
			if (charList.Count == 0)
				return 0;
#if NO
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
			nsmgr.AddNamespace("xml", Ns.xml);
			XmlNode labelNode = propertyNode.SelectSingleNode("Label[@xml:lang='" + strLang + "']",nsmgr);
			if (labelNode == null)
			{
				this.m_strLabel = "????????";
				Debug.Assert(false,"����Ϊ'" + this.m_strName + "'��<" + node.Name +">Ԫ��δ����Label��'" + strLang + "'���԰汾��ֵ");
			}
			this.m_strLabel = DomUtil.GetNodeText(labelNode);

#endif
            // ��һ��Ԫ�ص��¼��Ķ��<strElementName>Ԫ����, ��ȡ���Է��ϵ�XmlNode��InnerText
            // parameters:
            //      bReturnFirstNode    ����Ҳ���������Եģ��Ƿ񷵻ص�һ��<strElementName>
            this.m_strLabel = DomUtil.GetXmlLangedNodeText(
        strLang,
        propertyNode,
        "Label",
        true);
			
			if (this.Lines == null)
				this.Lines = new ArrayList();
			else 
				this.Lines.Clear();
			foreach(XmlNode charNode in charList)
			{
				TemplateLine line = new TemplateLine(this,
					charNode,
					strLang);
				this.Lines.Add(line);
			}

			return 1;
		}


		public string GetValue()
		{
			string strValue = "";

			this.Lines.Sort();

			for(int i=0;i<this.Lines.Count;i++)
			{
				TemplateLine line = (TemplateLine)this.Lines[i];
				strValue += line.TextBox_value.Text;
			}
			return strValue;
		}

        public string AdditionalValue = "";

		public int SetValue(string strValue)
		{

			int nTotalLength = 0;
			for (int i=0;i<this.Lines.Count;i++)
			{
				nTotalLength += ((TemplateLine)(this.Lines[i])).m_nValueLength;
			}

			if (strValue == null)
				strValue = "";

            // �����ַ�
			if (strValue.Length < nTotalLength)
				strValue = strValue + new string(' ',nTotalLength-strValue.Length);

            // ������ַ�
            if (strValue.Length > nTotalLength)
                AdditionalValue = strValue.Substring(nTotalLength);
            else
                AdditionalValue = "";

			for(int i=0;i<this.Lines.Count;i++)
			{
				TemplateLine line = (TemplateLine)this.Lines[i];
				line.m_strValue = strValue.Substring(line.m_nStart,line.m_nValueLength);
				line.TextBox_value.Text = line.m_strValue;
			}

			return 0;
		}

	}
}
