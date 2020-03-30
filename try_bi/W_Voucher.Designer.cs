namespace try_bi
{
    partial class W_Voucher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(W_Voucher));
            this.t_voucher = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.b_cancel2 = new Bunifu.Framework.UI.BunifuImageButton();
            this.b_ok2 = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.b_cancel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.b_ok2)).BeginInit();
            this.SuspendLayout();
            // 
            // t_voucher
            // 
            this.t_voucher.BackColor = System.Drawing.SystemColors.Window;
            this.t_voucher.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_voucher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.t_voucher.Location = new System.Drawing.Point(47, 139);
            this.t_voucher.Multiline = true;
            this.t_voucher.Name = "t_voucher";
            this.t_voucher.Size = new System.Drawing.Size(254, 44);
            this.t_voucher.TabIndex = 18;
            this.t_voucher.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(44, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Voucher Code";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(128, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 22);
            this.label1.TabIndex = 12;
            this.label1.Text = "VOUCHER";
            // 
            // b_cancel2
            // 
            this.b_cancel2.BackColor = System.Drawing.Color.Transparent;
            this.b_cancel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_cancel2.Image = ((System.Drawing.Image)(resources.GetObject("b_cancel2.Image")));
            this.b_cancel2.ImageActive = null;
            this.b_cancel2.Location = new System.Drawing.Point(47, 215);
            this.b_cancel2.Name = "b_cancel2";
            this.b_cancel2.Size = new System.Drawing.Size(113, 45);
            this.b_cancel2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.b_cancel2.TabIndex = 20;
            this.b_cancel2.TabStop = false;
            this.b_cancel2.Zoom = 0;
            this.b_cancel2.Click += new System.EventHandler(this.b_cancel2_Click);
            // 
            // b_ok2
            // 
            this.b_ok2.BackColor = System.Drawing.Color.Transparent;
            this.b_ok2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_ok2.Image = ((System.Drawing.Image)(resources.GetObject("b_ok2.Image")));
            this.b_ok2.ImageActive = null;
            this.b_ok2.Location = new System.Drawing.Point(188, 215);
            this.b_ok2.Name = "b_ok2";
            this.b_ok2.Size = new System.Drawing.Size(113, 45);
            this.b_ok2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.b_ok2.TabIndex = 19;
            this.b_ok2.TabStop = false;
            this.b_ok2.Zoom = 0;
            this.b_ok2.Click += new System.EventHandler(this.b_ok2_Click);
            // 
            // W_Voucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(345, 272);
            this.ControlBox = false;
            this.Controls.Add(this.b_cancel2);
            this.Controls.Add(this.b_ok2);
            this.Controls.Add(this.t_voucher);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "W_Voucher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.W_Voucher_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.W_Voucher_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.b_cancel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.b_ok2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox t_voucher;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuImageButton b_cancel2;
        private Bunifu.Framework.UI.BunifuImageButton b_ok2;
    }
}