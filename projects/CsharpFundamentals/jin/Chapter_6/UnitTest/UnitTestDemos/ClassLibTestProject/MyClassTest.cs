using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLib;

namespace ClassLibTestProject
{
    [TestClass]
    public class MyClassTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void DoubleValueTest()
        {
            //Arrange:准备
            MyClass target = new MyClass();

            //设定测试用例
            int value = 1;
            int expected = 2;

            //Act:执行
            int actual = target.DoubleValue(value);

            //Assert:断言
            Assert.AreEqual(expected, actual);

        }
       
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SumExceptionTest()
        {
          
            MyClass target = new MyClass();
            target.Sum(100, 50);

        }

        [TestMethod]
        public void CalculateAgeOutOfRangeTest()
        {
            try
            {
                //Arrange:准备
                MyClass target = new MyClass();
                target.CaculateAge(DateTime.Now.AddDays(1));
            }
            catch (ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, MyClass.AgeErrorString);
                return;
            }
            Assert.Fail("No exception was thrown.");

        }
       
    }
}
