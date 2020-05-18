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
    public partial class RO_SearchArticle : Form
    {
        public String t_id, S_barcode, S_article, S_nama, S_price, S_ID, tgl_req, store_code2,cust_id_store2, war_id2, bulan2, no_sj;
        int new_price;
        public int i, new_jumlah, new_harga, totall, price, number_trans;
        koneksi ckon = new koneksi();

        public RO_SearchArticle()
        {
            InitializeComponent();
        }
        public void save_auto_number(String bulan, int number)
        {
            bulan2 = bulan;
            number_trans = number;
        }

        //=========================FORM LOAD  =========================================
        private void RO_SearchArticle_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.OnnOrOff == "Offline")
            {
                //====HIT INVEN LOCAL==========================
                API_Inventory InvLocal = new API_Inventory();
                InvLocal.SetInvHitLocal();
                //=============================================
                dgv_2.EnableHeadersVisualStyles = false;
                String sql = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID";
                get_load_data(sql);
                dgv_2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv_2.MultiSelect = true;
            }
            else
            {
                dgv_2.EnableHeadersVisualStyles = false;
                String sql = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID";
                get_load_data(sql);
                dgv_2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv_2.MultiSelect = true;
            }
            
        }
        //======================RETREIVE DATA ====================================================

        public void get_load_data(String query)
        {
            string command;            

            dgv_2.Rows.Clear();
            //string sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);

            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_2.Rows.Add();
            //        dgv_2.Rows[n].Cells[0].Value = row["ARTICLE_ID"].ToString();
            //        dgv_2.Rows[n].Cells[1].Value = row["ARTICLE_NAME"].ToString();
            //        dgv_2.Rows[n].Cells[2].Value = row["SIZE"].ToString();
            //        dgv_2.Rows[n].Cells[3].Value = row["COLOR"].ToString();
            //        dgv_2.Rows[n].Cells[4].Value = row["PRICE"];
            //        dgv_2.Rows[n].Cells[5].Value = row["GOOD_QTY"];
            //    }
            //    dgv_2.Columns[4].DefaultCellStyle.Format = "#,###";
            //    //con.Close();
            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    ckon.con.Close();
            //}

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
                    dgv_2.Rows[dgRows].Cells[2].Value = row["SIZE"].ToString();
                    dgv_2.Rows[dgRows].Cells[3].Value = row["COLOR"].ToString();
                    dgv_2.Rows[dgRows].Cells[4].Value = row["PRICE"];
                    dgv_2.Rows[dgRows].Cells[5].Value = row["GOOD_QTY"];
                }
                dgv_2.Columns[4].DefaultCellStyle.Format = "#,###";
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


        //=================COMBOBOX KATEGORI =================================================
        private void combo_ktg2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo_ktg2.Text == "ALL")
            {
                combo_value.Items.Clear();
                String sql = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID";
                combo_value.Text = "ALL";
                get_load_data(sql);
            }
            else if (combo_ktg2.Text == "Brand")
            {
                String query = "SELECT * FROM brand";
                isi_combo_value(query);
                combo_value.Text = "3 SECOND";
            }
            else if (combo_ktg2.Text == "Department")
            {
                String query = "SELECT * FROM departement";
                isi_combo_value(query);
                combo_value.Text = "Shirt";
            }
            else if (combo_ktg2.Text == "Department_Type")
            {
                String query = "SELECT * FROM departementtype";
                isi_combo_value(query);
                combo_value.Text = "Denim";
            }
            else if (combo_ktg2.Text == "Size")
            {
                String query = "SELECT * FROM Size";
                isi_combo_value(query);
                combo_value.Text = "L";
            }
            else if (combo_ktg2.Text == "Color")
            {
                String query = "SELECT * FROM Color";
                isi_combo_value(query);
                combo_value.Text = "Blue";
            }
            else if (combo_ktg2.Text == "Gender")
            {
                String query = "SELECT * FROM Gender";
                isi_combo_value(query);
                combo_value.Text = "Men";
            }
            else
            { }
        }
        //===================================================================================

        //========================ISI VALUE COMBO ==============================================
        public void isi_combo_value(String query)
        {                                   
            combo_value.Items.Clear();
            //String sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        String name = ckon.myReader.GetString("DESCRIPTION");
            //        combo_value.Items.Add(name);
            //    }
            //    ckon.con.Close();

            //}
            //catch
            //{ }

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

        //=============COMBO VALUE DIILIH ============================================
        private void combo_value_SelectedIndexChanged(object sender, EventArgs e)
        {
            String sql = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE  article." + combo_ktg2.Text + " = '" + combo_value.Text + "'";
            get_load_data(sql);
        }
        //=============================================================================

        //================TEXTBOX SEARCH CHANGED ======================================
        private void t_find_article_OnTextChange(object sender, EventArgs e)
        {
            String count_article = t_find_article.text;
            int count_article_int = count_article.Count();
            if (count_article_int < 5)
            {
                if (t_find_article.text == "")
                {

                    String sql2a = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID";
                    get_load_data(sql2a);

                }
                if (t_find_article.text == "" && (combo_ktg2.Text == "Brand" || combo_ktg2.Text == "Department" || combo_ktg2.Text == "Department_Type" || combo_ktg2.Text == "Size" || combo_ktg2.Text == "Color" || combo_ktg2.Text == "Gender"))
                {
                    String sql4 = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE  article." + combo_ktg2.Text + " = '" + combo_value.Text + "'";
                    get_load_data(sql4);
                    ckon.con.Close();
                }
            }
            if (count_article_int >= 5)
            {
                if (t_find_article.text != "")
                {
                    String sql2 = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE article.ARTICLE_ID LIKE '%" + t_find_article.text + "%' OR article.ARTICLE_NAME LIKE '%" + t_find_article.text + "%'";
                    get_load_data(sql2);

                }
                if (t_find_article.text != "" && (combo_ktg2.Text == "Brand" || combo_ktg2.Text == "Department" || combo_ktg2.Text == "Department_Type" || combo_ktg2.Text == "Size" || combo_ktg2.Text == "Color" || combo_ktg2.Text == "Gender"))
                {

                    String sql3 = "select TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE article." + combo_ktg2.Text + " = '" + combo_value.Text + "' AND (article.ARTICLE_ID LIKE '%" + t_find_article.text + "%' OR article.ARTICLE_NAME LIKE '%" + t_find_article.text + "%' )";
                    get_load_data(sql3);
                }
            }
           
           
        }
        //=============================================================================

        //================DATAGRIDVIEW TO DATABASE =========================================
        public void cek_article2()
        {
            string command;            

            new_price = int.Parse(S_price);
            string j = "1";
            int count = 0;
            //i = 0;
            //string sql = "SELECT * FROM requestorder_line WHERE REQUEST_ORDER_ID ='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //int count = 0;
            //while (ckon.myReader.Read())
            //{
            //    count = count + 1;
            //    new_jumlah = ckon.myReader.GetInt32("QUANTITY");
            //    new_harga = ckon.myReader.GetInt32("SUBTOTAL");
            //}
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM requestorder_line WHERE REQUEST_ORDER_ID ='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {                        
                        new_jumlah = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"].ToString());
                        new_harga = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                    }

                    new_jumlah = new_jumlah + 1;
                    new_harga = new_harga + new_price;
                    String sql3 = "UPDATE requestorder_line SET QUANTITY='" + new_jumlah + "', SUBTOTAL='" + new_harga + "' WHERE REQUEST_ORDER_ID='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
                    CRUD input = new CRUD();
                    input.ExecuteNonQuery(sql3);
                }
                else
                {
                    string sql2 = "INSERT INTO requestorder_line (REQUEST_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT,SUBTOTAL) VALUES ('" + t_id + "','" + S_ID + "', '" + j + "', 'Pcs','" + new_price + "')";
                    CRUD input = new CRUD();
                    input.ExecuteNonQuery(sql2);
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

            //if (count == 0)
            //{
            //    string sql2 = "INSERT INTO requestorder_line (REQUEST_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT,SUBTOTAL) VALUES ('" + t_id + "','" + S_ID + "', '" + j + "', 'Pcs','"+ new_price +"')";
            //    CRUD input = new CRUD();
            //    input.ExecuteNonQuery(sql2);
            //}
            //else
            //{
            //    new_jumlah = new_jumlah + 1;
            //    new_harga = new_harga + new_price;
            //    String sql3 = "UPDATE requestorder_line SET QUANTITY='" + new_jumlah + "', SUBTOTAL='"+ new_harga +"' WHERE REQUEST_ORDER_ID='" + t_id + "' AND ARTICLE_ID='" + S_ID + "'";
            //    CRUD input = new CRUD();
            //    input.ExecuteNonQuery(sql3);
            //}
        }
        //===================================================================================

        //======================MASUK KO FORM UTAMA=====================================
        public void back_uc()
        {

            Form1 f1 = new Form1();
          
            UC_Req_order.Instance.retreive();
            UC_Req_order.Instance.qty();
            this.Close();
        }
        //=============================================================================


        //======================SIMPAN TRANSACTION HEADER============================================
        public void save_trans_header()
        {
            string command;            

            DateTime mydate = DateTime.Now;
            DateTime myhour = DateTime.Now;
            //ckon.con.Close();
            //String sql0 = "SELECT * FROM requestorder WHERE REQUEST_ORDER_ID ='" + t_id + "'";
            //ckon.cmd = new MySqlCommand(sql0, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //int count0 = 0;
            //while (ckon.myReader.Read())
            //{
            //    count0 = count0 + 1;
            //}
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM requestorder WHERE REQUEST_ORDER_ID ='" + t_id + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (!ckon.sqlDataRd.HasRows)
                {
                    String cmd_insert = "INSERT INTO requestorder (REQUEST_ORDER_ID,STORE_CODE,REQUEST_DELIVERY_DATE,TOTAL_QTY,STATUS, DATE, TIME, WAREHOUSE_ID,CUST_ID_STORE, NO_SJ) " +
                                    "VALUES ('" + t_id + "', '" + store_code2 + "' , '" + tgl_req + "' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','" + war_id2 + "','" + cust_id_store2 + "','" + no_sj + "') ";
                    CRUD Input = new CRUD();
                    Input.ExecuteNonQuery(cmd_insert);

                    String cmd_update = "UPDATE auto_number SET Month = '" + bulan2 + "', Number = '" + number_trans + "' WHERE Type_Trans='2'";
                    CRUD ubah = new CRUD();
                    ubah.ExecuteNonQuery(cmd_update);
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

            //if (count0 == 0)
            //{
            //    String sql = "INSERT INTO requestorder (REQUEST_ORDER_ID,STORE_CODE,REQUEST_DELIVERY_DATE,TOTAL_QTY,STATUS, DATE, TIME, WAREHOUSE_ID,CUST_ID_STORE, NO_SJ) VALUES ('" + t_id + "', '"+ store_code2 +"' , '" + tgl_req + "' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','"+ war_id2 +"','"+ cust_id_store2 +"','"+ no_sj +"') ";
            //    ckon.con.Open();
            //    ckon.cmd = new MySqlCommand(sql, ckon.con);
            //    ckon.cmd.ExecuteNonQuery();
            //    ckon.con.Close();

            //    String query = "UPDATE auto_number SET Month = '" + bulan2 + "', Number = '" + number_trans + "' WHERE Type_Trans='2'";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);
            //}
            //else
            //{

            //}
        }
        //==========================================================================================

        private void dgv_2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                S_ID = dgv_2.SelectedRows[0].Cells[0].Value.ToString();

                S_price = dgv_2.SelectedRows[0].Cells[4].Value.ToString();

                cek_article2();
                save_trans_header();
                back_uc();
            }
            catch
            {
                MessageBox.Show("Select a value in the table");
            }
        }
    }
}
