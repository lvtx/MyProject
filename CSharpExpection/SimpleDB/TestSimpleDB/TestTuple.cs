using System;
using SimpleDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuple = SimpleDB.Tuple;
using System.Collections;
using System.Collections.Generic;

namespace TestSimpleDB
{
    [TestClass]
    public class TestTuple
    {
        Tuple tup = null;
        TupleDesc td = null;
        [TestInitialize]
        public void InitializeTuple()
        {
            td = Utility.GetTupleDesc(2);
            tup = new Tuple(td);
            tup.SetField(0, new IntField(-1));
            tup.SetField(1, new IntField(0));

        }
        /// <summary>
        /// 测试Tuple.getField()和Tuple.setField()
        /// </summary>
        [TestMethod]
        public void ModifyFields()
        {
            Assert.AreEqual(new IntField(-1), tup.GetField(0));
            Assert.AreEqual(new IntField(0), tup.GetField(1));
            tup.SetField(0, new IntField(1));
            tup.SetField(1, new IntField(37)); 
            Assert.AreEqual(new IntField(1), tup.GetField(0));
            Assert.AreEqual(new IntField(37), tup.GetField(1));
        }
        [TestMethod]
        public void TestGetTupleDesc()
        {
            TupleDesc td = Utility.GetTupleDesc(5);
            Tuple tup = new Tuple(td);
            Assert.AreEqual(td, tup.GetTupleDesc());
        }
        /// <summary>
        /// 测试Tuple.getRecordId()和Tuple.setRecordId()
        /// </summary>
        [TestMethod]
        public void ModifyRecordId()
        {
            Tuple tup1 = new Tuple(Utility.GetTupleDesc(1));
            HeapPageId pid1 = new HeapPageId(0, 0);
            RecordId rid1 = new RecordId(pid1, 0);
            tup1.SetRecordId(rid1);

            try
            {
                Assert.AreEqual(rid1, tup1.GetRecordId());
            }
            catch (Exception)
            {
                //rethrow the exception with an explanation
                throw new Exception("modifyRecordId() test failed due to " +
                        "RecordId.equals() not being implemented.  This is not required for Lab 1, " +
                        "but should pass when you do implement the RecordId class.");
            }
        }
        [TestMethod]
        public void TestFields()
        {
            IEnumerable<Field> fields = tup.Fields();
            foreach (var field in fields)
            {
                Console.WriteLine((field as IntField).GetValue());
            }
        }
    }
}
