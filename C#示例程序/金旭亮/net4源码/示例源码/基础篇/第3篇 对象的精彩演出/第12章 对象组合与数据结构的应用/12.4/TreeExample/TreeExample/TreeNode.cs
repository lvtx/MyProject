using System;
using System.Collections.Generic;
using System.Text;

namespace TreeExample
{
    class TreeNode
    {
        public Object Data=null; //������
        public TreeNode Parent=null;//���ڵ�
        public List<TreeNode> Childs=new List<TreeNode>(); //�ӽڵ㼯��
        
        public TreeNode(Object NodeData) //���캯��
        {
            Data = NodeData;
        }
    }
}
