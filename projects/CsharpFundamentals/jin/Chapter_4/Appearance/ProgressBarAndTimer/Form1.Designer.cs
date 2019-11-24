namespace ProgressBarAndTimer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnDecrease = new System.Windows.Forms.Button();
            this.btnIncrease = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnTimer = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.btnSwitchIco = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(155, 37);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(527, 41);
            this.progressBar1.TabIndex = 2;
            // 
            // btnDecrease
            // 
            this.btnDecrease.BackColor = System.Drawing.Color.Transparent;
            this.btnDecrease.BackgroundImage = global::ProgressBarAndTimer.Properties.Resources.decrease;
            this.btnDecrease.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDecrease.Location = new System.Drawing.Point(714, 12);
            this.btnDecrease.Name = "btnDecrease";
            this.btnDecrease.Size = new System.Drawing.Size(90, 90);
            this.btnDecrease.TabIndex = 1;
            this.btnDecrease.UseVisualStyleBackColor = false;
            this.btnDecrease.Click += new System.EventHandler(this.BtnDecrease_Click);
            // 
            // btnIncrease
            // 
            this.btnIncrease.BackColor = System.Drawing.Color.Transparent;
            this.btnIncrease.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnIncrease.BackgroundImage")));
            this.btnIncrease.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnIncrease.Location = new System.Drawing.Point(32, 12);
            this.btnIncrease.Name = "btnIncrease";
            this.btnIncrease.Size = new System.Drawing.Size(90, 90);
            this.btnIncrease.TabIndex = 0;
            this.btnIncrease.UseVisualStyleBackColor = false;
            this.btnIncrease.Click += new System.EventHandler(this.BtnIncrease_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // btnTimer
            // 
            this.btnTimer.Image = global::ProgressBarAndTimer.Properties.Resources.EnableClock;
            this.btnTimer.Location = new System.Drawing.Point(39, 144);
            this.btnTimer.Name = "btnTimer";
            this.btnTimer.Size = new System.Drawing.Size(61, 77);
            this.btnTimer.TabIndex = 3;
            this.btnTimer.UseVisualStyleBackColor = true;
            this.btnTimer.Click += new System.EventHandler(this.BtnTimer_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(155, 160);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(527, 44);
            this.progressBar2.TabIndex = 4;
            // 
            // btnSwitchIco
            // 
            this.btnSwitchIco.Image = ((System.Drawing.Image)(resources.GetObject("btnSwitchIco.Image")));
            this.btnSwitchIco.Location = new System.Drawing.Point(728, 144);
            this.btnSwitchIco.Name = "btnSwitchIco";
            this.btnSwitchIco.Size = new System.Drawing.Size(61, 77);
            this.btnSwitchIco.TabIndex = 5;
            this.btnSwitchIco.UseVisualStyleBackColor = true;
            this.btnSwitchIco.MouseLeave += new System.EventHandler(this.BtnSwitchIco_MouseLeave);
            this.btnSwitchIco.MouseHover += new System.EventHandler(this.BtnSwitchIco_MouseHover);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 233);
            this.Controls.Add(this.btnSwitchIco);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.btnTimer);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnDecrease);
            this.Controls.Add(this.btnIncrease);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnIncrease;
        private System.Windows.Forms.Button btnDecrease;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnTimer;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Button btnSwitchIco;
    }
}

