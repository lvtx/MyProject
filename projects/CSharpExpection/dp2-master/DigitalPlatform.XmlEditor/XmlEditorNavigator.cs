using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

using DigitalPlatform.IO;

namespace DigitalPlatform.Xml
{
	// �����ͼ:
	// ���������Ϳ���ͨ��XPath��λItem���еĽڵ�
	public class XmlEditorNavigator : XPathNavigator
	{
		private NameTable m_nametable = null;
        internal NavigatorState m_navigatorState = null;



		// ע�������Լ�������element�ڵ�,�۲�.net
		public XmlEditorNavigator(Item item)
		{
			//StreamUtil.WriteText("I:\\debug.txt","���� ���캯��XmlEditorNavigator(editor)��\r\n");

			Debug.Assert(item != null,"item����Ϊnull");

			XmlEditor document = item.m_document;
			Debug.Assert(document != null,"document����Ϊnull");

            this.m_navigatorState = new NavigatorState();
            this.m_navigatorState.CurItem = item;        //�ѵ�ǰ�ڵ���Ϊ���
            this.m_navigatorState.DocRoot = document.docRoot;
            this.m_navigatorState.VirtualRoot = document.VirtualRoot;

            this.m_nametable = new NameTable();
            this.m_nametable.Add(String.Empty);
		}

		public XmlEditorNavigator(XmlEditorNavigator navigator ) 
		{
			//StreamUtil.WriteText("I:\\debug.txt","���� ���캯��XmlEditorNavigator(navigator)��\r\n");

            this.m_navigatorState = new NavigatorState(navigator.m_navigatorState);
            this.m_nametable = (NameTable)navigator.NameTable;
		}

		public Item Item
		{
			get
			{
				return this.m_navigatorState.CurItem;
			}
		}

		#region ���ص�����

		public override XPathNodeType NodeType 
		{ 
			get 
			{
				//StreamUtil.WriteText("I:\\debug.txt","���� NodeType����\r\n");
                Debug.Assert(this.m_navigatorState.CurItem != null, "");

                if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
					return XPathNodeType.Root;	// �����

                if (this.m_navigatorState.CurItem is AttrItem
                    && ((AttrItem)this.m_navigatorState.CurItem).IsNamespace == true)
					return XPathNodeType.Namespace;
                if (this.m_navigatorState.CurItem is ElementItem)
					return XPathNodeType.Element;
                if (this.m_navigatorState.CurItem is AttrItem)
					return XPathNodeType.Attribute;
                if (this.m_navigatorState.CurItem is TextItem)
					return XPathNodeType.Text;

				return XPathNodeType.All;
			}
		}

		public override string LocalName 
		{
			get 
			{
				//StreamUtil.WriteText("I:\\debug.txt","���� LocalName����\r\n");

				// LocalName������ǰ׺
				string strName = this.Name;
				int nIndex = strName.IndexOf(":");
				if (nIndex >= 0)
				{
					strName = strName.Substring(nIndex + 1);
				}

				this.m_nametable.Add(strName);
                return this.m_nametable.Get(strName); 
			}
		}

		public override string Name 
		{ 
			get 
			{ 

				//StreamUtil.WriteText("I:\\debug.txt","���� Name����\r\n");
                Debug.Assert(this.m_navigatorState.CurItem != null, "");

                if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
					return "";	// ���
				// ����ǰ׺
                return this.m_navigatorState.CurItem.Name;
			}
		}

		public override string NamespaceURI 
		{
			get 
			{
				//StreamUtil.WriteText("I:\\debug.txt","���� NamespaceURI����\r\n");
                Debug.Assert(this.m_navigatorState.CurItem != null, "");

				// ���
                if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
                    return this.m_nametable.Get(String.Empty);

                if (!(this.m_navigatorState.CurItem is ElementAttrBase))
                    return this.m_nametable.Get(String.Empty);

                ElementAttrBase element = (ElementAttrBase)this.m_navigatorState.CurItem;

				if (element.NamespaceURI != null)
				{
                    this.m_nametable.Add(element.NamespaceURI);
                    return this.m_nametable.Get(element.NamespaceURI);

				}

				// ʵ��û��
                return this.m_nametable.Get(String.Empty); 
			}
		}

