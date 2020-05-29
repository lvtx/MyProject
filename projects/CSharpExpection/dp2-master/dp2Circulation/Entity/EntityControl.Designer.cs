namespace dp2Circulation
{
    partial class EntityControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityControl));
            this.listView = new DigitalPlatform.GUI.ListViewNF();
            this.columnHeader_barcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_errorInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_publicshTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_location = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_seller = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_source = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_price = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_volume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_accessNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_bookType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_registerNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_comment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_mergeComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_batchNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_borrower = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_borrowDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_borrowPeriod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_intact = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_bindingCost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_binding = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_operations = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_recpath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_refID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList_itemType = new System.Windows.Forms.ImageList(this.components);
            this.columnHeader_shelfNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_barcode,
            this.columnHeader_errorInfo,
            this.columnHeader_state,
            this.columnHeader_publicshTime,
            this.columnHeader_location,
            this.columnHeader_seller,
            this.columnHeader_source,
            this.columnHeader_price,
            this.columnHeader_volume,
            this.columnHeader_accessNo,
            this.columnHeader_shelfNo,
            this.columnHeader_bookType,
            this.columnHeader_registerNo,
            this.columnHeader_comment,
            this.columnHeader_mergeComment,
            this.columnHeader_batchNo,
            this.columnHeader_borrower,
            this.columnHeader_borrowDate,
            this.columnHeader_borrowPeriod,
            this.columnHeader_intact,
            this.columnHeader_bindingCost,
            this.columnHeader_binding,
            this.columnHeader_operations,
            this.columnHeader_recpath,
            this.columnHeader_refID});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.LargeImageList = this.imageList_itemType;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Margin = new System.Windows.Forms.Padding(0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(424, 104);
            this.listView.SmallImageList = this.imageList_itemType;
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_items_ColumnClick);
            this.listView.DoubleClick += new System.EventHandler(this.listView_items_DoubleClick);
            this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListView_KeyDown);
            this.listView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView_items_MouseUp);
            // 
            // columnHeader_barcode
            // 
            this.columnHeader_barcode.Text = "�������";
            this.columnHeader_barcode.Width = 150;
            // 
            // columnHeader_errorInfo
            // 
            this.columnHeader_errorInfo.Text = "������Ϣ";
            this.columnHeader_errorInfo.Width = 200;
            // 
            // columnHeader_state
            // 
            this.columnHeader_state.Text = "״̬";
            this.columnHeader_state.Width = 100;
            // 
            // columnHeader_publicshTime
            // 
            this.columnHeader_publicshTime.Text = "����ʱ��(�ڶ�λ��Ϣ)";
            this.columnHeader_publicshTime.Width = 180;
            // 
            // columnHeader_location
            // 
            this.columnHeader_location.Text = "�ݲصص�";
            this.columnHeader_location.Width = 150;
            // 
            // columnHeader_seller
            // 
            this.columnHeader_seller.Text = "��������";
            this.columnHeader_seller.Width = 100;
            // 
            // columnHeader_source
            // 
            this.columnHeader_source.Text = "������Դ";
            this.columnHeader_source.Width = 100;
            // 
            // columnHeader_price
            // 
            this.columnHeader_price.Text = "��۸�";
            this.columnHeader_price.Width = 150;
            // 
            // columnHeader_volume
            // 
            this.columnHeader_volume.Text = "����";
            this.columnHeader_volume.Width = 150;
            // 
            // columnHeader_accessNo
            // 
            this.columnHeader_accessNo.Text = "��ȡ��";
            this.columnHeader_accessNo.Width = 150;
            // 
            // columnHeader_bookType
            // 
            this.columnHeader_bookType.Text = "������";
            this.columnHeader_bookType.Width = 150;
            // 
            // columnHeader_registerNo
            // 
            this.columnHeader_registerNo.Text = "��¼��";
            this.columnHeader_registerNo.Width = 150;
            // 
            // columnHeader_comment
            // 
            this.columnHeader_comment.Text = "��ע";
            this.columnHeader_comment.Width = 150;
            // 
            // columnHeader_mergeComment
            // 
            this.columnHeader_mergeComment.Text = "�ϲ�ע��";
            this.columnHeader_mergeComment.Width = 150;
            // 
            // columnHeader_batchNo
            // 
            this.columnHeader_batchNo.Text = "���κ�";
            // 
            // columnHeader_borrower
            // 
            this.columnHeader_borrower.Text = "������";
            this.columnHeader_borrower.Width = 150;
            // 
            // columnHeader_borrowDate
            // 
            this.columnHeader_borrowDate.Text = "��������";
            this.columnHeader_borrowDate.Width = 150;
            // 
            // columnHeader_borrowPeriod
            // 
            this.columnHeader_borrowPeriod.Text = "��������";
            this.columnHeader_borrowPeriod.Width = 150;
            // 
            // columnHeader_intact
            // 
            this.columnHeader_intact.Text = "�����";
            this.columnHeader_intact.Width = 150;
            // 
            // columnHeader_bindingCost
            // 
            this.columnHeader_bindingCost.Text = "װ����";
            this.columnHeader_bindingCost.Width = 150;
            // 
            // columnHeader_binding
            // 
            this.columnHeader_binding.Text = "װ��";
            this.columnHeader_binding.Width = 150;
            // 
            // columnHeader_operations
            // 
            this.columnHeader_operations.Text = "����";
            this.columnHeader_operations.Width = 150;
            // 
            // columnHeader_recpath
            // 
            this.columnHeader_recpath.Text = "���¼·��";
            this.columnHeader_recpath.Width = 200;
            // 
            // columnHeader_refID
            // 
            this.columnHeader_refID.Text = "�ο�ID";
            this.columnHeader_refID.Width = 200;
            // 
            // imageList_itemType
            // 
            this.imageList_itemType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_itemType.ImageStream")));
            this.imageList_itemType.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList_itemType.Images.SetKeyName(0, "normal_entity.bmp");
            this.imageList_itemType.Images.SetKeyName(1, "new_entity.bmp");
            this.imageList_itemType.Images.SetKeyName(2, "changed_entity.bmp");
            this.imageList_itemType.Images.SetKeyName(3, "deleted_entity.bmp");
            this.imageList_itemType.Images.SetKeyName(4, "error_entity.bmp");
            // 
            // columnHeader_shelfNo
            // 
            this.columnHeader_shelfNo.Text = "�ܺ�";
            this.columnHeader_shelfNo.Width = 100;
            // 
            // EntityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "EntityControl";
            this.Size = new System.Drawing.Size(424, 104);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader_barcode;
        private System.Windows.Forms.ColumnHeader columnHeader_errorInfo;
        private System.Windows.Forms.ColumnHeader columnHeader_state;
        private System.Windows.Forms.ColumnHeader columnHeader_location;
        private System.Windows.Forms.ColumnHeader columnHeader_price;
        private System.Windows.Forms.ColumnHeader columnHeader_volume;
        private System.Windows.Forms.ColumnHeader columnHeader_bookType;
        private System.Windows.Forms.ColumnHeader columnHeader_registerNo;
        private System.Windows.Forms.ColumnHeader columnHeader_comment;
        private System.Windows.Forms.ColumnHeader columnHeader_mergeComment;
        private System.Windows.Forms.ColumnHeader columnHeader_batchNo;
        private System.Windows.Forms.ColumnHeader columnHeader_borrower;
        private System.Windows.Forms.ColumnHeader columnHeader_borrowDate;
        private System.Windows.Forms.ColumnHeader columnHeader_borrowPeriod;
        private System.Windows.Forms.ColumnHeader columnHeader_recpath;
        private System.Windows.Forms.ColumnHeader columnHeader_publicshTime;
        private System.Windows.Forms.ColumnHeader columnHeader_seller;
        private System.Windows.Forms.ImageList imageList_itemType;
        private System.Windows.Forms.ColumnHeader columnHeader_source;
        private System.Windows.Forms.ColumnHeader columnHeader_refID;
        private DigitalPlatform.GUI.ListViewNF listView;
        private System.Windows.Forms.ColumnHeader columnHeader_accessNo;
        private System.Windows.Forms.ColumnHeader columnHeader_intact;
        private System.Windows.Forms.ColumnHeader columnHeader_binding;
        private System.Windows.Forms.ColumnHeader columnHeader_operations;
        private System.Windows.Forms.ColumnHeader columnHeader_bindingCost;
        private System.Windows.Forms.ColumnHeader columnHeader_shelfNo;
    }
}
