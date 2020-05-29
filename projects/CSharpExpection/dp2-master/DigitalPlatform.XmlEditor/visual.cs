using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using DigitalPlatform.IO;


namespace DigitalPlatform.Xml
{
	public class Visual
	{
		#region ��Ա����

        //����
		public Visual container = null; 
		// �����������
		public Rectangle Rect = new Rectangle(0,0,0,0);
       
        //��Catch�õĶ���
        public SizeCatch sizeCatch = new SizeCatch();


		private string m_strName = "";

		//����һЩ����
		//public bool bExpandable = false;    //�Ƿ����չ��

		//������,�Ƿ��ǵ�ǰ�϶��Ľڵ�
		public bool bDrag = false;

        // ����
		protected LayoutStyle layoutStyle = LayoutStyle.Horizontal;

		public const int TOPBORDERHEIGHT = 1;       // �Ϸ������ĸ߶�
		public const int BOTTOMBORDERHEIGHT = 1;    // �·������ĸ߶�
		public const int LEFTBORDERWIDTH = 1;       // �������Ŀ��
		public const int RIGHTBORDERWIDTH = 1;      // �ҷ������Ŀ��

		#endregion

        // ���ַ��
		public virtual LayoutStyle LayoutStyle
		{
			get
			{
				return this.layoutStyle;
			}
			set
			{
				this.layoutStyle = value;
			}
		}

        // ����
		public string Name
		{
			get
			{
				return this.m_strName;
			}
			set
			{
				this.m_strName = value;
			}
		}

        // ����Χ�Ŀ��
		public int LeftResWidth
		{
			get
			{
				return this.LeftBlank
					+ this.LeftBorderWidth;
			}
		}
        
        // �ҷ���Χ�Ŀ��
		public int RightResWidth
		{
			get
			{
				return this.RightBlank
					+ this.RightBorderWidth;
			}
		}

        // �Ϸ���Χ�ĸ߶�
		public int TopResHeight
		{
			get
			{
				return this.TopBlank
					+ this.TopBorderHeight;
			}
		}

        // �·���Χ�ĸ߶�
		public int BottomResHeight
		{
			get
			{
				return this.BottomBlank
					+ this.BottomBorderHeight;
			}
		}
        // �ܹ���Χ�Ŀ��
		public int TotalRestWidth
		{
			get
			{
				return this.LeftBlank 
					+ this.RightBlank
					+ this.LeftBorderWidth
					+ this.RightBorderWidth;
			}
		}

        // �ܹ���Χ�ĸ߶�
		public int TotalRestHeight
		{
			get			
			{
				return this.TopBlank
					+ this.BottomBlank
					+ this.TopBorderHeight
					+ this.BottomBorderHeight;
			}
		}



		#region ������������Ժͺ���

		// �õ�����ڸ���rectangle
		public Rectangle RectAbs
		{
			get
			{
				int nRootX = 0;
				int nRootY = 0;
				getAbs(out nRootX,
					out nRootY);

				return new Rectangle(nRootX,
					nRootY,
					this.Rect.Width,
					this.Rect.Height);
			}
		}

		// �õ���Ը���x
		public int getAbsX()
		{
			int nX,nY;
			getAbs(out nX,out nY);
			return nX;
		}

		// �õ�����ڸ���x,y
		public void getAbs(out int nX,
			out int nY)
		{
			nX = 0;
			nY = 0;
			Visual visual = this;
			while(true)
			{
				if (visual == null)
					break;
				nX += visual.Rect.X ;
				nY += visual.Rect.Y ; 
				visual = visual.container;
			}
		}

		#endregion

		#region һЩ��������

		public bool IsWriteInfo
		{
			get
			{
				return false;
			}
		}

		public bool Catch
		{
			get
			{
				Item item = this.GetItem ();
				return item.m_document.bCatch ;
			}
		}
		#endregion

		#region ����visual�Ĳ�εĺ���

		// �õ�visual�Ĳ��
		public int GetVisualLevel()
		{
			int nLevel = 0;
			Visual visual = this;
			while(true) 
			{
				if (visual == null)
					break;
				visual = visual.container ;
				nLevel ++;
			}
			return nLevel;
		}

