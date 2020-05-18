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
using System.Data.SqlClient;

namespace try_bi
{
    public partial class UC_Req_order : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String id_trans, article_id, id_list, qty2, store_code, cust_id_store, war_id,  epy_id, epy_name, bulan2,tipe2;
        int noo_inv_new,no_trans2, total_amount;
        //Variable untuk running number baru
        String bulan_now, tahun_now, bulan_trans, number_trans_string, final_running_number;
        int number_trans;
        //======================================================
        private static UC_Req_order _instance;
        public static UC_Req_order Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Req_order(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_Req_order(Form1 form1)
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
        //========================SET NAME EMPLOYEE ID AND EMPLOYEE NAME===================
        public void set_name(String id, String nama)
        {
            epy_id = id;
            epy_name = nama;
        }
        //=================================GENERATOR NUMBER=================================
        public void new_invoice()
        {
            CRUD sql = new CRUD();

            dgv_request.Rows.Clear();
            l_qty.Text = "0";
            no_sj.Text = "";
            l_amount.Text = "0,00";

            //ckon.con.Close();
            //String sql2 = "SELECT * FROM store";
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
                        cust_id_store = ckon.sqlDataRd["CUST_ID_STORE"].ToString();
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

            //============================================================================================            
        }
        //==================================================================================

        //======================== KLIK HOLD MASUK KERANJANG BELANJA=======================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
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

        //==================================================================================
        //====================GET DATA FROM ID =======================================================
        public void get_data()
        {
            CRUD sql = new CRUD();

            //String sql = "SELECT * FROM requestorder WHERE REQUEST_ORDER_ID ='" + l_transaksi.Text + "'";
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

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM requestorder WHERE REQUEST_ORDER_ID ='" + l_transaksi.Text + "'";
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
        }
        //==============BUTTON HOLD==================================
        private void b_hold_Click(object sender, EventArgs e)
        {
            if (l_qty.Text == "0")
            {
                PopupNotifier pop = new PopupNotifier();
                pop.TitleText = "Warning";
                pop.ContentText = "Pick Item First";
                pop.Popup();
            }
            else
            {
                //RunningNumber running = new RunningNumber();
                //running.get_data_before("2", "RO");
                new_invoice();
                holding();
                set_running_number();
                dgv_request.Rows.Clear();
                l_qty.Text = "0";
                l_amount.Text = "0,00";
                no_sj.Text = "";
            }
        }
        //==================================================================================
        //==================KLIK DATA HAPUS, PLUS, OR MINUS DATA===========================
        private void dgv_request_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CRUD sql = new CRUD();

            if (dgv_request.Columns[e.ColumnIndex].Name == "Delete")
            {
                String DEL = dgv_request.SelectedRows[0].Cells[0].Value.ToString();
                String cmd_del = "DELETE FROM requestorder_line WHERE REQUEST_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + DEL + "'";
                sql.ExecuteNonQuery(cmd_del);

                retreive();
                qty();
                
            }
            if (dgv_request.Columns[e.ColumnIndex].Name == "plus")
            {
                String ID = dgv_request.SelectedRows[0].Cells[0].Value.ToString();
                String quantity = dgv_request.SelectedRows[0].Cells[6].Value.ToString();
                String subtotal = dgv_request.SelectedRows[0].Cells[9].Value.ToString();
                String price = dgv_request.SelectedRows[0].Cells[4].Value.ToString();
                int new_price = Int32.Parse(price); int new_subtotal = Int32.Parse(subtotal);
                int new_qty = Int32.Parse(quantity);
                new_qty = new_qty + 1;
                new_subtotal = new_subtotal + new_price;

                String cmd_update = "UPDATE requestorder_line SET QUANTITY='" + new_qty + "', SUBTOTAL='" + new_subtotal + "' WHERE REQUEST_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
                sql.ExecuteNonQuery(cmd_update);

                retreive();
                qty();
            }
            if (dgv_request.Columns[e.ColumnIndex].Name == "minus")
            {
                String ID = dgv_request.SelectedRows[0].Cells[0].Value.ToString();
                String quantity = dgv_request.SelectedRows[0].Cells[6].Value.ToString();
                String subtotal = dgv_request.SelectedRows[0].Cells[9].Value.ToString();
                String price = dgv_request.SelectedRows[0].Cells[4].Value.ToString();
                int new_price = Int32.Parse(price); int new_subtotal = Int32.Parse(subtotal);
                int new_qty = Int32.Parse(quantity);
                new_subtotal = new_subtotal - new_price;
                new_qty = new_qty - 1;
                if(new_qty <= 0)
                {
                    MessageBox.Show("Minimum QTY 1");
                }
                else
                {
                    String cmd_update = "UPDATE requestorder_line SET QUANTITY='" + new_qty + "', SUBTOTAL='" + new_subtotal + "' WHERE REQUEST_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
                    sql.ExecuteNonQuery(cmd_update);

                    retreive();
                    qty();
                }
                
            }
            if (dgv_request.Columns[e.ColumnIndex].Name == "quantity")
            {
                
                String ID = dgv_request.SelectedRows[0].Cells[0].Value.ToString();
                String name = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String size = dgv_request.SelectedRows[0].Cells[2].Value.ToString();
                String color = dgv_request.SelectedRows[0].Cells[3].Value.ToString();
                String price = dgv_request.SelectedRows[0].Cells[4].Value.ToString();
                String quantity = dgv_request.SelectedRows[0].Cells[6].Value.ToString();
                String from = "Req_order";
                w_editQty edit_qty = new w_editQty();
                edit_qty.detail(l_transaksi.Text ,ID, name, size, color, price, quantity);
                edit_qty.menu_asal(from);
                edit_qty.ShowDialog();
            }
        }
        //==================================================================================

