namespace try_bi
{
    partial class uc_kembalian
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_kembalian));
            this.b_print = new Bunifu.Framework.UI.BunifuThinButton2();
            this.panel1 = new System.Windows.Forms.Panel();
            this.b_new_trans2 = new Bunifu.Framework.UI.BunifuImageButton();
            this.t_shorcut2 = new System.Windows.Forms.TextBox();
            this.t_detail_center = new System.Windows.Forms.TextBox();
            this.t_kembali_center = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.b_new_trans2)).BeginInit();
            this.SuspendLayout();
            // 
            // b_print
            // 
            this.b_print.ActiveBorderThickness = 1;
            this.b_print.ActiveCornerRadius = 20;
            this.b_print.ActiveFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_print.ActiveForecolor = System.Drawing.Color.White;
            this.b_print.ActiveLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_print.BackColor = System.Drawing.Color.White;
            this.b_print.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("b_print.BackgroundImage")));
            this.b_print.ButtonText = "PRINT RECEIPT";
            this.b_print.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_print.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_print.ForeColor = System.Drawing.Color.SeaGreen;
            this.b_print.IdleBorderThickness = 1;
            this.b_print.IdleCornerRadius = 20;
            this.b_print.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_print.IdleForecolor = System.Drawing.Color.White;
            this.b_print.IdleLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_print.Location = new System.Drawing.Point(469, 289);
            this.b_print.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_print.Name = "b_print";
            this.b_print.Size = new System.Drawing.Size(261, 59);
            this.b_print.TabIndex = 13;
            this.b_print.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_print.Click += new System.EventHandler(this.b_print_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.b_new_trans2);
            this.panel1.Controls.Add(this.t_shorcut2);
            this.panel1.Controls.Add(this.t_detail_center);
            this.panel1.Controls.Add(this.t_kembali_center);
            this.panel1.Controls.Add(this.b_print);
            this.panel1.Location = new System.Drawing.Point(14, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 655);
            this.panel1.TabIndex = 14;
            // 
            // b_new_trans2
            // 
            this.b_new_trans2.BackColor = System.Drawing.Color.Transparent;
            this.b_new_trans2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_new_trans2.Image = ((System.Drawing.Image)(resources.GetObject("b_new_trans2.Image")));
            this.b_new_trans2.ImageActive = null;
            this.b_new_trans2.Location = new System.Drawing.Point(469, 367);
            this.b_new_trans2.Name = "b_new_trans2";
            this.b_new_trans2.Size = new System.Drawing.Size(261, 51);
            this.b_new_trans2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.b_new_trans2.TabIndex = 28;
            this.b_new_trans2.TabStop = false;
            this.b_new_trans2.Zoom = 0;
            this.b_new_trans2.Click += new System.EventHandler(this.b_new_trans2_Click);
            // 
            // t_shorcut2
            // 
            this.t_shorcut2.BackColor = System.Drawing.Color.White;
            this.t_shorcut2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.t_shorcut2.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_shorcut2.ForeColor = System.Drawing.Color.White;
            this.t_shorcut2.Location = new System.Drawing.Point(591, 93);
            this.t_shorcut2.Name = "t_shorcut2";
            this.t_shorcut2.ReadOnly = true;
            this.t_shorcut2.Size = new System.Drawing.Size(67, 34);
            this.t_shorcut2.TabIndex = 27;
            this.t_shorcut2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.t_shorcut2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.t_shorcut2_KeyDown);
            // 
            // t_detail_center
            // 
            this.t_detail_center.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.t_detail_center.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_detail_center.Location = new System.Drawing.Point(30, 215);
            this.t_detail_center.Name = "t_detail_center";
            this.t_detail_center.Size = new System.Drawing.Size(1120, 31);
            this.t_detail_center.TabIndex = 26;
            this.t_detail_center.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // t_kembali_center
            // 
            this.t_kembali_center.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.t_kembali_center.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_kembali_center.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.t_kembali_center.Location = new System.Drawing.Point(269, 148);
            this.t_kembali_center.Name = "t_kembali_center";
            this.t_kembali_center.Size = new System.Drawing.Size(633, 34);
            this.t_kembali_center.TabIndex = 25;
            this.t_kembali_center.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // uc_kembalian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "uc_kembalian";
            this.Size = new System.Drawing.Size(1214, 684);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.b_new_trans2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuThinButton2 b_print;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox t_kembali_center;
        private System.Windows.Forms.TextBox t_detail_center;
        private System.Windows.Forms.TextBox t_shorcut2;
        private Bunifu.Framework.UI.BunifuImageButton b_new_trans2;
    }
}
