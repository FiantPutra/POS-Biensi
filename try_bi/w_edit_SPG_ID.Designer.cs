namespace try_bi
{
    partial class w_edit_SPG_ID
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
            this.combo_spg = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // combo_spg
            // 
            this.combo_spg.BackColor = System.Drawing.Color.White;
            this.combo_spg.DropDownHeight = 150;
            this.combo_spg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_spg.DropDownWidth = 250;
            this.combo_spg.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.combo_spg.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_spg.ForeColor = System.Drawing.Color.Gray;
            this.combo_spg.FormattingEnabled = true;
            this.combo_spg.IntegralHeight = false;
            this.combo_spg.Location = new System.Drawing.Point(12, 58);
            this.combo_spg.Name = "combo_spg";
            this.combo_spg.Size = new System.Drawing.Size(353, 30);
            this.combo_spg.TabIndex = 13;
            this.combo_spg.SelectedIndexChanged += new System.EventHandler(this.combo_spg_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(85, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 22);
            this.label1.TabIndex = 14;
            this.label1.Text = "SELECT EMPLOYEE";
            // 
            // w_edit_SPG_ID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(377, 133);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.combo_spg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "w_edit_SPG_ID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.w_edit_SPG_ID_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.w_edit_SPG_ID_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox combo_spg;
        private System.Windows.Forms.Label label1;
    }
}