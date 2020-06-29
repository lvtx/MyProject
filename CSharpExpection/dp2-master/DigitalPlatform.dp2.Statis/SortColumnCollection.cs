using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using DigitalPlatform.Text;

namespace DigitalPlatform.dp2.Statis
{
    // �����йص���: SortColumn SortColumnCollection

    // ����������
    public enum DataType
    {
        Auto = 0,
        String = 1,
        Number = 2,
        Price = 3,// 100���������
        PriceDouble = 4,    // double��������ʾ��Ҳ�������ֻ����λС������ -- ע�⣬���ۼ�������⣬�Ժ����ֹ
        PriceDecimal = 5,   // decimal��������ʾ��
        Currency = 6,   // �����ַ��������л��ҵ�λ���ַ��������������ɸ��Ӵ�����������
        RecPath = 7,    // ��¼·�����̵���ʽ�����硰����ͼ��/1��
    }

    // һ���е���������
    public class SortColumn
    {
        internal bool bAsc = true;	// �Ƿ�Ϊ����
        internal DataType dataType = DataType.Auto;
        internal bool bIgnorCase = true;
        internal int nColumnNumber = -1;
        internal bool bPatronBarcode = false;   // �Ƿ����֤����š�����ǣ������ʱ��Ҫ�Ȱ���λ����

        public string ToStyleString()
        {
            StringBuilder text = new StringBuilder();
            if (this.bAsc == true)
                text.Append("a");
            else
                text.Append("d");

            if (this.dataType == DataType.String)
                text.Append("s");
            if (this.dataType == DataType.Number)
                text.Append("n");
            if (this.dataType == DataType.RecPath)
                text.Append("P");

            if (this.bPatronBarcode == true)
                text.Append("p");

            if (this.bIgnorCase == false)
                text.Append("c");

            return text.ToString();
        }

        public static void GetStyle(string strStyle,
            out bool bAsc,
            out bool bIgnoreCase,
            out bool bPatronBarcode,
            out DataType dataType)
        {
            bAsc = true;
            dataType = DataType.Auto;
            bIgnoreCase = true;
            bPatronBarcode = false;

            for (int i = 0; i < strStyle.Length; i++)
            {
                if (strStyle[i] == 'a')
                {
                    bAsc = true;
                }
                else if (strStyle[i] == 'd')
                {
                    bAsc = false;
                }
                else if (strStyle[i] == 's')
                {
                    dataType = DataType.String;
                }
                else if (strStyle[i] == 'n')
                {
                    dataType = DataType.Number;
                }
                else if (strStyle[i] == 'P')
                {
                    dataType = DataType.RecPath;
                }
                else if (strStyle[i] == 'p')
                {
                    bPatronBarcode = true;
                }
                else if (strStyle[i] == 'c') // ��Сд����
                {
                    bIgnoreCase = false;	// ����ע��ȱʡ���Ϊ��Сд������
                }

            }
        }

        public int CompareObject(object o1, object o2)
        {
            //int nRet = 0;
            Int64 n1 = 0;
            Int64 n2 = 0;
            string s1 = null;
            string s2 = null;
            bool bException = false;

            if ((o1 is Int32))  // 2013/12/19
                n1 = (Int32)o1;
            else if ((o1 is Int32)
                || (o1 is Int64))
                n1 = (Int64)o1;
            else if (o1 is string)
            {
                // 2014/1/12
                bool bRet = Int64.TryParse((string)o1, out n1);
                if (bRet == false)
                {
                    s1 = (string)o1;
                    bException = true;
                }
#if NO
                try
                {
                    n1 = Convert.ToInt64((string)o1);	// �����׳��쳣
                }
                catch
                {
                    s1 = (string)o1;
                    bException = true;
                }
#endif
            }


            if ((o2 is Int32))  // 2013/12/19
                n2 = (Int32)o2;
            else if ((o2 is Int32)
                || (o2 is Int64))
            {
                n2 = (Int64)o2;
                if (bException == true)
                    s2 = Convert.ToString(n2);
            }
            else if (o2 is string)
            {
                if (bException == true)
                    s2 = (string)o2;
                else
                {
                    // 2014/1/12
                    bool bRet = Int64.TryParse((string)o2, out n2);
                    if (bRet == false)
                    {
                        s2 = (string)o2;
                        bException = true;
                        s1 = Convert.ToString(n1);
                    }

#if NO
                    try
                    {
                        n2 = Convert.ToInt64((string)o2);
                    }
                    catch
                    {
                        s2 = (string)o2;
                        bException = true;
                        s1 = Convert.ToString(n1);
                    }
#endif
                }
            }

            if (bException == true)
            {
                return CompareString(s1, s2);
            }
            else
            {
                Int64 n64Ret = n1 - n2;
                if (this.bAsc == false)
                    n64Ret = n64Ret * (-1);
                if (n64Ret != 0)
                    return (int)n64Ret;
            }

            return 0;
        }

