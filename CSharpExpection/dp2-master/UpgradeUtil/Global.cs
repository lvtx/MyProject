using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradeUtil
{
    public class Global
    {
        // ����ʱ�䷶Χ�ַ���
        // parameters:
        //      strText ���ڷ�Χ�ַ�������̬Ϊ ��19980101-19991231��
        public static int ParseTimeRangeString(string strText,
            out string strStart,
            out string strEnd,
            out string strError)
        {
            strError = "";
            strStart = "";
            strEnd = "";

            int nRet = strText.IndexOf("-");
            if (nRet == -1)
            {
                strError = "ȱ�����ۺ� '-'";
                return -1;
            }

            strStart = strText.Substring(0, nRet).Trim();
            strEnd = strText.Substring(nRet + 1).Trim();

            if (strStart.Length != strEnd.Length)
            {
                strError = "��ʼ '"+strStart+"' �ͽ���ʱ���ַ��� '"+strEnd+"' ���Ȳ�ͬ";
                return -1;
            }

            return 0;
        }

    }
}
