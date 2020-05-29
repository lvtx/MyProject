﻿namespace dp2Circulation
{
    partial class ChangeEntityActionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeEntityActionDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_location = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_bookType = new System.Windows.Forms.ComboBox();
            this.comboBox_state = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_focusAction = new System.Windows.Forms.ComboBox();
            this.comboBox_batchNo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkedComboBox_stateRemove = new DigitalPlatform.CommonControl.CheckedComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkedComboBox_stateAdd = new DigitalPlatform.CommonControl.CheckedComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label_state = new System.Windows.Forms.Label();
            this.label_location = new System.Windows.Forms.Label();
            this.label_bookType = new System.Windows.Forms.Label();
            this.label_batchNo = new System.Windows.Forms.Label();
            this.comboBox_returnInEdit = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox_writeToRfidTag = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "馆藏地点(&L):";
            // 
            // comboBox_location
            // 
            this.comboBox_location.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_location.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_location.FormattingEnabled = true;
            this.comboBox_location.Location = new System.Drawing.Point(156, 15);
            this.comboBox_location.Name = "comboBox_location";
            this.comboBox_location.Size = new System.Drawing.Size(244, 26);
            this.comboBox_location.TabIndex = 2;
            this.comboBox_location.Text = "<不改变>";
            this.comboBox_location.DropDown += new System.EventHandler(this.comboBox_location_DropDown);
            this.comboBox_location.SizeChanged += new System.EventHandler(this.comboBox_location_SizeChanged);
            this.comboBox_location.TextChanged += new System.EventHandler(this.comboBox_location_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "册类型(&B):";
            // 
            // comboBox_bookType
            // 
            this.comboBox_bookType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_bookType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_bookType.FormattingEnabled = true;
            this.comboBox_bookType.Location = new System.Drawing.Point(156, 56);
            this.comboBox_bookType.Name = "comboBox_bookType";
            this.comboBox_bookType.Size = new System.Drawing.Size(244, 26);
            this.comboBox_bookType.TabIndex = 5;
            this.comboBox_bookType.Text = "<不改变>";
            this.comboBox_bookType.DropDown += new System.EventHandler(this.comboBox_bookType_DropDown);
            this.comboBox_bookType.SelectedIndexChanged += new System.EventHandler(this.comboBox_bookType_SelectedIndexChanged);
            this.comboBox_bookType.SizeChanged += new System.EventHandler(this.comboBox_bookType_SizeChanged);
            this.comboBox_bookType.TextChanged += new System.EventHandler(this.comboBox_bookType_TextChanged);
            // 
            // comboBox_state
            // 
            this.comboBox_state.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_state.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_state.FormattingEnabled = true;
            this.comboBox_state.Location = new System.Drawing.Point(156, 96);
            this.comboBox_state.Name = "comboBox_state";
            this.comboBox_state.Size = new System.Drawing.Size(244, 26);
            this.comboBox_state.TabIndex = 8;
            this.comboBox_state.Text = "<不改变>";
            this.comboBox_state.DropDown += new System.EventHandler(this.comboBox_state_DropDown);
            this.comboBox_state.SelectedIndexChanged += new System.EventHandler(this.comboBox_state_SelectedIndexChanged);
            this.comboBox_state.SizeChanged += new System.EventHandler(this.comboBox_state_SizeChanged);
            this.comboBox_state.TextChanged += new System.EventHandler(this.comboBox_state_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "状态(&S):";
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(225, 453);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(84, 33);
            this.button_OK.TabIndex = 21;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(316, 453);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(84, 33);
            this.button_Cancel.TabIndex = 22;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 302);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 18);
            this.label4.TabIndex = 17;
            this.label4.Text = "装入后输入焦点自动切换到(&F):";
            // 
            // comboBox_focusAction
            // 
            this.comboBox_focusAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_focusAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_focusAction.FormattingEnabled = true;
            this.comboBox_focusAction.Items.AddRange(new object[] {
            "册条码号，并全选",
            "册信息编辑器-册条码号",
            "册信息编辑器-状态",
            "册信息编辑器-馆藏地",
            "册信息编辑器-图书类型",
            "册信息编辑器-登录号"});
            this.comboBox_focusAction.Location = new System.Drawing.Point(156, 324);
            this.comboBox_focusAction.Name = "comboBox_focusAction";
            this.comboBox_focusAction.Size = new System.Drawing.Size(244, 26);
            this.comboBox_focusAction.TabIndex = 18;
            this.comboBox_focusAction.Text = "册条码号，并全选";
            this.comboBox_focusAction.SizeChanged += new System.EventHandler(this.comboBox_focusAction_SizeChanged);
            // 
            // comboBox_batchNo
            // 
            this.comboBox_batchNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_batchNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_batchNo.FormattingEnabled = true;
            this.comboBox_batchNo.Items.AddRange(new object[] {
            "<不改变>"});
            this.comboBox_batchNo.Location = new System.Drawing.Point(156, 212);
            this.comboBox_batchNo.Name = "comboBox_batchNo";
            this.comboBox_batchNo.Size = new System.Drawing.Size(244, 26);
            this.comboBox_batchNo.TabIndex = 15;
            this.comboBox_batchNo.Text = "<不改变>";
            this.comboBox_batchNo.SizeChanged += new System.EventHandler(this.comboBox_batchNo_SizeChanged);
            this.comboBox_batchNo.TextChanged += new System.EventHandler(this.comboBox_batchNo_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "批次号(&N):";
            // 
            // checkedComboBox_stateRemove
            // 
            this.checkedComboBox_stateRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedComboBox_stateRemove.BackColor = System.Drawing.SystemColors.Window;
            this.checkedComboBox_stateRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkedComboBox_stateRemove.Location = new System.Drawing.Point(220, 162);
            this.checkedComboBox_stateRemove.Margin = new System.Windows.Forms.Padding(0);
            this.checkedComboBox_stateRemove.Name = "checkedComboBox_stateRemove";
            this.checkedComboBox_stateRemove.Padding = new System.Windows.Forms.Padding(4);
            this.checkedComboBox_stateRemove.Size = new System.Drawing.Size(180, 29);
            this.checkedComboBox_stateRemove.TabIndex = 12;
            this.checkedComboBox_stateRemove.DropDown += new System.EventHandler(this.checkedComboBox_stateRemove_DropDown);
            this.checkedComboBox_stateRemove.TextChanged += new System.EventHandler(this.checkedComboBox_stateRemove_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(152, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 18);
            this.label6.TabIndex = 11;
            this.label6.Text = "减(&R):";
            // 
            // checkedComboBox_stateAdd
            // 
            this.checkedComboBox_stateAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedComboBox_stateAdd.BackColor = System.Drawing.SystemColors.Window;
            this.checkedComboBox_stateAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkedComboBox_stateAdd.Location = new System.Drawing.Point(220, 129);
            this.checkedComboBox_stateAdd.Margin = new System.Windows.Forms.Padding(0);
            this.checkedComboBox_stateAdd.Name = "checkedComboBox_stateAdd";
            this.checkedComboBox_stateAdd.Padding = new System.Windows.Forms.Padding(4);
            this.checkedComboBox_stateAdd.Size = new System.Drawing.Size(180, 29);
            this.checkedComboBox_stateAdd.TabIndex = 10;
            this.checkedComboBox_stateAdd.DropDown += new System.EventHandler(this.checkedComboBox_stateAdd_DropDown);
            this.checkedComboBox_stateAdd.TextChanged += new System.EventHandler(this.checkedComboBox_stateAdd_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(152, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 18);
            this.label7.TabIndex = 9;
            this.label7.Text = "增(&A):";
            // 
            // label_state
            // 
            this.label_state.Location = new System.Drawing.Point(134, 96);
            this.label_state.Margin = new System.Windows.Forms.Padding(0);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(15, 99);
            this.label_state.TabIndex = 7;
            // 
            // label_location
            // 
            this.label_location.Location = new System.Drawing.Point(134, 15);
            this.label_location.Margin = new System.Windows.Forms.Padding(0);
            this.label_location.Name = "label_location";
            this.label_location.Size = new System.Drawing.Size(15, 30);
            this.label_location.TabIndex = 1;
            // 
            // label_bookType
            // 
            this.label_bookType.Location = new System.Drawing.Point(134, 56);
            this.label_bookType.Margin = new System.Windows.Forms.Padding(0);
            this.label_bookType.Name = "label_bookType";
            this.label_bookType.Size = new System.Drawing.Size(15, 30);
            this.label_bookType.TabIndex = 4;
            // 
            // label_batchNo
            // 
            this.label_batchNo.Location = new System.Drawing.Point(134, 212);
            this.label_batchNo.Margin = new System.Windows.Forms.Padding(0);
            this.label_batchNo.Name = "label_batchNo";
            this.label_batchNo.Size = new System.Drawing.Size(15, 30);
            this.label_batchNo.TabIndex = 14;
            // 
            // comboBox_returnInEdit
            // 
            this.comboBox_returnInEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_returnInEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_returnInEdit.FormattingEnabled = true;
            this.comboBox_returnInEdit.Items.AddRange(new object[] {
            "<无>",
            "将焦点切换到条码号文本框",
            "保存当前记录"});
            this.comboBox_returnInEdit.Location = new System.Drawing.Point(156, 402);
            this.comboBox_returnInEdit.Name = "comboBox_returnInEdit";
            this.comboBox_returnInEdit.Size = new System.Drawing.Size(244, 26);
            this.comboBox_returnInEdit.TabIndex = 20;
            this.comboBox_returnInEdit.Text = "<无>";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 381);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(170, 18);
            this.label8.TabIndex = 19;
            this.label8.Text = "在册信息区回车(&R):";
            // 
            // checkBox_writeToRfidTag
            // 
            this.checkBox_writeToRfidTag.AutoSize = true;
            this.checkBox_writeToRfidTag.Location = new System.Drawing.Point(17, 259);
            this.checkBox_writeToRfidTag.Name = "checkBox_writeToRfidTag";
            this.checkBox_writeToRfidTag.Size = new System.Drawing.Size(187, 22);
            this.checkBox_writeToRfidTag.TabIndex = 16;
            this.checkBox_writeToRfidTag.Text = "写入 RFID 标签(&W)";
            this.checkBox_writeToRfidTag.UseVisualStyleBackColor = true;
            // 
            // ChangeEntityActionDialog
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(414, 501);
            this.Controls.Add(this.checkBox_writeToRfidTag);
            this.Controls.Add(this.comboBox_returnInEdit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label_batchNo);
            this.Controls.Add(this.label_bookType);
            this.Controls.Add(this.label_location);
            this.Controls.Add(this.label_state);
            this.Controls.Add(this.checkedComboBox_stateRemove);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkedComboBox_stateAdd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox_batchNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox_focusAction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.comboBox_state);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_bookType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_location);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChangeEntityActionDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "动作参数";
            this.Load += new System.EventHandler(this.ChangeParamDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_location;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_bookType;
        private System.Windows.Forms.ComboBox comboBox_state;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_focusAction;
        private System.Windows.Forms.ComboBox comboBox_batchNo;
        private System.Windows.Forms.Label label5;
        private DigitalPlatform.CommonControl.CheckedComboBox checkedComboBox_stateRemove;
        private System.Windows.Forms.Label label6;
        private DigitalPlatform.CommonControl.CheckedComboBox checkedComboBox_stateAdd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_state;
        private System.Windows.Forms.Label label_location;
        private System.Windows.Forms.Label label_bookType;
        private System.Windows.Forms.Label label_batchNo;
        private System.Windows.Forms.ComboBox comboBox_returnInEdit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox_writeToRfidTag;
    }
}