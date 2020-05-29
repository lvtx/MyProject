using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using DigitalPlatform.Xml;
using DigitalPlatform.Text;

namespace DigitalPlatform.rms
{
    public class QueryUtil
    {
        // ����nodeItem�õ������������Ϣ
        // parameter:
        //		nodeItem	���ýڵ�
        //		strTarget	out���������ؼ���Ŀ��,���Ƕ��Ķ�;��
        //		strWord	    out���������ؼ�����
        //		strMatch	out����������ƥ�䷽ʽ
        //		strRelation	out���������ع�ϵ��
        //		strDataType	out���������ؼ�������������
        //		strIdOrder	out����������id���������
        //		strKeyOrder	out����������key���������
        //		strOrderBy	out������������order��originOrder��ϵ������������" keystring ASC,idstring DESC "
        //		nMax	    out�����������������
        //		strError	out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�ɹ�
        public static int GetSearchInfo(XmlNode nodeItem,
            out string strTarget,
            out string strWord,
            out string strMatch,
            out string strRelation,
            out string strDataType,
            out string strIdOrder,
            out string strKeyOrder,
            out string strOrderBy,
            out int nMaxCount,
            out string strError)
        {
            strTarget = "";
            strWord = "";
            strMatch = "";
            strRelation = "";
            strDataType = "";
            strIdOrder = "";
            strKeyOrder = "";
            strOrderBy = "";
            nMaxCount = 0;
            strError = "";

            //--------------------------------------
            //��GetTarget�������õ�����Ŀ��target�ڵ�
            XmlNode nodeTarget = QueryUtil.GetTarget(nodeItem);
            if (nodeTarget == null)
            {
                strError = "����ʽ��targetԪ��δ����";
                return -1;
            }
            strTarget = DomUtil.GetAttrDiff(nodeTarget, "list");
            if (strTarget == null)
            {
                strError = "targetԪ�ص�list����δ����";
            }
            strTarget = strTarget.Trim();
            if (strTarget == "")
            {
                strError = "targetԪ�ص�list����ֵ����Ϊ���ַ���";
                return -1;
            }


            //-------------------------------------------
            //�����ı� ����Ϊ���ַ���
            XmlNode nodeWord = nodeItem.SelectSingleNode("word");
            if (nodeWord == null)
            {
                strError = "����ʽ��wordԪ��δ����";
                return -1;
            }
            strWord = DomUtil.GetNodeText(nodeWord);
            strWord = strWord.Trim();


            //------------------------------------
            //ƥ�䷽ʽ
            XmlNode nodeMatch = nodeItem.SelectSingleNode("match");
            if (nodeMatch == null)
            {
                strError = "����ʽ��matchԪ��δ����";
                return -1;
            }
            strMatch = DomUtil.GetNodeText(nodeMatch);
            strMatch = strMatch.Trim();
            if (strMatch == "")
            {
                strError = "����ʽ��matchԪ�����ݲ���Ϊ���ַ���";
                return -1;
            }
            if (QueryUtil.CheckMatch(strMatch) == false)
            {
                strError = "����ʽ��matchԪ������'" + strMatch + "'���Ϸ�������Ϊleft,middle,right,exact";
                return -1;
            }

            //--------------------------------------------
            //��ϵ������
            XmlNode nodeRelation = nodeItem.SelectSingleNode("relation");
            if (nodeRelation == null)
            {
                strError = "����ʽ��relationԪ��δ����";
                return -1;
            }
            strRelation = DomUtil.GetNodeText(nodeRelation);
            strRelation = strRelation.Trim();
            if (strRelation == "")
            {
                strError = "����ʽ��relationԪ�����ݲ���Ϊ���ַ���";
                return -1;
            }
            strRelation = QueryUtil.ConvertLetterToOperator(strRelation);
            if (QueryUtil.CheckRelation(strRelation) == false)
            {
                strError = "����ʽ��relationԪ������'" + strRelation + "'���Ϸ�.";
                return -1;
            }

            //-------------------------------------------
            //��������
            XmlNode nodeDataType = nodeItem.SelectSingleNode("dataType");
            if (nodeDataType == null)
            {
                strError = "����ʽ��dataTypeԪ��δ����";
                return -1;
            }
            strDataType = DomUtil.GetNodeText(nodeDataType);
            strDataType = strDataType.Trim();
            if (strDataType == "")
            {
                strError = "����ʽ��dataTypeԪ�����ݲ���Ϊ���ַ���";
                return -1;
            }
            if (QueryUtil.CheckDataType(strDataType) == false)
            {
                strError = "����ʽ��dataTypeԪ������'" + strDataType + "'���Ϸ�������Ϊstring,number";
                return -1;
            }


            // ----------order���Բ�����----------
            int nOrderIndex = -1;
            string strOrder = null;
            int nOriginOrderIndex = -1;
            string strOriginOrder = null;

            //id����  //ASC:����  //DESC:����
            XmlNode nodeOrder = nodeItem.SelectSingleNode("order");
            // ��������orderԪ��ʱ���Ż�id��������
            if (nodeOrder != null)
            {
                string strOrderText = DomUtil.GetNodeText(nodeOrder);
                strOrderText = strOrderText.Trim();
                if (strOrderText != "")
                {
                    strOrder = "idstring " + strOrderText;
                    nOrderIndex = DomUtil.GetIndex(nodeOrder);
                    strIdOrder = strOrderText;
                }
            }

            //key����  //ASC:����  //DESC:����
            XmlNode nodeOriginOrder = nodeItem.SelectSingleNode("originOrder");
            // ��������orderԪ��ʱ���Ż�id��������
            if (nodeOriginOrder != null)
            {
                string strOriginOrderText = DomUtil.GetNodeText(nodeOriginOrder);
                strOriginOrderText = strOriginOrderText.Trim();
                if (strOriginOrderText != "")
                {
                    strOriginOrder = "keystring " + strOriginOrderText;
                    nOriginOrderIndex = DomUtil.GetIndex(nodeOriginOrder);
                    strKeyOrder = strOriginOrderText;
                }
            }

            if (strOrder != null
                && strOriginOrder != null)
            {
                if (nOrderIndex == -1
                    || nOriginOrderIndex == -1)
                {
                    strError = "��ʱnOrderIndex��nOriginOrderIndex��������Ϊ-1";
                    return -1;
                }
                if (nOrderIndex == nOriginOrderIndex)
                {
                    strError = "nOrderIndex �� nOriginOrderIndex���������";
                    return -1;
                }
                if (nOrderIndex > nOriginOrderIndex)
                {
                    strOrderBy = strOrder + "," + strOriginOrder;
                }
                else
                {
                    strOrderBy = strOriginOrder + "," + strOrder;
                }
            }
            else
            {
                if (strOrder != null)
                    strOrderBy = strOrder;
                if (strOriginOrder != null)
                    strOrderBy = strOriginOrder;
            }


            //-------------------------------------------
            //������
            XmlNode nodeMaxCount = nodeItem.SelectSingleNode("maxCount");
            /*			
                        if (nodeMaxCount == null)
                        {
                            strError = "����ʽ��maxCountԪ��δ����";
                            return -1;
                        }
            */
            string strMaxCount = "";
            if (nodeMaxCount != null)
                strMaxCount = DomUtil.GetNodeText(nodeMaxCount).Trim();
            if (strMaxCount == "")
                strMaxCount = "-1";
            /*
                        if (strMaxCount == "")
                        {
                            strError = "����ʽ��maxCountԪ�ص�ֵΪ���ַ���";
                            return -1;
                        }
            */
            try
            {
                nMaxCount = Convert.ToInt32(strMaxCount);
                if (nMaxCount < -1)
                {
                    strError = "xml����ʽ��maxCount��ֵ'" + strMaxCount + "'���Ϸ�,��������ֵ��";
                    return -1;
                }
            }
            catch
            {
                strError = "xml����ʽ��maxCount��ֵ'" + strMaxCount + "'���Ϸ�,��������ֵ��";
                return -1;
            }

            return 0;
        }


