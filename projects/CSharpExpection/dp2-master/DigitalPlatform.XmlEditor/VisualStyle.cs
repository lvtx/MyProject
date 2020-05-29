using System;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using DigitalPlatform;
using DigitalPlatform.Xml;
using DigitalPlatform.Text;
using DigitalPlatform.IO;
using DigitalPlatform.Drawing;


namespace DigitalPlatform.Xml
{
	// ��ʽ������
	public class VisualCfg
	{
		public string strXml = "";
		public string m_debugInfo = "";

		public Color transparenceColor = Color.FromArgb (127,128,129);
		public string strBackPicUrl = "";
		public BackPicStyle backPicStyle = BackPicStyle.Fill ;

		// �����ĸ�list
		public CaseList caseList = new CaseList ();
		public VisualStyleList visualStyleList = new VisualStyleList ();
		public FontList fontList = new FontList ();
		public BorderList borderList = new BorderList ();

		// ��ʼ��list
		public void Initial()
		{
			caseList.cfg = this;
			visualStyleList.cfg = this;

			XmlDocument dom = new XmlDocument ();
			dom.LoadXml (strXml);

			//��վ�ֵ
			caseList.Clear ();
			visualStyleList.Clear ();
			fontList.Clear ();
			borderList.Clear ();

			XmlNode root = dom.DocumentElement ;


			XmlNode nodeGlobal = root.SelectSingleNode ("//global");
			string strColor = DomUtil.GetAttrDiff (nodeGlobal,"transparenceColor");
			if (strColor != null && strColor != "")
			{
				transparenceColor = ColorUtil.String2Color (strColor);
			}
		
			string myBackPicUrl = DomUtil.GetAttrDiff (nodeGlobal,"backPicture");
			if (myBackPicUrl != null 
				&& strColor != "")
			{
				strBackPicUrl = myBackPicUrl;
			}

			string strBackPicStyle = DomUtil.GetAttrDiff (nodeGlobal,"backPicStyle");
			if (strBackPicStyle != null
				&& strBackPicStyle != "")
			{
				backPicStyle = (BackPicStyle)(Enum.Parse (typeof(BackPicStyle),strBackPicStyle));
			}

			borderList.Initial (root);
			fontList.Initial (root);
			visualStyleList.Initial (root);
			caseList.Initial (root);

			visualStyleList.InitailBelongList();  //��ʼ��belongList

			//��ʼ����parallelNodes��Ա��û�����ˣ���ʡ�ռ�
			visualStyleList.parallelNodes = null;
			fontList.parallelNodes = null;
			borderList.parallelNodes = null;
		}

		public VisualStyle GetVisualStyle(string strElement,
			string strLevel,
			string strXpath,
			string strRegion)
		{
			ItemRegion region = ItemRegion.No;
			region =  (ItemRegion)Enum.Parse(typeof(ItemRegion), strRegion,true);

			return caseList.GetVisualStyle(strElement,
				strLevel,
				strXpath,
				region);
		}


		public VisualStyle GetVisualStyle(Item item,
			ItemRegion region)
		{
			return caseList.GetVisualStyle(item.Name,
				Convert.ToString (item.GetLevel()),
				"",
				region);
		}


		// ֱ�Ӵ�visualStyleList������
		public VisualStyle GetVisualStyle(string strName)
		{
			return visualStyleList.GetVisualStyle(strName);
		}


		public string Xml
		{
			get 
			{
				return "";
			}
			set
			{
				strXml = value;
				Initial();
			}
		}


		//��������
		public string ShowList(string strListName)
		{
			string strResult = "";
			if (strListName == "case")
				strResult = caseList.Dump();
			else if (strListName == "visualstyle")
				strResult = visualStyleList.Dump();
			else if (strListName == "font")
				strResult = fontList.Dump();
			else if (strListName == "border")
				strResult = borderList.Dump();
			
			return strResult;
		}

	}


	#region Case��

	// CaseList //////////////////////////////////////////
	public class CaseList : CollectionBase
	{
		public VisualCfg cfg = null;   //VisualCfgָ��,���ڴ�����visualStyleListȡֵ

		public Case this[int index]
		{
			get 
			{
				return (Case)InnerList[index];
			}
			set
			{
				InnerList[index] = value;
			}
		}

		public void Add(Case item)
		{
			InnerList.Add(item);
		}

		public void Initial(XmlNode root)
		{
			XmlNodeList nodeList = root.SelectNodes("//case");
			for(int i=0;i<nodeList.Count ;i++)
			{
				Case oneCase = new Case();
				oneCase.container = this;
				oneCase.CreateBy (nodeList[i]);

				this.Add (oneCase );
			}
		}

