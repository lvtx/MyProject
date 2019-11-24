using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    class TreeNode<T>
    {
        public TreeNode<T> LeftTreeNode = null;
        public TreeNode<T> RightTreeNode = null;
        public T value;
        public TreeNode(T value)
        {
            this.value = value;
        }
    }
}
