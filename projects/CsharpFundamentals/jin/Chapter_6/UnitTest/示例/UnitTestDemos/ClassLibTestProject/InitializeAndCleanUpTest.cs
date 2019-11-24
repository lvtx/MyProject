using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibTestProject
{
    [TestClass]
    public class InitializeAndCleanUpTest
    {


        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("Begin Test");
        }
        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine("Test finished");
        }

        [ClassInitialize]
        public static void MyClassInitialize(TestContext context)
        {
            Console.WriteLine("Class is Initialized");
        }
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            Console.WriteLine("Class clean up");
        }

        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("Running TestMethod1");
        }
        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine("Running TestMethod2");
        }
    }
}
