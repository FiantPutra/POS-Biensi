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
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace try_bi
{
    class BackDateRunningNumber
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();

        String type_trans, awal_number, store_code, BackDate, bulan_BackDate, tahun_BackDate, bulan_trans_backdate, number_trans_string, final_running_number;
        int number_trans_backdate;
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
            string command;                                    

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM store";
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
            BackDate = Properties.Settings.Default.ValueBackDate;

            bulan_BackDate = BackDate.Substring(5,2);
            tahun_BackDate = BackDate.Substring(2,2);
        }
        //=========METHOD GET DATA FROM AUTO_NUMBER TABLE FOR SALES TRANSACTION
        public void get_running_number()
        {
            string command;                       

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM auto_number_backdate WHERE Store_Code = '" + store_code + "' AND Type_Trans = '" + type_trans + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        bulan_trans_backdate = ckon.sqlDataRd["Month"].ToString();
                        number_trans_backdate = Convert.ToInt32(ckon.sqlDataRd["Number"].ToString());
                    }

                    if (bulan_BackDate == bulan_trans_backdate)
                    {
                        number_trans_backdate = number_trans_backdate + 1;
                        if (number_trans_backdate < 10)
                        { 
                            number_trans_string = "0000" + number_trans_backdate.ToString(); 
                        }
                        else if (number_trans_backdate < 100)
                        { 
                            number_trans_string = "000" + number_trans_backdate.ToString(); 
                        }
                        else if (number_trans_backdate < 1000)
                        { 
                            number_trans_string = "00" + number_trans_backdate.ToString(); 
                        }
                        else if (number_trans_backdate < 10000)
                        { 
                            number_trans_string = "0" + number_trans_backdate.ToString(); 
                        }
                        else
                        { 
                            number_trans_string = number_trans_backdate.ToString(); 
                        }
                        //==MEMBUAT STRING FINAL RUNNING NUMBER
                        final_running_number = awal_number + "/" + store_code + "-" + tahun_BackDate + "" + bulan_BackDate + "-" + number_trans_string;                        
                    }
                    else
                    {
                        number_trans_backdate = 1;
                        bulan_trans_backdate = bulan_BackDate;//MENJADIKAN BULAN TRANSAKSI = BULAN SEKARANG
                                                              //==MEMBUAT STRING FINAL RUNNING NUMBER
                        final_running_number = awal_number + "/" + store_code + "-" + tahun_BackDate + "" + bulan_trans_backdate + "-00001";
                    }
                }
                else
                {
                    number_trans_backdate = 1;
                    bulan_trans_backdate = bulan_BackDate;//BULAN TRANSAKSI = BULAN SEKARANG
                    final_running_number = awal_number + "/" + store_code + "-" + tahun_BackDate + "" + bulan_trans_backdate + "-00001";                   
                    
                    command = "INSERT INTO auto_number_backdate (Store_Code,Month,Number,Type_Trans) VALUES ('" + store_code + "','" + bulan_trans_backdate + "','0','" + type_trans + "')";
                    CRUD sqlInsert = new CRUD();
                    sqlInsert.ExecuteNonQuery(command);
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
                UC_Popup_Info.Instance.get_running_number_shift(final_running_number, bulan_trans_backdate, number_trans_backdate, type_trans);
            }
            if (type_trans == "8")
            {
                UC_Popup_Info.Instance.get_running_number_CloseStore(final_running_number, bulan_trans_backdate, number_trans_backdate, type_trans);
            }
        }
    }
}
