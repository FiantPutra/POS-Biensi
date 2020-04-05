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
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            String art_id, art_name, spg_id, discAmount, qty, disc, sub_total2;
            int price, sub_total;
            string command;
            CRUD sql = new CRUD();

            dgv_purchase.Rows.Clear();            

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT  transaction_line.ARTICLE_ID ,transaction_line.QUANTITY, transaction_line.SUBTOTAL, transaction_line.SPG_ID,transaction_line.DISCOUNT, transaction_line.DISCOUNT,article.ARTICLE_NAME, " +
                            "transaction_line.DISCOUNTAMOUNT, article.PRICE FROM transaction_line, article  WHERE article.ARTICLE_ID = transaction_line. ARTICLE_ID " +
                            "AND transaction_line.TRANSACTION_ID='" + l_transaksi.Text + "' ORDER BY transaction_line._id ASC";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        spg_id = ckon.sqlDataRd["SPG_ID"].ToString();
                        discAmount = ckon.sqlDataRd["DISCOUNTAMOUNT"].ToString();                        
                        price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        disc = ckon.sqlDataRd["DISCOUNT"].ToString();
                        sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                                                
                        int dgRows = dgv_purchase.Rows.Add();
                        dgv_purchase.Rows[dgRows].Cells[0].Value = art_id;
                        dgv_purchase.Rows[dgRows].Cells[1].Value = art_name;
                        dgv_purchase.Rows[dgRows].Cells[2].Value = spg_id;
                        dgv_purchase.Rows[dgRows].Cells[3].Value = qty;                            
                        dgv_purchase.Rows[dgRows].Cells[4].Value = price;
                        dgv_purchase.Rows[dgRows].Cells[5].Value = disc;
                        dgv_purchase.Rows[dgRows].Cells[6].Value = discAmount;
                        dgv_purchase.Rows[dgRows].Cells[7].Value = sub_total == 0 ? "0,00" : sub_total.ToString();                        
                    }
                }

                dgv_purchase.Columns[3].DefaultCellStyle.Format = "#,###";
                dgv_purchase.Columns[4].DefaultCellStyle.Format = "#,###";
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
        //==================================================================================
        //===========================ITUNG TOTAL BELANJA=====================================================
        public void itung_total()
        {
            string command;
            CRUD sql = new CRUD();            

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
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }

            //==========================================================================================            

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
                        qty = ckon.sqlDataRd["total"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["total"].ToString()) : 0;
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
                           
                command = "SELECT SUM([transaction].TOTAL) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND(STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        amount = ckon.sqlDataRd["total"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["total"].ToString()) : 0;
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
                           
                command = "SELECT SUM([transaction].CASH) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        cash = ckon.sqlDataRd["total"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["total"].ToString()) : 0;
                    }
                }
                else
                {
                    cash = 0;
                }                        
                
                command = "SELECT SUM([transaction].CHANGEE) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        change = ckon.sqlDataRd["total"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["total"].ToString()) : 0;
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
                          
                command = "SELECT SUM([transaction].DISCOUNT) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());
                
                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        diskon = ckon.sqlDataRd["total"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["total"].ToString()) : 0;
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
                
                command = "SELECT SUM([transaction].EDC) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        edc = ckon.sqlDataRd["total"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["total"].ToString()) : 0;
                    }
                }
                else
                {
                    edc = 0;
                }                          
                           
                command = "SELECT SUM([transaction].EDC2) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        edc2 = ckon.sqlDataRd["total"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["total"].ToString()) : 0;
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
                           
                command = "SELECT SUM([transaction].VOUCHER) as total FROM [transaction] WHERE ID_C_STORE = '" + id_Cstore + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        voucher = ckon.sqlDataRd["total"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["total"].ToString()) : 0;
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
                           
                command = "SELECT STORE.BUDGET_TO_STORE FROM STORE";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        petty = ckon.sqlDataRd["BUDGET_TO_STORE"].ToString() != "" ? Convert.ToInt32(ckon.sqlDataRd["BUDGET_TO_STORE"].ToString()) : 0;
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
