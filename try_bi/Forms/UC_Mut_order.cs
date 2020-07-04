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
using try_bi.Class;

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
        //==========================================================================

        //====================ACTION COMBOBOX COVERAGE ==============================
        private void combo_coverage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(combo_coverage.Text == "City")
            {
                this.ActiveControl = t_barcode;
                t_barcode.Focus();
                combo_mutasiTo.Text = "";                
                String sql = "SELECT * FROM store_relasi WHERE CITY='" + city + "' AND code != '" + store_code + "'";
                isi_combo_mutasi(sql);
            }
            else if(combo_coverage.Text == "Regional")
            {
                this.ActiveControl = t_barcode;
                t_barcode.Focus();
                combo_mutasiTo.Text = "";
                String sql = "SELECT * FROM store_relasi WHERE REGIONAL = '"+ region +"' AND code != '"+ store_code +"'";
                isi_combo_mutasi(sql);           
            } 
            else
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
            
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT mutasiorder.MUTASI_ORDER_ID FROM mutasiorder "                            
                            + "WHERE STATUS_API = '0' AND TOTAL_QTY > 0";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["MUTASI_ORDER_ID"].ToString();
                        //article_id = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        //numbersList.Add(ckon.sqlDataRd["ARTICLE_NAME"].ToString());

                        //string[] numbersArray = numbersList.ToArray();
                        //numbersList.Clear();
                        //string result = String.Join(", ", numbersArray);
                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_trans;
                        //dgv_hold.Rows[dgRows].Cells[1].Value = result;
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
        //=======================================================================================

        //=====================ITEM mutasi ORDER ==========================================
        public void retreive(string sort="", string filter="", string search="")
        {
            CRUD sql = new CRUD();

            dgv_request.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT  mutasiorder_line.ARTICLE_ID ,mutasiorder_line.QUANTITY, mutasiorder_line.UNIT,mutasiorder_line.SUBTOTAL, article._id,article.ARTICLE_NAME, itemdimensionsize.Description as SIZE_ID, itemdimensioncolor.Description as COLOR_ID, article.PRICE FROM mutasiorder_line, article, itemdimensioncolor, itemdimensionsize  WHERE article.ARTICLE_ID = mutasiorder_line.ARTICLE_ID AND itemdimensioncolor.Id = article.COLOR_ID AND itemdimensionsize.Id = article.SIZE_ID AND mutasiorder_line.MUTASI_ORDER_ID='" + l_transaksi.Text + "' ";
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
                    dgv_request.Rows[dgRows].Cells[3].Value = row["SIZE_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[4].Value = row["COLOR_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[5].Value = row["PRICE"];
                    dgv_request.Rows[dgRows].Cells[7].Value = row["QUANTITY"].ToString();
                    dgv_request.Rows[dgRows].Cells[9].Value = row["UNIT"];
                    dgv_request.Rows[dgRows].Cells[10].Value = row["SUBTOTAL"];
                }
                dgv_request.Columns[5].DefaultCellStyle.Format = "#,###";
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
                        if (ckon.sqlDataRd["total"].ToString() != "")
                        {
                            qty2 = ckon.sqlDataRd["total"].ToString();
                            l_qty.Text = qty2.ToString();
                        }
                        else
                        {
                            l_qty.Text = "0";
                        }
                    }
                }
                else
                {
                    l_qty.Text = "0";
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
                        if (ckon.sqlDataRd["total_amount"].ToString() != "")
                        {
                            total_amount = Convert.ToInt32(ckon.sqlDataRd["total_amount"].ToString());
                            l_amount.Text = string.Format("{0:#,###}" + ",00", total_amount);
                        }              
                        else
                        {
                            l_amount.Text = "0,00";
                        }
                    }
                }
                else
                {
                    l_amount.Text = "0,00";
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
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //=============================================================================================

        public void runRetreive()
        {
            CRUD sql = new CRUD();

            try
            {                
                String cmd_trans = "SELECT TOP 1 MUTASI_ORDER_ID FROM mutasiorder WHERE STATUS='0' ORDER BY _id asc";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_trans, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        l_transaksi.Text = ckon.sqlDataRd["MUTASI_ORDER_ID"].ToString();
                    }
                }
                else
                {                                        
                    set_running_number();
                    save_trans_header();                    
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

        //================================BUTTON HOLD=================================================
        private void b_hold_Click(object sender, EventArgs e)
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
            if (l_qty.Text == "0")
            {                
                MessageBox.Show("Pick Item First");
            }
            else
            {
                String cmd_update = "UPDATE mutasiorder SET MUTASI_FROM_WAREHOUSE='" + combo_store2 + "' ,MUTASI_TO_WAREHOUSE = '" + store_code + "',REQUEST_DELIVERY_DATE = '" + tanggal_req.Text + "' ,TOTAL_QTY='" + l_qty.Text + "', CUST_ID_STORE='" + cust_id_store + "', EMPLOYEE_ID='" + epy_id + "', EMPLOYEE_NAME='" + epy_name + "',TOTAL_AMOUNT='" + total_amount + "', NO_SJ = '" + no_sj.Text + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                CRUD update = new CRUD();
                update.ExecuteNonQuery(cmd_update);

                new_invoice();
                //set_running_number();
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
                    String cmd = "SELECT * FROM inventory WHERE ARTICLE_ID = '" + _id2 + "'";
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
                    updateInventArticle();

                    if (Properties.Settings.Default.OnnOrOff == "Offline")
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
                String cmd = "SELECT article._id, article.ARTICLE_ID, article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID, article.PRICE, article.UNIT, inventory.GOOD_QTY FROM article INNER JOIN inventory "
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
                        size = ckon.sqlDataRd["SIZE_ID"].ToString();
                        color = ckon.sqlDataRd["COLOR_ID"].ToString();
                        new_price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        unit = ckon.sqlDataRd["UNIT"].ToString();
                        good_qty = ckon.sqlDataRd["GOOD_QTY"].ToString();
                        good_qty_int = Convert.ToInt32(ckon.sqlDataRd["GOOD_QTY"]);
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
        //========================UPDATE REQ ORDER HEADER ===============================================

        public void update_header()
        {            
            String cmd_update = "UPDATE mutasiorder SET  MUTASI_TO_WAREHOUSE = '" + combo_mutasiTo.Text + "',REQUEST_DELIVERY_DATE = '" + tanggal_req.Text + "' ,TOTAL_QTY='" + l_qty.Text + "', CUST_ID_STORE='"+ cust_id_store + "', EMPLOYEE_ID='"+epy_id+"', EMPLOYEE_NAME='"+ epy_name +"' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
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
            updateInventArticle();

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
            API_MutasiOrder mutasiOrder = new API_MutasiOrder();
            bool api_response;

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
                    
                    if (combo_InOut.Text == "IN")
                    {
                        String cmd_update = "UPDATE mutasiorder SET MUTASI_FROM_WAREHOUSE='" + combo_store2 + "' ,MUTASI_TO_WAREHOUSE = '" + store_code + "',REQUEST_DELIVERY_DATE = '" + tanggal_req.Text + "' ,TOTAL_QTY='" + l_qty.Text + "', STATUS='1', CUST_ID_STORE='" + cust_id_store + "', EMPLOYEE_ID='" + epy_id + "', EMPLOYEE_NAME='" + epy_name + "',TOTAL_AMOUNT='" + total_amount + "', NO_SJ = '" + no_sj.Text + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                        CRUD update = new CRUD();
                        update.ExecuteNonQuery(cmd_update);

                        api_response = mutasiOrder.mutasiOrder().Result;
                        if (api_response)
                        {                            
                            MessageBox.Show("data successfully added");
                            reset();
                            holding();
                            new_invoice();
                            set_running_number();
                            save_trans_header();
                        }
                        else
                        {
                            MessageBox.Show("Make Sure You are Connected To Internet");
                        }
                    }
                    else
                    {
                        if (count_eror != 0)
                        {
                            MessageBox.Show("There Is A Total Quantity That Exceeds Inventory. Please Check Again !");
                        }
                        else
                        {
                            String cmd_update = "UPDATE mutasiorder SET MUTASI_FROM_WAREHOUSE='" + store_code + "' ,MUTASI_TO_WAREHOUSE = '" + combo_store2 + "',REQUEST_DELIVERY_DATE = '" + tanggal_req.Text + "' ,TOTAL_QTY='" + l_qty.Text + "', STATUS='1', CUST_ID_STORE='" + cust_id_store + "', EMPLOYEE_ID='" + epy_id + "', EMPLOYEE_NAME='" + epy_name + "',TOTAL_AMOUNT='" + total_amount + "', NO_SJ = '" + no_sj.Text + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                            CRUD update = new CRUD();
                            update.ExecuteNonQuery(cmd_update);

                            api_response = mutasiOrder.mutasiOrder().Result;
                            if (api_response)
                            {                                
                                //===POTONG INVENTORY SAAT MUTASI OUT
                                Inv_Line inv = new Inv_Line();
                                String type_trans = "3";
                                inv.cek_type_trans(type_trans);
                                inv.mutasi_out(l_transaksi.Text);
                                MessageBox.Show("data successfully added");
                                reset();
                                holding();
                                new_invoice();
                                set_running_number();
                                save_trans_header();
                            }
                            else
                            {
                                MessageBox.Show("Make Sure You are Connected To Internet");
                            }
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
            no_sj.Text = "";
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
                String cmd = "SELECT TOP 1 Number FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '3' AND Month='" + bulan_now + "' AND Year='" + tahun_now + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        number_trans = Convert.ToInt32(ckon.sqlDataRd["Number"].ToString());
                    }

                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');//
                    if (Properties.Settings.Default.DevCode == "null")
                        final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                    else
                        final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
                    l_transaksi.Text = final_running_number;

                    String cmd_update = "UPDATE auto_number SET Number = '" + number_trans + "' WHERE Type_Trans='3' AND Year='" + tahun_now + "' AND Month='" + bulan_now + "'";
                    CRUD update = new CRUD();
                    update.ExecuteNonQuery(cmd_update);
                }
                else
                {
                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');//wahyu

                    String cmd_insert = "";
                    if (Properties.Settings.Default.DevCode == "null")
                    {
                        final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                        cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','3')";
                    }
                    else
                    {
                        final_running_number = "MT/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
                        cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans,Dev_Code) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','3','" + Properties.Settings.Default.DevCode + "')";
                    }
                    l_transaksi.Text = final_running_number;

                    CRUD insert = new CRUD();
                    insert.ExecuteNonQuery(cmd_insert);
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

        public void updateInventArticle()
        {
            CRUD sql = new CRUD();
            int inventArtId;

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT ARTICLE_ID FROM inventory GROUP BY ARTICLE_ID HAVING COUNT(*) > 1";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        inventArtId = Convert.ToInt32(ckon.sqlDataRd["ARTICLE_ID"].ToString());

                        string cmd_update = "UPDATE inventory SET GOOD_QTY = (SELECT SUM(GOOD_QTY) FROM inventory WHERE ARTICLE_ID = '" + inventArtId + "') WHERE ARTICLE_ID = '" + inventArtId + "'";
                        sql.ExecuteNonQuery(cmd_update);

                        string cmd_delete = "WITH Temp " +
                                                "AS(SELECT Article_ID, ROW_NUMBER() OVER(PARTITION by Article_ID ORDER BY Article_ID) AS duplicateRecCount FROM inventory where ARTICLE_ID = '" + inventArtId + "') " +
                                                "DELETE FROM Temp WHERE duplicateRecCount > 1";
                        sql.ExecuteNonQuery(cmd_delete);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