        // ���ƥ�䷽ʽ�ַ����Ƿ�Ϸ�
        // return:
        //      true    �Ϸ�
        //      false   ���Ϸ�
        public static bool CheckMatch(string strMatch)
        {
            strMatch = strMatch.ToLower();

            string strMatchList = "left,middle,right,exact";
            return StringUtil.IsInList(strMatch, strMatchList);
        }

        // ע: ���Ⱥ�Ϊ!=
        public static bool CheckRelation(string strRelation)
        {
            strRelation = strRelation.ToLower();

            string strRelationList = ">,>=,<,<=,=,!=,draw";
            return StringUtil.IsInList(strRelation, strRelationList);
        }

        // �����������
        public static bool CheckDataType(string strDataType)
        {
            strDataType = strDataType.ToLower();
            string strDataTypeList = "string,number";
            return StringUtil.IsInList(strDataType, strDataTypeList);
        }

        // �õ��ϼ�target�ڵ�
        // parameter:
        //		nodeItem    item�ڵ�
        // return:
        //		target�ڵ㣬û�ҵ�����null
        private static XmlNode GetTarget(XmlNode nodeItem)
        {
            XmlNode nodeCurrent = nodeItem;
            while (true)
            {
                if (nodeCurrent == null)
                    break;

                if (nodeCurrent.Name == "target")
                    return nodeCurrent;

                nodeCurrent = nodeCurrent.ParentNode;
            }
            return null;
        }


        // ����ĸ��ʾ���Ĺ�ϵ���ĳɷ��ű�ʾ��
        // parameter:
        //		strLetterRelation   ��ĸ��ʾ���Ĺ�ϵ��
        // return:
        //		���ط��ű�ʾ���Ĺ�ϵ��
        public static string ConvertLetterToOperator(string strLetterRelation)
        {
            string strResult = strLetterRelation;
            if (strLetterRelation == "E")
                strResult = "=";

            if (strLetterRelation == "GE")
                strResult = ">=";

            if (strLetterRelation == "LE")
                strResult = "<=";

            if (strLetterRelation == "G")
                strResult = ">";

            if (strLetterRelation == "L")
                strResult = "<";

            if (strLetterRelation == "NE")
                strResult = "!=";

            return strResult;
        }

        // У���ϵ��ע������׳�NoMatch�쳣
        public static int VerifyRelation(ref string strMatch,
            ref string strRelation,
            ref string strDataType)
        {
            if (strDataType == "number")
            {
                if (strMatch == "left" || strMatch == "right" || strMatch == "middle")
                {
                    strMatch = "exact";
                    //�޸�������:1.��left����exact;2.��dataType��Ϊstring,�����Ȱ���ΪdataType���ȣ���match��Ϊexact

                    NoMatchException ex =
                        new NoMatchException("ƥ�䷽ʽ'" + strMatch + "'����������" + strDataType + "��ƥ��");
                    throw (ex);
                }
            }

            if (strDataType == "string")
            {
                if (strMatch == "left" || strMatch == "right" || strMatch == "middle")
                {
                    if (strRelation != "=")
                    {
                        //Ҳ���Խ�left��right��Ϊexact�������岻��
                        strRelation = "=";
                        NoMatchException ex =
                            new NoMatchException("��ϵ������'" + strRelation + "'����������" + strDataType + "��ƥ�䷽ʽ'" + strMatch + "'��ƥ��");

                        throw (ex);
                    }
                }
            }
            return 0;
        }
    }
}
