using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using DigitalPlatform.IO;

namespace DigitalPlatform.Xml
{
	public class Content : BoxExpandable
	{
		public override ItemRegion GetRegionName()
		{
			if (this.bExpand == true)
				return ItemRegion.Content;
			else
				return ItemRegion.Text;
		}


		// ��ʼvisual����
		public override void InitialVisual()
		{
			ElementItem item = (ElementItem)this.GetItem();
			XmlEditor editor = item.m_document;
			if (item == null)
				return;

			if (item.m_childrenExpand == ExpandStyle.Collapse)
			{
				this.childrenVisual = null;

				this.MyText.container = this.container;
				this.MyText.Name = "content";

				this.MyText.Text = "��δ��ʼ��";
				// ʹ�ó�ʼitemʱ������ֵ
				//this.MyText.Text = item.strInnerXml;	// m_strContentXmlר���������ݴ�������״̬��element���¼�����Xml��Ϣ


				//�ѱ�����Ӹ�������ɾ������m_text�ӽ�����,
				//ʵ����ʵ�ʵ��滻����֪���Ժ����û�˹ܵ�content�ܵ������ˣ�
				((Box)this.container).AddChildVisual(this.MyText);
				((Box)this.container).childrenVisual.Remove (this);

				item.Flush();
				
			}
			else if (item.m_childrenExpand == ExpandStyle.Expand)
			{
				// ��ԭ��ʼ���
				foreach(Item child in item.children)
				{
					if (!(child is ElementItem))
					{
						Debug.Assert(child.GetValue() != null,"׼��ֵ����Ϊnull");
						child.m_paraValue1 = child.GetValue();
					}
				}

				if (this.childrenVisual != null)
					this.childrenVisual.Clear ();

				this.LayoutStyle = LayoutStyle.Vertical ;

				foreach(Item child in item.children)
				{
					//����Ԫ�ص�style��ʽ�븸����ͬ
					child.LayoutStyle = item.LayoutStyle ;
						
					//��child�ӵ�content����ΪChildVisual
					child.container = this;
					this.AddChildVisual (child);


					//ʵ��Ƕ��
					child.InitialVisual();

				}
				
			}
			else 
			{
				Debug.Assert(false, "");
			}
		}


		public override void Layout(int x,
			int y,
			int nInitialWidth,
			int nInitialHeight,
			int nTimeStamp,
			out int nRetWidth,
			out int nRetHeight,
			LayoutMember layoutMember)
		{
			nRetWidth = nInitialWidth;
			nRetHeight = nInitialHeight;

			bool bEnlargeWidth = false;
			if ((layoutMember & LayoutMember.EnlargeWidth  ) == LayoutMember.EnlargeWidth  )
				bEnlargeWidth = true;

			bool bEnlargeHeight = false;
			if ((layoutMember & LayoutMember.EnLargeHeight ) == LayoutMember.EnLargeHeight )
				bEnlargeHeight = true;

			if (bEnlargeWidth == true
				|| bEnlargeHeight == true)
			{
				if (LayoutStyle == LayoutStyle.Horizontal )
				{
					if (bEnlargeHeight == true)
						this.Rect.Height  = nInitialHeight;
				}
				else if (LayoutStyle == LayoutStyle.Vertical )
				{
					if (bEnlargeWidth == true)
						this.Rect.Width = nInitialWidth;
				}

				if ((layoutMember & LayoutMember.Up ) != LayoutMember.Up )
					return;	
			}

			base.Layout (x,
				y,
				nInitialWidth,
				nInitialHeight,
				nTimeStamp,
				out nRetWidth,
				out nRetHeight,
				layoutMember);
		}


	}

}
