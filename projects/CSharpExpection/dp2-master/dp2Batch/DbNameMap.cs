using System;
using System.Collections;
using System.Diagnostics;

namespace dp2Batch1 // �����Ѿ�����ֹ
{
	/// <summary>
	/// backup�ļ��е����ݿ��� �� ������ת���Ŀ�����֮��Ķ��ձ�
	/// </summary>
	public class DbNameMap
	{
		Hashtable m_hashtable = new Hashtable();
		ArrayList m_list = new ArrayList();

		public DbNameMap()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// ����map����
		// parameters:
		//		strDbPaths	�ֺż����·����������ΪOrigin��Target��ͬ��overwrite���
		//					��ʽΪ dbpathorigin-dbpathtarget|style;...
		//					����ʡ��Ϊdbpath1;dbpath2;... ÿ��path�ȵ�originҲ��target��
		public static DbNameMap Build(string strDbPaths,
            out string strError)
		{
            strError = "";
			DbNameMap map = new DbNameMap();

			string[] aPath = strDbPaths.Split(new char[]{';'});

			for(int i=0;i<aPath.Length;i++)
			{
				string strLine = aPath[i].Trim();
				if (strLine == "")
					continue;
				string strOrigin = "";
				string strTarget = "";
				string strStyle = "";

				int nRet = strLine.IndexOf("|");
				if (nRet == -1)
					strStyle = "overwrite";
				else 
				{
					strStyle = strLine.Substring(nRet + 1).Trim().ToLower();
					strLine = strLine.Substring(0, nRet).Trim();
				}

				nRet = strLine.IndexOf("-");
				if (nRet == -1)
				{
					strOrigin = strLine;
					strTarget = strLine;
				}
				else 
				{
					strOrigin = strLine.Substring(0, nRet).Trim();
					strTarget = strLine.Substring(nRet + 1).Trim();
				}

				if (map.NewItem(strOrigin, strTarget, strStyle, out strError) == null)
                    return null;
			}

			return map;
		}

		// ��ȫ������ת��Ϊ�ַ�����̬
		// parameters:
		//		bCompact	�Ƿ�Ϊ������̬
		public string ToString(bool bCompact)
		{
			string strResult = "";
			for(int i=0;i<this.m_list.Count;i++)
			{
				DbNameMapItem item = (DbNameMapItem)this.m_list[i];

				if (strResult != "")
					strResult += ";";

				if (bCompact == true)
				{
					if (item.Origin == item.Target)
					{
						if (item.Style == "overwrite")
							strResult += item.Origin;
						else
							strResult += item.Origin + "|" + item.Style;
					}
					else 
					{
						if (item.Style == "overwrite")
							strResult += item.Origin + "-" + item.Target;
						else
							strResult += item.Origin + "-" + item.Target + "|" + item.Style;

					}

				}
				else
					strResult += item.Origin + "-" + item.Target + "|" + item.Style;
			}

			return strResult;
		}

		public DbNameMapItem NewItem(string strOrigin,
			string strTarget,
			string strStyle,
            out string strError)
		{
			return NewItem(strOrigin,
				strTarget,
				strStyle,
                -1,
                out strError);
		}

		// ��������
		// ��Ҫ����
		public DbNameMapItem NewItem(string strOrigin,
			string strTarget,
			string strStyle,
			int nInsertPos,
            out string strError)
		{
            strError = "";

            Debug.Assert(strStyle.IndexOf("--") == -1, "");

			DbNameMapItem item  = new DbNameMapItem();
			item.Origin = strOrigin;
			item.Target = strTarget;
			item.Style = strStyle;

			if (nInsertPos == -1)
				this.m_list.Add(item);
			else 
				this.m_list.Insert(nInsertPos, item);

            // 2010/2/25
            if (this.m_hashtable.ContainsKey(strOrigin.ToUpper()) == true)
            {
                strError = "������Ϊ '" + strOrigin + "' ����������ظ�����";
                return null;
            }

			this.m_hashtable.Add(strOrigin.ToUpper(), item);

			return item;
		}

		public void Clear()
		{
			this.m_list.Clear();
			this.m_hashtable.Clear();
		}

		public int Count
		{
			get 
			{
				return m_list.Count;
			}
		}

		public DbNameMapItem this[int nIndex]
		{
			get 
			{
				return (DbNameMapItem)this.m_list[nIndex];
			}
			set 
			{
				DbNameMapItem olditem = (DbNameMapItem)this.m_list[nIndex];
				this.m_hashtable.Remove(olditem.Origin.ToUpper());

				this.m_list[nIndex] = value;
				this.m_hashtable.Add(value.Origin.ToUpper(), value);
			}
		}

		// ��Origin�ַ�����Ϊkey����
		public DbNameMapItem this[string strOrigin]
		{
			get 
			{
				return (DbNameMapItem)this.m_hashtable[strOrigin.ToUpper()];
			}
			set 
			{
				DbNameMapItem olditem = (DbNameMapItem)this.m_hashtable[strOrigin.ToUpper()];
				int nOldIndex = -1;
				if (olditem != null)
				{
					nOldIndex = this.m_list.IndexOf(olditem);
					this.m_hashtable.Remove(olditem.Origin.ToUpper());
					this.m_list.Remove(olditem);
				}

				if (nOldIndex != -1)
					this.m_list[nOldIndex] = value;
				else
					this.m_list.Add(value);

				this.m_hashtable.Add(value.Origin.ToUpper(), value);
			}
		}

		public DbNameMap Clone()
		{
			/*
			DbNameMap result = new DbNameMap();

			foreach( DictionaryEntry entry in this)
			{
				// int i =0;
				

				DbNameMapItem olditem = (DbNameMapItem)this[entry.Key];

				DbNameMapItem newitem = new DbNameMapItem();

				newitem.strOrigin = olditem.strOrigin;
				newitem.strStyle = olditem.strStyle;
				newitem.strTarget = olditem.strTarget;

				result.Add(entry.Key, newitem);
			}
			*/
			DbNameMap result = new DbNameMap();

			for(int i=0;i<this.m_list.Count;i++)
			{
				DbNameMapItem olditem = (DbNameMapItem)this.m_list[i];

                string strError = "";
				if (result.NewItem(olditem.Origin, olditem.Target, olditem.Style, out strError) == null)
                    throw new Exception(strError);
			}
			return result;
		}

		// ����Դ·��ƥ���ʺϵ�����
		public DbNameMapItem MatchItem(string strOrigin)
		{
			for(int i=0;i<this.Count;i++)
			{
				DbNameMapItem item = this[i];

				if (strOrigin == "{null}" || strOrigin == "" || strOrigin == null)
				{
					if (item.Origin == "{null}")
						return item;
					if (item.Origin == "*")
					{
						// ���������ƥ������,���ǻ���Ҫ��ϸ����strTarget�Ƿ�Ϊ"*"
						if (item.Target != "*")
							return item;
					}
					continue;
				}

				if (item.Origin == "*")
					return item;

				if (String.Compare(item.Origin, strOrigin, true) == 0)
					return item;
				
			}


			return null;
		}


	}

	public class DbNameMapItem
	{
		public string Origin = "";	// Դͷ
		public string Target = "";	// Ŀ��

		public string Style;	// ��񡣱ȷ�˵"overwrite"����"append"

	}
}
