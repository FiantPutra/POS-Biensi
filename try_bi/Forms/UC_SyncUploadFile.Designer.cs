namespace try_bi
{
    partial class UC_SyncUploadFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_SyncUploadFile));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.b_back2 = new Bunifu.Framework.UI.BunifuImageButton();
            this.b_uploadFTP = new Bunifu.Framework.UI.BunifuThinButton2();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_UploadFile = new Bunifu.Framework.UI.BunifuCustomDataGrid();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.b_back2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_UploadFile)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.b_back2);
            this.panel1.Controls.Add(this.b_uploadFTP);
            this.panel1.Location = new System.Drawing.Point(15, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 66);
            this.panel1.TabIndex = 16;
            // 
            // b_back2
            // 
            this.b_back2.BackColor = System.Drawing.Color.Transparent;
            this.b_back2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_back2.Image = ((System.Drawing.Image)(resources.GetObject("b_back2.Image")));
            this.b_back2.ImageActive = null;
            this.b_back2.Location = new System.Drawing.Point(1046, 10);
            this.b_back2.Name = "b_back2";
            this.b_back2.Size = new System.Drawing.Size(124, 46);
            this.b_back2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.b_back2.TabIndex = 15;
            this.b_back2.TabStop = false;
            this.b_back2.Zoom = 0;
            this.b_back2.Click += new System.EventHandler(this.b_back2_Click);
            // 
            // b_uploadFTP
            // 
            this.b_uploadFTP.ActiveBorderThickness = 1;
            this.b_uploadFTP.ActiveCornerRadius = 20;
            this.b_uploadFTP.ActiveFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_uploadFTP.ActiveForecolor = System.Drawing.Color.White;
            this.b_uploadFTP.ActiveLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_uploadFTP.BackColor = System.Drawing.Color.White;
            this.b_uploadFTP.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("b_uploadFTP.BackgroundImage")));
            this.b_uploadFTP.ButtonText = "Upload Files";
            this.b_uploadFTP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_uploadFTP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_uploadFTP.ForeColor = System.Drawing.Color.SeaGreen;
            this.b_uploadFTP.IdleBorderThickness = 1;
            this.b_uploadFTP.IdleCornerRadius = 20;
            this.b_uploadFTP.IdleFillColor = System.Drawing.Color.White;
            this.b_uploadFTP.IdleForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_uploadFTP.IdleLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_uploadFTP.Location = new System.Drawing.Point(6, 5);
            this.b_uploadFTP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_uploadFTP.Name = "b_uploadFTP";
            this.b_uploadFTP.Size = new System.Drawing.Size(222, 56);
            this.b_uploadFTP.TabIndex = 14;
            this.b_uploadFTP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_uploadFTP.Click += new System.EventHandler(this.b_uploadFTP_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.dgv_UploadFile);
            this.panel2.Location = new System.Drawing.Point(15, 87);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1184, 580);
            this.panel2.TabIndex = 17;
            // 
            // dgv_UploadFile
            // 
            this.dgv_UploadFile.AllowUserToAddRows = false;
            this.dgv_UploadFile.AllowUserToResizeColumns = false;
            this.dgv_UploadFile.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgv_UploadFile.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_UploadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_UploadFile.BackgroundColor = System.Drawing.Color.White;
            this.dgv_UploadFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_UploadFile.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenHorizontal;
            this.dgv_UploadFile.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_UploadFile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_UploadFile.ColumnHeadersHeight = 40;
            this.dgv_UploadFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_UploadFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column6,
            this.Column7,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_UploadFile.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_UploadFile.DoubleBuffered = true;
            this.dgv_UploadFile.EnableHeadersVisualStyles = false;
            this.dgv_UploadFile.HeaderBgColor = System.Drawing.Color.White;
            this.dgv_UploadFile.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.dgv_UploadFile.Location = new System.Drawing.Point(3, 3);
            this.dgv_UploadFile.Name = "dgv_UploadFile";
            this.dgv_UploadFile.ReadOnly = true;
            this.dgv_UploadFile.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_UploadFile.RowHeadersVisible = false;
            this.dgv_UploadFile.RowTemplate.Height = 40;
            this.dgv_UploadFile.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_UploadFile.Size = new System.Drawing.Size(1178, 574);
            this.dgv_UploadFile.TabIndex = 14;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "Table Name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Row Fatch";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 130;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Row Applied";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 130;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column3.HeaderText = "Status";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 250;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column4.HeaderText = "Sync Date";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 200;
            // 
            // UC_SyncUploadFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UC_SyncUploadFile";
            this.Size = new System.Drawing.Size(1214, 684);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.b_back2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_UploadFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Bunifu.Framework.UI.BunifuCustomDataGrid dgv_UploadFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private Bunifu.Framework.UI.BunifuThinButton2 b_uploadFTP;
        private Bunifu.Framework.UI.BunifuImageButton b_back2;
    }
}
