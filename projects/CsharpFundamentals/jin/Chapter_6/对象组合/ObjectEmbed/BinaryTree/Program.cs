using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    class Program
    {
        static void Main(string[] args)
        {
            TreeNode<string> rootNode = new TreeNode<string>("A");
            TreeNode<string> treeNode1 = new TreeNode<string>("B");
            TreeNode<string> treeNode2 = new TreeNode<string>("C");
            TreeNode<string> treeNode3 = new TreeNode<string>("D");
            TreeNode<string> treeNode4 = new TreeNode<string>("E");
            rootNode.LeftTreeNode = treeNode1;
            rootNode.RightTreeNode = treeNode2;
            treeNode2.LeftTreeNode = treeNode3;
            treeNode2.RightTreeNode = treeNode4;
            preTraverseBTree(rootNode);
            Console.ReadLine();
        }
        static void preTraverseBTree(TreeNode<string> node)
        {
            if(node != null)
            {
                Console.Write(node.value);
                preTraverseBTree(node.LeftTreeNode);
                preTraverseBTree(node.RightTreeNode);
            }
        }    
    }
}
