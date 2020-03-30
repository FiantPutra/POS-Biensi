namespace try_bi
{
    partial class W_PettyCash_Confirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(W_PettyCash_Confirm));
            this.b_close = new Bunifu.Framework.UI.BunifuThinButton2();
            this.b_ok = new Bunifu.Framework.UI.BunifuThinButton2();
            this.l_judul = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // b_close
            // 
            this.b_close.ActiveBorderThickness = 1;
            this.b_close.ActiveCornerRadius = 20;
            this.b_close.ActiveFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(204)))));
            this.b_close.ActiveForecolor = System.Drawing.Color.Red;
            this.b_close.ActiveLineColor = System.Drawing.Color.Red;
            this.b_close.BackColor = System.Drawing.Color.White;
            this.b_close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("b_close.BackgroundImage")));
            this.b_close.ButtonText = "CANCEL";
            this.b_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_close.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_close.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_close.IdleBorderThickness = 1;
            this.b_close.IdleCornerRadius = 20;
            this.b_close.IdleFillColor = System.Drawing.Color.Red;
            this.b_close.IdleForecolor = System.Drawing.Color.White;
            this.b_close.IdleLineColor = System.Drawing.Color.Red;
            this.b_close.Location = new System.Drawing.Point(45, 105);
            this.b_close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_close.Name = "b_close";
            this.b_close.Size = new System.Drawing.Size(123, 48);
            this.b_close.TabIndex = 17;
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
            this.b_ok.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_ok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_ok.IdleBorderThickness = 1;
            this.b_ok.IdleCornerRadius = 20;
            this.b_ok.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_ok.IdleForecolor = System.Drawing.Color.White;
            this.b_ok.IdleLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_ok.Location = new System.Drawing.Point(218, 105);
            this.b_ok.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(123, 48);
            this.b_ok.TabIndex = 16;
            this.b_ok.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
            // 
            // l_judul
            // 
            this.l_judul.AutoSize = true;
            this.l_judul.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_judul.Location = new System.Drawing.Point(99, 43);
            this.l_judul.Name = "l_judul";
            this.l_judul.Size = new System.Drawing.Size(187, 22);
            this.l_judul.TabIndex = 18;
            this.l_judul.Text = "Confirm Expense ?";
            // 
            // W_PettyCash_Confirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(397, 160);
            this.ControlBox = false;
            this.Controls.Add(this.l_judul);
            this.Controls.Add(this.b_close);
            this.Controls.Add(this.b_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "W_PettyCash_Confirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuThinButton2 b_close;
        private Bunifu.Framework.UI.BunifuThinButton2 b_ok;
        private System.Windows.Forms.Label l_judul;
    }
}