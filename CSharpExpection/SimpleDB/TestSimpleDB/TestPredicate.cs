using System;
using SimpleDB;
using TestSimpleDB.Systemtest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSimpleDB
{
    [TestClass]
    public class PredicateTest : SimpleDBTestBase
    {
        [TestMethod]
        /// <summary>
        /// Unit test for Predicate.Filter()
        /// </summary>
        public void TestFilter()
        {
            int[] vals = new int[] { -1, 0, 1 };

            foreach (int i in vals)
            {
                Predicate p = new Predicate(0, Predicate.Op.EQUALS, TestUtil.GetField(i));
                Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i - 1)));
                Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i)));
                Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i + 1)));
            }

            foreach (int i in vals)
            {
                Predicate p = new Predicate(0, Predicate.Op.GREATER_THAN,
                    TestUtil.GetField(i));
                Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i - 1)));
                Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i)));
                Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i + 1)));
            }

            foreach (int i in vals)
            {
                Predicate p = new Predicate(0, Predicate.Op.GREATER_THAN_OR_EQ,
                    TestUtil.GetField(i));
                Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i - 1)));
                Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i)));
                Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i + 1)));
            }

            foreach (int i in vals)
            {
                Predicate p = new Predicate(0, Predicate.Op.LESS_THAN,
                    TestUtil.GetField(i));
                Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i - 1)));
                Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i)));
                Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i + 1)));
            }

            foreach (int i in vals)
            {
                Predicate p = new Predicate(0, Predicate.Op.LESS_THAN_OR_EQ,
                    TestUtil.GetField(i));
                Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i - 1)));
                Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i)));
                Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i + 1)));
            }
        }
    }
}
