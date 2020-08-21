using System;
using SimpleDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSimpleDB.Systemtest
{
    /// <summary>
    /// 所有SimpleDb测试类的基类
    /// </summary>
    [TestClass]
    public class SimpleDBTestBase
    {
        /// <summary>
        /// 在运行每个测试之前重置数据库
        /// </summary>
        [TestMethod]
        public virtual void SetUp()
        {
            Database.Reset();
        }
    }
}