		public override string Prefix 
		{
			get 
			{
				//StreamUtil.WriteText("I:\\debug.txt","���� Prefix����\r\n");
                Debug.Assert(this.m_navigatorState.CurItem != null, "");

				// ���
                if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
                    return this.m_nametable.Get(String.Empty);

                if (!(this.m_navigatorState.CurItem is ElementAttrBase))
                    return this.m_nametable.Get(String.Empty);

                ElementAttrBase element = (ElementAttrBase)this.m_navigatorState.CurItem;


				if (element.Prefix != null)
				{
                    this.m_nametable.Add(element.Prefix);
                    return this.m_nametable.Get(element.Prefix);
				}

                return this.m_nametable.Get(String.Empty); 
			}
		}

		public override string Value
		{
			get 
			{ 
				//StreamUtil.WriteText("I:\\debug.txt","���� Value����\r\n");
                Debug.Assert(this.m_navigatorState.CurItem != null, "");

                if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
                    return this.m_navigatorState.DocRoot.GetValue(); // ???

                return this.m_navigatorState.CurItem.GetValue();
			}
		}

		public override String BaseURI 
		{
			get { 

				//StreamUtil.WriteText("I:\\debug.txt","���� BaseURI����\r\n");
                Debug.Assert(this.m_navigatorState.CurItem != null, "");


                if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
					return "";

                if (!(this.m_navigatorState.CurItem is ElementAttrBase))
					return "";

                ElementAttrBase curItem = (ElementAttrBase)this.m_navigatorState.CurItem;

				return curItem.BaseURI;
			} 
		}

		public override bool IsEmptyElement
		{
			get 
			{
				//StreamUtil.WriteText("I:\\debug.txt","���� IsEmptyElement����\r\n");

                if (this.m_navigatorState.CurItem is ElementItem) 
				{
                    ElementItem element = (ElementItem)this.m_navigatorState.CurItem;

					return element.IsEmpty;
				}
				return false;
			}
		}

		public override string XmlLang
		{
			get
			{
				//StreamUtil.WriteText("I:\\debug.txt","���� XmlLang����\r\n");

				string strLang = this.GetAttribute("lang",
						"http://www.w3.org/2000/xmlns");
				return strLang;

			}
		}

		public override XmlNameTable NameTable 
		{
			get 
			{ 
				//StreamUtil.WriteText("I:\\debug.txt","���� NameTable����\r\n");

                return this.m_nametable; 
			}
		}

		public override bool HasAttributes 
		{
			get
			{
				//StreamUtil.WriteText("I:\\debug.txt","���� HasAttributes����\r\n");

				if (!(this.m_navigatorState.CurItem is ElementItem))
					return false;

                ElementItem curItem = (ElementItem)this.m_navigatorState.CurItem;

				if (curItem.PureAttrList != null
					&& curItem.PureAttrList.Count > 0)
					return true;	// ������
				else
					return false;	// ע:���Ҳ����element
			}
		}


		#endregion

		#region ������Խڵ�

		public override string GetAttribute(string localName,
			string namespaceURI) 
		{

			//StreamUtil.WriteText("I:\\debug.txt","���� GetAttribute()\r\n");

			if (HasAttributes == false)
				return String.Empty;

            ElementItem element = (ElementItem)this.m_navigatorState.CurItem;

			string strAttr = element.GetAttribute(localName,
				namespaceURI);
			if (strAttr != "")
				return strAttr;

			return String.Empty;
		}


		// �ƶ��������һ��element,ͨ�����ϼ�
		void MoveToElement()
		{
			//StreamUtil.WriteText("I:\\debug.txt","���� MoveToElement()\r\n");

			Debug.Assert(this.m_navigatorState.CurItem != null, "");

			if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
				return;

			if (!(this.m_navigatorState.CurItem is ElementItem)) 
			{
                this.m_navigatorState.CurItem = this.m_navigatorState.CurItem.parent;
			}
		}

		public override bool MoveToAttribute(string localName,
			string namespaceURI) 
		{
			//StreamUtil.WriteText("I:\\debug.txt","���� MoveToAttribute()\r\n");


            if (!(this.m_navigatorState.CurItem is ElementItem))
				MoveToElement();

            if (!(this.m_navigatorState.CurItem is ElementItem)) 
			{
				return false;
			}

            ElementItem element = (ElementItem)this.m_navigatorState.CurItem;

			// namespace ��ǿ???

			Item temp = element.GetAttrItem(localName);
			if (temp == null)
				return false;

            this.m_navigatorState.CurItem = temp;
			return true;
		}

