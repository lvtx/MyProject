using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TestSimpleDB.Systemtest;

namespace TestSimpleDB
{
    /// <summary>
    ///
    /// Dumps the contents of a table.
    /// args[1] is the number of columns.E.g., if it's 5, then ScanTest will end
    /// up dumping the contents of f4.0.txt.
    /// </summary>
    [TestClass]
    public class TestScan
    {
        private readonly static Random r = new Random();

        // Tests the scan operator for a table with the specified dimensions.
        private void ValidateScan(int[] columnSizes, int[] rowSizes)
        {
            int row = 0;
            int column = 0;
            try
            {
                foreach (int columns in columnSizes)
                {
                    foreach (int rows in rowSizes)
                    {
                        row = rows;
                        column = columns;
                        List<List<int>> tuples = new List<List<int>>();
                        HeapFile f = SystemTestUtil.CreateRandomHeapFile(columns, rows, null, tuples);
                        //Console.WriteLine("row:{0} column:{1}", rows, columns);
                        SystemTestUtil.MatchTuples(f, tuples);
                        Database.ResetBufferPool(BufferPool.DEFAULT_PAGES);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("bad row:{0} bad columns {1}", row, column);
                Console.WriteLine(e.Message);
            }
        }

        /** Scan 1-4 columns. */
        [TestMethod]
        public void TestSmall()
        {
            int[] columnSizes = new int[] { 1, 2, 3, 4 };
            int[] rowSizes =
                    new int[] { 1, 2, 511, 512, 513, 1023, 1024, 1025, 4096 + r.Next(4096) };
            //int[] columnSizes = new int[] { 1, 2};
            //int[] rowSizes =
            //        new int[] { 1, 2, 511, 512, 513, 1023, 1024, 1025, };
            ValidateScan(columnSizes, rowSizes);
            //HeapFile f = SystemTestUtil.CreateRandomHeapFile(4, 4096 + r.Next(4096), null, null);
        }

        /// <summary>
        /// 测试SeqScan迭代器的重置功能是否正常
        /// </summary>
        [TestMethod]
        public void TestRewind()
        {
            List<List<int>> tuples = new List<List<int>>();
            HeapFile f = SystemTestUtil.CreateRandomHeapFile(2, 1000, null, tuples);
            TransactionId tid = new TransactionId();
            SeqScan scan = new SeqScan(tid, f.GetId(), "table");
            scan.Open();
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(scan.HasNext());
                SimpleDB.Tuple t = scan.Next();
                CollectionAssert.AreEqual(tuples[i], SystemTestUtil.TupleToList(t));
            }
            //int i = 1;
            //try
            //{       
            //    while (scan.HasNext())
            //    {
            //        SimpleDB.Tuple t = scan.Next();
            //        List<int> tuple = SystemTestUtil.TupleToList(t);
            //        //Console.Write("{0}  ", i);
            //        foreach (var e in tuple)
            //        {
            //            //Console.Write(e + ",");
            //        }
            //        //Console.Write("-->");
            //        i++;
            //    }
            //Console.WriteLine("遍历完{0}个元组未发现问题",--i);
            //}
            //catch (NullReferenceException)
            //{
            //    Console.WriteLine("第{0}行出现问题",i);
            //}

            scan.Rewind();
            for (int i = 0; i < 100; ++i)
            {
                Assert.IsTrue(scan.HasNext());
                SimpleDB.Tuple t = scan.Next();
                CollectionAssert.AreEqual(tuples[i], SystemTestUtil.TupleToList(t));
            }
            scan.Close();
            Database.GetBufferPool().TransactionComplete(tid);
        }

        /// <summary>
        /// 验证缓冲池是否在缓存数据
        /// @throws TransactionAbortedException
        /// @throws DbException
        /// </summary>

        [TestMethod]
        public void TestCache()
        {
            // 统计ReadPage操作的数量
            // Create the table
            const int PAGES = 30;
            List<List<int>> tuples = new List<List<int>>();
            FileInfo f = SystemTestUtil.
                CreateRandomHeapFileUnopened(1, 992 * PAGES, 1000, null, tuples);
            TupleDesc td = Utility.GetTupleDesc(1);
            //string path = @"D:\test\table.dat";
            //TupleDesc td = Utility.GetTupleDesc(1);
            //FileInfo f = new FileInfo(path);
            InstrumentedHeapFile table = new InstrumentedHeapFile(f, td);
            Database.GetCatalog().AddTable(table, SystemTestUtil.GetUUID());

            // Scan the table once
            SystemTestUtil.MatchTuples(table, tuples);
            Assert.AreEqual(PAGES, table.ReadCount);
            table.ReadCount = 0;
            // Scan the table again: all pages should be cached
            SystemTestUtil.MatchTuples(table, tuples);
            Assert.AreEqual(0, table.ReadCount);
        }
        class InstrumentedHeapFile : HeapFile
        {
            public InstrumentedHeapFile(FileInfo f,TupleDesc td)
                : base(f,td){}

            public override Page ReadPage(PageId pid)
            {
                readCount += 1;
                return base.ReadPage(pid);
            }

            private int readCount;
            public int ReadCount
            {
                get { return readCount; }
                set { readCount = value; }
            }

        }
    }
}

