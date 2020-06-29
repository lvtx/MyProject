using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DigitalPlatform.MarcDom
{
    /// <summary>
    /// FilterDocument �洢������
    /// �������������� FilterDocument ����
    /// </summary>
    public class FilterCollection
    {
        Hashtable _table = new Hashtable();

        public bool IgnoreCase = true;

        ReaderWriterLock _lock = new ReaderWriterLock();
        static int _nLockTimeout = 5000;	// 5000=5��

        public int Max = 100;   // ÿ��List�ж��������ޡ�100

        public FilterDocument GetFilter(string strName)
        {
            if (IgnoreCase == true)
                strName = strName.ToLower();

            FilterList filterlist = null;

            this._lock.AcquireWriterLock(_nLockTimeout);
            try
            {
                // �鿴һ�������Ƿ��ж�Ӧ�� FilterList ������
                filterlist = (FilterList)_table[strName];

                // �����û�У��򴴽�һ���¶���
                if (filterlist == null)
                {
                    filterlist = new FilterList();
                    filterlist.Container = this;
                    _table[strName] = filterlist;
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }

            // �� FilterList �����л�ȡһ�� FilterDocument ����
            // ע�� FilterList �������ڹ����� FilterDocument �������б����õ���ʱ�Ͳ��ܱ�ʹ���ˣ���Ҫ�����¶���
            FilterDocument filter = filterlist.GetFilter();
            return filter;
        }

        public void Clear()
        {
            this._lock.AcquireWriterLock(_nLockTimeout);
            try
            {
                this._table.Clear();
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        // �Ӽ���������ض����ֵ� FilterList ����
        public void ClearFilter(string strName)
        {
            if (IgnoreCase == true)
                strName = strName.ToLower();

            this._lock.AcquireWriterLock(_nLockTimeout);
            try
            {
                FilterList filterlist = (FilterList)_table[strName];
                if (filterlist != null)
                    _table.Remove(strName);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public void SetFilter(string strName,
            FilterDocument filter)
        {
            if (IgnoreCase == true)
                strName = strName.ToLower();
            FilterList filterlist = null;

            this._lock.AcquireWriterLock(_nLockTimeout);
            try
            {
                filterlist = (FilterList)_table[strName];

                if (filterlist == null)
                {
                    filterlist = new FilterList();
                    filterlist.Container = this;
                    _table[strName] = filterlist;
                }
                Debug.Assert(filterlist != null, "");
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }

            filterlist.SetFilter(filter);
        }

        public int Count
        {
            get
            {
                return this._table.Count;
            }
        }

        public string Dump()
        {
            string strResult = "";

            strResult += "�������й���'" + Convert.ToString(this._table.Count) + "'��FilterList����.\r\n";

            foreach (DictionaryEntry item in _table)
            {
                strResult += "  " + item.Key + "\r\n";

                FilterList list = (FilterList)item.Value;

                strResult += "    " + list.Dump();
            }

            return strResult;
        }
    }

    /// <summary>
    /// ���� FilterDocument ��������ɸ�����
    /// ȷ�� FilterDocument �����������ڼ䱻��ռʹ��
    /// </summary>
    public class FilterList
    {
        List<FilterHolder> _list = new List<FilterHolder>();

        ReaderWriterLock _lock = new ReaderWriterLock();
        static int _nLockTimeout = 5000;	// 5000=5��

        private FilterCollection _container = null;

        public FilterCollection Container
        {
            get
            {
                return _container;
            }

            set
            {
                _container = value;
            }
        }

        /// <summary>
        /// ���һ����δ�����õ� FilterDocument ��������������
        /// </summary>
        /// <returns>FilterDocument ����</returns>
        public FilterDocument GetFilter()
        {
            this._lock.AcquireReaderLock(_nLockTimeout);
            try
            {
                foreach (FilterHolder item in _list)
                {
                    if (Interlocked.Increment(ref item.UsedCount) == 1)
                        return item.FilterDocument;

                    Interlocked.Decrement(ref item.UsedCount);
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// �黹���߼���һ�� FilterDocument ����
        /// ��������� list ���Ѿ����ڣ���黹��������� list �в����ڣ�������������� list����ʱ������δ������״̬��
        /// �� list �����������ǽ����˿��Ƶ�
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>true: �ɹ�; false: û�ܼ��� list����Ϊ������������ Max ��</returns>
        public bool SetFilter(FilterDocument filter)
        {
            // string strMessage = "";

            this._lock.AcquireReaderLock(_nLockTimeout);
            try
            {
                foreach (FilterHolder item in _list)
                {
                    if (item.FilterDocument == filter)
                    {
                        int nValue = Interlocked.Decrement(ref item.UsedCount);
                        if (nValue < 0)
                        {
                            throw new Exception("���غ�UsedCountС��0, ����");
                        }

                        return true;
                    }
                }
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }

            return NewFilter(filter);
        }

#if NO  // ��ʱû���õ�
        public void ReturnFilter(FilterDocument filter)
        {
            this.m_lock.AcquireReaderLock(m_nLockTimeout);
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    FilterHolder item = this.list[i];

                    if (item.FilterDocument == filter)
                    {
                        int nValue = Interlocked.Decrement(ref item.UsedCount);
                        if (nValue < 0)
                        {
                            throw new Exception("���غ�UsedCountС��0, ����");
                        } 
                        return;
                    }
                }
            }
            finally
            {
                this.m_lock.ReleaseReaderLock();
            }

            throw new Exception("���صĶ�����������û���ҵ�");
        }
#endif

        // ����һ�� FilterDocument ����
        // ��� list �еĶ������������� Max ���ޣ��򲻻����
        // return:
        //      true    �Ѿ�����
        //      false   δ����
        public bool NewFilter(FilterDocument filter)
        {
            this._lock.AcquireWriterLock(_nLockTimeout);
            try
            {
                if (this._list.Count >= this._container.Max)
                    return false;

                FilterHolder item = new FilterHolder();
                item.FilterDocument = filter;
                this._list.Add(item);
                return true;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public string Dump()
        {
            string strResult = "";

            strResult += "����'" + Convert.ToString(this._list.Count) + "'��FilterDocumnet����.\r\n";

            return strResult;
        }
    }

    /// <summary>
    /// ���� FilterDocument �����������ṩ��ʹ�ü���
    /// </summary>
    public class FilterHolder
    {
        public FilterDocument FilterDocument = null;
        public int UsedCount = 0;
    }
}
