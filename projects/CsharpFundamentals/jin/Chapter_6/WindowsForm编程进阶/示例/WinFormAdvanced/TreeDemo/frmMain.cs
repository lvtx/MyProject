using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeDemo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnNewTopNode_Click(object sender, EventArgs e)
        {
            string NodeText = "";
            if (!string.IsNullOrEmpty(txtNewNodeText.Text.Trim()))
            {
                NodeText = txtNewNodeText.Text;
            }
            else
            {
                NodeText = "新根节点" + (treeView1.GetNodeCount(true) + 1);
            }
            treeView1.Nodes.Add(NodeText);
        }

        private void btnAddBrotherNode_Click(object sender, EventArgs e)
        {
            string NodeText = "";
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Parent!=null)
            {
                if (!string.IsNullOrEmpty(txtNewNodeText.Text.Trim()))
                {
                    NodeText = txtNewNodeText.Text;
                }
                else
                {
                    NodeText = "新兄弟节点" + (treeView1.GetNodeCount(true) + 1);
                }
                treeView1.SelectedNode.Parent.Nodes.Add(NodeText);
            }
        }

        private void btnAddChildNode_Click(object sender, EventArgs e)
        {
            string NodeText = "";
            if (treeView1.SelectedNode != null)
            {
                if (!string.IsNullOrEmpty(txtNewNodeText.Text.Trim()))
                {
                    NodeText = txtNewNodeText.Text;
                }
                else
                {
                    NodeText = "新子节点" + (treeView1.GetNodeCount(true) + 1);
                }
                treeView1.SelectedNode.Nodes.Add(NodeText);
                treeView1.SelectedNode.Expand();
            }
        }

        private void btnDeleteNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent != null)
                {
                    treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);
                }
                else
                {
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void btnClearTreeNodes_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
        }

        private void btnExpandCollapseNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.IsExpanded)
                {
                    treeView1.SelectedNode.Collapse(true);
                }
                else
                {
                    treeView1.SelectedNode.Expand();
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtNodeText.Text = e.Node.Text;
        }

        private void btnNodeRename_Click(object sender, EventArgs e)
        {
            if (txtNodeText.Text.Trim().Length > 0)
            {
                treeView1.SelectedNode.Text = txtNodeText.Text.Trim();
            }
        }
    }
}
