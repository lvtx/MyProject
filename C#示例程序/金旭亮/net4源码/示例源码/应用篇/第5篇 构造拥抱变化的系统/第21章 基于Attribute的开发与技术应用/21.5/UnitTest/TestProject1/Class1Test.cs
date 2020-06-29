using ClassLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for Class1Test and is intended
    ///to contain all Class1Test Unit Tests
    ///</summary>
    [TestClass()]
    public class Class1Test
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for DoubleValue
        ///</summary>
        [TestMethod()]
        public void DoubleValueTest()
        {
            Class1 target = new Class1(); // TODO: Initialize to an appropriate value
            int i = 1; // TODO: Initialize to an appropriate value
            int expected = 2; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.DoubleValue(i);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ToUpper
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ClassLib.dll")]
        public void ToUpperTest()
        {
            Class1_Accessor target = new Class1_Accessor();
            string str = "abcd";
            string expected = "ABCD";
            string actual;
            actual = target.ToUpper(str);
            Assert.AreEqual(expected, actual);
        }
    }
}
