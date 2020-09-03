using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SimpleDB
{
    /// <summary>
    /// HeapFile是DbFile的实现，该实现以不特定的顺序存储元组的集合
    /// 元组存储在页面上，每个页面的大小都是固定的，并且文件只是这些页面的集合。
    /// HeapFile与HeapPage紧密合作。
    /// HeapPages的格式在HeapPage构造函数中进行了描述。
    /// </summary>
    public class HeapFile : DbFile
    {
        private FileInfo f = null;
        private TupleDesc td = null;
        /// <summary>
        /// 构造一个由指定文件支持的堆文件
        /// </summary>
        /// <param name="f">存储该堆文件的磁盘后备存储的文件</param>
        /// <param name="td">schema</param>
        public HeapFile(FileInfo f, TupleDesc td)
        {
            this.f = f;
            this.td = td;
        }

        /// <summary>
        /// 返回磁盘上备份此堆文件的文件
        /// </summary>
        /// <returns>返回在磁盘上备份此堆文件的文件</returns>
        public FileInfo GetFile()
        {
            return null;
        }
        /// <summary>
        /// 返回唯一标识此堆文件的ID
        /// 实现注意:您需要在某个地方生成这个tableid，
        /// 以确保每个HeapFile都有一个"唯一的id",
        /// 并且您总是为特定的HeapFile返回相同的值
        /// 我们建议对堆文件下面的文件的绝对文件名进行散列
        /// 即f.getAbsoluteFile().hashcode()
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            return f.GetHashCode();
        }
        /// <summary>
        /// Returns the TupleDesc of the table stored in this DbFile.
        /// </summary>
        /// <returns></returns>
        public TupleDesc GetTupleDesc()
        {
            return td;
        }
        /// <summary>
        /// Returns the number of pages in this HeapFile.
        /// </summary>
        /// <returns></returns>
        public int NumPages()
        {
            Int64 npageBytes = BufferPool.GetPageSize();
            Int64 fileSize = f.Length;
            int numPages = (int)(fileSize / npageBytes);
            try
            {
                if (numPages * npageBytes - fileSize < 0)
                {
                    numPages++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("HeapFile.NumPages()异常:{0}", ex);
            }
            return numPages;
        }
        /// <summary>
        /// Read the specified page from disk.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Page ReadPage(PageId id)
        {
            HeapPageId pid = id as HeapPageId;
            int pgno = pid.PageNumber();
            HeapPage hp = null;
            try
            {
                byte[] data = ReadFileBytes(f.FullName,pgno);
                hp = new HeapPage(pid,data);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            return hp;
        }
        /// <summary>
        /// 从指定的文件返回btye数组
        /// </summary>
        /// <param name="path">文件的路径名</param>
        /// <returns></returns>
        private byte[] ReadFileBytes(string path,int pgno)
        {
            FileInfo f = new FileInfo(path);
            using (BinaryReader br = new BinaryReader(f.OpenRead()))
            {
                int npageBytes = BufferPool.GetPageSize();
                byte[] buf = new byte[npageBytes];
                int offset = pgno * npageBytes;
                br.BaseStream.Position = offset;
                try
                {
                    br.Read(buf, 0, npageBytes);
                }
                catch (Exception)
                {
                    Console.WriteLine("Position:{0}", br.BaseStream.Position);
                }
                return buf;
            }                       
        }
        /// <summary>
        /// 测试ReadFileBytes用
        /// </summary>
        /// <param name="pgno"></param>
        /// <returns></returns>
        public byte[] ReadFileBytes(int pgno)
        {
            string path = f.FullName;
            return ReadFileBytes(path, pgno);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void WritePage(Page p)
        {
            throw new NotImplementedException();
        }
 
        public List<Page> InsertTuple(TransactionId tid, Tuple t)
        {
            throw new NotImplementedException();
        }
        public List<Page> DeleteTuple(TransactionId tid, Tuple t)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 返回此DbFile中存储的所有元组的迭代器
        /// 迭代器必须使用BufferPool.GetPage()
        /// 不能使用ReadPage()来迭代Page
        /// </summary>
        /// <param name="tid"></param>
        /// <returns>返回此DbFile中存储的所有元组的迭代器</returns>
        public DbFileIterator Iterator(TransactionId tid)
        {
            HeapFileIterator iterator = new HeapFileIterator(tid,this);
            return iterator;
        }
        private class HeapFileIterator : DbFileIterator
        {
            private TransactionId tid;
            private HeapFile hf;
            private bool isSwitchPage = false;
            private int pgNo = 0;
            private IEnumerator<Tuple> tupleIterator;
            public HeapFileIterator(TransactionId tid,HeapFile hf)
            {
                this.tid = tid;
                this.hf = hf;
            }
            private HeapPage GetNextPage(int pgNo)
            {
                HeapPageId pid = new HeapPageId(hf.GetId(), pgNo);
                HeapPage hp = Database.GetBufferPool().
                    GetPage(tid, pid, Permissions.READ_ONLY) as HeapPage;
                //HeapPage hp = hf.ReadPage(pid) as HeapPage;
                return hp;
            }
            public void Open()
            {
                HeapPage hp = GetNextPage(pgNo);
                IEnumerable<Tuple> tuples = hp.Iterator();
                tupleIterator = tuples.GetEnumerator();
            }

            public bool HasNext()
            {
                if (tupleIterator == null)
                {
                    return false;
                }
                if (tupleIterator.MoveNext())
                {
                    return true;
                }
                else
                {
                    if (pgNo + 1 < hf.NumPages())
                    {
                        tupleIterator = null;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public Tuple Next()
            {
                Tuple oldTuple;
                if (tupleIterator != null)
                {
                    oldTuple = tupleIterator.Current;
                    return oldTuple;
                }
                else
                {
                    if (pgNo + 1 < hf.NumPages() && pgNo + 1 != 0)
                    {
                        pgNo++;
                        //Console.WriteLine("切换到第{0}页",pgNo + 1);
                        IEnumerable<Tuple> tuples = GetNextPage(pgNo).Iterator();
                        tupleIterator = tuples.GetEnumerator();
                        if (tupleIterator.MoveNext())
                        {
                            oldTuple = tupleIterator.Current;
                        }
                        else
                        {
                            Console.WriteLine("获取下一页的第一个元组失败");
                            oldTuple = null;
                        }                   
                        return oldTuple;
                    }
                    else
                    {
                        Console.WriteLine("读取页面时发生错误");
                        throw new NullReferenceException();
                    }
                }
            }

            public void Rewind()
            {
                Open();
            }
            public void Close()
            {
                pgNo = -1;
                tupleIterator = null;
            }
        }
    }
}