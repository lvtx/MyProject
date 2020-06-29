using System;
using System.Collections.Generic;
using System.Text;

namespace TreeExample
{
    class TreeNode
    {
        public Object Data=null; //数据域
        public TreeNode Parent=null;//父节点
        public List<TreeNode> Childs=new List<TreeNode>(); //子节点集合
        
        public TreeNode(Object NodeData) //构造函数
        {
            Data = NodeData;
        }
    }
}
