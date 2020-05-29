﻿namespace dp2Circulation
{
    partial class BatchOrderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchOrderForm));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton_newOrder = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolStripMenuItem_newOrderTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton_select = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItem_selectAllBiblio = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_selectAllBiblio_hasOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_selectAllBiblio_noOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_removeSelectedBiblio = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_selectAllOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton_change = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItem_quickChange = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_loadBiblio = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_refresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_deleteOrder = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_save = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_orderList = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_rate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_test = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 25);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(512, 280);
            this.webBrowser1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton_newOrder,
            this.toolStripDropDownButton_select,
            this.toolStripDropDownButton_change,
            this.toolStripSeparator1,
            this.toolStripButton_loadBiblio,
            this.toolStripButton_refresh,
            this.toolStripButton_deleteOrder,
            this.toolStripButton_save,
            this.toolStripButton_orderList,
            this.toolStripButton_rate,
            this.toolStripButton_test});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(512, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSplitButton_newOrder
            // 
            this.toolStripSplitButton_newOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton_newOrder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_newOrderTemplate});
            this.toolStripSplitButton_newOrder.Enabled = false;
            this.toolStripSplitButton_newOrder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton_newOrder.Image")));
            this.toolStripSplitButton_newOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton_newOrder.Name = "toolStripSplitButton_newOrder";
            this.toolStripSplitButton_newOrder.Size = new System.Drawing.Size(60, 22);
            this.toolStripSplitButton_newOrder.Text = "新订购";
            this.toolStripSplitButton_newOrder.ButtonClick += new System.EventHandler(this.toolStripSplitButton_newOrder_ButtonClick);
            // 
            // ToolStripMenuItem_newOrderTemplate
            // 
            this.ToolStripMenuItem_newOrderTemplate.Enabled = false;
            this.ToolStripMenuItem_newOrderTemplate.Name = "ToolStripMenuItem_newOrderTemplate";
            this.ToolStripMenuItem_newOrderTemplate.Size = new System.Drawing.Size(161, 22);
            this.ToolStripMenuItem_newOrderTemplate.Text = "新订购 (模板) ...";
            this.ToolStripMenuItem_newOrderTemplate.Click += new System.EventHandler(this.ToolStripMenuItem_newOrderTemplate_Click);
            // 
            // toolStripDropDownButton_select
            // 
            this.toolStripDropDownButton_select.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton_select.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_selectAllBiblio,
            this.toolStripMenuItem_selectAllBiblio_hasOrders,
            this.toolStripMenuItem_selectAllBiblio_noOrder,
            this.toolStripMenuItem_removeSelectedBiblio,
            this.toolStripSeparator2,
            this.ToolStripMenuItem_selectAllOrder});
            this.toolStripDropDownButton_select.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton_select.Image")));
            this.toolStripDropDownButton_select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton_select.Name = "toolStripDropDownButton_select";
            this.toolStripDropDownButton_select.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton_select.Text = "选择";
            // 
            // ToolStripMenuItem_selectAllBiblio
            // 
            this.ToolStripMenuItem_selectAllBiblio.Name = "ToolStripMenuItem_selectAllBiblio";
            this.ToolStripMenuItem_selectAllBiblio.Size = new System.Drawing.Size(196, 22);
            this.ToolStripMenuItem_selectAllBiblio.Text = "所有书目";
            this.ToolStripMenuItem_selectAllBiblio.Click += new System.EventHandler(this.ToolStripMenuItem_selectAllBiblio_Click);
            // 
            // toolStripMenuItem_selectAllBiblio_hasOrders
            // 
            this.toolStripMenuItem_selectAllBiblio_hasOrders.Name = "toolStripMenuItem_selectAllBiblio_hasOrders";
            this.toolStripMenuItem_selectAllBiblio_hasOrders.Size = new System.Drawing.Size(196, 22);
            this.toolStripMenuItem_selectAllBiblio_hasOrders.Text = "所有包含订购的书目";
            this.toolStripMenuItem_selectAllBiblio_hasOrders.Click += new System.EventHandler(this.toolStripMenuItem_selectAllBiblio_hasOrders_Click);
            // 
            // toolStripMenuItem_selectAllBiblio_noOrder
            // 
            this.toolStripMenuItem_selectAllBiblio_noOrder.Name = "toolStripMenuItem_selectAllBiblio_noOrder";
            this.toolStripMenuItem_selectAllBiblio_noOrder.Size = new System.Drawing.Size(196, 22);
            this.toolStripMenuItem_selectAllBiblio_noOrder.Text = "所有不包含订购的书目";
            this.toolStripMenuItem_selectAllBiblio_noOrder.Click += new System.EventHandler(this.toolStripMenuItem_selectAllBiblio_noOrder_Click);
            // 
            // toolStripMenuItem_removeSelectedBiblio
            // 
            this.toolStripMenuItem_removeSelectedBiblio.Enabled = false;
            this.toolStripMenuItem_removeSelectedBiblio.Name = "toolStripMenuItem_removeSelectedBiblio";
            this.toolStripMenuItem_removeSelectedBiblio.Size = new System.Drawing.Size(196, 22);
            this.toolStripMenuItem_removeSelectedBiblio.Text = "移除所选的书目";
            this.toolStripMenuItem_removeSelectedBiblio.Click += new System.EventHandler(this.toolStripMenuItem_removeSelectedBiblio_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // ToolStripMenuItem_selectAllOrder
            // 
            this.ToolStripMenuItem_selectAllOrder.Name = "ToolStripMenuItem_selectAllOrder";
            this.ToolStripMenuItem_selectAllOrder.Size = new System.Drawing.Size(196, 22);
            this.ToolStripMenuItem_selectAllOrder.Text = "所有订购";
            this.ToolStripMenuItem_selectAllOrder.Click += new System.EventHandler(this.ToolStripMenuItem_selectAllOrder_Click);
            // 
            // toolStripDropDownButton_change
            // 
            this.toolStripDropDownButton_change.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton_change.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_quickChange});
            this.toolStripDropDownButton_change.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton_change.Image")));
            this.toolStripDropDownButton_change.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton_change.Name = "toolStripDropDownButton_change";
            this.toolStripDropDownButton_change.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton_change.Text = "修改";
            // 
            // ToolStripMenuItem_quickChange
            // 
            this.ToolStripMenuItem_quickChange.Enabled = false;
            this.ToolStripMenuItem_quickChange.Name = "ToolStripMenuItem_quickChange";
            this.ToolStripMenuItem_quickChange.Size = new System.Drawing.Size(137, 22);
            this.ToolStripMenuItem_quickChange.Text = "快速修改 ...";
            this.ToolStripMenuItem_quickChange.ToolTipText = "快速修改多个订购记录的部分字段";
            this.ToolStripMenuItem_quickChange.Click += new System.EventHandler(this.ToolStripMenuItem_quickChange_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_loadBiblio
            // 
            this.toolStripButton_loadBiblio.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_loadBiblio.Enabled = false;
            this.toolStripButton_loadBiblio.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_loadBiblio.Image")));
            this.toolStripButton_loadBiblio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_loadBiblio.Name = "toolStripButton_loadBiblio";
            this.toolStripButton_loadBiblio.Size = new System.Drawing.Size(72, 22);
            this.toolStripButton_loadBiblio.Text = "装入种册窗";
            this.toolStripButton_loadBiblio.Click += new System.EventHandler(this.toolStripButton_loadBiblio_Click);
            // 
            // toolStripButton_refresh
            // 
            this.toolStripButton_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_refresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_refresh.Image")));
            this.toolStripButton_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_refresh.Name = "toolStripButton_refresh";
            this.toolStripButton_refresh.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton_refresh.Text = "刷新";
            // 
            // toolStripButton_deleteOrder
            // 
            this.toolStripButton_deleteOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_deleteOrder.Enabled = false;
            this.toolStripButton_deleteOrder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_deleteOrder.Image")));
            this.toolStripButton_deleteOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_deleteOrder.Name = "toolStripButton_deleteOrder";
            this.toolStripButton_deleteOrder.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton_deleteOrder.Text = "删除";
            this.toolStripButton_deleteOrder.Click += new System.EventHandler(this.toolStripButton_deleteOrder_Click);
            // 
            // toolStripButton_save
            // 
            this.toolStripButton_save.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_save.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_save.Image")));
            this.toolStripButton_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_save.Name = "toolStripButton_save";
            this.toolStripButton_save.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton_save.Text = "保存";
            this.toolStripButton_save.Click += new System.EventHandler(this.toolStripButton_save_Click);
            // 
            // toolStripButton_orderList
            // 
            this.toolStripButton_orderList.CheckOnClick = true;
            this.toolStripButton_orderList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_orderList.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_orderList.Image")));
            this.toolStripButton_orderList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_orderList.Name = "toolStripButton_orderList";
            this.toolStripButton_orderList.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_orderList.Text = "订单视图";
            this.toolStripButton_orderList.CheckedChanged += new System.EventHandler(this.toolStripButton_orderList_CheckedChanged);
            // 
            // toolStripButton_rate
            // 
            this.toolStripButton_rate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_rate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_rate.Image")));
            this.toolStripButton_rate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_rate.Name = "toolStripButton_rate";
            this.toolStripButton_rate.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton_rate.Text = "汇率";
            this.toolStripButton_rate.Click += new System.EventHandler(this.toolStripButton_rate_Click);
            // 
            // toolStripButton_test
            // 
            this.toolStripButton_test.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_test.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_test.Image")));
            this.toolStripButton_test.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_test.Name = "toolStripButton_test";
            this.toolStripButton_test.Size = new System.Drawing.Size(33, 22);
            this.toolStripButton_test.Text = "test";
            this.toolStripButton_test.Visible = false;
            this.toolStripButton_test.Click += new System.EventHandler(this.toolStripButton_test_Click);
            // 
            // BatchOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 305);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "BatchOrderForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "批订购";
            this.Activated += new System.EventHandler(this.BatchOrderForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BatchOrderForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BatchOrderForm_FormClosed);
            this.Load += new System.EventHandler(this.BatchOrderForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_refresh;
        private System.Windows.Forms.ToolStripButton toolStripButton_deleteOrder;
        private System.Windows.Forms.ToolStripButton toolStripButton_save;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton_newOrder;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_newOrderTemplate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton_select;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_selectAllBiblio;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_selectAllOrder;
        private System.Windows.Forms.ToolStripButton toolStripButton_loadBiblio;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton_change;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_quickChange;
        private System.Windows.Forms.ToolStripButton toolStripButton_orderList;
        private System.Windows.Forms.ToolStripButton toolStripButton_rate;
        private System.Windows.Forms.ToolStripButton toolStripButton_test;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_selectAllBiblio_hasOrders;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_selectAllBiblio_noOrder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_removeSelectedBiblio;
    }
}