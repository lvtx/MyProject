using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using DigitalPlatform.IO;

namespace DigitalPlatform.Xml
{
	public class Attributes: BoxExpandable
	{
		#region ��д������麯��

		public override ItemRegion GetRegionName()
		{
			if (this.bExpand == true)
				return ItemRegion.Attributes;
			else
				return ItemRegion.Text ;
		}


		public override void InitialVisual()
		{
			ElementItem item = (ElementItem)this.GetItem();
			if (item == null)
				return;

            // ����̬
			if (item.m_attrsExpand == ExpandStyle.Collapse)
			{
				this.childrenVisual = null;

				this.MyText.container = this.container ;
				this.MyText.Name = "attributes";
				this.MyText.Text = "��δ��ʼ��"; //item.strAttrsValue;


				// ��this.MyText�ӽ�������ĸ�����,��this�Ӹ�����ɾ��
				((Box)this.container).AddChildVisual(this.MyText);
				((Box)this.container).childrenVisual.Remove (this);

				item.Flush();
			}
			else if (item.m_attrsExpand == ExpandStyle.Expand)
			{
				foreach(AttrItem attr in item.attrs)
				{
					Debug.Assert(attr.GetValue() != null,"׼��ֵ����Ϊnull");
					attr.m_paraValue1 = attr.GetValue();
				}

				if (this.childrenVisual != null)
				{
					this.childrenVisual.Clear();
				}

                // ��������
				this.LayoutStyle = LayoutStyle.Vertical;

				foreach(AttrItem attr in item.attrs)
				{
					// ����Ԫ�ص�style��ʽ�븸����ͬ
					attr.LayoutStyle = item.LayoutStyle ;
						
					// ��child�ӵ�content����ΪChildVisual
					attr.container = this;
					this.AddChildVisual(attr);

					// �˴�
					Debug.Assert(attr.m_paraValue1 != null,"��ʱ��ӦΪnull");
					// ʵ��Ƕ��
					attr.InitialVisual();//elementStyle);
				}
			}
			else 
			{
				Debug.Assert(false, "");
			}
		}


		#endregion
	}

}
