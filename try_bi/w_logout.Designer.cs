namespace try_bi
{
    partial class w_logout
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
            this.label1 = new System.Windows.Forms.Label();
            this.b_logout = new Bunifu.Framework.UI.BunifuFlatButton();
            this.b_back = new Bunifu.Framework.UI.BunifuFlatButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Are You Sure Want To Exit..?";
            // 
            // b_logout
            // 
            this.b_logout.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.b_logout.BackColor = System.Drawing.Color.Red;
            this.b_logout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_logout.BorderRadius = 0;
            this.b_logout.ButtonText = "Logout";
            this.b_logout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_logout.DisabledColor = System.Drawing.Color.Gray;
            this.b_logout.Iconcolor = System.Drawing.Color.Transparent;
            this.b_logout.Iconimage = null;
            this.b_logout.Iconimage_right = null;
            this.b_logout.Iconimage_right_Selected = null;
            this.b_logout.Iconimage_Selected = null;
            this.b_logout.IconMarginLeft = 0;
            this.b_logout.IconMarginRight = 0;
            this.b_logout.IconRightVisible = true;
            this.b_logout.IconRightZoom = 0D;
            this.b_logout.IconVisible = true;
            this.b_logout.IconZoom = 90D;
            this.b_logout.IsTab = false;
            this.b_logout.Location = new System.Drawing.Point(150, 85);
            this.b_logout.Name = "b_logout";
            this.b_logout.Normalcolor = System.Drawing.Color.Red;
            this.b_logout.OnHovercolor = System.Drawing.Color.Red;
            this.b_logout.OnHoverTextColor = System.Drawing.Color.White;
            this.b_logout.selected = false;
            this.b_logout.Size = new System.Drawing.Size(151, 48);
            this.b_logout.TabIndex = 1;
            this.b_logout.Text = "Logout";
            this.b_logout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_logout.Textcolor = System.Drawing.Color.White;
            this.b_logout.TextFont = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_logout.Click += new System.EventHandler(this.b_logout_Click);
            // 
            // b_back
            // 
            this.b_back.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.b_back.BackColor = System.Drawing.Color.ForestGreen;
            this.b_back.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_back.BorderRadius = 0;
            this.b_back.ButtonText = "Back To Menu";
            this.b_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_back.DisabledColor = System.Drawing.Color.Gray;
            this.b_back.Iconcolor = System.Drawing.Color.Transparent;
            this.b_back.Iconimage = null;
            this.b_back.Iconimage_right = null;
            this.b_back.Iconimage_right_Selected = null;
            this.b_back.Iconimage_Selected = null;
            this.b_back.IconMarginLeft = 0;
            this.b_back.IconMarginRight = 0;
            this.b_back.IconRightVisible = true;
            this.b_back.IconRightZoom = 0D;
            this.b_back.IconVisible = true;
            this.b_back.IconZoom = 90D;
            this.b_back.IsTab = false;
            this.b_back.Location = new System.Drawing.Point(0, 85);
            this.b_back.Name = "b_back";
            this.b_back.Normalcolor = System.Drawing.Color.ForestGreen;
            this.b_back.OnHovercolor = System.Drawing.Color.ForestGreen;
            this.b_back.OnHoverTextColor = System.Drawing.Color.White;
            this.b_back.selected = false;
            this.b_back.Size = new System.Drawing.Size(151, 48);
            this.b_back.TabIndex = 2;
            this.b_back.Text = "Back To Menu";
            this.b_back.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.b_back.Textcolor = System.Drawing.Color.White;
            this.b_back.TextFont = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_back.Click += new System.EventHandler(this.b_back_Click);
            // 
            // w_logout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(300, 132);
            this.ControlBox = false;
            this.Controls.Add(this.b_back);
            this.Controls.Add(this.b_logout);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = new System.Drawing.Point(1040, 60);
            this.Name = "w_logout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuFlatButton b_logout;
        private Bunifu.Framework.UI.BunifuFlatButton b_back;
    }
}