using System;
using System.Collections;
using System.Diagnostics;

namespace DigitalPlatform.Xml
{
	// ���һЩ��̬����
	public class ItemUtil
	{
		static void RemoveDup(ref Hashtable target,
			Hashtable dup)
		{
			if (dup.Count == 0)
				return;

			ICollection array = target.Keys;

			int nOldCount = array.Count;

			ArrayList aWantRemove = new ArrayList();

			foreach(object strKey in array)
			{
				if (dup.Contains(strKey) == true) 
				{
					aWantRemove.Add(strKey);
				}
			}

			for(int i=0;i<aWantRemove.Count;i++)
			{
				target.Remove(aWantRemove[i]);
				// Debug.Assert(nOldCount == array.Count, "Hashtable.Keys����ֻ����?");
			}

		}


		
		// ���start���еĲ�����topȦ������δ�����������prefix
		// ע: start��top��Χ�ڱ�start����Ԫ��ʹ�ù���prefix����������
		// parameters:
		//		start	���Ԫ��
		//		top		��Χ����Ԫ�ء�ע��������Ԫ�ء�
		public static void GetUndefinedPrefix(ElementItem start,
			ElementItem top,
			out ArrayList aResult)
		{
			aResult = null;
			

			Hashtable aPrefix = start.GetUsedPrefix(true);

			if (start == top)
				goto END1;

			ElementItem current = (ElementItem)start.parent;
			while(true)
			{
				if (current == null)
					break;

				Hashtable aCurrentLevelPrefix = current.GetPrefix(ElementItem.GetPrefixStyle.All);

				// ��aPrefix��ȥ����aCurrentLevelPrefix�ཻ�Ĳ���
				RemoveDup(ref aPrefix,
					aCurrentLevelPrefix);

				if (current == top)
					break;

				current = (ElementItem)current.parent;
			}

			END1:

				aResult = new ArrayList();

			aResult.AddRange(aPrefix.Keys);
		}

		// �ٶ�nsColl�е���Ϣ������ԭΪxml�������ַ���������element�ڵ�����Լ����У�
		// ������Ϊȷ�����ᷢ�����������Ĵ��󣬶������nsCollȥ��
		public static void RemoveAttributeDup(ElementItem element,
			ref NamespaceItemCollection nsColl)
		{

			if (nsColl.Count == 0)
				return;

			// �Ի�׼Ԫ�ص����Խ���һ��ȥ�ز���
			if (element.attrs != null)
			{
				for(int i=0; i<nsColl.Count; i++)
				{
					NamespaceItem item = (NamespaceItem)nsColl[i];

					string strAttrString = item.AttrName;

					bool bOccurNamespaceAttr = false;
					foreach(AttrItem attr in element.attrs)
					{
						if (attr.IsNamespace == false)
							continue;

						bOccurNamespaceAttr = true;

						if (attr.Name == strAttrString) 
						{
							nsColl.RemoveAt(i);
							i--;
							break;
						}
					
					}

					if (bOccurNamespaceAttr == false)
						break;	// ˵��element.attrs�����У�ȫ������ͨ���͵����ԣ�û�����ֿռ��͵����ԣ���������ζ�Ų���ȥ����
				}

			}

		}

	

		// ture �ҵ�
		public static bool LocateNamespaceByUri(ElementItem startItem,
			string strURI,
			out string strPrefix,
			out Item namespaceAttr)
		{
			strPrefix = "";
			namespaceAttr = null;

			ElementItem currentItem = startItem;
			while(true)
			{
				if (currentItem == null)
					break;

				if (currentItem.attrs != null)
				{
					foreach(AttrItem attr in currentItem.attrs)
					{
						if (attr.IsNamespace == false)
							continue;

						if (attr.GetValue() == strURI)
						{
							strPrefix = attr.Name;
							namespaceAttr = attr;
							return true;
						}
					}
				}
				currentItem = (ElementItem)currentItem.parent;
			}

			return false;

		}

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
		public static bool LocateNamespaceByPrefix(ElementItem startItem,
			string strPrefix,
			out string strURI,
			out AttrItem namespaceAttr)
		{
			strURI = "";
			namespaceAttr = null;

			/*
			Debug.Assert(strPrefix != "", "strPrefix������Ӧ��Ϊ�ա�ǰ׺Ϊ��ʱ��������ñ�������֪��û���ҵ�");
			if (strPrefix == "")
				return false;
			*/

			ElementItem currentItem = startItem;
			while(true)
			{
				if (currentItem == null)
					break;

				foreach(AttrItem attr in currentItem.attrs)
				{
					if (attr.IsNamespace == false)
						continue;

					string strLocalName = "";
					int nIndex = attr.Name.IndexOf(":");
					if (nIndex >= 0)
					{
						strLocalName = attr.Name.Substring(nIndex + 1);
					}
					else
					{
						Debug.Assert(attr.Name == "xmlns", "���ֿռ��͵����ԣ����name����ð�ţ���ȻΪxmlns��");
						if (attr.Name == "xmlns")
						{
							strLocalName = "";
						}
					}

					if (strLocalName == strPrefix)
					{
						strURI = attr.GetValue();
						namespaceAttr = attr;
						return true;
					}
				}

				currentItem = (ElementItem)currentItem.parent;
			}

			return false;

		}

