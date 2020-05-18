using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;

namespace try_bi
{
    public partial class UC_Article_In_Out : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        koneksi3 ckon3 = new koneksi3();
        String id_trans, article_id, qty2, id_list, store_code, cust_id_store, article_dept; 
        String combo_store2, city, id_inv, unit, size, price, color, art_name, art_id, good_qty, bulan2, tipe2, art_id_mut, inv_id;
        public String epy_id, epy_name;
        //String id_store;
        int noo_inv_new, GOOD_QTY_PLUS, no_trans2, total_amount, new_price, count_eror, qty_mut_line, inv_good_qty;
        //Variable untuk running number baru
        String bulan_now, tahun_now, tahun_trans, bulan_trans, number_trans_string, final_running_number, next_qty, kode_from, kode_to, kode_tipe_from, kode_tipe_to, kode_movement;
        int comboTypeCurrentIndex, comboToCurrentIndex;
        int number_trans, transtypeid;

        private static UC_Article_In_Out _instance;
        private Form1 form1;

        private void UC_Article_In_Out_Load(object sender, EventArgs e)
        {
            combo_transtype();
            setFocus();
        }

        public static UC_Article_In_Out Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Article_In_Out(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_Article_In_Out(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        public void setFocus()
        {
            this.ActiveControl = t_barcode;
            t_barcode.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Update Article", "Form", "");
            if (input.Length > 0)
            {
                API_Article_Single art = new API_Article_Single();
                art.getData(input);
            }

            this.ActiveControl = t_barcode;
            t_barcode.Focus();
        }
        //sync all article
        private void btn_sync_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Are you sure to sync?",
                                     "Confirm Sync",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                // Your Code
                try
                {
                    API_HO_Article artHo = new API_HO_Article();
                    artHo.execArticle();
                    API_HO_Transactiontype tr = new API_HO_Transactiontype();
                    tr.execData();
                }
                catch (Exception ex) { }

                Cursor.Current = Cursors.Default;
            }
        }

