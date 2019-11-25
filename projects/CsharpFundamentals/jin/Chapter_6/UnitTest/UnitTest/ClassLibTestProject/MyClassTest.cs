using System;
using ClassLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibTestProject
{
    [TestClass]
    public class MyClassTest
    {
        [TestMethod]
        public void TestMethod()
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
            //实验
        }
    }
}
