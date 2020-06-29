using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DigitalPlatform.MarcDom;

// 2013/3/16 ��� XML ע��

namespace dp2Circulation
{
    /// <summary>
    /// ��� ColumnFilterHost �� FilterDocument ������(MARC �������ĵ���)
    /// </summary>
    public class ColumnFilterDocument : FilterDocument
    {
        /// <summary>
        /// ��������
        /// </summary>
        public ColumnFilterHost Host = null;
    }

    /// <summary>
    /// BiblioSearchForm (��Ŀ��ѯ��) �� .fltx �ű����ܵ�������
    /// </summary>
    public class ColumnFilterHost
    {
        /// <summary>
        /// ������
        /// </summary>
        public Hashtable ColumnTable = null;// ����

        /// <summary>
        /// �Ӿ��������
        /// </summary>
        public object UiItem = null;
    }
}
