using MathArithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestMathArithmetic
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            exprs = new List<ExprAndValue>();
            String str = "";
            ExprAndValue expr;
            String[] strs;
            //打开测试用例文件
            using (StreamReader reader = new StreamReader("Exprs.txt"))
            {
                //每次读取文件中的一行
                while ((str = reader.ReadLine()) != null)
                {   //按逗号切分字串
                    strs = str.Split(new char[] { ',' });
                    expr.expr = strs[0];
                    expr.ExpectedValue = Convert.ToDouble(strs[1]);
                    //加入到用例集合中
                    exprs.Add(expr);
                }
            }
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            exprs.Clear();
        }
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
        public void CalculateTest()
        {
            //测试单个表达式
            InfixAlgorithm target = new InfixAlgorithm();
            string expr = "5.5+10.5*10";
            double expected = 110.5;
            double actual;
            actual = target.Calculate(expr);
            Assert.AreEqual(expected, actual, 1e-6);

        }

        #region "从文本文件中读取测试用例"
        [TestMethod()]
        public void CalculateTestFromTXTFile()
        {

            InfixAlgorithm target = new InfixAlgorithm();
            double actual = 0;
            foreach (ExprAndValue element in exprs)
            {
                actual = target.Calculate(element.expr);

                Assert.AreEqual(element.ExpectedValue, actual, 1e-6);

            }
        }
        //用于保存单个的测试用例
        private struct ExprAndValue
        {
            public string expr; 			//表达式
            public double ExpectedValue; //期望值
        }
        //测试用例的集合
        private static List<ExprAndValue> exprs;

        #endregion

        #region "从数据库中提取测试用例"

        [TestMethod()]
        [DataSource(
      "System.Data.SqlClient", @"Data Source=localhost\SQLEXPRESS;
		Initial Catalog=CalculateUnitTest;	Integrated Security=True",
      "TestCases", DataAccessMethod.Sequential)]
        public void CalculateTestFromDatabase()
        {
            InfixAlgorithm target = new InfixAlgorithm();
            double actual =
           target.Calculate(TestContext.DataRow["Expression"].ToString());
            double expected =
           Convert.ToDouble(TestContext.DataRow["ExpectedValue"]);
            Assert.AreEqual(expected, actual,1e-6);
        }

        #endregion
    }
}
