using System;
using SimpleDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;

namespace TestSimpleDB
{
    [TestClass]
    public class TestTupleDesc
    {
        TupleDesc td;
        [TestInitialize]
        public void TestInitialize()
        {
            td = Utility.GetTupleDesc(3, "field");
        }
        [TestMethod]
        public void TestNumFields()
        {
            int[] lengths = new int[] { 1, 2, 1000 };
            foreach (var len in lengths)
            {
                TupleDesc td = Utility.GetTupleDesc(len);
                Assert.AreEqual(len, td.NumFields());
            }
        }
        [TestMethod]
        public void TestGetFieldName()
        {
            string fieldName = td.GetFieldName(2);
            Console.WriteLine("Field的名字为:{0}", fieldName);
        }
        [TestMethod]
        public void TestGetFieldType()
        {
            int[] lengths = new int[] { 1, 2, 1000 };
            foreach (var len in lengths)
            {
                TupleDesc td = Utility.GetTupleDesc(len);
                for (int i = 0; i < len; i++)
                {
                    Assert.AreEqual(typeof(INT_TYPE), td.GetFieldType(i));
                }
            }
        }
        [TestMethod]
        public void TestFieldNameToIndex()
        {
            int[] lengths = new int[] { 1, 2, 1000 };
            string prefix = "test";
            foreach (var len in lengths)
            {
                TupleDesc td = Utility.GetTupleDesc(len, prefix);
                for (int i = 0; i < len; i++)
                {
                    Assert.AreEqual(i, td.FieldNameToIndex(prefix + i));
                }
                try
                {
                    td.FieldNameToIndex("foo");
                    Assert.Fail("foo 不是一个有效的字段名");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("foo:{0}",ex.Message);
                }
                try
                {
                    td.FieldNameToIndex(null);
                    Assert.Fail("null 不是一个有效的字段名");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("null:{0}", ex.Message);
                }
                td = Utility.GetTupleDesc(len);
                try
                {
                    td.FieldNameToIndex(prefix);
                    Assert.Fail("没有此字段名所以查找失败");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}:{1}",prefix,ex.Message);
                }
            }
        }
        [TestMethod]
        public void TestGetSize()
        {
            int[] lengths = new int[] { 1, 2, 1000 };
            foreach (var len in lengths)
            {
                TupleDesc td = Utility.GetTupleDesc(len);
                Assert.AreEqual(len * sizeof(int), td.GetSize());
            }
        }
        [TestMethod]
        public void TestMerge()
        {
            TupleDesc td1 = Utility.GetTupleDesc(5, "field");
            TupleDesc td2 = Utility.GetTupleDesc(8, "field");
            TupleDesc td3 = TupleDesc.Merge(td1, td2);
            Assert.IsTrue(td3.NumFields() == 13);
        }
        [TestMethod]
        public void TestEquals()
        {
            TupleDesc singleInt = new TupleDesc(new Type[] { typeof(INT_TYPE) });
            TupleDesc singleInt2 = new TupleDesc(new Type[] { typeof(INT_TYPE) });
            TupleDesc intString = new TupleDesc(new Type[] { typeof(INT_TYPE),typeof(STRING_TYPE)});
            Assert.IsFalse(singleInt.Equals(null));
            Assert.IsFalse(singleInt.Equals(new object()));

            Assert.IsTrue(singleInt.Equals(singleInt));
            Assert.IsTrue(singleInt.Equals(singleInt2));
            Assert.IsTrue(singleInt2.Equals(singleInt));
            Assert.IsTrue(intString.Equals(intString));
        }
        [TestMethod]
        public void TestToString()
        {
            string desc = td.ToString();
            Console.WriteLine(desc);
        }
    }
}
