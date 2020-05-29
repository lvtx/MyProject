using System;
using System.Xml;
using System.Diagnostics;

using DigitalPlatform.Text;


namespace DigitalPlatform.Xml
{
	// �ı��ڵ�
	public class TextItem : Item
	{
		public TextItem()
		{
		}

		internal TextItem(XmlEditor document)
		{
			this.Name = "#text";
			this.m_document = document;
		}


		// parameters:
		//		style	�ݲ�ʹ��.
		public override int Initial(XmlNode node, 
			ItemAllocator allocator,
            object style,
            bool bConnect)
		{
			this.Name = node.Name;
			if (node.Value != null)
			{
				this.SetValue(node.Value);
			}
			else
			{
				Debug.Assert(false,"node�ڵ��Value��ȻΪnull");
			}

			return 0;
		}

		// parameters:
		//		style	��ʼ�������δʹ�á�
		public override void InitialVisualSpecial(Box boxTotal)
		{
			XmlText text = new XmlText ();
			text.Name = "TextOfTextItem";
			text.container = boxTotal;
			Debug.Assert(this.m_paraValue1 != null,"��ʼֵ����Ϊnull");
			text.Text = this.GetValue(); //this.m_paraValue;;
			boxTotal.AddChildVisual(text);

			this.m_paraValue1 = null;

/*
			if (this.parent .ReadOnly == true
				|| this.ReadOnly == true)
			{
				text.Editable = false;
			}
*/			
		}
		
        internal override string GetOuterXml(ElementItem FragmentRoot)
		{
			return StringUtil.GetXmlStringSimple(this.GetValue());
		}
	}

}