		// ����Ƿ�ö����Ƿ����ָ��������
		bool isInRegion(ItemRegion region,Case myCase)
		{
			if ((region & ItemRegion.Frame) == ItemRegion.Frame)
			{
				if ((ItemRegion.Frame & myCase.region ) == ItemRegion.Frame)
					return true;
			}
			if ((region & ItemRegion.Label) == ItemRegion.Label)
			{
				if ((ItemRegion.Label & myCase.region) == ItemRegion.Label)
					return true;
			}

			if ((region & ItemRegion.Text   )  == ItemRegion.Text   )
			{
				if ((ItemRegion.Text  & myCase.region) == ItemRegion.Text   )
					return true;
			}
			if ((region & ItemRegion.Content)  == ItemRegion .Content)
			{
				if ((ItemRegion.Content & myCase.region) == ItemRegion.Content)
					return true;
			}	
			if ((region & ItemRegion.Comment)  == ItemRegion.Comment)
			{
				if ((ItemRegion.Comment & myCase.region) == ItemRegion.Comment)
					return true;
			}
			if ((region & ItemRegion.Attributes  )  == ItemRegion.Attributes  )
			{
				if ((ItemRegion.Attributes   & myCase.region) == ItemRegion.Attributes  )
					return true;
			}
			if ((region & ItemRegion.ExpandAttributes )== ItemRegion.ExpandAttributes)
			{
				if ((ItemRegion.ExpandAttributes & myCase.region) == ItemRegion.ExpandAttributes)
					return true;
			}
			if ((region & ItemRegion.ExpandContent)  == ItemRegion.ExpandContent )
			{
				if ((ItemRegion.ExpandContent & myCase.region) == ItemRegion.ExpandContent)
					return true;
			}			
			if ((region & ItemRegion.BoxTotal  )  == ItemRegion.BoxTotal    )
			{
				if ((ItemRegion.BoxTotal  & myCase.region) == ItemRegion.BoxTotal )
					return true;
			}
			if ((region & ItemRegion.BoxAttributes )  == ItemRegion.BoxAttributes )
			{
				if ((ItemRegion.BoxAttributes & myCase.region) == ItemRegion.BoxAttributes)
					return true;
			}
			if ((region & ItemRegion.BoxContent   )  == ItemRegion.BoxContent )
			{
				if ((ItemRegion.BoxContent & myCase.region) == ItemRegion.BoxContent )
					return true;
			}

			return false;
		}

		// �����������ı���ʽ���Ƿ�����
        // parameters:
        //      strText         �ı��ַ���
        //      strExpression   ʽ��
		bool IsMatch(string strText,
            string strExpression)
		{
			if (strExpression == "" || strExpression == "*")
				return true;
			if (strText == strExpression)
				return true;
			if (StringUtil.IsInList (strText,strExpression) == true)
				return true;
			if (RegexCompare(strText,strExpression) == true)
				return true;

			return false;
		}

		// �Ƚ�һ���ַ���ʵ���Ƿ���������ʽƥ��
		public bool RegexCompare(string strInstance,string strPattern)	
		{
			Regex  r = new Regex(strPattern, RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Match m = r.Match(strInstance);
			if (m.Success)
				return true;
			else
				return false;
		}

		public VisualStyle GetVisualStyle(string strElement,
			string strLevel,
			string strXpath,
			ItemRegion region)
		{
			for(int i=0;i<this.Count ;i++)
			{
				Case myCase = this[i];
				if (IsMatch(strElement,myCase.strElement) == true &&
					IsMatch(strLevel,myCase.strLevel) == true &&
					strXpath == myCase.strXpath  &&
					isInRegion (region,myCase) == true)
				{
					return myCase.Style ;
				}
			}
			return null;
		}

		public string Dump()
		{
			string strResult = "";
			foreach(Case mycase in this)
			{
				strResult += mycase.Dump ();
			}
			return strResult;
		}

	}

	public class Case 
	{
		public CaseList container = null;
		public string strName = null;

		public string strElement = null;
		public string strLevel = null;
		public string strXpath = null;
		//public string strRegion = null;

		public ItemRegion region = ItemRegion.No ;


		public VisualStyle refStyle = null;   //���õ�
		public VisualStyle innerStyle = null; //�ڲ������

		public Case ()
		{}

		public VisualStyle Style
		{
			get
			{
				if (refStyle != null)
					return refStyle ;
				if (innerStyle != null)
					return innerStyle;
				return null;
			}
		}