		// ����visual��ŵõ��ַ���
		public string GetStringFormLevel(int nLevel)
		{
			string strResult = "";
			for(int i=0;i<nLevel ;i++)
			{
				strResult += "  ";
			}
			return strResult;
		}

		
		// �õ�·��
		public string GetPath()
		{
			string strPath = "";
			Visual visual = this;
		
			while(true)
			{
				if (visual == null)
					break;

				if (strPath != "")
					strPath += "\\";

				strPath += this.GetType ().Name  ;

				Visual parent = visual.container ;
				if (parent != null)
				{
					strPath += "["+((Box)parent).childrenVisual .IndexOf (visual)+"]";
				}
				visual = parent;
			}

			return strPath;
		}

		#endregion

		#region  ������ʽ������Ϣ������

        // �󷽿հ�
		public int LeftBlank
		{
			get
			{
				return GetDigitalValue(ValueStyle.LeftBlank );
			}
		}
		
        // �ҷ��հ�
        public int RightBlank
		{
			get
			{
				return GetDigitalValue(ValueStyle.RightBlank );
			}
		}
		
        // �Ϸ��հ�
        public int TopBlank
		{
			get
			{
				return GetDigitalValue(ValueStyle.TopBlank);
			}
		}

        // �·��հ�
		public int BottomBlank
		{
			get
			{
				return GetDigitalValue(ValueStyle.BottomBlank);
			}
		}


		// �Ϸ������ĸ߶�
		public int TopBorderHeight
		{
			get
			{
				int nTopBorderHeight = GetDigitalValue(ValueStyle.TopBorderHeight);
				if(nTopBorderHeight != -1)
					return nTopBorderHeight;

				
				if (this is VirtualRootItem)
					return 0;//Visual.TOPBORDERHEIGHT;

				Visual parent = this.container;
				if (parent != null)
				{
					if (((Box)parent).LayoutStyle == LayoutStyle.Vertical)
					{
						int nIndex = ((Box)parent).childrenVisual.IndexOf(this);
						if (nIndex == 0)
							return 0;
						return Visual.TOPBORDERHEIGHT;
					}
				}
				return 0;

			}
		}

		// �·������ĸ߶�
		public int BottomBorderHeight
		{
			get
			{
				int nBottomBorderHeight = GetDigitalValue(ValueStyle.BottomBorderHeight);
				if(nBottomBorderHeight != -1)
					return nBottomBorderHeight;

				if (this is VirtualRootItem)
					return Visual.BOTTOMBORDERHEIGHT;
				else
					return 0;
			}
		}

		// �������Ŀ��
		public int LeftBorderWidth
		{
			get
			{
				int nLeftBorderWidth = GetDigitalValue(ValueStyle.LeftBorderWidth);
				if(nLeftBorderWidth != -1)
					return nLeftBorderWidth;


				if (this is VirtualRootItem)
					return Visual.LEFTBORDERWIDTH;

				Visual parent = this.container;
				if (parent != null)
				{
					if (((Box)parent).LayoutStyle == LayoutStyle.Horizontal)
					{
						int nIndex = ((Box)parent).childrenVisual.IndexOf(this);
						if (nIndex == 0)
							return 0;

						return Visual.LEFTBORDERWIDTH;
					}
				}
				return 0;
			}
		}

		// �ҷ������Ŀ��
		public int RightBorderWidth
		{
			get
			{
				int nRightBorderWidth = GetDigitalValue(ValueStyle.RightBorderWidth);
				if(nRightBorderWidth != -1)
					return nRightBorderWidth;

				if (this is VirtualRootItem)
					return Visual.RIGHTBORDERWIDTH;
				else 
					return 0;
			}
		}

        // ��ȡ��ֵ
		public int GetDigitalValue(ValueStyle valueStyle)
		{
			Item item = this.GetItem ();
			if (item == null)
				return 0;

			ItemRegion region = GetRegionName();
			if (region == ItemRegion.No )
				return 0;
			
			if (valueStyle == ValueStyle.LeftBlank )
				return item.GetLeftBlank(region);

			if (valueStyle == ValueStyle.RightBlank )
				return item.GetRightBlank (region);

			if (valueStyle == ValueStyle.TopBlank )
				return item.GetTopBlank  (region);

			if (valueStyle == ValueStyle.BottomBlank  )
				return item.GetBottomBlank(region);

			if (valueStyle == ValueStyle.TopBorderHeight)
				return item.GetTopBorderHeight(region);

			if (valueStyle == ValueStyle.BottomBorderHeight)
				return item.GetBottomBorderHeight(region);

			if (valueStyle == ValueStyle.LeftBorderWidth)
				return item.GetLeftBorderWidth(region);

			if (valueStyle == ValueStyle.RightBorderWidth)
				return item.GetRightBorderWidth(region);

			return 0;
		}

