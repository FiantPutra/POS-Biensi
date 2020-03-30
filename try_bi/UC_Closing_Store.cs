using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class UC_Closing_Store : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        DateTime mydate = DateTime.Now;
        String id_trans, view_date, view_time, id_list, date, id_Cstore, epy_id, epy_name, date_close_store, date_close_store2;
        int totall, amount, cash, diskon, edc, change, edc2, voucher, get_voucher, get_dis_vou, get_diskon;
        int total_qty2 = 0;
        //======================================================
        private static UC_Closing_Store _instance;

        public static UC_Closing_Store Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Closing_Store(f1);
                return _instance;
            }
        }

        //=======================================================
        public UC_Closing_Store(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //================================RESET==============================================
        public void reset()
        {
            dgv_hold.Rows.Clear();
            dgv_purchase.Rows.Clear();
            l_transaksi.Text = "";
            t_custId.Text = "";
            //t_employId.Text = "";
            t_spgId.Text = "";
            l_diskon.Text = "0";
            l_total.Text = "0";
        }
        //============set id epmloyee and name employee===============================
        public void set_name(String id, String name)
        {
            epy_id = id;
            epy_name = name;
        }
        //===method untuk mengambil id terakhir dati tabel close shift============================
        public void get_id_close()
        {
            string command;
            CRUD sql = new CRUD();

            //ckon.con.Close();
            //String sql3 = "SELECT * FROM closing_store WHERE STATUS_CLOSE='0' ORDER BY _id DESC LIMIT 1";
            //ckon.cmd = new MySqlCommand(sql3, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_Cstore = ckon.myReader.GetString("ID_C_STORE");
            //        date_close_store = ckon.myReader.GetString("OPENING_TIME");
            //    }
            //}
            //else
            //{ id_Cstore = "0"; }
            //date_close_store2 = date_close_store.Substring(0, 10);
            //l_date.Text = date_close_store2;
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM closing_store WHERE STATUS_CLOSE='0' ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_Cstore = ckon.sqlDataRd["ID_C_STORE"].ToString();
                        date_close_store = ckon.sqlDataRd["OPENING_TIME"].ToString();
                    }
                    date_close_store2 = date_close_store.Substring(0, 10);                    
                }
                else
                {
                    id_Cstore = string.Empty;
                    date_close_store = string.Empty;
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

            l_date.Text = date_close_store2;
        }
        //==========METHOD YANG AKAN DIPANGGIL DARI FORM1, AKAN DIJALANKAN UNTUK CLOSING SHIFT==================
        public void from_form1()
        {
            //String sql = "SELECT [transaction].TRANSACTION_ID, [transaction].TIME, article.ARTICLE_NAME FROM [transaction] INNER JOIN transaction_line "
            //                + "ON transaction_line.TRANSACTION_ID = [transaction].TRANSACTION_ID INNER JOIN article "
            //                + "ON article.ARTICLE_ID = transaction_line.ARTICLE_ID "
            //                + "WHERE [transaction].ID_C_STORE = '" + id_Cstore + "' AND([transaction].STATUS = '1' or [transaction].STATUS = '2')";

            String sql = "SELECT [transaction].TRANSACTION_ID, [transaction].DATE, [transaction].TIME FROM [transaction] "
                            + "WHERE [transaction].ID_C_STORE = '" + id_Cstore + "' AND([transaction].STATUS = '1' or [transaction].STATUS = '2')";

            holding(sql);
        }
        //=====================================================================================
        public void holding(String cmd)
        {            
            CRUD sql = new CRUD();
            List<string> numbersList = new List<string>();            

            dgv_hold.Rows.Clear();
            //koneksi2 ckon2 = new koneksi2();
            //ckon.con.Close();                        
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //List<string> numbersList = new List<string>();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_trans = ckon.myReader.GetString("TRANSACTION_ID");
            //        view_date = ckon.myReader.GetString("TIME");
            //        String sql2 = "SELECT article.ARTICLE_NAME FROM transaction_line, article  WHERE article.ARTICLE_ID = transaction_line.ARTICLE_ID AND transaction_line.TRANSACTION_ID='" + id_trans + "'";
            //        ckon2.cmd2 = new MySqlCommand(sql2, ckon2.con2);
            //        ckon2.con2.Open();
            //        ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //        while (ckon2.myReader2.Read())
            //        {
            //            article_id = ckon2.myReader2.GetString("ARTICLE_NAME");
            //            numbersList.Add(Convert.ToString(ckon2.myReader2["ARTICLE_NAME"]));
            //        }
            //        string[] numbersArray = numbersList.ToArray();
            //        numbersList.Clear();
            //        string result = String.Join(", ", numbersArray);
            //        int n = dgv_hold.Rows.Add();
            //        dgv_hold.Rows[n].Cells[0].Value = id_trans;
            //        dgv_hold.Rows[n].Cells[1].Value = result;
            //        dgv_hold.Rows[n].Cells[2].Value = view_date; 
            //        ckon2.con2.Close();
            //    }

            //}
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["TRANSACTION_ID"].ToString();
                        view_date = ckon.sqlDataRd["DATE"].ToString();
                        view_time = ckon.sqlDataRd["TIME"].ToString();
                        //numbersList.Add(Convert.ToString(ckon.sqlDataRd["ARTICLE_NAME"].ToString()));

                        //string[] numbersArray = numbersList.ToArray();
                        //numbersList.Clear();
                        //string result = String.Join(", ", numbersArray);
                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_trans;
                        dgv_hold.Rows[dgRows].Cells[1].Value = view_date;
                        dgv_hold.Rows[dgRows].Cells[2].Value = view_time;
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
        //=====================================================================================

        //===============TAMPILKAN DATA PENJUALAN===================================================
        public void retreive()
        {
            String art_id, art_name, spg_id, size, color, qty, disc_desc, sub_total2;
            int price, sub_total;
            string command;
            CRUD sql = new CRUD();

            dgv_purchase.Rows.Clear();
            //ckon.con.Close();
            //dgv_purchase.Rows.Clear();
            //String sql = "SELECT  transaction_line.ARTICLE_ID ,transaction_line.QUANTITY, transaction_line.SUBTOTAL, transaction_line.SPG_ID,transaction_line.DISCOUNT, transaction_line.DISCOUNT_DESC,article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE FROM transaction_line, article  WHERE article.ARTICLE_ID = transaction_line. ARTICLE_ID AND transaction_line.TRANSACTION_ID='" + l_transaksi.Text + "' ORDER BY transaction_line._id ASC";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    //ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    //ckon.adapter.Fill(ckon.dt);
            //    //foreach (DataRow row in ckon.dt.Rows)
            //    //{
            //    //    int n = dgv_purchase.Rows.Add();
            //    //    dgv_purchase.Rows[n].Cells[0].Value = row["ARTICLE_ID"].ToString();
            //    //    dgv_purchase.Rows[n].Cells[1].Value = row["ARTICLE_NAME"].ToString();
            //    //    dgv_purchase.Rows[n].Cells[2].Value = row["SPG_ID"];
            //    //    dgv_purchase.Rows[n].Cells[3].Value = row["SIZE"].ToString();
            //    //    dgv_purchase.Rows[n].Cells[4].Value = row["COLOR"].ToString();
            //    //    dgv_purchase.Rows[n].Cells[5].Value = row["PRICE"];
            //    //    dgv_purchase.Rows[n].Cells[6].Value = row["QUANTITY"].ToString();
            //    //    dgv_purchase.Rows[n].Cells[7].Value = row["DISCOUNT_DESC"];
            //    //    dgv_purchase.Rows[n].Cells[8].Value = row["SUBTOTAL"];
            //    //}
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        art_id = ckon.myReader.GetString("ARTICLE_ID");
            //        art_name = ckon.myReader.GetString("ARTICLE_NAME");
            //        spg_id = ckon.myReader.GetString("SPG_ID");
            //        size = ckon.myReader.GetString("SIZE");
            //        color = ckon.myReader.GetString("COLOR");
            //        price = ckon.myReader.GetInt32("PRICE");
            //        qty = ckon.myReader.GetString("QUANTITY");
            //        disc_desc = ckon.myReader.GetString("DISCOUNT_DESC");
            //        sub_total = ckon.myReader.GetInt32("SUBTOTAL");

            //        if (sub_total == 0)
            //        {
            //            sub_total2 = "0,00";
            //            int n = dgv_purchase.Rows.Add();
            //            dgv_purchase.Rows[n].Cells[0].Value = art_id;
            //            dgv_purchase.Rows[n].Cells[1].Value = art_name;
            //            dgv_purchase.Rows[n].Cells[2].Value = spg_id;
            //            dgv_purchase.Rows[n].Cells[3].Value = size;
            //            dgv_purchase.Rows[n].Cells[4].Value = color;
            //            dgv_purchase.Rows[n].Cells[5].Value = price;
            //            dgv_purchase.Rows[n].Cells[6].Value = qty;
            //            dgv_purchase.Rows[n].Cells[7].Value = disc_desc;
            //            dgv_purchase.Rows[n].Cells[8].Value = sub_total2;
            //        }
            //        else
            //        {

            //            int n = dgv_purchase.Rows.Add();
            //            dgv_purchase.Rows[n].Cells[0].Value = art_id;
            //            dgv_purchase.Rows[n].Cells[1].Value = art_name;
            //            dgv_purchase.Rows[n].Cells[2].Value = spg_id;
            //            dgv_purchase.Rows[n].Cells[3].Value = size;
            //            dgv_purchase.Rows[n].Cells[4].Value = color;
            //            dgv_purchase.Rows[n].Cells[5].Value = price;
            //            dgv_purchase.Rows[n].Cells[6].Value = qty;
            //            dgv_purchase.Rows[n].Cells[7].Value = disc_desc;
            //            dgv_purchase.Rows[n].Cells[8].Value = sub_total;
            //        }

            //    }
            //    dgv_purchase.Columns[5].DefaultCellStyle.Format = "#,###";
            //    dgv_purchase.Columns[7].DefaultCellStyle.Format = "#,###";
            //    dgv_purchase.Columns[8].DefaultCellStyle.Format = "#,###";
            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT  transaction_line.ARTICLE_ID ,transaction_line.QUANTITY, transaction_line.SUBTOTAL, transaction_line.SPG_ID,transaction_line.DISCOUNT, transaction_line.DISCOUNT_DESC,article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE FROM transaction_line, article  WHERE article.ARTICLE_ID = transaction_line. ARTICLE_ID AND transaction_line.TRANSACTION_ID='" + l_transaksi.Text + "' ORDER BY transaction_line._id ASC";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        spg_id = ckon.sqlDataRd["SPG_ID"].ToString();
                        size = ckon.sqlDataRd["SIZE_ID"].ToString();
                        color = ckon.sqlDataRd["COLOR_ID"].ToString();
                        price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        disc_desc = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();
                        sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());

                        if (sub_total == 0)
                        {
                            sub_total2 = "0,00";
                            int dgRows = dgv_purchase.Rows.Add();
                            dgv_purchase.Rows[dgRows].Cells[0].Value = art_id;
                            dgv_purchase.Rows[dgRows].Cells[1].Value = art_name;
                            dgv_purchase.Rows[dgRows].Cells[2].Value = spg_id;
                            dgv_purchase.Rows[dgRows].Cells[3].Value = size;
                            dgv_purchase.Rows[dgRows].Cells[4].Value = color;
                            dgv_purchase.Rows[dgRows].Cells[5].Value = price;
                            dgv_purchase.Rows[dgRows].Cells[6].Value = qty;
                            dgv_purchase.Rows[dgRows].Cells[7].Value = disc_desc;
                            dgv_purchase.Rows[dgRows].Cells[8].Value = sub_total2;
                        }
                        else
                        {

                            int dgRows = dgv_purchase.Rows.Add();
                            dgv_purchase.Rows[dgRows].Cells[0].Value = art_id;
                            dgv_purchase.Rows[dgRows].Cells[1].Value = art_name;
                            dgv_purchase.Rows[dgRows].Cells[2].Value = spg_id;
                            dgv_purchase.Rows[dgRows].Cells[3].Value = size;
                            dgv_purchase.Rows[dgRows].Cells[4].Value = color;
                            dgv_purchase.Rows[dgRows].Cells[5].Value = price;
                            dgv_purchase.Rows[dgRows].Cells[6].Value = qty;
                            dgv_purchase.Rows[dgRows].Cells[7].Value = disc_desc;
                            dgv_purchase.Rows[dgRows].Cells[8].Value = sub_total;
                        }
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

            //String sql2 = "SELECT transaction.*,bank.`BANK_NAME` AS payment, c.`BANK_NAME` AS payment2  FROM TRANSACTION LEFT JOIN bank ON transaction.`BANK_NAME`=bank.`BANK_ID` LEFT JOIN bank c ON transaction.`BANK_NAME2`=c.`BANK_ID` WHERE TRANSACTION_ID ='" + l_transaksi.Text + "'";
            //ckon.cmd = new MySqlCommand(sql2, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        if (ckon.myReader.GetInt32("EDC") > 0)
            //        {
            //            payment1_txt.Text = "Payment Method : EDC";
            //            payment1_value.Text = ckon.myReader.GetString("payment") + " : " + string.Format("{0:#,###}" + ",00", ckon.myReader.GetInt32("EDC")) + " - No Ref : " + ckon.myReader.GetString("NO_REF");
            //            if (ckon.myReader.GetInt32("EDC2") > 0)
            //            {
            //                payment2_value.Text = ckon.myReader.GetString("payment2") + " : " + string.Format("{0:#,###}" + ",00", ckon.myReader.GetInt32("EDC2")) + " - No Ref : " + ckon.myReader.GetString("NO_REF2");
            //            }
            //        }
            //        else
            //        {
            //            payment1_txt.Text = "Payment Method : Cash";
            //            payment1_value.Text = "";
            //            payment2_value.Text = "";
            //            //payment1_value.Text = string.Format("{0:#,###}" + ",00", ckon.myReader.GetInt32("CASH"));
            //        }
            //    }
            //    get_dis_vou = get_diskon + get_voucher;
            //    ckon.con.Close();
            //}
            //catch
            //{ }

            try
            {            
                ckon.sqlCon().Open();
                command = "SELECT [transaction].*,bank.BANK_NAME AS payment, c.BANK_NAME AS payment2 FROM [TRANSACTION] LEFT JOIN bank ON [transaction].BANK_NAME=bank.BANK_ID "
                           + "LEFT JOIN bank c ON [transaction].BANK_NAME2=c.BANK_ID WHERE TRANSACTION_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        if (Convert.ToInt32(ckon.sqlDataRd["EDC"].ToString()) > 0)
                        {
                            payment1_txt.Text = "Payment Method : EDC";
                            payment1_value.Text = ckon.sqlDataRd["payment"].ToString() + " : " + string.Format("{0:#,###}" + ",00", Convert.ToInt32(ckon.sqlDataRd["EDC"].ToString())) + " - No Ref : " + ckon.sqlDataRd["NO_REF"].ToString();
                            if (ckon.myReader.GetInt32("EDC2") > 0)
                            {
                                payment2_value.Text = ckon.sqlDataRd["payment2"].ToString() + " : " + string.Format("{0:#,###}" + ",00", Convert.ToInt32(ckon.sqlDataRd["EDC2"].ToString())) + " - No Ref : " + ckon.sqlDataRd["NO_REF2"].ToString();
                            }
                        }
                        else
                        {
                            payment1_txt.Text = "Payment Method : Cash";
                            payment1_value.Text = "";
                            payment2_value.Text = "";                        
                        }
                    }
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

        //===========================================================================================

        //==========================TEXTBOX SEARCH TRANSAKSI=============================
        private void t_search_trans_OnTextChange(object sender, EventArgs e)
        {
            if (t_search_trans.text == "")
            {                
                date = mydate.ToString("yyyy-MM-dd");
                String query2 = "SELECT [transaction].TRANSACTION_ID, [transaction].TIME, article.ARTICLE_NAME FROM [transaction] INNER JOIN transaction_line "
                                    + "ON transaction_line.TRANSACTION_ID = [transaction].TRANSACTION_ID INNER JOIN article "
                                    + "ON article.ARTICLE_ID = transaction_line.ARTICLE_ID "
                                    + "WHERE [transaction].ID_C_STORE = '" + id_Cstore + "' AND([transaction].STATUS = '1' or [transaction].STATUS = '2')";
                holding(query2);
            }
            else
            {                
                date = mydate.ToString("yyyy-MM-dd");
                String query2 = "SELECT [transaction].TRANSACTION_ID, [transaction].TIME, article.ARTICLE_NAME FROM [transaction] INNER JOIN transaction_line "
                                    + "ON transaction_line.TRANSACTION_ID = [transaction].TRANSACTION_ID INNER JOIN article "
                                    + "ON article.ARTICLE_ID = transaction_line.ARTICLE_ID "
                                    + "WHERE [transaction].ID_C_STORE = '" + id_Cstore + "' AND([transaction].STATUS = '1' or [transaction].STATUS = '2') AND [transaction].TRANSACTION_ID LIKE '%" + t_search_trans.text +"%'";
                holding(query2);
            }
        }

        //========================================================================================


        //=================== GET DATA ID===================================================
        public void get_data_id()
        {
            string command;
            CRUD sql = new CRUD();

            //ckon.con.Close();
            //String sql = "SELECT * FROM transaction WHERE TRANSACTION_ID='" + id_list + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    t_custId.Text = ckon.myReader.GetString("CUSTOMER_ID");
            //    //t_employId.Text = ckon.myReader.GetString("EMPLOYEE_ID");
            //    t_spgId.Text = ckon.myReader.GetString("SPG_ID");
            //    //l_diskon.Text = ckon.myReader.GetString("DISCOUNT");
            //}
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM [transaction] WHERE TRANSACTION_ID='" + id_list + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        t_custId.Text = ckon.sqlDataRd["CUSTOMER_ID"].ToString();                        
                        t_spgId.Text = ckon.sqlDataRd["SPG_ID"].ToString();                        
                    }
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
        //==================================================================================
        //===========================ITUNG TOTAL BELANJA=====================================================
        public void itung_total()
        {
            string command;
            CRUD sql = new CRUD();

            //ckon.con.Close();
            ////=====================================GET VALUE DISCOUNT FROM TRANSACTION HEADER======================
            //String sql2 = "SELECT * FROM TRANSACTION WHERE TRANSACTION_ID ='" + l_transaksi.Text + "'";
            //ckon.cmd = new MySqlCommand(sql2, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        get_diskon = ckon.myReader.GetInt32("DISCOUNT");
            //        get_voucher = ckon.myReader.GetInt32("VOUCHER");
            //    }
            //    get_dis_vou = get_diskon + get_voucher;
            //    ckon.con.Close();
            //    if (get_diskon == 0)
            //    { 
            //        l_diskon.Text = "0,00";
            //    }
            //    else
            //    { 
            //        l_diskon.Text = string.Format("{0:#,###}" + ",00", get_diskon);
            //    }
            //    if(get_voucher==0)
            //    { l_vou.Text = "0,00"; }
            //    else
            //    { l_vou.Text = string.Format("{0:#,###}" + ",00", get_voucher); }
            //}
            //catch
            //{
            //    get_dis_vou = 0;
            //    l_diskon.Text = "0,00";
            //    l_vou.Text = "0,00";
            //}

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM [TRANSACTION] WHERE TRANSACTION_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.Read())
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        get_diskon = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                        get_voucher = Convert.ToInt32(ckon.sqlDataRd["VOUCHER"].ToString());
                    }

                    get_dis_vou = get_diskon + get_voucher;
                    if (get_diskon == 0)                    
                        l_diskon.Text = "0,00";                    
                    else                    
                        l_diskon.Text = string.Format("{0:#,###}" + ",00", get_diskon);                    

                    if (get_voucher == 0)
                        l_vou.Text = "0,00";
                    else
                        l_vou.Text = string.Format("{0:#,###}" + ",00", get_voucher);
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

            //==========================================================================================

            //ckon.con.Close();
            //String sql = "SELECT SUM(transaction_line.SUBTOTAL) as total FROM transaction_line WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        totall = ckon.myReader.GetInt32("total");
            //        totall = totall - get_voucher;
            //        l_total.Text = string.Format("{0:#,###}" + ",00", totall);
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT SUM(transaction_line.SUBTOTAL) as total FROM transaction_line WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        totall = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                        totall = totall - get_voucher;
                        l_total.Text = string.Format("{0:#,###}" + ",00", totall);
                    }
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
        //===================================================================================

        //==================================================================================
        public void total_trans()
        {
            string command;            
            int qty = 0;
            int total_qty = 0;
            int cash2 = 0;
            int edc_total = 0;
            int petty = 0;
            CRUD sql = new CRUD();            

            //ckon.con.Close();
            //int qty = 0;
            //int total_qty = 0;
            //String sql1 = "SELECT * FROM transaction WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
            //ckon.cmd = new MySqlCommand(sql1, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    String id_trans = ckon.myReader.GetString("TRANSACTION_ID");
            //    String sql11 = "SELECT SUM(transaction_line.QUANTITY) as total from transaction_line WHERE TRANSACTION_ID='" + id_trans + "'";
            //    ckon2.cmd2 = new MySqlCommand(sql11, ckon2.con2);
            //    ckon2.con2.Open();
            //    ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //    while (ckon2.myReader2.Read())
            //    {
            //        qty = ckon2.myReader2.GetInt32("total");
            //        total_qty = total_qty + qty;
            //    }
            //    ckon2.con2.Close();
            //    total_qty2 = total_qty;
            //}
            //l_qty.Text = total_qty.ToString();
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT SUM(transaction_line.QUANTITY) as total FROM [transaction] INNER JOIN transaction_line "
                            + "ON transaction_line.TRANSACTION_ID = [transaction].TRANSACTION_ID "
                            + "WHERE [transaction].ID_C_STORE = '" + id_Cstore + "' AND([transaction].STATUS = '1' or [transaction].STATUS = '2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        qty = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                        total_qty = total_qty + qty;
                    }
                }
                else
                {
                    total_qty = 0;
                }

                l_qty.Text = total_qty.ToString();         
                //========================================================================================================
                date = mydate.ToString("yyyy-MM-dd");

                //String sql = "SELECT SUM(transaction.TOTAL) as total FROM transaction WHERE ID_C_STORE = '" + id_Cstore + "' AND(STATUS='1' or STATUS='2')";

                //ckon.cmd = new MySqlCommand(sql, ckon.con);
                //ckon.con.Close();
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{

                //    try
                //    { amount = ckon.myReader.GetInt32("total");
                //        if(amount <= 0)
                //        { l_total_amount.Text = "0,00"; }
                //        else
                //        { l_total_amount.Text = string.Format("{0:#,###}" + ",00", amount); }

                //        //l_total_amount.Text = amount.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
                //    }
                //    catch
                //    {
                //        l_total_amount.Text = "0,00";
                //    }
                //}
                //ckon.con.Close();
                           
                command = "SELECT SUM([transaction].TOTAL) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND(STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        amount = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                        if (amount <= 0)
                            l_total_amount.Text = "0,00";
                        else
                            l_total_amount.Text = string.Format("{0:#,###}" + ",00", amount);                        
                    }
                }
                else
                {
                    l_total_amount.Text = "0,00";
                }        
                //===================================================================================================

                //String sql2 = "SELECT SUM(transaction.CASH) as total FROM transaction WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                //ckon.cmd = new MySqlCommand(sql2, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    try
                //    { cash = ckon.myReader.GetInt32("total");
                //        //l_cash.Text = cash.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
                //    }
                //    catch
                //    { cash = 0; }
                //}
                //ckon.con.Close();
                           
                command = "SELECT SUM([transaction].CASH) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        cash = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                    }
                }
                else
                {
                    cash = 0;
                }
          
                //String sql2a = "SELECT SUM(transaction.CHANGEE) as total FROM transaction WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                //ckon.cmd = new MySqlCommand(sql2a, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    try
                //    {
                //        change = ckon.myReader.GetInt32("total");
                //        //l_cash.Text = cash.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
                //    }
                //    catch
                //    { change = 0; }
                //}
                //ckon.con.Close();
                //int cash2 = cash - change;
                //if(cash2 <= 0)
                //{ l_cash.Text = "0,00"; }
                //else
                //{ l_cash.Text = string.Format("{0:#,###}" + ",00", cash2); }
                
                command = "SELECT SUM([transaction].CHANGEE) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        change = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                    }
                }
                else
                {
                    change = 0;
                }
         
                cash2 = cash - change;
                if (cash2 <= 0)
                    l_cash.Text = "0,00";
                else
                    l_cash.Text = string.Format("{0:#,###}" + ",00", cash2);
                //===================================================================================================

                //String sql3 = "SELECT SUM(transaction.DISCOUNT) as total FROM transaction WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                //ckon.cmd = new MySqlCommand(sql3, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    try
                //    { diskon = ckon.myReader.GetInt32("total");
                //        if(diskon <= 0)
                //        { l_discount.Text = "0,00"; }
                //        else
                //        { l_discount.Text = string.Format("{0:#,###}" + ",00", diskon); }
                //        //l_discount.Text = diskon.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
                //    }
                //    catch
                //    {
                //        l_discount.Text = "0,00";
                //    }
                //}
                //ckon.con.Close();
                          
                command = "SELECT SUM([transaction].DISCOUNT) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());
                
                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        diskon = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                        if (diskon <= 0)
                            l_discount.Text = "0,00";
                        else
                            l_discount.Text = string.Format("{0:#,###}" + ",00", diskon);                   
                    }
                }
                else
                {
                    l_discount.Text = "0,00";
                }           
                //===================================================================================================

                //String sql4 = "SELECT SUM(transaction.EDC) as total FROM transaction WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                //ckon.cmd = new MySqlCommand(sql4, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    try
                //    {
                //        edc = ckon.myReader.GetInt32("total");
                //        //if(edc <= 0)
                //        //{ l_edc.Text = "0,00"; }
                //        //else
                //        //{ l_edc.Text = string.Format("{0:#,###}" + ",00", edc); }
                //        //l_edc.Text = edc.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
                //    }
                //    catch
                //    {
                //        edc = 0;
                //        //l_edc.Text = "0,00";
                //    }
                //}
                //ckon.con.Close();
                
                command = "SELECT SUM([transaction].EDC) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        edc = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                    }
                }
                else
                {
                    edc = 0;
                }
           
                //String sql5 = "SELECT SUM(transaction.EDC2) as total FROM transaction WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                //ckon.cmd = new MySqlCommand(sql5, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    try
                //    {
                //        edc2 = ckon.myReader.GetInt32("total");
                //    }
                //    catch
                //    {
                //        edc2=0;
                //    }
                //}
                //ckon.con.Close();
                //int edc_total = edc + edc2;
                //if (edc_total <= 0)
                //{ l_edc.Text = "0,00"; }
                //else
                //{ l_edc.Text = string.Format("{0:#,###}" + ",00", edc_total); }
                           
                command = "SELECT SUM([transaction].EDC2) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        edc2 = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                    }
                }
                else
                {
                    edc2 = 0;
                }
           
                edc_total = edc + edc2;
                if (edc_total <= 0)
                    l_edc.Text = "0,00";
                else
                    l_edc.Text = string.Format("{0:#,###}" + ",00", edc_total);
                //===================================================================================================
                //String sql6 = "SELECT SUM(transaction.VOUCHER) as total FROM transaction WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                //ckon.cmd = new MySqlCommand(sql6, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    try
                //    {
                //        voucher = ckon.myReader.GetInt32("total");
                //        if (voucher <= 0)
                //        { l_voucher.Text = "0,00"; }
                //        else
                //        { l_voucher.Text = string.Format("{0:#,###}" + ",00", voucher); }
                //        //l_discount.Text = diskon.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
                //    }
                //    catch
                //    {
                //        l_voucher.Text = "0,00";
                //    }
                //}
                //ckon.con.Close();
                           
                command = "SELECT SUM([transaction].VOUCHER) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        voucher = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                        if (voucher <= 0)
                            l_voucher.Text = "0,00";
                        else
                            l_voucher.Text = string.Format("{0:#,###}" + ",00", voucher);
                    }
                }
                else
                {
                    l_voucher.Text = "0,00";
                }            
                //============MENDAPATKAN PETTY CASH YG TERSISA DARI TABEL STORE===========

                //String sql7 = "Select * from store";
                //ckon.cmd = new MySqlCommand(sql7, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    int petty = ckon.myReader.GetInt32("BUDGET_TO_STORE");
                //    if (petty == 0)
                //    {
                //        l_petty.Text = "0,00";
                //    }
                //    else
                //    {
                //        l_petty.Text = string.Format("{0:#,###}" + ",00", petty);
                //    }
                //}
                //ckon.con.Close();
                           
                command = "SELECT STORE.BUDGET_TO_STORE FROM STORE";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        petty = Convert.ToInt32(ckon.sqlDataRd["BUDGET_TO_STORE"].ToString());
                        if (petty == 0)
                        {
                            l_petty.Text = "0,00";
                        }
                        else
                        {
                            l_petty.Text = string.Format("{0:#,###}" + ",00", petty);
                        }
                    }
                }
                else
                {
                    l_petty.Text = "0,00";
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
        //==================================================================================

        //================KLIK TABEL TRANSAKSI LIST==========================================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
            l_transaksi.Text = id_list;
            retreive();
            itung_total();
            get_data_id();
        }
        //====================================================================================

        //====================================================================================
        private void b_edc_Click(object sender, EventArgs e)
        {
            w_edc_closing_store edc_close = new w_edc_closing_store();
            edc_close.id_Cstore2 = id_Cstore;
            edc_close.ShowDialog();
        }
        //====================================================================================
        //================button closing store ===============================================
        private void b_print_Click(object sender, EventArgs e)
        {
            w_form_closing closing = new w_form_closing(f1);
            //closing.itung_cash();
            closing.set_name2(epy_id, epy_name);
            closing.get_qty(total_qty2);
            closing.id_cStrore2 = id_Cstore;
            closing.date_closing_store3 = date_close_store2;
            closing.ShowDialog();
        }
        //====================================================================================
    }
}
