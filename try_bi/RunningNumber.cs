using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace try_bi
{
    class RunningNumber
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String store_code, bulan_now, tahun_now,bulan_trans, tahun_trans, type_trans, awal_number, number_trans_string, final_running_number;
        int number_trans;
        //=======METHOD UNTUK MENDAPATKAN DATA DARI FORM SEBELUMNYA==============
        public void get_data_before(String tipe, String awal)
        {
            type_trans = tipe;//MENDAPATKAN TIPE TRANSAKSI
            awal_number = awal;//MENDAPATKAN AWALAN RUNNING NUMBER (TR, RO, MT, RT, DO)

            get_id_store();
            get_year_month();
            get_running_number();
            give_id();
        }
        //=================BERGUNA UNTUK MENGAMBIL CODE STORE===============
        public void get_id_store()
        {
            try
            {
                ckon.sqlCon().Open();
                String command = "SELECT * FROM store";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

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
            DevCode code = new DevCode();

            String device_code = "";
            device_code = code.aDevCode;

            String command = "";            

            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if(ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        //tahun_trans = ckon.myReader.GetString("Year");
            //        bulan_trans = ckon.myReader.GetString("Month");
            //        number_trans = ckon.myReader.GetInt32("Number");
            //    }
            //    if(bulan_now == bulan_trans)
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
            //        if (Properties.Settings.Default.DevCode != "null" && type_trans == "7")
            //        {
            //            final_running_number = awal_number+"/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
            //        }
            //        else
            //        {
            //            final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
            //        }
            //        //final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
            //        //============UPDATE KE TABEL AUTO_NUMBER================
            //    }
            //    else
            //    {
            //        number_trans = 1;
            //        bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
            //        //==MEMBUAT STRING FINAL RUNNING NUMBER
            //        //final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //        if (Properties.Settings.Default.DevCode != "null" && type_trans == "7")
            //        {
            //            final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001-"+ device_code;
            //        }
            //        else
            //        {
            //            final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //        }
            //    }


            //}
            //else
            //{
            //    String query = "";
            //    number_trans = 1;
            //    bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
            //    if (Properties.Settings.Default.DevCode != "null" && type_trans == "7")
            //    {
            //        final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001-" + device_code;
            //        query = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans,Dev_Code) VALUES ('" + store_code + "','" + bulan_trans + "','0','"+type_trans+"','" + Properties.Settings.Default.DevCode + "')";
            //    }
            //    else
            //    {
            //        final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
            //        query = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','" + type_trans + "')";
            //    }
            //    //final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";

            //    //String query = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','" + type_trans + "')";
            //    CRUD ubah = new CRUD();
            //    ubah.ExecuteNonQuery(query);

            //    //MessageBox.Show(final_running_number);
            //}
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                if (Properties.Settings.Default.DevCode != "null" && type_trans == "7")
                {
                    command = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '7' AND Dev_Code='" + device_code + "'";
                }
                else
                {
                    command = "SELECT * FROM auto_number WHERE Store_Code = '" + store_code + "' AND Type_Trans = '" + type_trans + "'";
                }
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

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
                        if (Properties.Settings.Default.DevCode != "null" && type_trans == "7")
                        {
                            final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string + "-" + Properties.Settings.Default.DevCode;
                        }
                        else
                        {
                            final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
                        }
                        //final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-" + number_trans_string;
                        //============UPDATE KE TABEL AUTO_NUMBER================
                    }
                    else
                    {
                        number_trans = 1;
                        bulan_trans = bulan_now;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
                                                //==MEMBUAT STRING FINAL RUNNING NUMBER
                                                //final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                        if (Properties.Settings.Default.DevCode != "null" && type_trans == "7")
                        {
                            final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001-" + device_code;
                        }
                        else
                        {
                            final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                        }
                    }
                }
                else
                {                    
                    number_trans = 1;
                    bulan_trans = bulan_now;//BULAN TRANSAKSI = BULAN SEKARANG
                    if (Properties.Settings.Default.DevCode != "null" && type_trans == "7")
                    {
                        final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001-" + device_code;
                        command = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans,Dev_Code) VALUES ('" + store_code + "','" + bulan_trans + "','0','" + type_trans + "','" + Properties.Settings.Default.DevCode + "')";
                    }
                    else
                    {
                        final_running_number = awal_number + "/" + store_code + "-" + tahun_now + "" + bulan_trans + "-00001";
                        command = "INSERT INTO auto_number (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans + "','0','" + type_trans + "')";
                    }                    
                    
                    CRUD ubah = new CRUD();
                    ubah.ExecuteNonQuery(command);                    
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
        //=======METHOD DIGUNAKAN UNTUK MEMBERI RUNNING NUMBER KE HALAMAN TRANSAKSI SESUAI TYPE TRANSAKSI==
        public void give_id()
        {
            if (type_trans == "7")
            {
                UC_Popup_Info.Instance.get_running_number_shift(final_running_number, bulan_now, number_trans, type_trans);                
            }
            else if (type_trans == "8")
            {
                UC_Popup_Info.Instance.get_running_number_CloseStore(final_running_number, bulan_now, number_trans, type_trans);                
            }            
        }
    }
}
