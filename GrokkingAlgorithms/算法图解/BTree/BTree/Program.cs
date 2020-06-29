using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeProject
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 静态二叉树
            //BiTree biTree1 = new BiTree();
            //BiTree biTree2 = new BiTree();
            //BiTree biTree3 = new BiTree();
            //BiTree biTree4 = new BiTree();
            //BiTree biTree5 = new BiTree();
            //biTree1.Element = 10;
            //biTree2.Element = 9;
            //biTree3.Element = 20;
            //biTree4.Element = 15;
            //biTree5.Element = 35;
            //biTree1.LeftBiTree = biTree2;
            //biTree1.RightBiTree = biTree3;
            //biTree2.LeftBiTree = null;
            //biTree2.RightBiTree = null;
            //biTree3.LeftBiTree = biTree4;
            //biTree3.RightBiTree = biTree5;
            //biTree4.LeftBiTree = null;
            //biTree4.RightBiTree = null;
            //biTree5.LeftBiTree = null;
            //biTree5.RightBiTree = null;
            //PreTraverseBTree(biTree1);
            #endregion
            //向二叉树中插入数据
            List<int> items = new List<int>() { 9, 4, 8, 3, 7, 2, 6, 1 };
            BTree bTree = new BTree(5);
            //foreach (var item in items)
            //{
            //    bTree.Put(item);
            //    //Console.WriteLine("{0},",item);
            //}
            bTree.Put(9);
            bTree.Put(10);
            //bTree.Put(6);
            //bTree.Put(7);
            bTree.PreTraverseBTree();
            Console.ReadLine();
        }

    }
}
