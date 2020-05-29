﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalPlatform.rms
{
    /// <summary>
    /// 一个 Page 事项
    /// </summary>
    public class PageItem
    {
        internal ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public string RecordPath { get; set; }
        public int Page { get; set; }
        public int DPI { get; set; }
        public string Format { get; set; }  // jpeg png 等等

        public string FilePath { get; set; }    // 临时文件路径

        public int TotalPage { get; set; }  // 总页数

        string _state = "";   // Lock 表示正在创建物理文件; OK 表示物理文件已经准备好; Freely 表示为自由对象，不被集合管理
        public string State
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _state;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            set
            {
                _lock.EnterWriteLock();
                try
                {
                    _state = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        // 最近一次使用时间
        DateTime _lastUse = DateTime.Now;
        public DateTime LastUse
        {
            get { return _lastUse; }
            set { _lastUse = value; }
        }

        public static string MakeKey(string object_recpath, int page_no, int dpi, string format)
        {
            return object_recpath + "_" + page_no + "_" + dpi + "_" + format;
        }

        public void Delete()
        {
            _lock.EnterWriteLock();
            try
            {
                if (string.IsNullOrEmpty(this.FilePath) == false)
                    File.Delete(this.FilePath);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }


    /// <summary>
    /// 从 PDF 提取的单页图像文件 Cache 机制
    /// </summary>
    public class PageCache
    {
        int MAX_ITEMS = 1000;

        internal ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        Dictionary<string, PageItem> _pageTable = new Dictionary<string, PageItem>();

        public PageCache(int nMaxItems = -1)
        {
            if (nMaxItems != -1)
                MAX_ITEMS = nMaxItems;
        }

        public delegate Tuple<string, int> Delegate_prepareFile();

        // 准备一个 PageItem 对象。如果在 _pageTable 中已经存在了，则直接利用它；如果没有，则新创建一个
        // 但要处理好一个文件，新创建好的 PageItem 对象，其物理文件还没有来得及创建，如果这段时间又有另一个线程并发获得使用了这个对象，则会发生冲突。解决办法是，规定只返回准备好物理文件的 PageItem 对象。这样可能会出现两个线程都同时在创建相似属性的 PageItem 对象的可能
        public PageItem GetPage(string object_recpath,
            int page_no,
            int dpi,
            string format,
            Delegate_prepareFile procPrepareFile,
            Delegate_deleteFile procDeleteFile,
            CancellationToken token)
        {
            string key = PageItem.MakeKey(object_recpath, page_no, dpi, format);
            PageItem item = null;
            bool bNew = false;  // 是否新创建了 PageItem 对象
            bool bFreely = false;   // 是否为独立的 Page Item 对象。独立的 PageItem 对象不被集合管理
            _lock.EnterWriteLock();
            try
            {
                if (_pageTable.TryGetValue(key, out item) == true
                    && item.State == "OK")
                    return item;

                if (item == null)
                {
                    if (_pageTable.Count >= MAX_ITEMS)
                    {
                        // 紧急清理一次
                        Clean(false, TimeSpan.FromMilliseconds(0), procDeleteFile, token);
                    }

                    item = new PageItem
                    {
                        RecordPath = object_recpath,
                        Page = page_no,
                        DPI = dpi,
                        Format = format,
                        State = "Lock"
                    };

                    if (this._pageTable.Count < MAX_ITEMS)
                        _pageTable.Add(key, item);
                    else
                        bFreely = true;
                    bNew = true;
                }
                else
                {
                    // 等待状态变成 OK
                    bNew = false;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            if (bNew == true)
            {
                try
                {
                    // 创建物理文件
                    var ret = procPrepareFile();
                    item.FilePath = ret.Item1;
                    item.TotalPage = ret.Item2;
                    if (bFreely)
                        item.State = "Freely";
                    else
                        item.State = "OK";
                    // Thread.Sleep(10000);
                }
                catch
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        _pageTable.Remove(key);
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }

                    throw;
                }
            }
            else
            {
                TimeSpan timeout = TimeSpan.FromSeconds(10);
                DateTime start = DateTime.Now;
                // 等待状态变为 OK
                while (true)
                {
                    Thread.Sleep(100);
                    if (item.State == "OK")
                        break;
                    // TODO: 超时要抛出异常
                    if (DateTime.Now - start >= timeout)
                        throw new TimeoutException("PageItem 对象被其它请求占用。请稍后重试");
                }
            }
            return item;
        }

#if NO
        void Shrink(Delegate_deleteFile procDeleteFile)
        {
            List<string> keys = new List<string>();
            List<PageItem> items = new List<PageItem>();

            foreach (string key in _pageTable.Keys)
            {
                PageItem item = _pageTable[key];
                if (item.State == "OK")
                {
                    keys.Add(key);
                }
            }

            if (keys.Count > 0)
            {
                foreach (string key in keys)
                {
                    _pageTable.Remove(key);
                }

                foreach (PageItem item in items)
                {
                    if (procDeleteFile != null)
                        procDeleteFile(item.FilePath);
                    else
                        item.Delete();
                }
            }
        }
#endif
        public delegate void Delegate_deleteFile(string strFileName);

        // 清除已经失效的 PageItem
        // parameters:
        //      delta   闲置多少时间以上才清理
        public void Clean(bool bLock,
            TimeSpan delta,
            Delegate_deleteFile procDeleteFile,
            CancellationToken token)
        {
            List<string> keys = new List<string>();
            List<PageItem> items = new List<PageItem>();
            if (bLock)
                _lock.EnterReadLock();
            try
            {
                DateTime now = DateTime.Now;
                foreach (string key in _pageTable.Keys)
                {
                    token.ThrowIfCancellationRequested();

                    PageItem item = _pageTable[key];
                    if (item.State == "OK"
                        && now - item.LastUse > delta)
                    {
                        keys.Add(key);
                        items.Add(item);
                    }
                }
            }
            finally
            {
                if (bLock)
                    _lock.ExitReadLock();
            }

            if (keys.Count > 0)
            {
                if (bLock)
                    _lock.EnterWriteLock();
                try
                {
                    foreach (string key in keys)
                    {
                        token.ThrowIfCancellationRequested();

                        _pageTable.Remove(key);
                    }
                }
                finally
                {
                    if (bLock)
                        _lock.ExitWriteLock();
                }

                foreach (PageItem item in items)
                {
                    token.ThrowIfCancellationRequested();

                    if (procDeleteFile != null)
                        procDeleteFile(item.FilePath);
                    else
                        item.Delete();
                }
            }
        }

        // 清除所有 PageItem 元素。包括删除 PageItem 元素的物理文件
        public void ClearAll()
        {
            _lock.EnterWriteLock();
            try
            {
                foreach (var pair in this._pageTable)
                {
                    try
                    {
                        pair.Value.Delete();
                    }
                    catch
                    {

                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        // 清除和指定路径相关的 PageItem 元素
        public void ClearByRecPath(string strRecPath,
            Delegate_deleteFile procDeleteFile)
        {
            var pairs = new List<KeyValuePair<string, PageItem>>();
            _lock.EnterReadLock();
            try
            {
                DateTime now = DateTime.Now;
                foreach (var pair in _pageTable)
                {
                    PageItem item = pair.Value;
                    if (item.RecordPath == strRecPath
                        || item.RecordPath.StartsWith(strRecPath+ "_"))
                    {
                        pairs.Add(pair);
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if (pairs.Count > 0)
            {
                _lock.EnterWriteLock();
                try
                {
                    foreach (var pair in pairs)
                    {
                        _pageTable.Remove(pair.Key);
                    }
                }
                finally
                {
                    _lock.ExitWriteLock();
                }

                foreach (var pair in pairs)
                {
                    if (procDeleteFile != null)
                        procDeleteFile(pair.Value.FilePath);
                    else
                        pair.Value.Delete();
                }
            }
        }

    }
}
