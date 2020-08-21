using System;
using SimpleDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSimpleDB
{
    [TestClass]
    public class TestRecordId
    {
        private static RecordId hrid;
        private static RecordId hrid2;
        private static RecordId hrid3;
        private static RecordId hrid4;
        [TestInitialize]
        public void CreatePids()
        {
            HeapPageId hpid = new HeapPageId(-1, 2);
            HeapPageId hpid2 = new HeapPageId(-1, 2);
            HeapPageId hpid3 = new HeapPageId(-2, 2);
            hrid = new RecordId(hpid, 3);
            hrid2 = new RecordId(hpid2, 3);
            hrid3 = new RecordId(hpid, 4);
            hrid4 = new RecordId(hpid3, 3);
        }
        [TestMethod]
        public void TestGetPageId()
        {
            HeapPageId hpid = new HeapPageId(-1, 2);
            Assert.AreEqual(hpid, hrid.GetPageId());
        }
        [TestMethod]
        public void TestTupleno()
        {
            Assert.AreEqual(3, hrid.Tupleno());
        }
        [TestMethod]
        public void TestEquals()
        {
            Assert.AreEqual(hrid, hrid2);
            Assert.AreEqual(hrid2, hrid);
            Assert.IsFalse(hrid.Equals(hrid3));
            Assert.IsFalse(hrid3.Equals(hrid));
            Assert.IsFalse(hrid2.Equals(hrid4));
            Assert.IsFalse(hrid4.Equals(hrid2));
        }
        [TestMethod]
        public void TestHashCode()
        {
            Assert.AreEqual(hrid.HashCode(), hrid2.HashCode());
        }
    }
}
