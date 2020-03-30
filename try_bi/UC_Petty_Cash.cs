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
using Tulpep.NotificationWindow;

namespace try_bi
{
    public partial class UC_Petty_Cash : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String id_trans, exp_ctg, exp_date, exp_total, id_list, S_Articleid,store_code, exp_code, bulan2, tipe2;
        int amount, totall, noo_inv_new, bg_ToStore, bg_ToCasir, tot_exp, no_trans2;
        DateTime mydate = DateTime.Now;
        DateTime myhour = DateTime.Now;
        public string cust_id_store2;

        //Variable untuk running number baru
        String bulan_now, tahun_now, bulan_trans, number_trans_string, final_running_number;
        int number_trans;
        //======================================================
        private static UC_Petty_Cash _instance;
        public static UC_Petty_Cash Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Petty_Cash(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_Petty_Cash(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //=================GET NUMBER RUNNING=========================
        /*
        public void get_running_number(String number, String bulan, int no_trans, String tipe)
        {
            l_transaksi.Text = number;
            bulan2 = bulan;
            no_trans2 = no_trans;
            tipe2 = tipe;
        }
        */
        //=================================GENERATOR NUMBER=================================
        public void new_invoice()
        {
            CRUD sql = new CRUD();

            dgv_petty.Rows.Clear(); 
            combo_expense.SelectedIndex = -1; 
            l_total.Text = "0";
            //=====================================================
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        store_code = ckon.sqlDataRd["CODE"].ToString();
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

            //ckon.cmd = new MySqlCommand(sql2, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    if (ckon.myReader.HasRows)
            //    {
            //        while (ckon.myReader.Read())
            //        {
            //            store_code = ckon.myReader.GetString("CODE");
            //        }
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ MessageBox.Show("Failed when get data from store data"); }
            //==========================================================================================

            //string sql = "SELECT SUBSTRING(PETTY_CASH_ID, 8) AS inv FROM pettycash ORDER BY PETTY_CASH_ID DESC LIMIT 1";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    if (ckon.myReader.HasRows)
            //    {
            //        while (ckon.myReader.Read())
            //        {
            //             noo_inv_new = ckon.myReader.GetInt32("inv");
            //            noo_inv_new = noo_inv_new + 1;
            //            //l_transaksi.Text = "PC-" + noo_inv_new.ToString();
            //        }
            //        if(noo_inv_new < 10)
            //        { l_transaksi.Text = store_code + "/PC-000" + noo_inv_new.ToString(); }
            //        else if(noo_inv_new < 100)
            //        { l_transaksi.Text = store_code + "/PC-00" + noo_inv_new.ToString(); }
            //        else if(noo_inv_new < 1000 )
            //        { l_transaksi.Text = store_code + "/PC-0" + noo_inv_new.ToString();}
            //        else if(noo_inv_new < 10000)
            //        { l_transaksi.Text = store_code + "/PC-" + noo_inv_new.ToString(); }
            //        else
            //        { }
            //    }
            //    else
            //    { l_transaksi.Text = store_code +"/PC-0001"; }
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        //==================================================================================

        //=====================ITUNG TOTAL BUDGET===========================================
        public void get_budget()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        bg_ToStore = Convert.ToInt32(ckon.sqlDataRd["BUDGET_TO_STORE"].ToString());
                    }

                    if (bg_ToStore == 0)
                        l_budget.Text = "0,00";
                    else
                        l_budget.Text = String.Format("{0:#,###}" + ",00", bg_ToStore);                   
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
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if(ckon.myReader.HasRows)
            //{
            //    while(ckon.myReader.Read())
            //    {
            //        bg_ToStore = ckon.myReader.GetInt32("BUDGET_TO_STORE");
            //        //bg_ToCasir = ckon.myReader.GetInt32("BUDGET_TO_CASHIER");
            //    }
            //    if(bg_ToStore==0)
            //    { l_budget.Text = "0,00"; }
            //    else
            //    { l_budget.Text = String.Format("{0:#,###}" + ",00", bg_ToStore); }
            //    //if(bg_ToCasir==0)
            //    //{ l_b_ToCashier.Text = "0,00"; }
            //    //else
            //    //{ l_b_ToCashier.Text = String.Format("{0:#,###}" + ",00", bg_ToCasir); }

            //}
            //ckon.con.Close();
        }
        //==================================================================================