        // ������ɫ
		public Color BorderColor
		{
			get
			{
				return GetColor(ValueStyle.BorderColor );
			}
		}

        // ������ɫ
		public virtual Color BackColor
		{
			get
			{
				if (this.bDrag == true)
				{
					return Color.Red;
				}
				else
				{
					return GetColor(ValueStyle.BackColor);
				}
			}
		}

        // ��ȡ��ɫ
		public Color GetColor(ValueStyle valueStyle)
		{
			Item item = this.GetItem ();
			ItemRegion region = GetRegionName();
			
			if (valueStyle == ValueStyle.BackColor  )
				return item.GetBackColor(region);
			if (valueStyle == ValueStyle.BorderColor )
				return item.GetBorderColor (region);

			return Color.Red;  // �������
		}

		// ȡ��ɫ
		public virtual ItemRegion GetRegionName()  //�麯������������д
		{
			return ItemRegion.No ;
		}

		#endregion

		#region Visual��һЩ��������

		// �ҵ���visual������Item�������м���˶��visual
		public Item GetItem()
		{
			Item item = null;
			Visual visual = this;
			while(true)
			{
				if (visual == null)
					break;

				if (Visual.IsDerivedFromItem(visual) == true)
				{
					item = (Item)visual;
					break;
				}
				visual = visual.container;
			}
			// Debug.Assert(item != null, "");
			return item;
		}

        // ������
        // parameters:
        //      myRect              Rectangle����
        //      nTopBorderHeight    �Ϸ������ĸ߶�
        //      nBottomBorderHeight �·������ĸ߶�
        //      nLeftBorderWidth    �������Ŀ��
        //      nRightBorderWidth   �·������Ŀ��
        //      color               ��ɫ
        // return:
        //      void
		public void DrawLines(Rectangle myRect,
			int nTopBorderHeight,
			int nBottomBorderHeight,
			int nLeftBorderWidth,
			int nRightBorderWidth,
			Color color)
		{
			if (nTopBorderHeight < 0
				|| nBottomBorderHeight < 0
				|| nLeftBorderWidth < 0
				|| nRightBorderWidth < 0)
			{
				return;
			}

			if (nTopBorderHeight > myRect.Height 
				|| nBottomBorderHeight > myRect.Height)
			{
				/*
								Debug.Assert (false,"����:" + this.GetType ().Name 
									+ " ����:'" + this.strName + "'\r\n"
									+ "DrawLine�����ˮƽ�����߶�" + nBorderHorzHeight + "���ھ��θ߶�" + myRect.Height);
				*/
				return;
			}

			if (nLeftBorderWidth > myRect.Width
				|| nRightBorderWidth > myRect.Width)
			{
				/*
								Debug.Assert (false,"����:" + this.GetType ().Name 
									+ " ����:'" + this.strName + "'\r\n"
									+ "DrawLine����Ĵ�ֱ�������" + nBorderVertWidth + "���ھ��ο��" + myRect.Width );
				*/
				return;
			}

			Pen penLeft = null;  //��ߴ�ֱ�ֱ�
			Pen penRight = null;  // �ұߵĴ�ֱ�ֱ�
			Pen penTop = null;  //�Ϸ���ˮƽ�ֱ�
			Pen penBottom = null;  //�·���ˮƽ�ֱ�

			//penVert = new Pen(color, nBorderVertWidth);
			//penHorz = new Pen(color, nBorderHorzHeight);

			penLeft = new Pen(color,nLeftBorderWidth);
			penRight = new Pen(color,nRightBorderWidth);
			penTop = new Pen(color,nTopBorderHeight);
			penBottom = new Pen(color,nBottomBorderHeight);


			//int nHorzDelta = nBorderVertWidth / 2;
			//int nVertDelta = nBorderHorzHeight / 2;

			int nLeftDelta = nLeftBorderWidth / 2;
			int nRightDelta = nRightBorderWidth / 2;
			int nTopDelta = nTopBorderHeight / 2;
			int nBottomDelta = nBottomBorderHeight / 2 ;

			//int nHorzMode = nBorderVertWidth % 2;
			//int nVertMode = nBorderHorzHeight % 2;

			int nLeftMode = nLeftBorderWidth % 2;
			int nRightMode = nRightBorderWidth % 2;
			int nTopMode = nTopBorderHeight % 2;
			int nBottomMode = nBottomBorderHeight % 2;

			Rectangle rectMiddle = new Rectangle(
				myRect.X + nLeftDelta,
				myRect.Y + nTopDelta,
				myRect.Width  - nLeftDelta - nRightDelta,
				myRect.Height  - nTopDelta - nBottomDelta);

			Item item = this.GetItem ();
			if (item == null)
			{
				Debug.Assert (false,"DrawLine�ҵ���item Ϊnull");
				return;
			}

			XmlEditor editor = item.m_document;
			if (editor == null)
			{
				Debug.Assert (false,"DrawLine�ҵ���xmleditor Ϊnull");
				return;
			}

            using (Graphics g = Graphics.FromHwnd(editor.Handle))
            {

                //�Ϸ�
                if (nTopBorderHeight > 0)
                {
                    g.DrawLine(penTop,
                        rectMiddle.Left, rectMiddle.Top,
                        rectMiddle.Right, rectMiddle.Top);
                }

                //�·�
                if (nBottomBorderHeight > 0)
                {
                    g.DrawLine(penBottom,
                        rectMiddle.Left, rectMiddle.Bottom,
                        rectMiddle.Right, rectMiddle.Bottom);
                }

                int nLeftTemp = nLeftDelta + nLeftMode;
                if (nLeftBorderWidth == 1)
                {
                    if (nLeftMode == 0)
                        nLeftTemp = nLeftDelta - 1;
                    else
                        nLeftTemp = nLeftDelta;
                }
                //��
                if (nLeftBorderWidth > 0)
                {
                    g.DrawLine(penLeft,
                        rectMiddle.Left, rectMiddle.Top/* - nLeftDelta*/,
                        rectMiddle.Left, rectMiddle.Bottom/* + nLeftTemp*/);
                }

                int nRightTemp = nRightDelta + nRightMode;
                if (nRightBorderWidth == 1)
                {
                    if (nRightMode == 0)
                        nRightTemp = nRightDelta - 1;
                    else
                        nRightTemp = nRightDelta;
                }
                //�ҷ�
                if (nRightBorderWidth > 0)
                {
                    g.DrawLine(penRight,
                        rectMiddle.Right, rectMiddle.Top - nRightDelta,
                        rectMiddle.Right, rectMiddle.Bottom + nRightTemp);
                }
                if (penLeft != null)
                    penLeft.Dispose();
                if (penRight != null)
                    penRight.Dispose();
                if (penTop != null)
                    penTop.Dispose();
                if (penBottom != null)
                    penBottom.Dispose();
            }
		}
	
