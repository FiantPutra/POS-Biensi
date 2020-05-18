using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace try_bi
{
    public partial class UC_Popup_Info : UserControl
    {
        public static Form1 f1;
        public String comp_name;
        String date, store_name, shift_name, nm_cur, cust_id_store, id_epy, nm_epy, store_id, string_id_shift, string_id_Cstore, SHIFT, status_store, status_shift, tipe_shift, bulan_shift, tipe_closing_shift, bulan_CloseStore, tipe_closing_CloseStore;
        koneksi ckon = new koneksi();
        int open_trans, open_petty, open_deposit, pantek_open_trans_balance=0, inv_id_shift, inv_id_Cstore, set_open_trans, set_open_petty, set_open_deposit, no_trans_shift, no_trans_CloseStore;
        //======================================================
        private static UC_Popup_Info _instance;
        public static UC_Popup_Info Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Popup_Info(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_Popup_Info(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //====MEMBERIKAN PILIHAN SHIFT KE SHIFT 1 SETIAP HALAMAN DIBUKA==
        public void set_shift()
        {
            combo_spg.SelectedIndex = 0;
        }
        //=======SET ANGKA UNTUK INFO PERHITUNGAN DI POP UP DEPAN DARI TABEL CLOSING SHIFT========
        public void set_angka()
        {
            CRUD sql = new CRUD();

            cek_status_Cstore();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM closing_shift WHERE STATUS_CLOSE='1' ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        SHIFT = ckon.sqlDataRd["SHIFT"].ToString();
                        set_open_trans = Convert.ToInt32(ckon.sqlDataRd["REAL_TRANS_BALANCE"].ToString());
                        set_open_petty = Convert.ToInt32(ckon.sqlDataRd["REAL_PETTY_CASH"].ToString());
                        set_open_deposit = Convert.ToInt32(ckon.sqlDataRd["REAL_DEPOSIT"].ToString());
                    }

                    if (SHIFT == "1" && status_store == "0")
                    {
                        if (set_open_trans == 0)
                        { l_cash_amount.Text = "0,00"; }
                        else { l_cash_amount.Text = string.Format("{0:#,###}" + ",00", set_open_trans); }

                        if (set_open_petty == 0)
                        { l_petty_amount.Text = "0,00"; }
                        else { l_petty_amount.Text = string.Format("{0:#,###}" + ",00", set_open_petty); }

                        if (set_open_deposit == 0)
                        { l_dep_amount.Text = "0,00"; }
                        else { l_dep_amount.Text = string.Format("{0:#,###}" + ",00", set_open_deposit); }                        
                    }
                    if (SHIFT == "2" && status_store == "0")
                    {
                        if (set_open_trans == 0)
                        { l_cash_amount.Text = "0,00"; }
                        else { l_cash_amount.Text = string.Format("{0:#,###}" + ",00", set_open_trans); }

                        if (set_open_petty == 0)
                        { l_petty_amount.Text = "0,00"; }
                        else { l_petty_amount.Text = string.Format("{0:#,###}" + ",00", set_open_petty); }

                        if (set_open_deposit == 0)
                        { l_dep_amount.Text = "0,00"; }
                        else { l_dep_amount.Text = string.Format("{0:#,###}" + ",00", set_open_deposit); }
                    }

                    if ((SHIFT == "2" && status_store == "1") || (SHIFT == "1" && status_store == "1"))
                    {

                        l_cash_amount.Text = "0,00";

                        if (set_open_petty == 0)
                        { l_petty_amount.Text = "0,00"; }
                        else { l_petty_amount.Text = string.Format("{0:#,###}" + ",00", set_open_petty); }

                        if (set_open_deposit == 0)
                        { l_dep_amount.Text = "0,00"; }
                        else { l_dep_amount.Text = string.Format("{0:#,###}" + ",00", set_open_deposit); }
                    }                    
                }
                else
                {
                    l_cash_amount.Text = "0,00"; 
                    l_petty_amount.Text = "0,00"; 
                    l_dep_amount.Text = "0,00";
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

            //ckon.con.Close();

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        SHIFT = ckon.myReader.GetString("SHIFT");
            //        set_open_trans = ckon.myReader.GetInt32("REAL_TRANS_BALANCE");
            //        set_open_petty = ckon.myReader.GetInt32("REAL_PETTY_CASH");
            //        set_open_deposit = ckon.myReader.GetInt32("REAL_DEPOSIT");
            //    }
            //    if(SHIFT=="1" && status_store =="0")
            //    {
            //        if (set_open_trans == 0)
            //        { l_cash_amount.Text = "0,00"; }
            //        else { l_cash_amount.Text = string.Format("{0:#,###}" + ",00", set_open_trans); }

            //        if (set_open_petty == 0)
            //        { l_petty_amount.Text = "0,00"; }
            //        else { l_petty_amount.Text = string.Format("{0:#,###}" + ",00", set_open_petty); }

            //        if (set_open_deposit == 0)
            //        { l_dep_amount.Text = "0,00"; }
            //        else { l_dep_amount.Text = string.Format("{0:#,###}" + ",00", set_open_deposit); }
            //        //====================================================================

            //    }
            //    if(SHIFT=="2" && status_store == "0")
            //    {
            //        if (set_open_trans == 0)
            //        { l_cash_amount.Text = "0,00"; }
            //        else { l_cash_amount.Text = string.Format("{0:#,###}" + ",00", set_open_trans); }

            //        if (set_open_petty == 0)
            //        { l_petty_amount.Text = "0,00"; }
            //        else { l_petty_amount.Text = string.Format("{0:#,###}" + ",00", set_open_petty); }

            //        if (set_open_deposit == 0)
            //        { l_dep_amount.Text = "0,00"; }
            //        else { l_dep_amount.Text = string.Format("{0:#,###}" + ",00", set_open_deposit); }
            //    }

            //    if ((SHIFT == "2" && status_store == "1")|| (SHIFT == "1" && status_store == "1"))
            //    {

            //         l_cash_amount.Text = "0,00";

            //        if (set_open_petty == 0)
            //        { l_petty_amount.Text = "0,00"; }
            //        else { l_petty_amount.Text = string.Format("{0:#,###}" + ",00", set_open_petty); }

            //        if (set_open_deposit == 0)
            //        { l_dep_amount.Text = "0,00"; }
            //        else { l_dep_amount.Text = string.Format("{0:#,###}" + ",00", set_open_deposit); }
            //    }

            //    //l_diskon.Text = string.Format("{0:#,###}" + ",00", get_diskon);
            //}
            //else
            //{ l_cash_amount.Text = "0,00"; l_petty_amount.Text= "0,00"; l_dep_amount.Text = "0,00"; }
            //ckon.con.Close();
        }
        //========================BUTTON OK ==================================================
        private void b_ok_Click(object sender, EventArgs e)
        {
            try
            {
                cek_status_Cstore();
                c_Closing_Shift();
                if (combo_spg.Text == "")
                {
                    MessageBox.Show("Please Select Shift First");
                }
                else
                {
                    if (combo_spg.Text == "Shift 2" && status_store == "1")
                    {
                        MessageBox.Show("Can't Open Shift");
                    }
                    else
                    {
                        if (combo_spg.Text == "Shift 1" && status_store == "0")
                        {
                            MessageBox.Show("Please Select Another Shift");
                        }
                        else
                        {
                            //Cek apakah dia PC Master atau PC Slave, Jika dia offline brarti dia konek ke web local, tak perlu closing store saat buka shift
                            if(Properties.Settings.Default.OnnOrOff == "Offline")
                            {
                                save_modal_tbl();
                                DateTime mydate = DateTime.Now;
                                date = mydate.ToString("yyyy-MM-dd");
                                uc_coba c = new uc_coba(f1);
                                f1.p_kanan.Controls.Clear();
                                if (!f1.p_kanan.Controls.Contains(uc_coba.Instance))
                                {
                                    f1.p_kanan.Controls.Add(uc_coba.Instance);
                                    uc_coba.Instance.Dock = DockStyle.Fill;
                                    //uc_coba.Instance.shift_name_trans = shift_name;
                                    uc_coba.Instance.nm_cur2 = nm_cur;
                                    uc_coba.Instance.cust_id_store2 = cust_id_store;
                                    uc_coba.Instance.new_invoice();
                                    uc_coba.Instance.isi_combo_spg();
                                    uc_coba.Instance.holding(date);
                                    uc_coba.Instance.id_employe2 = id_epy;
                                    uc_coba.Instance.comp_name = comp_name;
                                    uc_coba.Instance.runRetreive();
                                    uc_coba.Instance.BringToFront();
                                }
                                else
                                {
                                    //uc_coba.Instance.shift_name_trans = shift_name;
                                    uc_coba.Instance.nm_cur2 = nm_cur;
                                    uc_coba.Instance.cust_id_store2 = cust_id_store;
                                    uc_coba.Instance.new_invoice();
                                    uc_coba.Instance.isi_combo_spg();
                                    uc_coba.Instance.holding(date);
                                    uc_coba.Instance.id_employe2 = id_epy;
                                    uc_coba.Instance.comp_name = comp_name;
                                    uc_coba.Instance.runRetreive();
                                    uc_coba.Instance.BringToFront();
                                }
                            }
                            //PC tersebut Online
                            else
                            {
                                if (status_shift == "1" && combo_spg.Text == "Shift 2" && tipe_shift == "2")
                                {
                                    MessageBox.Show("Please Close Store First");
                                }
                                else
                                {
                                    save_modal_tbl();
                                    DateTime mydate = DateTime.Now;
                                    date = mydate.ToString("yyyy-MM-dd");
                                    uc_coba c = new uc_coba(f1);
                                    f1.p_kanan.Controls.Clear();
                                    if (!f1.p_kanan.Controls.Contains(uc_coba.Instance))
                                    {
                                        f1.p_kanan.Controls.Add(uc_coba.Instance);
                                        uc_coba.Instance.Dock = DockStyle.Fill;
                                        //uc_coba.Instance.shift_name_trans = shift_name;
                                        uc_coba.Instance.nm_cur2 = nm_cur;
                                        uc_coba.Instance.cust_id_store2 = cust_id_store;
                                        uc_coba.Instance.new_invoice();
                                        uc_coba.Instance.isi_combo_spg();
                                        uc_coba.Instance.holding(date);
                                        uc_coba.Instance.id_employe2 = id_epy;
                                        uc_coba.Instance.comp_name = comp_name;
                                        uc_coba.Instance.runRetreive();
                                        uc_coba.Instance.BringToFront();
                                    }
                                    else
                                    {
                                        //uc_coba.Instance.shift_name_trans = shift_name;
                                        uc_coba.Instance.nm_cur2 = nm_cur;
                                        uc_coba.Instance.cust_id_store2 = cust_id_store;
                                        uc_coba.Instance.new_invoice();
                                        uc_coba.Instance.isi_combo_spg();
                                        uc_coba.Instance.holding(date);
                                        uc_coba.Instance.id_employe2 = id_epy;
                                        uc_coba.Instance.comp_name = comp_name;
                                        uc_coba.Instance.runRetreive();
                                        uc_coba.Instance.BringToFront();
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        //=============================BUTTON CLOSE==================================
        private void b_close_Click(object sender, EventArgs e)
        {
            //CEK STATUS TERAKHIR DARI TABEL CLOSING STORE
            cek_status_Cstore();
            //JIKA STATUS 0, MAKA KASIH HAK AKSES (MASUK KE MENU CLOSING STORE)|| JIKA STATUS ADALAH 1 MAKA JGN BERI HAK AKSES
            if(status_store == "0")
            {
                //MEMBERI TULISAN CLOSING STORE KE LABEL 3 DI FORM 1
                f1.label3.Text = "Closing Store";
                f1.status_buka_menu = "1";
                DateTime mydate = DateTime.Now;
                date = mydate.ToString("yyyy-MM-dd");
                UC_Closing_Store c = new UC_Closing_Store(f1);
                f1.p_kanan.Controls.Clear();
                if (!f1.p_kanan.Controls.Contains(UC_Closing_Store.Instance))
                {
                    f1.p_kanan.Controls.Add(UC_Closing_Store.Instance);
                    UC_Closing_Store.Instance.Dock = DockStyle.Fill;
                    UC_Closing_Store.Instance.set_name(id_epy, nm_epy);
                    UC_Closing_Store.Instance.get_id_close();
                    UC_Closing_Store.Instance.from_form1();
                    UC_Closing_Store.Instance.total_trans();
                    UC_Closing_Store.Instance.BringToFront();
                }
            }
            else
            {
                MessageBox.Show("Please Open Shift First");
            }
        }

        //========================================================================================

        //=============GET NAME STORE==========================================================
        public void get_name()
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
                        store_id = ckon.sqlDataRd["CODE"].ToString();
                        store_name = ckon.sqlDataRd["NAME"].ToString();
                        cust_id_store = ckon.sqlDataRd["CUST_ID_STORE"].ToString();
                    }
                    t_store_name.Text = store_name;
                    t_store_name.ReadOnly = true;
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
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //=====================================================================================

        //===============================PUBLIC VOID SAVE DATA TO MODAL_STORE DB===============
        public void save_modal_tbl()
        {
            if(combo_spg.Text=="Shift 1")
            {
                shift_name = "1";
                inv_shift();
                inv_CStore();
                //save_closing_shift();
                save_closing_store();
            }
            else
            {
                shift_name = "2";
                inv_shift();
                save_closing_shift();
            }

        }
      
        //==========================GET DATA CURRENCY==================================
        public void get_currency()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM currency";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        nm_cur = ckon.sqlDataRd["NAME"].ToString();
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
            
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    nm_cur = ckon.myReader.GetString("NAME");
            //}
            //ckon.con.Close();
        }
        //=============================================================================


        //=============SET NAME ADMIN  LOGIN AND ID EMPLOYEE==========================
        public void set_name(String id, String name)
        {
            id_epy = id;
            nm_epy = name;

            l_E_id.Text = id_epy;
            l_name.Text = nm_epy;
        }
        //============================================================================

        //========================METHOD SAVE INTO TABLE CLOSING_SHIFT WHEN OPENED SHIFT====================
        public void save_closing_shift()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1* FROM closing_shift ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        open_trans = Convert.ToInt32(ckon.sqlDataRd["REAL_TRANS_BALANCE"].ToString());
                        open_petty = Convert.ToInt32(ckon.sqlDataRd["REAL_PETTY_CASH"].ToString());
                        open_deposit = Convert.ToInt32(ckon.sqlDataRd["REAL_DEPOSIT"].ToString());
                    }
                }
                else
                {
                    open_trans = 0; 
                    open_petty = 0; 
                    open_deposit = 0;
                }

                string name = System.Environment.MachineName;
                DateTime mydate = DateTime.Now;
                String time_now = mydate.ToString("yyyy-MM-dd H:mm:ss");
                String cmd_insert = "INSERT INTO closing_shift (ID_SHIFT,STORE_ID,SHIFT,OPENING_TIME,OPENING_TRANS_BALANCE, OPENING_PETTY_CASH,OPENING_DEPOSIT,DEVICE_NAME,EMPLOYEE_ID,EMPLOYEE_NAME) VALUES ('" + string_id_shift + "','" + store_id + "', '" + shift_name + "', '" + time_now + "','" + open_trans + "','" + open_petty + "', '" + open_deposit + "','" + name + "','"+ id_epy +"','"+ nm_epy +"')";                
                sql.ExecuteNonQuery(cmd_insert);

                String cmd_update = "UPDATE auto_number SET Month = '" + bulan_shift + "', Number = '" + no_trans_shift + "' WHERE Type_Trans='" + tipe_closing_shift + "'";      
                sql.ExecuteNonQuery(cmd_update);
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
        //==================================================================================================
        //========================METHOD SAVE INTO TABLE CLOSING_STORE WHEN OPENED SHIFT====================
        public void save_closing_store()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM closing_store ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        open_trans = Convert.ToInt32(ckon.sqlDataRd["REAL_TRANS_BALANCE"].ToString());
                        open_petty = Convert.ToInt32(ckon.sqlDataRd["REAL_PETTY_CASH"].ToString());
                        open_deposit = Convert.ToInt32(ckon.sqlDataRd["REAL_DEPOSIT"].ToString());
                    }
                }
                else
                {
                    open_trans = 0; 
                    open_petty = 0; 
                    open_deposit = 0;
                }

                string name = System.Environment.MachineName;
                String cmd_insert = "";
                String cmd_update = "";
                DateTime mydate = DateTime.Now;
                String time_now = mydate.ToString("yyyy-MM-dd H:mm:ss");
                cmd_insert = "INSERT INTO closing_store (ID_C_STORE,STORE_ID,OPENING_TIME,OPENING_TRANS_BALANCE, OPENING_PETTY_CASH,OPENING_DEPOSIT,DEVICE_NAME) VALUES ('" + string_id_Cstore + "','" + store_id + "', '" + time_now + "','" + open_trans + "','" + open_petty + "', '" + open_deposit + "','" + name + "')";                
                sql.ExecuteNonQuery(cmd_insert);

                cmd_update = "UPDATE auto_number SET Month = '" + bulan_shift + "', Number = '" + no_trans_shift + "' WHERE Type_Trans='" + tipe_closing_shift + "'";                
                sql.ExecuteNonQuery(cmd_update);

                cmd_insert = "INSERT INTO closing_shift (ID_SHIFT,STORE_ID,SHIFT,OPENING_TIME,OPENING_TRANS_BALANCE, OPENING_PETTY_CASH,OPENING_DEPOSIT,DEVICE_NAME,EMPLOYEE_ID,EMPLOYEE_NAME) VALUES ('" + string_id_shift + "','" + store_id + "', '" + shift_name + "', '" + time_now + "','" + pantek_open_trans_balance + "','" + open_petty + "', '" + open_deposit + "','" + name + "','" + id_epy + "','" + nm_epy + "')";                
                sql.ExecuteNonQuery(cmd_insert);

                cmd_update = "UPDATE auto_number SET Month = '" + bulan_CloseStore + "', Number = '" + no_trans_CloseStore + "' WHERE Type_Trans='" + tipe_closing_CloseStore + "'";                
                sql.ExecuteNonQuery(cmd_update);
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
        //=========================NEW INVOICE CLOSING SHIFT=================================
       public void inv_shift()
        {
            RunningNumber running = new RunningNumber();
            running.get_data_before("7", "CS");
            
        }
        public void get_running_number_shift(String number, String bulan, int no_trans, String tipe)
        {
            string_id_shift = number;
            bulan_shift = bulan;
            no_trans_shift = no_trans;
            tipe_closing_shift = tipe;
        }
        //=========================NEW INVOICE CLOSING SHIFT=================================
        public void inv_CStore()
        {
            RunningNumber running = new RunningNumber();
            running.get_data_before("8", "CL");
            
        }
        public void get_running_number_CloseStore(String number, String bulan, int no_trans, String tipe)
        {
            string_id_Cstore = number;
            bulan_CloseStore = bulan;
            no_trans_CloseStore = no_trans;
            tipe_closing_CloseStore = tipe;
        }
        //===================================================================================

        //FUNGSI UNTUK MENGECHECK, JIKA SHIFT 1 DIPILIH, NAMUN BELUM CLOSE STORE, MAKA MUNCULKAN PESAN=====
        public void cek_status_Cstore()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM closing_store  ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        status_store = ckon.sqlDataRd["STATUS_CLOSE"].ToString();
                    }
                }
                else
                {
                    status_store = "1";
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
        //BERFUNGSI UNTUK MENGECHECK STATUS TERAKHIR CLOSING SHIFT, BERGUNA UNTUK TOMBOL CLOSING STORE, JIKA 1 MAKA JGN IZINKAN BUKA, 0 IZINKAN BUKA, DATA PERTAMA KALI SAMA DENGAN 0==============
        public void c_Closing_Shift()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM closing_shift ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        status_shift = ckon.sqlDataRd["STATUS_CLOSE"].ToString();
                        tipe_shift = ckon.sqlDataRd["SHIFT"].ToString();
                    }
                }
                else
                {
                    status_shift = "1";
                    tipe_shift = "2";
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
       
    }
}