		public ItemRegion SplitStr2Enum(string strList)
		{
			ItemRegion regionResult = ItemRegion.No ;

			string[] aList;
			aList = strList.Split(new char[]{','});

			for(int i=0;i<aList.Length;i++)
			{
				if (String.Compare("frame",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.Frame ;
				}
				if (String.Compare("label",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.Label  ;
				}
				if (String.Compare("text",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.Text   ;
				}
				if (String.Compare("content",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.Content  ;
				}
				if (String.Compare("comment",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.Comment  ;
				}
				if (String.Compare("attributes",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.Attributes     ;
				}
				if (String.Compare("expandAttributes",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.ExpandAttributes;
				}
				if (String.Compare("expandContent",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.ExpandContent;
				}
				if (String.Compare("boxTotal",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.BoxTotal ;
				}
				if (String.Compare("boxAttributes",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.BoxAttributes  ;
				}
				if (String.Compare("boxContent",aList[i],true) == 0) 
				{
					regionResult |= ItemRegion.BoxContent;
				}
			}

			return regionResult;

		}

		public void  CreateBy(XmlNode node)
		{
			strName = DomUtil.GetAttrDiff (node,"name");

			strElement = DomUtil.GetAttrDiff (node,"element");
			strLevel = DomUtil.GetAttrDiff (node,"level");
			strXpath = DomUtil.GetAttrDiff (node,"xpath");
			
			string strRegion = DomUtil.GetAttrDiff (node,"region");
			region = SplitStr2Enum(strRegion);


			string strStyleName = DomUtil.GetAttrDiff (node,"style");
			if (strStyleName != null)
			{
				refStyle = container.cfg.visualStyleList.GetVisualStyle(strStyleName);
			}

			XmlNode nodeStyle = node.SelectSingleNode ("style");
			//���ڰ�����Case�����Styleע��Ҫ��new�����ӵ�������
			if (nodeStyle != null)
			{
				innerStyle = container.cfg.visualStyleList.GetVisualStyle(nodeStyle);
			}
		}

		public string Dump()
		{
			string strInfo = "\r\n";
			strInfo += "strName:"+strName + "\r\n";
			strInfo += "strElement:"+strElement+ "\r\n";
			strInfo += "strLevel:"+strLevel+ "\r\n";
			strInfo += "strXpath:"+strXpath+ "\r\n";
			//strInfo += "strRegion:"+strRegion+ "\r\n";

			if (refStyle != null)
				strInfo += "refStyle:" + refStyle.strName + "\r\n";
			else
				strInfo += "refStyle:null\r\n";

			if (innerStyle != null)
				strInfo += "innerStyle:" + innerStyle.strName + "\r\n";
			else
				strInfo += "innerStyle:null\r\n";


			return strInfo;
		}
	}


	#endregion

	#region VisualStyle��

	//VisualStyleList ///////////////////////////////////
	public class VisualStyleList : CollectionBase
	{
		public VisualCfg cfg = null;   //ָ��,������font���󣬻�border����

		//�����Ҷ���
		public ArrayList parallelNodes = new ArrayList ();


		public VisualStyle this[int index]
		{
			get 
			{
				return (VisualStyle)InnerList[index];
			}
			set
			{
				InnerList[index] = value;
			}
		}

		public void Add(VisualStyle item)
		{
			InnerList.Add(item);
		}

		public void Initial(XmlNode root)
		{
			XmlNodeList nodeList = root.SelectNodes("//style");
			for(int i=0;i<nodeList.Count ;i++)
			{
				VisualStyle style = new VisualStyle();
				style.container = this;
				style.CreateBy (nodeList[i]);
				this.Add (style);

				if (parallelNodes == null)
					parallelNodes = new ArrayList ();
				parallelNodes.Add (nodeList[i]);
			}
		}

		public VisualStyle GetVisualStyle(string strMyName)
		{
			foreach(VisualStyle style in this)
			{
				if (style.strName == strMyName)
					return style;
			}
			return null;
		}

		public VisualStyle GetVisualStyle(XmlNode nodeStyle)
		{
			for(int i=0;i<parallelNodes .Count ;i++)
			{
				XmlNode node = (XmlNode)parallelNodes[i];
				if (node.Equals (nodeStyle) == true)
					return this[i];
			}
			return null;
		}


		public void InitailBelongList()
		{
			foreach(VisualStyle style in this)
			{
				style.InitailBelongList();
			}
		}

		public string Dump()
		{
			string strInfo = "";
			foreach(VisualStyle visualStyle in this)
			{
				strInfo += visualStyle.Dump ();
			}
			return strInfo;
		}

	}

	public class VisualStyle 
	{
		//public string strDebugInfo = "";  //������Ϣ

		public VisualStyleList container = null;
		public string strName = null;

		object nTopBlank = null;         //int
		object nBottomBlank = null;      //int
		object nLeftBlank = null;        //int
		object nRightBlank = null;       //int

		object cBackColor = null;       //color
		object cTextColor = null;

		DpFont refFont = null;
		XmlBorder refBorder = null;
		DpFont innerFont = null;
		XmlBorder innerBorder = null;

		string strBelongName = null;
		ArrayList belongList = null;	// �̳еķ��

		public VisualStyle()
		{
		}


		public int TopBlank
		{
			get
			{
				object topBlank = GetValue(ValueStyle.TopBlank);
				if (topBlank != null)
					return (int)topBlank;
				else
					return 0 ;
			}
		}

		public int BottomBlank
		{
			get
			{
				object bottomBlank = GetValue(ValueStyle.BottomBlank );
				if (bottomBlank != null)
					return (int)bottomBlank;
				else
					return 0 ;
			}
		}

		public int LeftBlank
		{
			get
			{
				object leftBlank = GetValue(ValueStyle.LeftBlank );
				if (leftBlank != null)
					return (int)leftBlank;
				else
					return 0;
			}
		}

		public int RightBlank
		{
			get
			{
				object rightBlank = GetValue(ValueStyle.RightBlank );
				if (rightBlank != null)
					return (int)rightBlank;
				else
					return 0;
			}
		}

		public Color BackColor
		{
			get
			{
				object backColor = GetValue(ValueStyle.BackColor);
				if (backColor != null)
					return (Color)backColor;
				else
					return container.cfg .transparenceColor;//Color.Gray ;
			}
		}

		public Color TextColor
		{
			get
			{
				object textColor = GetValue(ValueStyle.TextColor );
				if (textColor != null)
					return (Color)textColor;
				else
					return Color.Red  ;
			}
		}

		public string FontFace
		{
			get
			{
				object strFontFace = GetValue(ValueStyle.FontFace);
				if (strFontFace != null)
					return (string)strFontFace;
				else
					return "Arial";
			}
		}

		public int FontSize
		{
			get
			{
				object fontSize = GetValue(ValueStyle.FontSize);
				if (fontSize != null)
					return (int)fontSize;
				else
					return 10;
			}
		}

		public FontStyle FontStyle
		{
			get
			{
				object fontStyle = GetValue(ValueStyle.FontStyle);
				if (fontStyle != null)
					return (FontStyle)fontStyle;
				else 
					return FontStyle.Regular;
			}
		}

		public Font Font
		{
			get
			{
				//��������������
				return (new Font (FontFace,FontSize,FontStyle));
			}
		}

		public int TopBorderHeight
		{
			get
			{
				object vertWidth = GetValue(ValueStyle.TopBorderHeight);
				if (vertWidth != null)
					return (int)vertWidth;
				else
					return 0;
			}
		}

		public int BottomBorderHeight
		{
			get
			{
				object bottomHeight = GetValue(ValueStyle.BottomBorderHeight);
				if (bottomHeight != null)
					return (int)bottomHeight;
				else
					return 0;
			}
		}

		public int LeftBorderWidth
		{
			get
			{
				object leftWidth = GetValue(ValueStyle.LeftBorderWidth);
				if (leftWidth != null)
					return (int)leftWidth;
				else
					return 0;
			}
		}

		public int RightBorderWidth
		{
			get
			{
				object rightWidth = GetValue(ValueStyle.RightBorderWidth);
				if (rightWidth != null)
					return (int)rightWidth;
				else
					return 0;
			}
		}

		public Color BorderColor
		{
			get
			{
				object color = GetValue(ValueStyle.BorderColor);
				if (color != null)
					return (Color)color;
				else
					return Color.LightGray; // Gray
			}
		}


		public object GetValue(ValueStyle valueStyle)
		{
			//strDebugInfo = ""; //�����һ�£������Խ��Խ��
			object oResult = null;

			if (valueStyle == ValueStyle.TopBlank )
			{
				oResult = nTopBlank;
				if (oResult != null)
				{
					//strDebugInfo += this.strName  +"��nTopBlank�о����ֵΪ"+Convert.ToString (nTopBlank)+"\r\n" ;
					goto END1;
				}
				//strDebugInfo += this.strName  +"��nTopBlankΪnull,��belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.BottomBlank )
			{
				oResult = nBottomBlank;
				if (oResult != null)
				{
					//strDebugInfo += this.strName  +"��nBottomBlank�о����ֵΪ"+Convert.ToString (nBottomBlank)+"\r\n";
					goto END1;
				}
				//strDebugInfo += this.strName  +"��nBottomBlankΪnull,��belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.LeftBlank )
			{
				oResult = nLeftBlank;
				if (oResult != null)
				{
					//strDebugInfo += this.strName  +"��nLeftBlank�о����ֵΪ"+Convert.ToString (nLeftBlank)+"\r\n";
					goto END1;
				}
				//strDebugInfo += this.strName  +"��nLeftBlankΪnull,��belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.RightBlank )
			{
				oResult = nRightBlank;
				if (oResult != null)
				{
					//strDebugInfo += this.strName  +"��nRightBlank�о����ֵΪ"+Convert.ToString (nRightBlank)+"\r\n";
					goto END1;
				}
				//strDebugInfo += this.strName  +"��nRightBlankΪnull,��belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.BackColor )
			{
				oResult = this.cBackColor;
				if (oResult != null)
				{
					//strDebugInfo += this.strName  +"��cBackColor�о����ֵΪ"+ColorUtil.Color2String((Color)oResult)+"\r\n";
					goto END1;
				}
				//strDebugInfo += this.strName  +"��cBackColorΪnull,��belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.TextColor )
			{
				oResult = this.cTextColor ;
				if (oResult != null)
				{
					//strDebugInfo += this.strName  +"��cTextColor�о����ֵΪ"+ColorUtil.Color2String ((Color)oResult)+"\r\n";
					goto END1;
				}
				//strDebugInfo += this.strName  +"��cTextColorΪnull,��belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.FontFace)
			{
				if (refFont != null)
				{
					//strDebugInfo += this.strName +"��refFont�о����ֵ:"+ refFont.Dump () + "\r\n";
					oResult = refFont.strFace ;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�strFace�о����ֵΪ"+refFont.strFace + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�strFaceΪnull�����Լ�����\r\n";
				}
				
				if (innerFont != null)
				{
					//strDebugInfo += this.strName +"��innerFont�о����ֵ:"+innerFont.Dump () + "\r\n";
					oResult = innerFont.strFace ;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�strFace�о����ֵΪ"+innerFont.strFace + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�strFaceΪnull�����Լ�����\r\n";
				}
				//strDebugInfo += this.strName +"��refFont��innerFont��û�ҵ�strFaceֵ����belongList����\r\n";
			}
			else  if (valueStyle == ValueStyle.FontSize)
			{
				if (refFont != null)
				{
					//strDebugInfo += this.strName +"��refFont�о����ֵ:"+refFont.Dump () + "\r\n";

					oResult = refFont.nSize  ;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nSize�о����ֵΪ"+refFont.nSize + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nSizeΪnull�����Լ�����\r\n";
				}
				
				if (innerFont != null)
				{
					//strDebugInfo += this.strName +"��innerFont�о����ֵ:"+innerFont.Dump () + "\r\n";

					oResult = innerFont.nSize ;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nSize�о����ֵΪ"+innerFont.nSize + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nSizeΪnull�����Լ�����\r\n";
				}
				//strDebugInfo += this.strName +"��refFont��innerFont��û�ҵ�nSizeֵ����belongList����\r\n";
			}
			else  if (valueStyle == ValueStyle.FontStyle )
			{
				if (refFont != null)
				{
					//strDebugInfo += this.strName +"��refFont�о����ֵ:"+refFont.Dump () + "\r\n";

					oResult = refFont.style;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nSize�о����ֵΪ"+refFont.nSize + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nSizeΪnull�����Լ�����\r\n";
				}
				
				if (innerFont != null)
				{
					//strDebugInfo += this.strName +"��innerFont�о����ֵ:"+innerFont.Dump () + "\r\n";

					oResult = innerFont.style  ;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nSize�о����ֵΪ"+innerFont.nSize + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nSizeΪnull�����Լ�����\r\n";
				}
				//strDebugInfo += this.strName +"��refFont��innerFont��û�ҵ�nSizeֵ����belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.LeftBorderWidth)
			{
				if (refBorder != null)
				{
					//strDebugInfo += this.strName +"��refBorder�о����ֵ:"+refBorder.Dump () + "\r\n";
					oResult = refBorder.nLeftWidth;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nVertWidth�о����ֵΪ"+refBorder.nVertWidth + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nVertWidthΪnull�����Լ�����\r\n";
				}
				
				if (innerBorder != null)
				{
					//strDebugInfo += this.strName +"��innerBorder�о����ֵ:"+innerBorder.Dump () + "\r\n";
					oResult = innerBorder.nLeftWidth;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nVertWidth�о����ֵΪ"+innerBorder.nVertWidth + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nVertWidthΪnull�����Լ�����\r\n";
				}
				//strDebugInfo += this.strName +"��refBorder��innerBorder��û�ҵ�nVertWidthֵ����belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.RightBorderWidth)
			{
				if (refBorder != null)
				{
					//strDebugInfo += this.strName +"��refBorder�о����ֵ:"+refBorder.Dump () + "\r\n";
					oResult = refBorder.nRightWidth;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nHorzHeight�о����ֵΪ"+refBorder.nHorzHeight + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nHorzHeightΪnull�����Լ�����\r\n";
				}
				
				if (innerBorder != null)
				{
					//strDebugInfo += this.strName +"��innerBorder�о����ֵ:"+innerBorder.Dump () + "\r\n";
					oResult = innerBorder.nRightWidth;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nHorzHeight�о����ֵΪ"+innerBorder.nHorzHeight + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nHorzHeightΪnull�����Լ�����\r\n";
				}
				//strDebugInfo += this.strName +"��refBorder��innerBorder��û�ҵ�nHorzHeightֵ����belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.TopBorderHeight)
			{
				if (refBorder != null)
				{
					//strDebugInfo += this.strName +"��refBorder�о����ֵ:"+refBorder.Dump () + "\r\n";
					oResult = refBorder.nTopHeight;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nHorzHeight�о����ֵΪ"+refBorder.nHorzHeight + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nHorzHeightΪnull�����Լ�����\r\n";
				}
				
				if (innerBorder != null)
				{
					//strDebugInfo += this.strName +"��innerBorder�о����ֵ:"+innerBorder.Dump () + "\r\n";
					oResult = innerBorder.nTopHeight;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nHorzHeight�о����ֵΪ"+innerBorder.nHorzHeight + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nHorzHeightΪnull�����Լ�����\r\n";
				}
				//strDebugInfo += this.strName +"��refBorder��innerBorder��û�ҵ�nHorzHeightֵ����belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.BottomBorderHeight)
			{
				if (refBorder != null)
				{
					//strDebugInfo += this.strName +"��refBorder�о����ֵ:"+refBorder.Dump () + "\r\n";
					oResult = refBorder.nBottomHeight;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nHorzHeight�о����ֵΪ"+refBorder.nHorzHeight + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nHorzHeightΪnull�����Լ�����\r\n";
				}
				
				if (innerBorder != null)
				{
					//strDebugInfo += this.strName +"��innerBorder�о����ֵ:"+innerBorder.Dump () + "\r\n";
					oResult = innerBorder.nBottomHeight;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�nHorzHeight�о����ֵΪ"+innerBorder.nHorzHeight + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�nHorzHeightΪnull�����Լ�����\r\n";
				}
				//strDebugInfo += this.strName +"��refBorder��innerBorder��û�ҵ�nHorzHeightֵ����belongList����\r\n";
			}
			else if (valueStyle == ValueStyle.BorderColor )
			{
				if (refBorder != null)
				{
					//strDebugInfo += this.strName +"��refBorder�о����ֵ:"+refBorder.Dump () + "\r\n";
					oResult = refBorder.color ;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�color�о����ֵΪ"+ColorUtil.Color2String ((Color)refBorder.color) + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�colorΪnull�����Լ�����\r\n";
				}
				
				if (innerBorder != null)
				{
					//strDebugInfo += this.strName +"��innerBorder�о����ֵ:" + innerBorder.Dump () + "\r\n";
					oResult = innerBorder.color;
					if (oResult != null)
					{
						//strDebugInfo += "�������е�color�о����ֵΪ" + ColorUtil.Color2String((Color)innerBorder.color) + "\r\n";
						goto END1;
					}
					//strDebugInfo += "�����е�colorΪnull�����Լ�����\r\n";
				}
				//strDebugInfo += this.strName +"��refBorder��innerBorder��û�ҵ�colorֵ����belongList����\r\n";
			}

			oResult = GetValueFormBList(valueStyle);
		
			END1:
				//container.m_strDebugInfo += strDebugInfo;
				//FileUtil.WriteText ("L:debug.txt",strDebugInfo);
			return oResult;
		}


		// ΪGetValue˽�еĺ���
		object GetValueFormBList(ValueStyle valueStyle)
		{
			if (belongList == null)
			{
				//strDebugInfo += this.strName  +"��belongListΪnull\r\n";
				return null;
			}

			object oResult = null;
			for(int i=0;i<belongList.Count ;i++)
			{
				VisualStyle style = (VisualStyle)belongList[i];
				if (valueStyle == ValueStyle.TopBlank )
				{
					if (style.nTopBlank != null)
					{
						oResult = style.nTopBlank ; 
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�nTopBlank��,ֵΪ"+Convert.ToString (oResult)+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��nTopBlankΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.BottomBlank )
				{
					if (style.nBottomBlank != null)
					{
						oResult = style.nBottomBlank ; 
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�nBottomBlank��,ֵΪ"+Convert.ToString (oResult)+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��nBottomBlankΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.LeftBlank )
				{
					if (style.nLeftBlank != null)
					{
						oResult = style.nLeftBlank ; 
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�nLeftBlank��,ֵΪ"+Convert.ToString (oResult)+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��nLeftBlankΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.RightBlank )
				{
					if (style.nRightBlank != null)
					{
						oResult = style.nRightBlank ; 
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�nRightBlank��,ֵΪ"+Convert.ToString (oResult)+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��nRightBlankΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.BackColor )
				{
					if (style.cBackColor != null)
					{
						oResult = style.cBackColor ; 
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�cBackColor��,ֵΪ"+ColorUtil.Color2String ((Color)oResult)+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��cBackColorΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.TextColor )
				{
					if (style.cTextColor != null)
					{
						oResult = style.cTextColor ; 
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�cTextColor��,ֵΪ"+ ColorUtil.Color2String ((Color)oResult)+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��cTextColorΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.FontFace )
				{
					oResult = style.FontFace ;
					if (oResult != null)
					{
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�FontFace��,ֵΪ"+oResult+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��FontFaceΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.FontSize)
				{
					oResult = style.FontSize ;
					if (oResult != null)
					{
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�FontSize��,ֵΪ"+oResult+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��FontSizeΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.FontStyle )
				{
					oResult = style.FontStyle  ;
					if (oResult != null)
					{
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�FontSize��,ֵΪ"+oResult+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��FontSizeΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.TopBorderHeight)
				{
					oResult = style.TopBorderHeight ;
					if (oResult != null)
					{
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�BorderVertWidth��,ֵΪ"+oResult+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��BorderVertWidthΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.BottomBorderHeight)
				{
					oResult = style.BottomBorderHeight;
					if (oResult != null)
					{
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�BorderHorzHeight��,ֵΪ"+oResult+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��BorderHorzHeightΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.LeftBorderWidth)
				{
					oResult = style.LeftBorderWidth  ;
					if (oResult != null)
					{
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�BorderHorzHeight��,ֵΪ"+oResult+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��BorderHorzHeightΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.RightBorderWidth)
				{
					oResult = style.RightBorderWidth;
					if (oResult != null)
					{
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�BorderHorzHeight��,ֵΪ"+oResult+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��BorderHorzHeightΪnull,������belongList����\r\n";
				}
				else if (valueStyle == ValueStyle.BorderColor )
				{
					oResult = style.BorderColor    ;
					if (oResult != null)
					{
						//strDebugInfo += "    " + Convert.ToString (i) +": ��"+style.strName +"���ҵ�BorderColor��,ֵΪ" + ColorUtil.Color2String ((Color)oResult)+"\r\n";
						break;
					}
					//strDebugInfo += "    " + Convert.ToString (i) +": "+style.strName +"��BorderColorΪnull,������belongList����\r\n";
				}
			}

			return oResult;
		}


		// �������ýڵ㴴��������
		public void CreateBy(XmlNode node)
		{
			strName = DomUtil.GetAttrDiff (node,"name");

			string strTopBlank = DomUtil.GetAttrDiff (node,"topblank");
			if (strTopBlank != null)
				nTopBlank = ConvertUtil.S2Int32(strTopBlank);

			string strBottomBlank = DomUtil.GetAttrDiff (node,"bottomblank");
			if (strBottomBlank != null)
				nBottomBlank = ConvertUtil.S2Int32(strBottomBlank);

			string strLeftBlank = DomUtil.GetAttrDiff (node,"leftblank");
			if (strLeftBlank != null)
				nLeftBlank = ConvertUtil.S2Int32(strLeftBlank);

			string strRightBlank = DomUtil.GetAttrDiff (node,"rightblank");
			if (strRightBlank != null)
				nRightBlank = ConvertUtil.S2Int32(strRightBlank);

			string strBackColor = DomUtil.GetAttrDiff (node,"backcolor");
			if (strBackColor != null)  
			{
				this.cBackColor = ColorUtil.String2Color(strBackColor);
			}

			string strTextColor = DomUtil.GetAttrDiff (node,"textcolor");
			if (strTextColor != null)  
			{
				this.cTextColor  = ColorUtil.String2Color(strTextColor);
			}

			string strFontName = DomUtil.GetAttrDiff (node,"font");
			if (strFontName != null)
			{
				refFont = container.cfg.fontList.GetFont(strFontName);
			}

			string strBorderName = DomUtil.GetAttrDiff (node,"border");
			if (strBorderName != null)
			{
				refBorder = container.cfg.borderList.GetBorder(strBorderName);
			}

			//��ס�̳е�����
			strBelongName = DomUtil.GetAttrDiff (node,"belong");

			XmlNode nodeFont = node.SelectSingleNode("font");
			if (nodeFont != null)
			{
				innerFont = container.cfg.fontList.GetFont(nodeFont);
			}

			XmlNode nodeBorder = node.SelectSingleNode("border");
			if (nodeBorder != null)
			{
				innerBorder = container.cfg.borderList.GetBorder(nodeBorder);
			}
		}


		// ��һ��style�ӵ�belongList��ȼ����������û�У�
		void Add2BelongList(VisualStyle style)
		{
			if (belongList == null)
				belongList = new ArrayList ();

			for(int i=0;i<belongList.Count ;i++)
			{
				VisualStyle tempStyle = (VisualStyle)belongList[i];
				if (tempStyle.Equals (style) == true)
				{
					throw(new Exception ("belong�������Ѵ������style��,ѭ���̳���"));
				}
			}
			belongList.Add (style);
		}

		// ��ʼ��belongList
		public void InitailBelongList()
		{
			string strName = strBelongName;
			while(true)
			{
				if (strName == null)
					break;

				VisualStyle style = container.GetVisualStyle (strName);
				if (style == null)
					break;

				if (this.Equals (style) == true)
				{
					throw(new Exception ("ѭ���̳���"));
				}
				Add2BelongList(style);
				strName = style.strBelongName;
			}
		}

		public string Dump()
		{
			string strInfo = "\r\n";
			strInfo += "strName:"+strName + "\r\n";

			strInfo += "\r\n";
			strInfo += "TopBlank:"+ TopBlank + "\r\n";
			strInfo += "BottomBlank:"+ BottomBlank+ "\r\n";
			strInfo += "LeftBlank:"+LeftBlank+ "\r\n";
			strInfo += "RightBlank:"+RightBlank+ "\r\n";

			strInfo += "\r\n";
			strInfo += "BackColor:"+ ColorUtil.Color2String(BackColor) + "\r\n";
			strInfo += "TextColor:"+ ColorUtil.Color2String(TextColor) + "\r\n";

			strInfo += "\r\n";
			strInfo += "FontFace:"+FontFace + "\r\n";
			strInfo += "FontSize:"+FontSize + "\r\n";

			strInfo += "\r\n";
			strInfo += "TopBorderHeight:"+ this.TopBorderHeight + "\r\n";
			strInfo += "BottomBorderHeight:"+ this.BottomBorderHeight + "\r\n";
			strInfo += "LeftBorderWidth:"+ this.LeftBorderWidth + "\r\n";
			strInfo += "RightBorderWidth:"+ this.RightBorderWidth + "\r\n";
			strInfo += "BorderColor:" + ColorUtil.Color2String(BorderColor)+ "\r\n";

			return strInfo;
		}
	}


	#endregion

	#region Border��

	// BorderList
	public class BorderList : CollectionBase
	{
		public ArrayList parallelNodes = new ArrayList ();

		public XmlBorder this[int index]
		{
			get 
			{
				return (XmlBorder)InnerList[index];
			}
			set
			{
				InnerList[index] = value;
			}
		}

		public void Add(XmlBorder item)
		{
			InnerList.Add(item);
		}

		public void Initial(XmlNode root)
		{
			XmlNodeList nodeList = root.SelectNodes("//border");
			for(int i=0;i<nodeList.Count ;i++)
			{
				XmlBorder border = new XmlBorder();
				border.CreateBy (nodeList[i]);

				this.Add (border);

				//�ӵ�ƽ������
				if (parallelNodes == null)
					parallelNodes = new ArrayList ();
				parallelNodes.Add (nodeList[i]);
			}
		}

		public XmlBorder GetBorder (string strBorderName)
		{
			foreach(XmlBorder border in this)
			{
				if (border.strName == strBorderName)
					return border;
			}
			return null;
		}

		public XmlBorder GetBorder (XmlNode nodeBorder)
		{
			for(int i=0;i<parallelNodes.Count ;i++)
			{
				XmlNode node = (XmlNode)parallelNodes[i];
				if (node.Equals (nodeBorder) == true)
					return this[i];
			}
			return null;
		}

		public string Dump()
		{
			string strInfo = "";
			foreach(XmlBorder border in this)
			{
				strInfo += border.Dump ();
			}
			return strInfo;
		}

	}

	public class XmlBorder
	{
		public string strName = null;

		//public object nHorzHeight = null;  //int
		//public object nVertWidth = null;   //int

		public object nLeftWidth = null; //int
		public object nRightWidth = null;  //int
		public object nTopHeight = null; //int
		public object nBottomHeight = null; //int

		public object color = null ;   //Color

		public XmlBorder ()
		{
		}

		//����Node����border
		public void CreateBy(XmlNode node)
		{
			strName = DomUtil.GetAttrDiff (node,"name");

/*
			string strHorzHeight = DomUtil.GetAttrDiff(node,"horzheight");
			if (strHorzHeight != null)
				nHorzHeight = ConvertUtil.S2Int32(strHorzHeight);

			string strVertWidth = DomUtil.GetAttrDiff (node,"vertwidth");
			if (strVertWidth != null)
				nVertWidth = ConvertUtil.S2Int32(strVertWidth);
*/

			string strLeftWidth = DomUtil.GetAttrDiff(node,"leftwidth");
			if (strLeftWidth != null)
				this.nLeftWidth = ConvertUtil.S2Int32(strLeftWidth);

			string strRightWidth = DomUtil.GetAttrDiff(node,"rightwidth");
			if (strRightWidth != null)
				this.nRightWidth = ConvertUtil.S2Int32(strRightWidth);

			string strTopHeight = DomUtil.GetAttrDiff(node,"topheight");
			if (strTopHeight != null)
				this.nTopHeight = ConvertUtil.S2Int32(strTopHeight);

			string strBottomHeight = DomUtil.GetAttrDiff(node,"bottomheight");
			if (strBottomHeight != null)
				this.nBottomHeight = ConvertUtil.S2Int32(strBottomHeight);

			string strColor = DomUtil.GetAttrDiff (node,"color");
			if (strColor != null)
			{
				color = ColorUtil.String2Color(strColor);
			}
		}

		public string Dump()
		{
			string strInfo = "\r\n";
			strInfo += "strName:"+strName + "\r\n";
			strInfo += "nTopHeight:"+ this.nTopHeight + "\r\n";
			strInfo += "nBottomHeight:"+ this.nTopHeight + "\r\n";
			strInfo += "nLeftWidth:"+ this.nLeftWidth + "\r\n";
			strInfo += "nRightWidth:"+ this.nRightWidth + "\r\n";
			strInfo += "color:" + ColorUtil.Color2String((Color)color) + "\r\n";

			return strInfo;
		}

	}

	#endregion

	#region Font��
	
    // FontList
	public class FontList : CollectionBase
	{
		public ArrayList parallelNodes = new ArrayList ();

		public DpFont this[int index]
		{
			get 
			{
				return (DpFont)InnerList[index];
			}
			set
			{
				InnerList[index] = value;
			}
		}

		public void Add(DpFont item)
		{
			InnerList.Add(item);
		}

		public void Initial(XmlNode root)
		{
			XmlNodeList nodeList = root.SelectNodes("//font");
			for(int i=0;i<nodeList.Count ;i++)
			{
				DpFont font = new DpFont();
				font.CreateBy (nodeList[i]);

				this.Add (font);

				if (parallelNodes == null)
					parallelNodes = new ArrayList ();
				parallelNodes.Add (nodeList[i]);
			}
		}
		public DpFont GetFont (string strFontName)
		{
			foreach(DpFont font in this)
			{
				if (font.strName == strFontName)
					return font;
			}
			return null;
		}

		public DpFont GetFont (XmlNode nodeFont)
		{
			for(int i=0;i<parallelNodes .Count ;i++)
			{
				XmlNode node = (XmlNode)parallelNodes[i];
				if (node.Equals (nodeFont) == true)
					return this[i];
			}
			return null;
		}

		public string Dump()
		{
			string strInfo = "";
			foreach(DpFont font in this)
			{
				strInfo += font.Dump ();
			}
			return strInfo;
		}
	}

	public class DpFont   //�����System.Drawing.Font���ò���Ҫ���鷳
	{
		public string strName = null;

		public string strFace = null;
		public object nSize = null;
		public object style = null;

		public DpFont()
		{}

		public void CreateBy(XmlNode node)
		{
			strName = DomUtil.GetAttrDiff (node,"name");
			strFace = DomUtil.GetAttrDiff (node,"face");
			string strStyle = DomUtil.GetAttrDiff (node,"style");
			if (strStyle != null)
			{
				if (strStyle == "")
					style = (FontStyle)0;
				else
					style = (FontStyle)Enum.Parse(typeof(FontStyle), strStyle,true);
			}
			string strSize = DomUtil.GetAttrDiff (node,"size");
			if (strSize != null)
				nSize = ConvertUtil.S2Int32(strSize);
		}

		public string Dump()
		{
			string strInfo = "\r\n";
			strInfo += "strName:"+strName + "\r\n";
			strInfo += "face:"+ strFace + "\r\n";
			strInfo += "size:"+ nSize + "\r\n";
			return strInfo;
		}
	}

	#endregion

}
