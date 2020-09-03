using System;
using SimpleDB;
using TestSimpleDB.Systemtest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSimpleDB
{
    /// <summary>
    /// Unit test for JoinPredicate.Filter()
    /// </summary>
    [TestClass]
    public class TestJoinPredicate:SimpleDBTestBase
    {
        [TestMethod]
		public virtual void FilterVaryingVals()
		{
			int[] vals = new int[] { -1, 0, 1 };

			foreach (int i in vals)
			{
				JoinPredicate p = new JoinPredicate(0, Predicate.Op.EQUALS, 0);
				Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i - 1)));
				Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i)));
				Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i + 1)));
			}

			foreach (int i in vals)
			{
				JoinPredicate p = new JoinPredicate(0, Predicate.Op.GREATER_THAN, 0);
				Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i - 1)));
				Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i)));
				Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i + 1)));
			}

			foreach (int i in vals)
			{
				JoinPredicate p = new JoinPredicate(0, Predicate.Op.GREATER_THAN_OR_EQ, 0);
				Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i - 1)));
				Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i)));
				Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i + 1)));
			}

			foreach (int i in vals)
			{
				JoinPredicate p = new JoinPredicate(0, Predicate.Op.LESS_THAN, 0);
				Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i - 1)));
				Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i)));
				Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i + 1)));
			}

			foreach (int i in vals)
			{
				JoinPredicate p = new JoinPredicate(0, Predicate.Op.LESS_THAN_OR_EQ, 0);
				Assert.IsFalse(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i - 1)));
				Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i)));
				Assert.IsTrue(p.Filter(Utility.GetHeapTuple(i), Utility.GetHeapTuple(i + 1)));
			}
		}
	}
}
