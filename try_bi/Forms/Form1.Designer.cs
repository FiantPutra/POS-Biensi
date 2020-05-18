namespace try_bi
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            BunifuAnimatorNS.Animation animation3 = new BunifuAnimatorNS.Animation();
            BunifuAnimatorNS.Animation animation2 = new BunifuAnimatorNS.Animation();
            BunifuAnimatorNS.Animation animation1 = new BunifuAnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.p_kiri = new System.Windows.Forms.Panel();
            this.b_SyncStore = new Bunifu.Framework.UI.BunifuFlatButton();
            this.bunifuFlatButton1 = new Bunifu.Framework.UI.BunifuFlatButton();
            this.b_petyCash = new Bunifu.Framework.UI.BunifuFlatButton();
            this.btn_close = new System.Windows.Forms.PictureBox();
            this.b_do = new Bunifu.Framework.UI.BunifuFlatButton();
            this.b_return = new Bunifu.Framework.UI.BunifuFlatButton();
            this.b_mutasi = new Bunifu.Framework.UI.BunifuFlatButton();
            this.b_deliveryCust = new Bunifu.Framework.UI.BunifuFlatButton();
            this.b_stockTake = new Bunifu.Framework.UI.BunifuFlatButton();
            this.b_inventory = new Bunifu.Framework.UI.BunifuFlatButton();
            this.bunifuFlatButton4 = new Bunifu.Framework.UI.BunifuFlatButton();
            this.bunifuFlatButton3 = new Bunifu.Framework.UI.BunifuFlatButton();
            this.bunifuFlatButton2 = new Bunifu.Framework.UI.BunifuFlatButton();
            this.b_transaction = new Bunifu.Framework.UI.BunifuFlatButton();
            this.btn_maax = new System.Windows.Forms.PictureBox();
            this.p_atas = new System.Windows.Forms.Panel();
            this.menu_inout = new System.Windows.Forms.PictureBox();
            this.t_nama = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pic_profil = new System.Windows.Forms.PictureBox();
            this.label_title = new System.Windows.Forms.Label();
            this.p_kanan = new System.Windows.Forms.Panel();
            this.LogoMax = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.LogoClose = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.AnimatorKiri = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.p_kiri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_maax)).BeginInit();
            this.p_atas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menu_inout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_profil)).BeginInit();
            this.SuspendLayout();
            // 
            // p_kiri
            // 
            this.p_kiri.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.p_kiri.Controls.Add(this.b_SyncStore);
            this.p_kiri.Controls.Add(this.bunifuFlatButton1);
            this.p_kiri.Controls.Add(this.b_petyCash);
            this.p_kiri.Controls.Add(this.btn_close);
            this.p_kiri.Controls.Add(this.b_do);
            this.p_kiri.Controls.Add(this.b_return);
            this.p_kiri.Controls.Add(this.b_mutasi);
            this.p_kiri.Controls.Add(this.b_deliveryCust);
            this.p_kiri.Controls.Add(this.b_stockTake);
            this.p_kiri.Controls.Add(this.b_inventory);
            this.p_kiri.Controls.Add(this.bunifuFlatButton4);
            this.p_kiri.Controls.Add(this.bunifuFlatButton3);
            this.p_kiri.Controls.Add(this.bunifuFlatButton2);
            this.p_kiri.Controls.Add(this.b_transaction);
            this.p_kiri.Controls.Add(this.btn_maax);
            this.LogoMax.SetDecoration(this.p_kiri, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.p_kiri, BunifuAnimatorNS.DecorationType.None);
            this.AnimatorKiri.SetDecoration(this.p_kiri, BunifuAnimatorNS.DecorationType.None);
            this.p_kiri.Dock = System.Windows.Forms.DockStyle.Left;
            this.p_kiri.Location = new System.Drawing.Point(0, 49);
            this.p_kiri.Name = "p_kiri";
            this.p_kiri.Size = new System.Drawing.Size(50, 755);
            this.p_kiri.TabIndex = 0;
            // 
            // b_SyncStore
            // 
            this.b_SyncStore.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_SyncStore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_SyncStore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_SyncStore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_SyncStore.BorderRadius = 0;
            this.b_SyncStore.ButtonText = "   Sync Store";
            this.b_SyncStore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_SyncStore, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_SyncStore, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_SyncStore, BunifuAnimatorNS.DecorationType.None);
            this.b_SyncStore.DisabledColor = System.Drawing.Color.Gray;
            this.b_SyncStore.Iconcolor = System.Drawing.Color.Transparent;
            this.b_SyncStore.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_SyncStore.Iconimage")));
            this.b_SyncStore.Iconimage_right = null;
            this.b_SyncStore.Iconimage_right_Selected = null;
            this.b_SyncStore.Iconimage_Selected = null;
            this.b_SyncStore.IconMarginLeft = 0;
            this.b_SyncStore.IconMarginRight = 0;
            this.b_SyncStore.IconRightVisible = true;
            this.b_SyncStore.IconRightZoom = 0D;
            this.b_SyncStore.IconVisible = true;
            this.b_SyncStore.IconZoom = 90D;
            this.b_SyncStore.IsTab = true;
            this.b_SyncStore.Location = new System.Drawing.Point(1, 65);
            this.b_SyncStore.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.b_SyncStore.Name = "b_SyncStore";
            this.b_SyncStore.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_SyncStore.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_SyncStore.OnHoverTextColor = System.Drawing.Color.White;
            this.b_SyncStore.selected = false;
            this.b_SyncStore.Size = new System.Drawing.Size(222, 48);
            this.b_SyncStore.TabIndex = 15;
            this.b_SyncStore.Text = "   Sync Store";
            this.b_SyncStore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_SyncStore.Textcolor = System.Drawing.Color.White;
            this.b_SyncStore.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_SyncStore.Click += new System.EventHandler(this.b_SyncStore_Click);
            // 
            // bunifuFlatButton1
            // 
            this.bunifuFlatButton1.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.bunifuFlatButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bunifuFlatButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bunifuFlatButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuFlatButton1.BorderRadius = 0;
            this.bunifuFlatButton1.ButtonText = "   Closing Shift";
            this.bunifuFlatButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.bunifuFlatButton1, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.bunifuFlatButton1, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.bunifuFlatButton1, BunifuAnimatorNS.DecorationType.None);
            this.bunifuFlatButton1.DisabledColor = System.Drawing.Color.Gray;
            this.bunifuFlatButton1.Iconcolor = System.Drawing.Color.Transparent;
            this.bunifuFlatButton1.Iconimage = ((System.Drawing.Image)(resources.GetObject("bunifuFlatButton1.Iconimage")));
            this.bunifuFlatButton1.Iconimage_right = null;
            this.bunifuFlatButton1.Iconimage_right_Selected = null;
            this.bunifuFlatButton1.Iconimage_Selected = null;
            this.bunifuFlatButton1.IconMarginLeft = 0;
            this.bunifuFlatButton1.IconMarginRight = 0;
            this.bunifuFlatButton1.IconRightVisible = true;
            this.bunifuFlatButton1.IconRightZoom = 0D;
            this.bunifuFlatButton1.IconVisible = true;
            this.bunifuFlatButton1.IconZoom = 90D;
            this.bunifuFlatButton1.IsTab = true;
            this.bunifuFlatButton1.Location = new System.Drawing.Point(0, 225);
            this.bunifuFlatButton1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.bunifuFlatButton1.Name = "bunifuFlatButton1";
            this.bunifuFlatButton1.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bunifuFlatButton1.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.bunifuFlatButton1.OnHoverTextColor = System.Drawing.Color.White;
            this.bunifuFlatButton1.selected = false;
            this.bunifuFlatButton1.Size = new System.Drawing.Size(222, 48);
            this.bunifuFlatButton1.TabIndex = 14;
            this.bunifuFlatButton1.Text = "   Closing Shift";
            this.bunifuFlatButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bunifuFlatButton1.Textcolor = System.Drawing.Color.White;
            this.bunifuFlatButton1.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuFlatButton1.Click += new System.EventHandler(this.bunifuFlatButton1_Click_1);
            // 
            // b_petyCash
            // 
            this.b_petyCash.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_petyCash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_petyCash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_petyCash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_petyCash.BorderRadius = 0;
            this.b_petyCash.ButtonText = "   Petty Cash";
            this.b_petyCash.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_petyCash, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_petyCash, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_petyCash, BunifuAnimatorNS.DecorationType.None);
            this.b_petyCash.DisabledColor = System.Drawing.Color.Gray;
            this.b_petyCash.Iconcolor = System.Drawing.Color.Transparent;
            this.b_petyCash.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_petyCash.Iconimage")));
            this.b_petyCash.Iconimage_right = null;
            this.b_petyCash.Iconimage_right_Selected = null;
            this.b_petyCash.Iconimage_Selected = null;
            this.b_petyCash.IconMarginLeft = 0;
            this.b_petyCash.IconMarginRight = 0;
            this.b_petyCash.IconRightVisible = true;
            this.b_petyCash.IconRightZoom = 0D;
            this.b_petyCash.IconVisible = true;
            this.b_petyCash.IconZoom = 90D;
            this.b_petyCash.IsTab = true;
            this.b_petyCash.Location = new System.Drawing.Point(0, 383);
            this.b_petyCash.Name = "b_petyCash";
            this.b_petyCash.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_petyCash.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_petyCash.OnHoverTextColor = System.Drawing.Color.White;
            this.b_petyCash.selected = false;
            this.b_petyCash.Size = new System.Drawing.Size(222, 48);
            this.b_petyCash.TabIndex = 13;
            this.b_petyCash.Text = "   Petty Cash";
            this.b_petyCash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_petyCash.Textcolor = System.Drawing.Color.White;
            this.b_petyCash.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_petyCash.Click += new System.EventHandler(this.b_petyCash_Click);
            // 
            // btn_close
            // 
            this.btn_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.btn_close, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.btn_close, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.btn_close, BunifuAnimatorNS.DecorationType.None);
            this.btn_close.Image = ((System.Drawing.Image)(resources.GetObject("btn_close.Image")));
            this.btn_close.Location = new System.Drawing.Point(181, 10);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(35, 35);
            this.btn_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btn_close.TabIndex = 12;
            this.btn_close.TabStop = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // b_do
            // 
            this.b_do.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_do.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_do.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_do.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_do.BorderRadius = 0;
            this.b_do.ButtonText = "   DO Confirmation";
            this.b_do.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_do, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_do, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_do, BunifuAnimatorNS.DecorationType.None);
            this.b_do.DisabledColor = System.Drawing.Color.Gray;
            this.b_do.Iconcolor = System.Drawing.Color.Transparent;
            this.b_do.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_do.Iconimage")));
            this.b_do.Iconimage_right = null;
            this.b_do.Iconimage_right_Selected = null;
            this.b_do.Iconimage_Selected = null;
            this.b_do.IconMarginLeft = 0;
            this.b_do.IconMarginRight = 0;
            this.b_do.IconRightVisible = true;
            this.b_do.IconRightZoom = 0D;
            this.b_do.IconVisible = true;
            this.b_do.IconZoom = 90D;
            this.b_do.IsTab = true;
            this.b_do.Location = new System.Drawing.Point(0, 707);
            this.b_do.Name = "b_do";
            this.b_do.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_do.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_do.OnHoverTextColor = System.Drawing.Color.White;
            this.b_do.selected = false;
            this.b_do.Size = new System.Drawing.Size(216, 48);
            this.b_do.TabIndex = 11;
            this.b_do.Text = "   DO Confirmation";
            this.b_do.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_do.Textcolor = System.Drawing.Color.White;
            this.b_do.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_do.Click += new System.EventHandler(this.b_do_Click);
            // 
            // b_return
            // 
            this.b_return.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_return.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_return.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_return.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_return.BorderRadius = 0;
            this.b_return.ButtonText = "   Return Order";
            this.b_return.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_return, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_return, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_return, BunifuAnimatorNS.DecorationType.None);
            this.b_return.DisabledColor = System.Drawing.Color.Gray;
            this.b_return.Iconcolor = System.Drawing.Color.Transparent;
            this.b_return.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_return.Iconimage")));
            this.b_return.Iconimage_right = null;
            this.b_return.Iconimage_right_Selected = null;
            this.b_return.Iconimage_Selected = null;
            this.b_return.IconMarginLeft = 0;
            this.b_return.IconMarginRight = 0;
            this.b_return.IconRightVisible = true;
            this.b_return.IconRightZoom = 0D;
            this.b_return.IconVisible = true;
            this.b_return.IconZoom = 90D;
            this.b_return.IsTab = true;
            this.b_return.Location = new System.Drawing.Point(0, 653);
            this.b_return.Name = "b_return";
            this.b_return.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_return.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_return.OnHoverTextColor = System.Drawing.Color.White;
            this.b_return.selected = false;
            this.b_return.Size = new System.Drawing.Size(216, 48);
            this.b_return.TabIndex = 10;
            this.b_return.Text = "   Return Order";
            this.b_return.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_return.Textcolor = System.Drawing.Color.White;
            this.b_return.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_return.Click += new System.EventHandler(this.b_return_Click);
            // 
            // b_mutasi
            // 
            this.b_mutasi.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_mutasi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_mutasi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_mutasi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_mutasi.BorderRadius = 0;
            this.b_mutasi.ButtonText = "   Mutasi Order";
            this.b_mutasi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_mutasi, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_mutasi, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_mutasi, BunifuAnimatorNS.DecorationType.None);
            this.b_mutasi.DisabledColor = System.Drawing.Color.Gray;
            this.b_mutasi.Iconcolor = System.Drawing.Color.Transparent;
            this.b_mutasi.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_mutasi.Iconimage")));
            this.b_mutasi.Iconimage_right = null;
            this.b_mutasi.Iconimage_right_Selected = null;
            this.b_mutasi.Iconimage_Selected = null;
            this.b_mutasi.IconMarginLeft = 0;
            this.b_mutasi.IconMarginRight = 0;
            this.b_mutasi.IconRightVisible = true;
            this.b_mutasi.IconRightZoom = 0D;
            this.b_mutasi.IconVisible = true;
            this.b_mutasi.IconZoom = 90D;
            this.b_mutasi.IsTab = true;
            this.b_mutasi.Location = new System.Drawing.Point(0, 599);
            this.b_mutasi.Name = "b_mutasi";
            this.b_mutasi.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_mutasi.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_mutasi.OnHoverTextColor = System.Drawing.Color.White;
            this.b_mutasi.selected = false;
            this.b_mutasi.Size = new System.Drawing.Size(216, 48);
            this.b_mutasi.TabIndex = 9;
            this.b_mutasi.Text = "   Mutasi Order";
            this.b_mutasi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_mutasi.Textcolor = System.Drawing.Color.White;
            this.b_mutasi.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_mutasi.Click += new System.EventHandler(this.b_mutasi_Click);
            // 
            // b_deliveryCust
            // 
            this.b_deliveryCust.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_deliveryCust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_deliveryCust.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_deliveryCust.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_deliveryCust.BorderRadius = 0;
            this.b_deliveryCust.ButtonText = "   Delivery Customer";
            this.b_deliveryCust.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_deliveryCust, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_deliveryCust, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_deliveryCust, BunifuAnimatorNS.DecorationType.None);
            this.b_deliveryCust.DisabledColor = System.Drawing.Color.Gray;
            this.b_deliveryCust.Iconcolor = System.Drawing.Color.Transparent;
            this.b_deliveryCust.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_deliveryCust.Iconimage")));
            this.b_deliveryCust.Iconimage_right = null;
            this.b_deliveryCust.Iconimage_right_Selected = null;
            this.b_deliveryCust.Iconimage_Selected = null;
            this.b_deliveryCust.IconMarginLeft = 0;
            this.b_deliveryCust.IconMarginRight = 0;
            this.b_deliveryCust.IconRightVisible = true;
            this.b_deliveryCust.IconRightZoom = 0D;
            this.b_deliveryCust.IconVisible = true;
            this.b_deliveryCust.IconZoom = 90D;
            this.b_deliveryCust.IsTab = true;
            this.b_deliveryCust.Location = new System.Drawing.Point(0, 545);
            this.b_deliveryCust.Name = "b_deliveryCust";
            this.b_deliveryCust.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_deliveryCust.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_deliveryCust.OnHoverTextColor = System.Drawing.Color.White;
            this.b_deliveryCust.selected = false;
            this.b_deliveryCust.Size = new System.Drawing.Size(216, 48);
            this.b_deliveryCust.TabIndex = 8;
            this.b_deliveryCust.Text = "   Delivery Customer";
            this.b_deliveryCust.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_deliveryCust.Textcolor = System.Drawing.Color.White;
            this.b_deliveryCust.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_deliveryCust.Click += new System.EventHandler(this.b_deliveryCust_Click);
            // 
            // b_stockTake
            // 
            this.b_stockTake.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_stockTake.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_stockTake.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_stockTake.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_stockTake.BorderRadius = 0;
            this.b_stockTake.ButtonText = "   Stock Take";
            this.b_stockTake.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_stockTake, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_stockTake, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_stockTake, BunifuAnimatorNS.DecorationType.None);
            this.b_stockTake.DisabledColor = System.Drawing.Color.Gray;
            this.b_stockTake.Iconcolor = System.Drawing.Color.Transparent;
            this.b_stockTake.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_stockTake.Iconimage")));
            this.b_stockTake.Iconimage_right = null;
            this.b_stockTake.Iconimage_right_Selected = null;
            this.b_stockTake.Iconimage_Selected = null;
            this.b_stockTake.IconMarginLeft = 0;
            this.b_stockTake.IconMarginRight = 0;
            this.b_stockTake.IconRightVisible = true;
            this.b_stockTake.IconRightZoom = 0D;
            this.b_stockTake.IconVisible = true;
            this.b_stockTake.IconZoom = 90D;
            this.b_stockTake.IsTab = true;
            this.b_stockTake.Location = new System.Drawing.Point(0, 491);
            this.b_stockTake.Name = "b_stockTake";
            this.b_stockTake.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_stockTake.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_stockTake.OnHoverTextColor = System.Drawing.Color.White;
            this.b_stockTake.selected = false;
            this.b_stockTake.Size = new System.Drawing.Size(216, 48);
            this.b_stockTake.TabIndex = 7;
            this.b_stockTake.Text = "   Stock Take";
            this.b_stockTake.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_stockTake.Textcolor = System.Drawing.Color.White;
            this.b_stockTake.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_stockTake.Click += new System.EventHandler(this.b_stockTake_Click);
            // 
            // b_inventory
            // 
            this.b_inventory.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_inventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_inventory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_inventory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_inventory.BorderRadius = 0;
            this.b_inventory.ButtonText = "   Inventory List";
            this.b_inventory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_inventory, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_inventory, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_inventory, BunifuAnimatorNS.DecorationType.None);
            this.b_inventory.DisabledColor = System.Drawing.Color.Gray;
            this.b_inventory.Iconcolor = System.Drawing.Color.Transparent;
            this.b_inventory.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_inventory.Iconimage")));
            this.b_inventory.Iconimage_right = null;
            this.b_inventory.Iconimage_right_Selected = null;
            this.b_inventory.Iconimage_Selected = null;
            this.b_inventory.IconMarginLeft = 0;
            this.b_inventory.IconMarginRight = 0;
            this.b_inventory.IconRightVisible = true;
            this.b_inventory.IconRightZoom = 0D;
            this.b_inventory.IconVisible = true;
            this.b_inventory.IconZoom = 90D;
            this.b_inventory.IsTab = true;
            this.b_inventory.Location = new System.Drawing.Point(0, 437);
            this.b_inventory.Name = "b_inventory";
            this.b_inventory.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_inventory.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_inventory.OnHoverTextColor = System.Drawing.Color.White;
            this.b_inventory.selected = false;
            this.b_inventory.Size = new System.Drawing.Size(222, 48);
            this.b_inventory.TabIndex = 5;
            this.b_inventory.Text = "   Inventory List";
            this.b_inventory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_inventory.Textcolor = System.Drawing.Color.White;
            this.b_inventory.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_inventory.Click += new System.EventHandler(this.b_inventory_Click);
            // 
            // bunifuFlatButton4
            // 
            this.bunifuFlatButton4.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.bunifuFlatButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bunifuFlatButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bunifuFlatButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuFlatButton4.BorderRadius = 0;
            this.bunifuFlatButton4.ButtonText = "   Promotion";
            this.bunifuFlatButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.bunifuFlatButton4, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.bunifuFlatButton4, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.bunifuFlatButton4, BunifuAnimatorNS.DecorationType.None);
            this.bunifuFlatButton4.DisabledColor = System.Drawing.Color.Gray;
            this.bunifuFlatButton4.Iconcolor = System.Drawing.Color.Transparent;
            this.bunifuFlatButton4.Iconimage = ((System.Drawing.Image)(resources.GetObject("bunifuFlatButton4.Iconimage")));
            this.bunifuFlatButton4.Iconimage_right = null;
            this.bunifuFlatButton4.Iconimage_right_Selected = null;
            this.bunifuFlatButton4.Iconimage_Selected = null;
            this.bunifuFlatButton4.IconMarginLeft = 0;
            this.bunifuFlatButton4.IconMarginRight = 0;
            this.bunifuFlatButton4.IconRightVisible = true;
            this.bunifuFlatButton4.IconRightZoom = 0D;
            this.bunifuFlatButton4.IconVisible = true;
            this.bunifuFlatButton4.IconZoom = 90D;
            this.bunifuFlatButton4.IsTab = true;
            this.bunifuFlatButton4.Location = new System.Drawing.Point(0, 329);
            this.bunifuFlatButton4.Name = "bunifuFlatButton4";
            this.bunifuFlatButton4.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bunifuFlatButton4.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.bunifuFlatButton4.OnHoverTextColor = System.Drawing.Color.White;
            this.bunifuFlatButton4.selected = false;
            this.bunifuFlatButton4.Size = new System.Drawing.Size(222, 48);
            this.bunifuFlatButton4.TabIndex = 4;
            this.bunifuFlatButton4.Text = "   Promotion";
            this.bunifuFlatButton4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bunifuFlatButton4.Textcolor = System.Drawing.Color.White;
            this.bunifuFlatButton4.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuFlatButton4.Click += new System.EventHandler(this.bunifuFlatButton4_Click);
            // 
            // bunifuFlatButton3
            // 
            this.bunifuFlatButton3.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.bunifuFlatButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bunifuFlatButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bunifuFlatButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuFlatButton3.BorderRadius = 0;
            this.bunifuFlatButton3.ButtonText = "   Closing Store";
            this.bunifuFlatButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.bunifuFlatButton3, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.bunifuFlatButton3, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.bunifuFlatButton3, BunifuAnimatorNS.DecorationType.None);
            this.bunifuFlatButton3.DisabledColor = System.Drawing.Color.Gray;
            this.bunifuFlatButton3.Iconcolor = System.Drawing.Color.Transparent;
            this.bunifuFlatButton3.Iconimage = ((System.Drawing.Image)(resources.GetObject("bunifuFlatButton3.Iconimage")));
            this.bunifuFlatButton3.Iconimage_right = null;
            this.bunifuFlatButton3.Iconimage_right_Selected = null;
            this.bunifuFlatButton3.Iconimage_Selected = null;
            this.bunifuFlatButton3.IconMarginLeft = 0;
            this.bunifuFlatButton3.IconMarginRight = 0;
            this.bunifuFlatButton3.IconRightVisible = true;
            this.bunifuFlatButton3.IconRightZoom = 0D;
            this.bunifuFlatButton3.IconVisible = true;
            this.bunifuFlatButton3.IconZoom = 90D;
            this.bunifuFlatButton3.IsTab = true;
            this.bunifuFlatButton3.Location = new System.Drawing.Point(0, 275);
            this.bunifuFlatButton3.Name = "bunifuFlatButton3";
            this.bunifuFlatButton3.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bunifuFlatButton3.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.bunifuFlatButton3.OnHoverTextColor = System.Drawing.Color.White;
            this.bunifuFlatButton3.selected = false;
            this.bunifuFlatButton3.Size = new System.Drawing.Size(222, 48);
            this.bunifuFlatButton3.TabIndex = 3;
            this.bunifuFlatButton3.Text = "   Closing Store";
            this.bunifuFlatButton3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bunifuFlatButton3.Textcolor = System.Drawing.Color.White;
            this.bunifuFlatButton3.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuFlatButton3.Click += new System.EventHandler(this.bunifuFlatButton3_Click);
            // 
            // bunifuFlatButton2
            // 
            this.bunifuFlatButton2.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.bunifuFlatButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bunifuFlatButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bunifuFlatButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuFlatButton2.BorderRadius = 0;
            this.bunifuFlatButton2.ButtonText = "   Transaction";
            this.bunifuFlatButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.bunifuFlatButton2, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.bunifuFlatButton2, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.bunifuFlatButton2, BunifuAnimatorNS.DecorationType.None);
            this.bunifuFlatButton2.DisabledColor = System.Drawing.Color.Gray;
            this.bunifuFlatButton2.Iconcolor = System.Drawing.Color.Transparent;
            this.bunifuFlatButton2.Iconimage = ((System.Drawing.Image)(resources.GetObject("bunifuFlatButton2.Iconimage")));
            this.bunifuFlatButton2.Iconimage_right = null;
            this.bunifuFlatButton2.Iconimage_right_Selected = null;
            this.bunifuFlatButton2.Iconimage_Selected = null;
            this.bunifuFlatButton2.IconMarginLeft = 0;
            this.bunifuFlatButton2.IconMarginRight = 0;
            this.bunifuFlatButton2.IconRightVisible = true;
            this.bunifuFlatButton2.IconRightZoom = 0D;
            this.bunifuFlatButton2.IconVisible = true;
            this.bunifuFlatButton2.IconZoom = 90D;
            this.bunifuFlatButton2.IsTab = true;
            this.bunifuFlatButton2.Location = new System.Drawing.Point(0, 124);
            this.bunifuFlatButton2.Name = "bunifuFlatButton2";
            this.bunifuFlatButton2.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.bunifuFlatButton2.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.bunifuFlatButton2.OnHoverTextColor = System.Drawing.Color.White;
            this.bunifuFlatButton2.selected = false;
            this.bunifuFlatButton2.Size = new System.Drawing.Size(222, 48);
            this.bunifuFlatButton2.TabIndex = 2;
            this.bunifuFlatButton2.Text = "   Transaction";
            this.bunifuFlatButton2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bunifuFlatButton2.Textcolor = System.Drawing.Color.White;
            this.bunifuFlatButton2.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuFlatButton2.Click += new System.EventHandler(this.bunifuFlatButton2_Click);
            // 
            // b_transaction
            // 
            this.b_transaction.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_transaction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_transaction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_transaction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.b_transaction.BorderRadius = 0;
            this.b_transaction.ButtonText = "   Transaction History";
            this.b_transaction.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.b_transaction, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.b_transaction, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.b_transaction, BunifuAnimatorNS.DecorationType.None);
            this.b_transaction.DisabledColor = System.Drawing.Color.Gray;
            this.b_transaction.Iconcolor = System.Drawing.Color.Transparent;
            this.b_transaction.Iconimage = ((System.Drawing.Image)(resources.GetObject("b_transaction.Iconimage")));
            this.b_transaction.Iconimage_right = null;
            this.b_transaction.Iconimage_right_Selected = null;
            this.b_transaction.Iconimage_Selected = null;
            this.b_transaction.IconMarginLeft = 0;
            this.b_transaction.IconMarginRight = 0;
            this.b_transaction.IconRightVisible = true;
            this.b_transaction.IconRightZoom = 0D;
            this.b_transaction.IconVisible = true;
            this.b_transaction.IconZoom = 90D;
            this.b_transaction.IsTab = true;
            this.b_transaction.Location = new System.Drawing.Point(0, 175);
            this.b_transaction.Name = "b_transaction";
            this.b_transaction.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.b_transaction.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.b_transaction.OnHoverTextColor = System.Drawing.Color.White;
            this.b_transaction.selected = false;
            this.b_transaction.Size = new System.Drawing.Size(222, 48);
            this.b_transaction.TabIndex = 1;
            this.b_transaction.Text = "   Transaction History";
            this.b_transaction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_transaction.Textcolor = System.Drawing.Color.White;
            this.b_transaction.TextFont = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_transaction.Click += new System.EventHandler(this.bunifuFlatButton1_Click);
            // 
            // btn_maax
            // 
            this.btn_maax.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.btn_maax, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.btn_maax, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.btn_maax, BunifuAnimatorNS.DecorationType.None);
            this.btn_maax.Image = ((System.Drawing.Image)(resources.GetObject("btn_maax.Image")));
            this.btn_maax.Location = new System.Drawing.Point(9, 10);
            this.btn_maax.Name = "btn_maax";
            this.btn_maax.Size = new System.Drawing.Size(35, 35);
            this.btn_maax.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btn_maax.TabIndex = 0;
            this.btn_maax.TabStop = false;
            this.btn_maax.Click += new System.EventHandler(this.btn_maax_Click);
            // 
            // p_atas
            // 
            this.p_atas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.p_atas.Controls.Add(this.menu_inout);
            this.p_atas.Controls.Add(this.t_nama);
            this.p_atas.Controls.Add(this.label3);
            this.p_atas.Controls.Add(this.pic_profil);
            this.p_atas.Controls.Add(this.label_title);
            this.LogoMax.SetDecoration(this.p_atas, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.p_atas, BunifuAnimatorNS.DecorationType.None);
            this.AnimatorKiri.SetDecoration(this.p_atas, BunifuAnimatorNS.DecorationType.None);
            this.p_atas.Dock = System.Windows.Forms.DockStyle.Top;
            this.p_atas.Location = new System.Drawing.Point(0, 0);
            this.p_atas.Name = "p_atas";
            this.p_atas.Size = new System.Drawing.Size(1264, 49);
            this.p_atas.TabIndex = 1;
            // 
            // menu_inout
            // 
            this.menu_inout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.menu_inout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.menu_inout, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.menu_inout, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.menu_inout, BunifuAnimatorNS.DecorationType.None);
            this.menu_inout.Image = ((System.Drawing.Image)(resources.GetObject("menu_inout.Image")));
            this.menu_inout.Location = new System.Drawing.Point(6, 4);
            this.menu_inout.Name = "menu_inout";
            this.menu_inout.Size = new System.Drawing.Size(40, 40);
            this.menu_inout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.menu_inout.TabIndex = 27;
            this.menu_inout.TabStop = false;
            this.menu_inout.Click += new System.EventHandler(this.menu_inout_Click);
            // 
            // t_nama
            // 
            this.t_nama.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.t_nama.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.t_nama.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AnimatorKiri.SetDecoration(this.t_nama, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.t_nama, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.t_nama, BunifuAnimatorNS.DecorationType.None);
            this.t_nama.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_nama.ForeColor = System.Drawing.Color.White;
            this.t_nama.Location = new System.Drawing.Point(792, 13);
            this.t_nama.Name = "t_nama";
            this.t_nama.Size = new System.Drawing.Size(390, 24);
            this.t_nama.TabIndex = 26;
            this.t_nama.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.AnimatorKiri.SetDecoration(this.label3, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.label3, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.label3, BunifuAnimatorNS.DecorationType.None);
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(251, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Transaction";
            // 
            // pic_profil
            // 
            this.pic_profil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pic_profil.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnimatorKiri.SetDecoration(this.pic_profil, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.pic_profil, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.pic_profil, BunifuAnimatorNS.DecorationType.None);
            this.pic_profil.Image = ((System.Drawing.Image)(resources.GetObject("pic_profil.Image")));
            this.pic_profil.Location = new System.Drawing.Point(1206, 3);
            this.pic_profil.Name = "pic_profil";
            this.pic_profil.Size = new System.Drawing.Size(40, 40);
            this.pic_profil.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_profil.TabIndex = 2;
            this.pic_profil.TabStop = false;
            this.pic_profil.Click += new System.EventHandler(this.pic_profil_Click);
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.AnimatorKiri.SetDecoration(this.label_title, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.label_title, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this.label_title, BunifuAnimatorNS.DecorationType.None);
            this.label_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_title.ForeColor = System.Drawing.Color.White;
            this.label_title.Location = new System.Drawing.Point(48, 13);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(131, 20);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "BIENSI V1.3 - ";
            // 
            // p_kanan
            // 
            this.p_kanan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(221)))), ((int)(((byte)(222)))));
            this.LogoMax.SetDecoration(this.p_kanan, BunifuAnimatorNS.DecorationType.None);
            this.LogoClose.SetDecoration(this.p_kanan, BunifuAnimatorNS.DecorationType.None);
            this.AnimatorKiri.SetDecoration(this.p_kanan, BunifuAnimatorNS.DecorationType.None);
            this.p_kanan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p_kanan.Location = new System.Drawing.Point(50, 49);
            this.p_kanan.Name = "p_kanan";
            this.p_kanan.Size = new System.Drawing.Size(1214, 755);
            this.p_kanan.TabIndex = 2;
            // 
            // LogoMax
            // 
            this.LogoMax.AnimationType = BunifuAnimatorNS.AnimationType.Particles;
            this.LogoMax.Cursor = null;
            animation3.AnimateOnlyDifferences = true;
            animation3.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation3.BlindCoeff")));
            animation3.LeafCoeff = 0F;
            animation3.MaxTime = 1F;
            animation3.MinTime = 0F;
            animation3.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation3.MosaicCoeff")));
            animation3.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation3.MosaicShift")));
            animation3.MosaicSize = 1;
            animation3.Padding = new System.Windows.Forms.Padding(100, 50, 100, 150);
            animation3.RotateCoeff = 0F;
            animation3.RotateLimit = 0F;
            animation3.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation3.ScaleCoeff")));
            animation3.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation3.SlideCoeff")));
            animation3.TimeCoeff = 2F;
            animation3.TransparencyCoeff = 0F;
            this.LogoMax.DefaultAnimation = animation3;
            // 
            // LogoClose
            // 
            this.LogoClose.AnimationType = BunifuAnimatorNS.AnimationType.Particles;
            this.LogoClose.Cursor = null;
            animation2.AnimateOnlyDifferences = true;
            animation2.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.BlindCoeff")));
            animation2.LeafCoeff = 0F;
            animation2.MaxTime = 1F;
            animation2.MinTime = 0F;
            animation2.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.MosaicCoeff")));
            animation2.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation2.MosaicShift")));
            animation2.MosaicSize = 1;
            animation2.Padding = new System.Windows.Forms.Padding(100, 50, 100, 150);
            animation2.RotateCoeff = 0F;
            animation2.RotateLimit = 0F;
            animation2.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.ScaleCoeff")));
            animation2.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.SlideCoeff")));
            animation2.TimeCoeff = 2F;
            animation2.TransparencyCoeff = 0F;
            this.LogoClose.DefaultAnimation = animation2;
            // 
            // AnimatorKiri
            // 
            this.AnimatorKiri.AnimationType = BunifuAnimatorNS.AnimationType.Particles;
            this.AnimatorKiri.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 1;
            animation1.Padding = new System.Windows.Forms.Padding(100, 50, 100, 150);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 2F;
            animation1.TransparencyCoeff = 0F;
            this.AnimatorKiri.DefaultAnimation = animation1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1264, 804);
            this.Controls.Add(this.p_kanan);
            this.Controls.Add(this.p_kiri);
            this.Controls.Add(this.p_atas);
            this.LogoClose.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.LogoMax.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.AnimatorKiri.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.p_kiri.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_maax)).EndInit();
            this.p_atas.ResumeLayout(false);
            this.p_atas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menu_inout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_profil)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel p_kiri;
        private System.Windows.Forms.Panel p_atas;
        private System.Windows.Forms.PictureBox pic_profil;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.PictureBox btn_maax;
        private Bunifu.Framework.UI.BunifuFlatButton b_transaction;
        private Bunifu.Framework.UI.BunifuFlatButton bunifuFlatButton3;
        private Bunifu.Framework.UI.BunifuFlatButton bunifuFlatButton2;        
        private Bunifu.Framework.UI.BunifuFlatButton b_do;
        private Bunifu.Framework.UI.BunifuFlatButton b_return;
        private Bunifu.Framework.UI.BunifuFlatButton b_mutasi;
        private Bunifu.Framework.UI.BunifuFlatButton b_deliveryCust;
        private Bunifu.Framework.UI.BunifuFlatButton b_stockTake;
        private Bunifu.Framework.UI.BunifuFlatButton b_inventory;
        private Bunifu.Framework.UI.BunifuFlatButton bunifuFlatButton4;
        private System.Windows.Forms.PictureBox btn_close;
        private BunifuAnimatorNS.BunifuTransition AnimatorKiri;
        private BunifuAnimatorNS.BunifuTransition LogoClose;
        private BunifuAnimatorNS.BunifuTransition LogoMax;
        public System.Windows.Forms.Panel p_kanan;
        private Bunifu.Framework.UI.BunifuFlatButton b_petyCash;
        private Bunifu.Framework.UI.BunifuFlatButton bunifuFlatButton1;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox t_nama;
        private System.Windows.Forms.PictureBox menu_inout;
        private Bunifu.Framework.UI.BunifuFlatButton b_SyncStore;
    }
}

