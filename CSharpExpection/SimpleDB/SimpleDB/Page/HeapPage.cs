using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace SimpleDB
{
    /// <summary>
    /// 每个HeapPage实例都存储一页HeapFiles的数据，
    /// 并实现BufferPool使用的Page接口
    /// @see HeapFile
    /// @see BufferPool
    /// </summary>
    public class HeapPage : Page
    {
        private readonly HeapPageId pid;
        private readonly TupleDesc td;
        private readonly byte[] header;
        private readonly Tuple[] tuples;
        private readonly int numSlots;

        private byte[] oldData;
        private object oldDataLock = new object();

        /// <summary>
        /// 根据从磁盘读取的一组字节数据创建HeapPage
        /// HeapPage的格式是一组标头字节，指示正在使用的页面槽，一些元组槽
        /// 具体来说元组数量等于floor((BufferPool.GetPageSize()*8) / (tuplesize * 8 + 1))
        /// 其中元组大小是此数据库表中元组的大小，可以通过Catalog.GetTupleDesc确定
        /// 8位header words的数量等于 ceiling(no.tuple slots / 8)
        /// see Database.GetCatalog()
        /// see Catalog.GetTupleDesc()
        /// see BufferPool.GetPageSize()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public HeapPage(HeapPageId id, byte[] data)
        {
            this.pid = id;
            this.td = Database.GetCatalog().GetTupleDesc(id.GetTableId());
            this.numSlots = GetNumTuples();
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            //分配并读取此页面的页眉槽
            header = new byte[GetHeaderSize()];
            for (int i = 0; i < header.Length; i++)
            {
                header[i] = br.ReadByte();
            }
            tuples = new Tuple[numSlots];
            try
            {
                // allocate and read the actual records of this page
                for (int i = 0; i < tuples.Length; i++)
                {
                    tuples[i] = ReadNextTuple(br, i);
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            SetBeforeImage();
        }

        /// <summary>
        /// 在此页面上检索元组数
        /// </summary>
        /// <returns>返回此页面的元组数</returns>
        private int GetNumTuples()
        {
            int npagebytes = BufferPool.GetPageSize();
            int nrecbytes = td.GetSize();
            // 分配的插槽数为
            int numSlots = (npagebytes * 8) / (nrecbytes * 8 + 1);
            //int numSlots = 0;
            //// 实际有数据被使用的插槽数
            //for (int i = 0; i < numSlotsByAss; i++)
            //{
            //    if (IndexOfBit(header[i / 8],i % 8) == '1')
            //    {
            //        //Console.Write("{0},",numSlots);
            //        numSlots++;
            //    }
            //}
            return numSlots;
        }

        /// <summary>
        /// 计算HeapFile中每个元组占用tup​​leSize字节的页面的页眉中的字节数
        /// </summary>
        /// <returns>HeapFile中每个元组占用tup​​leSize字节的页面标头的字节数</returns>
        private int GetHeaderSize()
        {
            int nrecords = GetNumTuples();
            int nheaderbytes = (nrecords / 8);
            if (nheaderbytes * 8 < nrecords)
                nheaderbytes++;  //ceiling
            return nheaderbytes;
        }

        /// <summary>
        /// 在修改之前返回此页面的视图-由恢复使用
        /// </summary>
        /// <returns></returns>
        public Page GetBeforeImage()
        {
            try
            {
                byte[] oldDataRef = null;
                lock (oldDataLock)
                {
                    oldDataRef = oldData;
                }
                return new HeapPage(pid, oldDataRef);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
                //should never happen -- we parsed it OK before!
                Environment.Exit(1);
            }
            return null;
        }

        public void SetBeforeImage()
        {
            lock (oldDataLock)
            {
                oldData = GetPageData().Clone() as byte[];
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>返回与此页面关联的PageId</returns>
        public PageId GetId()
        {
            // some code goes here
            try
            {
                if (pid != null)
                {
                    return pid;
                }
                else
                {
                    Console.WriteLine("PageId为空");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("捕获HeapPage.GetId()异常{0}", ex);
                return null;
            }
        }

        /// <summary>
        /// 从源文件中提取元组
        /// </summary>
        /// <param name="br"></param>
        /// <param name="slotId"></param>
        /// <returns></returns>
        private Tuple ReadNextTuple(BinaryReader br, int slotId)
        {
            //if associated bit is not set, read forward to the next tuple, and
            // return null.
            if (!IsSlotUsed(slotId))
            {
                for (int i = 0; i < td.GetSize(); i++)
                {
                    try
                    {
                        br.ReadByte();
                    }
                    catch (IOException)
                    {
                        throw new ArgumentNullException("error reading empty tuple");
                    }
                }
                return null;
            }
            // read fields in the tuple
            Tuple t = new Tuple(td);
            RecordId rid = new RecordId(pid, slotId);
            t.SetRecordId(rid);
            try
            {
                for (int j = 0; j < td.NumFields(); j++)
                {
                    Field f = td.GetFieldType(j).Parse(br);
                    t.SetField(j, f);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new ArgumentNullException("parsing error!");
            }
            return t;
        }

        /// <summary>
        /// 生成表示此页内容的字节数组
        /// 用于将此页序列化到磁盘
        /// 这里的不变之处是应该可以将由getPageData生成的字节数组传递给HeapPage构造函数，
        /// 并让它生成相同的HeapPage对象
        /// </summary>
        /// <see cref="HeapPage"/>
        /// <returns>返回一个与该页字节对应的字节数组</returns>
        public byte[] GetPageData()
        {
            int len = BufferPool.GetPageSize();
            MemoryStream ms = new MemoryStream(len);
            BinaryWriter bw = new BinaryWriter(ms);

            // create the header of the page
            for (int i = 0; i < header.Length; i++)
            {
                try
                {
                    bw.Write(header[i]);
                }
                catch (IOException e)
                {
                    // this really shouldn't happen
                    Console.WriteLine(e.StackTrace);
                }
            }

            // create the tuples
            for (int i = 0; i < tuples.Length; i++)
            {
                // empty slot
                if (!IsSlotUsed(i))
                {
                    for (int j = 0; j < td.GetSize(); j++)
                    {
                        try
                        {
                            bw.Write(0);
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                    continue;
                }

                // non-empty slot
                for (int j = 0; j < td.NumFields(); j++)
                {
                    Field f = tuples[i].GetField(j);
                    try
                    {
                        f.Serialize(bw);
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            // padding
            int zerolen = BufferPool.GetPageSize() - (header.Length + td.GetSize() * tuples.Length); //- numSlots * td.getSize();
            byte[] zeroes = new byte[zerolen];
            try
            {
                bw.Write(zeroes, 0, zerolen);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            try
            {
                bw.Flush();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return ms.ToArray();
        }

        /**
        * Returns true if associated slot on this page is filled.
        */

        public bool IsSlotUsed(int i)
        {
            //hi header数组的索引
            try
            {
                int hi = i / 8;
                //位索引
                int index = i % 8;
                int bit = IndexOfBit(header[hi], index);
                if (bit == '1')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("HeapPage.IsSlotUsed捕获异常:{0}",e);
                return false;
            }
        }
        //查找二进制位
        //例如:查找0001001的第三位
        //从右向左
        public char IndexOfBit(int num, int index)
        {
            int bit = num & (1 << index);
            char ret = (char)((bit >> index) + 48);
            return ret;
        }
        /**
         * Static method to generate a byte array corresponding to an empty
         * HeapPage.
         * Used to add new, empty pages to the file. Passing the results of
         * this method to the HeapPage constructor will create a HeapPage with
         * no valid tuples in it.
         *
         * @return The returned ByteArray.
         */

        public static byte[] CreateEmptyPageData()
        {
            int len = BufferPool.GetPageSize();
            return new byte[len]; //all 0
        }

        /**
         * Delete the specified tuple from the page;  the tuple should be updated to reflect
         *   that it is no longer stored on any page.
         * @throws DbException if this tuple is not on this page, or tuple slot is
         *         already empty.
         * @param t The tuple to delete
         */

        public void DeleteTuple(Tuple t)
        {
            // some code goes here
            // not necessary for lab1
        }

        /**
         * Adds the specified tuple to the page;  the tuple should be updated to reflect
         *  that it is now stored on this page.
         * @throws DbException if the page is full (no empty slots) or tupledesc
         *         is mismatch.
         * @param t The tuple to add.
         */

        public void InsertTuple(Tuple t)
        {
            // some code goes here
            // not necessary for lab1
        }

        /**
         * Marks this page as dirty/not dirty and record that transaction
         * that did the dirtying
         */

        public void MarkDirty(bool dirty, TransactionId tid)
        {
            // some code goes here
            // not necessary for lab1
        }

        /**
         * Returns the tid of the transaction that last dirtied this page, or null if the page is not dirty
         */

        public TransactionId IsDirty()
        {
            // some code goes here
            // Not necessary for lab1
            return null;
        }

        /**
         * Returns the number of empty slots on this page.
         */

        public int GetNumEmptySlots()
        {
            int numEmptySlots = 0;
            for (int i = 0; i < tuples.Length; i++)
            {
                if (!IsSlotUsed(i))
                {
                    numEmptySlots++;
                }
            }
            return numEmptySlots;
        }

        /**
         * Abstraction to fill or clear a slot on this page.
         */

        private void MarkSlotUsed(int i, bool value)
        {
            // some code goes here
            // not necessary for lab1
        }
        /// <summary>
        /// 在这个迭代器上调用remove会抛出一个UnsupportedOperationException异常
        /// (注意这个迭代器不应该在空槽中返回元组!)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple> Iterator()
        {
            for (int i = 0; i < tuples.Length; i++)
            {
                if (tuples[i] != null)
                {
                    yield return tuples[i];
                }
            }
        }
    }
}