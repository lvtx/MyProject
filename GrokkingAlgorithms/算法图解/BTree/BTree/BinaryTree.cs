using System;
using System.Runtime.InteropServices;

namespace BinaryTreeProject
{
    public class Node
    {
        public int Element { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }
        public int Count { get; set; }
    }
    public class BTree
    {
        private Node root;
        private Memory memory = new Memory();
        public BTree(int element)
        {
            root = new Node();
            root.Element = element;
            root.LeftNode = null;
            root.RightNode = null;
        }
        public Node Create(int element)
        {
            Node node = new Node()
            {
                Element = element,
                LeftNode = null,
                RightNode = null
            };
            return node;
        }
        /// <summary>
        /// 向树种插入元素
        /// </summary>
        //public int count = 0;
        public void Put(int element)
        {
            //Console.WriteLine("root address {0}",
            //    memory.GetMemory(root));
            Put(root, element);
        }
        #region "Put()调试版"
        //int leftCount = 1;
        //int rightCount = 1;
        //private Node Put(Node root, int element)
        //{
        //    if (root == null)
        //    {
        //        if (leftCount == 2 || rightCount == 2)
        //        {
        //            Console.WriteLine("old node address {0}",
        //                memory.GetMemory(root));
        //        }
        //        root = Create(element);
        //        Console.WriteLine(root.Element);
        //        Console.WriteLine("new node address {0}",
        //            memory.GetMemory(root));
        //        return root;
        //    }
        //    if (element > root.Element)
        //    {
        //        Console.WriteLine("RightNode{0} address {1}",
        //            rightCount, memory.GetMemory(root.RightNode));
        //        rightCount++;
        //        root.RightNode = Put(root.RightNode, element);
        //    }
        //    else if (element < root.Element)
        //    {
        //        Console.WriteLine("LeftNode{0} address {1}",
        //            leftCount, memory.GetMemory(root.LeftNode));
        //        leftCount++;
        //        root.LeftNode = Put(root.LeftNode, element);
        //    }
        //    return null;
        //    //root.LeftNode = Create(4);
        //    //root.RightNode = Create(6);
        //}
        #endregion

        #region "Put()"
        private Node Put(Node root, int element)
        {
            if (root == null)
            {
                Node node = Create(element);
                return node;
            }
            if (element > root.Element)
            {
                root.RightNode = Put(root.RightNode, element);
            }
            else if (element < root.Element)
            {
                root.LeftNode = Put(root.LeftNode, element);
            }
            return root;
        }
        #endregion
        /// <summary>
        /// 先序遍历
        /// </summary>
        /// <param name="bTree">传入根节点</param>
        public void PreTraverseBTree()
        {
            PreTraverseBTree(root);
        }
        private void PreTraverseBTree(Node root)
        {
            if (root != null)
            {
                Console.Write("{0},", root.Element);
                PreTraverseBTree(root.LeftNode);
                PreTraverseBTree(root.RightNode);
            }
        }
    }
    public class Memory
    {
        // 获取引用类型的内存地址方法  
        public string GetMemory(object o)
        {
            GCHandle h = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);

            IntPtr addr = GCHandle.ToIntPtr(h);

            return "0x" + addr.ToString("X");
        }
    }
}
