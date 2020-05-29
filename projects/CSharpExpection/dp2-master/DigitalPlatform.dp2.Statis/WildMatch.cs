using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DigitalPlatform.dp2.Statis
{

    public class ModeCell
    {
        internal int m_nType = 0;	// ��Ԫ����
        internal string m_strValue = "";	// ֵ
        // ����Ԫ����ΪMC_CHARʱ��m_strValue��Ϊһ���ַ�
        // ����Ԫ����ΪMC_STRINGʱ��m_strValue��Ϊ����ַ�
        // ����Ԫ����ΪMC_CHARLISTʱ��m_strValue��Ϊ�ַ�����ֵ���о�
        //		�о�Ϊ˫�ַ�������֣����˫�ַ���β��ͬ��ʵ���ϱ�ʾ�˵��ַ�

        internal int m_nValue = 0;	// ֵ��һ������MC_MULTI���͵�Ԫ��ź��������Ĺ̶�����ƥ�������ܿ��
        internal int m_nStartOffs = 0;	// ��ʼƥ���ƫ��
        internal bool m_bMatch = false;		// ����Ԫ�Ƿ�ƥ��ɹ�
        internal int m_nStyle = 0;		// ����Ԫ���

    }

    /// <summary>
    /// ͨ���
    /// </summary>
    public class WildMatch
    {
        string m_strMode = "";
        string m_strModeHead = "";
        bool m_bCaseSensitive = false;
        List<ModeCell> m_ModeArray = null;

        public char CHAR_MULTI = '%';
        public char CHAR_SINGLE = '_';
        public char CHAR_QUOTELEFT = '[';
        public char CHAR_QUOTERIGHT = ']';

        const int MC_MULTI = 1;	// ����0-����ַ�
        const int MC_SINGLE = 2;	// ���ⵥ���ַ�	
        const int MC_CHAR = 3;	// �����ַ�
        const int MC_STRING = 4;	// ����ַ�
        const int MC_CHARLIST = 5;	// �ַ��������о�

        const int MC_STYLE_RIGHTMOST_MULTI = 0x01;	// ���ұ�һ��MS_MULTI���͵�Ԫ


        // ���캯��
        // parameters:
        //      strWildCharDef  ͨ���ַ��Ķ��塣���Ϊnull����ʾ��ȱʡ�ģ��൱��"%_[]"
        //              ˳����Ҫ���� �����ַ� �����ַ� �ַ��оٵ������� �ַ��оٵ�������
        public WildMatch(string strPattern,
            string strWildCharDef)
        {
            if (String.IsNullOrEmpty(strWildCharDef) == false)
            {
                if (strWildCharDef.Length >= 1)
                    this.CHAR_MULTI = strWildCharDef[0];
                if (strWildCharDef.Length >= 2)
                    this.CHAR_SINGLE = strWildCharDef[1];
                if (strWildCharDef.Length >= 3)
                    this.CHAR_QUOTELEFT = strWildCharDef[2];
                if (strWildCharDef.Length >= 4)
                    this.CHAR_QUOTERIGHT = strWildCharDef[3];
            }

            MakeMatchArray(strPattern);
        }

        /*

        public static int[] GetNext(string a)
        {
            int[] next = new int[a.Length];

            next[0] = 0;
            int i = 0;
            int j = 0;
            while (i < next.Length - 1)
            {
                if (j == 0 || a[i] == a[j - 1])
                {
                    ++i;
                    ++j;
                    if (a[i] != a[j - 1])
                        next[i] = j;
                    else
                        next[i] = next[j - 1];
                }
                else
                    j = next[j - 1];
            }

            return next;
        }

        public static int KMP(string zc, string ppc)
        {
            int[] next = Program.GetNext(ppc);
            int i = 0; int j = 0;
            while (i < zc.Length && j < ppc.Length)
            {
                if (j == 0 || zc[i] == ppc[j - 1])
                {
                    ++i;
                    ++j;
                }
                else
                    j = next[j - 1];
            }
            if (j >= ppc.Length)
                return i - ppc.Length + 1;//ƥ��Ļ�����startindex
            else
                return -1;//��ƥ��Ļ�����-1
        }
         * */

        static string GetModeHead(string strMode)
        {
            string strModeHead = "";

            if (strMode.Length != 0)
            {
                char chFirst;

                chFirst = strMode[0];

                if (chFirst == '_'
                    || chFirst == '%'
                    || chFirst == '[')
                {
                    // ��
                }
                else
                {
                    int nRet;
                    // ��ȡǰ�洿����һ��
                    nRet = strMode.IndexOfAny(new char[] { '_', '%', '[' });
                    if (nRet != -1)
                    {
                        strModeHead = strMode.Substring(0, nRet);
                    }
                    else
                    {
                        strModeHead = strMode;
                    }

                }
            }
            return strModeHead;	// ��
        }


        // ����ƥ��ģʽ����
        // return:
        //		-1	error
        //		0	suceed
        int MakeMatchArray(string strMode)
        {
            // LPTSTR p;
            ModeCell cell = null;
            bool bInQuote = false;
            char chMin = (char)0;
            char chMax = (char)0;
            bool bMinFited = false;

            // ����
            this.m_strMode = strMode;

            this.m_strModeHead = GetModeHead(strMode);

            if (this.m_ModeArray == null)
                this.m_ModeArray = new List<ModeCell>();
            else
                this.m_ModeArray.Clear();

            for (int i = 0; i < strMode.Length; i++)
            {
                char ch = strMode[i];

                if (ch == CHAR_MULTI)
                {
                    cell = new ModeCell();
                    m_ModeArray.Add(cell);
                    cell.m_nType = MC_MULTI;
                    cell = null;
                    continue;
                }

                if (ch == CHAR_SINGLE)
                {
                    cell = new ModeCell();
                    m_ModeArray.Add(cell);
                    cell.m_nType = MC_SINGLE;
                    cell = null;
                    continue;
                }

                if (ch == CHAR_QUOTERIGHT)
                {
                    if (bInQuote == false)
                        return -1;
                    Debug.Assert(cell != null, "");
                    Debug.Assert(cell.m_nType == MC_CHARLIST, "");
                    if (chMin != (char)0)
                    {
                        chMax = chMin;
                        cell.m_strValue += chMin;
                        cell.m_strValue += chMax;
                        chMin = (char)0;
                        chMax = (char)0;
                        bMinFited = false;
                    }
                    // ��β
                    bInQuote = false;
                    cell = null;
                    continue;
                }

                if (ch == CHAR_QUOTELEFT)
                {
                    cell = new ModeCell();
                    m_ModeArray.Add(cell);
                    cell.m_nType = MC_CHARLIST;
                    bInQuote = true;
                    continue;
                }

                if (bInQuote == true)
                {
                    Debug.Assert(cell != null, "");
                    Debug.Assert(cell.m_nType == MC_CHARLIST, "");

                    if (chMin != (char)0 && bMinFited == false)
                    {
                        if (ch == '-')
                        {
                            bMinFited = true;
                            continue;
                        }
                        else
                        {
                            // ��дǰһ��
                            chMax = chMin;
                            cell.m_strValue += chMin;
                            cell.m_strValue += chMax;
                            chMin = (char)0;
                            chMax = (char)0;
                            bMinFited = false;
                            // ��д��һ��(��ǰ��һ��)
                            cell.m_strValue += ch;
                            cell.m_strValue += ch;
                            continue;
                        }
                    }

                    if (chMin != (char)0 && bMinFited == true)
                    {
                        chMax = ch;
                        // min > max��ô��?
                        // min > max��ô��?
                        if (chMin > chMax)
                        {
                            char chTemp = chMin;
                            chMin = chMax;
                            chMax = chTemp;
                        }
                        cell.m_strValue += chMin;
                        cell.m_strValue += chMax;
                        chMin = (char)0;
                        chMax = (char)0;
                        bMinFited = false;
                        continue;
                    }
                    if (chMin == (char)0)
                    {
                        Debug.Assert(bMinFited == false, "");
                        chMin = ch;
                        continue;
                    }

                }

                Debug.Assert(bInQuote == false, "");

                if (cell != null)
                {
                    Debug.Assert(cell.m_nType == MC_STRING, "");
                }
                else
                {
                    cell = new ModeCell();
                    m_ModeArray.Add(cell);
                    cell.m_nType = MC_STRING;
                }
                cell.m_strValue += ch;

            }


            // ��Ҫ��������MC_MULTI���͵�Ԫ�鲢?

            return 0;
        }

        // ��������е���ʱ������ �����һЩֵ
        // 
        void InitialArrayVars()
        {
            int nRightMostMulti = -1;	// ���ұ�һ��MS_MULTI��Ԫ
            int nFixWidth = 0;
            ModeCell lastMultiCell = null;

            ModeCell cell = null;

            for (int i = 0; i < this.m_ModeArray.Count; i++)
            {
                cell = m_ModeArray[i];
                cell.m_nStartOffs = 0;
                cell.m_bMatch = false;
                cell.m_nStyle = 0;

                if (cell.m_nType == MC_MULTI)
                {
                    if (lastMultiCell != null)
                    {
                        if (nFixWidth == 0)
                        {
                            // �����ŵ�����MC_MULTIҪȥ��
                            Debug.Assert(i != 0, "");
                            m_ModeArray.RemoveAt(i);
                            i--;
                            cell = m_ModeArray[i];
                            Debug.Assert(lastMultiCell == cell, "");
                        }
                        lastMultiCell.m_nValue = nFixWidth;
                    }
                    nFixWidth = 0;

                    nRightMostMulti = i;

                    lastMultiCell = cell;
                    continue;
                }
                /*
    #define MC_SINGLE		2	// ���ⵥ���ַ�	
    #define MC_CHAR			3	// �����ַ�
    #define MC_STRING		4	// ����ַ�
    #define MC_CHARLIST		5	// �ַ��������о�
                */
                if (cell.m_nType == MC_SINGLE
                    || cell.m_nType == MC_CHAR
                    || cell.m_nType == MC_CHARLIST)
                    nFixWidth += 1;
                else if (cell.m_nType == MC_STRING)
                    nFixWidth += cell.m_strValue.Length;
                else
                {
                    Debug.Assert(false, "");	// �����ܳ��ֵ�����
                }

            }

            if (lastMultiCell != null)
                lastMultiCell.m_nValue = nFixWidth;

            if (nRightMostMulti != -1)
            {
                cell = m_ModeArray[nRightMostMulti];
                cell.m_nStyle = MC_STYLE_RIGHTMOST_MULTI;
            }
        }

        // ��һ���ַ�������ƥ��
        // parameters:
        //		strResult	ƥ���ϵ��ַ����ֲ�����
        // return:
        //		-1	not match
        //		����	�״�ƥ���λ��
        public int Match(string strString,
                out string strResult)
        {
            int nStartOffs = 0;
            ModeCell cell = null;
            ModeCell firstcell = null;
            int nRet;
            int nWidth;
            int nUsedIdx;

            strResult = "";

            InitialArrayVars();

            if (m_ModeArray.Count == 0)
                return 0;

            for (int i = 0; i < this.m_ModeArray.Count; )
            {
                cell = m_ModeArray[i];

                if (i == 0)
                    firstcell = cell;

                if (cell.m_nType == MC_MULTI)
                {
                    if (i == this.m_ModeArray.Count - 1)	// ��ǰ��Ԫ�Ѿ������һ����Ԫ
                        goto END1;
                    if ((cell.m_nStyle & MC_STYLE_RIGHTMOST_MULTI) != 0)
                    {
                        int nTempOffs;
                        nTempOffs = strString.Length - cell.m_nValue;
                        if (nTempOffs < nStartOffs)
                            return -1;
                        nStartOffs = nTempOffs;
                    }

                    cell.m_nStartOffs = nStartOffs;
                    // ƥ��̶����ȵ�һ������
                    // return:
                    //		-1	error
                    //		0	not match
                    //		1	match
                    //		2	cell index out of range
                    //		3	�Ƚϵ������Ѿ�Խ���ַ������ұ�
                    nRet = MatchFixedLength(i + 1,
                        strString,
                        nStartOffs,
                        out nWidth,
                        out nUsedIdx);
                    if (nRet == -1)
                    {
                        Debug.Assert(false, "");
                        return -1;
                    }
                    if (nRet == 0)
                    { // not match
                        nStartOffs += 1;
                        cell.m_nStartOffs = nStartOffs;
                        continue;
                    }
                    if (nRet == 1)
                    { // match
                        nStartOffs += nWidth;
                        //cell.m_nStartOffs;
                        i += nUsedIdx + 1;
                        continue;	// �����Ѿ��ȽϹ��ľ��룬�����ȽϺ��浥Ԫ
                    }
                    if (nRet == 2)	// ��ǰ��Ԫ�Ѿ������һ����Ԫ
                        goto END1;

                    if (nRet == 3)
                    {
                        return -1;
                    }


                }

                // ֱ��������MC_MULTI��Ԫ
                nRet = MatchFixedLength(i,
                    strString,
                    nStartOffs,
                    out nWidth,
                    out nUsedIdx);
                if (nRet == -1)
                {
                    Debug.Assert(false, "");
                    return -1;
                }
                if (nRet == 0)
                { // not match
                    return -1;
                }
                if (nRet == 1)
                { // match
                    nStartOffs += nWidth;
                    i += nUsedIdx;
                    continue;	// �����Ѿ��ȽϹ��ľ��룬�����ȽϺ��浥Ԫ
                }
                if (nRet == 3)
                {
                    return -1;
                }
            }

            if (nStartOffs < strString.Length)
                return -1;

        END1:
            if (firstcell.m_nType == MC_MULTI)
                return firstcell.m_nStartOffs;	// found, match pos
            return 0;	// found, match pos
        }

        // ���Դ�Сд�ıȽ�
        static int memicmp(string s1,
            int start1,
            string s2,
            int start2,
            int len)
        {
            if (start1 + len > s1.Length
                || start2 + len > s2.Length)
            {
                Debug.Assert(false, "memicmp()������start+len������ĳһ�ַ�����β��");
            }

            for (int i = 0; i < len; i++)
            {
                char ch1 = char.ToLower(s1[start1 + i]);
                char ch2 = char.ToLower(s2[start2 + i]);


                int delta = ch1 - ch2;
                if (delta != 0)
                    return delta;
            }

            return 0;
        }

        // �Ƚ�
        static int memcmp(string s1,
            int start1,
            string s2, 
            int start2,
            int len)
        {
            if (start1 + len > s1.Length
                || start2 + len > s2.Length)
            {
                Debug.Assert(false, "memicmp()������start+len������ĳһ�ַ�����β��");
            }


            for(int i=0;i<len;i++)
            {
                char ch1 = s1[start1+i];
                char ch2 = s2[start2+i];

                int delta = ch1 - ch2;
                if (delta != 0)
                    return delta;
            }

            return 0;
        }

        // ƥ��̶����ȵ�һ������
        // parameters:
        //		nCellIdx	��ʼ��_CModeCell��Ԫ�±�
        //		pszString	�������ַ���
        //		bRightMost	�Ƿ�Ϊ����һ����Ԫ����Ҫ����ƥ��(�Ҷ���)
        //		nWidth		�ӿ�ʼ��_CModeCell��Ԫֱ��MC_MUITL��Ԫ֮�䣬�̶������ֵ��ܳ���
        //		nUsedIdx	�����̶�����Ԫ�ĸ���
        // return:
        //		-1	error
        //		0	not match
        //		1	match
        //		2	cell index out of range
        //		3	�Ƚϵ������Ѿ�Խ���ַ������ұ�
        int MatchFixedLength(int nCellIdx,
            string strString,
            int start,
            out int nWidth,
            out int nUsedIdx)
        {
            nWidth = 0;
            nUsedIdx = 0;

            ModeCell cell = null;
            int nOffs = 0;
            int nPartLen = 0;
            int nStringLen;

            nStringLen = strString.Length - start;

            int nMax = m_ModeArray.Count;
            if (nCellIdx >= nMax)
                return 2;

            for (int i = nCellIdx; i < this.m_ModeArray.Count; i++, nUsedIdx++)
            {
                cell = m_ModeArray[i];

                if (cell.m_nType == MC_MULTI)
                {
                    nWidth = nOffs;
                    return 1;
                }

                if (cell.m_nType == MC_STRING)
                {
                    nPartLen = cell.m_strValue.Length;
                    Debug.Assert(nPartLen != 0, "");

                    if (nOffs + nPartLen > nStringLen)
                        return 3;

                    if (m_bCaseSensitive)
                    {
                        if (memcmp(cell.m_strValue, 0,
                            strString, start + nOffs,
                            nPartLen)
                            == 0)
                        {
                            nOffs += nPartLen;
                            continue;
                        }
                    }
                    else
                    {
                        if (memicmp(cell.m_strValue, 0,
                            strString, start + nOffs,
                            nPartLen) == 0)
                        {
                            nOffs += nPartLen;
                            continue;
                        }
                    }
                    return 0;
                }
                else if (cell.m_nType == MC_CHARLIST)
                {
                    char chStart;
                    char chEnd;
                    char chChar = strString[start + nOffs];
                    bool bFound = false;
                    nPartLen = 1;

                    if (nOffs + nPartLen > nStringLen)
                        return 3;	// not match
                    int nCount = cell.m_strValue.Length;

                    Debug.Assert(nCount % 2 == 0, "");	// ������ż��
                    nCount = nCount / 2;
                    for (int j = 0; j < nCount; j++)
                    {
                        chStart = cell.m_strValue[j * 2];
                        chEnd = cell.m_strValue[j * 2 + 1];
                        if (chChar < chStart
                            || chChar > chEnd)
                            continue;	// not match
                        else
                        {
                            bFound = true;
                            break;
                        }
                    }
                    if (bFound == false)
                        return 0;

                    nOffs += nPartLen;
                }
                else if (cell.m_nType == MC_SINGLE)
                {
                    nPartLen = 1;

                    if (nOffs + nPartLen > nStringLen)
                        return 3;	// not match
                    nOffs += nPartLen;
                    continue;
                }
                else
                {
                    Debug.Assert(false, "");
                }


            }


            nWidth = nOffs;
            return 1;	// match
        }

    }



}
