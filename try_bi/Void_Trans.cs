using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace try_bi
{
    public partial class Void_Trans : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        String id_trans, new_idTrans, new_idTrans2, bulan2, tipe2;
        public String cust_id, employe_id, receipt_id, spg_id, discount, total, status, payment, cash, edc, changee, bank_name, no_ref, store_code, spg_id2,store_code2, no_ref2,id_shift2,id_c_store,edc2,cust_id_Store,curr, bank_name2, spg_id_line, vou_id, vou_code, employee_id, cust_store_id, comp_nam;
        int noo_inv_new, voucher, no_trans2, head_stts = 0;
        //======================variabel yang digunakan di number sequence==========
        String bulan_now, tahun_now, type_trans="1",bulan_trans, number_trans_string, final_running_number, awal_number= "TR", id_shift, id_CStore;
        int number_trans;
        private void b_ok_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();
            //wahyu 13-11-19
            //set_running_number();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT a.TRANSACTION_ID FROM [transaction] a LEFT JOIN transaction_line b ON a.`TRANSACTION_ID`=b.`TRANSACTION_ID` WHERE b.transaction_ID is null AND STATUS IN ('0','3') and COMP_NAME='" + comp_nam + "' ORDER BY a._id asc LIMIT 1";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        head_stts = 1;
                        new_idTrans = ckon.sqlDataRd["TRANSACTION_ID"].ToString();
                    }
                }

                DateTime mydate = DateTime.Now;

                Inv_Line inv = new Inv_Line();
                String type_trans = "4";
                inv.cek_type_trans(type_trans);
                inv.void_trans(id_trans);
                //====================================
                String a = mydate.ToString("yyyy-MM-dd");
                save_trans_header();
                save_trans_line();
                save_trans_header_new();
                //
                this.Close();
                f1.label3.Text = "TRANSACTION";
                f1.p_kanan.Controls.Clear();
                if (!f1.p_kanan.Controls.Contains(uc_coba.Instance))
                {
                    f1.p_kanan.Controls.Add(uc_coba.Instance);
                    f1.setProp();
                    uc_coba.Instance.Dock = DockStyle.Fill;
                    uc_coba.Instance.holding(a);
                    uc_coba.Instance.id_employe2 = employee_id;
                    uc_coba.Instance.voidClick(new_idTrans2);
                    uc_coba.Instance.BringToFront();
                }
                else
                {
                    f1.p_kanan.Controls.Add(uc_coba.Instance);
                    f1.setProp();
                    uc_coba.Instance.Dock = DockStyle.Fill;
                    uc_coba.Instance.holding(a);
                    uc_coba.Instance.id_employe2 = employee_id;
                    uc_coba.Instance.voidClick(new_idTrans2);
                    uc_coba.Instance.BringToFront();
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

            //String sqlStatus = "UPDATE transaction SET STATUS='3' WHERE STATUS='0' AND COMP_NAME='" + comp_nam + "'";
            //CRUD ubah = new CRUD();
            //ubah.ExecuteNonQuery(sqlStatus);
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        head_stts = 1;
            //        new_idTrans = ckon.myReader.GetString("TRANSACTION_ID");
            //    }
            //} else
            //{
            //    new_idTrans = final_running_number;
            //}
            //ckon.con.Close();
            //end            
        }

        private void b_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Void_Trans_Load(object sender, EventArgs e)
        {
            this.ActiveControl = t_remark;
            t_remark.Focus();
            new_invoice();
            get_data_id();
        }


        public Void_Trans(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        public void get_id_trans(String id)
        {
            id_trans = id;
        }

        //=================================GENERATOR NUMBER=================================
        public void new_invoice()
        {
            //===================AMBIL ID DARI TABEL CLOSING SHIFT===================
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd_clShift = "SELECT TOP 1 ID_SHIFT FROM closing_shift ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_clShift, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_shift = ckon.sqlDataRd["ID_SHIFT"].ToString();
                    }                    
                }

                String cmd_clStore = "SELECT TOP 1 ID_C_STORE FROM closing_store ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_clStore, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_CStore = ckon.sqlDataRd["ID_C_STORE"].ToString();
                    }                    
                }

                String cmd_store = "SELECT CODE FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_store, ckon.sqlCon());

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

                throw;
            }

            //String sql3 = "SELECT ID_SHIFT FROM closing_shift ORDER BY _id DESC LIMIT 1";
            //ckon.cmd = new MySqlCommand(sql3, ckon.con);
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
            //==================AMBIL ID DARI TABEL CLOSING STORE=====================
            //ckon.con.Close();
            //String sql4 = "SELECT ID_C_STORE FROM closing_store ORDER BY _id DESC LIMIT 1";
            //ckon.cmd = new MySqlCommand(sql4, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_CStore = ckon.myReader.GetString("ID_C_STORE");
            //    }
            //}
            //ckon.con.Close();
            //==================AMBIL ID DARI TABLE STORE=============================
            //ckon.con.Close();
            //String sql2 = "SELECT CODE FROM store";
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
        }
        //=================== GET DATA ID===================================================
        public void get_data_id()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM [transaction] WHERE TRANSACTION_ID='" + id_trans + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        cust_id = ckon.sqlDataRd["CUSTOMER_ID"].ToString();
                        store_code2 = ckon.sqlDataRd["STORE_CODE"].ToString();
                        receipt_id = ckon.sqlDataRd["RECEIPT_ID"].ToString();
                        spg_id = ckon.sqlDataRd["SPG_ID"].ToString();
                        discount = ckon.sqlDataRd["DISCOUNT"].ToString();
                        total = ckon.sqlDataRd["TOTAL"].ToString();
                        status = ckon.sqlDataRd["STATUS"].ToString();
                        payment = ckon.sqlDataRd["PAYMENT_TYPE"].ToString();
                        cash = ckon.sqlDataRd["CASH"].ToString();
                        edc = ckon.sqlDataRd["EDC"].ToString();
                        edc2 = ckon.sqlDataRd["EDC2"].ToString();
                        changee = ckon.sqlDataRd["CHANGEE"].ToString();
                        voucher = Convert.ToInt32(ckon.sqlDataRd["VOUCHER"].ToString());
                        bank_name = ckon.sqlDataRd["BANK_NAME"].ToString();
                        bank_name2 = ckon.sqlDataRd["BANK_NAME2"].ToString();
                        no_ref = ckon.sqlDataRd["NO_REF"].ToString();
                        no_ref2 = ckon.sqlDataRd["NO_REF2"].ToString();
                        cust_id_Store = ckon.sqlDataRd["CUST_ID_STORE"].ToString();
                        curr = ckon.sqlDataRd["CURRENCY"].ToString();
                        id_shift2 = ckon.sqlDataRd["ID_SHIFT"].ToString();
                        id_c_store = ckon.sqlDataRd["ID_C_STORE"].ToString();
                        vou_id = ckon.sqlDataRd["VOUCHER_ID"].ToString();
                        vou_code = ckon.sqlDataRd["VOUCHER_CODE"].ToString();
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
            //     cust_id = ckon.myReader.GetString("CUSTOMER_ID");
            //    store_code2 = ckon.myReader.GetString("STORE_CODE");
            //     receipt_id = ckon.myReader.GetString("RECEIPT_ID");
            //     spg_id = ckon.myReader.GetString("SPG_ID");
            //     discount = ckon.myReader.GetString("DISCOUNT");
            //     total = ckon.myReader.GetString("TOTAL");
            //     status = ckon.myReader.GetString("STATUS");
            //     payment =  ckon.myReader.GetString("PAYMENT_TYPE");
            //     cash = ckon.myReader.GetString("CASH");
            //     edc = ckon.myReader.GetString("EDC");
            //    edc2 = ckon.myReader.GetString("EDC2");
            //    changee = ckon.myReader.GetString("CHANGEE");
            //    voucher = ckon.myReader.GetInt32("VOUCHER");
            //     bank_name = ckon.myReader.GetString("BANK_NAME");
            //    bank_name2 = ckon.myReader.GetString("BANK_NAME2");
            //    no_ref = ckon.myReader.GetString("NO_REF");
            //    no_ref2 = ckon.myReader.GetString("NO_REF2");
            //    cust_id_Store  = ckon.myReader.GetString("CUST_ID_STORE");
            //    curr  = ckon.myReader.GetString("CURRENCY");
            //    id_shift2  = ckon.myReader.GetString("ID_SHIFT");
            //    id_c_store = ckon.myReader.GetString("ID_C_STORE");
            //    vou_id = ckon.myReader.GetString("VOUCHER_ID");
            //    vou_code = ckon.myReader.GetString("VOUCHER_CODE");
            //}
            //ckon.con.Close();
        }
        //======================INPUT TRANSAKSI HEADER======================================
        public void save_trans_header()
        {            
            //ckon.con.Close();
            DateTime mydate = DateTime.Now;

            if (head_stts == 1)
            { //update by delete
                String cmd_delete = "DELETE FROM [transaction] WHERE TRANSACTION_ID='"+ new_idTrans + "'";
                CRUD delete = new CRUD();
                delete.ExecuteNonQuery(cmd_delete);
            }

            String cmd_insert = "INSERT INTO [transaction] (TRANSACTION_ID,STORE_CODE,CUSTOMER_ID,EMPLOYEE_ID,RECEIPT_ID,SPG_ID,DISCOUNT,TOTAL,STATUS,PAYMENT_TYPE,CASH,EDC,EDC2,CHANGEE,VOUCHER,BANK_NAME,BANK_NAME2,NO_REF,NO_REF2, DATE, TIME,CUST_ID_STORE,CURRENCY,ID_SHIFT,ID_C_STORE,VOUCHER_ID,VOUCHER_CODE,REFERENCE_DOC,COMP_NAME) VALUES ('" + new_idTrans + "', '" + store_code2 + "','" + cust_id + "','" + employee_id + "','" + receipt_id + "','" + spg_id + "','-" + discount + "', '-" + total + "', '2', '" + payment + "', '-" + cash + "', '-" + edc + "','-" + edc2 + "', '-" + changee + "', '-" + voucher + "','" + bank_name + "', '" + bank_name2 + "','" + no_ref + "','" + no_ref2 + "', '" + mydate.ToString("yyyy-MM-dd") + "', '" + mydate.ToLocalTime().ToString("H:mm:ss") + "','" + cust_id_Store + "','" + curr + "','" + id_shift2 + "','" + id_c_store + "','" + vou_id + "', '" + vou_code + "', '" + id_trans + "', '" + comp_nam + "') ";
            CRUD insert = new CRUD();
            insert.ExecuteNonQuery(cmd_insert);

            String cmd_update = "UPDATE [transaction] SET STATUS=2 WHERE TRANSACTION_ID='" + id_trans + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);

            //String query = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='1'";
            //CRUD ubah = new CRUD();
            //ubah.ExecuteNonQuery(query);                       
        }

        //======================INPUT TRANSAKSI HEADER NEW======================================
        public void save_trans_header_new()
        {            
            set_running_number();
            new_idTrans2 = final_running_number;

            ckon.con.Close();
            DateTime mydate = DateTime.Now;

            String cmd_insert = "INSERT INTO [transaction] (TRANSACTION_ID,STORE_CODE,CUSTOMER_ID,EMPLOYEE_ID,RECEIPT_ID,SPG_ID,STATUS,DATE,TIME,CUST_ID_STORE,CURRENCY,ID_SHIFT,ID_C_STORE,REFERENCE_DOC,COMP_NAME) VALUES ('" + new_idTrans2 + "', '" + store_code + "','" + cust_id + "','" + employee_id + "','" + receipt_id + "','" + spg_id + "','0', '" + mydate.ToString("yyyy-MM-dd") + "', '" + mydate.ToLocalTime().ToString("H:mm:ss") + "','" + cust_store_id + "','" + curr + "','" + id_shift + "','" + id_CStore + "', '" + id_trans + "', '" + comp_nam + "') ";
            CRUD insert = new CRUD();
            insert.ExecuteNonQuery(cmd_insert);            

            //String query = "UPDATE auto_number SET Month = '" + bulan_now + "', Number = '" + number_trans + "' WHERE Type_Trans='1'";
            //CRUD ubah = new CRUD();
            //ubah.ExecuteNonQuery(query);            
        }
        //======================INPUT TRANSAKSI LINE========================================
        public void save_trans_line()
        {
            CRUD sql = new CRUD();

            try
            {
                //ckon.con.Close();
                //koneksi2 ckon2 = new koneksi2();
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM transaction_line WHERE TRANSACTION_ID = '" + id_trans + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        String article_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        String qty = ckon.sqlDataRd["QUANTITY"].ToString();
                        String price = ckon.sqlDataRd["PRICE"].ToString();
                        String subtotal = ckon.sqlDataRd["SUBTOTAL"].ToString();
                        String discount = ckon.sqlDataRd["DISCOUNT"].ToString();
                        String spg_id_line = ckon.sqlDataRd["SPG_ID"].ToString();
                        String dis_code = ckon.sqlDataRd["DISCOUNT_CODE"].ToString();
                        String dis_type = ckon.sqlDataRd["DISCOUNT_TYPE"].ToString();

                        String cmd_insert = "INSERT INTO transaction_line(TRANSACTION_ID, ARTICLE_ID, QUANTITY, PRICE,DISCOUNT, SUBTOTAL,SPG_ID,DISCOUNT_CODE, DISCOUNT_TYPE) VALUES ('" + new_idTrans + "', '" + article_id + "', '-" + qty + "', '" + price + "', '-" + discount + "','-" + subtotal + "','" + spg_id_line + "','" + dis_code + "', '" + dis_type + "')";
                        sql.ExecuteNonQuery(cmd_insert);
                    }
                }
                //ckon.cmd = new MySqlCommand(sql, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    String article_id = ckon.myReader.GetString("ARTICLE_ID");
                //    String qty = ckon.myReader.GetString("QUANTITY");
                //    String price = ckon.myReader.GetString("PRICE");
                //    String subtotal = ckon.myReader.GetString("SUBTOTAL");
                //    String discount = ckon.myReader.GetString("DISCOUNT");
                //    String spg_id_line = ckon.myReader.GetString("SPG_ID");
                //    String dis_code = ckon.myReader.GetString("DISCOUNT_CODE");
                //    String dis_type = ckon.myReader.GetString("DISCOUNT_TYPE");

                //    String SQL2 = "INSERT INTO transaction_line(TRANSACTION_ID, ARTICLE_ID, QUANTITY, PRICE,DISCOUNT, SUBTOTAL,SPG_ID,DISCOUNT_CODE, DISCOUNT_TYPE) VALUES ('" + new_idTrans + "', '" + article_id + "', '-" + qty + "', '" + price + "', '-" + discount + "','-" + subtotal + "','" + spg_id_line + "','" + dis_code + "', '" + dis_type + "')";
                //    ckon2.con2.Open();
                //    ckon2.cmd2 = new MySqlCommand(SQL2, ckon2.con2);
                //    ckon2.cmd2.ExecuteNonQuery();
                //    ckon2.con2.Close();
                //}
                //ckon.con.Close();
            }
            catch(Exception e)
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
        //=========METHOD GET DATA FROM AUTO_NUMBER TABLE FOR SALES TRANSACTION
        public void set_running_number()
        {
            CRUD sql = new CRUD();
            DateTime mydate = DateTime.Now;
            bulan_now = mydate.ToString("MM");
            tahun_now = mydate.ToString("yy");
            number_trans = 0;

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 Number FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '1' AND Month='" + bulan_now + "' AND Year='" + tahun_now + "'";
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
                        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
                    else
                        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;

                    String cmd_update = "UPDATE auto_number SET Number = '" + number_trans + "' WHERE Type_Trans='1' AND Year='" + tahun_now + "' AND Month='" + bulan_now + "'";                    
                    sql.ExecuteNonQuery(cmd_update);
                }
                else
                {
                    number_trans = number_trans + 1;
                    number_trans_string = number_trans.ToString().PadLeft(5, '0');//wahyu

                    String cmd_insert = "";
                    if (Properties.Settings.Default.DevCode == "null")
                    {
                        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
                        cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','1')";
                    }
                    else
                    {
                        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
                        cmd_insert = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans,Dev_Code) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','1','" + Properties.Settings.Default.DevCode + "')";
                    }                    
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
            //    number_trans_string = number_trans.ToString().PadLeft(5, '0');//
            //    if (Properties.Settings.Default.DevCode == "null")
            //        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string;
            //    else
            //        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;

            //    String query = "UPDATE auto_number SET Number = '" + number_trans + "' WHERE Type_Trans='1' AND Year='" + tahun_now + "' AND Month='" + bulan_now + "'";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);
            //}
            //else
            //{
            //    number_trans = number_trans + 1;
            //    number_trans_string = number_trans.ToString().PadLeft(5, '0');//wahyu

            //    String query = "";
            //    if (Properties.Settings.Default.DevCode == "null")
            //    {
            //        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
            //        query = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','1')";
            //    }
            //    else
            //    {
            //        final_running_number = "TR/" + store_code + "-" + tahun_now + "" + bulan_now + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
            //        query = "INSERT INTO auto_number (Store_Code,Year,Month,Number,Type_Trans,Dev_Code) VALUES ('" + store_code + "','" + tahun_now + "','" + bulan_now + "','" + number_trans + "','1','" + Properties.Settings.Default.DevCode + "')";
            //    }

            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);

            //}
            //ckon.con.Close();
        }
    }
}