		public override bool MoveToFirstAttribute() 
		{

			//StreamUtil.WriteText("I:\\debug.txt","���� MoveToFirstAttribute()\r\n");

            if (!(this.m_navigatorState.CurItem is ElementItem))
				MoveToElement();

            Debug.Assert(this.m_navigatorState.CurItem is ElementItem, "������Ԫ�ؽڵ�");

            ElementItem curItem = (ElementItem)this.m_navigatorState.CurItem;

			if (curItem.PureAttrList == null)
				return false;

			if (curItem.PureAttrList.Count == 0)
				return false;

            this.m_navigatorState.CurItem = curItem.PureAttrList[0];
			return true;
		}

		public override bool MoveToNextAttribute() 
		{

			//StreamUtil.WriteText("I:\\debug.txt","���� MoveToNextAttribute()\r\n");


            if (!(this.m_navigatorState.CurItem is AttrItem))
				return false;

            Debug.Assert(this.m_navigatorState.CurItem is AttrItem, "���������Խڵ�");

            ElementItem parent = (ElementItem)this.m_navigatorState.CurItem.parent;	// ���Ե�parentһ����ElementItem����

			if (parent == null)
				return false;	// ???

            int nIndex = parent.PureAttrList.IndexOf(this.m_navigatorState.CurItem);
			if (nIndex + 1 >= parent.PureAttrList.Count)
				return false;	// �Ѿ����ֵ�ĩβ

            this.m_navigatorState.CurItem = parent.PureAttrList[nIndex + 1];

			return true;
		}


		public override bool MoveToId( string id ) 
		{
			//StreamUtil.WriteText("I:\\debug.txt","���� MoveToId()\r\n");

			return false;
		}
		#endregion

		#region ��������ռ�ڵ�

		public override string GetNamespace(string localname)
		{
			//StreamUtil.WriteText("I:\\debug.txt","GetNamespace() !");

            if (!(this.m_navigatorState.CurItem is ElementItem))
				return String.Empty;

            ElementItem element = (ElementItem)this.m_navigatorState.CurItem;

			ItemList namespaceList = element.NamespaceList;
			for(int i=0;i<namespaceList.Count;i++)
			{
				string strName = namespaceList[i].Name;
				int nIndex = strName.IndexOf(":");
				if (nIndex >= 0)
					strName = strName.Substring(nIndex + 1);

				if (strName == localname)
					return namespaceList[i].GetValue();
			}
			return String.Empty;
		}
		public override bool MoveToNamespace(string Namespace)
		{
			//StreamUtil.WriteText("I:\\debug.txt","���� MoveToNamespace()\r\n");


            if (!(this.m_navigatorState.CurItem is ElementItem))
				return false;

            ElementItem element = (ElementItem)this.m_navigatorState.CurItem;


			ItemList namespaceList = element.NamespaceList;
			for(int i=0;i<namespaceList.Count;i++)
			{
				if (namespaceList[i].Name == Namespace)
				{
                    this.m_navigatorState.CurItem = namespaceList[i];
					return true;
				}
			}

			return false;
		}

		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{

			//StreamUtil.WriteText("I:\\debug.txt","���� MoveToFirstNamespace()\r\n");

            if (!(this.m_navigatorState.CurItem is ElementItem))
				return false;

            ElementItem element = (ElementItem)this.m_navigatorState.CurItem;

			ItemList namespaceList = element.NamespaceList;
			if (namespaceList.Count > 0)
			{
                this.m_navigatorState.CurItem = namespaceList[0];
				return true;
			}

			return false;
		}

		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			//StreamUtil.WriteText("I:\\debug.txt","���� MoveToNextNamespace()\r\n");
            Debug.Assert(this.m_navigatorState.CurItem != null, "");


            if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
				return false;

            if (!(this.m_navigatorState.CurItem is AttrItem))
				return false;

            AttrItem attr = (AttrItem)this.m_navigatorState.CurItem;

			if (attr.IsNamespace == false)
				return false;

