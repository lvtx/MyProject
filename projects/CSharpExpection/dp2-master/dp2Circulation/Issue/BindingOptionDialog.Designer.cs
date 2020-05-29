namespace dp2Circulation
{
    partial class BindingOptionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BindingOptionDialog));
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.tabControl_main = new System.Windows.Forms.TabControl();
            this.tabPage_general = new System.Windows.Forms.TabPage();
            this.button_defaultEntityFields = new System.Windows.Forms.Button();
            this.textBox_general_batchNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_ui = new System.Windows.Forms.TabPage();
            this.checkBox_ui_displayLockedOrderGroup = new System.Windows.Forms.CheckBox();
            this.checkBox_ui_displayOrderInfoXY = new System.Windows.Forms.CheckBox();
            this.comboBox_ui_splitterDirection = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage_cellContents = new System.Windows.Forms.TabPage();
            this.button_cellContents_modify = new System.Windows.Forms.Button();
            this.button_cellContents_delete = new System.Windows.Forms.Button();
            this.button_cellContents_new = new System.Windows.Forms.Button();
            this.button_cellContents_moveDown = new System.Windows.Forms.Button();
            this.button_cellContents_moveUp = new System.Windows.Forms.Button();
            this.listView_cellContents_lines = new DigitalPlatform.GUI.ListViewNF();
            this.columnHeader_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_caption = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage_groupContents = new System.Windows.Forms.TabPage();
            this.button_groupContents_modify = new System.Windows.Forms.Button();
            this.button_groupContents_delete = new System.Windows.Forms.Button();
            this.button_groupContents_new = new System.Windows.Forms.Button();
            this.button_groupContents_moveDown = new System.Windows.Forms.Button();
            this.button_groupContents_moveUp = new System.Windows.Forms.Button();
            this.listView_groupContents_lines = new DigitalPlatform.GUI.ListViewNF();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl_main.SuspendLayout();
            this.tabPage_general.SuspendLayout();
            this.tabPage_ui.SuspendLayout();
            this.tabPage_cellContents.SuspendLayout();
            this.tabPage_groupContents.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(386, 334);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(84, 33);
            this.button_Cancel.TabIndex = 6;
            this.button_Cancel.Text = "ȡ��";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(294, 334);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(84, 33);
            this.button_OK.TabIndex = 5;
            this.button_OK.Text = "ȷ��";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // tabControl_main
            // 
            this.tabControl_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_main.Controls.Add(this.tabPage_general);
            this.tabControl_main.Controls.Add(this.tabPage_ui);
            this.tabControl_main.Controls.Add(this.tabPage_cellContents);
            this.tabControl_main.Controls.Add(this.tabPage_groupContents);
            this.tabControl_main.Location = new System.Drawing.Point(15, 15);
            this.tabControl_main.Name = "tabControl_main";
            this.tabControl_main.SelectedIndex = 0;
            this.tabControl_main.Size = new System.Drawing.Size(456, 312);
            this.tabControl_main.TabIndex = 7;
            // 
            // tabPage_general
            // 
            this.tabPage_general.Controls.Add(this.button_defaultEntityFields);
            this.tabPage_general.Controls.Add(this.textBox_general_batchNo);
            this.tabPage_general.Controls.Add(this.label3);
            this.tabPage_general.Location = new System.Drawing.Point(4, 28);
            this.tabPage_general.Name = "tabPage_general";
            this.tabPage_general.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_general.Size = new System.Drawing.Size(448, 280);
            this.tabPage_general.TabIndex = 0;
            this.tabPage_general.Text = "һ������";
            this.tabPage_general.UseVisualStyleBackColor = true;
            // 
            // button_defaultEntityFields
            // 
            this.button_defaultEntityFields.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_defaultEntityFields.Location = new System.Drawing.Point(11, 119);
            this.button_defaultEntityFields.Name = "button_defaultEntityFields";
            this.button_defaultEntityFields.Size = new System.Drawing.Size(258, 45);
            this.button_defaultEntityFields.TabIndex = 14;
            this.button_defaultEntityFields.Text = "���¼Ĭ��ֵ...";
            this.button_defaultEntityFields.UseVisualStyleBackColor = true;
            this.button_defaultEntityFields.Click += new System.EventHandler(this.button_defaultEntityFields_Click);
            // 
            // textBox_general_acceptBatchNo
            // 
            this.textBox_general_batchNo.Location = new System.Drawing.Point(206, 60);
            this.textBox_general_batchNo.Name = "textBox_general_acceptBatchNo";
            this.textBox_general_batchNo.Size = new System.Drawing.Size(176, 28);
            this.textBox_general_batchNo.TabIndex = 3;
            this.textBox_general_batchNo.Leave += new System.EventHandler(this.textBox_general_acceptBatchNo_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "�ǵ�/װ�����κ�(&A):";
            // 
            // tabPage_ui
            // 
            this.tabPage_ui.Controls.Add(this.checkBox_ui_displayLockedOrderGroup);
            this.tabPage_ui.Controls.Add(this.checkBox_ui_displayOrderInfoXY);
            this.tabPage_ui.Controls.Add(this.comboBox_ui_splitterDirection);
            this.tabPage_ui.Controls.Add(this.label2);
            this.tabPage_ui.Location = new System.Drawing.Point(4, 28);
            this.tabPage_ui.Name = "tabPage_ui";
            this.tabPage_ui.Size = new System.Drawing.Size(448, 280);
            this.tabPage_ui.TabIndex = 1;
            this.tabPage_ui.Text = "���";
            this.tabPage_ui.UseVisualStyleBackColor = true;
            // 
            // checkBox_ui_displayLockedOrderGroup
            // 
            this.checkBox_ui_displayLockedOrderGroup.AutoSize = true;
            this.checkBox_ui_displayLockedOrderGroup.Location = new System.Drawing.Point(8, 118);
            this.checkBox_ui_displayLockedOrderGroup.Name = "checkBox_ui_displayLockedOrderGroup";
            this.checkBox_ui_displayLockedOrderGroup.Size = new System.Drawing.Size(403, 22);
            this.checkBox_ui_displayLockedOrderGroup.TabIndex = 3;
            this.checkBox_ui_displayLockedOrderGroup.Text = "��ʾ��ǰ�û���Ͻ�ֹݷ�Χ֮��Ķ�����Ϣ(&L)";
            this.checkBox_ui_displayLockedOrderGroup.UseVisualStyleBackColor = true;
            // 
            // checkBox_ui_displayOrderInfoXY
            // 
            this.checkBox_ui_displayOrderInfoXY.AutoSize = true;
            this.checkBox_ui_displayOrderInfoXY.Location = new System.Drawing.Point(8, 70);
            this.checkBox_ui_displayOrderInfoXY.Name = "checkBox_ui_displayOrderInfoXY";
            this.checkBox_ui_displayOrderInfoXY.Size = new System.Drawing.Size(223, 22);
            this.checkBox_ui_displayOrderInfoXY.TabIndex = 2;
            this.checkBox_ui_displayOrderInfoXY.Text = "��ʾ������Ϣ����ֵ(&O)";
            this.checkBox_ui_displayOrderInfoXY.UseVisualStyleBackColor = true;
            // 
            // comboBox_ui_splitterDirection
            // 
            this.comboBox_ui_splitterDirection.FormattingEnabled = true;
            this.comboBox_ui_splitterDirection.Items.AddRange(new object[] {
            "��ֱ",
            "ˮƽ"});
            this.comboBox_ui_splitterDirection.Location = new System.Drawing.Point(136, 24);
            this.comboBox_ui_splitterDirection.Name = "comboBox_ui_splitterDirection";
            this.comboBox_ui_splitterDirection.Size = new System.Drawing.Size(136, 26);
            this.comboBox_ui_splitterDirection.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "���ַ�ʽ(&L):";
            // 
            // tabPage_cellContents
            // 
            this.tabPage_cellContents.Controls.Add(this.button_cellContents_modify);
            this.tabPage_cellContents.Controls.Add(this.button_cellContents_delete);
            this.tabPage_cellContents.Controls.Add(this.button_cellContents_new);
            this.tabPage_cellContents.Controls.Add(this.button_cellContents_moveDown);
            this.tabPage_cellContents.Controls.Add(this.button_cellContents_moveUp);
            this.tabPage_cellContents.Controls.Add(this.listView_cellContents_lines);
            this.tabPage_cellContents.Controls.Add(this.label4);
            this.tabPage_cellContents.Location = new System.Drawing.Point(4, 28);
            this.tabPage_cellContents.Name = "tabPage_cellContents";
            this.tabPage_cellContents.Size = new System.Drawing.Size(448, 280);
            this.tabPage_cellContents.TabIndex = 2;
            this.tabPage_cellContents.Text = "���������";
            this.tabPage_cellContents.UseVisualStyleBackColor = true;
            // 
            // button_cellContents_modify
            // 
            this.button_cellContents_modify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cellContents_modify.Enabled = false;
            this.button_cellContents_modify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_cellContents_modify.Location = new System.Drawing.Point(358, 189);
            this.button_cellContents_modify.Name = "button_cellContents_modify";
            this.button_cellContents_modify.Size = new System.Drawing.Size(84, 33);
            this.button_cellContents_modify.TabIndex = 9;
            this.button_cellContents_modify.Text = "�޸�(&M)";
            this.button_cellContents_modify.UseVisualStyleBackColor = true;
            this.button_cellContents_modify.Click += new System.EventHandler(this.button_cellContents_modify_Click);
            // 
            // button_cellContents_delete
            // 
            this.button_cellContents_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cellContents_delete.Enabled = false;
            this.button_cellContents_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_cellContents_delete.Location = new System.Drawing.Point(358, 230);
            this.button_cellContents_delete.Name = "button_cellContents_delete";
            this.button_cellContents_delete.Size = new System.Drawing.Size(84, 33);
            this.button_cellContents_delete.TabIndex = 10;
            this.button_cellContents_delete.Text = "ɾ��(&R)";
            this.button_cellContents_delete.UseVisualStyleBackColor = true;
            this.button_cellContents_delete.Click += new System.EventHandler(this.button_cellContents_delete_Click);
            // 
            // button_cellContents_new
            // 
            this.button_cellContents_new.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cellContents_new.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_cellContents_new.Location = new System.Drawing.Point(358, 147);
            this.button_cellContents_new.Name = "button_cellContents_new";
            this.button_cellContents_new.Size = new System.Drawing.Size(84, 33);
            this.button_cellContents_new.TabIndex = 8;
            this.button_cellContents_new.Text = "����(&N)";
            this.button_cellContents_new.UseVisualStyleBackColor = true;
            this.button_cellContents_new.Click += new System.EventHandler(this.button_cellContents_new_Click);
            // 
            // button_cellContents_moveDown
            // 
            this.button_cellContents_moveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cellContents_moveDown.Enabled = false;
            this.button_cellContents_moveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_cellContents_moveDown.Location = new System.Drawing.Point(358, 81);
            this.button_cellContents_moveDown.Name = "button_cellContents_moveDown";
            this.button_cellContents_moveDown.Size = new System.Drawing.Size(84, 33);
            this.button_cellContents_moveDown.TabIndex = 7;
            this.button_cellContents_moveDown.Text = "����(&D)";
            this.button_cellContents_moveDown.UseVisualStyleBackColor = true;
            this.button_cellContents_moveDown.Click += new System.EventHandler(this.button_cellContents_moveDown_Click);
            // 
            // button_cellContents_moveUp
            // 
            this.button_cellContents_moveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cellContents_moveUp.Enabled = false;
            this.button_cellContents_moveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_cellContents_moveUp.Location = new System.Drawing.Point(358, 39);
            this.button_cellContents_moveUp.Name = "button_cellContents_moveUp";
            this.button_cellContents_moveUp.Size = new System.Drawing.Size(84, 33);
            this.button_cellContents_moveUp.TabIndex = 6;
            this.button_cellContents_moveUp.Text = "����(&U)";
            this.button_cellContents_moveUp.UseVisualStyleBackColor = true;
            this.button_cellContents_moveUp.Click += new System.EventHandler(this.button_cellContents_moveUp_Click);
            // 
            // listView_cellContents_lines
            // 
            this.listView_cellContents_lines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_cellContents_lines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_name,
            this.columnHeader_caption});
            this.listView_cellContents_lines.FullRowSelect = true;
            this.listView_cellContents_lines.HideSelection = false;
            this.listView_cellContents_lines.Location = new System.Drawing.Point(4, 39);
            this.listView_cellContents_lines.Name = "listView_cellContents_lines";
            this.listView_cellContents_lines.Size = new System.Drawing.Size(348, 235);
            this.listView_cellContents_lines.TabIndex = 1;
            this.listView_cellContents_lines.UseCompatibleStateImageBehavior = false;
            this.listView_cellContents_lines.View = System.Windows.Forms.View.Details;
            this.listView_cellContents_lines.SelectedIndexChanged += new System.EventHandler(this.listView_cellContents_lines_SelectedIndexChanged);
            // 
            // columnHeader_name
            // 
            this.columnHeader_name.Text = "����";
            this.columnHeader_name.Width = 100;
            // 
            // columnHeader_caption
            // 
            this.columnHeader_caption.Text = "����";
            this.columnHeader_caption.Width = 150;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "����ʾ��������(&L):";
            // 
            // tabPage_groupContents
            // 
            this.tabPage_groupContents.Controls.Add(this.button_groupContents_modify);
            this.tabPage_groupContents.Controls.Add(this.button_groupContents_delete);
            this.tabPage_groupContents.Controls.Add(this.button_groupContents_new);
            this.tabPage_groupContents.Controls.Add(this.button_groupContents_moveDown);
            this.tabPage_groupContents.Controls.Add(this.button_groupContents_moveUp);
            this.tabPage_groupContents.Controls.Add(this.listView_groupContents_lines);
            this.tabPage_groupContents.Controls.Add(this.label5);
            this.tabPage_groupContents.Location = new System.Drawing.Point(4, 28);
            this.tabPage_groupContents.Name = "tabPage_groupContents";
            this.tabPage_groupContents.Size = new System.Drawing.Size(448, 280);
            this.tabPage_groupContents.TabIndex = 3;
            this.tabPage_groupContents.Text = "���������";
            this.tabPage_groupContents.UseVisualStyleBackColor = true;
            // 
            // button_groupContents_modify
            // 
            this.button_groupContents_modify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_groupContents_modify.Enabled = false;
            this.button_groupContents_modify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_groupContents_modify.Location = new System.Drawing.Point(358, 189);
            this.button_groupContents_modify.Name = "button_groupContents_modify";
            this.button_groupContents_modify.Size = new System.Drawing.Size(84, 33);
            this.button_groupContents_modify.TabIndex = 16;
            this.button_groupContents_modify.Text = "�޸�(&M)";
            this.button_groupContents_modify.UseVisualStyleBackColor = true;
            this.button_groupContents_modify.Click += new System.EventHandler(this.button_groupContents_modify_Click);
            // 
            // button_groupContents_delete
            // 
            this.button_groupContents_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_groupContents_delete.Enabled = false;
            this.button_groupContents_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_groupContents_delete.Location = new System.Drawing.Point(358, 230);
            this.button_groupContents_delete.Name = "button_groupContents_delete";
            this.button_groupContents_delete.Size = new System.Drawing.Size(84, 33);
            this.button_groupContents_delete.TabIndex = 17;
            this.button_groupContents_delete.Text = "ɾ��(&R)";
            this.button_groupContents_delete.UseVisualStyleBackColor = true;
            this.button_groupContents_delete.Click += new System.EventHandler(this.button_groupContents_delete_Click);
            // 
            // button_groupContents_new
            // 
            this.button_groupContents_new.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_groupContents_new.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_groupContents_new.Location = new System.Drawing.Point(358, 147);
            this.button_groupContents_new.Name = "button_groupContents_new";
            this.button_groupContents_new.Size = new System.Drawing.Size(84, 33);
            this.button_groupContents_new.TabIndex = 15;
            this.button_groupContents_new.Text = "����(&N)";
            this.button_groupContents_new.UseVisualStyleBackColor = true;
            this.button_groupContents_new.Click += new System.EventHandler(this.button_groupContents_new_Click);
            // 
            // button_groupContents_moveDown
            // 
            this.button_groupContents_moveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_groupContents_moveDown.Enabled = false;
            this.button_groupContents_moveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_groupContents_moveDown.Location = new System.Drawing.Point(358, 81);
            this.button_groupContents_moveDown.Name = "button_groupContents_moveDown";
            this.button_groupContents_moveDown.Size = new System.Drawing.Size(84, 33);
            this.button_groupContents_moveDown.TabIndex = 14;
            this.button_groupContents_moveDown.Text = "����(&D)";
            this.button_groupContents_moveDown.UseVisualStyleBackColor = true;
            this.button_groupContents_moveDown.Click += new System.EventHandler(this.button_groupContents_moveDown_Click);
            // 
            // button_groupContents_moveUp
            // 
            this.button_groupContents_moveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_groupContents_moveUp.Enabled = false;
            this.button_groupContents_moveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_groupContents_moveUp.Location = new System.Drawing.Point(358, 39);
            this.button_groupContents_moveUp.Name = "button_groupContents_moveUp";
            this.button_groupContents_moveUp.Size = new System.Drawing.Size(84, 33);
            this.button_groupContents_moveUp.TabIndex = 13;
            this.button_groupContents_moveUp.Text = "����(&U)";
            this.button_groupContents_moveUp.UseVisualStyleBackColor = true;
            this.button_groupContents_moveUp.Click += new System.EventHandler(this.button_groupContents_moveUp_Click);
            // 
            // listView_groupContents_lines
            // 
            this.listView_groupContents_lines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_groupContents_lines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView_groupContents_lines.FullRowSelect = true;
            this.listView_groupContents_lines.HideSelection = false;
            this.listView_groupContents_lines.Location = new System.Drawing.Point(4, 39);
            this.listView_groupContents_lines.Name = "listView_groupContents_lines";
            this.listView_groupContents_lines.Size = new System.Drawing.Size(348, 235);
            this.listView_groupContents_lines.TabIndex = 12;
            this.listView_groupContents_lines.UseCompatibleStateImageBehavior = false;
            this.listView_groupContents_lines.View = System.Windows.Forms.View.Details;
            this.listView_groupContents_lines.SelectedIndexChanged += new System.EventHandler(this.listView_groupContents_lines_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "����";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "����";
            this.columnHeader2.Width = 150;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "����ʾ��������(&L):";
            // 
            // BindingOptionDialog
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(483, 382);
            this.Controls.Add(this.tabControl_main);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BindingOptionDialog";
            this.ShowInTaskbar = false;
            this.Text = "�ǵ�/װ��ѡ��";
            this.Load += new System.EventHandler(this.BindingOptionDialog_Load);
            this.tabControl_main.ResumeLayout(false);
            this.tabPage_general.ResumeLayout(false);
            this.tabPage_general.PerformLayout();
            this.tabPage_ui.ResumeLayout(false);
            this.tabPage_ui.PerformLayout();
            this.tabPage_cellContents.ResumeLayout(false);
            this.tabPage_cellContents.PerformLayout();
            this.tabPage_groupContents.ResumeLayout(false);
            this.tabPage_groupContents.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.TabControl tabControl_main;
        private System.Windows.Forms.TabPage tabPage_general;
        private System.Windows.Forms.TabPage tabPage_ui;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_ui_splitterDirection;
        private System.Windows.Forms.TextBox textBox_general_batchNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_ui_displayOrderInfoXY;
        private System.Windows.Forms.TabPage tabPage_cellContents;
        private System.Windows.Forms.Label label4;
        private DigitalPlatform.GUI.ListViewNF listView_cellContents_lines;
        private System.Windows.Forms.Button button_cellContents_modify;
        private System.Windows.Forms.Button button_cellContents_delete;
        private System.Windows.Forms.Button button_cellContents_new;
        private System.Windows.Forms.Button button_cellContents_moveDown;
        private System.Windows.Forms.Button button_cellContents_moveUp;
        private System.Windows.Forms.ColumnHeader columnHeader_name;
        private System.Windows.Forms.ColumnHeader columnHeader_caption;
        private System.Windows.Forms.TabPage tabPage_groupContents;
        private System.Windows.Forms.Button button_groupContents_modify;
        private System.Windows.Forms.Button button_groupContents_delete;
        private System.Windows.Forms.Button button_groupContents_new;
        private System.Windows.Forms.Button button_groupContents_moveDown;
        private System.Windows.Forms.Button button_groupContents_moveUp;
        private DigitalPlatform.GUI.ListViewNF listView_groupContents_lines;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox_ui_displayLockedOrderGroup;
        private System.Windows.Forms.Button button_defaultEntityFields;
    }
}