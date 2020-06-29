using System;
using System.Collections ;

namespace DigitalPlatform.Xml
{
	// �����������ж���Ŀ��
	// ��Ŵ�����
	public class ItemWidthList : CollectionBase
	{
		public ItemWidth this[int index]
		{
			get 
			{
				return (ItemWidth)InnerList[index];
			}
			set
			{
				InnerList[index] = value;
			}
		}
		public void Add(ItemWidth width)
		{
			InnerList.Add(width);
		}

		// ȡһ��������飬���û���壬���´���һ��
		public ItemWidth GetItemWidth(int nLevel)
		{
			while(nLevel >= this.Count )
			{
				this.Add (new ItemWidth ());
			}
			return this[nLevel];
		}
	}

	//��ͼ��һ��level�ĸ����������width
	public class ItemWidth:ArrayList
	{
		public ItemWidth()
		{  
			this.Add (new PartWidth ("Label",80,1,1));
			this.Add (new PartWidth ("Comment",0,1,1));
			this.Add (new PartWidth ("Text",400,0,0));

			this.Add (new PartWidth ("ExpandHandle",13,1,1));
			this.Add (new PartWidth ("Content",100,0,0));
			this.Add (new PartWidth ("Attributes",100,0,0));
			this.Add (new PartWidth ("Box",100,0,0));
		}

		// ȡ������ȡ��ָ�������width
		public int GetValue(string strName)
		{
			foreach(PartWidth part in this)
			{
				if (part.strName == strName)
					return part.nWidth ;
			}
			return -1;
		}

		public PartWidth GetPartWidth(string strName)
		{
			foreach(PartWidth part in this)
			{
				if(part.strName == strName)
					return part;
			}
			return null;
		}


		// ��ָ�������width������Ѿ����ڣ����޸���ֵ����������ڣ����½�һ��
		public void  SetValue(string strName,int nValue)
		{
			PartWidth partWidth = null;
			foreach(PartWidth part in this)
			{
				if (part.strName == strName)
				{
					partWidth = part;
					break;
				}
			}
			if (partWidth == null)
			{
				partWidth = new PartWidth  (strName,nValue,0,0);
				this.Add (partWidth);
			}
			else 
			{
				partWidth.nWidth = nValue;
			}
		}

		// ����ָ������ļ���
		public void UpGradeNo(string strName)
		{
			foreach(PartWidth part in this)
			{
				if (part.strName == strName)
				{
					part.UpGradeNo ();
					break;
				}
			}
		}

		// ��ָ������ļ���ԭΪȱʡֵ
		public void BackDefaultGradeNo(string strName)
		{
			foreach(PartWidth part in this)
			{
				if (part.strName == strName)
				{
					part.BackDefaultGradeNo ();
					break;
				}
			}
		}

		
	}

	public class PartWidth
	{
		public string strName = "";   //����
		public int nWidth = -1;       //���
  
		public int nDefaultGradeNo = 0; //ȱʡ�����
		public int nGradeNo = 0;        //�����

		// ���캯��
		public PartWidth(string myName,
			int myWidth,
			int myDefaultGradeNo,
			int myGradeNo)
		{
			strName = myName;
			nWidth = myWidth;
			nDefaultGradeNo = myDefaultGradeNo;
			nGradeNo = myGradeNo;			
		}



		// ����ָ������ļ���
		public void UpGradeNo()
		{
			this.nGradeNo ++ ;
		}

		// ��ָ������ļ���ԭΪȱʡֵ
		public void BackDefaultGradeNo()
		{
			this.nGradeNo = this.nDefaultGradeNo ;
		}
	}
}