			ElementItem element = (ElementItem)attr.parent;

			ItemList namespaceList = element.NamespaceList;
			if (namespaceList.Count > 0)
			{
                int nIndex = namespaceList.IndexOf(this.m_navigatorState.CurItem);
				if (nIndex == -1)
					return false;
				if (nIndex + 1 >= namespaceList.Count)
					return false;

                this.m_navigatorState.CurItem = namespaceList[nIndex + 1];
				return true;
			}

			return false;
		}
        

		#endregion

		#region �����ͨ�ڵ�,��element comment...���ƶ��Ȳ���

		public override void MoveToRoot()
		{
			//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToRoot() \r\n");
            this.m_navigatorState.CurItem = this.m_navigatorState.VirtualRoot;

            Debug.Assert(this.m_navigatorState.CurItem != null, "");

		}

		public override bool MoveToParent() 
		{
			//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToParent() \r\n");
            Debug.Assert(this.m_navigatorState.CurItem != null, "");

            if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
				return false;

            Item temp = this.m_navigatorState.CurItem.parent;
            this.m_navigatorState.CurItem = temp;
			return true;
		}

		public override bool MoveToFirstChild() 
		{

			//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToFirstChild() \r\n");
            Debug.Assert(this.m_navigatorState.CurItem != null, "");

			// ���
            if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot) 
			{
                if (this.m_navigatorState.DocRoot == null)
				{
					Debug.Assert(false,
                        "DocRoot����Ϊnull");
					return false;
				}

				/*
				//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToFirstChild() ��ǰ����� \r\n");

				Debug.Assert(state.DocRoot != null,	"DocRoot����Ϊnull");
				state.CurItem = state.DocRoot;

				return true;
				*/
			}

			//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToFirstChild() \r\n");

            if (!(this.m_navigatorState.CurItem is ElementItem))
				return false;

            ElementItem parent = (ElementItem)this.m_navigatorState.CurItem;

			if (parent == null)
				return false;

			if (parent.children == null || parent.children.Count == 0) 
			{
				return false;
			}

            this.m_navigatorState.CurItem = parent.children[0];

