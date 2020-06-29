﻿namespace dp2Circulation
{
    partial class BiblioDupDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BiblioDupDialog));
            this.listView_browse = new System.Windows.Forms.ListView();
            this.columnHeader_path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_mergeTo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_createNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_skip = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_autoSelect = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_browse
            // 
            this.listView_browse.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_path});
            this.listView_browse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_browse.FullRowSelect = true;
            this.listView_browse.HideSelection = false;
            this.listView_browse.Location = new System.Drawing.Point(0, 0);
            this.listView_browse.Name = "listView_browse";
            this.listView_browse.Size = new System.Drawing.Size(553, 79);
            this.listView_browse.TabIndex = 0;
            this.listView_browse.UseCompatibleStateImageBehavior = false;
            this.listView_browse.View = System.Windows.Forms.View.Details;
            this.listView_browse.SelectedIndexChanged += new System.EventHandler(this.listView_browse_SelectedIndexChanged);
            // 
            // columnHeader_path
            // 
            this.columnHeader_path.Text = "记录路径";
            this.columnHeader_path.Width = 100;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(553, 207);
            this.webBrowser1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_mergeTo,
            this.toolStripButton_createNew,
            this.toolStripButton_skip,
            this.toolStripButton_stop,
            this.toolStripButton_autoSelect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 294);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(553, 28);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_mergeTo
            // 
            this.toolStripButton_mergeTo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_mergeTo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_mergeTo.Image")));
            this.toolStripButton_mergeTo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_mergeTo.Name = "toolStripButton_mergeTo";
            this.toolStripButton_mergeTo.Size = new System.Drawing.Size(79, 25);
            this.toolStripButton_mergeTo.Text = "合并到 ...";
            this.toolStripButton_mergeTo.Click += new System.EventHandler(this.toolStripButton_mergeTo_Click);
            // 
            // toolStripButton_createNew
            // 
            this.toolStripButton_createNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_createNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_createNew.Image")));
            this.toolStripButton_createNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_createNew.Name = "toolStripButton_createNew";
            this.toolStripButton_createNew.Size = new System.Drawing.Size(94, 25);
            this.toolStripButton_createNew.Text = "创建新记录";
            this.toolStripButton_createNew.Click += new System.EventHandler(this.toolStripButton_createNew_Click);
            // 
            // toolStripButton_skip
            // 
            this.toolStripButton_skip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_skip.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_skip.Image")));
            this.toolStripButton_skip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_skip.Name = "toolStripButton_skip";
            this.toolStripButton_skip.Size = new System.Drawing.Size(46, 25);
            this.toolStripButton_skip.Text = "跳过";
            this.toolStripButton_skip.Click += new System.EventHandler(this.toolStripButton_skip_Click);
            // 
            // toolStripButton_stop
            // 
            this.toolStripButton_stop.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_stop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_stop.Image")));
            this.toolStripButton_stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_stop.Name = "toolStripButton_stop";
            this.toolStripButton_stop.Size = new System.Drawing.Size(46, 25);
            this.toolStripButton_stop.Text = "停止";
            this.toolStripButton_stop.Click += new System.EventHandler(this.toolStripButton_stop_Click);
            // 
            // toolStripButton_autoSelect
            // 
            this.toolStripButton_autoSelect.CheckOnClick = true;
            this.toolStripButton_autoSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_autoSelect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_autoSelect.Image")));
            this.toolStripButton_autoSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_autoSelect.Name = "toolStripButton_autoSelect";
            this.toolStripButton_autoSelect.Size = new System.Drawing.Size(78, 25);
            this.toolStripButton_autoSelect.Text = "自动选择";
            this.toolStripButton_autoSelect.Click += new System.EventHandler(this.toolStripButton_autoSelect_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView_browse);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer1.Size = new System.Drawing.Size(553, 294);
            this.splitContainer1.SplitterDistance = 79;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 2;
            // 
            // BiblioDupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 322);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "BiblioDupDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "处理重复书目";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BiblioDupDialog_FormClosing);
            this.Load += new System.EventHandler(this.BiblioDupDialog_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_browse;
        private System.Windows.Forms.ColumnHeader columnHeader_path;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton toolStripButton_mergeTo;
        private System.Windows.Forms.ToolStripButton toolStripButton_createNew;
        private System.Windows.Forms.ToolStripButton toolStripButton_skip;
        private System.Windows.Forms.ToolStripButton toolStripButton_stop;
        private System.Windows.Forms.ToolStripButton toolStripButton_autoSelect;
    }
}