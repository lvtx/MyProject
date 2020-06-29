using System;
using System.Collections.Generic;
using System.Text;

namespace dp2ZServer
{
    /// <summary>
    /// ȫ�ֺ���
    /// </summary>
    public class Global
    {
        // ��·����ȡ����������
        // parammeters:
        //      strPath ·��������"����ͼ��/3"
        public static string GetDbName(string strPath)
        {
            int nRet = strPath.LastIndexOf("/");
            if (nRet == -1)
                return strPath;

            return strPath.Substring(0, nRet).Trim();
        }

        // ��·����ȡ����¼�Ų���
        // parammeters:
        //      strPath ·��������"����ͼ��/3"
        public static string GetRecordID(string strPath)
        {
            int nRet = strPath.LastIndexOf("/");
            if (nRet == -1)
                return "";

            return strPath.Substring(nRet + 1).Trim();
        }
    }
}