        public void runRetrive()
        {
            //ckon.con.Close();
            String command = "SELECT TOP 100 * FROM ho_header where STATUS='0' AND EMPLOYEE_ID='"+ epy_id +"' ORDER BY _id ASC";
            try
            {
                ckon.sqlCon().Open();
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

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
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        l_transaksi.Text = ckon.myReader.GetString("MUTASI_ORDER_ID");
            //    }
            //}
            //else
            //{
            //    set_running_number();
            //    save_trans_header();
            //}
            retreive();
            qty();
            setFocus();
            //ckon.con.Close();
        }
        //==============================ISI COMBO================================
        public void combo_transtype()
        {            
            combo_mutasiTo.Items.Clear();
            trans_type.Items.Clear();

            try
            {
                ckon.sqlCon().Open();
                String command = "SELECT * FROM ho_transaction_type";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        String code = ckon.sqlDataRd["CODE"].ToString();
                        String name = ckon.sqlDataRd["DESCRIPTION"].ToString();
                        trans_type.Items.Add(code + " -- " + name);
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
            //String sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        String code = ckon.myReader.GetString("CODE");
            //        String name = ckon.myReader.GetString("DESCRIPTION");
            //        trans_type.Items.Add(code + " -- " + name);
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        
        public void isi_combo_mutasi(String query)
        {
            combo_mutasiTo.Items.Clear();
            //String sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    combo_mutasiTo.Items.Add("");
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        String code = ckon.myReader.GetString("CODE");
            //        String name = ckon.myReader.GetString("NAME");
            //        combo_mutasiTo.Items.Add(name + "--" + code);
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
                        String code = ckon.sqlDataRd["CODE"].ToString();
                        String name = ckon.sqlDataRd["NAME"].ToString();
                        combo_mutasiTo.Items.Add(name + "--" + code);
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
        private void getCodeFromTo(String kode)
        {
            setFocus();

            //ckon.con.Close();
            try
            {
                String command = "SELECT * FROM ho_transaction_type WHERE CODE='" + kode + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        kode_tipe_from = ckon.sqlDataRd["WAREHOUSE_FROM"].ToString();
                        kode_tipe_to = ckon.sqlDataRd["WAREHOUSE_TO"].ToString();
                        kode_movement = ckon.sqlDataRd["CODE"].ToString();
                        transtypeid = Convert.ToInt32(ckon.sqlDataRd["ID"].ToString());
                    }

                    if (kode_tipe_from == "Store")
                    {
                        kode_to = kode_tipe_to;
                        combo_mutasiTo.Text = "";
                        combo_mutasiTo.Items.Clear();
                        String query = "SELECT CODE, NAME FROM store UNION SELECT CODE, NAME FROM store_relasi";
                        isi_combo_mutasi(query);
                    }
                    else if (kode_tipe_to == "Store")
                    {
                        kode_from = kode_tipe_from;
                        combo_mutasiTo.Text = "";
                        combo_mutasiTo.Items.Clear();
                        String query = "SELECT CODE, NAME FROM store UNION SELECT CODE, NAME FROM store_relasi";
                        isi_combo_mutasi(query);
                    }
                    else
                    {
                        kode_from = kode_tipe_from;
                        kode_to = kode_tipe_to;                        
                        combo_mutasiTo.Text = "";
                        combo_mutasiTo.Items.Clear();
                        combo_mutasiTo.Items.Add(kode_to);
                        combo_mutasiTo.SelectedIndex = 0;                        
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        kode_tipe_from = ckon.myReader.GetString("WAREHOUSE_FROM");
            //        kode_tipe_to = ckon.myReader.GetString("WAREHOUSE_TO");
            //        kode_movement = ckon.myReader.GetString("CODE");
            //        transtypeid = ckon.myReader.GetInt32("ID");
            //    }
            //    ckon.con.Close();

            //    if (kode_tipe_from == "Store")
            //    {
            //        kode_to = kode_tipe_to;
            //        combo_mutasiTo.Text = "";
            //        combo_mutasiTo.Items.Clear();
            //        String query = "SELECT CODE, NAME FROM store UNION SELECT CODE, NAME FROM store_relasi";
            //        isi_combo_mutasi(query);
            //    }
            //    else if (kode_tipe_to == "Store")
            //    {
            //        kode_from = kode_tipe_from;
            //        combo_mutasiTo.Text = "";
            //        combo_mutasiTo.Items.Clear();
            //        String query = "SELECT CODE, NAME FROM store UNION SELECT CODE, NAME FROM store_relasi";
            //        isi_combo_mutasi(query);
            //    }
            //    else
            //    {
            //        kode_from = kode_tipe_from;
            //        kode_to = kode_tipe_to;
            //        //
            //        combo_mutasiTo.Text = "";
            //        combo_mutasiTo.Items.Clear();
            //        combo_mutasiTo.Items.Add(kode_to);
            //        combo_mutasiTo.SelectedIndex = 0;
            //    }
            //}
            //catch
            //{ }
        }

        private void combo_transType_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFocus();

            getCodeFromTo(trans_type.Text.Substring(0, 3));
            comboTypeCurrentIndex = trans_type.FindStringExact(trans_type.Text);

            update_header();
        }

        private void combo_mutasiTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFocus();

            String mutasiTo = (combo_mutasiTo.Text == "") ? "" : combo_mutasiTo.Text.Substring(combo_mutasiTo.Text.Length - 3);

            if (kode_tipe_from == "Store")
            {
                kode_from = mutasiTo;
            } else if (kode_tipe_to == "Store")
            {
                kode_to = mutasiTo;
            }
            comboToCurrentIndex = combo_mutasiTo.FindStringExact(combo_mutasiTo.Text);

            update_header();
        }

        private void btn_find_item_Click(object sender, EventArgs e)
        {
            //===FOKUES KE SCAN BARCODE
            setFocus();

            SearchArticleHo filter = new SearchArticleHo();
            filter.ShowDialog();
        }
        //============method reset=======================
        public void reset()
        {
            dgv_request.Rows.Clear();
            trans_type.SelectedIndex = 0;
            //combo_mutasiTo.Text = "";
            l_qty.Text = "0";
            l_amount.Text = "0,00";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setFocus();
            if (l_qty.Text == "0")
            {
                MessageBox.Show("No item on list");
            }
            else
            {
                DialogResult confirmResult = MessageBox.Show("Are you sure to delete all item?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    String sql = "DELETE FROM ho_line WHERE MUTASI_ORDER_ID ='" + l_transaksi.Text + "'";
                    CRUD delete = new CRUD();
                    delete.ExecuteNonQuery(sql);
                    dgv_request.Rows.Clear();
                    qty();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setFocus();
            if (l_qty.Text == "0" || trans_type.Text == "" || combo_mutasiTo.Text == "")
            {
                if (trans_type.Text == "")
                {
                    MessageBox.Show("Field Transaction Type is required.");
                } else if (combo_mutasiTo.Text == "")
                {
                    MessageBox.Show("Field Box Store is required.");
                } else
                {
                    MessageBox.Show("No item on list.");
                }
            }
            else
            {
                update_header();

                DialogResult confirmResult = MessageBox.Show("Are you sure to approve?",
                                     "Confirm Approve",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    API_Post_Inout movement = new API_Post_Inout();
                    movement.execMovement(l_transaksi.Text);
                    //test wahyu
                    //String query = "UPDATE ho_header SET STATUS_API='1', STATUS='1' WHERE MUTASI_ORDER_ID='" + l_transaksi.Text + "'";
                    //CRUD input = new CRUD();
                    //input.ExecuteNonQuery(query);
                    //HO_Print hp = new HO_Print();
                    //hp.set_print(l_transaksi.Text);
                    //end
                    setCurrentCombo();
                    runRetrive();
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void setCurrentCombo()
        {
            trans_type.SelectedIndex = comboTypeCurrentIndex;
            combo_mutasiTo.SelectedIndex = comboToCurrentIndex;
        }
        //==============================ACTION MINUS, PLUS OR DELETE DATA============================
        private void dgv_request_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_request.Columns[e.ColumnIndex].Name == "Delete")
            {
                String DEL = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String sql = "DELETE FROM ho_line WHERE MUTASI_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + DEL + "'";
                CRUD sql_delete = new CRUD();
                sql_delete.ExecuteNonQuery(sql);
                retreive();
                qty();
            }
            if (dgv_request.Columns[e.ColumnIndex].Name == "plus")
            {
                ckon.con.Close();
                //String _id2 = dgv_request.SelectedRows[0].Cells[0].Value.ToString();
                String ID = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String quantity = dgv_request.SelectedRows[0].Cells[7].Value.ToString();
                String subtotal = dgv_request.SelectedRows[0].Cells[10].Value.ToString();
                String price = dgv_request.SelectedRows[0].Cells[5].Value.ToString();
                int new_price = Int32.Parse(price); int new_subtotal = Int32.Parse(subtotal);
                
                int new_qty = Int32.Parse(quantity);
                new_qty = new_qty + 1;
                new_subtotal = new_subtotal + new_price;
               
                String update = "UPDATE ho_line SET QUANTITY='" + new_qty + "',SUBTOTAL='" + new_subtotal + "' WHERE MUTASI_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
                CRUD sql_update = new CRUD();
                sql_update.ExecuteNonQuery(update);
                retreive();
                qty();
            }
            if (dgv_request.Columns[e.ColumnIndex].Name == "minus")
            {
                String ID = dgv_request.SelectedRows[0].Cells[1].Value.ToString();
                String quantity = dgv_request.SelectedRows[0].Cells[6].Value.ToString();
                String subtotal = dgv_request.SelectedRows[0].Cells[8].Value.ToString();
                String price = dgv_request.SelectedRows[0].Cells[7].Value.ToString();
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
                    String update = "UPDATE ho_line SET QUANTITY='" + new_qty + "', SUBTOTAL='" + new_subtotal + "' WHERE MUTASI_ORDER_ID='" + l_transaksi.Text + "' AND ARTICLE_ID='" + ID + "'";
                    CRUD sql_update = new CRUD();
                    sql_update.ExecuteNonQuery(update);
                    retreive();
                    qty();
                }

            }

            setFocus();
        }

        //===============FUNGSI SCAN BARCODE, JIKA ADA TAMPILKAN QTY SESUAI INVENTORY===========================
        private void t_barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (l_transaksi.Text == "" || l_transaksi.Text == "0")
                    {
                        set_running_number();
                    }

                    cek_article();//fungsi memasukan data article ke dalam mutasi order line sesuai dengan scan barcode
                    save_trans_header();
                    retreive();
                    qty();
                    t_barcode.Text = "";
                    t_barcode.Focus();
                }
                else
                { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //====================AMBIL TOTAL QTY, AMOUNT FROM MUTASI ORDER========================================
        public void qty()
        {
            total_amount = 0;
            int total_qty = 0;
            //ckon.con.Close();
            try
            {
                String command = "SELECT SUM(CONVERT(INT,QUANTITY)) as total, SUM(SUBTOTAL) as total_amount FROM ho_line where MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        qty2 = ckon.sqlDataRd["total"].ToString();
                        if (qty2 != "")
                        {
                            l_qty.Text = qty2.ToString();
                            total_qty = Int32.Parse(qty2);                            
                        }
                        else
                        {
                            l_qty.Text = "0";                            
                        }

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
                    l_qty.Text = "0";
                    l_amount.Text = "0,00";
                }

                String cmd_Update = "UPDATE ho_header SET TOTAL_QTY='" + total_qty + "', TOTAL_AMOUNT='" + total_amount + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
                CRUD update = new CRUD();
                update.ExecuteNonQuery(cmd_Update);
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
            //            total_qty = Int32.Parse(qty2);

            //            total_amount = ckon.myReader.GetInt32("total_amount");
            //            l_amount.Text = string.Format("{0:#,###}" + ",00", total_amount);
            //        }
            //        catch
            //        {
            //            l_qty.Text = "0";
            //            l_amount.Text = "0,00";
            //        }
            //    }

            //}

            //String upd = "UPDATE ho_header SET TOTAL_QTY='" + total_qty + "', TOTAL_AMOUNT='" + total_amount + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
            //CRUD masuk = new CRUD();
            //masuk.ExecuteNonQuery(upd);
            //ckon.con.Close();
        }
        //
        public void to_line(String articleIdHo = "")
        {
            if (articleIdHo != "")
            {
                t_barcode.Text = articleIdHo;
            }

            cek_article();//fungsi memasukan data article ke dalam mutasi order line sesuai dengan scan barcode
            save_trans_header();
            retreive();
            qty();
        }
        //============FUNGSI CEK TABEL ARTICLE BERDASARKAN ARTICLE ID YANG TELAH DI SCAN==
        public void cek_article()
        {
            int total_amount_new = 0;
            int good_qty_int = 0;
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String sql_articleHO = "SELECT distinct TOP 1 * FROM article_ho WHERE ARTICLE_ID = '" + t_barcode.Text + "'";                
                ckon.sqlDataRd = sql.ExecuteDataReader(sql_articleHO, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_inv = ckon.sqlDataRd["_id"].ToString();
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        art_name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();                        
                        color = ckon.sqlDataRd["COLOR"].ToString();
                        new_price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        unit = ckon.sqlDataRd["UNIT"].ToString();
                        article_dept = ckon.sqlDataRd["DEPARTMENT"].ToString();
                        good_qty = "1";
                        search_txt.Text = color;
                    }
                }
                else
                {
                    String sql_hoLine = "SELECT distinct * FROM ho_line WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "' AND ARTICLE_ID = '" + art_id + "'";                    
                    ckon.sqlDataRd = sql.ExecuteDataReader(sql_hoLine, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            next_qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        }

                        int qty = Convert.ToInt32(next_qty) + 1;
                        total_amount_new = new_price * qty;
                        String upd = "UPDATE ho_line SET QUANTITY='" + qty + "', SUBTOTAL='" + total_amount_new + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "' AND ARTICLE_ID = '" + art_id + "'";                        
                        sql.ExecuteNonQuery(upd);
                    }
                    else
                    {
                        String input = "INSERT INTO ho_line (MUTASI_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT,SUBTOTAL,DEPARTMENT,PRICE,ARTICLE_NAME,COLOR) VALUES ('" + l_transaksi.Text + "','" + art_id + "', '" + good_qty + "', '" + unit + "','" + total_amount_new + "','" + article_dept + "','" + new_price + "','" + art_name + "','" + color + "')";                        
                        sql.ExecuteNonQuery(input);
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //int count = 0;
            //if (ckon.myReader.HasRows)
            //{
            //    count = count + 1;
            //    while (ckon.myReader.Read())
            //    {
            //        id_inv = ckon.myReader.GetString("_id");
            //        art_id = ckon.myReader.GetString("ARTICLE_ID");
            //        art_name = ckon.myReader.GetString("ARTICLE_NAME");
            //        //size = ckon.myReader.GetString("SIZE");
            //        color = ckon.myReader.GetString("COLOR");
            //        new_price = ckon.myReader.GetInt32("PRICE");
            //        unit = ckon.myReader.GetString("UNIT");
            //        article_dept = ckon.myReader.GetString("DEPARTMENT");
            //        good_qty = "1";
            //        search_txt.Text = color;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Article Not Found");
            //    ckon.con.Close();
            //}
            //ckon.con.Close();
            //===mengecek apakah article id tersebut dengan mutasi order yang ada sudah ada di mutasi order list atau belum
            //if (count == 1)
            //{
            //    String sql3 = "SELECT distinct * FROM ho_line WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "' AND ARTICLE_ID = '" + art_id + "'";
            //    ckon.con.Open();
            //    ckon.cmd = new MySqlCommand(sql3, ckon.con);
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    total_amount_new = new_price * Convert.ToInt32(good_qty);
            //    if (ckon.myReader.HasRows)
            //    {
            //        while (ckon.myReader.Read())
            //        {
            //            next_qty = ckon.myReader.GetString("QUANTITY");
            //        }

            //        int qt1 = Convert.ToInt32(next_qty) + 1;
            //        total_amount_new = new_price * qt1;
            //        String upd = "UPDATE ho_line SET QUANTITY='" + qt1 + "', SUBTOTAL='" + total_amount_new + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "' AND ARTICLE_ID = '" + art_id + "'";
            //        CRUD masuk = new CRUD();
            //        masuk.ExecuteNonQuery(upd);
            //    }
            //    else
            //    {   
            //        String input = "INSERT INTO ho_line (MUTASI_ORDER_ID,ARTICLE_ID,QUANTITY,UNIT,SUBTOTAL,DEPARTMENT,PRICE,ARTICLE_NAME,COLOR) VALUES ('" + l_transaksi.Text + "','" + art_id + "', '" + good_qty + "', '" + unit + "','" + total_amount_new + "','" + article_dept + "','" + new_price + "','" + art_name + "','" + color + "')";
            //        CRUD masuk = new CRUD();
            //        masuk.ExecuteNonQuery(input);
            //    }
            //    ckon.con.Close();
            //}
            t_barcode.Text = "";
            setFocus();
        }

        //======================SIMPAN TRANSACTION HEADER============================================
        public void save_trans_header()
        {
            DateTime mydate = DateTime.Now;
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String command = "SELECT distinct * FROM ho_header WHERE MUTASI_ORDER_ID ='" + l_transaksi.Text + "'";                
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (!ckon.sqlDataRd.HasRows)
                {
                    String cmd_insert = "INSERT INTO ho_header (MUTASI_ORDER_ID,STORE_CODE,TOTAL_QTY,STATUS, DATE, TIME, CUST_ID_STORE, NO_SJ, EMPLOYEE_ID, EMPLOYEE_NAME) VALUES ('" + l_transaksi.Text + "', 'HO' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + mydate.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_store + "','-','" + epy_id + "','" + epy_name + "') ";
                    sql.ExecuteNonQuery(cmd_insert);
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
            //    String sql = "INSERT INTO ho_header (MUTASI_ORDER_ID,STORE_CODE,TOTAL_QTY,STATUS, DATE, TIME, CUST_ID_STORE, NO_SJ, EMPLOYEE_ID, EMPLOYEE_NAME) VALUES ('" + l_transaksi.Text + "', 'HO' ,'0','0','" + mydate.ToString("yyyy-MM-dd") + "', '" + mydate.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_store + "','-','" + epy_id + "','" + epy_name + "') ";
            //    ckon.con.Open();
            //    ckon.cmd = new MySqlCommand(sql, ckon.con);
            //    ckon.cmd.ExecuteNonQuery();
            //    ckon.con.Close();
            //}

        }
        //========================UPDATE REQ ORDER HEADER ===============================================
        public void update_header()
        {
            DateTime mydate = DateTime.Now;
            String cmd_update = "UPDATE ho_header SET STORE_CODE = '" + kode_movement + "', MUTASI_FROM_WAREHOUSE = '" + kode_from + "', MUTASI_TO_WAREHOUSE = '" + kode_to + "',REQUEST_DELIVERY_DATE = '" + mydate.ToString("yyyy-MM-dd") + "', CUST_ID_STORE='" + cust_id_store + "', EMPLOYEE_ID='" + epy_id + "', EMPLOYEE_NAME='" + epy_name + "', TRANS_TYPE_ID='" + transtypeid + "', TRANS_TYPE_CODE='"+ kode_movement +"', DATE='"+ mydate.ToString("yyyy-MM-dd") + "', TIME='"+ mydate.ToLocalTime().ToString("H:mm:ss") + "' WHERE MUTASI_ORDER_ID = '" + l_transaksi.Text + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }
        //======MEMBERIKAN FUNGSI FOKUS KE SCAN BARCODE SETELAH COMBO BOX DIPILIH
        private void combo_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFocus();
        }
        ///
        private void dev_request_Sorting(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        private void dgv_request_FilterStringChanged(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        //===================== GET LINE ==========================================
        public void retreive(string sort = "", string filter = "", string search = "")
        {
            //ckon.con.Close();
            CRUD sql = new CRUD();
            dgv_request.Rows.Clear();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT DISTINCT ho_line.ARTICLE_ID, ho_line.QUANTITY, ho_line.UNIT, ho_line.SUBTOTAL, ho_line.ARTICLE_NAME, article_ho.SIZE, ho_line.COLOR, ho_line.PRICE, ho_line.DEPARTMENT, ho_line._id FROM ho_line LEFT JOIN article_ho ON ho_line.ARTICLE_ID=article_ho.ARTICLE_ID WHERE ho_line.MUTASI_ORDER_ID='" + l_transaksi.Text + "' ";                
                if (filter != "")
                {
                    filter = filter.Replace("ARTICLE_ID", "ho_line.ARTICLE_ID");
                    cmd += " AND " + filter;
                }
                if (sort != "")
                {
                    cmd += " ORDER BY " + sort;
                }
                else
                {
                    cmd += " ORDER BY ho_line._id DESC";
                }

                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());
                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_request.Rows.Add();
                    dgv_request.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"];
                    dgv_request.Rows[dgRows].Cells[1].Value = row["ARTICLE_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[2].Value = row["ARTICLE_NAME"].ToString();
                    dgv_request.Rows[dgRows].Cells[3].Value = row["COLOR"].ToString();
                    dgv_request.Rows[dgRows].Cells[4].Value = row["SIZE"];
                    dgv_request.Rows[dgRows].Cells[6].Value = row["QUANTITY"].ToString();
                    dgv_request.Rows[dgRows].Cells[7].Value = row["PRICE"];
                    dgv_request.Rows[dgRows].Cells[8].Value = row["SUBTOTAL"];
                    dgv_request.Rows[dgRows].Cells[9].Value = row["DEPARTMENT"].ToString();
                }
                dgv_request.Columns[7].DefaultCellStyle.Format = "#,###";
                dgv_request.Columns[8].DefaultCellStyle.Format = "#,###";
                dgv_request.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_request.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_request.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
            //        dgv_request.Rows[n].Cells[0].Value = row["ARTICLE_ID"];
            //        dgv_request.Rows[n].Cells[1].Value = row["ARTICLE_ID"].ToString();
            //        dgv_request.Rows[n].Cells[2].Value = row["ARTICLE_NAME"].ToString();
            //        dgv_request.Rows[n].Cells[3].Value = row["COLOR"].ToString();
            //        dgv_request.Rows[n].Cells[4].Value = row["SIZE"];
            //        dgv_request.Rows[n].Cells[6].Value = row["QUANTITY"].ToString();
            //        dgv_request.Rows[n].Cells[7].Value = row["PRICE"];
            //        dgv_request.Rows[n].Cells[8].Value = row["SUBTOTAL"];
            //        dgv_request.Rows[n].Cells[9].Value = row["DEPARTMENT"].ToString();
            //    }
            //    dgv_request.Columns[7].DefaultCellStyle.Format = "#,###";
            //    dgv_request.Columns[8].DefaultCellStyle.Format = "#,###";
            //    dgv_request.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    dgv_request.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    dgv_request.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }

        private void b_list_history_Click(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM ho_header WHERE  STATUS='1' ";
            UC_Article_IO_List c = new UC_Article_IO_List(f1);
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_Article_IO_List.Instance))
            {
                f1.p_kanan.Controls.Add(UC_Article_IO_List.Instance);
                UC_Article_IO_List.Instance.Dock = DockStyle.Fill;
                UC_Article_IO_List.Instance.holding(sql);
                UC_Article_IO_List.Instance.Show();
            }
            else
            {
                UC_Article_IO_List.Instance.holding(sql);
                UC_Article_IO_List.Instance.Show();
            }
        }

        public void set_running_number()
        {
            DateTime mydate = DateTime.Now;
            CRUD sql = new CRUD();

            bulan_now = mydate.ToString("MM");
            tahun_now = mydate.ToString("yy");
            number_trans = 0;

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM auto_number WHERE Store_Code = 'HO' AND Type_Trans = '45' AND Month='" + bulan_now + "' AND Year='" + tahun_now + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        number_trans = Convert.ToInt32(ckon.sqlDataRd["Number"].ToString());
                    }

                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');//wahyu

                    final_running_number = "IO/HO-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                    l_transaksi.Text = final_running_number;

                    String cmd_update = "UPDATE auto_number SET Number = '" + number_trans + "' WHERE Type_Trans='45' AND Year='" + tahun_now + "' AND Month='" + bulan_now + "'";                    
                    sql.ExecuteNonQuery(cmd_update);
                }
                else
                {
                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');//wahyu

                    final_running_number = "IO/HO-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                    l_transaksi.Text = final_running_number;

                    String cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans) VALUES ('HO','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','45')";                    
                    sql.ExecuteNonQuery(cmd_insert);
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

            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        number_trans = ckon.myReader.GetInt32("Number");
            //    }

            //    number_trans = number_trans + 1;
            //    number_trans_string = number_trans.ToString().PadLeft(5, '0');//wahyu

            //    final_running_number = "IO/HO-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
            //    l_transaksi.Text = final_running_number;

            //    String query = "UPDATE auto_number SET Number = '" + number_trans + "' WHERE Type_Trans='45' AND Year='" + tahun_now + "' AND Month='" + bulan_now + "'";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);
            //}
            //else
            //{
            //    number_trans = number_trans + 1;
            //    number_trans_string = number_trans.ToString().PadLeft(5, '0');//wahyu

            //    final_running_number = "IO/HO-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
            //    l_transaksi.Text = final_running_number;

            //    String query = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans) VALUES ('HO','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','45')";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);
            //}
            //ckon.con.Close();
        }
        //
    }
}
