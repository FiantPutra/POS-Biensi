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
    public partial class UC_Mut_order : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        koneksi3 ckon3 = new koneksi3();
        String id_trans, article_id, qty2, id_list, store_code, cust_id_store, epy_id, epy_name, combo_store2, city, id_inv, unit, size, price, color,art_name, art_id, good_qty, bulan2, tipe2, art_id_mut, inv_id, region;
        //String id_store;
        int noo_inv_new, GOOD_QTY_PLUS, no_trans2, total_amount, new_price, count_eror, qty_mut_line, inv_good_qty;
        //Variable untuk running number baru
        String bulan_now, tahun_now, bulan_trans, number_trans_string, final_running_number;
        int number_trans;
        //======================================================
        private static UC_Mut_order _instance;

        private void dev_request_Sorting(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        private void dgv_request_FilterStringChanged(object sender, EventArgs e)
        {
            //"(Convert([ARTICLE_ID],System.String) IN ('152041822', 'G01061821'))"
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        public static UC_Mut_order Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Mut_order(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_Mut_order(Form1 form1)
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
        //==========================FILTERCONVERTER===============================================
       
        //==============GET EMPLOYEE ID AND EMPLOYEE NAME FROM FORM1======================
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
            //========================================================================
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
                        cust_id_store = ckon.sqlDataRd["CUST_ID_STORE"].ToString();
                        city = ckon.sqlDataRd["CITY"].ToString();
                        region = ckon.sqlDataRd["Regional"].ToString();
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
            //            cust_id_store = ckon.myReader.GetString("CUST_ID_STORE");
            //            city = ckon.myReader.GetString("CITY");
            //        }
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ MessageBox.Show("Failed when get data from store data"); }
            //========================================================================
            //string sql = "SELECT SUBSTRING(MUTASI_ORDER_ID, 8) AS inv  FROM mutasiorder ORDER BY MUTASI_ORDER_ID DESC LIMIT 1";
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
            //            //l_transaksi.Text = "MO-" + noo_inv_new.ToString();
            //        }
            //        if (noo_inv_new < 10)
            //        { l_transaksi.Text = store_code + "/MO-000" + noo_inv_new.ToString(); }
            //        else if (noo_inv_new < 100)
            //        { l_transaksi.Text = store_code + "/MO-00" + noo_inv_new.ToString(); }
            //        else if (noo_inv_new < 1000)
            //        { l_transaksi.Text = store_code + "/MO-0" + noo_inv_new.ToString(); }
            //        else if (noo_inv_new < 10000)
            //        { l_transaksi.Text = store_code + "/MO-" + noo_inv_new.ToString(); }
            //        else
            //        { }
            //    }
            //    else
            //    { l_transaksi.Text = store_code + "/MO-0001"; }

            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        //==================================================================================
        //==============================ISI COMBO MUTASI================================
        public void isi_combo_mutasi(String query)
        {
            CRUD sql = new CRUD();

            combo_mutasiTo.Items.Clear();
            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        String code = ckon.sqlDataRd["CODE"].ToString();
                        String name = ckon.sqlDataRd["NAME"].ToString();
                        combo_mutasiTo.Items.Add(code + "--" + name);
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

            //String sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        String code = ckon.myReader.GetString("CODE");
            //        String name = ckon.myReader.GetString("NAME");
            //        combo_mutasiTo.Items.Add(code+"--"+name);
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        //==========================================================================

        //====================ACTION COMBOBOX COVERAGE ==============================
        private void combo_coverage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(combo_coverage.Text == "City")
            {
                this.ActiveControl = t_barcode;
                t_barcode.Focus();
                combo_mutasiTo.Text = "";
                //String sql = "SELECT * FROM store_relasi WHERE CITY='"+ city +"'";
                String sql = "SELECT * FROM store WHERE CITY='" + city + "' AND code != '" + store_code + "'";
                isi_combo_mutasi(sql);
            }
            else if(combo_coverage.Text == "Regional")
            {
                this.ActiveControl = t_barcode;
                t_barcode.Focus();
                combo_mutasiTo.Text = "";
                String sql = "SELECT * FROM store WHERE REGIONAL = '"+ region +"' AND code != '"+ store_code +"'";
                isi_combo_mutasi(sql);
                //combo_mutasiTo.Text = "";
                //combo_mutasiTo.Items.Clear();
            } else
            {
                this.ActiveControl = t_barcode;
                t_barcode.Focus();
                combo_mutasiTo.Text = "";
                String sql = "SELECT CODE, NAME FROM store UNION SELECT CODE, NAME FROM store_relasi WHERE code != '" + store_code + "'";
                isi_combo_mutasi(sql);
            }
            
        }

        //======================================================================================
        //=================================GET ID store FROM COMBO BOX =================================
        public void get_data_combo_store()
        {
            String combo_store;
            try
            {
                combo_store = combo_mutasiTo.Text;
                combo_store2 = combo_store.Substring(0, 3);
            }
            catch
            {
                combo_store2 = "";
            }
        }
        //============================================================================================

        //======================LIST HOLD mutasi============================================
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
                String cmd = "SELECT article.ARTICLE_NAME, mutasiorder_line.ARTICLE_ID FROM mutasiorder INNER JOIN mutasiorder_line "
                            + "ON mutasiorder_line.MUTASI_ORDER_ID = mutasiorder.MUTASI_ORDER_ID INNER JOIN article "
                            + "ON article.ARTICLE_ID = mutasiorder_line.ARTICLE_ID "
                            + "WHERE mutasiorder.STATUS = '0'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["MUTASI_ORDER_ID"].ToString();
                        article_id = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //List<string> numbersList = new List<string>();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_trans = ckon.myReader.GetString("MUTASI_ORDER_ID");
            //        String sql2 = "SELECT article.ARTICLE_NAME FROM mutasiorder_line, article  WHERE article.ARTICLE_ID = mutasiorder_line.ARTICLE_ID AND mutasiorder_line.MUTASI_ORDER_ID='" + id_trans + "'";
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

        //=====================ITEM mutasi ORDER ==========================================
        public void retreive(string sort="", string filter="", string search="")
        {
            CRUD sql = new CRUD();

            dgv_request.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT  mutasiorder_line.ARTICLE_ID ,mutasiorder_line.QUANTITY, mutasiorder_line.UNIT,mutasiorder_line.SUBTOTAL, article._id,article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE FROM mutasiorder_line, article  WHERE article.ARTICLE_ID = mutasiorder_line.ARTICLE_ID AND mutasiorder_line.MUTASI_ORDER_ID='" + l_transaksi.Text + "' ";
                if (filter != "")
                {
                    filter = filter.Replace("ARTICLE_ID", "mutasiorder_line.ARTICLE_ID");
                    cmd += " AND " + filter;
                }
                if (sort != "")
                {
                    cmd += " ORDER BY " + sort;
                }
                else
                {
                    cmd += " ORDER BY mutasiorder_line._id DESC";
                }
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_request.Rows.Add();
                    dgv_request.Rows[dgRows].Cells[0].Value = row["_id"];
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

            //Console.Write(sql);
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);
            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_request.Rows.Add();
            //        dgv_request.Rows[n].Cells[0].Value = row["_id"];
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

            try
            {
                ckon.sqlCon().Open();
                String cmd_moQty = "SELECT SUM(mutasiorder_line.QUANTITY) as total FROM mutasiorder_line where MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_moQty, ckon.sqlCon());

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
            try
            {
                ckon.sqlCon().Open();
                String cmd_moSubTotal = "SELECT SUM(mutasiorder_line.SUBTOTAL) as total_amount FROM mutasiorder_line where MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_moSubTotal, ckon.sqlCon());

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
        //====================GET DATA FROM ID =======================================================
        public void get_data()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM mutasiorder WHERE MUTASI_ORDER_ID ='" + l_transaksi.Text + "'";
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
        //==================================KLIK DATA HOLD MASUK KE DAFTAR MUTASI======================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
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
        //=============================================================================================


        //================================BUTTON HOLD=================================================
        private void b_hold_Click(object sender, EventArgs e)
        {
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
                //running.get_data_before("3", "MT");
                new_invoice();
                set_running_number();
                holding();
                dgv_request.Rows.Clear();
                l_qty.Text = "0";
                l_amount.Text = "0,00";
                no_sj.Text = "";
            }

        }


        //===========================================================================================

        //==============================ACTION MINUS, PLUS OR DELETE DATA============================
        private void dgv_request_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CRUD sql = new CRUD();

            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            if (dgv_request.Columns[e.ColumnIndex].Name == "Delete")
            {
                String DEL = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String cmd_delete = "DELETE FROM mutasiorder_line WHERE MUTASI_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + DEL + "'";
                CRUD delete = new CRUD();
                delete.ExecuteNonQuery(cmd_delete);
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
                    String cmd = "SELECT * FROM inventory WHERE _id = '" + _id2 + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

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
                        String cmd_update = "UPDATE mutasiorder_line SET QUANTITY='" + new_qty + "',SUBTOTAL='" + new_subtotal + "' WHERE MUTASI_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
                        CRUD update = new CRUD();
                        update.ExecuteNonQuery(cmd_update);
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
                //while (ckon.myReader.Read())
                //{
                //    GOOD_QTY_PLUS = ckon.myReader.GetInt32("GOOD_QTY");
                //}
                //ckon.con.Close();                
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
                    String cmd_update = "UPDATE mutasiorder_line SET QUANTITY='" + new_qty + "', SUBTOTAL='" + new_subtotal + "' WHERE MUTASI_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
                    CRUD update = new CRUD();
                    update.ExecuteNonQuery(cmd_update);
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
                String from = "Mut_order";
                w_editQty edit_qty = new w_editQty();
                edit_qty.detail(l_transaksi.Text, ID, name, size, color, price, quantity);
                edit_qty.menu_asal(from);
                edit_qty.cek_qty();
                edit_qty.ShowDialog();
            }
        }
        //===============FUNGSI SCAN BARCODE, JIKA ADA TAMPILKAN QTY SESUAI INVENTORY===========================
        private void t_barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if(e.KeyChar == (char)Keys.Enter)
                {
                    if(Properties.Settings.Default.OnnOrOff == "Offline")
                    {
                        //====HIT INVEN LOCAL==========================
                        API_Inventory InvLocal = new API_Inventory();
                        InvLocal.SetInvHitLocal();
                        //=============================================
                        cek_article();//fungsi memasukan data article ke dalam mutasi order line sesuai dengan scan barcode
                        save_trans_header();
                        retreive();
                        qty();
                        t_barcode.Text = "";
                    }
                    else
                    {
                        cek_article();//fungsi memasukan data article ke dalam mutasi order line sesuai dengan scan barcode
                        save_trans_header();
                        retreive();
                        qty();
                        t_barcode.Text = "";
                    }
                   
                }
                else
                { }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //===============FUNGSI CARI ITEM FROM ENTERED===========================
        private void search_txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    
                }
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
            int good_qty_int=0;
            int count = 0;
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT article._id, article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE, article.UNIT, inventory.GOOD_QTY FROM article INNER JOIN inventory "
                            + "ON inventory.ARTICLE_ID = article._id "
                            + "WHERE article.ARTICLE_ID = '" + t_barcode.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    count = count + 1;
                    while (ckon.sqlDataRd.Read())
                    {
                        id_inv = ckon.sqlDataRd["_id"].ToString();
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        size = ckon.sqlDataRd["SIZE"].ToString();
                        color = ckon.sqlDataRd["COLOR"].ToString();
                        new_price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        unit = ckon.sqlDataRd["UNIT"].ToString();
                        good_qty = ckon2.myReader2.GetString("GOOD_QTY");
                        good_qty_int = ckon2.myReader2.GetInt32("GOOD_QTY");
                    }
                }

                //===mengecek apakah article id tersebut dengan mutasi order yang ada sudah ada di mutasi order list atau belum
                if (count == 1 && good_qty_int >= 1)
                {
                    total_amount_new = new_price * Convert.ToInt32(good_qty);
                    String cmd_moLine = "SELECT * FROM mutasiorder_line WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "' AND ARTICLE_ID = '" + art_id + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_moLine, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        MessageBox.Show("This Article Has Been Entered");
                    }
                    else
                    {
                        String cmd_insert = "INSERT INTO mutasiorder_line (MUTASI_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT, SUBTOTAL) VALUES ('" + l_transaksi.Text + "','" + art_id + "', '" + good_qty + "', '" + unit + "','" + total_amount_new + "')";
                        CRUD insert = new CRUD();
                        insert.ExecuteNonQuery(cmd_insert);
                    }
                    //ckon.cmd = new MySqlCommand(sql3, ckon.con);
                    //ckon.con.Open();
                    //ckon.myReader = ckon.cmd.ExecuteReader();
                    //if (ckon.myReader.HasRows)
                    //{
                    //    MessageBox.Show("This Article Has Been Entered");
                    //}
                    //else
                    //{
                    //    String input = "INSERT INTO mutasiorder_line (MUTASI_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT, SUBTOTAL) VALUES ('" + l_transaksi.Text + "','" + art_id + "', '" + good_qty + "', '" + unit + "','" + total_amount_new + "')";
                    //    CRUD masuk = new CRUD();
                    //    masuk.ExecuteNonQuery(input);
                    //}
                    //ckon.con.Close();
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
            //if(ckon.myReader.HasRows)
            //{
            //    count = count + 1;
            //    while (ckon.myReader.Read())
            //    {
            //        id_inv = ckon.myReader.GetString("_id");
            //        art_id = ckon.myReader.GetString("ARTICLE_ID");
            //        art_name = ckon.myReader.GetString("ARTICLE_NAME");
            //        size = ckon.myReader.GetString("SIZE");
            //        color = ckon.myReader.GetString("COLOR");
            //        new_price = ckon.myReader.GetInt32("PRICE");
            //        unit = ckon.myReader.GetString("UNIT");
            //        //===CARI TOTAL QTY BERDASARKAN _id yang didapat ke tabel inventory==
            //        String sql2 = "SELECT * FROM inventory WHERE ARTICLE_ID = '" + id_inv + "'";
            //        ckon2.cmd2 = new MySqlCommand(sql2, ckon2.con2);
            //        ckon2.con2.Open();
            //        ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //        while(ckon2.myReader2.Read())
            //        {
            //            good_qty = ckon2.myReader2.GetString("GOOD_QTY");
            //            good_qty_int = ckon2.myReader2.GetInt32("GOOD_QTY");
            //        }

            //        ckon2.con2.Close();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Article Not Found");
            //    ckon.con.Close();
            //}
            //ckon.con.Close();            
        }
        //======================SIMPAN TRANSACTION HEADER============================================
        public void save_trans_header()
        {
            DateTime mydate = DateTime.Now;
            DateTime myhour = DateTime.Now;
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM mutasiorder WHERE MUTASI_ORDER_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (!ckon.sqlDataRd.HasRows)
                {
                    String cmd_insert = "INSERT INTO mutasiorder (MUTASI_ORDER_ID,STORE_CODE,TOTAL_QTY,STATUS, DATE, TIME, CUST_ID_STORE, NO_SJ) VALUES ('" + l_transaksi.Text + "', '" + store_code + "' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_store + "','" + no_sj.Text + "') ";
                    sql.ExecuteNonQuery(cmd_insert);

                    String cmd_update = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='3'";                    
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
            //    String sql = "INSERT INTO mutasiorder (MUTASI_ORDER_ID,STORE_CODE,TOTAL_QTY,STATUS, DATE, TIME, CUST_ID_STORE, NO_SJ) VALUES ('" + l_transaksi.Text + "', '" + store_code + "' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + myhour.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_store + "','"+ no_sj.Text +"') ";
            //    ckon.con.Open();
            //    ckon.cmd = new MySqlCommand(sql, ckon.con);
            //    ckon.cmd.ExecuteNonQuery();
            //    ckon.con.Close();

            //    String query = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='3'";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);
            //}
            //else
            //{

            //}

        }
        //========================UPDATE REQ ORDER HEADER ===============================================

        public void update_header()
        {
            //String sql = "UPDATE mutasiorder SET MUTASI_FROM_WAREHOUSE ='AAB', MUTASI_TO_WAREHOUSE = '"+ combo_mutasiTo.Text +"',REQUEST_DELIVERY_DATE = '"+ tanggal_req.Text +"' ,TOTAL_QTY='" + l_qty.Text + "', STATUS='1' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
            String cmd_update = "UPDATE mutasiorder SET  MUTASI_TO_WAREHOUSE = '" + combo_mutasiTo.Text + "',REQUEST_DELIVERY_DATE = '" + tanggal_req.Text + "' ,TOTAL_QTY='" + l_qty.Text + "', STATUS='1', CUST_ID_STORE='"+ cust_id_store + "', EMPLOYEE_ID='"+epy_id+"', EMPLOYEE_NAME='"+ epy_name +"' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }

        //===============================================================================================


        //============================= BUTTON LIST MO======================================================
        private void b_list_ro_Click(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM mutasiorder WHERE  STATUS='1' ";
            UC_MO_List c = new UC_MO_List(f1);
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_MO_List.Instance))
            {
                f1.p_kanan.Controls.Add(UC_MO_List.Instance);
                UC_MO_List.Instance.Dock = DockStyle.Fill;
                UC_MO_List.Instance.holding(sql);
                UC_MO_List.Instance.Show();
            }
            else
                UC_MO_List.Instance.holding(sql);
            UC_MO_List.Instance.Show();
        }
        //================================================================================================

        //========================SEARCH ARTICLE ===============================================
        private void b_search_Click(object sender, EventArgs e)
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            MO_SearchArticle ro_search = new MO_SearchArticle();
            ro_search.t_id = l_transaksi.Text;
            ro_search.no_sj = no_sj.Text;
            ro_search.cust_id_store2 = cust_id_store;
            ro_search.store_code2 = store_code;
            ro_search.save_auto_number(bulan_now, number_trans);
            ro_search.ShowDialog();
        }
        //======================================================================================

        //===============================BUTTON CLEAR===========================================
        private void b_clear_Click(object sender, EventArgs e)
        {
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
                String cmd_delete = "DELETE FROM mutasiorder_line WHERE MUTASI_ORDER_ID ='" + l_transaksi.Text + "'";
                CRUD delete = new CRUD();
                delete.ExecuteNonQuery(cmd_delete);
                dgv_request.Rows.Clear(); l_qty.Text = "0";
            }
           
        }
        //====================================================================================
        
        //============================BUTTON CONFIRM======================================
        private void b_confirm_Click(object sender, EventArgs e)
        {
            try
            {
                this.ActiveControl = t_barcode;
                t_barcode.Focus();
                if (l_qty.Text == "0" || combo_InOut.Text == "" || combo_coverage.Text == "" || combo_mutasiTo.Text == "")
                {
                    MessageBox.Show("No Item On List And Please Select All Info");
                }
                else
                {
                    get_data_combo_store();
                    //RunningNumber running = new RunningNumber();
                    //running.get_data_before("3", "MT");
                    if (combo_InOut.Text == "IN")
                    {
                        String cmd_update = "UPDATE mutasiorder SET MUTASI_FROM_WAREHOUSE='" + combo_store2 + "' ,MUTASI_TO_WAREHOUSE = '" + store_code + "',REQUEST_DELIVERY_DATE = '" + tanggal_req.Text + "' ,TOTAL_QTY='" + l_qty.Text + "', STATUS='1', CUST_ID_STORE='" + cust_id_store + "', EMPLOYEE_ID='" + epy_id + "', EMPLOYEE_NAME='" + epy_name + "',TOTAL_AMOUNT='" + total_amount + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                        CRUD update = new CRUD();
                        update.ExecuteNonQuery(cmd_update);
                        MessageBox.Show("data successfully added");
                        reset();
                        //RunningNumber running = new RunningNumber();
                        //running.get_data_before("3", "MT");
                        holding();
                        new_invoice();
                        set_running_number();
                    }
                    else
                    {
                        if (count_eror != 0)
                        {
                            MessageBox.Show("There Is A Total Quantity That Exceeds Inventory. Please Check Again !");
                        }
                        else
                        {
                            String cmd_update = "UPDATE mutasiorder SET MUTASI_FROM_WAREHOUSE='" + store_code + "' ,MUTASI_TO_WAREHOUSE = '" + combo_store2 + "',REQUEST_DELIVERY_DATE = '" + tanggal_req.Text + "' ,TOTAL_QTY='" + l_qty.Text + "', STATUS='1', CUST_ID_STORE='" + cust_id_store + "', EMPLOYEE_ID='" + epy_id + "', EMPLOYEE_NAME='" + epy_name + "',TOTAL_AMOUNT='" + total_amount + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                            CRUD update = new CRUD();
                            update.ExecuteNonQuery(cmd_update);
                            //===POTONG INVENTORY SAAT MUTASI OUT
                            Inv_Line inv = new Inv_Line();
                            String type_trans = "3";
                            inv.cek_type_trans(type_trans);
                            inv.mutasi_out(l_transaksi.Text);
                            MessageBox.Show("data successfully added");
                            reset();
                            //RunningNumber running = new RunningNumber();
                            //running.get_data_before("3", "MT");
                            holding();
                            new_invoice();
                            set_running_number();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
           
        }
        //================================================================================

        //============method reset=======================
        public void reset()
        {
            dgv_request.Rows.Clear();
            combo_coverage.SelectedIndex = -1;
            //combo_InOut.SelectedIndex = -1;
            combo_InOut.SelectedIndex = 0;
            combo_mutasiTo.Text = "";
            l_qty.Text = "0";
            l_amount.Text = "0,00";
        }
        //======MEMBERIKAN FUNGSI FOKUS KE SCAN BARCODE SETELAH COMBO BOX DIPILIH
        private void combo_InOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
        }
        private void combo_mutasiTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
        }
        private void tanggal_req_ValueChanged(object sender, EventArgs e)
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
        }

        //METHOD UNTUK MENGECHECK TOTAL QTY DI RET_ORDER_LINE DIBANDINGKAN DENGAN TOTAL INVENTORY
        /* DESC = AKAN DIHITUNG TOTAL BARIS DARI RET_ORDER_LINE, LALU AKAN DIHITUNG BERAPA LINE YANG TIDAK SESUAI, LINE YG TIDAK SESUAI AKAN DIBANDINGKAN JUMLAHNYA DENGAN BERAPA BARIS RET_ORDER_LINE, JIKA TOTAL TIDAK SESUAI, MAKA TIDAK BISA MENJALAN METHOD "UPDATE_HEADER", JIKA JUMLAH SAMA MAKA JALANKAN METHOD UPDATE HEADER*/
        public void cek_qty_line()
        {
            CRUD sql = new CRUD();

            //ckon3.con3.Close();
            //count_eror = 0;
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM mutasiorder_line WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        art_id_mut = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        qty_mut_line = ckon3.myReader3.GetInt32("QUANTITY");
                        compare(art_id_mut, qty_mut_line);
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

            //ckon3.con3.Open();
            //ckon3.cmd3 = new MySqlCommand(sql, ckon3.con3);
            //ckon3.myReader3 = ckon3.cmd3.ExecuteReader();
            //while (ckon3.myReader3.Read())
            //{
            //    art_id_mut = ckon3.myReader3.GetString("ARTICLE_ID");
            //    qty_mut_line = ckon3.myReader3.GetInt32("QUANTITY");
            //    compare(art_id_mut, qty_mut_line);
            //}
            //ckon3.con3.Close();
        }
        public void compare(String art_id_mut2, int qty_mut_line2)
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT inventory.GOOD_QTY FROM article INNER JOIN inventory "
                            + "ON inventory.ARTICLE_ID = article._id "
                            + "WHERE article.ARTICLE_ID = '" + art_id_mut2 + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        inv_good_qty = Convert.ToInt32(ckon.sqlDataRd["GOOD_QTY"].ToString());
                        if (qty_mut_line2 > inv_good_qty)
                        {
                            count_eror = count_eror + 1;
                        }
                    }
                }
            }
            catch (Exception)
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
            //while (ckon.myReader.Read())
            //{
            //    inv_id = ckon.myReader.GetString("_id");
            //    String sql2 = "SELECT * FROM inventory WHERE ARTICLE_ID = '" + inv_id + "'";
            //    ckon2.con2.Open();
            //    ckon2.cmd2 = new MySqlCommand(sql2, ckon2.con2);
            //    ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //    while (ckon2.myReader2.Read())
            //    {
            //        inv_good_qty = ckon2.myReader2.GetInt32("GOOD_QTY");
            //        if (qty_mut_line2 > inv_good_qty)
            //        {
            //            count_eror = count_eror + 1;
            //        }
            //        else
            //        {

            //        }
            //    }
            //    ckon2.con2.Close();
            //}
            //ckon.con.Close();

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
                String cmd = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '3'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        bulan_trans = ckon.sqlDataRd["Month"].ToString();                        
                    }

                    if (bulan_now == bulan_trans)
                        //    {
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

                    final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
                    l_transaksi.Text = final_running_number;

                }
                else
                {
                    number_trans = 1;
                    bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
                                            //==MEMBUAT STRING FINAL RUNNING NUMBER

                    final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                    l_transaksi.Text = final_running_number;
                }            
            }
            catch (Exception e)
            {

                throw;
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
                   
            //        final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
            //        l_transaksi.Text = final_running_number;

            //    }
            //    else
            //    {
            //        number_trans = 1;
            //        bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
            //        //==MEMBUAT STRING FINAL RUNNING NUMBER
                   
            //        final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //        l_transaksi.Text = final_running_number;
            //    }


            //}
            //else
            //{
            //    number_trans = 1;
            //    bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
                
            //    final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //    l_transaksi.Text = final_running_number;

            //    String query = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','3')";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);

            //    //MessageBox.Show(final_running_number);
            //}
            //ckon.con.Close();
        }
    }
}
