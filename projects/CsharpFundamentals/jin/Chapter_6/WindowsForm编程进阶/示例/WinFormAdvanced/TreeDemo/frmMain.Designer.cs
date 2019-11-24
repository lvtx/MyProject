namespace TreeDemo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnAddChildNode = new System.Windows.Forms.Button();
            this.btnNewTopNode = new System.Windows.Forms.Button();
            this.btnAddBrotherNode = new System.Windows.Forms.Button();
            this.btnDeleteNode = new System.Windows.Forms.Button();
            this.btnExpandCollapseNode = new System.Windows.Forms.Button();
            this.btnClearTreeNodes = new System.Windows.Forms.Button();
            this.btnNodeRename = new System.Windows.Forms.Button();
            this.txtNodeText = new System.Windows.Forms.TextBox();
            this.txtNewNodeText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(24, 32);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(360, 412);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // btnAddChildNode
            // 
            this.btnAddChildNode.Location = new System.Drawing.Point(557, 136);
            this.btnAddChildNode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddChildNode.Name = "btnAddChildNode";
            this.btnAddChildNode.Size = new System.Drawing.Size(136, 46);
            this.btnAddChildNode.TabIndex = 1;
            this.btnAddChildNode.Text = "新加子节点";
            this.btnAddChildNode.UseVisualStyleBackColor = true;
            this.btnAddChildNode.Click += new System.EventHandler(this.btnAddChildNode_Click);
            // 
            // btnNewTopNode
            // 
            this.btnNewTopNode.Location = new System.Drawing.Point(413, 82);
            this.btnNewTopNode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNewTopNode.Name = "btnNewTopNode";
            this.btnNewTopNode.Size = new System.Drawing.Size(280, 46);
            this.btnNewTopNode.TabIndex = 1;
            this.btnNewTopNode.Text = "新加顶层节点";
            this.btnNewTopNode.UseVisualStyleBackColor = true;
            this.btnNewTopNode.Click += new System.EventHandler(this.btnNewTopNode_Click);
            // 
            // btnAddBrotherNode
            // 
            this.btnAddBrotherNode.Location = new System.Drawing.Point(413, 136);
            this.btnAddBrotherNode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddBrotherNode.Name = "btnAddBrotherNode";
            this.btnAddBrotherNode.Size = new System.Drawing.Size(136, 46);
            this.btnAddBrotherNode.TabIndex = 1;
            this.btnAddBrotherNode.Text = "新加同级节点";
            this.btnAddBrotherNode.UseVisualStyleBackColor = true;
            this.btnAddBrotherNode.Click += new System.EventHandler(this.btnAddBrotherNode_Click);
            // 
            // btnDeleteNode
            // 
            this.btnDeleteNode.Location = new System.Drawing.Point(413, 212);
            this.btnDeleteNode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteNode.Name = "btnDeleteNode";
            this.btnDeleteNode.Size = new System.Drawing.Size(136, 46);
            this.btnDeleteNode.TabIndex = 1;
            this.btnDeleteNode.Text = "删除当前节点";
            this.btnDeleteNode.UseVisualStyleBackColor = true;
            this.btnDeleteNode.Click += new System.EventHandler(this.btnDeleteNode_Click);
            // 
            // btnExpandCollapseNode
            // 
            this.btnExpandCollapseNode.Location = new System.Drawing.Point(413, 282);
            this.btnExpandCollapseNode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExpandCollapseNode.Name = "btnExpandCollapseNode";
            this.btnExpandCollapseNode.Size = new System.Drawing.Size(280, 46);
            this.btnExpandCollapseNode.TabIndex = 1;
            this.btnExpandCollapseNode.Text = "展开/折叠当前节点";
            this.btnExpandCollapseNode.UseVisualStyleBackColor = true;
            this.btnExpandCollapseNode.Click += new System.EventHandler(this.btnExpandCollapseNode_Click);
            // 
            // btnClearTreeNodes
            // 
            this.btnClearTreeNodes.Location = new System.Drawing.Point(557, 212);
            this.btnClearTreeNodes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClearTreeNodes.Name = "btnClearTreeNodes";
            this.btnClearTreeNodes.Size = new System.Drawing.Size(136, 46);
            this.btnClearTreeNodes.TabIndex = 1;
            this.btnClearTreeNodes.Text = "清除所有节点";
            this.btnClearTreeNodes.UseVisualStyleBackColor = true;
            this.btnClearTreeNodes.Click += new System.EventHandler(this.btnClearTreeNodes_Click);
            // 
            // btnNodeRename
            // 
            this.btnNodeRename.Location = new System.Drawing.Point(553, 399);
            this.btnNodeRename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNodeRename.Name = "btnNodeRename";
            this.btnNodeRename.Size = new System.Drawing.Size(136, 46);
            this.btnNodeRename.TabIndex = 1;
            this.btnNodeRename.Text = "节点改名";
            this.btnNodeRename.UseVisualStyleBackColor = true;
            this.btnNodeRename.Click += new System.EventHandler(this.btnNodeRename_Click);
            // 
            // txtNodeText
            // 
            this.txtNodeText.Location = new System.Drawing.Point(417, 365);
            this.txtNodeText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNodeText.Name = "txtNodeText";
            this.txtNodeText.Size = new System.Drawing.Size(275, 25);
            this.txtNodeText.TabIndex = 2;
            // 
            // txtNewNodeText
            // 
            this.txtNewNodeText.Location = new System.Drawing.Point(413, 32);
            this.txtNewNodeText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNewNodeText.Name = "txtNewNodeText";
            this.txtNewNodeText.Size = new System.Drawing.Size(275, 25);
            this.txtNewNodeText.TabIndex = 2;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 466);
            this.Controls.Add(this.txtNewNodeText);
            this.Controls.Add(this.txtNodeText);
            this.Controls.Add(this.btnNewTopNode);
            this.Controls.Add(this.btnExpandCollapseNode);
            this.Controls.Add(this.btnClearTreeNodes);
            this.Controls.Add(this.btnNodeRename);
            this.Controls.Add(this.btnDeleteNode);
            this.Controls.Add(this.btnAddBrotherNode);
            this.Controls.Add(this.btnAddChildNode);
            this.Controls.Add(this.treeView1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmMain";
            this.Text = "树控件的使用";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnAddChildNode;
        private System.Windows.Forms.Button btnNewTopNode;
        private System.Windows.Forms.Button btnAddBrotherNode;
        private System.Windows.Forms.Button btnDeleteNode;
        private System.Windows.Forms.Button btnExpandCollapseNode;
        private System.Windows.Forms.Button btnClearTreeNodes;
        private System.Windows.Forms.Button btnNodeRename;
        private System.Windows.Forms.TextBox txtNodeText;
        private System.Windows.Forms.TextBox txtNewNodeText;
    }
}