        // parameters:
		//      descendant  ����ĺ��  ��
		//      ancestor    ���������    ��
		public static bool IsAncestorOf(ElementItem descendant,
			ElementItem ancestor)
		{

			Item currentItem = descendant.parent;
			while(true)
			{
				if (currentItem == null)
					return false;

				if (currentItem == ancestor)
					return true;

				currentItem = currentItem.parent;
			}
			// return false;
		}

		// �ж�һ���ͼ��ڵ��Ƿ��Ǹ߼��ڵ���¼�
		public static bool IsBelong(Item lowerItem,Item higherItem)
		{
			Item thisItem = lowerItem;
			while(true)
			{
				if (thisItem == null)
					break;
				if (thisItem.Equals (higherItem) == true)
					return true;

				thisItem = thisItem.parent ;
			}
			return false;
		}

		
		public static string GetLocalName(string strName)
		{
			string strLocalName = "";
			bool bIsNamespace = false;

			if (strName.Length >= 5)
			{
				if (strName.Substring(0,5) == "xmlns")	// ��Сд����
				{
					if (strName.Length == 5)	// �������ԣ���������ǰ׺�ַ��������ֿռ�
					{
						bIsNamespace = true;
						strLocalName = "";

						// ���Ҫ��.net dom���ݵĻ�
						//this.Prefix = "";	// ��Ӧ��"xmlns"������.net��domΪ"";
						//this.LocalName = "xmlns";	// ��Ӧ��"",����.net��domΪ"xmlns";
					}
					else if (strName[5] == ':')	// �������ԣ���������ǰ׺�ַ��������ֿռ�
					{
						bIsNamespace = true;
						strLocalName = strName.Substring(6);
						if (strLocalName == "")
						{
							throw(new Exception("ð�ź���Ӧ��������"));
						}
					}
					else // ��ͨ����
					{
						bIsNamespace = false;
					}
				}
			}

			// ������ͨ����
			if (bIsNamespace == false)
			{
				int nRet = strName.IndexOf(":");
				if (nRet == -1) 
				{
					strLocalName = strName;
				}
				else 
				{
					strLocalName = strName.Substring(nRet + 1);
				}
			}
			return strLocalName;
		}


		// �õ�startItem��һ�������Ľڵ�
		// MoveMember.Auto      ���£����ϣ��ٸ���
		// MoveMember.Front     ǰ��
		// MoveMember.Behind    ��
		public static Item GetNearItem(Item startItem,
			MoveMember moveMember)
		{
			Item item = startItem;

			if (item == null)
				return null;

			if (item.parent == null)
				return null;

			int nRet = -1;
			ItemList aItem = null;
			// ���item��parent������ElementItem������ͼ�������
			if (item.parent != null) 
			{
				aItem = item.parent.children;
				if (aItem != null)
					nRet = aItem.IndexOf(item);
				if (nRet == -1)
				{
					Debug.Assert(item.parent != null,
						"�����Ѿ��жϹ���");

					aItem = ((ElementItem)item.parent).attrs ;
					if (aItem != null)
						nRet = aItem.IndexOf(item);
				}	
			
			}

			if (nRet == -1)
				return null;

			Debug.Assert(aItem != null, "�����Ѿ��жϹ���nRet");

			if (moveMember == MoveMember.Auto)
			{
				if (aItem.Count > nRet + 1)  //������һ��
					return aItem[nRet+1];
				else if (nRet > 0)
					return aItem[nRet -1];  // ������һ��
			}

			if (moveMember == MoveMember.Front)
			{
				if (nRet > 0)
					return aItem[nRet - 1];
			}
			if (moveMember== MoveMember.Behind  )
			{
				if (aItem.Count  > nRet + 1)
					return aItem[nRet + 1];
			}

			return item.parent;   //���ظ���
		}

