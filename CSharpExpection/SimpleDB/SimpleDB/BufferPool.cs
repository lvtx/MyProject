using System;
using System.Collections;
using System.IO;

namespace SimpleDB
{
    /// <summary>
    /// BufferPool管理从磁盘到内存的页面读取和写入
    /// 访问方法对其进行调用以检索页面，然后从适当的位置获取页面
    /// BufferPool还负责锁定
    /// 当事务获取页面时，BufferPool会检查该事务是否具有适当的锁来读取/写入页面
    /// </summary>
    public class BufferPool
    {
        // 每页字节数，包括标题
        private static readonly int PAGE_SIZE = 4096;

        private static int pageSize = PAGE_SIZE;

        // 传递给构造函数的默认页面数
        // 其他类使用此方法
        // BufferPool应该使用numPages作为构造函数参数
        public static readonly int DEFAULT_PAGES = 50;
        // 用来缓存已经查找过的页面,下次查找可以直接读取
        Hashtable bufferPool = null;
        /// <summary>
        /// 创建一个最多可缓存numPages页的BufferPool
        /// </summary>
        /// <param name="numPages">此缓冲池中的最大页面数</param>
        public BufferPool(int numPages)
        {
            bufferPool = new Hashtable(numPages);
        }

        public static int GetPageSize()
        {
            return pageSize;
        }

        // THIS FUNCTION SHOULD ONLY BE USED FOR TESTING!!
        public static void SetPageSize(int pageSize)
        {
            BufferPool.pageSize = pageSize;
        }

        /// <summary>
        /// 检索具有关联权限的指定页面
        /// 获取一个锁，如果该锁被另一笔交易持有，则可能会阻塞
        /// 应该在缓冲池中查找检索到的页面。
        /// 如果存在，则应将其返回。
        /// 如果不存在，则应将其添加到缓冲池中并返回。
        /// 如果缓冲池中没有足够的空间，则应逐出一个页面，并在该位置添加新页面。
        /// </summary>
        /// <param name="tid">请求页面的交易的ID</param>
        /// <param name="pid">被请求页面的id</param>
        /// <param name="perm">被请求页面的权限</param>
        /// <returns></returns>
        public Page GetPage(TransactionId tid, PageId pid, Permissions perm)
        {
            HeapPage hp = null;
            try
            {
                if (bufferPool.ContainsKey(pid))
                {
                    hp = bufferPool[pid] as HeapPage;
                }
                else
                {
                    HeapFile table = Database.GetCatalog().GetDatabaseFile(pid.GetTableId()) as HeapFile;
                    hp = table.ReadPage(pid) as HeapPage;
                    bufferPool.Add(pid, hp);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return hp;
        }

        /**
         * Releases the lock on a page.
         * Calling this is very risky, and may result in wrong behavior. Think hard
         * about who needs to call this and why, and why they can run the risk of
         * calling it.
         *
         * @param tid the ID of the transaction requesting the unlock
         * @param pid the ID of the page to unlock
         */

        public void ReleasePage(TransactionId tid, PageId pid)
        {
            // some code goes here
            // not necessary for lab1|lab2
        }

        /**
         * Release all locks associated with a given transaction.
         *
         * @param tid the ID of the transaction requesting the unlock
         */

        public void TransactionComplete(TransactionId tid)
        {
            // some code goes here
            // not necessary for lab1|lab2
        }

        /** Return true if the specified transaction has a lock on the specified page */

        public bool HoldsLock(TransactionId tid, PageId p)
        {
            // some code goes here
            // not necessary for lab1|lab2
            return false;
        }

        /**
         * Commit or abort a given transaction; release all locks associated to
         * the transaction.
         *
         * @param tid the ID of the transaction requesting the unlock
         * @param commit a flag indicating whether we should commit or abort
         */

        public void TransactionComplete(TransactionId tid, bool commit)
        {
            // some code goes here
            // not necessary for lab1|lab2
        }

        /**
         * Add a tuple to the specified table on behalf of transaction tid.  Will
         * acquire a write lock on the page the tuple is added to and any other
         * pages that are updated (Lock acquisition is not needed for lab2).
         * May block if the lock(s) cannot be acquired.
         *
         * Marks any pages that were dirtied by the operation as dirty by calling
         * their markDirty bit, and updates cached versions of any pages that have
         * been dirtied so that future requests see up-to-date pages.
         *
         * @param tid the transaction adding the tuple
         * @param tableId the table to add the tuple to
         * @param t the tuple to add
         */

        public void InsertTuple(TransactionId tid, int tableId, Tuple t)
        {
            // some code goes here
            // not necessary for lab1
        }

        /**
         * Remove the specified tuple from the buffer pool.
         * Will acquire a write lock on the page the tuple is removed from and any
         * other pages that are updated. May block if the lock(s) cannot be acquired.
         *
         * Marks any pages that were dirtied by the operation as dirty by calling
         * their markDirty bit, and updates cached versions of any pages that have
         * been dirtied so that future requests see up-to-date pages.
         *
         * @param tid the transaction deleting the tuple.
         * @param t the tuple to delete
         */

        public void DeleteTuple(TransactionId tid, Tuple t)
        {
            // some code goes here
            // not necessary for lab1
        }

        /**
         * Flush all dirty pages to disk.
         * NB: Be careful using this routine -- it writes dirty data to disk so will
         *     break simpledb if running in NO STEAL mode.
         */
        //需要加锁
        //    public synchronized void FlushAllPages()
        //    {
        //    // some code goes here
        //    // not necessary for lab1

        //    }

        ///** Remove the specific page id from the buffer pool.
        //    Needed by the recovery manager to ensure that the
        //    buffer pool doesn't keep a rolled back page in its
        //    cache.
        //*/
        //public synchronized void DiscardPage(PageId pid)
        //{
        //    // some code goes here
        //    // only necessary for lab5
        //}

        ///**
        // * Flushes a certain page to disk
        // * @param pid an ID indicating the page to flush
        // */
        //private synchronized void flushPage(PageId pid) throws IOException
        //{
        //    // some code goes here
        //    // not necessary for lab1
        //}

        ///** Write all pages of the specified transaction to disk.
        // */
        //public synchronized void flushPages(TransactionId tid) throws IOException
        //{
        //    // some code goes here
        //    // not necessary for lab1|lab2
        //}

        ///**
        // * Discards a page from the buffer pool.
        // * Flushes the page to disk to ensure dirty pages are updated on disk.
        // */
        //private synchronized void evictPage() throws DbException
        //{
        //    // some code goes here
        //    // not necessary for lab1
        //}
    }
}