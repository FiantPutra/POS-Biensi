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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PrinterUtility;
using System.Collections;
using System.IO;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class UC_Trans_hist : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();

        public String employee_id, cust_store_id, comp_name;
        String id_trans, article_id, id_list, date, view_date, a_date, a_jam, a_id, a_total, a_name, a_sub, a_subtotal, pay_type, rev, string_tipe, string_diskon, id_shift, id_bank1, id_bank2, bank_name1, bank_name2;

        private void dgv_purchase_FilterStringChanged(object sender, EventArgs e)
        {
            var sort = dgv_purchase.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_purchase.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        private void dgv_purchase_Sorting(object sender, EventArgs e)
        {
            var sort = dgv_purchase.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_purchase.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        int totall, int_subtotal, edc2, st;
        
        int diskon, total, cash, edc, change, tot_dis, tot_pay, get_diskon, get_voucher, get_dis_vou;
        String noref, noref2;
        //-----variable validasi tanggal
        String tgl_trans, tgl_validasi;
        int days = 7;
        //======================================================
        private static UC_Trans_hist _instance;
        public static UC_Trans_hist Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Trans_hist(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_Trans_hist(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            DateTime mydate = DateTime.Now;
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    date = mydate.ToString("yyyy-MM-dd");
                    id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    l_transaksi.Text = id_list;
                    retreive();
                    itung_total();
                    get_data_id();
                }
            }
            catch (Exception)
            {

            }
        }
        //=============================GET DATA ID SHIFT===============================
        public void get_id_shift()
        {
            //ckon.con.Close();
            //String qClosingShift = "SELECT * FROM closing_shift ORDER BY _id DESC LIMIT 1";
            //ckon.cmd = new MySqlCommand(qClosingShift, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_shift = ckon.myReader.GetString("ID_SHIFT");
            //    }
            //}
            //ckon.con.Close();
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM closing_shift ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_shift = ckon.sqlDataRd["ID_SHIFT"].ToString();
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
        //MENCARI ID TRANSAKSI BERDASARKAN KOLOM PENCARIAN====
        private void t_search_trans_OnTextChange(object sender, EventArgs e)
        {
            if (t_search_trans.text == "")
            {
                String sql = "SELECT * FROM transactions WHERE DATE = '"+ d_tgl_trans.Text +"' AND (STATUS='1' or STATUS='2')";
                holding(sql);
            }
            else
            {
                String sql = "SELECT * FROM transactions WHERE TRANSACTION_ID LIKE '%" + t_search_trans.text + "%' AND DATE = '"+ d_tgl_trans.Text +"' AND (STATUS='1' or STATUS='2')";
                holding(sql);
            }
        }
        //======================LIST HOLD TRANSACTION============================================
        public void holding(String cmd)
        {
            ////String date2;
            ////date2 = tanggal;
            //dgv_hold.Rows.Clear();
            //koneksi2 ckon2 = new koneksi2();
            //ckon.con.Close();
            ////String sql = "SELECT * FROM transaction WHERE ID_SHIFT='" + id_shift + "' AND (STATUS='1' OR STATUS = '2')";
            ////String sql = "SELECT * FROM transaction WHERE DATE = '"+ date2 +"' AND (STATUS='1' OR STATUS = '2')";
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
            //        //String sql2 = "SELECT article.ARTICLE_NAME FROM transaction_line, article  WHERE article.ARTICLE_ID = transaction_line.ARTICLE_ID AND transaction_line.TRANSACTION_ID='" + id_trans + "'";
            //        //ckon2.cmd2 = new MySqlCommand(sql2, ckon2.con2);
            //        //ckon2.con2.Open();
            //        //ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //        //while (ckon2.myReader2.Read())
            //        //{
            //        //    article_id = ckon2.myReader2.GetString("ARTICLE_NAME");
            //        //    numbersList.Add(Convert.ToString(ckon2.myReader2["ARTICLE_NAME"]));
            //        //}
            //        //string[] numbersArray = numbersList.ToArray();
            //        //numbersList.Clear();
            //        //string result = String.Join(", ", numbersArray);
            //        int st_api = ckon.myReader.GetInt32("STATUS_API");
            //        String tr_date = ckon.myReader.GetString("DATE");


            //        int n = dgv_hold.Rows.Add();
            //        dgv_hold.Rows[n].Cells[0].Value = id_trans;
            //        //dgv_hold.Rows[n].Cells[1].Value = result;
            //        dgv_hold.Rows[n].Cells[1].Value = tr_date + " " + view_date;
            //        dgv_hold.Rows[n].Cells[2].Value = st_api.ToString(); 
            //        ckon2.con2.Close();
            //    }

            //}
            //ckon.con.Close();
            CRUD sql = new CRUD();            
            int st_api;
            string tr_date;
            int dgRows;

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
                        view_date = ckon.sqlDataRd["TIME"].ToString();
                        st_api = Convert.ToInt32(ckon.sqlDataRd["STATUS_API"].ToString());
                        tr_date = ckon.sqlDataRd["DATE"].ToString();

                        dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_trans;
                        dgv_hold.Rows[dgRows].Cells[1].Value = tr_date + " " + view_date;
                        dgv_hold.Rows[dgRows].Cells[2].Value = st_api.ToString();
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
        //=======================================================================================
        private void b_void_Click(object sender, EventArgs e)
        {
            DateTime mydate = DateTime.Now;
            CRUD sql = new CRUD();

            //cek validasi tanggal, lebih dari 7 hari maka tidak bisa buka menu remark void
            DateTime dt = Convert.ToDateTime(tgl_trans);//convert tanggal transaksi
            DateTime tgl_validasi = dt.AddDays(days); //ambil tanggal 7 hari dari tanggal transaksi
            String tgl_skrng = mydate.ToString("yyyy-MM-dd"); //ambil tanggal hari ini
            DateTime tgl_skrng2 = Convert.ToDateTime(tgl_skrng); //convert tanggal skrng 
            TimeSpan ts = new TimeSpan();
            ts = tgl_skrng2.Subtract(dt);//ambil brapa jeda hari dari tgl skrng ke tanggal transaksi yg udh ditambah 7 hari
            int count_day = Convert.ToInt32(ts.Days);//ubah jeda hari kedalam variabel
                                                     //jika lebih dari 7 hari maka transaksi tidak bisa di void 
            //MessageBox.Show(count_day + "");
            if (count_day >= 7)
            {
                MessageBox.Show("Transaction Can't Be Void");
            }
            else
            {
                //wahyu
                //ckon.con.Close();
                //String sql = "SELECT STATUS FROM transaction WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
                //ckon.cmd = new MySqlCommand(sql, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    st = ckon.myReader.GetInt32("STATUS");
                //}
                //ckon.con.Close();                
                try
                {
                    ckon.sqlCon().Open();
                    String cmd = "SELECT STATUS FROM [transaction] WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            st = Convert.ToInt32(ckon.sqlDataRd["STATUS"].ToString());
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

                if (st == 2)
                {
                    MessageBox.Show("Transaction has been voided");
                } else
                {
                    Void_Trans VT = new Void_Trans(f1);
                    VT.get_id_trans(l_transaksi.Text);
                    VT.spg_id = t_spgId.Text;
                    VT.employee_id = employee_id;
                    VT.cust_store_id = cust_store_id;
                    VT.comp_nam = comp_name;
                    VT.ShowDialog();
                }
            }
            //============================
        }
        //=======================================================================================
        private void d_tgl_trans_ValueChanged(object sender, EventArgs e)
        {
            //String a = d_tgl_trans.Value.ToString("yyyy-MM-dd");
            String sql = "SELECT * FROM [transaction] WHERE DATE = '" + d_tgl_trans.Text + "' AND (STATUS='1' OR STATUS='2')";
            holding(sql);
        }
        //=============================================================================================

        //===============TAMPILKAN DATA PENJUALAN===================================================
        public void retreive(string sort = "", string filter = "", string search = "")
        {
            String art_id, art_name, spg_id, size, color, qty, disc_desc, sub_total2;
            int price, sub_total;
            CRUD sql = new CRUD();      
            //ckon.con.Close();

            dgv_purchase.Rows.Clear();            

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
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
                String cmd = "SELECT transaction_line.ARTICLE_ID ,transaction_line.QUANTITY, transaction_line.SUBTOTAL, transaction_line.SPG_ID,transaction_line.DISCOUNT, transaction_line.DISCOUNT_DESC,article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE FROM transaction_line, article  WHERE article.ARTICLE_ID = transaction_line. ARTICLE_ID AND transaction_line.TRANSACTION_ID='" + l_transaksi.Text + "' ";
                if (filter != "")
                {
                    filter = filter.Replace("ARTICLE_ID", "transaction_line.ARTICLE_ID");
                    cmd += " AND " + filter;
                }

                if (sort != "")
                {
                    cmd += " ORDER BY " + sort;
                }
                else
                {
                    cmd += " ORDER BY transaction_line._id DESC";
                }
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

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
                    dgv_purchase.Columns[5].DefaultCellStyle.Format = "#,###";
                    dgv_purchase.Columns[7].DefaultCellStyle.Format = "#,###";
                    dgv_purchase.Columns[8].DefaultCellStyle.Format = "#,###";
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
        //===========================ITUNG TOTAL BELANJA=====================================================
        public void itung_total()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            //=====================================GET VALUE DISCOUNT FROM TRANSACTION HEADER======================
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
            //    { l_diskon.Text = "0,00"; }
            //    else
            //    { l_diskon.Text = string.Format("{0:#,###}" + ",00", get_diskon); }
            //    if(get_voucher==0)
            //    { l_voucher.Text = "0,00"; }
            //    else
            //    { l_voucher.Text = string.Format("{0:#,###}" + ",00", get_voucher); }
            //}
            //catch
            //{
            //    get_dis_vou = 0;
            //    l_diskon.Text = "0,00";
            //    l_voucher.Text = "0,00";
            //}            

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM [TRANSACTION] WHERE TRANSACTION_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
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

            //String sql = "SELECT SUM(transaction_line.SUBTOTAL) as total, SUM(transaction_line.QUANTITY) as qty, sum(QUANTITY*PRICE) AS amount FROM transaction_line WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {

            //        totall = ckon.myReader.GetInt32("total");
            //        qty_txt.Text = ckon.myReader.GetString("qty");
            //        amount_txt.Text = string.Format("{0:#,###}" + ",00", ckon.myReader.GetInt32("amount"));
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
                String cmd = "SELECT SUM(transaction_line.SUBTOTAL) as total, SUM(transaction_line.QUANTITY) as qty, sum(QUANTITY*PRICE) AS amount FROM transaction_line WHERE TRANSACTION_ID='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        totall = Convert.ToInt32(ckon.sqlDataRd["TOTAL"].ToString());
                        qty_txt.Text = ckon.sqlDataRd["QTY"].ToString();
                        amount_txt.Text = string.Format("{0:#,###}" + ",00", Convert.ToInt32(ckon.sqlDataRd["AMOUNT"].ToString()));
                        totall = totall - get_voucher;
                        l_total.Text = string.Format("{0:#,###}" + ",00", totall);
                    }
                }
                else
                {
                    qty_txt.Text = "0,00";
                    amount_txt.Text = "0,00";
                    l_total.Text = "0,00";
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
        //=================== GET DATA ID===================================================
        public void get_data_id()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            //String sql = "SELECT * FROM transaction WHERE TRANSACTION_ID='" + id_list + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    t_custId.Text = ckon.myReader.GetString("CUSTOMER_ID");
            //    t_spgId.Text = ckon.myReader.GetString("SPG_ID");
            //     diskon = ckon.myReader.GetInt32("DISCOUNT");
            //     total = ckon.myReader.GetInt32("TOTAL");
            //     cash = ckon.myReader.GetInt32("CASH");
            //     edc = ckon.myReader.GetInt32("EDC");
            //    edc2 = ckon.myReader.GetInt32("EDC2");
            //     change = ckon.myReader.GetInt32("CHANGEE");
            //     noref = ckon.myReader.GetString("NO_REF");
            //    noref2 = ckon.myReader.GetString("NO_REF2");
            //    pay_type = ckon.myReader.GetString("PAYMENT_TYPE");
            //    tgl_trans = ckon.myReader.GetString("DATE");
            //    id_bank1 = ckon.myReader.GetString("BANK_NAME");
            //    id_bank2 = ckon.myReader.GetString("BANK_NAME2");

            //}

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM [transaction] WHERE TRANSACTION_ID='" + id_list + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        t_custId.Text = ckon.sqlDataRd["CUSTOMER_ID"].ToString();
                        t_spgId.Text = ckon.sqlDataRd["SPG_ID"].ToString();
                        diskon = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                        total = Convert.ToInt32(ckon.sqlDataRd["TOTAL"].ToString());
                        cash = Convert.ToInt32(ckon.sqlDataRd["CASH"].ToString());
                        edc = Convert.ToInt32(ckon.sqlDataRd["EDC"].ToString());
                        edc2 = Convert.ToInt32(ckon.sqlDataRd["EDC2"].ToString());
                        change = Convert.ToInt32(ckon.sqlDataRd["CHANGEE"].ToString());
                        noref = ckon.sqlDataRd["NO_REF"].ToString();
                        noref2 = ckon.sqlDataRd["NO_REF2"].ToString();
                        pay_type = ckon.sqlDataRd["PAYMENT_TYPE"].ToString();
                        tgl_trans = ckon.sqlDataRd["DATE"].ToString();
                        id_bank1 = ckon.sqlDataRd["BANK_NAME"].ToString();
                        id_bank2 = ckon.sqlDataRd["BANK_NAME2"].ToString();
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

            if (pay_type == "0")
            {
                string_tipe = "CASH";
                rev = "-";
            }
            if (pay_type == "1")
            {
                string_tipe = "EDC";
                rev = noref;
                //mencari bank name sesuai bank id
                get_id_bank(id_bank1, id_bank2);
            }
            if (pay_type == "2")
            {
                string_tipe = "SPLIT";
                rev = noref;
                //mencari bank name sesuai bank id
                get_id_bank(id_bank1, id_bank2);
            }
            if (pay_type == "3")
            {
                string_tipe = "SPLIT EDC";
                rev = noref;
                //mencari bank name sesuai bank id
                get_id_bank(id_bank1, id_bank2);
            }
            //MENCETAK METHOD PEMBAYARAN
            desc_method_payment();
            tot_dis = total + diskon;
            tot_pay = cash + edc;
        }
        //mencetak method pembayaran ke textbox
        public void desc_method_payment()
        {
            if(string_tipe == "CASH")
            {
                t_payment_method.Text = "Payment Method : Cash";
                t_detail_charge.Text = "";
            }
            if(string_tipe == "EDC")
            {
                t_payment_method.Text = "Payment Method : EDC";
                t_detail_charge.Text = bank_name1 + " : " + string.Format("{0:#,###}" + ",00", edc)+"-No Ref : "+noref;
            }
            if (string_tipe == "SPLIT")
            {
                t_payment_method.Text = "Payment Method : SPLIT";
                t_detail_charge.Text = "Cash : " + string.Format("{0:#,###}" + ",00", cash)+" . "+bank_name1 + " : "+ string.Format("{0:#,###}" + ",00", edc) + "-No Ref : " + noref;
            }
            if (string_tipe == "SPLIT EDC")
            {
                t_payment_method.Text = "Payment Method : SPLIT EDC";
                t_detail_charge.Text = bank_name1 + " : " + string.Format("{0:#,###}" + ",00", edc) + "-No Ref : "+ noref +". "+bank_name2+" : "+ string.Format("{0:#,###}" + ",00", edc2)+"-No Ref : "+noref2;
            }
        }
        //==================================================================================
        //mengambil id bank berdasarkan nama bank yang dipilih===
        public void get_id_bank(string _id_bank1, string _id_bank2)
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            //String sql = "SELECT BANK_NAME FROM bank WHERE BANK_ID LIKE '%" + id_bank1 + "%'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    bank_name1 = ckon.myReader.GetString("BANK_NAME");
            //}
            //ckon.con.Close();
            //String sql2 = "SELECT BANK_NAME FROM bank WHERE BANK_ID LIKE '%" + id_bank2 + "%'";
            //ckon.cmd = new MySqlCommand(sql2, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    bank_name2 = ckon.myReader.GetString("BANK_NAME");
            //}
            //ckon.con.Close();

            if (_id_bank1 != string.Empty)
            {
                try
                {
                    ckon.sqlCon().Open();
                    String cmd_bank1 = "SELECT BANK_NAME FROM bank WHERE BANK_ID LIKE '%" + _id_bank1 + "%'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_bank1, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            bank_name1 = ckon.sqlDataRd["BANK_NAME"].ToString();
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

            if (_id_bank2 != string.Empty)
            {
                try
                {
                    ckon.sqlCon().Open();
                    String cmd_bank2 = "SELECT BANK_NAME FROM bank WHERE BANK_ID LIKE '%" + _id_bank2 + "%'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_bank2, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            bank_name2 = ckon.sqlDataRd["BANK_NAME"].ToString();
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
        }
        //=========================================================================
        private void b_print_Click(object sender, EventArgs e)
        {
            ckon.con.Close();

            LinkApi ls = new LinkApi();
            if (ls.print_default == "1")
            {
                PrintThermal print = new PrintThermal();
                print.get_trans_id(id_list);
                print.get_nm_store();
                print.get_currency();
                print.get_trans_header();
                print.coba_print();
            }
            else
            {
                NewFunctionPrinter print = new NewFunctionPrinter();
                print.get_trans_id(id_list);
                print.get_nm_store();
                print.get_currency();
                print.get_trans_header();
                print.coba_print();
            }
        }
    }
}