		#endregion

		#region ��̬����

        // ���ϵݹ鲼��
		public static void UpLayout(Visual visual,
			int nTimeStamp)
		{
			Box myContainer = (Box)(visual.container);
			if (myContainer != null)
			{
				int nMyRetWidth,nMyRetHeight;
				int nWidth = 0;
				int nHeight = 0;
				if (myContainer.LayoutStyle == LayoutStyle.Horizontal )
				{
					nWidth = 0;
					nHeight = 0;
					foreach(Visual child in myContainer.childrenVisual )
					{
						nWidth += child.Rect.Width ;

						child.Layout (child.Rect.X ,
							child.Rect.Y,
							child.Rect.Width ,
							0,
							nTimeStamp,
							out nMyRetWidth,
							out nMyRetHeight,
							LayoutMember.Layout );

						if (child.Rect.Height > nHeight)
							nHeight = child.Rect.Height ;
					}
					foreach(Visual child in myContainer.childrenVisual )
					{
						child.Rect.Height = nHeight;
					}
				}
				else if (myContainer.LayoutStyle == LayoutStyle.Vertical )
				{
					nWidth = visual.Rect.Width ;
					nHeight = visual.Rect.Height ;
					//�Ȱ��ֵܼ���һ��
					foreach(Visual child in myContainer.childrenVisual )
					{
						if (child.Equals (visual) == true)
							continue;

						child.Layout (child.Rect.X ,
							child.Rect.Y,
							visual.Rect.Width ,
							0,
							nTimeStamp,
							out nMyRetWidth,
							out nMyRetHeight,
							LayoutMember.Layout );

						if (child.Rect.Width > nWidth)
							nWidth = child.Rect.Width ;
						nHeight += child.Rect.Height ;
					}
				}

				myContainer.Rect.Width = nWidth + myContainer.TotalRestWidth;
				myContainer.Rect.Height = nHeight + myContainer.TotalRestHeight;
				//���ֵ�����
				int nXDelta = myContainer.LeftResWidth;
				int nYDelta = myContainer.TopResHeight;
				if (myContainer.LayoutStyle == LayoutStyle.Horizontal )
				{
					foreach(Visual childVisual in myContainer.childrenVisual )
					{
						childVisual.Rect.X = nXDelta;
						childVisual.Rect.Y = nYDelta;
						nXDelta += childVisual.Rect.Width ;
					}
				}
				else if (myContainer.LayoutStyle == LayoutStyle.Vertical  )
				{
					foreach(Visual childVisual in myContainer.childrenVisual )
					{
						childVisual.Rect.X = nXDelta;
						childVisual.Rect.Y = nYDelta;
						nYDelta += childVisual.Rect.Height;
					}
				}
				myContainer.Layout(myContainer.Rect.X,
					myContainer.Rect.Y,
					myContainer.Rect.Width,
					myContainer.Rect.Height,
					nTimeStamp,
					out nMyRetWidth,
					out nMyRetHeight,
					LayoutMember.Up );
			}
		}