			return true;
		}

		public override bool MoveToFirst() 
		{
			//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToFirst() \r\n");
            Debug.Assert(this.m_navigatorState.CurItem != null, "");

            if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
				return true;

            if (this.m_navigatorState.CurItem is AttrItem)
				return false;

            ElementItem parent = (ElementItem)this.m_navigatorState.CurItem.parent; 

			Debug.Assert(parent != null, "�����������������κ�element,����parent");
			/*
			// �����
			if (parent == null)
			{
				Debug.Assert(state.CurItem == state.DocRoot,"��ǰ�ڵ�һ����ʵ����"); 
				return true;
			}
			*/

			if (parent.children == null || parent.children.Count == 0) 
			{
				Debug.Assert(false, "��̫���ܳ��ֵ����....");
				return false;
			}

            this.m_navigatorState.CurItem = parent.children[0];

			return true;
		}

		public override bool MoveToPrevious() 
		{

			//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToPrevious() \r\n");
            Debug.Assert(this.m_navigatorState.CurItem != null, "");


			// ���������
            if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
				return false;

            if (this.m_navigatorState.CurItem is AttrItem)
				return false;

            ElementItem parent = (ElementItem)this.m_navigatorState.CurItem.parent;
			
			Debug.Assert(parent != null, "�����������������κ�element,����parent");
			/*
			// �����������˵���Լ���ʵ��
			if (parent == null)
			{
				return false;  // ʵ����Ψһ��һ����û���ֵܣ������Ƶ���һ��
			}
			*/

			if (parent.children == null || parent.children.Count == 0) 
			{
				Debug.Assert(false, "��̫���ܳ��ֵ����....");
				return false;
			}

            int nIndex = parent.children.IndexOf(this.m_navigatorState.CurItem);
			if (nIndex - 1 < 0)
				return false;	// �Ѿ����ֵܿ�ͷ

            this.m_navigatorState.CurItem = parent.children[nIndex - 1];

			return true;
		}

		public override bool MoveToNext() 
		{

			//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToNext()1 \r\n");
            Debug.Assert(this.m_navigatorState.CurItem != null, "");


			// ���������
            if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
				return false;

            if (this.m_navigatorState.CurItem is AttrItem)
				return false;

            ElementItem parent = (ElementItem)this.m_navigatorState.CurItem.parent;

			Debug.Assert(parent != null, "�����������������κ�element,����parent");
			/*
			// �����������˵���Լ���ʵ��
			if (parent == null)
			{
				//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--ʵ����Ψһ��һ����û���ֵܣ������Ƶ���һ�� \r\n");

				return false;  // ʵ����Ψһ��һ����û���ֵܣ������Ƶ���һ��
			}
			*/

			//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--MoveToNext()2 \r\n");

			if (parent.children == null || parent.children.Count == 0) 
			{
				Debug.Assert(false, "��̫���ܳ��ֵ����....");
				return false;
			}

            int nIndex = parent.children.IndexOf(this.m_navigatorState.CurItem);
			if (nIndex + 1 >= parent.children.Count)
				return false;	// �Ѿ����ֵ�ĩβ

            this.m_navigatorState.CurItem = parent.children[nIndex + 1];

			return true;
		}
        

		public override bool HasChildren 
		{
			get 
			{ 
				//StreamUtil.WriteText("I:\\debug.txt",this.Name + "--HasChildren() \r\n");
                Debug.Assert(this.m_navigatorState.CurItem != null, "");

				/*
				// �������
				if (state.CurItem == null) 
				{
					if (state.DocRoot != null)
						return true;
					else
					{
						Debug.Assert(false,"?������ϣ�û��ʵ���ǲ����ܵ����");
					}
					return false;
				}
				*/

                if (!(this.m_navigatorState.CurItem is ElementItem)) 
				{
                    Debug.Assert(!(this.m_navigatorState.CurItem is VirtualRootItem), 
						"�����. һ��VirtualRootItemҲӦͬʱ��һ��ElementItem");
					return false;
				}

                ElementItem element = (ElementItem)this.m_navigatorState.CurItem;

				if (element.children == null)
					return false;

				if (element.children.Count == 0)
					return false;

				return true;
			}
		}
        
		#endregion

		#region ���XPathNavigator����

		public override XPathNavigator Clone() 
		{
			return new XmlEditorNavigator(this);
		}

		public override bool MoveTo( XPathNavigator other ) 
		{
			if (other is XmlEditorNavigator )
			{
                this.m_navigatorState = new NavigatorState(((XmlEditorNavigator)other).m_navigatorState);
				return true;
			}
			return false;
		}

        // �Ƿ���ͬ����λ��
        // parameters:
        //      other   XPathNavigator����
        // return:
        public override bool IsSamePosition(XPathNavigator other)
        {
            Debug.Assert(this.m_navigatorState.CurItem != null, "");
            if (other is XmlEditorNavigator)
            {
                if (this.m_navigatorState.CurItem == this.m_navigatorState.VirtualRoot)
                {
                    return (((XmlEditorNavigator)other).m_navigatorState.CurItem == ((XmlEditorNavigator)other).m_navigatorState.VirtualRoot);
                }
                else
                {
                    if (((XmlEditorNavigator)other).m_navigatorState.CurItem == ((XmlEditorNavigator)other).m_navigatorState.VirtualRoot)
                        return false;
                    return (this.m_navigatorState.CurItem.GetXPath() == ((XmlEditorNavigator)other).m_navigatorState.CurItem.GetXPath());
                }
            }
            return false;
        }
    
		#endregion

		// *********************
		// This class keeps track of the state the navigator is in.
		internal class NavigatorState
		{
			public Item CurItem = null;
			public Item DocRoot = null;
			public Item VirtualRoot = null;

			public NavigatorState()
			{}

			public NavigatorState(Item item)
			{
				CurItem = item;
				VirtualRoot = item.m_document.VirtualRoot;
                Debug.Assert(VirtualRoot != null, "VirtualRoot����Ϊnull");
				DocRoot = item.m_document.docRoot;
				Debug.Assert(DocRoot != null,"DocRoot����Ϊnull");
			}

			public NavigatorState(NavigatorState NavState)
			{
				this.CurItem = NavState.CurItem;
				this.DocRoot = NavState.DocRoot;
				this.VirtualRoot = NavState.VirtualRoot;
			}
		}

	}
}
