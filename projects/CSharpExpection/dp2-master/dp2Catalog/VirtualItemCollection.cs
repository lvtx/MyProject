using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dp2Catalog
{
    public class VirtualItemCollection : List<VirtualItem>
    {
        List<int> m_selectedIndices = null;

        public void ExpireSelectedIndices()
        {
            this.m_selectedIndices = null;
        }

        //
        // ժҪ:
        //     �� System.Collections.Generic.List<T> ���Ƴ�����Ԫ�ء�
        public new void Clear()
        {
            base.Clear();
            this.ExpireSelectedIndices();
        }

        // ժҪ:
        //     ��������ӵ� System.Collections.Generic.List<T> �Ľ�β����
        //
        // ����:
        //   item:
        //     Ҫ��ӵ� System.Collections.Generic.List<T> ��ĩβ���Ķ��󡣶����������ͣ���ֵ����Ϊ null��
        public new void Add(VirtualItem item)
        {
            base.Add(item);
            this.ExpireSelectedIndices();
        }
        //
        // ժҪ:
        //     ��ָ�����ϵ�Ԫ����ӵ� System.Collections.Generic.List<T> ��ĩβ��
        //
        // ����:
        //   collection:
        //     һ�����ϣ���Ԫ��Ӧ����ӵ� System.Collections.Generic.List<T> ��ĩβ������������Ϊ null���������԰���Ϊ
        //     null ��Ԫ�أ�������� T Ϊ�������ͣ���
        //
        // �쳣:
        //   System.ArgumentNullException:
        //     collection Ϊ null��
        public new void AddRange(IEnumerable<VirtualItem> collection)
        {
            base.AddRange(collection);
            this.ExpireSelectedIndices();
        }


        public List<int> SelectedIndices
        {
            get
            {
                if (m_selectedIndices != null)
                    return m_selectedIndices;

                this.m_selectedIndices = new List<int>();

                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Selected == true)
                        m_selectedIndices.Add(i);
                }
                return m_selectedIndices;
            }
            set
            {
                // ��ȫ�����
                for (int i = 0; i < this.Count; i++)
                {
                    this[i].Selected = false;
                }
                // Ȼ������
                for (int i = 0; i < value.Count; i++)
                {
                    int index = value[i];
                    this[index].Selected = true;
                }

            }
        }


    }

    public class VirtualItem
    {
        public bool Selected = false;

        public List<string> SubItems = null;

        public int ImageIndex = -1;

        public object Tag = null;

        public VirtualItem(string strFirstSubItemText,
            int nImageIndex)
        {
            if (this.SubItems == null)
                this.SubItems = new List<string>();

            if (this.SubItems.Count == 0)
                this.SubItems.Add(strFirstSubItemText);
            else
                this.SubItems[0] = strFirstSubItemText;

            this.ImageIndex = nImageIndex;
        }

        public ListViewItem GetListViewItem(int nColumnCount)
        {
            string strFirstSubItemText = "";
            if (this.SubItems != null && this.SubItems.Count > 0)
                strFirstSubItemText = this.SubItems[0];
            ListViewItem item = new ListViewItem(strFirstSubItemText,
                this.ImageIndex);
            for (int i = 1; i < nColumnCount; i++)
            {
                string strText = "";
                if (i<this.SubItems.Count)
                    strText = this.SubItems[i];
                item.SubItems.Add(strText);
            }

            item.Selected = this.Selected;

            return item;
        }
    }
}
