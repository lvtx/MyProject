using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using DigitalPlatform.IO;


namespace DigitalPlatform.Xml
{
	public class BoxExpandable : Box
	{
		public XmlText m_text1 = new XmlText ();

		public bool bExpand = true;         // ��ǰ״̬


		public XmlText MyText
		{
			get
			{
				this.m_text1.Editable = true;
				return this.m_text1 ;
			}
		}

		public override LayoutStyle LayoutStyle
		{
			get
			{
				return LayoutStyle.Vertical ;
			}
		}

		public virtual string Xml
		{
			get
			{
				return "";
			}
		}

		public virtual void InitialVisual()
		{}


		public override ItemRegion GetRegionName()
		{
			return ItemRegion.Content   ;
		}

        // ���ݴ����������꣬�õ����е�Visual����
        // parameters:
        //      p           ������������
        //      retVisual   out���������ػ��е�visual
        // return:
        //      -1  ���겻�ڱ�����
        //      0   ������
        //      1   �հ�
        //      2   ��϶��
		public override int HitTest(Point p,
			out Visual retVisual)
		{
			if (this.bExpand == false)
			{
				return this.MyText.HitTest(p,out retVisual);
			}
			else
			{
				return base.HitTest(p,out retVisual);
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

			if ( bExpand == false)
			{
				this.MyText.Layout (x,
					y,
					nInitialWidth,
					nInitialHeight,
					nTimeStamp,
					out nRetWidth,
					out nRetHeight,
					layoutMember);
			}
			else
			{
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


		public override void Paint(PaintEventArgs pe,
			int nBaseX,
			int nBaseY,
			PaintMember paintMember)
		{
			if (this.bExpand  == false)
			{
				this.MyText.Paint (pe,nBaseX,nBaseY,paintMember);
			}
			else
			{
				base.Paint(pe,nBaseX,nBaseY,paintMember);
			}

		}
	}

}
