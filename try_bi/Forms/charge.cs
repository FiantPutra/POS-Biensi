using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data.SqlClient;
using try_bi.Class;

namespace try_bi
{
    public partial class charge : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        public String l_trans, store_code, bulan_now, tahun_now, bulan_trans, number_trans_string, final_running_number, empl_Id, empl_Name;
        public int total, the_real_diskon, get_voucher2, tot_diskon2, number_trans;
        Double count;
        //string result;
        List<double> iList = new List<double>();
        List<double> iList2 = new List<double>();
        private static charge _instance;
        public static charge Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new charge(f1);
                return _instance;
            }
        }
        public charge(Form1 form1)
        {
            // iList2.Clear();
            f1 = form1;
            InitializeComponent();
           

        }
        //======================TERIMA ID DARI FORM SEBELUMNYA===========================
        public void id_trans(String id, int harga, int diskon, int voucher, int dapat_diskon, string cust_Id)
        {
            t_cust.Text = cust_Id;
            this.ActiveControl = t_cust;
            t_cust.Focus();
            the_real_diskon = diskon;//dapat dari variabel get_dis_vou dari form sebelumnya
            get_voucher2 = voucher;//mengambil nilai voucher dari trans header
            tot_diskon2 = dapat_diskon;//mengambil total diskon dari transaction line dari form sebelumnya            
            //====PERCABANGAN UNTUK MENENTUKAN KOMA DI LABEL DISKON=====
            if(tot_diskon2==0)
            { l_diskon2.Text = "0,00"; }
            else
            { l_diskon2.Text = string.Format("{0:#,###}" + ",00", tot_diskon2); }
            //======PERCABANGAN UNTUK MENENTUKAN KOMA DI LABEL VOUCHER====
            if(get_voucher2==0)
            { l_voucher.Text = "0,00"; }
            else
            { l_voucher.Text = string.Format("{0:#,###}" + ",00", get_voucher2); }
            //if(diskon == 0)
            //{
            //    l_diskon2.Text = "0,00";
            //}
            //else
            //{ l_diskon2.Text = string.Format("{0:#,###}" + ",00", diskon); }
            l_transaksi2.Text = id;
            l_total.Text = string.Format("{0:#,###}"+",00", harga);
            l_total2.Text = string.Format("{0:#,###}"+",00", harga);
            retreive();
            total = harga;
        }
        //===============================================================================

        //======================GET DATA FROM ID TRANSAKSI===========================
        public void get_data_id(String id)
        {
            string command;                        

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM [transaction] a JOIN employee b ON a.EMPLOYEE_ID = b.EMPLOYEE_ID "
                            + "WHERE a.TRANSACTION_ID='" + id + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        t_cust.Text = ckon.sqlDataRd["CUSTOMER_ID"].ToString();                        
                        t_spg.Text = ckon.sqlDataRd["SPG_ID"].ToString();
                        store_code = ckon.sqlDataRd["STORE_CODE"].ToString();
                        empl_Id = ckon.sqlDataRd["EMPLOYEE_ID"].ToString();
                        empl_Name = ckon.sqlDataRd["NAME"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
        //==========================================================================

        //==================TAMPILKAN DATA KERANJANG BELANJA================================
        public void retreive()
        {
            String art_id, art_name, spg_id, qty, disc_desc, sub_total2, deliveryAddress, courier, fromStore;
            int discAmount, disc, price, sub_total;
            string command;

            dgv_purchase2a.Rows.Clear();
            dgv_shipping.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                command = "SELECT transaction_line.QUANTITY, transaction_line.SUBTOTAL,transaction_line.SPG_ID,transaction_line.DISCOUNT,transaction_line.DISCOUNT_DESC, "
                            + "transaction_line.DELIVERYCUSTADDRESS, transaction_line.OMNICOURIER, transaction_line.OMNISTORECODE, transaction_line.DISCOUNTAMOUNT, "
                            + "article.ARTICLE_ID ,article.ARTICLE_NAME, article.PRICE FROM transaction_line, article "
                            + "WHERE article.ARTICLE_ID = transaction_line.ARTICLE_ID AND transaction_line.TRANSACTION_ID='" + l_transaksi2.Text + "' ORDER BY transaction_line._id ASC";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        spg_id = ckon.sqlDataRd["SPG_ID"].ToString();
                        //size = ckon.sqlDataRd["SIZE"].ToString();
                        //color = ckon.sqlDataRd["COLOR"].ToString();
                        discAmount = Convert.ToInt32(ckon.sqlDataRd["DISCOUNTAMOUNT"].ToString());
                        price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                        //disc_desc = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();
                        sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                        deliveryAddress = ckon.sqlDataRd["DELIVERYCUSTADDRESS"].ToString();
                        courier = ckon.sqlDataRd["OMNICOURIER"].ToString();
                        fromStore = ckon.sqlDataRd["OMNISTORECODE"].ToString();

                        if (sub_total == 0)
                        {
                            sub_total2 = "0,00";
                            int dgRows = dgv_purchase2a.Rows.Add();
                            dgv_purchase2a.Rows[dgRows].Cells[0].Value = art_id;
                            dgv_purchase2a.Rows[dgRows].Cells[1].Value = art_name;
                            dgv_purchase2a.Rows[dgRows].Cells[2].Value = spg_id;
                            dgv_purchase2a.Rows[dgRows].Cells[3].Value = qty;
                            dgv_purchase2a.Rows[dgRows].Cells[4].Value = price;                            
                            dgv_purchase2a.Rows[dgRows].Cells[5].Value = disc;
                            dgv_purchase2a.Rows[dgRows].Cells[6].Value = discAmount;
                            dgv_purchase2a.Rows[dgRows].Cells[7].Value = sub_total2;
                        }
                        else
                        {
                            int dgRows = dgv_purchase2a.Rows.Add();
                            dgv_purchase2a.Rows[dgRows].Cells[0].Value = art_id;
                            dgv_purchase2a.Rows[dgRows].Cells[1].Value = art_name;
                            dgv_purchase2a.Rows[dgRows].Cells[2].Value = spg_id;
                            dgv_purchase2a.Rows[dgRows].Cells[3].Value = qty;
                            dgv_purchase2a.Rows[dgRows].Cells[4].Value = price;
                            dgv_purchase2a.Rows[dgRows].Cells[5].Value = disc;
                            dgv_purchase2a.Rows[dgRows].Cells[6].Value = discAmount;
                            dgv_purchase2a.Rows[dgRows].Cells[7].Value = sub_total;
                        }

                        if (deliveryAddress != "0")
                        {
                            int dgRowsShipping = dgv_shipping.Rows.Add();
                            dgv_shipping.Rows[dgRowsShipping].Cells[0].Value = art_id;
                            dgv_shipping.Rows[dgRowsShipping].Cells[1].Value = art_name;
                            dgv_shipping.Rows[dgRowsShipping].Cells[2].Value = fromStore;
                            dgv_shipping.Rows[dgRowsShipping].Cells[3].Value = deliveryAddress;                            
                        }                       
                    }

                    dgv_purchase2a.Columns[4].DefaultCellStyle.Format = "#,###";
                    dgv_purchase2a.Columns[5].DefaultCellStyle.Format = "#,###";
                    dgv_purchase2a.Columns[6].DefaultCellStyle.Format = "#,###";
                    dgv_purchase2a.Columns[7].DefaultCellStyle.Format = "#,###";
                    //dgv_purchase2a.Columns[8].DefaultCellStyle.Format = "#,###";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
        //===============================================================================

        
        //=================FORM LOAD=====================================================
        private void charge_Load(object sender, EventArgs e)
        {
            l_transaksi2.Text = l_trans;
        }
        //===============================================================================

        //=============================SETUP QUICK CASK==============================================
        public void QuickCash()
        {
            string command;            

            btnArray = new System.Windows.Forms.Button[0];
            //ckon.con.Close();
            Double new_total = System.Convert.ToDouble(total);
            Double no_koma;            

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM denomination ORDER BY NOMINAL ASC";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        iList.Add(System.Convert.ToDouble(ckon.sqlDataRd["NOMINAL"]));
                    }

                    for (int i = 0; i < iList.Count; i++)
                    {
                        count = new_total / iList[i];
                        no_koma = Math.Floor(count);

                        Double Sugestion;
                        if (i == 0)
                        {
                            Sugestion = new_total;
                        }
                        else
                        {
                            Sugestion = no_koma * iList[i];
                            Sugestion = Sugestion + iList[i];
                        }
                        //ADD ITEM TO LIST2
                        iList2.Add(Sugestion);

                        //REMOVE SAME ITEM IN LIST2
                        int index = 0;
                        while (index < iList2.Count - 1)
                        {
                            if (iList2[index] == iList2[index + 1])
                            { iList2.RemoveAt(index); }
                            else
                            {
                                index++;
                            }                            
                        }                                                
                        AddButtons2();                        
                    }
                    //ASLINYA DISNI
                    iList2.Clear();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                iList.Clear();
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
        //===========================================================================================
        //=============================================================================================================================
        System.Windows.Forms.Button[] btnArray;

        private void b_cancelShipping_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            String cmd_deleteLine = "DELETE FROM deliverycustomer_line WHERE DELIVERY_CUST_ID = (SELECT DELIVERY_CUT_ID FROM deliverycustomer WHERE TRANSACTION_ID = '" + l_transaksi2.Text + "')";
            sql.ExecuteNonQuery(cmd_deleteLine);

            String cmd_deleteHeader = "DELETE FROM deliverycustomer WHERE TRANSACTION_ID = '" + l_transaksi2.Text + "'";
            sql.ExecuteNonQuery(cmd_deleteHeader);
        }

        public void AddButtons2()
        {
            int xPos = 0;
            int yPos = 0;
            // Declare and assign number of buttons = 26 
           btnArray = new System.Windows.Forms.Button[iList2.Count];
           // pnlButtons.Invalidate();
            pnlButtons.Controls.Clear();
            //System.Windows.Forms.FlatButtonAppearance
            // Create (26) Buttons: 
            for (int i = 0; i < iList2.Count; i++)
            {
                // Initialize one variable 
                btnArray[i] = new System.Windows.Forms.Button();
            }
            int n = 0;

            while (n < iList2.Count)
            {
                btnArray[n].Tag = n + 1; // Tag of button 
                btnArray[n].Width = 120; // Width of button 
                btnArray[n].Height = 60; // Height of button 
                if (n == 4) // Location of second line of buttons: 
                {
                    xPos = 0;
                    yPos = 80;
                }
                // Location of button: 
                btnArray[n].Left = xPos;
                btnArray[n].Top = yPos;
                // Add buttons to a Panel: 
                pnlButtons.Controls.Add(btnArray[n]); // Let panel hold the Buttons 
                xPos = xPos + btnArray[n].Width; // Left of next button 
                                                 // Write English Character: 
                Double value = double.Parse(iList2[n].ToString());
                String value2 = string.Format("{0:#,###}", value);
                btnArray[n].Text = value2;
                btnArray[n].Font = new Font("Arial", 14, FontStyle.Bold);
                btnArray[n].BackColor = Color.White;

                /* **************************************************************** 
                You can use following code instead previous line 
                char[] Alphabet = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 
                'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 
                'W', 'X', 'Y', 'Z'}; btnArray[n].Text = Alphabet[n].ToString(); 
                //**************************************************************** */

                // the Event of click Button 
                btnArray[n].Click += new System.EventHandler(ClickButton2);
                n++;

            }
            //}

            //btnAddButton.Enabled = false; // not need now to this button now 
            //label1.Visible = true;
        }
        //=============================================================================================================================
        public void ClickButton2(Object sender, System.EventArgs e)
        {
            //==MENGEMBALIKAN FOCUS KE TEXTBOXT CUSTOMER ID
            this.ActiveControl = t_cust;
            t_cust.Focus();
            //============================================
            Button btn = (Button)sender;
            String a = btn.Text;
            String b = "quick_cash";
            //MessageBox.Show("You clicked character [" + a + "]");
            payment py = new payment(f1);
            py.quick_cash(a);
            py.aksi_total(total, b, the_real_diskon, tot_diskon2);
            //py.textBox1.Text = "aaaaa";
            py.id_trans = l_transaksi2.Text;
            py.new_total = l_total2.Text;
            py.cust_Id = t_cust.Text;
            py.ShowDialog();
        }
        //==================BUTTON BACK BARU WITH UNDERLINE============================
        private void b_back2_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            f1.p_kanan.Controls.Clear();
            //bunifuFlatButton1.selected = true;
            if (!f1.p_kanan.Controls.Contains(uc_coba.Instance))
            {
                f1.p_kanan.Controls.Add(uc_coba.Instance);
                uc_coba.Instance.Dock = DockStyle.Fill;
                //charge.Instance.id_trans(l_transaksi.Text, l_total.Text);
                //uc_coba.Instance.Show();
                uc_coba.Instance.barcode_fokus();
                uc_coba.Instance.BringToFront();
            }
            else
            {
                //charge.Instance.Show();
                uc_coba.Instance.barcode_fokus();
                uc_coba.Instance.BringToFront();
            }

            String del_trans_line = "DELETE FROM transaction_line WHERE TRANSACTION_ID = '" + l_transaksi2.Text + "'";
            sql.ExecuteNonQuery(del_trans_line);
        }
        //===========BUTTON IMAGE SPLIT EDC ============================================
        private void b_Split_edc_Click(object sender, EventArgs e)
        {
            w_split_edc s_edc = new w_split_edc(f1);
            s_edc.get_nilai(total, l_transaksi2.Text, the_real_diskon, tot_diskon2);
            s_edc.cust_Id = t_cust.Text;
            s_edc.ShowDialog();
        }
        //=================BUTTON IMAGE SPLIT ===========================================
        private void b_split2_Click(object sender, EventArgs e)
        {
            w_split split = new w_split(f1);
            split.get_nilai(total, l_transaksi2.Text, the_real_diskon, tot_diskon2);
            split.cust_Id = t_cust.Text;
            split.ShowDialog();
        }

        private void b_confirmShipping_Click(object sender, EventArgs e)
        {
            String art_id, art_name, qty, deliveryAddress, courier, fromStore, deliveryType, deliveryCustId = "";            
            string command;
            CRUD sql = new CRUD();
            bool isValid = true, apiResponse;
            API_DeliveryCustomer delivCust = new API_DeliveryCustomer();

            try
            {
                for (int i = 0; i < dgv_shipping.Rows.Count; i++)
                {                    
                    string type = Convert.ToString(dgv_shipping.Rows[i].Cells["Type"].EditedFormattedValue);
                    string omniStore = Convert.ToString(dgv_shipping.Rows[i].Cells["FromStore"].Value);
                    string articleId = Convert.ToString(dgv_shipping.Rows[i].Cells["Article_Id"].Value);

                    if (type == "")
                    {
                        isValid = false;
                        MessageBox.Show("Please select type in shipping details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    else
                    {
                        String cmd_update = "UPDATE transaction_line SET DELIVERYTYPE = '" + type + "' "
                                            + "WHERE TRANSACTION_ID = '" + l_transaksi2.Text + "' AND OMNISTORECODE = '" + omniStore + "' AND ARTICLE_ID = '" + articleId + "'";
                        sql.ExecuteNonQuery(cmd_update); 
                    }
                }
                
                if (isValid)
                {
                    ckon.sqlCon().Open();
                    command = "SELECT DELIVERY_CUST_ID FROM deliverycustomer WHERE Status = '0'";

                    ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            deliveryCustId = ckon.sqlDataRd["DELIVERY_CUST_ID"].ToString();
                        }
                    }


                    if (deliveryCustId == "")
                    {
                        save_deliveryCust_header();
                        deliveryCustId = final_running_number;

                        command = "SELECT transaction_line.QUANTITY, transaction_line.DELIVERYCUSTADDRESS, transaction_line.OMNICOURIER, transaction_line.OMNISTORECODE, "
                                    + "transaction_line.ARTICLE_ID, transaction_line.DELIVERYTYPE FROM transaction_line "
                                    + "WHERE transaction_line.TRANSACTION_ID = '" + l_transaksi2.Text + "' AND transaction_line.OMNISTORECODE != '' ORDER BY transaction_line._id ASC";

                        ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                        if (ckon.sqlDataRd.HasRows)
                        {
                            while (ckon.sqlDataRd.Read())
                            {
                                art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                                qty = ckon.sqlDataRd["QUANTITY"].ToString();
                                deliveryAddress = ckon.sqlDataRd["DELIVERYCUSTADDRESS"].ToString();
                                courier = ckon.sqlDataRd["OMNICOURIER"].ToString();
                                fromStore = ckon.sqlDataRd["OMNISTORECODE"].ToString();
                                deliveryType = ckon.sqlDataRd["DELIVERYTYPE"].ToString();

                                String cmd_insert = "INSERT INTO deliverycustomer_line (DELIVERY_CUST_ID, ARTICLE_ID, QTY, STORE_FROM, STORE_TO, NO_RESI, COURIER, DELIVERYADDRESS, DELIVERYTYPE) VALUES ('" + deliveryCustId + "','" + art_id + "','" + qty + "','" + fromStore + "','" + store_code + "', '','" + courier + "', '" + deliveryAddress + "','" + deliveryType + "') ";
                                sql.ExecuteNonQuery(cmd_insert);
                            }

                            String cmd_update = "UPDATE deliverycustomer SET TOTAL_QTY = (SELECT SUM(QTY) FROM deliverycustomer_line WHERE DELIVERY_CUST_ID = '" + deliveryCustId + "') "
                                                + "WHERE DELIVERY_CUST_ID = '" + deliveryCustId + "'";
                            sql.ExecuteNonQuery(cmd_update);
                        }
                        else
                        {
                            MessageBox.Show("No shipping", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }                    

                    apiResponse = delivCust.deliveryCustPost(deliveryCustId).Result;
                    if (apiResponse)
                    {
                        MessageBox.Show("Shipping confirmed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Make sure you are connected to internet and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }                                          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                iList.Clear();
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }

        //===============BUTTON IMAGE EDC ==============================================
        private void b_edc2_Click(object sender, EventArgs e)
        {
            w_edc edc = new w_edc(f1);
            edc.aksi_total(total, the_real_diskon, tot_diskon2);
            edc.id_trans = l_transaksi2.Text;
            edc.cust_Id = t_cust.Text;
            edc.ShowDialog();
        }
        //==================BUTTON IMAGE CASH ==========================================
        private void b_cash2_Click(object sender, EventArgs e)
        {
            String a = "cash";
            payment py = new payment(f1);
            py.aksi_total(total, a, the_real_diskon, tot_diskon2);
            py.id_trans = l_transaksi2.Text;
            //py.new_total = l_total2.Text;
            py.cust_Id = t_cust.Text;
            py.ShowDialog();
        }
        //===============================================================================
        private void t_cust_KeyDown(object sender, KeyEventArgs e)
        {
            //==========BUTTON CASH==============
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_cash2_Click(null, null);
            }
            //======BUTTON EDC====================
            if (e.Control && e.KeyCode.ToString() == "E")
            {
                b_edc2_Click(null, null);
            }
            //=============BUTTON SPLIT=============
            if (e.Control && e.KeyCode.ToString() == "S")
            {
                b_split2_Click(null, null);
            }
            //======BUTTON SPLIT EDC===============
            if (e.Control && e.KeyCode.ToString() == "T")
            {
                b_Split_edc_Click(null, null);
            }
            //==============BUTTON BACK==============
            if (e.Control && e.KeyCode.ToString() == "B")
            {
                b_back2_Click(null, null);
            }
        }
        //===============================================================================
        
        //====METHOD GET MOUNT AND YEAR PRESENT=================
        public void get_year_month()
        {
            DateTime mydate = DateTime.Now;
            DateTime myhour = DateTime.Now;

            bulan_now = mydate.ToString("MM");
            tahun_now = mydate.ToString("yy");
        }
        //=========METHOD GET DATA FROM AUTO_NUMBER TABLE FOR DELIVERY CUST
        public void get_running_number()
        {
            CRUD sql = new CRUD();            

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 Number FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '10' AND Month='" + bulan_now + "' AND Year='" + tahun_now + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        number_trans = Convert.ToInt32(ckon.sqlDataRd["Number"].ToString());
                    }

                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');

                    if (Properties.Settings.Default.DevCode == "null")
                        final_running_number = "DC/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                    else
                        final_running_number = "DC/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;                    

                    String cmd_update = "UPDATE auto_number SET Number = '" + number_trans + "' WHERE Type_Trans='10' AND Year='" + tahun_now + "' AND Month='" + bulan_now + "'";
                    CRUD update = new CRUD();
                    update.ExecuteNonQuery(cmd_update);
                }
                else
                {
                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');

                    String cmd_insert = "";
                    if (Properties.Settings.Default.DevCode == "null")
                    {
                        final_running_number = "DC/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                        cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','10')";
                    }
                    else
                    {
                        final_running_number = "DC/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
                        cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans,Dev_Code) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','10','" + Properties.Settings.Default.DevCode + "')";
                    }                    

                    CRUD insert = new CRUD();
                    insert.ExecuteNonQuery(cmd_insert);
                }                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }            
        }

        public void save_deliveryCust_header()
        {
            DateTime mydate = DateTime.Now;
            DateTime myhour = DateTime.Now;
            CRUD sql = new CRUD();
            
            try
            {
                get_year_month();
                get_running_number();

                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM deliverycustomer WHERE DELIVERY_CUST_ID ='" + final_running_number + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (!ckon.sqlDataRd.HasRows)
                {
                    String cmd_insert = "INSERT INTO deliverycustomer (DELIVERY_CUST_ID, TOTAL_QTY, STATUS, DATE, TIME, STATUS_API, EMPLOYEE_ID, EMPLOYEE_NAME, TRANSACTION_ID) VALUES ('" + final_running_number + "', '0', '0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','0','" + empl_Id + "', '" + empl_Name + "','" + l_transaksi2.Text + "') ";
                    sql.ExecuteNonQuery(cmd_insert);

                    String cmd_update = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='10'";
                    sql.ExecuteNonQuery(cmd_update);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
    }
}
