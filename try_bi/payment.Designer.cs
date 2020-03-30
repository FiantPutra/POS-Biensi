namespace try_bi
{
    partial class payment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(payment));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.l_total = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.b_ok2 = new Bunifu.Framework.UI.BunifuImageButton();
            this.b_cancel2 = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.b_ok2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.b_cancel2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(148, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "CASH";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(46, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Total Amount";
            // 
            // l_total
            // 
            this.l_total.AutoSize = true;
            this.l_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_total.Location = new System.Drawing.Point(46, 100);
            this.l_total.Name = "l_total";
            this.l_total.Size = new System.Drawing.Size(34, 18);
            this.l_total.TabIndex = 7;
            this.l_total.Text = "Rp ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(44, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Cash Amount";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(48, 156);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(254, 44);
            this.textBox1.TabIndex = 11;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // b_ok2
            // 
            this.b_ok2.BackColor = System.Drawing.Color.Transparent;
            this.b_ok2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_ok2.Image = ((System.Drawing.Image)(resources.GetObject("b_ok2.Image")));
            this.b_ok2.ImageActive = null;
            this.b_ok2.Location = new System.Drawing.Point(189, 224);
            this.b_ok2.Name = "b_ok2";
            this.b_ok2.Size = new System.Drawing.Size(113, 45);
            this.b_ok2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.b_ok2.TabIndex = 15;
            this.b_ok2.TabStop = false;
            this.b_ok2.Zoom = 0;
            this.b_ok2.Click += new System.EventHandler(this.b_ok2_Click);
            // 
            // b_cancel2
            // 
            this.b_cancel2.BackColor = System.Drawing.Color.Transparent;
            this.b_cancel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_cancel2.Image = ((System.Drawing.Image)(resources.GetObject("b_cancel2.Image")));
            this.b_cancel2.ImageActive = null;
            this.b_cancel2.Location = new System.Drawing.Point(48, 224);
            this.b_cancel2.Name = "b_cancel2";
            this.b_cancel2.Size = new System.Drawing.Size(113, 45);
            this.b_cancel2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.b_cancel2.TabIndex = 16;
            this.b_cancel2.TabStop = false;
            this.b_cancel2.Zoom = 0;
            this.b_cancel2.Click += new System.EventHandler(this.b_cancel2_Click);
            // 
            // payment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(359, 309);
            this.ControlBox = false;
            this.Controls.Add(this.b_cancel2);
            this.Controls.Add(this.b_ok2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.l_total);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "payment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.payment_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.payment_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.b_ok2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.b_cancel2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label l_total;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBox1;
        private Bunifu.Framework.UI.BunifuImageButton b_ok2;
        private Bunifu.Framework.UI.BunifuImageButton b_cancel2;
    }
}