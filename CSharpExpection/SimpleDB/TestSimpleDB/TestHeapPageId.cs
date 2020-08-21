using System;
using SimpleDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSimpleDB
{
    [TestClass]
    public class TestHeapPageId
    {
        private HeapPageId pid;
        [TestInitialize]
        public void CreatePid()
        {
            pid = new HeapPageId(1, 1);
        }
        [TestMethod]
        public void TestGetTableId()
        {
            Assert.AreEqual(1, pid.GetTableId());
        }
        [TestMethod]
        public void TestPageNumber()
        {
            Assert.AreEqual(1, pid.PageNumber());
        }
        [TestMethod]
        public void TestHashCode()
        {
            int code1, code2;

            // NOTE(ghuo): the HashCode could be anything. test determinism,
            // at least.
            pid = new HeapPageId(1, 1);
            code1 = pid.HashCode();
            Assert.AreEqual(code1, pid.HashCode());
            Assert.AreEqual(code1, pid.HashCode());

            pid = new HeapPageId(2, 2);
            code2 = pid.HashCode();
            Assert.AreEqual(code2, pid.HashCode());
            Assert.AreEqual(code2, pid.HashCode());
        }
        [TestMethod]
        public void TestEquals()
        {
            HeapPageId pid1 = new HeapPageId(1, 1);
            HeapPageId pid1Copy = new HeapPageId(1, 1);
            HeapPageId pid2 = new HeapPageId(2, 2);

            // .Equals() with null should return false
            Assert.IsFalse(pid1.Equals(null));

            // .Equals() with the wrong type should return false
            Assert.IsFalse(pid1.Equals(new Object()));

            Assert.IsTrue(pid1.Equals(pid1));
            Assert.IsTrue(pid1.Equals(pid1Copy));
            Assert.IsTrue(pid1Copy.Equals(pid1));
            Assert.IsTrue(pid2.Equals(pid2));

            Assert.IsFalse(pid1.Equals(pid2));
            Assert.IsFalse(pid1Copy.Equals(pid2));
            Assert.IsFalse(pid2.Equals(pid1));
            Assert.IsFalse(pid2.Equals(pid1Copy));
        }
    }
}