        //=======================TOTAL EXPENSE==============================================
        public void get_expense()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT SUM(pettycash.TOTAL_EXPENSE) as total FROM pettycash WHERE STATUS='1'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        if (ckon.sqlDataRd["total"].ToString() != "")
                            tot_exp = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());

                        if (tot_exp <= 0)
                            l_expense.Text = "0,00";
                        else                                                   
                            l_expense.Text = string.Format("{0:#,###}" + ",00", tot_exp);                        
                    }
                }
                else
                {
                    l_expense.Text = "0,00";
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{

            //    try
            //    {
            //        tot_exp = ckon.myReader.GetInt32("total");
            //        if (tot_exp <= 0)
            //        { l_expense.Text = "0,00"; }
            //        else
            //        {
            //            //bg_ToStore = bg_ToStore - tot_exp;
            //            //l_budget.Text = string.Format("{0:#,###}" + ",00", bg_ToStore);
            //            l_expense.Text = string.Format("{0:#,###}" + ",00", tot_exp);
            //        }

            //        //l_total_amount.Text = amount.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
            //    }
            //    catch
            //    {
            //        l_expense.Text = "0,00";
            //    }
            //}
            //ckon.con.Close();
        }
        //==================================================================================

        
        //==============================ISI COMBOBOX ===========================
        public void isi_combo_expanse()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            combo_expense.Items.Clear();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM costcategory";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        String name = ckon.sqlDataRd["NAME"].ToString();
                        combo_expense.Items.Add(name);
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        String name = ckon.myReader.GetString("NAME");
            //        combo_expense.Items.Add(name);
            //    }
            //    ckon.con.Close();
            //}
            //catch(Exception ex)
            //{ MessageBox.Show(" "+ex); }
        }

        //======================================================================
        private void combo_expense_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM costcategory WHERE NAME='" + combo_expense.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        exp_code = ckon.sqlDataRd["COST_CATEGORY_ID"].ToString();
                    }
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        exp_code = ckon.myReader.GetString("COST_CATEGORY_ID");

            //    }
            //    ckon.con.Close();
            //}
            //catch 
            //{  }
        }

        //======================LIST HOLD TRANSACTION============================================
        public void holding()
        {
            CRUD sql = new CRUD();
            dgv_hold.Rows.Clear();

            //ckon.con.Close();
            //string date = tanggal;
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM pettycash WHERE STATUS='0' ORDER BY EXPENSE_DATE ASC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["PETTY_CASH_ID"].ToString();
                        exp_ctg = ckon.sqlDataRd["EXPENSE_CATEGORY"].ToString();
                        exp_date = ckon.sqlDataRd["EXPENSE_DATE"].ToString();
                        exp_total = ckon.sqlDataRd["TOTAL_EXPENSE"].ToString();
                        int exp_total2 = Int32.Parse(exp_total);
                        int n = dgv_hold.Rows.Add();
                        dgv_hold.Rows[n].Cells[0].Value = id_trans;
                        dgv_hold.Rows[n].Cells[1].Value = exp_ctg;
                        dgv_hold.Rows[n].Cells[2].Value = exp_total2;
                    }
                    dgv_hold.Columns[2].DefaultCellStyle.Format = "#,###";
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();

            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_trans = ckon.myReader.GetString("PETTY_CASH_ID");
            //        exp_ctg = ckon.myReader.GetString("EXPENSE_CATEGORY");
            //        exp_date = ckon.myReader.GetString("EXPENSE_DATE");
            //        exp_total = ckon.myReader.GetString("TOTAL_EXPENSE");
            //        int exp_total2 = Int32.Parse(exp_total);
            //        int n = dgv_hold.Rows.Add();
            //        dgv_hold.Rows[n].Cells[0].Value = id_trans;
            //        dgv_hold.Rows[n].Cells[1].Value = exp_ctg;
            //        dgv_hold.Rows[n].Cells[2].Value = exp_total2;

            //    }
            //    dgv_hold.Columns[2].DefaultCellStyle.Format = "#,###";
            //}
            //ckon.con.Close();
        }
        //=============================================================================================

        //===============TAMPILKAN DATA PETTY CASH ====================================================
        public void retreive()
        {
            CRUD sql = new CRUD();

           dgv_petty.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM pettycash_line WHERE PETTY_CASH_ID = '" + l_transaksi.Text + "' ORDER BY _id ASC";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_petty.Rows.Add();
                    dgv_petty.Rows[dgRows].Cells[0].Value = row["_id"].ToString();
                    dgv_petty.Rows[dgRows].Cells[1].Value = row["EXPENSE_NAME"].ToString();
                    dgv_petty.Rows[dgRows].Cells[2].Value = row["QUANTITY"].ToString();
                    dgv_petty.Rows[dgRows].Cells[3].Value = row["PRICE"];
                    dgv_petty.Rows[dgRows].Cells[4].Value = row["TOTAL"];
                }
                dgv_petty.Columns[3].DefaultCellStyle.Format = "#,###";
                dgv_petty.Columns[4].DefaultCellStyle.Format = "#,###";
            }
            catch (Exception e)
            {
                MessageBox.Show("No connection to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ckon.dt.Rows.Clear();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);
            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_petty.Rows.Add();
            //        dgv_petty.Rows[n].Cells[0].Value = row["_id"].ToString();
            //        dgv_petty.Rows[n].Cells[1].Value = row["EXPENSE_NAME"].ToString();
            //        dgv_petty.Rows[n].Cells[2].Value = row["QUANTITY"].ToString();
            //        dgv_petty.Rows[n].Cells[3].Value = row["PRICE"];
            //        dgv_petty.Rows[n].Cells[4].Value = row["TOTAL"];
            //    }
            //    dgv_petty.Columns[3].DefaultCellStyle.Format = "#,###";
            //    dgv_petty.Columns[4].DefaultCellStyle.Format = "#,###";
            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }

        }
        //===================================================================================


        //===================================================================================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    update_petty_header();
                    id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    //MessageBox.Show(id_list+"");
                    l_transaksi.Text = id_list;
                    holding();

                    get_data_id();
                    retreive();
                    itung_total();
                }
            }
            catch (Exception)
            {

            }
        }



        //===================================================================================

        //===========================GET DATA BY ID ================================================
        public void get_data_id()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM pettycash WHERE PETTY_CASH_ID='" + id_list + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {                        
                        d_tgl_expense.Text = ckon.sqlDataRd["EXPENSE_DATE"].ToString();                        
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{


            //    combo_expense.Text = ckon.myReader.GetString("EXPENSE_CATEGORY");
            //    d_tgl_expense.Text = ckon.myReader.GetString("EXPENSE_DATE");
            //    amount = ckon.myReader.GetInt32("TOTAL_EXPENSE");

            //    t_desc.Text = ckon.myReader.GetString("DESCRIPTION");
            //}
            //l_total.Text = string.Format("{0:#,###}" + ",00", amount);
            //ckon.con.Close();
            //l_total.Text = amount.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
        }

        //==========================================================================================

        //======================SIMPAN PETTY CASH HEADER============================================
        public void save_petty_header()
        {
            CRUD sql = new CRUD();
            DateTime mydate = DateTime.Now;
            DateTime myhour = DateTime.Now;
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM pettycash WHERE PETTY_CASH_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (!ckon.sqlDataRd.HasRows)
                {
                    String cmd_insert = "INSERT INTO pettycash (PETTY_CASH_ID,STORE_CODE,EXPENSE_CATEGORY_ID,EXPENSE_CATEGORY,EXPENSE_DATE,STATUS, DATE,TIME,CUST_ID_STORE) VALUES ('" + l_transaksi.Text + "','" + store_code + "','" + exp_code + "', '" + combo_expense.Text + "' , '" + d_tgl_expense.Text + "','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_store2 + "') ";
                    sql.ExecuteNonQuery(cmd_insert);

                    String cmd_update = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='6'";
                    sql.ExecuteNonQuery(cmd_update);
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

            //ckon.cmd = new MySqlCommand(sql0, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //int count0 = 0;
            //while (ckon.myReader.Read())
            //{
            //    count0 = count0 + 1;
            //}
            //ckon.con.Close();
            //if (count0 == 0)
            //{
            //    String sql = "INSERT INTO pettycash (PETTY_CASH_ID,STORE_CODE,EXPENSE_CATEGORY_ID,EXPENSE_CATEGORY,EXPENSE_DATE,STATUS, DATE,TIME,CUST_ID_STORE) VALUES ('" + l_transaksi.Text + "','"+ store_code +"','"+exp_code +"', '" + combo_expense.Text + "' , '" + d_tgl_expense.Text + "','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','"+ cust_id_store2 +"') ";
            //    ckon.con.Open();
            //    ckon.cmd = new MySqlCommand(sql, ckon.con);
            //    ckon.cmd.ExecuteNonQuery();
            //    ckon.con.Close();

            //    String query = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='6'";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);
            //}
            //else
            //{

            //}

        }
        //==========================================================================================

        //===========================ITUNG TOTAL BELANJA=====================================================
        public void itung_total()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT SUM(pettycash_line.TOTAL) as total FROM pettycash_line WHERE PETTY_CASH_ID='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        totall = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                    }
                    l_total.Text = string.Format("{0:#,###}" + ",00", totall);
                }
                else
                {
                    l_total.Text = "0";
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        totall = ckon.myReader.GetInt32("total");
            //    }
            //    ckon.con.Close();
            //    l_total.Text = string.Format("{0:#,###}" + ",00", totall);
            //    //l_total.Text = totall.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
            //}
            //catch
            //{ l_total.Text = "0"; }

        }


        //===================================================================================

        //====================UPDATE PETTY CASH HEADER=========================
        public void update_petty_header()
        {
            //MessageBox.Show(totall.ToString());
            String cmd_update = "UPDATE pettycash SET EXPENSE_DATE='" + d_tgl_expense.Text + "', TOTAL_EXPENSE = '" + totall + "' WHERE PETTY_CASH_ID='" + l_transaksi.Text + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }

        private void dgv_petty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_petty.Columns[e.ColumnIndex].Name == "Delete")
            {
                S_Articleid = dgv_petty.SelectedRows[0].Cells[0].Value.ToString();
                String cmd_delete = "DELETE FROM pettycash_line WHERE PETTY_CASH_ID='" + l_transaksi.Text + "' AND _id='" + S_Articleid + "'";
                CRUD input = new CRUD();
                input.ExecuteNonQuery(cmd_delete);
                retreive();
                itung_total();
            }
        }

        //=====================================================================

        //====================BUTTON ADD LINE ===================================
        private void b_ok_Click(object sender, EventArgs e)
        {
            w_input_expense input = new w_input_expense();
            input.id_petty = l_transaksi.Text;
            input.ShowDialog();
        }

        //=======================================================================

        //====================BUTTON CHARGE=====================================
        private void b_charge_Click(object sender, EventArgs e)
        {
            if (l_total.Text == "0")
            {
                //PopupNotifier pop = new PopupNotifier();
                //pop.TitleText = "Warning";
                //pop.ContentText = "No Item On List";
                //pop.Popup();
                MessageBox.Show("No Item On List");
            }
            else
            {
                //MEMBUKA WINDOWS FORM DIALOG
                W_PettyCash_Confirm confirm = new W_PettyCash_Confirm();
                confirm.ShowDialog();
                //if (MessageBox.Show("Confirm Expense ?", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                //{
                //    if(totall > bg_ToStore)
                //    {
                //        MessageBox.Show("Budget Exceeded");
                //    }
                //    else
                //    {
                //        int total_budget = bg_ToStore - totall;
                //        //String sql = "UPDATE pettycash SET EXPENSE_CATEGORY='" + combo_expense.Text + "', EXPENSE_DATE='" + d_tgl_expense.Text + "', DESCRIPTION = '" + t_desc.Text + "', TOTAL_EXPENSE = '" + totall + "', STATUS = 1 WHERE PETTY_CASH_ID='" + l_transaksi.Text + "'";
                //        //=============UPDATE PETTY CASH HEADER========================
                //        String sql = "UPDATE pettycash SET  EXPENSE_DATE='" + d_tgl_expense.Text + "', TOTAL_EXPENSE = '" + totall + "', STATUS = 1 WHERE PETTY_CASH_ID='" + l_transaksi.Text + "'";
                //        CRUD update = new CRUD();
                //        update.ExecuteNonQuery(sql);
                //        //===========UPDATE BUDGET TO STORE IN STORE TABLE====================
                //        String sql2 = "UPDATE store SET BUDGET_TO_STORE = '" + total_budget + "'";
                //        CRUD update2 = new CRUD();
                //        update2.ExecuteNonQuery(sql2);
                //        //==============
                //        new_invoice();
                //        combo_expense.SelectedIndex = -1; l_total.Text = "0"; dgv_petty.Rows.Clear();
                //        holding();
                //        get_budget();
                //        get_expense();
                //    }

                //}
                
            }
        }
        //======================================================================


        //==================BUTTON PETTY CASH LIST  =======================
        private void b_list_PC_Click(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM pettycash WHERE STATUS='1' ORDER BY EXPENSE_DATE ASC";
            UC_Petty_Cash_List c = new UC_Petty_Cash_List(f1);
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_Petty_Cash_List.Instance))
            {
                f1.p_kanan.Controls.Add(UC_Petty_Cash_List.Instance);
                UC_Petty_Cash_List.Instance.Dock = DockStyle.Fill;
                UC_Petty_Cash_List.Instance.holding(sql);
                UC_Petty_Cash_List.Instance.get_budget();
                UC_Petty_Cash_List.Instance.get_expense();
                UC_Petty_Cash_List.Instance.Show();
            }
            else
            {
                UC_Petty_Cash_List.Instance.holding(sql);
                UC_Petty_Cash_List.Instance.get_budget();
                UC_Petty_Cash_List.Instance.get_expense();
                UC_Petty_Cash_List.Instance.Show();
            }
        }
        //=========================================================================

        //=========================BUTTON HOLD=====================
        private void b_hold_Click(object sender, EventArgs e)
        {
            if (l_total.Text == "0")
            {
                //PopupNotifier pop = new PopupNotifier();
                //pop.TitleText = "Warning";
                //pop.ContentText = "Pick Item First";
                //pop.Popup();
                MessageBox.Show("Pick Item First");
            }
            else
            {
                update_petty_header();
                //RunningNumber running = new RunningNumber();
                //running.get_data_before("6", "PC");
                new_invoice();
                set_running_number();
                combo_expense.SelectedIndex = -1; l_total.Text = "0"; dgv_petty.Rows.Clear();
                holding();
            }
        }
        //=========================================================

        //=================BUTTON CLEAR============================
        private void b_clear_Click(object sender, EventArgs e)
        {
            if (l_total.Text == "0")
            {
                //PopupNotifier pop = new PopupNotifier();
                //pop.TitleText = "Warning";
                //pop.ContentText = "No Item On List";
                //pop.Popup();
                MessageBox.Show("No Item On List");
            }
            else
            {
                if(MessageBox.Show("Clear Item On List = ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)== DialogResult.OK)
                {
                    String cmd_delete = "DELETE FROM pettycash_line WHERE PETTY_CASH_ID = '" + l_transaksi.Text + "'";
                    CRUD del = new CRUD();
                    del.ExecuteNonQuery(cmd_delete);
                    dgv_petty.Rows.Clear();
                    l_total.Text = "0";
                }

            }
        }
        //=============================COBA METHOD DENGAN TAMPILAN BARU WINDOWS FORM, JIKA DI OK
        public void method_confim_ok()
        {
            if (totall > bg_ToStore)
            {
                MessageBox.Show("Budget Exceeded");
            }
            else
            {
                int total_budget = bg_ToStore - totall;
                //String sql = "UPDATE pettycash SET EXPENSE_CATEGORY='" + combo_expense.Text + "', EXPENSE_DATE='" + d_tgl_expense.Text + "', DESCRIPTION = '" + t_desc.Text + "', TOTAL_EXPENSE = '" + totall + "', STATUS = 1 WHERE PETTY_CASH_ID='" + l_transaksi.Text + "'";
                //=============UPDATE PETTY CASH HEADER========================
                String cmd_updatePtc = "UPDATE pettycash SET  EXPENSE_DATE='" + d_tgl_expense.Text + "', TOTAL_EXPENSE = '" + totall + "', STATUS = 1 WHERE PETTY_CASH_ID='" + l_transaksi.Text + "'";
                CRUD update = new CRUD();
                update.ExecuteNonQuery(cmd_updatePtc);
                //===========UPDATE BUDGET TO STORE IN STORE TABLE====================
                String cmd_updateStore = "UPDATE store SET BUDGET_TO_STORE = '" + total_budget + "'";
                CRUD update2 = new CRUD();
                update2.ExecuteNonQuery(cmd_updateStore);
                //==============
                //RunningNumber running = new RunningNumber();
                //running.get_data_before("6", "PC");
                new_invoice();
                set_running_number();
                combo_expense.SelectedIndex = -1; l_total.Text = "0"; dgv_petty.Rows.Clear();
                holding();
                get_budget();
                get_expense();
            }
        }
        //====FUNGSI BARU RUNNING NUMBER, UNTUK MENGATASI BUG NUMBER SEQ MACET
        public void set_running_number()
        {
            get_year_month();
            get_running_number();
        }
        //====METHOD GET MOUNT AND YEAR PRESENT=================
        public void get_year_month()
        {
            DateTime mydate = DateTime.Now;
            DateTime myhour = DateTime.Now;

            bulan_now = mydate.ToString("MM");
            tahun_now = mydate.ToString("yy");
        }
        //=========METHOD GET DATA FROM AUTO_NUMBER TABLE FOR SALES TRANSACTION
        public void get_running_number()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '6'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        bulan_trans = ckon.sqlDataRd["Month"].ToString();
                        number_trans = Convert.ToInt32(ckon.sqlDataRd["Number"].ToString());
                    }
                    if (bulan_now == bulan_trans)
                    {
                        number_trans = number_trans + 1;
                        if (number_trans < 10)
                        { number_trans_string = "0000" + number_trans.ToString(); }
                        else if (number_trans < 100)
                        { number_trans_string = "000" + number_trans.ToString(); }
                        else if (number_trans < 1000)
                        { number_trans_string = "00" + number_trans.ToString(); }
                        else if (number_trans < 10000)
                        { number_trans_string = "0" + number_trans.ToString(); }
                        else
                        { number_trans_string = number_trans.ToString(); }
                        //==MEMBUAT STRING FINAL RUNNING NUMBER
                        final_running_number = "PC/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
                        l_transaksi.Text = final_running_number;

                    }
                    else
                    {
                        number_trans = 1;
                        bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
                                                //==MEMBUAT STRING FINAL RUNNING NUMBER
                        final_running_number = "PC/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                        l_transaksi.Text = final_running_number;
                    }
                }
                else
                {
                    number_trans = 1;
                    bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
                    final_running_number = "PC/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                    l_transaksi.Text = final_running_number;

                    String cmd_insert = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','6')";                    
                    sql.ExecuteNonQuery(cmd_insert);

                    //MessageBox.Show(final_running_number);
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

            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        //tahun_trans = ckon.myReader.GetString("Year");
            //        bulan_trans = ckon.myReader.GetString("Month");
            //        number_trans = ckon.myReader.GetInt32("Number");
            //    }
            //    if (bulan_now == bulan_trans)
            //    {
            //        number_trans = number_trans + 1;
            //        if (number_trans < 10)
            //        { number_trans_string = "0000" + number_trans.ToString(); }
            //        else if (number_trans < 100)
            //        { number_trans_string = "000" + number_trans.ToString(); }
            //        else if (number_trans < 1000)
            //        { number_trans_string = "00" + number_trans.ToString(); }
            //        else if (number_trans < 10000)
            //        { number_trans_string = "0" + number_trans.ToString(); }
            //        else
            //        { number_trans_string = number_trans.ToString(); }
            //        //==MEMBUAT STRING FINAL RUNNING NUMBER
            //        final_running_number = "PC/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
            //        l_transaksi.Text = final_running_number;

            //    }
            //    else
            //    {
            //        number_trans = 1;
            //        bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
            //        //==MEMBUAT STRING FINAL RUNNING NUMBER
            //        final_running_number = "PC/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //        l_transaksi.Text = final_running_number;
            //    }


            //}
            //else
            //{
            //    number_trans = 1;
            //    bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
            //    final_running_number = "PC/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //    l_transaksi.Text = final_running_number;

            //    String query = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','6')";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);

            //    //MessageBox.Show(final_running_number);
            //}
            //ckon.con.Close();
        }


    }
}
