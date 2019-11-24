using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tree
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 当txtNewNodeText不为空
        /// 新节点名为txtNewNodeText中的文本
        /// 否则为新节点{0}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewTopNode_Click(object sender, EventArgs e)
        {
            string NodeName = null;
            if(!string.IsNullOrEmpty(txtNewNodeText.Text.Trim()))
            {
                NodeName = txtNewNodeText.Text;
            }
            else
            {
                NodeName = "新节点" + treeView1.GetNodeCount(true);
            }
            treeView1.Nodes.Add(NodeName);
        }

        private void btnAddBrotherNode_Click(object sender, EventArgs e)
        {
            string NodeName = null;
            if(treeView1.SelectedNode != null && treeView1.SelectedNode.Parent != null)
            {
                if (!string.IsNullOrEmpty(txtNewNodeText.Text.Trim()))
                {
                    NodeName = txtNewNodeText.Text;
                }
                else
                {
                    NodeName = "新兄弟节点" + treeView1.GetNodeCount(true);
                }
                treeView1.SelectedNode.Parent.Nodes.Add(NodeName);
            }        
        }

        private void btnAddChildNode_Click(object sender, EventArgs e)
        {
            string NodeName = null;
            if (treeView1.SelectedNode != null)
            {
                if (!string.IsNullOrEmpty(txtNewNodeText.Text.Trim()))
                {
                    NodeName = txtNewNodeText.Text;
                }
                else
                {
                    NodeName = "新节点" + treeView1.GetNodeCount(true);
                }
                treeView1.SelectedNode.Nodes.Add(NodeName);
                treeView1.SelectedNode.Expand();
            }
        }

        private void btnDeleteNode_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode.Remove();
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

        private void btnNodeRename_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNodeText.Text.Trim()))
            {
                treeView1.SelectedNode.Text = txtNodeText.Text;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                txtNodeText.Text = treeView1.SelectedNode.Text;
            }
        }
    }
}