        // �ı��ֵܲ���
        // parameters:
        //      startVisual ��ʼvisual
        //      nWidth      ��� -1��Ч
        //      nHeight     �߶� -1��Ч
        // return:
        //      void
		public static void ChangeSibling(Visual startVisual,
			int nWidth,
			int nHeight)
		{
			Box myContainer = (Box)(startVisual.container );
			if (myContainer == null)
				return;

			int nRetWidth,nRetHeight;

			if (nHeight != -1)
			{
				foreach(Visual child in myContainer.childrenVisual )
				{
					if (child.Equals (startVisual) == true)
						continue;
					child.Layout (child.Rect.X,
						child.Rect.Y,
						child.Rect.Width,
						nHeight,
						-1,
						out nRetWidth,
						out nRetHeight,
						LayoutMember.EnLargeHeight );
				}
			}

			if (nWidth != -1)
			{
				foreach(Visual child in myContainer.childrenVisual )
				{
					if (child.Equals (startVisual) == true)
						continue;
					child.Layout (child.Rect.X,
						child.Rect.Y,
						nWidth,
						child.Rect.Height,
						-1,
						out nRetWidth,
						out nRetHeight,
						LayoutMember.EnlargeWidth );
				}
			}

		}


		// �ж�visual�Ƿ��Item������
        // parameters:
        //      visual  Visual����
        // return:
        //      true    visual�Ǵ�Item������
        //      false   ����
		public static bool IsDerivedFromItem(Visual visual)
		{
			ArrayList aDeriveTypes = null;
			Type t = visual.GetType ();
			while(true)
			{
				if (t.Name == "Object")
					break;
				if (aDeriveTypes == null)
					aDeriveTypes = new ArrayList ();
				aDeriveTypes.Add (t );
				t = t.BaseType ;
			}

			if (aDeriveTypes != null)
			{
				for(int i= 0; i < aDeriveTypes.Count ;i++)
				{
					string strName1 = ((Type)aDeriveTypes[i]).FullName;
					string strName2 = typeof(Item).FullName;
					if (strName1 == strName2)
						return true;
				}
			}
			return false;
		}


		// �ж�visual�Ƿ��Box������
		public static bool IsDerivedFromBox(Visual visual)
		{
			ArrayList aDeriveTypes = null;
			Type t = visual.GetType ();
			while(true)
			{
				if (t.Name == "Object")
					break;
				if (aDeriveTypes == null)
					aDeriveTypes = new ArrayList ();
				aDeriveTypes.Add (t);
				t = t.BaseType ;
			}

			if (aDeriveTypes != null)
			{
				for(int i= 0; i < aDeriveTypes.Count ;i++)
				{
					string strName1 = ((Type)aDeriveTypes[i]).FullName;
					string strName2 = typeof(Box).FullName;
					if (strName1 == strName2)
						return true;
				}
			}
			return false;
		}
		
