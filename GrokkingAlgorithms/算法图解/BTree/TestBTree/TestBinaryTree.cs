using System;
using System.Collections.Generic;
using BinaryTreeProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestBinaryTree
{
    [TestClass]
    public class TestBinaryTree
    {
        [TestMethod]
        public void TestPut()
        {
            //向二叉树中插入数据
            List<int> items = new List<int>() { 9, 8, 7, 6, 4, 3, 2, 1 };
            BTree bTree = new BTree(5);
            foreach (var item in items)
            {
                bTree.Put(item);
                //Console.WriteLine("{0},",item);
            }
            bTree.Put(10);
            bTree.PreTraverseBTree();
        }
    }
}