        public int CompareString(string s1, string s2)
        {
            int nRet = 0;

            // ֤������ȱȽ�λ��
            if (this.bPatronBarcode == true
                && s1 != null & s2 != null
                && s1.Length != s2.Length)
            {
                nRet = s1.Length - s2.Length;
                goto END1;
            }

            if (this.dataType == DataType.Auto
                || this.dataType == DataType.Number)
            {
                if (s1.Length == s1.Length)
                {
                    nRet = String.Compare(s1, s2, this.bIgnorCase);
                }
                else
                {
                    // �Ҷ���?
                    if (s1.Length < s2.Length)
                    {
                        s1 = s1.PadLeft(s2.Length, ' ');
                    }
                    else if (s1.Length > s2.Length)
                    {
                        s2 = s2.PadLeft(s1.Length, ' ');
                    }
                    nRet = String.Compare(s1, s2, this.bIgnorCase);
                }
            }
            else if (this.dataType == DataType.String)
            {
                nRet = String.Compare(s1, s2, this.bIgnorCase);
            }
            else if (this.dataType == DataType.RecPath)
            {
                nRet = StringUtil.CompareRecPath(s1, s2);
            }
            else
            {
                nRet = String.Compare(s1, s2, this.bIgnorCase);
            }

            END1:
            if (this.bAsc == false)
                nRet = nRet * (-1);
            if (nRet != 0)
                return nRet;

            return 0;
        }
    }

    // ������������
    public class SortColumnCollection : List<SortColumn>
    {
        // ��ͨ�õ��б�ʾ����ת��Ϊ table ר�õ��б�ʾ��
        // ǰ���Ǵ� 0 ��ʼ�����кţ������Ǵ� -1 ��ʼ�����к�
        public static string NormalToTable(string strSource)
        {
            SortColumnCollection temp = new SortColumnCollection();
            temp.Build(strSource);

            StringBuilder text = new StringBuilder(4096);
            foreach (SortColumn column in temp)
            {
                if (text.Length > 0)
                    text.Append(",");
                text.Append((column.nColumnNumber -1).ToString() + ":");
                text.Append(column.ToStyleString());
            }

            return text.ToString();
        }

        // ��table ר�õ��б�ʾ���� ת��Ϊͨ�õ��б�ʾ��
        // ǰ���Ǵ� -1 ��ʼ�����кţ������Ǵ� 0 ��ʼ�����к�
        public static string TableToNormal(string strSource)
        {
            SortColumnCollection temp = new SortColumnCollection();
            temp.Build(strSource);

            StringBuilder text = new StringBuilder(4096);
            foreach (SortColumn column in temp)
            {
                if (text.Length > 0)
                    text.Append(",");
                text.Append((column.nColumnNumber + 1).ToString() + ":");
                text.Append(column.ToStyleString());
            }

            return text.ToString();
        }

        // 0:a,1:p
        // -1 ��ʾ Entry ��
        public void Build(string strColumnList)
        {
            string[] aName = strColumnList.Split(new Char[] { ',' });

            this.Clear();

            for (int i = 0; i < aName.Length; i++)
            {
                string strPart = aName[i];
                if (strPart == "")
                    continue;
                SortColumn column = new SortColumn();
                this.Add(column);

                int nRet = strPart.IndexOf(":");
                if (nRet == -1)
                {
                    column.nColumnNumber = Convert.ToInt32(strPart);
                }
                else
                {
                    column.nColumnNumber = Convert.ToInt32(strPart.Substring(0, nRet).Trim());

                    strPart = strPart.Substring(nRet + 1).Trim();

                    bool bAsc;
                    DataType dataType;
                    bool bIgnoreCase;
                    bool bPatronBarcode = false;
                    SortColumn.GetStyle(strPart,
                        out bAsc,
                        out bIgnoreCase,
                        out bPatronBarcode,
                        out dataType);
                    column.bAsc = bAsc;
                    column.bIgnorCase = bIgnoreCase;
                    column.dataType = dataType;
                    column.bPatronBarcode = bPatronBarcode;
                }
            }

        }

    }

}
