namespace try_bi
{
    partial class RT_SearchArticle
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RT_SearchArticle));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.t_find_article = new Bunifu.Framework.UI.BunifuTextbox();
            this.combo_ktg2 = new System.Windows.Forms.ComboBox();
            this.combo_value = new System.Windows.Forms.ComboBox();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_2
            // 
            this.dgv_2.AllowUserToAddRows = false;
            this.dgv_2.AllowUserToResizeColumns = false;
            this.dgv_2.AllowUserToResizeRows = false;
            this.dgv_2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_2.BackgroundColor = System.Drawing.Color.White;
            this.dgv_2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            this.dgv_2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column7});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_2.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_2.EnableHeadersVisualStyles = false;
            this.dgv_2.Location = new System.Drawing.Point(0, 76);
            this.dgv_2.Name = "dgv_2";
            this.dgv_2.ReadOnly = true;
            this.dgv_2.RowHeadersVisible = false;
            this.dgv_2.RowTemplate.Height = 40;
            this.dgv_2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_2.Size = new System.Drawing.Size(987, 585);
            this.dgv_2.TabIndex = 36;
            this.dgv_2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgv_2_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(653, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 22);
            this.label1.TabIndex = 39;
            this.label1.Text = "Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(415, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 22);
            this.label5.TabIndex = 38;
            this.label5.Text = "Category";
            // 
            // t_find_article
            // 
            this.t_find_article.BackColor = System.Drawing.Color.DimGray;
            this.t_find_article.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("t_find_article.BackgroundImage")));
            this.t_find_article.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.t_find_article.ForeColor = System.Drawing.Color.White;
            this.t_find_article.Icon = ((System.Drawing.Image)(resources.GetObject("t_find_article.Icon")));
            this.t_find_article.Location = new System.Drawing.Point(12, 18);
            this.t_find_article.Name = "t_find_article";
            this.t_find_article.Size = new System.Drawing.Size(364, 37);
            this.t_find_article.TabIndex = 37;
            this.t_find_article.text = "";
            this.t_find_article.OnTextChange += new System.EventHandler(this.t_find_article_OnTextChange);
            // 
            // combo_ktg2
            // 
            this.combo_ktg2.BackColor = System.Drawing.Color.White;
            this.combo_ktg2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_ktg2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.combo_ktg2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_ktg2.ForeColor = System.Drawing.Color.Gray;
            this.combo_ktg2.FormattingEnabled = true;
            this.combo_ktg2.Items.AddRange(new object[] {
            "ALL",
            "Brand",
            "Department",
            "Department_Type",
            "Size",
            "Color",
            "Gender"});
            this.combo_ktg2.Location = new System.Drawing.Point(419, 35);
            this.combo_ktg2.Name = "combo_ktg2";
            this.combo_ktg2.Size = new System.Drawing.Size(198, 26);
            this.combo_ktg2.TabIndex = 42;
            this.combo_ktg2.SelectedIndexChanged += new System.EventHandler(this.combo_ktg2_SelectedIndexChanged);
            // 
            // combo_value
            // 
            this.combo_value.BackColor = System.Drawing.Color.White;
            this.combo_value.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_value.DropDownWidth = 350;
            this.combo_value.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.combo_value.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_value.ForeColor = System.Drawing.Color.Gray;
            this.combo_value.FormattingEnabled = true;
            this.combo_value.Items.AddRange(new object[] {
            "ALL",
            "Brand",
            "Department",
            "Department_Type",
            "Size",
            "Color",
            "Gender"});
            this.combo_value.Location = new System.Drawing.Point(657, 35);
            this.combo_value.Name = "combo_value";
            this.combo_value.Size = new System.Drawing.Size(323, 26);
            this.combo_value.TabIndex = 43;
            this.combo_value.SelectedIndexChanged += new System.EventHandler(this.combo_value_SelectedIndexChanged);
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column6.HeaderText = "_id";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Visible = false;
            this.Column6.Width = 60;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "ARTICLE ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 160;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "ARTICLE NAME";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column3.HeaderText = "SIZE";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 150;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.HeaderText = "COLOR";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 150;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column5.HeaderText = "PRICE";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 90;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column7.HeaderText = "QTY";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 60;
            // 
            // RT_SearchArticle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(987, 660);
            this.Controls.Add(this.combo_value);
            this.Controls.Add(this.combo_ktg2);
            this.Controls.Add(this.dgv_2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.t_find_article);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RT_SearchArticle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.RT_SearchArticle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private Bunifu.Framework.UI.BunifuTextbox t_find_article;
        private System.Windows.Forms.ComboBox combo_ktg2;
        private System.Windows.Forms.ComboBox combo_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}