        // �ж�visual�Ƿ��TextVisual������
		public static bool IsDerivedFromTextVisual(Visual visual)
		{
			ArrayList aDeriveTypes = null;
			Type t = visual.GetType ();
			while(true)
			{
				if (t.Name == "Object")
					break;
				if (aDeriveTypes == null)
					aDeriveTypes = new ArrayList ();
				aDeriveTypes.Add (t);
				t = t.BaseType ;
			}

			if (aDeriveTypes != null)
			{
				for(int i= 0; i < aDeriveTypes.Count ;i++)
				{
					string strName1 = ((Type)aDeriveTypes[i]).FullName;
					string strName2 = typeof(TextVisual).FullName;
					if (strName1 == strName2)
						return true;
				}
			}
			return false;
		}
		#endregion

		#region ������һЩ�麯��

		// �жϵ�ǰ�����Ƿ���ExpandHandle
		public virtual bool IsExpandHandle()
		{
			return false;
		}

		// ��layoutMemberΪCalcuWidth����rectange��ֵ��ʵ�ʲ��ֲŸ�ֵ
        // parameters:
        //      x               x����
        //      y               y����
		//      nInitialWidth   ��ʼ���
		//      nInitialHeight  ��ʼ�߶�
        //      nRetWidth       ������Ҫ�Ŀ��
		//      nRectHeight     ������Ҫ�ĸ߶�
		//      layoutMember    ���ܲ���
		public virtual void Layout(int x,
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


			///////////////////////////////////////////
			//1.�����ж��Ƿ�ΪEnlarge����///////////////////
			///////////////////////////////////////////
			bool bEnlargeWidth = false;
			if ((layoutMember & LayoutMember.EnlargeWidth  ) == LayoutMember.EnlargeWidth  )
				bEnlargeWidth = true;

			bool bEnlargeHeight = false;
			if ((layoutMember & LayoutMember.EnLargeHeight ) == LayoutMember.EnLargeHeight )
				bEnlargeHeight = true;

			if (bEnlargeWidth == true
				|| bEnlargeHeight == true)
			{
				//���׺��ֵܶ�Ӱ����
				if ((layoutMember & LayoutMember.Up ) == LayoutMember.Up )
				{
					if (bEnlargeHeight == true)
					{
						this.Rect.Height = nInitialHeight;

						Box myContainer = (Box)(this.container );
						if (myContainer == null)
							return;
	
						if (myContainer.LayoutStyle == LayoutStyle.Horizontal )
						{
							//Ӱ���ֵ�
							foreach(Visual child in myContainer.childrenVisual )
							{
								if (child.Equals (this) == true)
									continue;

								child.Layout(
									child.Rect.X,
									child.Rect.Y,
									child.Rect.Width,
									this.Rect.Height,
									nTimeStamp,
									out nRetWidth,
									out nRetHeight,
									LayoutMember.EnLargeHeight );
							}

							int nMyHeight = this.Rect.Height;

							foreach(Visual child in myContainer.childrenVisual )
							{
								if (child.Rect.Height > nMyHeight)
									nMyHeight = child.Rect.Height ;
							}
							nMyHeight += myContainer.TotalRestHeight;

							myContainer.Layout (
								myContainer.Rect.X,
								myContainer.Rect.Y,
								myContainer.Rect.Width,
								nMyHeight,
								nTimeStamp,
								out nRetWidth,
								out nRetHeight,
								layoutMember);

						}

						if (myContainer.LayoutStyle == LayoutStyle.Vertical )
						{
							int nTempTotalHeight = 0;
							foreach(Visual childVisual in myContainer.childrenVisual )
							{
								nTempTotalHeight += childVisual.Rect.Height;
							}
							nTempTotalHeight += myContainer.TotalRestHeight;

							
							myContainer.Layout (
								myContainer.Rect.X,
								myContainer.Rect.Y,
								myContainer.Rect.Width,
								nTempTotalHeight,
								nTimeStamp,
								out nRetWidth,
								out nRetHeight,
								layoutMember);

							//���ֵ�����
							int nXDelta = myContainer.LeftResWidth;
							int nYDelta = myContainer.TopResHeight;
							foreach(Visual childVisual in myContainer.childrenVisual )
							{
								childVisual.Rect.X = nXDelta;
								childVisual.Rect.Y = nYDelta;
								nYDelta += childVisual.Rect.Height;
							}
						}
						return;
					}
				}
				if (bEnlargeHeight == true)
					this.Rect.Height  = nInitialHeight;

				if (bEnlargeHeight == true)
					this.Rect.Width = nInitialWidth;

				return;	
			}


			//2.������Ϣ///////////////////////////////////////
			Item item = this.GetItem();
			Debug.Assert(item != null, "");

			item.m_document.nTime ++;
			string strTempInfo = "";
			
			int nTempLevel = this.GetVisualLevel ();
			string strLevelString = this.GetStringFormLevel (nTempLevel);
			if (this.IsWriteInfo == true)
			{
				strTempInfo = "\r\n"
					+ strLevelString + "******************************\r\n"
					+ strLevelString + "���ǵ�" + nTempLevel + "���" + this.GetType ().Name + "��layout��ʼ\r\n" 
					+ strLevelString + "����Ϊ:\r\n"
					+ strLevelString + "x=" + x + "\r\n"
					+ strLevelString + "y=" + y + "\r\n"
					+ strLevelString + "nInitialWidth=" + nInitialWidth + "\r\n"
					+ strLevelString + "nInitialHeight=" + nInitialHeight + "\r\n"
					+ strLevelString + "nTimeStamp=" + nTimeStamp + "\r\n"
					+ strLevelString + "layoutMember=" + layoutMember.ToString () + "\r\n"
					+ strLevelString + "LayoutStyle=��\r\n"
					+ strLevelString + "ʹ�ܴ�����Ϊ" + item.m_document.nTime  + "\r\n";
				StreamUtil.WriteText ("I:\\debug.txt",strTempInfo);
			}


			//3.Catch///////////////////////////////////
			if (Catch == true)
			{
				//�����������ͬʱ��ֱ�ӷ���catch����
				if (sizeCatch.nInitialWidth == nInitialWidth
					&& sizeCatch.nInitialHeight == nInitialHeight
					&& (sizeCatch.layoutMember == layoutMember))
				{
					if (this.IsWriteInfo == true)
					{
						strTempInfo = "\r\n"
							+ strLevelString + "------------------"
							+ strLevelString + "�뻺��ʱ��ͬ\r\n"
							+ strLevelString + "�����ֵ: initialWidth:"+nInitialWidth + " initialHeight:" + nInitialHeight + " timeStamp: " + nTimeStamp + " layoutMember:" + layoutMember.ToString () + "\r\n"
							+ strLevelString + "�����ֵ: initialWidth:"+sizeCatch.nInitialWidth + " initialHeight:" + sizeCatch.nInitialHeight + " timeStamp: " + sizeCatch.nTimeStamp + " layoutMember:" + sizeCatch.layoutMember.ToString () + "\r\n";
					}

					if ((layoutMember & LayoutMember.Layout) != LayoutMember.Layout )
					{
						if (this.IsWriteInfo == true)
						{
							strTempInfo += strLevelString + "����ʵ����ֱ�ӷ��ػ�����ֵ\r\n";
						}

						nRetWidth = sizeCatch.nRetWidth  ;
						nRetHeight = sizeCatch.nRetHeight  ;

						if (this.IsWriteInfo == true)
						{
							strTempInfo +=   strLevelString + "----------����------\r\n";
							StreamUtil.WriteText ("I:\\debug.txt",strTempInfo);
						}

						goto END1;
					}
					else
					{
						if (this.IsWriteInfo == true)
						{
							strTempInfo += strLevelString + "����ʵ�������¼���\r\n";
						}
					}
					if (this.IsWriteInfo == true)
					{
						strTempInfo +=   strLevelString + "----------����------\r\n";
						StreamUtil.WriteText ("I:\\debug.txt",strTempInfo);
					}
				}
				else
				{
					if (this.IsWriteInfo == true)
					{
						strTempInfo = "\r\n"
							+ strLevelString + "------------------"
							+ strLevelString + "�뻺��ʱ��ͬ\r\n"
							+ strLevelString + "�����ֵ: initialWidth:"+nInitialWidth + " initialHeight:" + nInitialHeight + " timeStamp: " + nTimeStamp + " layoutMember:" + layoutMember.ToString () + "\r\n"
							+ strLevelString + "�����ֵ: initialWidth:"+sizeCatch.nInitialWidth + " initialHeight:" + sizeCatch.nInitialHeight + " timeStamp: " + sizeCatch.nTimeStamp + " layoutMember:" + sizeCatch.layoutMember.ToString () + "\r\n";

						strTempInfo +=   strLevelString + "----------����------\r\n";
						StreamUtil.WriteText ("I:\\debug.txt",strTempInfo);
					}
				}
			}


			//4.������и��������ʵ��//////////////////////////

			nRetWidth = nInitialWidth;
			nRetHeight = nInitialHeight;

			//�õ����
			int nTempWidth = GetWidth();
/*
			if (nRetWidth < 0) //(nTempWidth > nRetWidth)
				nRetWidth = 0;
				//nRetWidth = nTempWidth;
*/
			if (nRetWidth < nTempWidth)
				nRetWidth = nTempWidth;

			//1)ֻ����
			if (layoutMember == LayoutMember.CalcuWidth)   //����
				goto END1;

			//�õ��߶�
			int nTempHeight = GetHeight(nRetWidth
				- this.TotalRestWidth);

			if (nTempHeight > nRetHeight )
				nRetHeight = nTempHeight;
			if (nRetHeight < 0)
				nRetHeight = 0;

			//2)��߶�(���������ֻ��߶� �� ����߶��ֲ���)
			if ((layoutMember & LayoutMember.CalcuHeight ) == LayoutMember.CalcuHeight )
				goto END1;

			//3)ʵ��
			if ((layoutMember & LayoutMember.Layout) == LayoutMember.Layout )  //��������
			{
				this.Rect = new Rectangle (x,
					y,
					nRetWidth,
					nRetHeight);

				//�ѿ�ȼǵ�������
				item.SetValue (this.GetType().Name,
					nRetWidth);

				//goto END1;
			}

			if ((layoutMember & LayoutMember.Up  ) == LayoutMember.Up )
			{
				Visual.UpLayout(this,nTimeStamp);
			}


			END1:

				//***����catch***
				sizeCatch.SetValues (nInitialWidth,
					nInitialHeight,
					nRetWidth,
					nRetHeight,
					nTimeStamp,
					layoutMember);

			if (this.IsWriteInfo == true)
			{
				strTempInfo = "";
				strTempInfo = "\r\n"
					+ strLevelString + "���ǵ�" + nTempLevel + "���" + this.GetType ().Name + "��layout����\r\n" 
					+ strLevelString + "����ֵΪ: \r\n"
					+ strLevelString + "x=" + x + "\r\n"
					+ strLevelString + "y=" + y + "\r\n"
					+ strLevelString + "nRetWidth=" + nRetWidth + "\r\n"
					+ strLevelString + "nRetHeight=" + nRetHeight + "\r\n"
					+ strLevelString + "Rect.X=" + this.Rect.X + "\r\n"
					+ strLevelString + "Rect.Y=" + this.Rect.Y + "\r\n"
					+ strLevelString + "Rect.Width=" + this.Rect.Width + "\r\n"
					+ strLevelString + "Rect.Height=" + this.Rect.Height + "\r\n"
					+ strLevelString + "****************************\r\n\r\n" ;

				StreamUtil.WriteText ("I:\\debug.txt",strTempInfo);
			}
		}