		// ��path�õ�Item
		// parameters:
		//		itemRoot	��item
		//		strPath	path
		//		item	out����������item
		// return:
		//		-1	error
		//		0	succeed
		public static int Path2Item(ElementItem itemRoot,
			string strPath, 
			out Item item)
		{
			item = null;
			if (itemRoot == null)
				return -1;
			if (itemRoot.children == null)
				return -1;

			if (strPath == "")
				return -1;

			int nPosition = strPath.IndexOf ('/');

			string strLeft= "";
			string strRight = "";
			if (nPosition >= 0)
			{
				strLeft = strPath.Substring(0,nPosition);
				strRight = strPath.Substring(nPosition+1);
			}
			else
			{
				strLeft = strPath;
			}

			//�õ����
			int nIndex = getNo(strLeft);

			//�õ�����
			nPosition = strLeft.IndexOf ("[");
			string strName = strLeft.Substring (0,nPosition);

			int i=0;
			foreach(Item child in itemRoot.children )
			{
				if (child.Name == strName)
				{
					//�ݹ�
					if (i == nIndex)
					{
						if (strRight == "")
						{
							item = child;
							break;
						}
						else 
						{
							if (!(child is ElementItem))
								return -1;	// text���ͽڵ���Ҳ�޷����������Ҷ�����
							Debug.Assert(child is ElementItem);
							return Path2Item((ElementItem)child,
								strRight,
								out item);
						}
					}
					else
						i++;
				}
			}
			return 0;
		}

		
		// �õ�����[]�еĺ��룬ת��Ϊ��ֵ�����Ҽ�1
        // parameters:
		//      strText ������ַ���
        // return:
		//      �ú������ص���ֵ��ע��Ƚ���ţ�һ��Ҫ����ֵ���ͣ���Ҫ���ַ����ͣ��Լ�ԭ���ʹ������ַ�����
		private static int getNo(string strText)
		{
			//���ȿ�"["�ڸ��ַ������ֵ�λ��
			int nPositionNo = strText.IndexOf("[");

			//���instr����ֵ���ڣ����ʾȷʵ�����ˣ�����û��"]"���������
			if (nPositionNo > 0)
			{
				//�ص�strText��"["��ʼ��ߵ��ַ���ֻʣ�ұ�
				strText = strText.Substring(nPositionNo+1);

				//Ȼ���ٴ�ʣ�µ��ַ�����"]"���ֵ�λ��
				nPositionNo = strText.IndexOf("]");

				//����ҵ�����ֻ����"]"��ߵ����ݣ�
				if (nPositionNo > 0)
					strText = strText.Substring(0,nPositionNo);  //nPositionNo-1);
				else
					return 0;

				//������ҽ����ʣ�¿գ���û����ţ���������0����������
				if (strText == "")
					return 0;


				//����strPath�ַ��������һ��ֻ��������ʽ���ַ���
				//ʹ��cint()ת������ֵ���Ҽ�1����ΪDOM���Ǵ�0��ʼ��
				return System.Convert.ToInt32(strText)-1;
			}
			else
			{
				return 0;
			}
		}
		

		// ��item�õ�path
        // parameters:
		//      itemRoot    ��item
		//      item        ������item
		//      strPath     out����������item��path
        // return:
        //      -1  ����
        //      0   �ɹ�
		public static int Item2Path(ElementItem itemRoot,
			Item item,
			out string strPath)
		{
			strPath = "";
			if (itemRoot == null)
				return -1;
			if (item == null)
				return -1;


			Item itemMyself;
			Item itemTemp;

			int nIndex;


			//��Ϊ���Խڵ�ʱ����������path�ַ���
			string strAttr = "";
			if (item is AttrItem )  
			{
				strAttr = "/@" + item.Name;
				item = item.parent ;
			}

			while(item != null)
			{
				//����ڵ����
				if (item.Equals(itemRoot) == true)
					break;

				itemMyself = item;
				item = item.parent;

				if (item == null)
					break;
				
				itemTemp = null;
				if (item is ElementItem 
					&& ((ElementItem)item).children != null)
				{
					itemTemp = ((ElementItem)item).children[0];
				}

				nIndex = 1;

				while(itemTemp != null)
				{
					if (itemTemp.Equals(itemMyself) == true)
					{
						if (strPath != "")
							strPath = "/" + strPath;

						strPath = itemMyself.Name + "[" + System.Convert.ToString(nIndex) + "]" + strPath;
						
						break;
					}

					if (itemTemp.Name == itemMyself.Name)
						nIndex += 1;
					
					itemTemp = itemTemp.GetNextSibling();
				}
			}

			strPath = strPath + strAttr;

			if (strPath == "")
				return 0;
			else
				return 1;
		}

		public static string GetPartXpath(ElementItem parent,
			Item item)
		{
			string strPath = "";

			Item currentItem = null;
			if (parent.children != null)
			{
				currentItem = parent.children[0];
			}

			int nIndex = 1;

			while(currentItem != null)
			{
				if (currentItem == item)
				{
					strPath = item.Name + "[" + System.Convert.ToString(nIndex) + "]";
					break;
				}

				if (currentItem.Name == item.Name)
					nIndex += 1;
					
				currentItem = currentItem.GetNextSibling();
			}
			return strPath;
		}
	}

}
