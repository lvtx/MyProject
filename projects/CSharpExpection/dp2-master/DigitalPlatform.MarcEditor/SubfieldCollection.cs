using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DigitalPlatform.Marc
{
    // 
    /// <summary>
    /// ���ֶμ���
    /// </summary>
    public class SubfieldCollection : ArrayList
    {
        /// <summary>
        /// ������ Field ����
        /// </summary>
        public Field Container = null;

        // ժҪ:
        //     ��ȡ������ָ����������Ԫ�ء�
        //
        // ����:
        //   index:
        //     Ҫ��û����õ�Ԫ�ش��㿪ʼ��������
        //
        // ���ؽ��:
        //     ָ����������Ԫ�ء�
        //
        // �쳣:
        //   System.ArgumentOutOfRangeException:
        //     index С���㡣- �� -index ���ڻ���� System.Collections.ArrayList.Count��
        /// <summary>
        /// ��ȡ������ָ���������� Subfield Ԫ�ء�
        /// </summary>
        /// <param name="index">Ҫ��û����õ�Ԫ�ش��㿪ʼ��������</param>
        /// <returns>ָ���������� Subfield Ԫ�ء�</returns>
        public new Subfield this[int index]
        {
            get
            {
                return (Subfield)base[index];
            }
            set
            {
                base[index] = value;
                Flush();
            }

        }

        /// <summary>
        /// ��ȡ������ָ�� Name �� Subfield Ԫ�ء�
        /// ��ȡ��ʱ�������ͬ�������ֶΣ��򷵻ص�һ����
        /// ���õ�ʱ������в�����������ֵ����ֶζ�����׷��һ�����ֶζ���������ڶ��ͬ�������ֶζ������滻��һ��
        /// </summary>
        /// <param name="strName">���ֶ�����һ���ַ����ַ���</param>
        /// <returns>ָ�� Name �� Subfield Ԫ�ء�</returns>
        public Subfield this[string strName]
        {
            get
            {
                return GetSubfield(strName, 0);
            }
            set
            {
                Subfield subfield = GetSubfield(strName, 0);
                if (subfield == null)
                {
                    // throw(new Exception("δ�ҵ�"));
                    this.Add(value);
                    return;
                }
                int i = this.IndexOf(subfield);
                base[i] = value;
                Flush();
            }
        }

        /// <summary>
        /// ��ȡ������ָ�� Name �� Subfield Ԫ�ء�����ָ����ͬ�����ֶ�������һ����
        /// ���õ�ʱ������в�����������ֵ�ָ���ظ�λ�õ����ֶζ�����׷��һ�����ֶζ���
        /// </summary>
        /// <param name="strName">���ֶ�����һ���ַ����ַ���</param>
        /// <param name="nDupIndex">��ͬ�������ֶ��е��������� 0 ��ʼ����</param>
        /// <returns>ָ�� Name �� Subfield Ԫ�ء�</returns>
        public Subfield this[string strName, int nDupIndex]
        {
            get
            {
                return GetSubfield(strName, nDupIndex);
            }
            set
            {
                Subfield subfield = GetSubfield(strName, nDupIndex);
                if (subfield == null)
                {
                    //throw(new Exception("δ�ҵ�"));
                    this.Add(value);
                    return;
                }
                int i = this.IndexOf(subfield);
                base[i] = value;
                Flush();
            }

        }

        // parameters:
        //      strName ���ֶ���
        //      nDupIndex   �ڼ���
        /// <summary>
        /// �������ֶ������ظ�λ�û�ȡһ�� Subfield ����
        /// </summary>
        /// <param name="strName">���ֶ�����һ���ַ����ַ���</param>
        /// <param name="nDupIndex">��ͬ�������ֶ��е��������� 0 ��ʼ����</param>
        /// <returns>Subfield Ԫ�ء�</returns>
        public Subfield GetSubfield(string strName,
            int nDupIndex)
        {
            int nDup = 0;
            for (int i = 0; i < this.Count; i++)
            {
                Subfield subfield = this[i];
                if (subfield.Name == strName)
                {
                    if (nDupIndex == nDup)
                        return subfield;
                    nDup++;
                }

            }

            return null;
        }

        /// <summary>
        /// ����һ�� Field ��������ݴ���һ���µ� SubfieldCollection ����
        /// </summary>
        /// <param name="container">Field ����Ҳ������Ҫ������ SubfieldCollection ���������</param>
        /// <returns>�´����� SubfieldCollection ����</returns>
        public static SubfieldCollection BuildSubfields(Field container)
        {
            SubfieldCollection subfields = new SubfieldCollection();

            string strField = container.Text;

            for (int i = 0; ; i++)
            {
                string strSubfield = "";
                string strNextSubfieldName = "";

                // ���ֶλ����ֶ����еõ�һ�����ֶ�
                // parameters:
                //		strText		�ֶ����ݣ��������ֶ������ݡ�
                //		textType	��ʾstrText�а��������ֶ����ݻ��������ݡ���ΪItemType.Field����ʾstrText������Ϊ�ֶΣ���ΪItemType.Group����ʾstrText������Ϊ���ֶ��顣
                //		strSubfieldName	���ֶ���������Ϊ1λ�ַ������==null����ʾ�������ֶ�
                //					��ʽΪ'a'�����ġ�
                //		nIndex			��Ҫ���ͬ�����ֶ��еĵڼ�������0��ʼ���㡣
                //		strSubfield		[out]������ֶΡ����ֶ���(1�ַ�)�����ֶ����ݡ�
                //		strNextSubfieldName	[out]��һ�����ֶε����֣�����һ���ַ�
                // return:
                //		-1	����
                //		0	��ָ�������ֶ�û���ҵ�
                //		1	�ҵ����ҵ������ֶη�����strSubfield������
                int nRet = MarcUtil.GetSubfield(
                    strField,
                    ItemType.Field,
                    null,
                    i,
                    out strSubfield,
                    out strNextSubfieldName);
                if (nRet == -1 || nRet == 0)
                    break;

                Subfield subfield = new Subfield();
                subfield.Text = strSubfield;
                subfield.Container = subfields;

                subfields.Add(subfield);
            }

            subfields.Container = container;
            return subfields;
        }

        /// <summary>
        /// �� Subfield ���������ӵ� ��ǰ���� �Ľ�β����
        /// </summary>
        /// <param name="subfield">Ҫ��ӵ� Subfield ����</param>
        public void Add(Subfield subfield)
        {
            base.Add(subfield);

            Flush();
        }

        // �������ֶ����ҵ�ƫ����
        /// <summary>
        /// �������ֶ����ҵ����ڼ����е�ƫ����
        /// </summary>
        /// <param name="strSubfieldName">���ֶ���</param>
        /// <returns>�� 0 ��ʼ�����ƫ��������������ڣ��򷵻� this.Count ֵ</returns>
        public int GetPosition(string strSubfieldName)
        {
            int nPosition = 0;
            // Debug.Assert(false, "");
            for (int i = 0; i < this.Count; i++)
            {
                nPosition = i;

                Subfield subfield = this[i];
                if (String.Compare(subfield.Name, strSubfieldName) > 0)
                    return nPosition;
            }

            return this.Count;  // 2007/7/10 changed
            // return nPosition;
        }

        // ��Ҫ��һ�������ַ�ƫ������λ�����ֶεĺ��� 


        /// <summary>
        /// �� Subfield ���������ӵ� ��ǰ���� �Ľ�β�������߰������ֶ�����ĸ˳����롣
        /// </summary>
        /// <param name="subfield">Ҫ����� Subfield ����</param>
        /// <param name="bInOrder">�Ƿ������ֶ�����ĸ˳����롣���Ϊ false�����ʾ׷�ӵ�����ĩβ</param>
        public void Add(Subfield subfield,
            bool bInOrder)
        {
            if (bInOrder == true)
            {
                int nPosition = GetPosition(subfield.Name);
                base.Insert(nPosition, subfield);
            }
            else
            {
                base.Add(subfield);
            }

            this.Flush();
        }

        // ע������collection������Ϊ�˷���set��Field.Subfields
        /// <summary>
        /// �ӵ�ǰ�������Ƴ�һ�� Subfield ����
        /// </summary>
        /// <param name="subfield">Ҫ�Ƴ��� Subfield ����</param>
        /// <returns>˳�㷵�ص�ǰ���϶���</returns>
        public SubfieldCollection Remove(Subfield subfield)
        {
            base.Remove(subfield);

            Flush();

            return this;
        }

        /// <summary>
        /// �Ƴ� ��ǰ���� ��ָ���������� Subfield Ԫ�ء�
        /// </summary>
        /// <param name="index">Ҫ�Ƴ��Ķ��������</param>
        public override void RemoveAt(int index)
        {
            base.RemoveAt(index);

            Flush();
        }

        /// <summary>
        /// �� Subfield Ԫ�ز��� ��ǰ���ϵ� ��ָ����������
        /// </summary>
        /// <param name="index">����</param>
        /// <param name="subfield">Subfield ����</param>
        public void Insert(int index, Subfield subfield)
        {
            base.Insert(index, subfield);

            Flush();
        }

        /// <summary>
        /// ������Ԫ�ص�������ֵ���������� Value ��Ա��
        /// </summary>
        public void Flush()
        {
            if (this.Container == null)
                return;

            string strValue = "";
            for (int i = 0; i < this.Count; i++)
            {
                Subfield subfield = (Subfield)this[i];
                strValue += new string(Record.SUBFLD, 1) + subfield.Name + subfield.Value;
            }

            this.Container.Value = strValue;
        }
    }
}
