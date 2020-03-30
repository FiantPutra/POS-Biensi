namespace try_bi
{
    partial class Void_Trans
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Void_Trans));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.b_ok = new Bunifu.Framework.UI.BunifuThinButton2();
            this.b_cancel = new Bunifu.Framework.UI.BunifuThinButton2();
            this.t_remark = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(163, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Void Transaction";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(141)))), ((int)(((byte)(146)))));
            this.label2.Location = new System.Drawing.Point(12, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(440, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "You Will Void Transaction, Please Insert Your Description";
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
            this.b_ok.Location = new System.Drawing.Point(245, 200);
            this.b_ok.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(169, 54);
            this.b_ok.TabIndex = 12;
            this.b_ok.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
            // 
            // b_cancel
            // 
            this.b_cancel.ActiveBorderThickness = 1;
            this.b_cancel.ActiveCornerRadius = 20;
            this.b_cancel.ActiveFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(204)))));
            this.b_cancel.ActiveForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.b_cancel.ActiveLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.b_cancel.BackColor = System.Drawing.Color.White;
            this.b_cancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("b_cancel.BackgroundImage")));
            this.b_cancel.ButtonText = "CANCEL";
            this.b_cancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_cancel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_cancel.IdleBorderThickness = 1;
            this.b_cancel.IdleCornerRadius = 20;
            this.b_cancel.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.b_cancel.IdleForecolor = System.Drawing.Color.White;
            this.b_cancel.IdleLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.b_cancel.Location = new System.Drawing.Point(50, 200);
            this.b_cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_cancel.Name = "b_cancel";
            this.b_cancel.Size = new System.Drawing.Size(169, 54);
            this.b_cancel.TabIndex = 11;
            this.b_cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_cancel.Click += new System.EventHandler(this.b_cancel_Click);
            // 
            // t_remark
            // 
            this.t_remark.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.t_remark.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.t_remark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.t_remark.HintForeColor = System.Drawing.Color.Empty;
            this.t_remark.HintText = "";
            this.t_remark.isPassword = false;
            this.t_remark.LineFocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.t_remark.LineIdleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.t_remark.LineMouseHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.t_remark.LineThickness = 3;
            this.t_remark.Location = new System.Drawing.Point(16, 140);
            this.t_remark.Margin = new System.Windows.Forms.Padding(4);
            this.t_remark.Name = "t_remark";
            this.t_remark.Size = new System.Drawing.Size(436, 44);
            this.t_remark.TabIndex = 14;
            this.t_remark.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // Void_Trans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(466, 268);
            this.ControlBox = false;
            this.Controls.Add(this.t_remark);
            this.Controls.Add(this.b_ok);
            this.Controls.Add(this.b_cancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Void_Trans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Void_Trans_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Bunifu.Framework.UI.BunifuThinButton2 b_ok;
        private Bunifu.Framework.UI.BunifuThinButton2 b_cancel;
        private Bunifu.Framework.UI.BunifuMaterialTextbox t_remark;
    }
}