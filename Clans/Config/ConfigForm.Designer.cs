namespace Clans {
    partial class ConfigForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.configGridView = new System.Windows.Forms.DataGridView();
            this.ConfigName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Url = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.updateBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.configGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // configGridView
            // 
            this.configGridView.AllowUserToAddRows = false;
            this.configGridView.AllowUserToDeleteRows = false;
            this.configGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.configGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConfigName,
            this.Url,
            this.Timestamp});
            this.configGridView.Location = new System.Drawing.Point(12, 13);
            this.configGridView.MultiSelect = false;
            this.configGridView.Name = "configGridView";
            this.configGridView.ReadOnly = true;
            this.configGridView.RowHeadersVisible = false;
            this.configGridView.RowHeadersWidth = 72;
            this.configGridView.RowTemplate.Height = 33;
            this.configGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.configGridView.Size = new System.Drawing.Size(776, 330);
            this.configGridView.TabIndex = 0;
            // 
            // ConfigName
            // 
            this.ConfigName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ConfigName.DataPropertyName = "name";
            this.ConfigName.HeaderText = "名称";
            this.ConfigName.MinimumWidth = 9;
            this.ConfigName.Name = "ConfigName";
            this.ConfigName.ReadOnly = true;
            // 
            // Url
            // 
            this.Url.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Url.DataPropertyName = "url";
            this.Url.HeaderText = "链接";
            this.Url.MinimumWidth = 9;
            this.Url.Name = "Url";
            this.Url.ReadOnly = true;
            // 
            // Timestamp
            // 
            this.Timestamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Timestamp.DataPropertyName = "timeString";
            this.Timestamp.HeaderText = "更新时间";
            this.Timestamp.MinimumWidth = 9;
            this.Timestamp.Name = "Timestamp";
            this.Timestamp.ReadOnly = true;
            // 
            // deleteBtn
            // 
            this.deleteBtn.Enabled = false;
            this.deleteBtn.Location = new System.Drawing.Point(12, 363);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(132, 43);
            this.deleteBtn.TabIndex = 1;
            this.deleteBtn.Text = "删除";
            this.deleteBtn.UseVisualStyleBackColor = true;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // updateBtn
            // 
            this.updateBtn.Enabled = false;
            this.updateBtn.Location = new System.Drawing.Point(334, 363);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(132, 43);
            this.updateBtn.TabIndex = 2;
            this.updateBtn.Text = "更新";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(656, 363);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(132, 43);
            this.addBtn.TabIndex = 3;
            this.addBtn.Text = "添加";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 426);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.configGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "托管的配置文件";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.configGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView configGridView;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfigName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Url;
        private System.Windows.Forms.DataGridViewTextBoxColumn Timestamp;
    }
}