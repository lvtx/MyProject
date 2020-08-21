using System;
using SimpleDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSimpleDB
{
    [TestClass]
    public class TestCatalog
    {
        private static String name = "test";
        private String nameThisTestRun;
        [TestInitialize]
        public void AddTables()
        {
            Database.GetCatalog().Clear();
            nameThisTestRun = System.Guid.NewGuid().ToString();
            Database.GetCatalog().AddTable(new SkeletonFile(-1, Utility.GetTupleDesc(2)), nameThisTestRun);
            Database.GetCatalog().AddTable(new SkeletonFile(-2, Utility.GetTupleDesc(2)), name);
        }
        [TestMethod]
        public void TestGetTupleDesc()
        {
            TupleDesc expected = Utility.GetTupleDesc(2);
            TupleDesc actual = Database.GetCatalog().GetTupleDesc(-1);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestGetTableId()
        {
            Assert.AreEqual(-2, Database.GetCatalog().GetTableId(name));
            Assert.AreEqual(-1, Database.GetCatalog().GetTableId(nameThisTestRun));
            try
            {
                Database.GetCatalog().GetTableId(null);
                Assert.Fail("Should not find table with null name");
            }
            catch (ArgumentNullException)
            {
                
            }
            try
            {
                Database.GetCatalog().GetTableId("foo");
                Assert.Fail("Should not find table with name foo");
            }
            catch (ArgumentNullException)
            {
            }
        }
        [TestMethod]
        public void GetDatabaseFile()
        {
            DbFile f = Database.GetCatalog().GetDatabaseFile(-1);
            Assert.AreEqual(-1, f.GetId());
        }
    }
}
