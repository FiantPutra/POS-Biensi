using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class RT_SearchArticle : Form
    {
        public String t_id, S_barcode, S_article, S_nama, S_price, S_ID, tgl_req, store_code2, cust_id_store2, _id, good_qty, unit, bulan2,no_sj;
        int new_price, number_trans;
        public int i, new_jumlah, new_harga, totall, price;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();

        public RT_SearchArticle()
        {
            InitializeComponent();
        }
        public void save_auto_number(String bulan, int number)
        {
            bulan2 = bulan;
            number_trans = number;
        }
        //============================LOAD FORM==================================
        private void RT_SearchArticle_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.OnnOrOff == "Offline")
            {
                //====HIT INVEN LOCAL==========================
                API_Inventory InvLocal = new API_Inventory();
                InvLocal.SetInvHitLocal();
                //=============================================                
            }
            
            dgv_2.EnableHeadersVisualStyles = false;
            String sql = "select TOP 100 article._id,article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >=1";
            get_load_data(sql);
            dgv_2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_2.MultiSelect = true;                        
        }
        //======================================================================

        //======================RETREIVE DATA ===============================================
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
                    dgv_2.Rows[dgRows].Cells[0].Value = row["_id"];
                    dgv_2.Rows[dgRows].Cells[1].Value = row["ARTICLE_ID"].ToString();
                    dgv_2.Rows[dgRows].Cells[2].Value = row["ARTICLE_NAME"].ToString();
                    dgv_2.Rows[dgRows].Cells[3].Value = row["SIZE_ID"].ToString();
                    dgv_2.Rows[dgRows].Cells[4].Value = row["COLOR_ID"].ToString();
                    dgv_2.Rows[dgRows].Cells[5].Value = row["PRICE"];
                    dgv_2.Rows[dgRows].Cells[6].Value = row["GOOD_QTY"];
                }
                dgv_2.Columns[5].DefaultCellStyle.Format = "#,###";
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

        //=======================COMBOBOX KATEGORI =========================================
        private void combo_ktg2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo_ktg2.Text == "ALL")
            {
                combo_value.Items.Clear();
                String sql = "select TOP 100 article._id,article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >=1";
                combo_value.Text = "ALL";
                get_load_data(sql);

            }
            else if (combo_ktg2.Text == "BRAND_ID")
            {
                String query = "SELECT * FROM itemdimensionbrand";
                isi_combo_value(query);
                combo_value.Text = "3 SECOND";

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
            else if (combo_ktg2.Text == "SIZE_ID")
            {
                String query = "SELECT * FROM itemdimensionsize";
                isi_combo_value(query);
                combo_value.Text = "L";
            }
            else if (combo_ktg2.Text == "COLOR_ID")
            {
                String query = "SELECT * FROM itemdimensioncolor";
                isi_combo_value(query);
                combo_value.Text = "Blue";
            }
            else if (combo_ktg2.Text == "GENDER_ID")
            {
                String query = "SELECT * FROM itemdimensiongender";
                isi_combo_value(query);
                combo_value.Text = "Men";
            }
        }
        //===================================================================================

        //========================ISI VALUE COMBO =======================================
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
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
        //=============================================================================

        //=================COMBO VALUE DIILIH =========================================
        private void combo_value_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "", id = "";

            try
            {
                if (t_find_article.text == "")
                {
                    if (combo_ktg2.Text == "BRAND_ID")
                    {
                        query = "SELECT * FROM itemdimensionbrand WHERE Description = '"+ combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "DEPARTMENT_ID")
                    {
                        query = "SELECT * FROM itemdimensiondepartment WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "DEPARTMENT_TYPE_ID")
                    {
                        query = "SELECT * FROM itemdimensiondepartmenttype WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "SIZE_ID")
                    {
                        query = "SELECT * FROM itemdimensionsize WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "COLOR_ID")
                    {
                        query = "SELECT * FROM itemdimensioncolor WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "GENDER_ID")
                    {
                        query = "SELECT * FROM itemdimensiongender WHERE Description = '" + combo_value.Text + "'";
                    }

                    ckon.sqlCon().Open();
                    CRUD sql = new CRUD();
                    ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            id = ckon.sqlDataRd["Id"].ToString();
                        }
                    }
                }
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

            String cmd = "select TOP 100 article._id,article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE  article." + combo_ktg2.Text + " = '" + id + "' AND inventory.GOOD_QTY >=1";
            get_load_data(cmd);
        }
        //=============================================================================

        private void t_find_article_OnTextChange(object sender, EventArgs e)
        {
            String count_article = t_find_article.text;
            int count_article_int = count_article.Count();
            string query = "", id = "";

            try
            {
                if (t_find_article.text == "")
                {
                    if (combo_ktg2.Text == "BRAND_ID")
                    {
                        query = "SELECT * FROM itemdimensionbrand WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "DEPARTMENT_ID")
                    {
                        query = "SELECT * FROM itemdimensiondepartment WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "DEPARTMENT_TYPE_ID")
                    {
                        query = "SELECT * FROM itemdimensiondepartmenttype WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "SIZE_ID")
                    {
                        query = "SELECT * FROM itemdimensionsize WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "COLOR_ID")
                    {
                        query = "SELECT * FROM itemdimensioncolor WHERE Description = '" + combo_value.Text + "'";
                    }
                    else if (combo_ktg2.Text == "GENDER_ID")
                    {
                        query = "SELECT * FROM itemdimensiongender WHERE Description = '" + combo_value.Text + "'";
                    }

                    ckon.sqlCon().Open();
                    CRUD sql = new CRUD();
                    ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            id = ckon.sqlDataRd["Id"].ToString();
                        }
                    }
                }
            }
            catch (Exception er)
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

            if (count_article_int < 5)
            {
                if (t_find_article.text == "")
                {

                    String sql2a = "select TOP 100 article._id,article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >=1";
                    get_load_data(sql2a);

                }
                if (t_find_article.text == "" && (combo_ktg2.Text == "BRAND_ID" || combo_ktg2.Text == "DEPARTMENT_ID" || combo_ktg2.Text == "DEPARTMENT_TYPE_ID" || combo_ktg2.Text == "SIZE_ID" || combo_ktg2.Text == "COLOR_ID" || combo_ktg2.Text == "GENDER_ID"))
                {
                    String sql4 = "select TOP 100 article._id,article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >=1 AND article." + combo_ktg2.Text + " = '" + id + "'";
                    get_load_data(sql4);
                    ckon.con.Close();
                }
            }
            if (count_article_int >= 5)
            {
                if (t_find_article.text != "")
                {
                    String sql2 = "select TOP 100 article._id,article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >=1 AND (article.ARTICLE_ID LIKE '%" + t_find_article.text + "%' OR article.ARTICLE_NAME LIKE '%" + t_find_article.text + "%' )";
                    get_load_data(sql2);

                }
                if (t_find_article.text != "" && (combo_ktg2.Text == "BRAND_ID" || combo_ktg2.Text == "DEPARTMENT_ID" || combo_ktg2.Text == "DEPARTMENT_TYPE_ID" || combo_ktg2.Text == "SIZE_ID" || combo_ktg2.Text == "COLOR_ID" || combo_ktg2.Text == "GENDER_ID"))
                {

                    String sql3 = "select TOP 100 article._id,article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE article." + combo_ktg2.Text + " = '" + id + "' AND inventory.GOOD_QTY >=1 AND(article.ARTICLE_ID LIKE '%" + t_find_article.text + "%' OR article.ARTICLE_NAME LIKE '%" + t_find_article.text + "%' )";
                    get_load_data(sql3);
                }
            }
        }
        //================================================================================

        //======================SIMPAN TRANSACTION HEADER============================================
        public void save_trans_header()
        {
            string command;
            
            DateTime mydate = DateTime.Now;
            DateTime myhour = DateTime.Now;            

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM returnorder WHERE RETURN_ORDER_ID ='" + t_id + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (!ckon.sqlDataRd.HasRows)
                {
                    String cmd_Insert = "INSERT INTO returnorder (RETURN_ORDER_ID,STORE_CODE,TOTAL_QTY,STATUS, DATE, TIME,CUST_ID_STORE,NO_SJ) VALUES ('" + t_id + "', '" + store_code2 + "' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_store2 + "','" + no_sj + "') ";
                    CRUD input = new CRUD();
                    input.ExecuteNonQuery(cmd_Insert);

                    String cmd_Update = "UPDATE auto_number SET Month = '" + bulan2 + "', Number = '" + number_trans + "' WHERE Type_Trans='4'";
                    CRUD ubah = new CRUD();
                    ubah.ExecuteNonQuery(cmd_Update);
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
        //==========================================================================================
        //================DATAGRIDVIEW TO DATABASE =========================================
        public void cek_article2()
        {
            string command;
            
            int total_amount;            

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM article INNER JOIN inventory "
                            + "ON article._id = inventory.ARTICLE_ID "
                            + "WHERE article.ARTICLE_ID = '" + S_ID + "'";
                CRUD sql_articel = new CRUD();
                ckon.sqlDataRd = sql_articel.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        unit = ckon.sqlDataRd["UNIT"].ToString();
                        good_qty = ckon.sqlDataRd["GOOD_QTY"].ToString();
                    }
                }

                //====================================================
                new_price = int.Parse(S_price);
                total_amount = new_price * Convert.ToInt32(good_qty);
                string j = "1";

                command = "SELECT * FROM returnorder_line WHERE RETURN_ORDER_ID ='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
                CRUD sql_returnOrder = new CRUD();
                ckon.sqlDataRd = sql_returnOrder.ExecuteDataReader(command, ckon.sqlCon());

                if (!ckon.sqlDataRd.HasRows)
                {
                    string sql_insert = "INSERT INTO returnorder_line (RETURN_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT, SUBTOTAL) VALUES ('" + t_id + "','" + S_ID + "', '" + good_qty + "', '" + unit + "','" + total_amount + "')";
                    CRUD input = new CRUD();
                    input.ExecuteNonQuery(sql_insert);
                    save_trans_header();
                    back_uc();
                }
                else
                {                    
                    MessageBox.Show("This Article Has Been Entered");
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
        //===================================================================================

        //======================MASUK KO FORM UTAMA=====================================
        public void back_uc()
        {
            Form1 f1 = new Form1();
            UC_Ret_order.Instance.retreive();
            UC_Ret_order.Instance.qty();
            this.Close();
        }
        //=============================================================================

        private void dgv_2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                _id = dgv_2.SelectedRows[0].Cells[0].Value.ToString();
                S_ID = dgv_2.SelectedRows[0].Cells[1].Value.ToString();
                S_price = dgv_2.SelectedRows[0].Cells[5].Value.ToString();

                cek_article2();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());                
            }           
        }
    }
}
