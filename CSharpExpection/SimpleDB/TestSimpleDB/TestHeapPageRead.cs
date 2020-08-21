using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleDB;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestSimpleDB
{
    [TestClass]
    public class TestHeapPageRead
    {
        private HeapPageId pid;

        public static readonly int[][] EXAMPLE_VALUES = new int[][]
        {
            new int[] { 31933, 862 },
            new int[] { 29402, 56883 },
            new int[] { 1468, 5825 },
            new int[] { 17876, 52278 },
            new int[] { 6350, 36090 },
            new int[] { 34784, 43771 },
            new int[] { 28617, 56874 },
            new int[] { 19209, 23253 },
            new int[] { 56462, 24979 },
            new int[] { 51440, 56685 },
            new int[] { 3596, 62307 },
            new int[] { 45569, 2719 },
            new int[] { 22064, 43575 },
            new int[] { 42812, 44947 },
            new int[] { 22189, 19724 },
            new int[] { 33549, 36554 },
            new int[] { 9086, 53184 },
            new int[] { 42878, 33394 },
            new int[] { 62778, 21122 },
            new int[] { 17197, 16388 }
        };
        public static readonly byte[] EXAMPLE_DATA;
        static TestHeapPageRead()
        {
            List<List<int>> table = new List<List<int>>();
            foreach (int[] tuple in EXAMPLE_VALUES)
            {
                List<int> lstTuple = new List<int>();
                foreach (int value in tuple)
                {
                    lstTuple.Add(value);
                }
                table.Add(lstTuple);
            }
            try
            {
                FileInfo temp = new FileInfo(@"D:\test\table.dat");
                HeapFileEncoder.Convert(table, temp, BufferPool.GetPageSize(), 2);
                EXAMPLE_DATA = TestUtil.ReadFileBytes(temp.FullName);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }
        [TestInitialize]
        public void AddTable()
        {
            this.pid = new HeapPageId(-1, -1);
            Database.GetCatalog().AddTable(new SkeletonFile(-1, Utility.GetTupleDesc(2)),
                Guid.NewGuid().ToString());
        }
        [TestMethod]
        public void TestGetId()
        {
            HeapPage page = new HeapPage(pid, EXAMPLE_DATA);
            Assert.AreEqual(pid, page.GetId());
        }
        [TestMethod]
        public void TestIterator()
        {
            HeapPage page = new HeapPage(pid, EXAMPLE_DATA);
            IEnumerable<SimpleDB.Tuple> iterable = page.Iterator();
            IEnumerator<SimpleDB.Tuple> iterator = iterable.GetEnumerator();
            int row = 0;
            while (iterator.MoveNext())
            {
                SimpleDB.Tuple tup = iterator.Current;
                IntField f0 = (IntField)tup.GetField(0);
                IntField f1 = (IntField)tup.GetField(1);

                Assert.AreEqual(EXAMPLE_VALUES[row][0], f0.GetValue());
                Assert.AreEqual(EXAMPLE_VALUES[row][1], f1.GetValue());
                row++;
            }
        }
        [TestMethod]
        public void GetNumEmptySlots()
        {
            HeapPage page = new HeapPage(pid, EXAMPLE_DATA);
            Assert.AreEqual(484, page.GetNumEmptySlots());
        }
        [TestMethod]
        public void GetSlot()
        {
            HeapPage page = new HeapPage(pid, EXAMPLE_DATA);

        for (int i = 0; i< 20; ++i)
            Assert.IsTrue(page.IsSlotUsed(i));

        for (int i = 20; i< 504; ++i)
            Assert.IsFalse(page.IsSlotUsed(i));
    }
}
}