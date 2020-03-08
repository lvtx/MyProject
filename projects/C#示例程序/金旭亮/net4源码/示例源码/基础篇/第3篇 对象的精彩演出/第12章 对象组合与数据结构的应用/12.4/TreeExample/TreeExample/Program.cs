using System;
using System.Collections.Generic;
using System.Text;

namespace TreeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //建立多叉树
            TreeNode root=CreateTree();
            //深度遍历整棵树
            DeepVisitAllTree(root);

            Console.ReadKey();

        }

        //建立多叉树
        static TreeNode CreateTree()
        {
            //创建树节点
            TreeNode node0 = new TreeNode("Node0");
            TreeNode node1 = new TreeNode("Node1");
            TreeNode node2 = new TreeNode("Node2");
            TreeNode node3 = new TreeNode("Node3");
            TreeNode node4 = new TreeNode("Node4");
            TreeNode node5 = new TreeNode("Node5");

            //建立节点间联系
            //根节点有三个“孩子”
            node0.Childs.Add(node1);
            node0.Childs.Add(node2);
            node0.Childs.Add(node3);
            //指定三个“孩子”的父亲
            node1.Parent = node0;
            node2.Parent = node0;
            node3.Parent = node0;
            //node1还有两个“孩子”
            node1.Childs.Add(node4);
            node1.Childs.Add(node5);
            //指定两个“孙子”的父亲
            node4.Parent = node1;
            node5.Parent = node1;


             return node0;
        }

        //深度遍历整棵树
        static void DeepVisitAllTree(TreeNode root)
        {
            //当前节点没有子节点了，直接输出其数据
            if (root.Childs.Count == 0)
            {
                Console.WriteLine(root.Data.ToString());
                return;
            }
            //当前节点有子节点
            foreach (TreeNode node in root.Childs)
            {
                //对每个子节点递归调用此函数
                DeepVisitAllTree(node);
            }
            //输出当前节点的数据
            Console.WriteLine(root.Data.ToString());
            return;
        }

 

    }
}
