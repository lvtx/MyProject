using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ClassLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTestProject
{
    [TestClass]
    public class StringUtilsTest
    {
        [TestMethod]
        public void ReverseWordTest()
        {
            var OriginalString = "This is a string";
            var ActualResult = StringUtils.Reverse(OriginalString);
            Assert.AreEqual("string a is This", ActualResult);
        }
        [TestMethod]
        public void ReverseWordTest2()
        {
            var OriginalString = "This is    a string";
            var ActualResult = StringUtils.Reverse(OriginalString);
            Assert.AreEqual("string a is This", ActualResult);
        }
    }
}
