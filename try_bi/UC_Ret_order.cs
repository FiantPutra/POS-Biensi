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
    public partial class UC_Ret_order : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        String id_trans, article_id, qty2, id_list, store_code, cust_id_store, epy_id, epy_name, id_inv, art_id, unit, good_qty, _id, bulan2, tipe2;
        //String id_store;
        int noo_inv_new, GOOD_QTY_PLUS, no_trans2, total_amount, price;

        //Variable untuk running number baru
        String bulan_now, tahun_now, bulan_trans, number_trans_string, final_running_number;
        int number_trans;
        //======================================================
        private static UC_Ret_order _instance;

        private void dgv_request_FilterStringChanged(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        private void dev_request_Sorting(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        private void no_sj_Validated(object sender, EventArgs e)
        {
            int ab = no_sj.Text.Length;
            if (ab > 10)
            {
                MessageBox.Show("Maximum chars (No SJ) : 10");
                no_sj.Focus();
            }
        }

        public static UC_Ret_order Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Ret_order(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_Ret_order(Form1 form1)
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
        //===================GET ID EMPLOYEE AND NAME EPLOYEE=============
        public void set_name(String id, String name)
        {
            epy_id = id;
            epy_name = name;
        }
        //=================================GENERATOR NUMBER=================================
        public void new_invoice()
        {
            CRUD sql = new CRUD();

            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            dgv_request.Rows.Clear();
            l_amount.Text = "0,00";
            l_qty.Text = "0";
            no_sj.Text = "";
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd_store = "SELECT * FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_store, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        store_code = ckon.sqlDataRd["CODE"].ToString();
                        cust_id_store = ckon.sqlDataRd["CUST_ID_STORE"].ToString();
                    }
                }

                string cmd_ro = "SELECT TOP 1 SUBSTRING(RETURN_ORDER_ID, 8, 8) AS inv FROM returnorder ORDER BY RETURN_ORDER_ID DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_ro, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        noo_inv_new = Convert.ToInt32(ckon.sqlDataRd["inv"].ToString());
                        noo_inv_new = noo_inv_new + 1;
                    }

                    if (noo_inv_new < 10)
                    { l_transaksi.Text = store_code + "/RT-000" + noo_inv_new.ToString(); }
                    else if (noo_inv_new < 100)
                    { l_transaksi.Text = store_code + "/RT-00" + noo_inv_new.ToString(); }
                    else if (noo_inv_new < 1000)
                    { l_transaksi.Text = store_code + "/RT-0" + noo_inv_new.ToString(); }
                    else if (noo_inv_new < 10000)
                    { l_transaksi.Text = store_code + "/RT-" + noo_inv_new.ToString(); }
                }
                else
                {
                    l_transaksi.Text = store_code + "/RT-0001";
                }
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
                //            //l_transaksi.Text = "RT-" + noo_inv_new.ToString();
                //        }
                //        if (noo_inv_new < 10)
                //        { l_transaksi.Text = store_code + "/RT-000" + noo_inv_new.ToString(); }
                //        else if (noo_inv_new < 100)
                //        { l_transaksi.Text = store_code + "/RT-00" + noo_inv_new.ToString(); }
                //        else if (noo_inv_new < 1000)
                //        { l_transaksi.Text = store_code + "/RT-0" + noo_inv_new.ToString(); }
                //        else if (noo_inv_new < 10000)
                //        { l_transaksi.Text = store_code + "/RT-" + noo_inv_new.ToString(); }
                //        else
                //        { }
                //    }
                //    else
                //    { l_transaksi.Text = store_code + "/RT-0001"; }
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
            //            cust_id_store = ckon.myReader.GetString("CUST_ID_STORE");
            //        }
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ MessageBox.Show("Failed when get data from store data"); }
            //string sql = "SELECT SUBSTRING(RETURN_ORDER_ID, 8) AS inv  FROM returnorder ORDER BY RETURN_ORDER_ID DESC LIMIT 1";
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
            //            //l_transaksi.Text = "RT-" + noo_inv_new.ToString();
            //        }
            //        if (noo_inv_new < 10)
            //        { l_transaksi.Text = store_code + "/RT-000" + noo_inv_new.ToString(); }
            //        else if (noo_inv_new < 100)
            //        { l_transaksi.Text = store_code + "/RT-00" + noo_inv_new.ToString(); }
            //        else if (noo_inv_new < 1000)
            //        { l_transaksi.Text = store_code + "/RT-0" + noo_inv_new.ToString(); }
            //        else if (noo_inv_new < 10000)
            //        { l_transaksi.Text = store_code + "/RT-" + noo_inv_new.ToString(); }
            //        else
            //        { }
            //    }
            //    else
            //    { l_transaksi.Text = store_code + "/RT-0001"; }

            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        //==================================================================================

        //======================LIST HOLD TRANSACTION============================================
        public void holding()
        {
            CRUD sql = new CRUD();
            dgv_hold.Rows.Clear();
            List<string> numbersList = new List<string>();
            //koneksi2 ckon2 = new koneksi2();
            //ckon.con.Close();

            //date = mydate.ToString("yyyy-MM-dd");
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT article.ARTICLE_NAME FROM returnorder INNER JOIN returnorder_line "
                                + "ON returnorder_line.RETURN_ORDER_ID = returnorder.RETURN_ORDER_ID INNER JOIN article "
                                + "ON article.ARTICLE_ID = returnorder_line.ARTICLE_ID "
                                + "WHERE returnorder.STATUS = '0' ";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        article_id = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        numbersList.Add(Convert.ToString(ckon2.myReader2["ARTICLE_NAME"]));

                        string[] numbersArray = numbersList.ToArray();
                        numbersList.Clear();
                        string result = String.Join(", ", numbersArray);
                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_trans;
                        dgv_hold.Rows[dgRows].Cells[1].Value = result;
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
            //List<string> numbersList = new List<string>();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_trans = ckon.myReader.GetString("RETURN_ORDER_ID");
            //        String sql2 = "SELECT article.ARTICLE_NAME FROM returnorder_line, article  WHERE article.ARTICLE_ID = returnorder_line.ARTICLE_ID AND returnorder_line.RETURN_ORDER_ID='" + id_trans + "'";
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
            //        ckon2.con2.Close();
            //    }

            //}
            //ckon.con.Close();
        }
        //=======================================================================================
        //=====================ITEM REQUEST ORDER ==========================================
        public void retreive(string sort = "", string filter = "", string search = "")
        {
            CRUD sql = new CRUD();

            dgv_request.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT  returnorder_line.ARTICLE_ID ,returnorder_line.QUANTITY, returnorder_line.UNIT,returnorder_line.SUBTOTAL, article._id,article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE FROM returnorder_line, article  WHERE article.ARTICLE_ID = returnorder_line.ARTICLE_ID AND returnorder_line.RETURN_ORDER_ID='" + l_transaksi.Text + "' ";
                if (filter != "")
                {
                    filter = filter.Replace("ARTICLE_ID", "returnorder_line.ARTICLE_ID");
                    cmd += " AND " + filter;
                }
                if (sort != "")
                {
                    cmd += " ORDER BY " + sort;
                }
                else
                {
                    cmd += " ORDER BY returnorder_line._id DESC";
                }
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_request.Rows.Add();
                    dgv_request.Rows[dgRows].Cells[0].Value = row["_id"].ToString();
                    dgv_request.Rows[dgRows].Cells[1].Value = row["ARTICLE_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[2].Value = row["ARTICLE_NAME"].ToString();
                    dgv_request.Rows[dgRows].Cells[3].Value = row["SIZE"].ToString();
                    dgv_request.Rows[dgRows].Cells[4].Value = row["COLOR"].ToString();
                    dgv_request.Rows[dgRows].Cells[5].Value = row["PRICE"];
                    dgv_request.Rows[dgRows].Cells[7].Value = row["QUANTITY"].ToString();
                    dgv_request.Rows[dgRows].Cells[9].Value = row["UNIT"];
                    dgv_request.Rows[dgRows].Cells[10].Value = row["SUBTOTAL"];
                }
                dgv_request.Columns[5].DefaultCellStyle.Format = "#,###";
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
            //        int n = dgv_request.Rows.Add();
            //        dgv_request.Rows[n].Cells[0].Value = row["_id"].ToString();
            //        dgv_request.Rows[n].Cells[1].Value = row["ARTICLE_ID"].ToString();
            //        dgv_request.Rows[n].Cells[2].Value = row["ARTICLE_NAME"].ToString();
            //        dgv_request.Rows[n].Cells[3].Value = row["SIZE"].ToString();
            //        dgv_request.Rows[n].Cells[4].Value = row["COLOR"].ToString();
            //        dgv_request.Rows[n].Cells[5].Value = row["PRICE"];
            //        dgv_request.Rows[n].Cells[7].Value = row["QUANTITY"].ToString();
            //        dgv_request.Rows[n].Cells[9].Value = row["UNIT"];
            //        dgv_request.Rows[n].Cells[10].Value = row["SUBTOTAL"];
            //    }
            //    dgv_request.Columns[5].DefaultCellStyle.Format = "#,###";

            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        //==================================================================================

        //====================AMBIL TOTAL QTY FROM MUTASI ORDER========================================
        public void qty()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd_roQty = "SELECT SUM(returnorder_line.QUANTITY) as total FROM returnorder_line where RETURN_ORDER_ID = '" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_roQty, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        qty2 = ckon.sqlDataRd["total"].ToString();
                        l_qty.Text = qty2.ToString();
                    }
                }
                else
                {
                    l_qty.Text = "0";
                }

                String cmd_roTotal = "SELECT SUM(returnorder_line.SUBTOTAL) as total_amount FROM returnorder_line where RETURN_ORDER_ID = '" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_roTotal, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        total_amount = Convert.ToInt32(ckon.sqlDataRd["total_amount"].ToString());
                        l_amount.Text = string.Format("{0:#,###}" + ",00", total_amount);
                    }
                }
                else
                {
                    l_amount.Text = "0,00";
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
            //        try
            //        {
            //            qty2 = ckon.myReader.GetString("total");
            //            l_qty.Text = qty2.ToString();
            //        }
            //        catch
            //        {
            //            l_qty.Text = "0";
            //        }

            //    }

            //}
            //else
            //{ }
            //ckon.con.Close();
            //=======================MENGHITUNG TOTAL AMOUNT DARI TRANSASKI LINE===================
            //String query = "SELECT SUM(returnorder_line.SUBTOTAL) as total_amount FROM returnorder_line where RETURN_ORDER_ID = '" + l_transaksi.Text + "'";
            //ckon.cmd = new MySqlCommand(query, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    try
            //    {
            //        total_amount = ckon.myReader.GetInt32("total_amount");
            //        l_amount.Text = string.Format("{0:#,###}" + ",00", total_amount);
            //    }
            //    catch
            //    {
            //        l_amount.Text = "0,00";
            //    }
            //}
            //ckon.con.Close();
        }
        //==============================================================================================
        //========================UPDATE RETURN ORDER HEADER ===============================================
        //public void update_header()
        //{
        //    String sql = "UPDATE returnorder SET TOTAL_QTY='" + l_qty.Text + "', STATUS='1' WHERE RETURN_ORDER_ID = '" + l_transaksi.Text + "'";
        //    CRUD input = new CRUD();
        //    input.ExecuteNonQuery(sql);
        //}

        //===============================================================================================
        //====================GET DATA FROM ID =======================================================
        public void get_data()
        {
            CRUD sql = new CRUD();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM returnorder WHERE RETURN_ORDER_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        no_sj.Text = ckon.sqlDataRd["NO_SJ"].ToString();
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
            //        no_sj.Text = ckon.myReader.GetString("NO_SJ");
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }          
        }
        //==================================KLIK DATA HOLD MASUK KE DAFTAR RETURN======================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {

            //fungsi fokus ke scan barcode
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    l_transaksi.Text = id_list;
                    get_data();
                    holding();
                    retreive();
                    qty();
                }
            }
            catch (Exception)
            {

            }
        }
        //==============================================================================================

        //================================BUTTON HOLD=================================================
        private void b_hold_Click(object sender, EventArgs e)
        {
            //fungsi fokus ke scan barcode
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            if (l_qty.Text == "0")
            {
                //PopupNotifier pop = new PopupNotifier();
                //pop.TitleText = "Warning";
                //pop.ContentText = "Pick Item First";
                //pop.Popup();
                MessageBox.Show("Pick Item First");
            }
            else
            {
                //RunningNumber running = new RunningNumber();
                //running.get_data_before("4", "RT");

                new_invoice();
                set_running_number();
                holding();
                dgv_request.Rows.Clear();
                l_qty.Text = "0";
            }

        }

        //===========================================================================================

        //==============================ACTION MINUS, PLUS OR DELETE DATA============================
        private void dgv_request_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CRUD sql = new CRUD();
            //fungsi fokus ke scan barcode
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            if (dgv_request.Columns[e.ColumnIndex].Name == "Delete")
            {
                String DEL = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String cmd_delete = "DELETE FROM returnorder_line WHERE RETURN_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + DEL + "'";
                sql.ExecuteNonQuery(cmd_delete);
                retreive();
                qty();

            }
            if (dgv_request.Columns[e.ColumnIndex].Name == "plus")
            {
                //ckon.con.Close();
                String _id2 = dgv_request.SelectedRows[0].Cells[0].Value.ToString();
                String ID = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String quantity = dgv_request.SelectedRows[0].Cells[7].Value.ToString();
                String subtotal = dgv_request.SelectedRows[0].Cells[10].Value.ToString();
                String price = dgv_request.SelectedRows[0].Cells[5].Value.ToString();
                int new_price = Int32.Parse(price); int new_subtotal = Int32.Parse(subtotal);
                //mencari good qty dari tabel inventory
                try
                {
                    ckon.sqlCon().Open();
                    String cmd_inv = "SELECT * FROM inventory WHERE _id = '" + _id2 + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_inv, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            GOOD_QTY_PLUS = Convert.ToInt32(ckon.sqlDataRd["GOOD_QTY"].ToString());
                        }
                    }

                    int new_qty = Int32.Parse(quantity);
                    new_qty = new_qty + 1;
                    new_subtotal = new_subtotal + new_price;
                    //BANDINGKAN, JIKA LEBIH BESAR DARI GOOD_QTY JGN DI EKSEKUSI
                    if (new_qty > GOOD_QTY_PLUS)
                    {
                        MessageBox.Show("Quantity Exceeded");
                    }
                    else
                    {
                        String update = "UPDATE returnorder_line SET QUANTITY='" + new_qty + "', SUBTOTAL='" + new_subtotal + "' WHERE RETURN_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
                        sql.ExecuteNonQuery(update);
                        retreive();
                        qty();
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

                //ckon.cmd = new MySqlCommand(query, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while(ckon.myReader.Read())
                //{
                //    GOOD_QTY_PLUS = ckon.myReader.GetInt32("GOOD_QTY");
                //}
                //ckon.con.Close();
                //int new_qty = Int32.Parse(quantity);
                //new_qty = new_qty + 1;
                //new_subtotal = new_subtotal + new_price;
                ////BANDINGKAN, JIKA LEBIH BESAR DARI GOOD_QTY JGN DI EKSEKUSI
                //if (new_qty > GOOD_QTY_PLUS)
                //{
                //    MessageBox.Show("Quantity Exceeded");
                //}
                //else
                //{
                //    String update = "UPDATE returnorder_line SET QUANTITY='" + new_qty + "', SUBTOTAL='"+ new_subtotal +"' WHERE RETURN_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";

                //    CRUD input = new CRUD();
                //    input.ExecuteNonQuery(update);
                //    retreive();
                //    qty();
                //}

            }
            if (dgv_request.Columns[e.ColumnIndex].Name == "minus")
            {
                String ID = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String quantity = dgv_request.SelectedRows[0].Cells[7].Value.ToString();
                String subtotal = dgv_request.SelectedRows[0].Cells[10].Value.ToString();
                String price = dgv_request.SelectedRows[0].Cells[5].Value.ToString();
                int new_price = Int32.Parse(price); int new_subtotal = Int32.Parse(subtotal);
                int new_qty = Int32.Parse(quantity);
                new_qty = new_qty - 1;
                new_subtotal = new_subtotal - new_price;
                if (new_qty <= 0)
                {
                    MessageBox.Show("Minimum QTY 1");
                }
                else
                {
                    String update = "UPDATE returnorder_line SET QUANTITY='" + new_qty + "', SUBTOTAL='" + new_subtotal + "' WHERE RETURN_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
                    sql.ExecuteNonQuery(update);
                    retreive();
                    qty();
                }

            }
            if (dgv_request.Columns[e.ColumnIndex].Name == "quantity")
            {
                String ID = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String name = dgv_request.SelectedRows[0].Cells[2].Value.ToString();
                String size = dgv_request.SelectedRows[0].Cells[3].Value.ToString();
                String color = dgv_request.SelectedRows[0].Cells[4].Value.ToString();
                String price = dgv_request.SelectedRows[0].Cells[5].Value.ToString();
                String quantity = dgv_request.SelectedRows[0].Cells[7].Value.ToString();
                String from = "Ret_Order";
                w_editQty edit_qty = new w_editQty();
                edit_qty.detail(l_transaksi.Text, ID, name, size, color, price, quantity);
                edit_qty.menu_asal(from);
                edit_qty.cek_qty();
                edit_qty.ShowDialog();
            }
        }
        //===========================================================================================

        //===============================BUTTON CLEAR===========================================
        private void b_clear_Click(object sender, EventArgs e)
        {
            //fungsi fokus ke scan barcode
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            if (l_qty.Text == "0")
            {
                //PopupNotifier pop = new PopupNotifier();
                //pop.TitleText = "Warning";
                //pop.ContentText = "No Item On List";
                //pop.Popup();
                MessageBox.Show("No Item On List");
            }
            else
            {
                String cmd_delete = "DELETE FROM returnorder_line WHERE RETURN_ORDER_ID ='" + l_transaksi.Text + "'";
                CRUD delete = new CRUD();
                delete.ExecuteNonQuery(cmd_delete);
                dgv_request.Rows.Clear(); l_qty.Text = "0";
            }

        }
        //====================================================================================

        //============================BUTTON CONFIRM==========================================
        private void b_confirm_Click(object sender, EventArgs e)
        {

            int ab = no_sj.Text.Length;
            if (ab <= 10)
            {
                //fungsi fokus ke scan barcode
                this.ActiveControl = t_barcode;
                t_barcode.Focus();
                if (l_qty.Text == "0")
                {
                    MessageBox.Show("No Item On List");
                }
                else
                {
                    W_remark_RT remark = new W_remark_RT();
                    remark.update_header(l_qty.Text, l_transaksi.Text, total_amount, no_sj.Text);
                    remark.set_id(epy_id, epy_name);
                    remark.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Maximum chars (No SJ) : 10");
                no_sj.Focus();
            }
        }
        //====================================================================================

        public void reset()
        {
            l_qty.Text = "0";
            l_amount.Text = "0,00";
            dgv_request.Rows.Clear();
        }

        //=======================VIEW LIST RETURN ORDER=======================================
        private void b_list_RT_Click(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM returnorder WHERE  STATUS='1' ";
            UC_RT_list c = new UC_RT_list(f1);
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_RT_list.Instance))
            {
                f1.p_kanan.Controls.Add(UC_RT_list.Instance);
                UC_RT_list.Instance.Dock = DockStyle.Fill;
                UC_RT_list.Instance.holding(sql);
                UC_RT_list.Instance.Show();
            }
            else
                UC_RT_list.Instance.holding(sql);
            UC_RT_list.Instance.Show();
        }
        //====================================================================================

        //=====================SEARCH ARTICLE ===============================================
        private void b_search_Click(object sender, EventArgs e)
        {
            //fungsi fokus ke scan barcode
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            RT_SearchArticle rt_search = new RT_SearchArticle();
            rt_search.t_id = l_transaksi.Text;
            rt_search.no_sj = no_sj.Text;
            rt_search.cust_id_store2 = cust_id_store;
            rt_search.store_code2 = store_code;
            rt_search.save_auto_number(bulan_now, number_trans);
            rt_search.ShowDialog();
        }
        //=======FUNGSI SCAN BARCODE ARTICLE===================================
        private void t_barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (Properties.Settings.Default.OnnOrOff == "Offline")
                    {
                        //====HIT INVEN LOCAL==========================
                        API_Inventory InvLocal = new API_Inventory();
                        InvLocal.SetInvHitLocal();
                        //=============================================

                        cek_article();//fungsi memasukan data article ke dalam mutasi order line sesuai dengan scan barcode
                        save_trans_header();//menyimpan data ke return order header
                        retreive();//menampilkan ke datagridview
                        qty();//menghitung total quantity di return
                        t_barcode.Text = "";
                    }
                    else
                    {
                        cek_article();//fungsi memasukan data article ke dalam mutasi order line sesuai dengan scan barcode
                        save_trans_header();//menyimpan data ke return order header
                        retreive();//menampilkan ke datagridview
                        qty();//menghitung total quantity di return
                        t_barcode.Text = "";
                    }

                }
                else
                { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //============FUNGSI CEK TABEL ARTICLE BERDASARKAN ARTICLE ID YANG TELAH DI SCAN==
        public void cek_article()
        {
            CRUD sql = new CRUD();
            int total_amount_new;
            //ckon.con.Close();
            //String sql = "SELECT * FROM article WHERE ARTICLE_ID = '" + t_barcode.Text + "'";
            try
            {
                ckon.sqlCon().Open();
                String cmd = "Select article._id, article.ARTICLE_ID, article.UNIT, article.PRICE, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >=1 AND article.ARTICLE_ID = '" + t_barcode.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_inv = ckon.sqlDataRd["_id"].ToString();
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        unit = ckon.sqlDataRd["UNIT"].ToString();
                        price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        good_qty = ckon.sqlDataRd["GOOD_QTY"].ToString();
                    }

                    total_amount_new = price * Convert.ToInt32(good_qty);
                    String cmd_roLine = "SELECT * FROM returnorder_line WHERE RETURN_ORDER_ID = '" + l_transaksi.Text + "' AND ARTICLE_ID = '" + art_id + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_roLine, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        MessageBox.Show("This Article Has Been Entered");
                    }
                    else
                    {
                        String cmd_insert = "INSERT INTO returnorder_line (RETURN_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT, SUBTOTAL) VALUES ('" + l_transaksi.Text + "','" + art_id + "', '" + good_qty + "', '" + unit + "','" + total_amount_new + "')";
                        sql.ExecuteNonQuery(cmd_insert);
                    }
                }
                else
                {
                    MessageBox.Show("Article Not Found Or Quantity Empty");
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
            //int count = 0;
            //int status_qty = 0;
            //if (ckon.myReader.HasRows)
            //{
            //    count = count + 1;
            //    while (ckon.myReader.Read())
            //    {
            //        id_inv = ckon.myReader.GetString("_id");
            //        art_id = ckon.myReader.GetString("ARTICLE_ID");
            //        unit = ckon.myReader.GetString("UNIT");
            //        price = ckon.myReader.GetInt32("PRICE");
            //        good_qty = ckon.myReader.GetString("GOOD_QTY");
            //        //===CARI TOTAL QTY BERDASARKAN _id yang didapat ke tabel inventory==
            //        /*
            //        String sql2 = "SELECT * FROM inventory WHERE ARTICLE_ID = '" + id_inv + "'";
            //        ckon2.cmd2 = new MySqlCommand(sql2, ckon2.con2);
            //        ckon2.con2.Open();
            //        ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //        while (ckon2.myReader2.Read())
            //        {
            //            good_qty = ckon2.myReader2.GetString("GOOD_QTY");
            //        }
            //        ckon2.con2.Close();
            //        */
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Article Not Found Or Quantity Empty");
            //    ckon.con.Close();
            //}
            //ckon.con.Close();
            //===mengecek apakah article id tersebut dengan mutasi order yang ada sudah ada di mutasi order list atau belum
            //if (count == 1)
            //{
            //    total_amount_new = price * Convert.ToInt32(good_qty);
            //    String sql3 = "SELECT * FROM returnorder_line WHERE RETURN_ORDER_ID = '" + l_transaksi.Text + "' AND ARTICLE_ID = '" + art_id + "'";
            //    ckon.cmd = new MySqlCommand(sql3, ckon.con);
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    if (ckon.myReader.HasRows)
            //    {
            //        MessageBox.Show("This Article Has Been Entered");
            //    }
            //    else
            //    {
            //        String input = "INSERT INTO returnorder_line (RETURN_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT, SUBTOTAL) VALUES ('" + l_transaksi.Text + "','" + art_id + "', '" + good_qty + "', '" + unit + "','"+ total_amount_new +"')";
            //        CRUD masuk = new CRUD();
            //        masuk.ExecuteNonQuery(input);
            //    }
            //    ckon.con.Close();
            //}
        }
        //======================SIMPAN TRANSACTION HEADER============================================
        public void save_trans_header()
        {
            CRUD sql = new CRUD();
            DateTime mydate = DateTime.Now;
            DateTime myhour = DateTime.Now;
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM returnorder WHERE RETURN_ORDER_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (!ckon.sqlDataRd.HasRows)
                {
                    String cmd_insert = "INSERT INTO returnorder (RETURN_ORDER_ID,STORE_CODE,TOTAL_QTY,STATUS, DATE, TIME, CUST_ID_STORE, NO_SJ) VALUES ('" + l_transaksi.Text + "', '" + store_code + "' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_store + "','" + no_sj.Text + "') ";
                    sql.ExecuteNonQuery(cmd_insert);

                    String cmd_update = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='4'";
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
            //    String sql = "INSERT INTO returnorder (RETURN_ORDER_ID,STORE_CODE,TOTAL_QTY,STATUS, DATE, TIME, CUST_ID_STORE, NO_SJ) VALUES ('" + l_transaksi.Text + "', '" + store_code + "' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_store + "','"+ no_sj.Text +"') ";
            //    ckon.con.Open();
            //    ckon.cmd = new MySqlCommand(sql, ckon.con);
            //    ckon.cmd.ExecuteNonQuery();
            //    ckon.con.Close();

            //    String query = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='4'";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);
            //}
            //else
            //{

            //}
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
                String cmd = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '4'";
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
                        final_running_number = "RT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
                        l_transaksi.Text = final_running_number;

                    }
                    else
                    {
                        number_trans = 1;
                        bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
                                                //==MEMBUAT STRING FINAL RUNNING NUMBER
                        final_running_number = "RT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                        l_transaksi.Text = final_running_number;
                    }
                }
                else
                {
                    number_trans = 1;
                    bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
                    final_running_number = "RT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                    l_transaksi.Text = final_running_number;

                    String cmd_insert = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','4')";
                    sql.ExecuteNonQuery(cmd_insert);
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

            //    ckon.con.Open();
            //    ckon.cmd = new MySqlCommand(sql, ckon.con);
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    if (ckon.myReader.HasRows)
            //    {
            //        while (ckon.myReader.Read())
            //        {
            //            //tahun_trans = ckon.myReader.GetString("Year");
            //            bulan_trans = ckon.myReader.GetString("Month");
            //            number_trans = ckon.myReader.GetInt32("Number");
            //        }
            //        if (bulan_now == bulan_trans)
            //        {
            //            number_trans = number_trans + 1;
            //            if (number_trans < 10)
            //            { number_trans_string = "0000" + number_trans.ToString(); }
            //            else if (number_trans < 100)
            //            { number_trans_string = "000" + number_trans.ToString(); }
            //            else if (number_trans < 1000)
            //            { number_trans_string = "00" + number_trans.ToString(); }
            //            else if (number_trans < 10000)
            //            { number_trans_string = "0" + number_trans.ToString(); }
            //            else
            //            { number_trans_string = number_trans.ToString(); }
            //            //==MEMBUAT STRING FINAL RUNNING NUMBER
            //            final_running_number = "RT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
            //            l_transaksi.Text = final_running_number;

            //        }
            //        else
            //        {
            //            number_trans = 1;
            //            bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
            //            //==MEMBUAT STRING FINAL RUNNING NUMBER
            //            final_running_number = "RT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //            l_transaksi.Text = final_running_number;
            //        }


            //    }
            //    else
            //    {
            //        number_trans = 1;
            //        bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
            //        final_running_number = "RT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //        l_transaksi.Text = final_running_number;

            //        String query = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','4')";
            //        CRUD ubah = new CRUD();
            //        ubah.ExecuteNonQuery(query);

            //        //MessageBox.Show(final_running_number);
            //    }
            //    ckon.con.Close();
            //}

        }
    }
}