        //==================edit data langsung di datagridview========================
        private void dgv_request_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //String ID = dgv_request.Rows[e.RowIndex].Cells["Column7"].Value.ToString();
            //if(dgv_request.Columns[e.ColumnIndex].Name == "Column5")
            //{
            //    String new_qty = dgv_request.Rows[e.RowIndex].Cells["Column5"].Value.ToString();
            //    String update = "UPDATE requestorder_line SET QUANTITY='" + new_qty + "' WHERE REQUEST_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
            //    CRUD input = new CRUD();
            //    input.ExecuteNonQuery(update);
            //    retreive();
            //    qty();
            //}
        }
        //==================================================================================
        //============BUTTON VIEW LIST RO ==================================================
        private void b_list_ro_Click(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM requestorder WHERE  STATUS='1' ";
            UC_RO_list c = new UC_RO_list(f1);
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_RO_list.Instance))
            {
                f1.p_kanan.Controls.Add(UC_RO_list.Instance);
                UC_RO_list.Instance.Dock = DockStyle.Fill;
                UC_RO_list.Instance.holding(sql);
                UC_RO_list.Instance.Show();
            }
            else
            {
                UC_RO_list.Instance.holding(sql);
            }
            UC_RO_list.Instance.Show();
        }
        //==================================================================================

        //=====================ITEM REQUEST ORDER ==========================================
        public void retreive(string sort = "", string filter = "", string search = "")
        {
            CRUD sql = new CRUD();

            dgv_request.Rows.Clear();            

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);
            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_request.Rows.Add();
            //        dgv_request.Rows[n].Cells[0].Value = row["ARTICLE_ID"].ToString();
            //        dgv_request.Rows[n].Cells[1].Value = row["ARTICLE_NAME"].ToString();
            //        dgv_request.Rows[n].Cells[2].Value = row["SIZE"].ToString();
            //        dgv_request.Rows[n].Cells[3].Value = row["COLOR"].ToString();
            //        dgv_request.Rows[n].Cells[4].Value = row["PRICE"];
            //        dgv_request.Rows[n].Cells[6].Value = row["QUANTITY"].ToString();
            //        dgv_request.Rows[n].Cells[8].Value = row["UNIT"];
            //        dgv_request.Rows[n].Cells[9].Value = row["SUBTOTAL"];
            //    }
            //    dgv_request.Columns[4].DefaultCellStyle.Format = "#,###";

            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT  requestorder_line.ARTICLE_ID ,requestorder_line.QUANTITY, requestorder_line.UNIT,requestorder_line.SUBTOTAL, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE FROM requestorder_line, article  WHERE article.ARTICLE_ID = requestorder_line. ARTICLE_ID AND requestorder_line.REQUEST_ORDER_ID='" + l_transaksi.Text + "' ";
                if (filter != "")
                {
                    filter = filter.Replace("ARTICLE_ID", "requestorder_line.ARTICLE_ID");
                    cmd += " AND " + filter;
                }
                if (sort != "")
                {
                    cmd += " ORDER BY " + sort;
                }
                else
                {
                    cmd += " ORDER BY requestorder_line._id DESC";
                }
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());
                
                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_request.Rows.Add();
                    dgv_request.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_request.Rows[dgRows].Cells[2].Value = row["SIZE"].ToString();
                    dgv_request.Rows[dgRows].Cells[3].Value = row["COLOR"].ToString();
                    dgv_request.Rows[dgRows].Cells[4].Value = row["PRICE"];
                    dgv_request.Rows[dgRows].Cells[6].Value = row["QUANTITY"].ToString();
                    dgv_request.Rows[dgRows].Cells[8].Value = row["UNIT"];
                    dgv_request.Rows[dgRows].Cells[9].Value = row["SUBTOTAL"];
                }
                dgv_request.Columns[4].DefaultCellStyle.Format = "#,###";
            }
            catch (Exception e)
            {
                MessageBox.Show("No connection to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ckon.dt.Clear();
                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
        //==================================================================================

        //======================LIST HOLD TRANSACTION============================================
        public void holding()
        {
            CRUD sql = new CRUD();            
            List<string> numbersList = new List<string>();            

            dgv_hold.Rows.Clear();
            //koneksi2 ckon2 = new koneksi2();
            //ckon.con.Close();

            ////date = mydate.ToString("yyyy-MM-dd");
            //String sql = "SELECT * FROM requestorder WHERE  STATUS='0' ";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //List<string> numbersList = new List<string>();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_trans = ckon.myReader.GetString("REQUEST_ORDER_ID");
            //        String sql2 = "SELECT article.ARTICLE_NAME FROM requestorder_line, article  WHERE article.ARTICLE_ID = requestorder_line.ARTICLE_ID AND requestorder_line.REQUEST_ORDER_ID='" + id_trans + "'";
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

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT article.ARTICLE_NAME, requestorder.REQUEST_ORDER_ID FROM requestorder INNER JOIN requestorder_line"
                            + "ON requestorder_line.REQUEST_ORDER_ID = requestorder.REQUEST_ORDER_ID INNER JOIN article"
                            + "ON article.ARTICLE_ID = requestorder_line.ARTICLE_ID"
                            + "WHERE requestorder.STATUS = '0'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["REQUEST_ORDER_ID"].ToString();                        
                        numbersList.Add(ckon.sqlDataRd["ARTICLE_NAME"].ToString());

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
        }
        //=============================================================================================

        //====================AMBIL TOTAL QTY FROM REQUEST ORDER========================================
        public void qty()
        {
            CRUD sql = new CRUD();

            //========MENGHITUNG TOTAL QUANTITY DARI REWUEST ORDER LINE========================
            //String sql = "SELECT SUM(requestorder_line.QUANTITY) as total FROM requestorder_line where REQUEST_ORDER_ID = '" + l_transaksi.Text + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    if (ckon.myReader.HasRows)
            //    {
            //        while (ckon.myReader.Read())
            //        {
            //            try
            //            {
            //                qty2 = ckon.myReader.GetString("total");
            //                l_qty.Text = qty2.ToString();
            //            }
            //            catch
            //            {
            //                l_qty.Text = "0";
            //            }

            //        }

            //    }
            //    else
            //    {  }
            //    ckon.con.Close();            

            //=======================MENGHITUNG TOTAL AMOUNT DARI TRANSASKI LINE===================
            //String query = "SELECT SUM(requestorder_line.SUBTOTAL) as total_amount FROM requestorder_line where REQUEST_ORDER_ID = '" + l_transaksi.Text + "'";
            //ckon.cmd = new MySqlCommand(query, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while(ckon.myReader.Read())
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

            try
            {
                ckon.sqlCon().Open();
                String cmd_reqorQty = "SELECT SUM(requestorder_line.QUANTITY) as total FROM requestorder_line where REQUEST_ORDER_ID = '" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_reqorQty, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        qty2 = ckon.sqlDataRd["total"].ToString();
                        l_qty.Text = qty2;
                    }
                }
                else
                {
                    l_qty.Text = "0";
                }

                String cmd_reqorTotal = "SELECT SUM(requestorder_line.SUBTOTAL) as total_amount FROM requestorder_line where REQUEST_ORDER_ID = '" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_reqorTotal, ckon.sqlCon());

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
        }
        //==============================================================================================

        //========================UPDATE REQ ORDER HEADER ===============================================

        public void update_header()
        {
            CRUD sql = new CRUD();

            String cmd_update = "UPDATE requestorder SET TOTAL_QTY='" + l_qty.Text + "', STATUS='1', CUST_ID_STORE='"+ cust_id_store + "', EMPLOYEE_ID='"+epy_id +"', EMPLOYEE_NAME='"+ epy_name +"' WHERE REQUEST_ORDER_ID = '"+ l_transaksi.Text +"'";            
            sql.ExecuteNonQuery(cmd_update);           
        }
       //===============================================================================================

        //=============================BUTTON SEARCH ===================================================
        private void b_search_Click(object sender, EventArgs e)
        {
            RO_SearchArticle ro_search = new RO_SearchArticle();
            ro_search.cust_id_store2 = cust_id_store;
            ro_search.no_sj = no_sj.Text;
            ro_search.war_id2 = war_id;
            ro_search.t_id = l_transaksi.Text;
            ro_search.tgl_req = tanggal_req.Text;
            ro_search.store_code2 = store_code;
            ro_search.save_auto_number(bulan_now, number_trans);
            ro_search.ShowDialog();
        }
        //==============================================================================================

        //=============================BUTTON CONFIRM ==================================================
        private void b_confirm_Click(object sender, EventArgs e)
        {

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
                w_desc_ReqO ro = new w_desc_ReqO();
                ro.get_data(l_qty.Text, cust_id_store, epy_id, epy_name, l_transaksi.Text,total_amount,no_sj.Text);
                ro.ShowDialog();
                //update_header();
                //MessageBox.Show("data successfully added");
                
            }
            
        }
        //===reset dari tampilan w_desc_ReqO
        public void reset()
        {
            dgv_request.Rows.Clear();
            l_qty.Text = "0";
            holding();
            new_invoice();
            l_amount.Text = "0,00";
            no_sj.Text = "";
        }

        //===============================================================================================

            //=====================================BUTTON CLEAR====================================
        private void b_clear_Click(object sender, EventArgs e)
        {
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
                CRUD sql = new CRUD();

                String cmd_delete = "DELETE FROM requestorder_line WHERE REQUEST_ORDER_ID ='" + l_transaksi.Text + "'";
                sql.ExecuteNonQuery(cmd_delete);

                dgv_request.Rows.Clear(); 
                l_qty.Text = "0";
            }
            
        }
        //============================================================================================
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

            //String sql = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '2'";
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
            //        final_running_number = "RO/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
            //        l_transaksi.Text = final_running_number;

            //    }
            //    else
            //    {
            //        number_trans = 1;
            //        bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
            //        //==MEMBUAT STRING FINAL RUNNING NUMBER
            //        final_running_number = "RO/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //        l_transaksi.Text = final_running_number;
            //    }


            //}
            //else
            //{
            //    number_trans = 1;
            //    bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
            //    final_running_number = "RO/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //    l_transaksi.Text = final_running_number;

            //    String query = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','2')";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);

            //    //MessageBox.Show(final_running_number);
            //}
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '2'";
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
                        { 
                            number_trans_string = "0000" + number_trans.ToString(); 
                        }
                        else if (number_trans < 100)
                        { 
                            number_trans_string = "000" + number_trans.ToString(); 
                        }
                        else if (number_trans < 1000)
                        { 
                            number_trans_string = "00" + number_trans.ToString(); 
                        }
                        else if (number_trans < 10000)
                        { 
                            number_trans_string = "0" + number_trans.ToString(); 
                        }
                        else
                        { 
                            number_trans_string = number_trans.ToString(); 
                        }

                        //==MEMBUAT STRING FINAL RUNNING NUMBER
                        final_running_number = "RO/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
                        l_transaksi.Text = final_running_number;
                    }
                    else
                    {
                        number_trans = 1;
                        bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
                                                //==MEMBUAT STRING FINAL RUNNING NUMBER
                        final_running_number = "RO/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                        l_transaksi.Text = final_running_number;
                    }
                }
                else
                {
                    number_trans = 1;
                    bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
                    final_running_number = "RO/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                    l_transaksi.Text = final_running_number;

                    String cmd_insert = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','2')";
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
        }
    }
}
