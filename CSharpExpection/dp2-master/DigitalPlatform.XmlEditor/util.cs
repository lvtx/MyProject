using System;

namespace DigitalPlatform.Xml
{
	// ����ͼƬ��ʽ
	public enum BackPicStyle
	{
		Fill = 0,
		Center = 1,
		Tile,
	}

	// �ƶ�����
	public enum MoveMember
	{
		Front = 0,
		Behind = 1,
		Auto = 2,
	}

	// style����ö��
	public enum ValueStyle
	{
		TopBlank = 0,	
		BottomBlank = 1,	
			
		LeftBlank = 2,
		RightBlank = 3,
			
		BackColor = 4,
		TextColor = 5,

		FontFace = 6,
		FontSize = 7,
		FontStyle = 8,

		//BorderVertWidth = 9,
		//BorderHorzHeight = 10,
		TopBorderHeight = 9,
		BottomBorderHeight = 10,
		LeftBorderWidth = 11,
		RightBorderWidth = 12,
		BorderColor = 13,
	}


	// ����ö��
	public enum ItemRegion
	{
		Frame = 0x1,

		Label = 0x2,
		Text = 0x4,
		Comment = 0x8,

		Content = 0x10,
		Attributes = 0x20,

		ExpandAttributes = 0x40,
		ExpandContent = 0x80,
		
		BoxTotal = 0x100,
		BoxAttributes = 0x200,
		BoxContent = 0x400,

		No = 0x0,
	}


	// ���Ƴ�Աö��
	public enum PaintMember
	{
		Border = 0,
		Content = 1,
		Both = 2,
	};

	// �����ö��
	public enum ScrollBarMember 
	{
		Vert = 0,
		Horz = 1,
		Both = 2,
	};


	// ���ַ��ö��
	public enum LayoutStyle
	{
		Vertical = 0,
		Horizontal = 1,
	};

	// չ����ť���ö��
	public enum ExpandIconStyle
	{
		Plus  = 0,
		Minus  = 1,
	};

	// Layoutֵö��
	public enum LayoutMember  
	{
		CalcuWidth = 0x1,	// ������ȡ�����Ӧ���ñȽϽ�ʡ��Դ�ķ�ʽ����������ֵ����
		//CalcuBoth = 0x4,

		CalcuHeight = 0x2,
		Layout = 0x4,		// ��ʽ���֡����ո����Ŀ�ȣ���ʼ�������ڲ��ߴ����

		Up = 0x8,

		EnLargeHeight = 0x10,
		EnlargeWidth = 0x20,

		None = 0x0,
	
	}

    public class ElementInitialStyle
    {
        public ExpandStyle attrsExpandStyle = ExpandStyle.None;
        public ExpandStyle childrenExpandStyle = ExpandStyle.None;

        public bool bReinitial = false;	// �Ƿ���: ���ڴ�����Ѿ����ڵĻ��������³�ʼ��ĳЩ״̬�����==false����ʾΪ�״γ�ʼ��
    }

    public enum ExpandStyle
    {
        None = 0,
        Expand = 1,
        Collapse = 2,
    }
}
