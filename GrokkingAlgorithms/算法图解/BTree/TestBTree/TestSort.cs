using System;
using TestBinaryTree;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryTreeProject;

namespace TestBinaryTree
{
    [TestClass]
    public class TestSort
    {
        [TestMethod]
        public void TestInsertSort()
        {
            List<int> items = new List<int>() { 3, 6, 5, 4, 4, 3, 2, 1, 6, 10, 9, 7, 5 };
            //List<int> items = new List<int>() { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var rets = Sort.SelectionSort(items);
            foreach (var item in items)
            {
                Console.Write("{0},",item);
            }
        }
        [TestMethod]
        public void TestPrintAddress()
        {
            List<int> items = new List<int>();
            Memory memory = new Memory();
            string address = memory.GetMemory(items);
            Console.WriteLine("old itmes address = {0}", address);
            GetAddress(items);
        }
        private void GetAddress(List<int> items)
        {
            Memory memory = new Memory();
            string address = memory.GetMemory(items);
            Console.WriteLine("new itmes address = {0}", address);
        }
    }
}
