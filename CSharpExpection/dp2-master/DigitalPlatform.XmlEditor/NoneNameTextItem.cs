using System;

namespace DigitalPlatform.Xml
{	
	// �����ڴ���ָ���������ı��ڵ�
	// ���ص�Ϊ:
	//	1) label��ʾ�����������'?'
	//	2) this.Value����ת��(��TextItem��Ҫ��ת���)
	public class NoneNameTextItem : TextItem
	{

		public virtual void InitialLabelText(XmlLabel label)
		{
			// һ������
			if (this.Name.Length > 0 && this.Name[0] == '#')
				label.Text = this.Name;	
			else
				label.Text = "? " + this.Name;
		}


		// ������������������˵һ�㲻Ҫ���ء�һ������InitialVisualSpecial()���ɡ�
		// ��Ϊ����������ǰ�󲿷ֻ����ǹ��õģ�ֻ���жβ��õ���InitialVisualSpecial()�İ취��
		// ������������˱����������Ҳ��������е���InitialVisualSpecial()������Ҫ����ʵ��ȫ������
		public override void  InitialVisual()
		{
			if (this.childrenVisual != null)
				this.childrenVisual.Clear();

			// ��Label
			XmlLabel label = new XmlLabel();
			label.container = this;

			InitialLabelText(label);

			this.AddChildVisual(label);


			// ����һ���ܿ�
			Box boxTotal = new Box ();
			boxTotal.Name = "BoxTotal";
			boxTotal.container = this;
			this.AddChildVisual (boxTotal);

			// �����ܿ��layoutStyle��ʽΪ����
			boxTotal.LayoutStyle = LayoutStyle.Vertical;

			///
			InitialVisualSpecial(boxTotal);
			///

			//���boxTotalֻ��һ��box������Ϊ����
			if (boxTotal.childrenVisual != null && boxTotal.childrenVisual .Count == 1)
				boxTotal.LayoutStyle = LayoutStyle.Horizontal ;

/*
			Comment comment = new Comment ();
			comment.container = this;
			this.AddChildVisual(comment);
*/			
		}
	}
}
