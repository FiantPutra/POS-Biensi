namespace try_bi
{
    partial class W_DO_Confirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(W_DO_Confirm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.b_close = new Bunifu.Framework.UI.BunifuThinButton2();
            this.b_ok = new Bunifu.Framework.UI.BunifuThinButton2();
            this.dgv_do = new System.Windows.Forms.DataGridView();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_do)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(100, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(330, 22);
            this.label1.TabIndex = 17;
            this.label1.Text = "DELIVERY ORDER CONFIRMATION";
            // 
            // b_close
            // 
            this.b_close.ActiveBorderThickness = 1;
            this.b_close.ActiveCornerRadius = 20;
            this.b_close.ActiveFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(204)))));
            this.b_close.ActiveForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.b_close.ActiveLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.b_close.BackColor = System.Drawing.Color.White;
            this.b_close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("b_close.BackgroundImage")));
            this.b_close.ButtonText = "CANCEL";
            this.b_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_close.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_close.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_close.IdleBorderThickness = 1;
            this.b_close.IdleCornerRadius = 20;
            this.b_close.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.b_close.IdleForecolor = System.Drawing.Color.White;
            this.b_close.IdleLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.b_close.Location = new System.Drawing.Point(49, 413);
            this.b_close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_close.Name = "b_close";
            this.b_close.Size = new System.Drawing.Size(163, 48);
            this.b_close.TabIndex = 19;
            this.b_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_close.Click += new System.EventHandler(this.b_close_Click);
            // 
            // b_ok
            // 
            this.b_ok.ActiveBorderThickness = 1;
            this.b_ok.ActiveCornerRadius = 20;
            this.b_ok.ActiveFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(204)))));
            this.b_ok.ActiveForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_ok.ActiveLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_ok.BackColor = System.Drawing.Color.White;
            this.b_ok.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("b_ok.BackgroundImage")));
            this.b_ok.ButtonText = "OK";
            this.b_ok.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_ok.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_ok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_ok.IdleBorderThickness = 1;
            this.b_ok.IdleCornerRadius = 20;
            this.b_ok.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_ok.IdleForecolor = System.Drawing.Color.White;
            this.b_ok.IdleLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_ok.Location = new System.Drawing.Point(325, 413);
            this.b_ok.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(163, 48);
            this.b_ok.TabIndex = 18;
            this.b_ok.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
            // 
            // dgv_do
            // 
            this.dgv_do.AllowUserToAddRows = false;
            this.dgv_do.AllowUserToResizeColumns = false;
            this.dgv_do.AllowUserToResizeRows = false;
            this.dgv_do.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_do.BackgroundColor = System.Drawing.Color.White;
            this.dgv_do.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_do.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            this.dgv_do.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_do.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_do.ColumnHeadersHeight = 40;
            this.dgv_do.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_do.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column4});
            this.dgv_do.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_do.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_do.EnableHeadersVisualStyles = false;
            this.dgv_do.GridColor = System.Drawing.Color.White;
            this.dgv_do.Location = new System.Drawing.Point(49, 75);
            this.dgv_do.Name = "dgv_do";
            this.dgv_do.ReadOnly = true;
            this.dgv_do.RowHeadersVisible = false;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_do.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_do.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgv_do.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dgv_do.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgv_do.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dgv_do.RowTemplate.Height = 40;
            this.dgv_do.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_do.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_do.Size = new System.Drawing.Size(439, 330);
            this.dgv_do.TabIndex = 24;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column7.HeaderText = "ARTICLE ID";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column7.Width = 200;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column4.HeaderText = "QTY DISPUTE";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // W_DO_Confirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(549, 465);
            this.ControlBox = false;
            this.Controls.Add(this.dgv_do);
            this.Controls.Add(this.b_close);
            this.Controls.Add(this.b_ok);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "W_DO_Confirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dgv_do)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuThinButton2 b_close;
        private Bunifu.Framework.UI.BunifuThinButton2 b_ok;
        private System.Windows.Forms.DataGridView dgv_do;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}