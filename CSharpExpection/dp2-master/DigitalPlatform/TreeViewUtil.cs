using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

/*
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
 */

namespace DigitalPlatform.GUI
{
    // TreeViewʵ�ú���
    public class TreeViewUtil
    {
        // ������б�checkboxѡ�еĽڵ�
        public static TreeNode[] GetCheckedNodes(TreeView tree)
        {
            ArrayList aNode = new ArrayList();

            int i = 0;
            for (i = 0; i < tree.Nodes.Count; i++)
            {
                GetCheckedNodes(tree.Nodes[i], ref aNode);
            }

            TreeNode[] result = new TreeNode[aNode.Count];
            for (i = 0; i < aNode.Count; i++)
            {
                result[i] = (TreeNode)aNode[i];
            }

            return result;
        }
        // ���һ���ڵ�����±�checkboxѡ�еĽڵ�
        static void GetCheckedNodes(TreeNode node,
            ref ArrayList aNode)
        {
            if (node.Checked == true)
                aNode.Add(node);

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                GetCheckedNodes(node.Nodes[i], ref aNode);
            }
        }

        // ����·��,����һ��node��checked״̬
        // TODO: !=null �������Ϊtrue
        public static TreeNode CheckNode(TreeView tree,
            string strPath,
            bool bChecked)
        {
            TreeNode node = TreeViewUtil.GetTreeNode(tree, strPath);
            if (node == null)
                return null;
            node.Checked = bChecked;
            return node;
        }

        // �����ĸ���ʼ������·����λ�ڵ㡣·������'/'�ָ�
        public static TreeNode GetTreeNode(TreeView treeView,
            string strPath)
        {
            string[] aName = strPath.Split(new Char[] { '/','\\' });    // 2007/8/2 changed ����ԭ������/�������Ƽ���\

            TreeNode node = null;
            TreeNode nodeThis = null;
            for (int i = 0; i < aName.Length; i++)
            {
                TreeNodeCollection nodes = null;

                if (node == null)
                    nodes = treeView.Nodes;
                else
                    nodes = node.Nodes;

                bool bFound = false;
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (aName[i] == nodes[j].Text)
                    {
                        bFound = true;
                        nodeThis = nodes[j];
                        break;
                    }
                }
                if (bFound == false)
                    return null;

                node = nodeThis;

            }

            return nodeThis;
        }

        // ������ĳ���ڵ㿪ʼ������·����λ�ڵ㡣·������'/'�ָ�
        // 2006/1/12
        public static TreeNode GetTreeNode(TreeNode start,
            string strPath)
        {
            if (start == null)
                throw new Exception("start��������Ϊnull");

            string[] aName = strPath.Split(new Char[] { '/', '\\' });   // 2007/8/2 changed ����ԭ������/�������Ƽ���\

            TreeNode node = start;
            TreeNode nodeThis = null;
            for (int i = 0; i < aName.Length; i++)
            {
                TreeNodeCollection nodes = null;
                nodes = node.Nodes;

                bool bFound = false;
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (aName[i] == nodes[j].Text)
                    {
                        bFound = true;
                        nodeThis = nodes[j];
                        break;
                    }
                }
                if (bFound == false)
                    return null;

                node = nodeThis;

            }

            return nodeThis;
        }

        // ����·��ѡ���ڵ㡣·������'/'�ָ�
        public static void SelectTreeNode(TreeView treeView,
            string strPath,
            char delimeter)
        {
            string[] aName = null;

#if DEBUG
            if (delimeter != (char)0)
            {
                Debug.Assert(treeView.PathSeparator == new string(delimeter, 1), "delimeter��ú�treeview��PathSeparatorһ��");
            }
#endif

            if (delimeter == (char)0)
                aName = strPath.Split(new Char[] { '/', '\\' });   // 2007/8/2 changed ����ԭ������/�������Ƽ���\
            else
                aName = strPath.Split(new Char[] { delimeter });   // 2007/8/2


            TreeNode node = null;
            TreeNode nodeThis = null;
            for (int i = 0; i < aName.Length; i++)
            {
                TreeNodeCollection nodes = null;

                if (node == null)
                    nodes = treeView.Nodes;
                else
                    nodes = node.Nodes;

                bool bFound = false;
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (aName[i] == nodes[j].Text)
                    {
                        bFound = true;
                        nodeThis = nodes[j];
                        break;
                    }
                }
                if (bFound == false)
                    break;

                node = nodeThis;

            }

            if (nodeThis != null && nodeThis.Parent != null)
                nodeThis.Parent.Expand();

            treeView.SelectedNode = nodeThis;
        }

        /*
        // ����ԭ���İ汾
        public static string GetPath(TreeNode node)
        {
            // Ϊ�˼���ԭ���İ汾
            return GetPath(node, '/');
        }*/


        // �õ�һ���ڵ��·��
        // Ϊ�˼���ԭ���İ汾����'/'�������Ƽ���'\'
        public static string GetPath(TreeNode node,
            char delimeter)
        {
            TreeNode nodeCur = node;
            string strPath = "";

            while (true)
            {
                if (nodeCur == null)
                    break;
                if (strPath != "")
                    strPath = nodeCur.Text + new string(delimeter, 1) + strPath;
                else
                    strPath = nodeCur.Text;

                nodeCur = nodeCur.Parent;
            }

            return strPath;
        }

        /* Ϊ��ʹ�޸Ĵ��룬���������ص�
        // ��õ�ǰ��ѡ�нڵ��·��
        public static string GetSelectedTreeNodePath(TreeView treeView)
        {
            return GetPath(treeView.SelectedNode);
        }*/

        // ��õ�ǰ��ѡ�нڵ��·��
        public static string GetSelectedTreeNodePath(TreeView treeView,
            char delimeter)
        {
            return GetPath(treeView.SelectedNode, delimeter);
        }


        // �ڽڵ��������Ѱ�����ض����ֵĶ���
        public static TreeNode FindNodeByText(TreeNode parent,
            string strText)
        {
            for (int i = 0; i < parent.Nodes.Count; i++)
            {
                TreeNode node = parent.Nodes[i];
                if (node.Text == strText)
                    return node;
            }

            return null;
        }

        // ���������������Ѱ�����ض����ֵĶ���
        public static TreeNode FindTopNodeByText(TreeView tree,
            string strText)
        {
            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                TreeNode node = tree.Nodes[i];
                if (node.Text == strText)
                    return node;
            }

            return null;
        }

        // �ڽڵ��������Ѱ�����ض����ֵĶ���
        public static TreeNode FindNodeByText(TreeView tree,
            TreeNode parent,
            string strText)
        {
            if (parent != null)
            {
                for (int i = 0; i < parent.Nodes.Count; i++)
                {
                    TreeNode node = parent.Nodes[i];
                    if (node.Text == strText)
                        return node;
                }

                return null;
            }
            else
            {
                if (tree == null)
                {
                    throw (new Exception("��parent����Ϊ�յ�ʱ��tree��������Ϊ��"));
                }

                for (int i = 0; i < tree.Nodes.Count; i++)
                {
                    TreeNode node = tree.Nodes[i];
                    if (node.Text == strText)
                        return node;
                }

                return null;
            }

        }
    }
}
