using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalPlatform.DTLP
{
    public class Global
    {
        // ��·���ӹ�Ϊ���Ի���������̬
        // parameters:
        //      strCtlnoPart    ���Ϊ""����ʾ�ӹ�Ϊ���Եġ�ֻ����ǰ����ʾ��̬�����Ϊ"ctlno"����"��¼������"����ʾ�ӹ�Ϊ������̬
        public static string ModifyDtlpRecPath(string strPath,
            string strCtlnoPart)
        {
            int nRet = strPath.LastIndexOf("/");

            if (nRet == -1)
                return strPath;

            string strNumber = strPath.Substring(nRet + 1).Trim();

            nRet = strPath.LastIndexOf("/", nRet - 1);
            if (nRet == -1)
                return strPath;

            string strPrevPart = strPath.Substring(0, nRet).Trim();

            return strPrevPart + "/" + strCtlnoPart + "/" + strNumber;
        }

        // ��һ���ַ�������ȥ�ء�����ǰ��Ӧ���Ѿ�����
        public static void RemoveDup(ref List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string strItem = list[i];
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (strItem == list[j])
                    {
                        list.RemoveAt(j);
                        j--;
                    }
                    else
                    {
                        i = j - 1;
                        break;
                    }
                }
            }

        }
    }
}
