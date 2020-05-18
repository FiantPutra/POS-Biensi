using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class SearchArticle : Form
    {
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        public String t_id, S_barcode, S_article, S_nama, S_price, S_ID, id_spg, store_code2, id_inv, articleName;
        int new_price;
        DateTime mydate = DateTime.Now;
        //===============FOR API DISCOUNT==========
        String cust_store, customer, code_store, disc_code, disc_type, disc_desc;
        int qty = 1, subtotal, disc=0, new_disc, purchQty = 1;
        //========================================= 
        public int i, new_jumlah, new_harga, totall, price;


        private void dgv_2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CRUD sql = new CRUD();

            try
            {
                S_ID = dgv_2.SelectedRows[0].Cells[0].Value.ToString();
                S_price = dgv_2.SelectedRows[0].Cells[4].Value.ToString();
                id_inv = dgv_2.SelectedRows[0].Cells[5].Value.ToString();
               
                AddTransLine add = new AddTransLine();
                add.get_data(code_store, customer, S_ID, id_spg);
                add.get_data_trans_line(Convert.ToInt32(S_price),t_id);
                //add.Post_Discount();                
                discCalcSP(t_id, S_ID, purchQty, id_spg, "");
                //add.cek_article2();                                              

                //MEMOTONG ARTICLE
                Inv_Line inv = new Inv_Line();
                int qty_min_plus = -1;
                String type_trans = "1";
                inv.cek_qty_inv(id_inv);
                inv.cek_type_trans(type_trans);
                inv.cek_inv_line(t_id, qty_min_plus);

                back_uc();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        //================get CustIdStore, customer, store_code========
        public void get_data_store(String custId, String cust, String code)
        {
            cust_store = custId;
            customer = cust;
            code_store = code;
        }
        //======================
        public void scan_discount(String id, String store_code, String cust)
        {
            S_ID = id;
            code_store = store_code;
            customer = cust;
        }

        public void discCalcSP(String transId, String articleId, int qty, String spgId, String discountCode, int is_Service = 0, int is_OmniTrans = 0, String omniStoreCode = "")
        {
            CRUD sql = new CRUD();

            try
            {                
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
                        cmd.Parameters.Add("@IS_OMNITRANS", SqlDbType.VarChar).Value = is_OmniTrans;
                        cmd.Parameters.Add("@OMNISTORECODE", SqlDbType.VarChar).Value = omniStoreCode;

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

        public async Task Post_Discount()
        {
            Transaction transaction = new Transaction();
            transaction.storeCode = code_store;
            transaction.customerId = customer;
            List<TransactionLine> transLine = new List<TransactionLine>();
            Article articleFromDb = new Article();
            //==================================================================================
            try
            {
                string command = "SELECT * FROM ARTICLE WHERE ARTICLE_ID='" + S_ID + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

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
            try
            {
                foreach (var c in resultData.discountItems)
                {
                    disc = (Int32)c.amountDiscount;
                    disc_code = c.discountCode;
                    disc_type = c.discountType;
                    disc_desc = c.discountDesc;
                    //MessageBox.Show("" + disc.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //=======================END OF DISCOUNT=============================================================
        public SearchArticle()
        {
            InitializeComponent();
        }

        //=============TEXTBOX SEARCH CHANGED=====================================
        private void t_find_article_OnTextChange(object sender, EventArgs e)
        {
            String count_article = t_find_article.text;
            int count_article_int = count_article.Count();
            //if(count_article_int < 5)
            //{
                if (t_find_article.text == "")
                {

                    String sql2a = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article._id, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.STATUS = '0' AND inventory.GOOD_QTY >=1";
                    get_load_data(sql2a);

                }
                if (t_find_article.text == "" && (combo_ktg2.Text == "Brand" || combo_ktg2.Text == "Department" || combo_ktg2.Text == "Department_Type" || combo_ktg2.Text == "Size" || combo_ktg2.Text == "Color" || combo_ktg2.Text == "Gender"))
                {
                    String sql4 = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article._id, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.STATUS = '0' AND inventory.GOOD_QTY >=1  AND article." + combo_ktg2.Text + " = '" + combo_value.Text + "'";
                    get_load_data(sql4);
                    ckon.con.Close();
                }
            //}
            //if(count_article_int >= 5)
            //{
                if (t_find_article.text != "")
                {
                    String sql2 = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article._id, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.STATUS = '0' AND inventory.GOOD_QTY >= 1 AND (article.ARTICLE_ID LIKE '%" + t_find_article.text + "%' OR article.ARTICLE_NAME LIKE '%" + t_find_article.text + "%' )";
                    get_load_data(sql2);

                }
                if (t_find_article.text != "" && (combo_ktg2.Text == "Brand" || combo_ktg2.Text == "Department" || combo_ktg2.Text == "Department_Type" || combo_ktg2.Text == "Size" || combo_ktg2.Text == "Color" || combo_ktg2.Text == "Gender"))
                {

                    String sql3 = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article._id, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.STATUS = '0' AND inventory.GOOD_QTY >= 1 AND article." + combo_ktg2.Text + " = '" + combo_value.Text + "' AND (article.ARTICLE_ID LIKE '%" + t_find_article.text + "%' OR article.ARTICLE_NAME LIKE '%" + t_find_article.text + "%' )";
                    get_load_data(sql3);
                }
            //}
           
            

        }
        //===================================================================================

        //=================FORM KE LOAD========================================
        private void SearchArticle_Load(object sender, EventArgs e)
        {
            this.ActiveControl = t_find_article;
            t_find_article.Focus();
            dgv_2.Rows.Clear();
            if (Properties.Settings.Default.OnnOrOff == "Offline")
            {
                //====HIT INVEN LOCAL==========================
                /* wahyu tutup 13-11-19
                API_Inventory InvLocal = new API_Inventory();
                InvLocal.SetInvHitLocal();
                */
                //==============================================
                //dataGridView1.Columns[0].HeaderCell.Style.ForeColor = Color.Orange;
                dgv_2.EnableHeadersVisualStyles = false;
                String sql = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article._id, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.STATUS = '0' AND inventory.GOOD_QTY >= 1";
                get_load_data(sql);
                dgv_2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv_2.MultiSelect = true;
            }            
            else
            {
                dgv_2.EnableHeadersVisualStyles = false;
                String sql = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article._id, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.STATUS = '0' AND inventory.GOOD_QTY >= 1";
                get_load_data(sql);
                dgv_2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv_2.MultiSelect = true;
            }
           
          
        }
        //================DATAGRIDVIEW TO DATABASE =========================================
        public void cek_article2()
        {
            new_price = int.Parse(S_price);
            string j = "1";
            //i = 0;

            try
            {
                ckon.sqlCon().Open();
                string command = "SELECT * FROM transaction_line WHERE TRANSACTION_ID ='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        new_jumlah = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"].ToString());
                        new_harga = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                        new_disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                    }

                    new_price = new_price - disc;
                    new_jumlah = new_jumlah + 1;
                    new_harga = new_harga + new_price;
                    new_disc = new_disc + disc;                    
                    String sql_transLine = "UPDATE transaction_line SET QUANTITY='" + new_jumlah + "',DISCOUNT='" + new_disc + "' ,SUBTOTAL='" + new_harga + "' WHERE TRANSACTION_ID='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
                    CRUD update = new CRUD();
                    update.ExecuteNonQuery(sql_transLine);
                }
                else
                {
                    int convert_harga;//convert harga menjadi integer//Jika diskon tidak ada, maka subtotal dikurangi diskon

                    convert_harga = Int32.Parse(S_price);
                    new_harga = convert_harga - disc;

                    String sql_transLine = "INSERT INTO transaction_line (TRANSACTION_ID,ARTICLE_ID,QUANTITY,PRICE,DISCOUNT,SUBTOTAL, SPG_ID, DISCOUNT_CODE,DISCOUNT_TYPE,DISCOUNT_DESC) VALUES ('" + t_id + "','" + S_ID + "', '" + j + "', '" + S_price + "', '" + disc + "' ,'" + new_harga + "', '" + id_spg + "','" + disc_code + "','" + disc_type + "','" + disc_desc + "')";
                    CRUD input = new CRUD();
                    input.ExecuteNonQuery(sql_transLine);
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
        //======================MASUK KO FORM UTAMA=====================================
        public void back_uc()
        {
            //DiscountAfterUseProm afteruser = new DiscountAfterUseProm();
            //afteruser.retreive(t_id, code_store, customer);

            //Form1 f1 = new Form1();
            //uc_coba.Instance.get_data_combo();
            //uc_coba.Instance.save_trans_header();
            uc_coba.Instance.retreive();
            uc_coba.Instance.itung_total();
            this.Close();
        }

        //======================RETREIVE DATA ====================================================
        public void get_load_data(String query)
        {
            dgv_2.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                CRUD sql = new CRUD();
                ckon.dt = sql.ExecuteDataTable(query, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_2.Rows.Add();
                    dgv_2.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_2.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_2.Rows[dgRows].Cells[2].Value = row["SIZE_ID"].ToString();
                    dgv_2.Rows[dgRows].Cells[3].Value = row["COLOR_ID"].ToString();
                    dgv_2.Rows[dgRows].Cells[4].Value = row["PRICE"];
                    dgv_2.Rows[dgRows].Cells[5].Value = row["_id"];
                    dgv_2.Rows[dgRows].Cells[6].Value = row["GOOD_QTY"];
                }
                dgv_2.Columns[4].DefaultCellStyle.Format = "#,###";                
                
                if (dgv_2.Rows.Count > 1 || dgv_2.Rows.Count < 6)
                {
                    fokus_dgv();
                }
                if (dgv_2.Rows.Count > 5)
                {
                    this.ActiveControl = t_find_article;
                    t_find_article.Focus();

                }            
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ckon.dt.Rows.Clear();
                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }            
        }
        //===================================================================================

        private void combo_ktg2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo_ktg2.Text == "ALL")
            {
                combo_value.Items.Clear();
                String sql = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article._id, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.STATUS = '0' AND inventory.GOOD_QTY >= 1";
                //String query = "SELECT * FROM departement UNION ALL SELECT * FROM departementtype";
                //isi_combo_value(query);
                combo_value.Text = "ALL";
                get_load_data(sql);
                                
            }            
            else if (combo_ktg2.Text == "DEPARTMENT_ID")
            {
                String query = "SELECT * FROM itemdimensiondepartment";
                isi_combo_value(query);
                combo_value.Text = "Shirt";
            }
            else if (combo_ktg2.Text == "DEPARTMENT_TYPE_ID")
            {
                String query = "SELECT * FROM itemdimensiondepartmenttype";
                isi_combo_value(query);
                combo_value.Text = "Denim";
            }            
        }
        //========================ISI VALUE COMBO ==============================================
        public void isi_combo_value(String query)
        {
            combo_value.Items.Clear();
            try
            {
                ckon.sqlCon().Open();
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        String name = ckon.sqlDataRd["DESCRIPTION"].ToString();
                        combo_value.Items.Add(name);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if(ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }            
        }
        //=============================================================================
        private void combo_value_SelectedIndexChanged(object sender, EventArgs e)
        {
            String query = "", id = "";
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                if (combo_ktg2.Text == "DEPARTMENT_ID")
                {
                    query = "SELECT * FROM itemdimensiondepartment WHERE DESCRIPTION = '" + combo_value.Text + "'";                    
                }
                else if (combo_ktg2.Text == "DEPARTMENT_TYPE_ID")
                {
                    query = "SELECT * FROM itemdimensiondepartmenttype WHERE DESCRIPTION = '" + combo_value.Text + "'";                    
                }

                ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());
                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id = ckon.sqlDataRd["Id"].ToString();                        
                    }
                }

                String load_data = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article._id, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.STATUS = '0' AND inventory.GOOD_QTY >= 1 AND article." + combo_ktg2.Text + " = '" + id + "'";
                get_load_data(load_data);

            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }            
        }
        //==============CLOSE BY KEY DOWN IN FORM =====================================
        private void SearchArticle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "X")
            {
                this.Close();
            }

        }
        //=================SELECT VALUE BY PRESS ENTER=======================
        private void dgv_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    //S_ID = dgv_2.SelectedRows[0].Cells[0].Value.ToString();

                    //S_price = dgv_2.SelectedRows[0].Cells[4].Value.ToString();
                    //id_inv = dgv_2.SelectedRows[0].Cells[5].Value.ToString();

                    ////============================================================
                    //Post_Discount();
                    ////============================================================
                    //cek_article2();
                    ////MEMOTONG ARTICLE
                    //Inv_Line inv = new Inv_Line();
                    //int qty_min_plus = -1;
                    //String type_trans = "1";
                    //inv.cek_qty_inv(id_inv);
                    //inv.cek_type_trans(type_trans);
                    //inv.cek_inv_line(t_id, qty_min_plus);

                    //back_uc();
                    S_ID = dgv_2.SelectedRows[0].Cells[0].Value.ToString();
                    S_price = dgv_2.SelectedRows[0].Cells[4].Value.ToString();
                    id_inv = dgv_2.SelectedRows[0].Cells[5].Value.ToString();

                    AddTransLine add = new AddTransLine();
                    add.get_data(code_store, customer, S_ID, id_spg);
                    add.get_data_trans_line(Convert.ToInt32(S_price), t_id);
                    //add.Post_Discount();                
                    discCalcSP(t_id, S_ID, purchQty, id_spg, "");
                    //add.cek_article2();                                              

                    //MEMOTONG ARTICLE
                    Inv_Line inv = new Inv_Line();
                    int qty_min_plus = -1;
                    String type_trans = "1";
                    inv.cek_qty_inv(id_inv);
                    inv.cek_type_trans(type_trans);
                    inv.cek_inv_line(t_id, qty_min_plus);

                    back_uc();
                }
                catch
                {
                    MessageBox.Show("Select a value in the table");
                }
                //MessageBox.Show("Enter pressed");
            }
        }
        //=================MENCOBA MEMINDAHKAN FOKUS KE DATAGRIDVIEW SAAT SUDAH MEMASUKAN 5 ARTICLE
        public void fokus_dgv()
        {
            this.ActiveControl = dgv_2;
            dgv_2.Focus();

        }
    }
}