        // ���
		public virtual int GetWidth()
		{
			return 0;
		}

        // �߶�
		public virtual int GetHeight(int nWidth)
		{
			return 0;
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
		public virtual int HitTest(Point p,
			out Visual retVisual)
		{
			retVisual = null;
			return -1;
		}


		// ��ͼ
        // parameters:
        //      pe          PaintEventArgs����
		//      nBaseX      x������
		//      nBaseY      y������ 
		//      paintMember ���Ƴ�Ա:content,border,both
        // return:
        //      void
		public virtual void Paint(PaintEventArgs pe,
			int nBaseX,
			int nBaseY,
			PaintMember paintMember)
		{
		}

		// �������rect��Ϣ
		public virtual void WriteRect(string strName)
		{
		}

		
		#endregion
	}





	//�������������
	public class SizeCatch
	{
		public object data = null;

		public int nInitialWidth = -1;
		public int nInitialHeight = -1;

		public int nRetWidth = -1;
		public int nRetHeight = -1;

		public int nTimeStamp = -1;

		public LayoutMember layoutMember = LayoutMember.None ;

		public void SetValues(int initialWidth,
			int initialHeight,
			int retWidth,
			int retHeight,
			int tempStamp,
			LayoutMember mylayoutMember)
		{
			nInitialWidth = initialWidth;
			nInitialHeight = initialHeight;

			nRetWidth = retWidth;
			nRetHeight = retHeight;

			nTimeStamp = tempStamp;
			this.layoutMember = mylayoutMember;
		}
	}

	//������ʱvisual��ȵ���
	public class PartInfo
	{
		public string strName;
		public int  nWidth;
		public int nHeight;
	}
}
