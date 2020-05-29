using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

using DigitalPlatform.Text;

namespace DigitalPlatform.Xml
{
	public class NamespaceItemCollection : ArrayList
	{
		// ����һ�������������ַ���
		// �������Ϊ�գ�����""�����򣬴������������⣬�����Զ�����һ���ո�
		public string MakeAttrString()
		{
			if (this.Count == 0)
				return "";
			string strResult = "";
			foreach(NamespaceItem item in this)
			{
				strResult += " " + item.OuterXml;
			}

			return strResult + " ";
		}

		// ��֧�汾�����ʺ��ⲿֱ�ӵ��á�
		// ��ָ��λ������(����)������Ԫ�ص����Զ���չ��״̬�������ֱ������
		// �����Լ���DOM������ϵ���Ѽ����ֿռ���Ϣ��
		// �ռ�һ��Ԫ�ؽڵ�����(�Ϸ�)�����ֿռ���Ϣ��
		// ����element����
		// parameters:
		//		element ��׼Ԫ��
		// return:
		//		�������ֿռ���Ϣ����
		public static NamespaceItemCollection GatherOuterNamespacesByNativeDom(
			ElementItem element)
		{
			NamespaceItemCollection nsColl = new NamespaceItemCollection();

			ElementItem current = (ElementItem)element;
			while(true)
			{
				if (current == null)
					break;


				if (current.m_attrsExpand == ExpandStyle.Collapse) 
				{
					/*
					// Ϊ��ö�ٱ������ԣ����ò�չ������������󣬵�����ʱû���������صĹ���
					current.GetControl().ExpandChild(current, 
						ExpandStyle.Expand,
						current.m_childrenExpand, 
						true);
					Debug.Assert( current.m_attrsExpand == ExpandStyle.Expand, 
						"չ����m_attrsExpandӦ��Ϊtrue");
					*/
					Debug.Assert(false, "���ñ��������ˣ�Ӧ��ȷ����Ҫ��λ����������·���У�ÿ��Ԫ�ص����Լ��϶��Ǵ���չ��״̬��");
				}

				foreach(AttrItem attr in current.attrs)
				{
					if (attr.IsNamespace == false)
						continue;

					nsColl.Add(attr.LocalName, attr.GetValue(), true);	// ֻҪprefix�ؾͲ�����
				}

				current = (ElementItem)current.parent;
			}

			return nsColl;
		}


		// ��֧�汾�����ʺ��ⲿֱ�ӵ��á�
		// ��ָ��λ������(����)����һ������Ԫ�ص�����������״̬��������û��
		// ���������Լ���DOM������ϵ���Ѽ����ֿռ���Ϣ��ֻ��ģ��һ��XML�ֲ��ַ���������
		// .net DOM�������Ѽ����ֿռ���Ϣ
		public static NamespaceItemCollection GatherOuterNamespacesByDotNetDom(
			ElementItem element)
		{
			string strXml = "";

			NamespaceItemCollection nsColl = new NamespaceItemCollection();

			ElementItem current = (ElementItem)element;
			while(true)
			{
				if (current == null 
					|| current is VirtualRootItem)
					break;

				strXml = "<" + current.Name + current.GetAttrsXml() + ">" + strXml + "</" + current.Name + ">";

				current = (ElementItem)current.parent;
			}

			if (strXml == "")
				return nsColl;

			XmlDocument dom  = new XmlDocument();

			try 
			{
				dom.LoadXml(strXml);
			}
			catch (Exception ex)
			{
				throw (new Exception("GatherOuterNamespacesByDotNetDom()����ģ��xml�������: " + ex.Message));
			}

			// ��ȷ�����
			XmlNode currentNode = dom.DocumentElement;
			while(true)
			{
				if (currentNode.ChildNodes.Count == 0)
					break;
				currentNode = currentNode.ChildNodes[0];
			}

			Debug.Assert(currentNode != null, "");

			// ��ʼ�Ѽ���Ϣ
			while(true)
			{
				if (currentNode == null)
					break;

				foreach(XmlAttribute attr in currentNode.Attributes)
				{
					if (attr.Prefix != "xmlns" && attr.LocalName != "xmlns")
						continue;

					if (attr.LocalName == "xmlns")
						nsColl.Add("", attr.Value, true);	// ֻҪprefix�ؾͲ�����
					else
						nsColl.Add(attr.LocalName, attr.Value, true);	// ֻҪprefix�ؾͲ�����
				}

				currentNode = currentNode.ParentNode;
				if (currentNode is XmlDocument)	// �������㵽��������
					break;
			}

			return nsColl;
		}

		// �Զ��жϵİ汾���ʺϱ������á�
		// �ռ�һ��Ԫ�ؽڵ�����(�Ϸ�)�����ֿռ���Ϣ��
		// ����element����
		// parameters:
		//		element ��׼Ԫ��
		// return:
		//		�������ֿռ���Ϣ����
		public static NamespaceItemCollection GatherOuterNamespaces(
			ElementItem element)
		{
			bool bFound = false;	// �Ƿ���һ�����ϵ�Ԫ�����Դ�������״̬
			ElementItem current = (ElementItem)element;
			while(true)
			{
				if (current == null)
					break;

				if (current.m_attrsExpand == ExpandStyle.Collapse) 
				{
					bFound = true;
					break;
				}

				current = (ElementItem)current.parent;
			}

			if (bFound == true)
				return GatherOuterNamespacesByDotNetDom(element);
			else
				return GatherOuterNamespacesByNativeDom(element);
		}

	

		// ����һ����Ԫ��
		// ������������prefix��URI���Ѿ��е�����Ԫ�ز��أ���������ظ��ģ��������
		// parameters:
		//		bCheckPrefixDup	�Ƿ��prefixҲ�������ء����==true����prefix�أ�����URI����Ҳ���ü���
		// return:
		//		true	prefix��URI��ȫ��ͬ��Ԫ���Ѵ��ڣ� δ������Ԫ��
		//		false	û�������ظ��������Ԫ�سɹ�����
		public bool Add(string strPrefix,
			string strURI,
			bool bCheckPrefixDup)
		{
			Debug.Assert(strPrefix != null,"strPrefix��������Ϊnull");

			Debug.Assert(strURI != "" && strURI != null,"strURI��������Ϊnull");

			for(int i=0;i<this.Count;i++)
			{
				NamespaceItem item = (NamespaceItem)(this[i]);
				if (bCheckPrefixDup == true
					&& item.Prefix == strPrefix)
					return true;

				if (item.Prefix == strPrefix
					&& item.URI == strURI)
					return true;
			}

			NamespaceItem newItem = new NamespaceItem();
			newItem.Prefix = strPrefix;
			newItem.URI = strURI;
			this.Add(newItem);
			return false;
		}

	}

	public class NamespaceItem
	{
		public string Prefix = "";
		public string URI = "";

		// ��������ֿռ���Ϣ��ԭΪ���ԵĻ�����������ʲô?
		public string AttrName
		{
			get 
			{
				if (Prefix == "")   //xmlns
					return "xmlns";
				else
					return "xmlns:" + Prefix;
			}

		}

		public string AttrValue
		{
			get 
			{
				return URI;
			}
		}

		// ���Ҳ�������ո�
		public string OuterXml
		{
			get 
			{
				return AttrName + "='" + StringUtil.GetXmlStringSimple(URI) + "'";
			}
		}
	}

}
