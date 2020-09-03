using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TestSimpleDB.Systemtest;

namespace TestSimpleDB
{
    [TestClass]
    public class TestHeapFileRead : SimpleDBTestBase
    {
        private HeapFile hf;
        private TransactionId tid;
        private TupleDesc td;

        /**
         * Set up initial resources for each unit test.
         */
        //[TestInitialize]
        public override void SetUp()
        {
            hf = SystemTestUtil.CreateRandomHeapFile(2, 20, null, null);
            td = Utility.GetTupleDesc(2);
            tid = new TransactionId();
        }

        [TestCleanup]
        public void TearDown()
        {
            Database.GetBufferPool().TransactionComplete(tid);
        }

        /**
         * Unit test for HeapFile.getId()
         */
        [TestMethod]
        public void GetId()
        {
            int id = hf.GetId();
            HeapFile other = null;
            // NOTE(ghuo): the value could be anything. test determinism, at least.
            Assert.AreEqual(id, hf.GetId());
            Assert.AreEqual(id, hf.GetId());
            other = SystemTestUtil.CreateRandomHeapFile(1, 1, null, null);
            Assert.IsTrue(id != other.GetId());
        }

        /**
         * Unit test for HeapFile.getTupleDesc()
         */
        [TestMethod]
        public void GetTupleDesc()
        {
            Assert.AreEqual(td, hf.GetTupleDesc());
        }
        /**
         * Unit test for HeapFile.numPages()
         */
        [TestMethod]
        public void NumPages()
        {
            Assert.AreEqual(1, hf.NumPages());
            // Assert.Equals(1, empty.numPages());
        }

        /**
         * Unit test for HeapFile.readPage()
         */
        [TestMethod]
        public void ReadPage()
        {
            HeapPageId pid = new HeapPageId(hf.GetId(), 0);
            HeapPage page = (HeapPage)hf.ReadPage(pid);

            // NOTE(ghuo): we try not to dig too deeply into the Page API here; we
            // rely on HeapPageTest for that. perform some basic checks.
            Assert.AreEqual(484, page.GetNumEmptySlots());
            Assert.IsTrue(page.IsSlotUsed(1));
            Assert.IsFalse(page.IsSlotUsed(20));
        }

        [TestMethod]
        public void TestIteratorBasic()
        {
            HeapFile smallFile = SystemTestUtil.
                CreateRandomHeapFile(2, 3, null, null);

            DbFileIterator it = smallFile.Iterator(tid);
            // Not open yet
            Assert.IsFalse(it.HasNext());
            try
            {
                it.Next();
                Assert.Fail("expected exception");
            }
            catch (NullReferenceException)
            {
            }

            it.Open();
            int count = 0;
            while (it.HasNext())
            {
                Assert.IsNotNull(it.Next());
                count += 1;
            }
            Assert.AreEqual(3, count);
            it.Close();
        }
        [TestMethod]
        public void TestIteratorClose()
        {
            // make more than 1 page. Previous closed iterator would start fetching
            // from page 1.
            HeapFile twoPageFile = SystemTestUtil.CreateRandomHeapFile(2, 520,
                null, null);

            DbFileIterator it = twoPageFile.Iterator(tid);
            it.Open();
            Assert.IsTrue(it.HasNext());
            it.Close();
            try
            {
                it.Next();
                Assert.Fail("expected exception");
            }
            catch (NullReferenceException)
            {
            }
            // close twice is harmless
            it.Close();
        }
        [TestMethod]
        public void TestReadPage()
        {
            //string path = @"D:\test\table.dat";
            HeapFile twoPageFile = SystemTestUtil.
                CreateRandomHeapFile(2, 520, null, null);
            //TupleDesc td = Utility.GetTupleDesc(2);
            //HeapFile twoPageFile = new HeapFile(new FileInfo(path), td);
            HeapPageId hid = new HeapPageId(twoPageFile.GetId(), 1);
            HeapPage hp = twoPageFile.ReadPage(hid) as HeapPage;
            IEnumerable<SimpleDB.Tuple> tuples = hp.Iterator();
            IEnumerator<SimpleDB.Tuple> tupleIterator = tuples.GetEnumerator();
            //tupleIterator.MoveNext();
            Assert.IsTrue(tupleIterator.MoveNext());
            while (tupleIterator.MoveNext())
            {
                SimpleDB.Tuple t = tupleIterator.Current;
                List<int> Eletuple = SystemTestUtil.TupleToList(t);
                foreach (var e in Eletuple)
                {
                    Console.Write(e + ",");
                }
                Console.WriteLine();
            }
        }
        [TestMethod]
        public void TestReadFileBytes()
        {
            string path = @"D:\test\table.dat";
            //HeapFile twoPageFile = SystemTestUtil.
            //    CreateRandomHeapFile(2, 520, null, null);
            TupleDesc td = Utility.GetTupleDesc(2);
            HeapFile twoPageFile = new HeapFile(new FileInfo(path), td);
            //HeapPageId hid = new HeapPageId(twoPageFile.GetId(), 1);
            //HeapPage hp = twoPageFile.ReadPage(hid) as HeapPage;
            int pgno = 1;
            int i = 1;
            byte[] pageBytes = twoPageFile.ReadFileBytes(pgno);
            foreach (var b in pageBytes)
            {
                Console.Write("{0}  ",Convert.ToString(b,16)); 
                if (i % 16 == 0)
                {
                    Console.WriteLine();
                }
                i++;
            }
        }
        //[TestMethod]
        //public void TestScan()
        //{
        //    string path = @"D:\test\table.dat";
        //    TupleDesc td = Utility.GetTupleDesc(1);
        //    HeapFile file = new HeapFile(new FileInfo(path), td);
        //    Database.GetCatalog().AddTable(file);
        //    SeqScan scan = new SeqScan(tid, file.GetId());
        //    int i = 1;
        //    try
        //    {
        //        scan.Open();
        //        while (scan.HasNext())
        //        {
        //            SimpleDB.Tuple t = scan.Next();
        //            List<int> tuple = SystemTestUtil.TupleToList(t);
        //            //Console.Write("{0}  ", i);
        //            foreach (var e in tuple)
        //            {
        //                //Console.Write(e + ",");
        //            }
        //            //Console.Write("-->");
        //            i++;
        //        }
        //        Console.WriteLine("遍历完{0}个元组未发现问题", --i);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        Console.WriteLine("第{0}行出现问题", i);
        //    }
        //}
    }
}





