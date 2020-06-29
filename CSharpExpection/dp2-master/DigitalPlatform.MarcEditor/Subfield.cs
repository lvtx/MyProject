using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalPlatform.Marc
{
    // ���ֶ�
    /// <summary>
    /// ���ֶζ���
    /// </summary>
    public class Subfield
    {
        internal string m_strName = "";
        internal string m_strValue = "";

        /// <summary>
        /// ������
        /// SubfieldCollection ����
        /// </summary>
        public SubfieldCollection Container = null;

        // ���ֶ������е�ƫ����
        /// <summary>
        /// ����һ��ƫ��������ǰ���ֶε�һ�ַ��������������ֶ������е�ƫ����
        /// </summary>
        public int Offset
        {
            get
            {
                if (this.Container == null)
                    return -1;
                int v = 0;
                for (int i = 0; i < Container.Count; i++)
                {
                    Subfield subfield = Container[i];
                    if (subfield == this)
                    {
                        return v;
                    }

                    v += 1 + subfield.Name.Length + subfield.Value.Length;
                }

                return -1;  // û���ҵ��Լ�
            }
        }

        /// <summary>
        /// ��û��������ֶ���
        /// </summary>
        public string Name
        {
            get
            {
                return m_strName;
            }
            set
            {
                if (m_strName == value)
                    return;

                m_strName = value;
                if (this.Container != null)
                    this.Container.Flush();
            }
        }

        /// <summary>
        /// ��û������õ�ǰ���ֶε� Value ֵ��Ҳ�������ֶ����ݲ��֣����������ֶ���
        /// </summary>
        public string Value
        {
            get
            {
                return m_strValue;
            }
            set
            {

                if (m_strValue == value)
                    return;

                m_strValue = value;
                if (this.Container != null)
                    this.Container.Flush();

            }
        }

        /// <summary>
        /// ��ȡ�����õ�ǰ���ֶε� Text ֵ��Ҳ���� Name + Value ���ַ���
        /// </summary>
        public string Text
        {
            get
            {
                return this.Name + this.Value;
            }

            set
            {
                if (value.Length == 0)
                {
                    if (this.m_strName == "" && this.m_strValue == "")
                        return;

                    this.m_strName = "";
                    this.m_strValue = "";

                    if (this.Container != null)
                        this.Container.Flush();

                    return;
                }
                this.m_strName = value[0].ToString();
                this.m_strValue = value.Substring(1);

                if (this.Container != null)
                    this.Container.Flush();
            }
        }

    }

}
