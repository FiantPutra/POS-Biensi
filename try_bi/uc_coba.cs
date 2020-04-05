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
//using Tulpep.NotificationWindow;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Data.SqlClient;

/************
    Status:
        0 = Open, 1 = Verified, 2 = Void, 3=Hold, 4=Deleted
*/

namespace try_bi
{
    public partial class uc_coba : UserControl
    {
        public static Form1 f1;
        public String t_id, S_Articleid, S_article, S_nama, S_price, id_trans, article_id, date, nm_cur2, cust_id_store2, shift_name_trans, id_employe2, comp_name, id_article, transDate, artIdLoc;
        int noo_inv_new, status_diskon_api, subtotal, qty = 1, disc = 0, new_disc, tot_diskon, no_trans2, is_service, isService;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        String id_spg, nama_spg, sub_string, sub_string2, store_code, id_shift, id_CStore, article_id_API, id_trans_line, discount_code_get, id_inv, customer, code_store, disc_code, disc_type, bulan2, tipe2, disc_desc1, VarBackDate, DateHeaderTrans, discount_code, discAmount;        
        //Variable untuk running number baru
        String bulan_now, tahun_now, bulan_trans, number_trans_string, final_running_number, doc_ref, new_doc = "";                

        int number_trans, disc_type_new, PRICE_GLOBAL, amount_voided=0;
        //
        String articleName;
        string id_list;
        public int i, new_jumlah, new_harga, totall, price, totall_real, get_diskon, get_voucher, get_dis_vou;
        //================DGV WARNA===========================
        String kd_diskon, status;       
        //====================================================
        //======================================================
        private static uc_coba _instance;
        public static uc_coba Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_coba(f1);
                return _instance;
            }
        }
        //=======================================================
        public uc_coba(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
            dgv_purchase.AllowUserToAddRows = false;
        }
        //================CEK TANGGAL APAKAH BACKDATE ATAU TIDAK==============================
        public void cekBackDate()
        {
           
        }
        //====================================================================================
        //================MEMBERIKAN AUTOFOKUS KE SCAN BARCODE SAAT PEMILIHAN SPG DI PILIH====
        private void combo_spg_SelectedIndexChanged(object sender, EventArgs e)
        {
            //===FOKUES KE SCAN BARCODE
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
        }
        //=================================GENERATOR NUMBER=================================
        public void runRetreive()
        {
            CRUD sql = new CRUD();
     
            try
            {
                //ckon.sqlCon().Open();
                //String cmd_transLine = "SELECT TOP 1 a.TRANSACTION_ID, a.STATUS FROM [transaction] a LEFT JOIN transaction_line b ON a.TRANSACTION_ID=b.TRANSACTION_ID WHERE b.TRANSACTION_ID is null AND STATUS IN ('0','3') and COMP_NAME='" + comp_name + "' ORDER BY a._id asc";
                //ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transLine, ckon.sqlCon());

                //if (ckon.sqlDataRd.HasRows)
                //{
                //    while (ckon.sqlDataRd.Read())
                //    {
                //        l_transaksi.Text = ckon.sqlDataRd["TRANSACTION_ID"].ToString();
                //    }
                //}
                //else
                //{
                //    String cmd_trans = "SELECT TOP 1 TRANSACTION_ID FROM [transaction] WHERE STATUS='0' AND COMP_NAME='" + comp_name + "' ORDER BY _id asc";
                //    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_trans, ckon.sqlCon());

                //    if (ckon.sqlDataRd.HasRows)
                //    {
                //        while (ckon.sqlDataRd.Read())
                //        {
                //            l_transaksi.Text = ckon.sqlDataRd["TRANSACTION_ID"].ToString();
                //        }
                //    }
                //    else
                //    {
                //        set_running_number();
                //        save_trans_header();
                //        new_invoice();
                //    }
                //}         
                String cmd_trans = "SELECT TOP 1 TRANSACTION_ID FROM [transaction] WHERE STATUS='0' AND COMP_NAME='" + comp_name + "' ORDER BY _id asc";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_trans, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        l_transaksi.Text = ckon.sqlDataRd["TRANSACTION_ID"].ToString();
                    }
                }
                else
                {
                    set_running_number();
                    save_trans_header();
                    new_invoice();
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

            holding(date);
            retreive();
            get_data_id();
            itung_total();           
        }
        //==================================================================================
        public void new_invoice()
        {
            CRUD sql = new CRUD();
            LinkApi link = new LinkApi();
            dgv_purchase.Rows.Clear(); t_custId.Text = ""; l_total.Text = "0,00"; l_diskon.Text = "0,00"; qty_txt.Text = "0";
            this.ActiveControl = t_custId;
            t_custId.Focus();
            //===================AMBIL ID DARI TABEL CLOSING SHIFT===================           
            try
            {
                ckon.sqlCon().Open();
                String cmd_closeShift = "SELECT TOP 1 ID_SHIFT FROM closing_shift ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_closeShift, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_shift = ckon.sqlDataRd["ID_SHIFT"].ToString();
                    }
                }
                else
                {
                    id_shift = "";
                }

                String cmd_closeStore = "SELECT TOP 1 ID_C_STORE FROM closing_store ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_closeStore, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_CStore = ckon.sqlDataRd["ID_C_STORE"].ToString();
                    }
                }
                else
                {
                    id_CStore = "";
                }

                String cmd_store = "SELECT CODE FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_store, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        store_code = ckon.sqlDataRd["CODE"].ToString();
                    }
                }
                else
                {
                    store_code = link.storeId;
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
        //===========================METHOD ISI COMBO=============================================
        public void isi_combo_spg()
        {
            CRUD sql = new CRUD();

            combo_spg.Items.Clear();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT EMPLOYEE_ID, NAME FROM employee where CONVERT(nvarchar,Pass) != 'GABOLEHLOGINASDASDASDA'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_spg = ckon.sqlDataRd["EMPLOYEE_ID"].ToString();
                        nama_spg = ckon.sqlDataRd["NAME"].ToString();
                        combo_spg.Items.Add(id_spg + " -- " + nama_spg);
                    }                    
                }
                combo_spg.SelectedIndex = 0;
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
        //===HAPUS DATA DI GRIDVIEW==================================
        public void delete_rows()
        {
            dgv_diskon.Rows.Clear();
            dgv_purchase.Rows.Clear();
        }
        //===============TAMPILKAN DATA PENJUALAN===================================================
        public void retreive()
        {            
            CRUD sql = new CRUD();
            String art_id, art_name, spg_id, size, color, qty, disc_desc, sub_total2, disc_desc_substring;
            int price, sub_total, disc;

            dgv_purchase.Rows.Clear();
            dgv_diskon.Rows.Clear();
            Transaction transaction = new Transaction();
            transaction.storeCode = store_code;
            transaction.customerId = t_custId.Text;
            List<TransactionLine> transLine = new List<TransactionLine>();
            Article articleFromDb = new Article();
            
            try
            {                
                ckon.sqlCon().Open();
                //String cmd = "SELECT transaction_line.ARTICLE_ID ,transaction_line.QUANTITY, transaction_line.SUBTOTAL, transaction_line.SPG_ID, transaction_line.DISCOUNT, " +
                //    "transaction_line.DISCOUNT_DESC,transaction_line.DISCOUNT_TYPE,transaction_line.DISCOUNT_CODE, transaction_line.IS_SERVICE, transaction_line.PRICE as TRANS_PRICE, article.* " +
                //    "FROM transaction_line, article " +
                //    "WHERE article.ARTICLE_ID = transaction_line.ARTICLE_ID AND transaction_line.TRANSACTION_ID='" + l_transaksi.Text + "' ORDER BY transaction_line._id ASC";
                //ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                //if (ckon.sqlDataRd.HasRows)
                //{
                //    while (ckon.sqlDataRd.Read())
                //    {
                //        article_id_API = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                //        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                //        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                //        spg_id = ckon.sqlDataRd["SPG_ID"].ToString();
                //        size = ckon.sqlDataRd["SIZE_ID"].ToString();
                //        color = ckon.sqlDataRd["COLOR_ID"].ToString();
                //        PRICE_GLOBAL = Convert.ToInt32(ckon.sqlDataRd["TRANS_PRICE"].ToString());
                //        qty = ckon.sqlDataRd["QUANTITY"].ToString();
                //        disc_desc = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();
                //        sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                //        disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                //        String discType = ckon.sqlDataRd["DISCOUNT_TYPE"].ToString();
                //        disc_type_new = (discType == "") ? 0 : Convert.ToInt32(ckon.sqlDataRd["DISCOUNT_TYPE"].ToString());
                //        discount_code = ckon.sqlDataRd["DISCOUNT_CODE"].ToString();
                //        id_article = Convert.ToString(ckon.sqlDataRd["_id"]);

                //        articleFromDb.articleId = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                //        articleFromDb.articleName = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                //        articleFromDb.brand = ckon.sqlDataRd["BRAND_ID"].ToString();
                //        articleFromDb.color = ckon.sqlDataRd["COLOR_ID"].ToString();
                //        articleFromDb.department = ckon.sqlDataRd["DEPARTMENT_ID"].ToString();
                //        articleFromDb.departmentType = ckon.sqlDataRd["DEPARTMENT_TYPE_ID"].ToString();
                //        articleFromDb.gender = ckon.sqlDataRd["GENDER_ID"].ToString();
                //        articleFromDb.id = Convert.ToInt32(ckon.sqlDataRd["_id"].ToString());
                //        articleFromDb.price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                //        articleFromDb.size = ckon.sqlDataRd["SIZE_ID"].ToString();
                //        articleFromDb.unit = ckon.sqlDataRd["UNIT"].ToString();
                //        articleFromDb.articleIdAlias = ckon.sqlDataRd["ARTICLE_ID_ALIAS"].ToString();
                //        is_service = Convert.ToInt32(ckon.sqlDataRd["IS_SERVICE"].ToString());

                //        TransactionLine t = new TransactionLine();

                //        t.discount = disc;
                //        t.subtotal = sub_total;
                //        t.quantity = Int32.Parse(qty);
                //        t.price = PRICE_GLOBAL;
                //        t.discountType = disc_type_new;
                //        t.discountCode = discount_code;
                //        t.article = articleFromDb;
                //        transLine.Add(t);
                //    }
                //    transaction.transactionLines = transLine;                        

                //    retreive_data_to_dgv(cmd);                    
                //}      

                String cmd = "SELECT * FROM [tmp]." + store_code + " WHERE TRANSACTION_ID = '" + l_transaksi.Text + "' ORDER BY linenum ASC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {                        
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString(); 
                        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        spg_id = ckon.sqlDataRd["SPG_ID"].ToString();                        
                        PRICE_GLOBAL = Convert.ToInt32(ckon.sqlDataRd["PRICE"]);
                        qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        disc_desc = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();
                        sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"]);
                        disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                        //String discType = ckon.sqlDataRd["DISCOUNT_TYPE"].ToString();
                        //disc_type_new = (discType == " ") ? 0 : Convert.ToInt32(ckon.sqlDataRd["DISCOUNT_TYPE"].ToString());
                        discount_code = ckon.sqlDataRd["DISCOUNT_CODE"].ToString();
                        discAmount = ckon.sqlDataRd["DISCOUNTAMOUNT"].ToString();

                        int total_disc_desc = disc_desc.Count();
                        if (total_disc_desc > 4)
                        {
                            disc_desc_substring = disc_desc.Substring(0, 4);
                        }
                        else
                        {
                            disc_desc_substring = disc_desc;
                        }
                        
                        int dgRows = dgv_purchase.Rows.Add();
                        dgv_purchase.Rows[dgRows].Cells[0].Value = art_id;
                        dgv_purchase.Rows[dgRows].Cells[1].Value = art_name;
                        dgv_purchase.Rows[dgRows].Cells[2].Value = spg_id;
                        dgv_purchase.Rows[dgRows].Cells[3].Value = qty;
                        dgv_purchase.Rows[dgRows].Cells[4].Value = PRICE_GLOBAL;
                        dgv_purchase.Rows[dgRows].Cells[5].Value = disc;
                        dgv_purchase.Rows[dgRows].Cells[6].Value = discAmount;
                        dgv_purchase.Rows[dgRows].Cells[7].Value = sub_total == 0 ? "0,00" : sub_total.ToString();
                        //wahyu
                        if (Convert.ToInt32(ckon.sqlDataRd["IS_SERVICE"].ToString()) == 1)
                        {
                            dgv_purchase.Rows[dgRows].Cells[1].Style.BackColor = Color.Green;
                        }                        

                        data_diskon(art_id);
                    }
                }

                dgv_purchase.Columns[4].DefaultCellStyle.Format = "#,###";
                dgv_purchase.Columns[5].DefaultCellStyle.Format = "#,###";
                dgv_purchase.Columns[6].DefaultCellStyle.Format = "#,###";
                dgv_purchase.Columns[7].DefaultCellStyle.Format = "#,###";
            }
            catch (Exception e)
            {
                String msg = "Invalid object name 'tmp." + store_code + "'.";
                if (e.Message != msg)
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
        //===================================================================================
        public async Task Post_Discount()
        {
            String S_ID = t_barcode.Text;
            Transaction transaction = new Transaction();
            transaction.storeCode = store_code;
            transaction.customerId = t_custId.Text;
            List<TransactionLine> transLine = new List<TransactionLine>();
            Article articleFromDb = new Article();
            CRUD sql = new CRUD();
            //==================================================================================
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM ARTICLE WHERE ARTICLE_ID='" + S_ID + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        articleFromDb.articleId = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        articleFromDb.articleName = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        articleFromDb.brand = ckon.sqlDataRd["BRAND"].ToString();
                        articleFromDb.color = ckon.sqlDataRd["COLOR"].ToString();
                        articleFromDb.department = ckon.sqlDataRd["DEPARTMENT"].ToString();
                        articleFromDb.departmentType = ckon.sqlDataRd["DEPARTMENT_TYPE"].ToString();
                        articleFromDb.gender = ckon.sqlDataRd["GENDER"].ToString();
                        articleFromDb.id = Convert.ToInt32(ckon.sqlDataRd["_id"].ToString());
                        articleFromDb.price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        articleFromDb.size = ckon.sqlDataRd["SIZE"].ToString();
                        articleFromDb.unit = ckon.sqlDataRd["UNIT"].ToString();
                        subtotal = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        articleFromDb.articleIdAlias = ckon.sqlDataRd["ARTICLE_ID_ALIAS"].ToString();
                    }
                }

                TransactionLine t = new TransactionLine();
                t.subtotal = subtotal;
                t.quantity = qty;
                t.discount = 0;
                t.price = subtotal;

                t.article = articleFromDb;
                transLine.Add(t);

                transaction.transactionLines = transLine;
                BiensiPOSContext.BiensiPOSDataContext contex = new BiensiPOSContext.BiensiPOSDataContext();
                DiscountCalculateNew dc = new DiscountCalculateNew(contex);
                DiscountMaster resultData = dc.Post(transaction);
                //=================================================
                foreach (var c in resultData.discountItems)
                {
                    disc = (Int32)c.amountDiscount;
                    disc_code = c.discountCode;
                    disc_type = c.discountType;
                    disc_desc1 = c.discountDesc;                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("No connection to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }            
        }
        //===========================ITUNG TOTAL BELANJA=====================================================
        public void itung_total()
        {
            CRUD sql = new CRUD();            
            //==DIGUNAKAN UNTUNG MENGHITUNG TOTAL DISCOUNT DARI TRANS_LINE, LALU MASUKAN TOTAL KE DALAM TRANS_HEADER
            try
            {
                ckon.sqlCon().Open();
                //String cmd_transLineDisc = "SELECT SUM(transaction_line.DISCOUNT) as total FROM transaction_line WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
                String cmd_transLineDisc = "SELECT SUM(DISCOUNTAMOUNT) as total FROM [tmp].[" + store_code + "] WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transLineDisc, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        if (ckon.sqlDataRd["total"].ToString() != "")
                            tot_diskon = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                        else
                            tot_diskon = 0;
                    }
                }
                else
                {
                    tot_diskon = 0;
                }

                String cmd_trans = "SELECT VOUCHER FROM [TRANSACTION] WHERE TRANSACTION_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_trans, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        get_voucher = Convert.ToInt32(ckon.sqlDataRd["VOUCHER"].ToString());
                    }

                    get_dis_vou = tot_diskon + get_voucher;                    
                    
                    //============PERCABANGAN UNTUK LABEL DISKON===========================
                    if (tot_diskon == 0)
                        l_diskon.Text = "0,00";
                    else
                        l_diskon.Text = string.Format("{0:#,###}" + ",00", tot_diskon);
                    //==============PERCABANGAN UNTUK LABEL VOUCHER======================
                    if (get_voucher == 0)
                        l_voucher.Text = "0,00";
                    else
                        l_voucher.Text = string.Format("{0:#,###}" + ",00", get_voucher);
                }
                else
                {
                    get_dis_vou = 0;
                    l_diskon.Text = "0,00";
                    l_voucher.Text = "0,00";
                }

                //String cmd_transLine = "SELECT SUM(transaction_line.SUBTOTAL) as total, SUM(transaction_line.QUANTITY) AS qty FROM transaction_line WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
                String cmd_transLine = "SELECT SUM(SUBTOTAL) as total, SUM(QUANTITY) AS qty FROM [tmp].[" + store_code + "] WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transLine, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        if (ckon.sqlDataRd["total"].ToString() != "")
                        {
                            totall = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                            totall_real = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                        }
                        else
                        {
                            totall = 0;
                            totall_real = 0;
                        }
                        
                        totall = totall - get_voucher;
                        l_total.Text = string.Format("{0:#,###}" + ",00", totall);
                        qty_txt.Text = ckon.sqlDataRd["qty"].ToString();
                    }
                }
                else
                {
                    l_total.Text = "0,00"; 
                    qty_txt.Text = "0";
                }
            }
            catch (Exception e)
            {
                String msg = "Invalid object name 'tmp." + store_code + "'.";
                if (e.Message != msg)
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
        //===================================================================================
        //wahyu
        public void voidClick(String newDoc)
        {
            id_list = newDoc;
            runRetreive();
        }

        //==========KLIK HOLD TRANS MASUK KE KRANGJANG BELANJA===================================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    DateTime mydate = DateTime.Now;
                    this.ActiveControl = t_barcode;
                    t_barcode.Focus();

                    get_data_combo();
                    //wahyu
                    void_ref.Text = "";
                    void_amount.Text = "";                    
                    update_trans_header();
                    date = mydate.ToString("yyyy-MM-dd");
                    id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    if (new_doc != "")
                    {
                        id_list = new_doc;
                    }
                    l_transaksi.Text = id_list;
                    set_status(id_list, "0");
                    //set_to_hold(id_list);
                    holding(date);
                    retreive();
                    get_data_id();
                    itung_total();
                    new_doc = "";
                }
            }
            catch (Exception)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(nm_cur2 + "--" + cust_id_store2);
        }

        //===========================CHANGE SPG ID IN TRANSACTION LINE================================
        private void dgv_purchase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();

            if (dgv_purchase.Columns[e.ColumnIndex].Name == "spgid")
            {
                String id = dgv_purchase.Rows[e.RowIndex].Cells["articleId"].Value.ToString();
                String idspg = dgv_purchase.Rows[e.RowIndex].Cells["spgid"].Value.ToString();
                w_edit_SPG_ID spg = new w_edit_SPG_ID(f1);
                spg.get_data(id, l_transaksi.Text);
                spg.ShowDialog();
            }            
        }
        //=================================GET ID SPG FROM COMBO BOX =================================
        public void get_data_combo()
        {
            try
            {
                sub_string = combo_spg.Text;
                sub_string2 = sub_string.Substring(0, 9);
            }
            catch
            {
                sub_string2 = "";
            }
        }
        //================DELETE LIST BELANJA=========================================================
        private void dgv_purchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CRUD sql = new CRUD();            
            String DiscCode = "";
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            if (dgv_purchase.Columns[e.ColumnIndex].Name == "Delete")
            {
                String id = dgv_purchase.Rows[e.RowIndex].Cells["articleId"].Value.ToString();
                String qty = dgv_purchase.Rows[e.RowIndex].Cells["Column5"].Value.ToString();
                //String cmd_delete = "DELETE FROM transaction_line WHERE TRANSACTION_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + id + "'";   
                String cmd_delete = "DELETE FROM [tmp].[" + store_code + "] WHERE TRANSACTION_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + id + "'";
                sql.ExecuteNonQuery(cmd_delete);

                dgv_purchase.Rows.Clear();
                dgv_purchase.Refresh();
                retreive();
                itung_total();

                string command = "SELECT _id, Article_ID FROM ARTICLE WHERE ARTICLE_ID='" + id + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_article = ckon.sqlDataRd["_id"].ToString();                     
                    }
                }

                string cmd_TransDisc = "SELECT DiscountCode FROM TransactionDiscount WHERE ARTICLE_ID='" + id + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_TransDisc, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        DiscCode = ckon.sqlDataRd["DiscountCode"].ToString();

                        foreach (DataGridViewRow row in dgv_diskon.Rows)
                        {
                            String diskon = dgv_diskon.Rows[row.Index].Cells[1].Value.ToString();

                            if (DiscCode == diskon)
                            {
                                dgv_diskon.Rows.RemoveAt(row.Index);
                            }
                        }
                    }
                }                

                Inv_Line inv = new Inv_Line();
                int qty2 = int.Parse(qty);
                inv.get_id(id);
                String type_trans = "1";
                inv.cek_qty_inv(id_article);
                inv.cek_type_trans(type_trans);
                inv.cek_inv_line(l_transaksi.Text, qty2);

                String del_inv_line = "DELETE FROM inventory_line WHERE TRANS_REF_ID='" + l_transaksi.Text + "' AND ARTICLE_ID = '" + id_article + "'";
                sql.ExecuteNonQuery(del_inv_line);

                String del_trans_line = "DELETE FROM transaction_line WHERE TRANSACTION_ID = '" + l_transaksi.Text + "' AND ARTICLE_ID = '" + id_article + "'";
                sql.ExecuteNonQuery(del_trans_line);
            }

            if (dgv_purchase.Columns[e.ColumnIndex].Name == "Column6")
            {                         
                try
                {
                    String cmd = "SELECT IS_SERVICE FROM transaction_line WHERE ARTICLE_ID='" + dgv_purchase.Rows[e.RowIndex].Cells["articleId"].Value.ToString() + "' AND TRANSACTION_ID='" + l_transaksi.Text + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            isService = Convert.ToInt32(ckon.sqlDataRd["IS_SERVICE"].ToString());
                        }
                    }

                    if (isService == 1)
                    {
                        dgv_purchase.Rows[e.RowIndex].Cells[5].Style.ForeColor = Color.Blue;
                        String idArticle = dgv_purchase.Rows[e.RowIndex].Cells["articleId"].Value.ToString();
                        String price = dgv_purchase.Rows[e.RowIndex].Cells[5].Value.ToString();
                        String articlename = dgv_purchase.Rows[e.RowIndex].Cells[1].Value.ToString();
                        w_edit_sales_price sls = new w_edit_sales_price(f1);
                        sls.set_data(idArticle, l_transaksi.Text, price, articlename);
                        sls.ShowDialog();
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("No connection to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        //=============================== SCAN BARCODE SAVE TO DB=================================
        private void t_barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            CRUD sql = new CRUD();
            Boolean empDiscValid = false;

            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                { 
                    if (t_custId.Text != "")
                    {
                        DialogResult result = MessageBox.Show("Do you want set to employee discount?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            String cmd = "SELECT * FROM Employee WHERE EMPLOYEE_ID = '" + t_custId.Text + "'";
                            ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());
                            if (ckon.sqlDataRd.HasRows)
                            {
                                empDiscValid = true;
                            }

                            if (!empDiscValid)
                                MessageBox.Show("Employee ID not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if ((t_custId.Text != "" && empDiscValid) || t_custId.Text == "")
                    {
                        get_data_combo();
                        save_trans_header();
                        cek_article();
                        retreive();
                        itung_total();
                        t_barcode.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }            
        }

        //======================MASUKAN DATA PENJUALAN================================================
        public void cek_article()
        {
            String upper = t_barcode.Text.ToUpper();
            CRUD sql = new CRUD();
            int purchQty = 1;
            koneksi ckonCheck = new koneksi();
            String cmd;
            Boolean hasRows = false;

            try
            {
                ckon.sqlCon().Open();
                ckonCheck.sqlCon().Open();

                cmd = "SELECT * FROM article WHERE article.ARTICLE_ID_ALIAS = '" + t_barcode.Text + "'";
                ckonCheck.sqlDataRd = sql.ExecuteDataReader(cmd, ckonCheck.sqlCon());

                if (ckonCheck.sqlDataRd.HasRows)
                {
                    while (ckonCheck.sqlDataRd.Read())
                    {
                        artIdLoc = ckonCheck.sqlDataRd["ARTICLE_ID"].ToString();
                        hasRows = true;
                    }
                }

                if (hasRows)
                {
                    cmd = "SELECT article._id, article.PRICE FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >= 1 AND article.ARTICLE_ID = '" + artIdLoc + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            id_inv = ckon.sqlDataRd["_id"].ToString();
                            price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        }

                        AddTransLine add = new AddTransLine();
                        //add.get_data(store_code, t_custId.Text, upper, sub_string2);
                        //add.get_data_trans_line(price, l_transaksi.Text);
                        ////add.Post_Discount();
                        //add.cek_article2();
                        ////Post_Discount();

                        //Inv_Line inv = new Inv_Line();
                        //int qty_min_plus = -1;
                        //String type_trans = "1";
                        //inv.cek_qty_inv(id_inv);

                        //inv.cek_type_trans(type_trans);
                        //inv.cek_inv_line(l_transaksi.Text, qty_min_plus);

                        add.get_data(code_store, t_custId.Text, artIdLoc, id_spg);
                        add.get_data_trans_line(Convert.ToInt32(S_price), l_transaksi.Text);
                        //add.Post_Discount();                
                        discCalcSP(l_transaksi.Text, artIdLoc, purchQty, id_spg, "");
                        //add.cek_article2();                                              

                        //MEMOTONG ARTICLE
                        Inv_Line inv = new Inv_Line();
                        int qty_min_plus = -1;
                        String type_trans = "1";
                        inv.cek_qty_inv(id_inv);
                        inv.cek_type_trans(type_trans);
                        inv.cek_inv_line(l_transaksi.Text, qty_min_plus);
                    }
                    else
                    {
                        MessageBox.Show("Article Not Found Or Quantity Empty");
                    }                   
                }                
                else
                {
                    cmd = "SELECT article._id, article.PRICE FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >= 1 AND article.ARTICLE_ID = '" + upper + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            id_inv = ckon.sqlDataRd["_id"].ToString();
                            price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        }

                        AddTransLine add = new AddTransLine();
                        //add.get_data(store_code, t_custId.Text, upper, sub_string2);
                        //add.get_data_trans_line(price, l_transaksi.Text);
                        ////add.Post_Discount();
                        //add.cek_article2();
                        ////Post_Discount();

                        //Inv_Line inv = new Inv_Line();
                        //int qty_min_plus = -1;
                        //String type_trans = "1";
                        //inv.cek_qty_inv(id_inv);

                        //inv.cek_type_trans(type_trans);
                        //inv.cek_inv_line(l_transaksi.Text, qty_min_plus);

                        add.get_data(code_store, t_custId.Text, upper, id_spg);
                        add.get_data_trans_line(Convert.ToInt32(S_price), l_transaksi.Text);
                        //add.Post_Discount();                
                        discCalcSP(l_transaksi.Text, upper, purchQty, id_spg, "");
                        //add.cek_article2();                                              

                        //MEMOTONG ARTICLE
                        Inv_Line inv = new Inv_Line();
                        int qty_min_plus = -1;
                        String type_trans = "1";
                        inv.cek_qty_inv(id_inv);
                        inv.cek_type_trans(type_trans);
                        inv.cek_inv_line(l_transaksi.Text, qty_min_plus);
                    }
                    else
                    {
                        MessageBox.Show("Article Not Found Or Quantity Empty");
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

        public void discCalcSP(String transId, String articleId, int qty, String spgId, String discountCode, int is_Service = 0)
        {
            CRUD sql = new CRUD();            

            try
            {
                string command = "SELECT ARTICLE_NAME FROM ARTICLE WHERE ARTICLE_ID='" + articleId + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        articleName = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                    }
                }

                using (SqlConnection con = new SqlConnection(ckon.sqlCon().ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertTransaction", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@TRANSACTION_ID", SqlDbType.VarChar).Value = transId;
                        cmd.Parameters.Add("@ARTICLE_ID", SqlDbType.VarChar).Value = articleId;
                        cmd.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = qty;
                        cmd.Parameters.Add("@SPG_ID", SqlDbType.VarChar).Value = spgId;
                        cmd.Parameters.Add("@DISCOUNT_CODE", SqlDbType.VarChar).Value = discountCode;                        
                        cmd.Parameters.Add("@IS_SERVICE", SqlDbType.VarChar).Value = is_Service;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //======================LIST HOLD TRANSACTION============================================
        public void holding(String tanggal)
        {
            CRUD sql = new CRUD();
            dgv_hold.Rows.Clear();
            List<string> numbersList = new List<string>();
            
            string date = tanggal;
            try
            {
                ckon.sqlCon().Open();
                //String cmd = "SELECT [transaction].TRANSACTION_ID, article.ARTICLE_NAME FROM [transaction] INNER JOIN transaction_line "
                //            + "ON transaction_line.TRANSACTION_ID = [transaction].TRANSACTION_ID INNER JOIN article "
                //            + "ON article.ARTICLE_ID = transaction_line.ARTICLE_ID "
                //            + "WHERE [transaction].STATUS = '3' AND [transaction].ID_SHIFT = '" + id_shift + "'";
                String cmd = "SELECT TRANSACTION_ID, DATE FROM [transaction] "
                            + "WHERE [transaction].STATUS = '3'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["TRANSACTION_ID"].ToString();
                        transDate = ckon.sqlDataRd["DATE"].ToString();
                        //numbersList.Add(ckon.sqlDataRd["ARTICLE_NAME"].ToString());

                        //string[] numbersArray = numbersList.ToArray();
                        //numbersList.Clear();
                        //string result = String.Join(", ", numbersArray);
                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_trans;
                        dgv_hold.Rows[dgRows].Cells[1].Value = transDate;
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
        //======================SIMPAN TRANSACTION HEADER============================================
        public void save_trans_header()
        {
            CRUD sql = new CRUD();            
            DateTime mydate = DateTime.Now;
            DateHeaderTrans = mydate.ToString("yyyy-MM-dd");

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT _id FROM [transaction] WHERE TRANSACTION_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    update_trans_header();
                }
                else
                {
                    String cmd_insert = "INSERT INTO [transaction] (TRANSACTION_ID,STORE_CODE,CUSTOMER_ID,EMPLOYEE_ID,SPG_ID,STATUS, DATE, TIME, CUST_ID_STORE, CURRENCY,ID_SHIFT,ID_C_STORE,COMP_NAME) VALUES ('" + l_transaksi.Text + "','" + store_code + "' ,'" + t_custId.Text + "','" + id_employe2 + "' ,'" + sub_string2 + "','0','" + DateHeaderTrans + "', '" + mydate.ToLocalTime().ToString("H:mm:ss") + "', '" + cust_id_store2 + "', '" + nm_cur2 + "','" + id_shift + "','" + id_CStore + "','" + comp_name + "') ";
                    sql.ExecuteNonQuery(cmd_insert);
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
        //=======================UPDATE TRANSAKSI HEADER============================================
        public void update_trans_header()
        {
            DateTime mydate = DateTime.Now;
            DateHeaderTrans = mydate.ToString("yyyy-MM-dd");

            String cmd_update = "UPDATE [transaction] SET CUSTOMER_ID ='" + t_custId.Text + "', SPG_ID='" + sub_string2 + "', DATE='" + mydate.ToString("yyyy-MM-dd") + "', TIME='" + mydate.ToLocalTime().ToString("H:mm:ss") + "' WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);            
        }
        //===========================GET DATA BY ID ================================================
        public void get_data_id()
        {
            CRUD sql = new CRUD();
           
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT CUSTOMER_ID, SPG_ID, REFERENCE_DOC FROM [transaction] WHERE TRANSACTION_ID='" + id_list + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        t_custId.Text = ckon.sqlDataRd["CUSTOMER_ID"].ToString();
                        combo_spg.Text = ckon.sqlDataRd["SPG_ID"].ToString();
                        doc_ref = ckon.sqlDataRd["REFERENCE_DOC"].ToString();
                    }
                }

                if (doc_ref != "")
                {
                    void_ref.Text = "Void Ref : " + doc_ref;                    
                    String cmd_transTotal = "SELECT TOTAL FROM [transaction] WHERE TRANSACTION_ID='" + doc_ref + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transTotal, ckon.sqlCon());
                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            amount_voided = Convert.ToInt32(ckon.sqlDataRd["TOTAL"].ToString());
                            void_amount.Text = "Void Amount : Rp " + string.Format("{0:#,###}" + ",00", amount_voided);
                        }
                    }                    
                }
                else
                {
                    amount_voided = 0;
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
        //============================DATAGRIDVIEW DISKON===========================================
        public void data_diskon(String articleId)        
        {
            CRUD sql = new CRUD();
            String discountName = "";
            int promotionId = 0;
            koneksi ckon_disc = new koneksi();

            try
            {
                dgv_diskon.Rows.Clear();

                ckon_disc.sqlCon().Open();
                //String cmd_discount = "SELECT a.DiscountCode, a.DiscountName FROM DiscountSetup a INNER JOIN DiscountSetupLines b "
                //                + "ON b.DiscountSetupId = a.Id INNER JOIN [tmp].[" + store_code + "] c ON c.ARTICLE_ID = b.code "
                //                + "WHERE(a.DiscountType = 4 or a.DiscountType = 5)";
                String cmd_discount = "SELECT a.DiscountCode, a.DiscountName FROM DiscountSetup a INNER JOIN DiscountSetupLines b "
                                + "ON b.DiscountSetupId = a.Id "
                                + "WHERE(a.DiscountType = 4 or a.DiscountType = 5) AND b.Code = '"+ articleId +"'";
                ckon_disc.sqlDataRdHeader = sql.ExecuteDataReader(cmd_discount, ckon_disc.sqlCon());

                if (ckon_disc.sqlDataRdHeader.HasRows)
                {
                    while (ckon_disc.sqlDataRdHeader.Read())
                    {
                        kd_diskon = ckon_disc.sqlDataRdHeader["DiscountCode"].ToString();
                        discountName = ckon_disc.sqlDataRdHeader["DiscountName"].ToString();

                        String cmd_PromotionId = "SELECT TOP 1 _id FROM promotion ORDER BY _id DESC";
                        ckon_disc.sqlDataRd = sql.ExecuteDataReader(cmd_PromotionId, ckon_disc.sqlCon());

                        if (ckon_disc.sqlDataRd.HasRows)
                        {
                            while (ckon_disc.sqlDataRd.Read())
                            {
                                promotionId = Convert.ToInt32(ckon_disc.sqlDataRd["_id"]) + 1;                              
                            }                            
                        }

                        String cmd_insert = "IF NOT EXISTS (SELECT * FROM promotion WHERE DISCOUNT_CODE = '" + kd_diskon + "') " +
                                            "BEGIN " +
                                            "INSERT INTO promotion(DISCOUNT_CODE, DISCOUNT_NAME, _id) " +
                                            "VALUES('" + kd_diskon + "', '" + discountName + "', " + promotionId + ") " +
                                            "END";

                        sql.ExecuteNonQuery(cmd_insert);                        
                    }                                 
                }

                String cmd_promotion = "SELECT DISCOUNT_CODE FROM promotion";
                ckon_disc.sqlDataRd = sql.ExecuteDataReader(cmd_promotion, ckon_disc.sqlCon());

                if (ckon_disc.sqlDataRd.HasRows)
                {
                    while (ckon_disc.sqlDataRd.Read())
                    {
                        int dgRows = dgv_diskon.Rows.Add();
                        dgv_diskon.Rows[dgRows].Cells[1].Value = ckon_disc.sqlDataRd["DISCOUNT_CODE"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon_disc.sqlDataRd != null)
                    ckon_disc.sqlDataRd.Close();

                if (ckon_disc.sqlCon().State == ConnectionState.Open)
                    ckon_disc.sqlCon().Close();
            }            
        }
        //==========================================================================================
        public void dgv_color()
        {
            for (int i = 0; i < dgv_diskon.Rows.Count; i++)
            {
                int val = Int32.Parse(status);               
                if (val == 0)
                {
                    dgv_diskon.Rows[i].DefaultCellStyle.ForeColor = Color.Red;                    
                }


            }
        }
        //=============================================================================================
        private void dgv_diskon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            try
            {
                if (dgv_diskon.Columns[e.ColumnIndex].Name == "kd_dskn")
                {
                    //untuk ambil data spg
                    get_data_combo();
                    //ambil id diskon
                    String id = dgv_diskon.Rows[e.RowIndex].Cells["kd_dskn"].Value.ToString();
                    W_Promotion_Popup diskon = new W_Promotion_Popup(f1);
                    diskon.get_id(id, totall_real, l_transaksi.Text, sub_string2, t_custId.Text, store_code);
                    diskon.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("Please Select Value In List");
            }

        }
        //============BUTTON CHARGE 2 (BARU)==================
        private void b_charge2_Click(object sender, EventArgs e)
        {
            if (qty_txt.Text == "0")
            {
                MessageBox.Show("Your Cart Is Empty");
            }
            else
            {
                //wahyu
                if (amount_voided > totall)
                {
                    DialogResult result = MessageBox.Show("Transaksi void lebih kecil dari transaksi baru, apakah anda ingin melanjutkan transaksi ?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {
                        MessageBox.Show("Transaksi baru dikenai charge item service");
                        
                        String sqldel = "DELETE FROM transaction_line where ARTICLE_ID='290892' AND TRANSACTION_ID='" + l_transaksi.Text + "'";
                        CRUD del = new CRUD();
                        del.ExecuteNonQuery(sqldel);
                        
                        double gab = amount_voided - totall;
                        String sqlins = "INSERT INTO transaction_line(TRANSACTION_ID, ARTICLE_ID, QUANTITY, PRICE, SUBTOTAL,SPG_ID) VALUES ('" + l_transaksi.Text + "', '290892', 1, '" + gab + "','" + gab + "','" + id_employe2 + "')";
                        CRUD ins = new CRUD();
                        ins.ExecuteNonQuery(sqlins);

                        itung_total();
                        showCharge();
                    }
                } 
                else
                {
                    insertIntoTransactionTable();
                    showCharge();
                }                
            }
        }

        private void showCharge()
        {
            get_data_combo();
            update_trans_header();
            charge c = new charge(f1);
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(charge.Instance))
            {
                f1.p_kanan.Controls.Add(charge.Instance);
                charge.Instance.Dock = DockStyle.Fill;
                charge.Instance.id_trans(l_transaksi.Text, totall, get_dis_vou, get_voucher, tot_diskon);
                charge.Instance.get_data_id(l_transaksi.Text);

                charge.Instance.QuickCash();
                //charge.Instance.AddButtons2();
                charge.Instance.Show();
            }
            else
            {
                charge.Instance.Show();
            }
        }

        public void insertIntoTransactionTable()
        {
            CRUD sql = new CRUD();
            koneksi ckonCheck = new koneksi();
            int price, qty_Trans, subtotal_Trans, disc_Trans, discAmount, maxDiscQty, maxDiscAmount, isService;
            String spg_Id, article_Id, discountCode, discountType, discountDesc;            

            String cmd = "SELECT * FROM [tmp].[" + store_code + "] WHERE TRANSACTION_ID = '"+ l_transaksi.Text + "'  ORDER BY linenum ASC";
            ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

            if (ckon.sqlDataRd.HasRows)
            {
                while (ckon.sqlDataRd.Read())
                {
                    article_Id = Convert.ToString(ckon.sqlDataRd["ARTICLE_ID"]);
                    qty_Trans = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"]);
                    subtotal_Trans = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"]);
                    disc_Trans = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"]);
                    price = Convert.ToInt32(ckon.sqlDataRd["PRICE"]);
                    discAmount = Convert.ToInt32(ckon.sqlDataRd["DISCOUNTAMOUNT"]);
                    spg_Id = Convert.ToString(ckon.sqlDataRd["SPG_ID"]);
                    maxDiscQty = Convert.ToInt32(ckon.sqlDataRd["MAXDISCOUNTQUANTITY"]);
                    maxDiscAmount = Convert.ToInt32(ckon.sqlDataRd["MAXDISCOUNTAMOUNT"]);
                    discountCode = Convert.ToString(ckon.sqlDataRd["DISCOUNT_CODE"]);
                    discountType = Convert.ToString(ckon.sqlDataRd["DISCOUNT_TYPE"]);
                    discountDesc = Convert.ToString(ckon.sqlDataRd["DISCOUNT_DESC"]);
                    isService = Convert.ToInt32(ckon.sqlDataRd["IS_SERVICE"]);

                    String cmd_check = "SELECT * FROM transaction_line WHERE TRANSACTION_ID ='" + l_transaksi.Text + "' AND ARTICLE_ID='" + article_Id + "'";
                    ckonCheck.sqlDataRd = sql.ExecuteDataReader(cmd_check, ckon.sqlCon());

                    if (ckonCheck.sqlDataRd.HasRows)
                    {
                        String cmd_update = "UPDATE transaction_line SET QUANTITY='" + qty_Trans + "',DISCOUNT='" + disc_Trans + "' ,SUBTOTAL='" + subtotal_Trans + "', PRICE = '" + price + "', DISCOUNTAMOUNT = '" + discAmount + "', MAXDISCOUNTQUANTITY = '" + maxDiscQty + "', MAXDISCOUNTAMOUNT = '" + maxDiscAmount + "', DISCOUNTSETUPLINES = 0 " +
                            "WHERE TRANSACTION_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + article_Id + "'";
                        sql.ExecuteNonQuery(cmd_update);
                    }
                    else
                    {
                        //int convert_harga;//convert harga menjadi integer
                        //                  //                      //jika diskon tidak ada, maka subtotal dikurangi diskon

                        //convert_harga = price;
                        //subtotal_Trans = convert_harga - disc_Trans;

                        if (disc_type == null)
                        {
                            disc_type = "99";
                        }

                        string cmd_insert = "INSERT INTO transaction_line (TRANSACTION_ID,ARTICLE_ID,QUANTITY,PRICE,DISCOUNT,SUBTOTAL, SPG_ID, DISCOUNT_CODE,DISCOUNT_TYPE,DISCOUNT_DESC, IS_SERVICE,DISCOUNTAMOUNT,MAXDISCOUNTQUANTITY,MAXDISCOUNTAMOUNT,DISCOUNTSETUPLINES) " +
                            "VALUES ('" + l_transaksi.Text + "','" + article_Id + "', '" + qty_Trans + "', '" + price + "', '" + disc_Trans + "' ,'" + subtotal_Trans + "', '" + spg_Id + "','" + discountCode + "','" + discountType + "','" + discountDesc + "', '" + isService + "', '" + discAmount + "', '" + maxDiscQty + "', '" + maxDiscAmount + "', 0)";
                        sql.ExecuteNonQuery(cmd_insert);
                    }
                }
            }
        }

        //===================BUTTON CLEAR 2(BARU)=======================================
        private void b_clear2_Click(object sender, EventArgs e)
        {
            DateTime mydate = DateTime.Now;
            if (l_total.Text == "0,00")
            {
                MessageBox.Show("No Item On List");
            }
            else
            {
                date = mydate.ToString("yyyy-MM-dd");
                //
                Inv_Line inv = new Inv_Line();
                String type_trans = "4";
                inv.cek_type_trans(type_trans);
                inv.void_trans(l_transaksi.Text);
                inv.del_inv_line(l_transaksi.Text);
                dgv_purchase.Rows.Clear();
                dgv_purchase.Refresh();

                holding(date);
                retreive();
                l_total.Text = "0,00";
                l_diskon.Text = "0,00";
                l_voucher.Text = "0,00";
                qty_txt.Text = "0";
            }
        }
        //===========BUTTON HOLD 2====================================
        private void b_hold2_Click(object sender, EventArgs e)
        {
            DateTime mydate = DateTime.Now;
            if (qty_txt.Text == "0")
            {
                MessageBox.Show("Pick Item First");
            }
            else
            {
                get_data_combo();
                save_trans_header();
                update_trans_header();
                date = mydate.ToString("yyyy-MM-dd");
                //new_invoice();
                //set_running_number();
                set_status(l_transaksi.Text, "3");
                runRetreive();
                dgv_purchase.Rows.Clear();
                l_total.Text = "0,00";
                l_diskon.Text = "0,00";
                l_voucher.Text = "0,00";
                t_custId.Text = ""; //combo_spg.SelectedIndex = 0;
                qty_txt.Text = "0";
                isi_combo_spg();
                delete_rows();
            }
        }
        private void set_status(String trId, String stts)
        {
            String cmd_update = "UPDATE [transaction] SET STATUS='"+ stts +"' WHERE TRANSACTION_ID='"+ trId + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);            
        }

        private void set_to_hold(String trId)
        {
            String cmd_update = "UPDATE [transaction] SET STATUS='3' WHERE STATUS='0' AND TRANSACTION_ID !='" + trId + "' AND COMP_NAME='"+ comp_name +"'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);           
        }
        //==================BUTTON FIND ITEM (BARU)===================================
        private void B_FIND2_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();
            Boolean empDiscValid = false;

            try
            {
                if (t_custId.Text != "")
                {
                    DialogResult result = MessageBox.Show("Do you want set to employee discount?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        String cmd = "SELECT * FROM Employee WHERE EMPLOYEE_ID = '" + t_custId.Text + "'";
                        ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());
                        if (ckon.sqlDataRd.HasRows)
                        {
                            empDiscValid = true;
                        }

                        if (!empDiscValid)
                            MessageBox.Show("Employee ID not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                                        
                }
                
                if ((t_custId.Text != "" && empDiscValid) || t_custId.Text == "")
                {
                    //===FOKUES KE SCAN BARCODE
                    this.ActiveControl = t_barcode;
                    t_barcode.Focus();
                    get_data_combo();
                    SearchArticle filter = new SearchArticle();
                    filter.get_data_store(cust_id_store2, t_custId.Text, store_code);
                    filter.t_id = l_transaksi.Text;
                    filter.id_spg = sub_string2;
                    filter.store_code2 = store_code;
                    filter.ShowDialog();
                }                               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
        //=======================SHORTCUT BUTTON FROM T_CUST_ID=======================================
        private void t_custId_KeyDown(object sender, KeyEventArgs e)
        {
            //====BUTTON SEARCH ARTICLE (FIND ITEM)==============
            if (e.Control && e.KeyCode.ToString() == "F")
            {
                B_FIND2_Click(null, null);
            }
            //=======BUTTON CLEAR ALL (DELETE ALL)==============
            if (e.Control && e.KeyCode.ToString() == "D")
            {
                b_clear2_Click(null, null);
            }
            //======BUTTON HOLD========================
            if (e.Control && e.KeyCode.ToString() == "H")
            {
                b_hold2_Click(null, null);
            }
            //======BUTTON CHARGE===============
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_charge2_Click(null, null);
            }
            //======BUTTON VOUCHER==================
            if (e.Control && e.KeyCode.ToString() == "V")
            {
                b_voucher2_Click(null, null);
                t_custId.Text = "";
            }
        }
        //============================================================================================
        private void t_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            //====BUTTON SEARCH ARTICLE (FIND ITEM)==============
            if (e.Control && e.KeyCode.ToString() == "F")
            {
                B_FIND2_Click(null, null);
            }
            //=======BUTTON CLEAR ALL (DELETE ALL)==============
            if (e.Control && e.KeyCode.ToString() == "D")
            {
                b_clear2_Click(null, null);
            }
            //======BUTTON HOLD========================
            if (e.Control && e.KeyCode.ToString() == "H")
            {
                b_hold2_Click(null, null);
            }
            //======BUTTON CHARGE===============
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_charge2_Click(null, null);
            }
            //======BUTTON VOUCHER==================
            if (e.Control && e.KeyCode.ToString() == "V")
            {
                b_voucher2_Click(null, null);
                t_barcode.Text = "";
            }
        }
        //============================================================================================
        private void combo_spg_KeyDown(object sender, KeyEventArgs e)
        {
            //====BUTTON SEARCH ARTICLE (FIND ITEM)==============
            if (e.Control && e.KeyCode.ToString() == "F")
            {
                B_FIND2_Click(null, null);
            }
            //=======BUTTON CLEAR ALL (DELETE ALL)==============
            if (e.Control && e.KeyCode.ToString() == "D")
            {
                b_clear2_Click(null, null);
            }
            //======BUTTON HOLD========================
            if (e.Control && e.KeyCode.ToString() == "H")
            {
                b_hold2_Click(null, null);
            }
            //======BUTTON CHARGE===============
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_charge2_Click(null, null);
            }
            //======BUTTON VOUCHER==================
            if (e.Control && e.KeyCode.ToString() == "V")
            {
                b_voucher2_Click(null, null);
                combo_spg.SelectedIndex = 0;
            }
        }
        //=================================FOCUS FROM CHARGE MENU ====================================
        public void barcode_fokus()
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();

        }
        //=======B_VOUCHER2 (BARU)=======================
        private void b_voucher2_Click(object sender, EventArgs e)
        {
            if (l_total.Text == "0,00")
            {
                MessageBox.Show("Your Shopping Cart Is Empty");
            }
            else
            {
                W_Voucher vou = new W_Voucher();
                vou.id_transaksi2 = l_transaksi.Text;
                vou.store_code2 = store_code;
                vou.ShowDialog();
            }
        }
        //=========METHOD GET DATA FROM AUTO_NUMBER TABLE FOR SALES TRANSACTION
        public void set_running_number()
        {
            DateTime mydate = DateTime.Now;
            CRUD sql = new CRUD();

            bulan_now = mydate.ToString("MM");
            tahun_now = mydate.ToString("yy");
            number_trans = 0;
            /* tutup - wahyu 8-11-19
            DevCode code = new DevCode();

            String device_code = "";
            device_code = code.aDevCode;

            String sql = "";
            if (Properties.Settings.Default.DevCode == "null")
            {
                sql = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '1'";
            }
            else
            {
                sql = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '1' AND Dev_Code='" + device_code + "'";
            }
            */
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 Number FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '1' AND Month='" + bulan_now + "' AND Year='" + tahun_now + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        number_trans = Convert.ToInt32(ckon.sqlDataRd["Number"].ToString());
                    }

                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');//
                    if (Properties.Settings.Default.DevCode == "null")
                        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                    else
                        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
                    l_transaksi.Text = final_running_number;

                    String cmd_update = "UPDATE auto_number SET Number = '" + number_trans + "' WHERE Type_Trans='1' AND Year='" + tahun_now + "' AND Month='" + bulan_now + "'";
                    CRUD update = new CRUD();
                    update.ExecuteNonQuery(cmd_update);
                }
                else
                {
                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');//wahyu

                    String cmd_insert = "";
                    if (Properties.Settings.Default.DevCode == "null")
                    {
                        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                        cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','1')";
                    }
                    else
                    {
                        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
                        cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans,Dev_Code) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','1','" + Properties.Settings.Default.DevCode + "')";
                    }
                    l_transaksi.Text = final_running_number;

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

        public void retreive_data_to_dgv(String cmd)
        {
            String art_id, art_name, spg_id, size, color, qty, disc_desc, sub_total2, disc_desc_substring;
            int price, sub_total, disc;
            CRUD sql = new CRUD();

            dgv_purchase.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        //article_id_API = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        //art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        //art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        //spg_id = ckon.sqlDataRd["SPG_ID"].ToString();
                        //size = ckon.sqlDataRd["SIZE_ID"].ToString();
                        //color = ckon.sqlDataRd["COLOR_ID"].ToString();
                        //price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        //qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        //disc_desc = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();
                        //sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                        //disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                        //String discType = ckon.sqlDataRd["DISCOUNT_TYPE"].ToString();
                        //disc_type_new = (discType == "") ? 0 : Convert.ToInt32(ckon.sqlDataRd["DISCOUNT_TYPE"].ToString());                        
                        //discount_code = ckon.sqlDataRd["DISCOUNT_CODE"].ToString();
                        //id_article = Convert.ToString(ckon.sqlDataRd["_id"]);

                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        spg_id = ckon.sqlDataRd["SPG_ID"].ToString();
                        price = Convert.ToInt32(ckon.sqlDataRd["PRICE"]);
                        qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        disc_desc = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();
                        sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"]);
                        disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                        String discType = ckon.sqlDataRd["DISCOUNT_TYPE"].ToString();
                        disc_type_new = (discType == "") ? 0 : Convert.ToInt32(ckon.sqlDataRd["DISCOUNT_TYPE"].ToString());
                        discount_code = ckon.sqlDataRd["DISCOUNT_CODE"].ToString();
                        discAmount = ckon.sqlDataRd["DISCOUNTAMOUNT"].ToString();

                        int total_disc_desc = disc_desc.Count();
                        if (total_disc_desc > 4)
                        {
                            disc_desc_substring = disc_desc.Substring(0, 4);
                        }
                        else
                        {
                            disc_desc_substring = disc_desc;
                        }
                        
                        int dgRows = dgv_purchase.Rows.Add();
                        dgv_purchase.Rows[dgRows].Cells[0].Value = art_id;
                        dgv_purchase.Rows[dgRows].Cells[1].Value = art_name;
                        dgv_purchase.Rows[dgRows].Cells[2].Value = spg_id;
                        dgv_purchase.Rows[dgRows].Cells[3].Value = qty;
                        dgv_purchase.Rows[dgRows].Cells[4].Value = price;
                        dgv_purchase.Rows[dgRows].Cells[5].Value = disc;
                        dgv_purchase.Rows[dgRows].Cells[6].Value = discAmount;                            
                        dgv_purchase.Rows[dgRows].Cells[7].Value = sub_total == 0 ? "0,00" : sub_total.ToString();
                        //wahyu
                        if (Convert.ToInt32(ckon.sqlDataRd["IS_SERVICE"].ToString()) == 1)
                        {
                            dgv_purchase.Rows[dgRows].Cells[1].Style.BackColor = Color.Green;
                        }                        
                    }
                }

                dgv_purchase.Columns[4].DefaultCellStyle.Format = "#,###";
                dgv_purchase.Columns[5].DefaultCellStyle.Format = "#,###";
                dgv_purchase.Columns[6].DefaultCellStyle.Format = "#,###";
                dgv_purchase.Columns[7].DefaultCellStyle.Format = "#,###";                
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