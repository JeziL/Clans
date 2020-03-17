namespace Clans {
    partial class BenchmarkForm {
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
            this.proxyGridView = new System.Windows.Forms.DataGridView();
            this.groupListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.proxyGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // proxyGridView
            // 
            this.proxyGridView.AllowUserToAddRows = false;
            this.proxyGridView.AllowUserToDeleteRows = false;
            this.proxyGridView.AllowUserToResizeRows = false;
            this.proxyGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.proxyGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.delayColumn});
            this.proxyGridView.Location = new System.Drawing.Point(191, 12);
            this.proxyGridView.MultiSelect = false;
            this.proxyGridView.Name = "proxyGridView";
            this.proxyGridView.ReadOnly = true;
            this.proxyGridView.RowHeadersVisible = false;
            this.proxyGridView.RowHeadersWidth = 51;
            this.proxyGridView.RowTemplate.Height = 27;
            this.proxyGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.proxyGridView.Size = new System.Drawing.Size(362, 432);
            this.proxyGridView.TabIndex = 0;
            this.proxyGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.proxyGridView_CellMouseUp);
            // 
            // groupListBox
            // 
            this.groupListBox.FormattingEnabled = true;
            this.groupListBox.ItemHeight = 15;
            this.groupListBox.Location = new System.Drawing.Point(9, 35);
            this.groupListBox.Name = "groupListBox";
            this.groupListBox.Size = new System.Drawing.Size(173, 364);
            this.groupListBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "代理组：";
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.DataPropertyName = "name";
            this.nameColumn.HeaderText = "代理名称";
            this.nameColumn.MinimumWidth = 6;
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // delayColumn
            // 
            this.delayColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.delayColumn.DataPropertyName = "delayString";
            this.delayColumn.HeaderText = "延迟";
            this.delayColumn.MinimumWidth = 6;
            this.delayColumn.Name = "delayColumn";
            this.delayColumn.ReadOnly = true;
            this.delayColumn.Width = 158;
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(9, 405);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(173, 39);
            this.startBtn.TabIndex = 2;
            this.startBtn.Text = "全部开始";
            this.startBtn.UseVisualStyleBackColor = true;
            // 
            // BenchmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 451);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.proxyGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BenchmarkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "延迟测试";
            this.Load += new System.EventHandler(this.BenchmarkForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.proxyGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView proxyGridView;
        private System.Windows.Forms.ListBox groupListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn delayColumn;
        private System.Windows.Forms.Button startBtn;
    }
}