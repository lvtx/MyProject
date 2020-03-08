using MathArithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for InfixAlgorithmTest and is intended
    ///to contain all InfixAlgorithmTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InfixAlgorithmTest
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
        ///A test for Calculate
        ///</summary>
        [TestMethod()]
        [DataSource(
       "System.Data.SqlClient", @"Data Source=localhost\SQLEXPRESS;
		Initial Catalog=CalculateUnitTest;	Integrated Security=True",
       "TestCases", DataAccessMethod.Sequential)]

        public void CalculateTest()
        {
            InfixAlgorithm target = new InfixAlgorithm();
            double actual =
                target.Calculate(TestContext.DataRow["Expression"].ToString());
            double expected =
                Convert.ToDouble(TestContext.DataRow["ExpectedValue"]);
            Assert.AreEqual(expected, actual,
                  1e-6, "MathArithmetic.InfixAlgorithm.Calculate 未返回所需的值。");

        }
    }
}
