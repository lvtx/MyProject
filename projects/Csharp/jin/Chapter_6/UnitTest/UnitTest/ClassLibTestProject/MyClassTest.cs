using System;
using ClassLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibTestProject
{
    [TestClass]
    public class MyClassTest
    {
        /// <summary>
        /// 相等测试
        /// </summary>
        [TestMethod]
        public void DoubleValueTest()
        {
            //准备
            MyClass testMethod = new MyClass();
            //设定测试用例
            int value = 1;
            int expected = 2;
            //执行
            int actual = testMethod.DoubleValue(value);
            //断言
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// 异常测试
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SumExpectionTest()
        {
            MyClass target = new MyClass();
            target.Sum(10, 5);
        }
        /// <summary>
        /// 捕获异常
        /// </summary>
        [TestMethod]
        public void CalculateAgeTest()
        {
            MyClass targetObj = new MyClass();
            try
            {
                targetObj.CalculateAge(DateTime.Now.AddDays(1));
            }
            catch (ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, MyClass.AgeErrorMessage);
                return;
            }
            Assert.Fail("000");
        }
    }
}
