using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PrinterUtility;
using System.Collections;
using System.IO;
using MySql.Data.MySqlClient;

namespace try_bi
{
    public partial class W_Vou_Confirm : Form
    {
        String voucher_code, desc;
        int value, tot_diskon, tot_bel, id_disc;
        public String id_transaksi3, voucher_code2;
        koneksi ckon = new koneksi();
        public W_Vou_Confirm()
        {
            InitializeComponent();
        }

        private void W_Vou_Confirm_Load(object sender, EventArgs e)
        {
            //get_data_voucher();
            ambil_discount();
            count_total_TransLine();
        }
        private void b_ok2_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();
            //validasi agar total belanja tidak boleh dibawah Rp.0
            tot_bel = tot_bel - (value + tot_diskon);
            if (tot_bel < 0)
            {
                MessageBox.Show("Voucher Can't Be Apllied");
            }
            else
            {
                int value_voucher_exist;
                //ckon.con.Close();
                try
                {
                    ckon.sqlCon().Open();
                    String cmd = "SELECT * FROM [transaction] WHERE TRANSACTION_ID = '" + id_transaksi3 + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            value_voucher_exist = Convert.ToInt32(ckon.sqlDataRd["VOUCHER"].ToString());
                            if (value_voucher_exist == 0)
                            {
                                update();
                            }
                            else
                            {
                                MessageBox.Show("Voucher Has Been Used");
                            }
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

                //ckon.con.Open();
                //ckon.cmd = new MySqlCommand(sql, ckon.con);
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    value_voucher_exist = ckon.myReader.GetInt32("VOUCHER");
                //    if (value_voucher_exist == 0)
                //    {
                //        update();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Voucher Has Been Used");
                //    }
                //}
                //ckon.con.Close();

                //update();
            }
            //update();
        }
        private void b_cancel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //===ambil data dari form voucher jika valid====
        public void get_voucher_valid(String v_code, String  v_desc, int v_value, int id_diskon )
        {
            voucher_code = v_code;
            desc = v_desc;
            value = v_value;
            id_disc = id_diskon;
            //=====masukan ke dalam text
            l_voucher.Text = voucher_code;
            l_desc.Text = desc;
            l_value.Text = string.Format("{0:#,###}" + ",00", value);
        }
        //======AMBIL TOTAL DISCOUNT DARI TRANSAKSI HEADER SESUAI TERANSAKSI ID===
        public void ambil_discount()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM [transaction] WHERE TRANSACTION_ID='" + id_transaksi3 + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        tot_diskon = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
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
            //while(ckon.myReader.Read())
            //{
            //    tot_diskon = ckon.myReader.GetInt32("DISCOUNT");
            //}
            //ckon.con.Close();
        }
        //=============HITUNG TOTAL BELANJA DARI TRANSAKSI LINE===
        public void count_total_TransLine()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT SUM(transaction_line.SUBTOTAL) as total FROM transaction_line WHERE TRANSACTION_ID='" + id_transaksi3 + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        tot_bel = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
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
            //    tot_bel = ckon.myReader.GetInt32("total");
            //}
            //ckon.con.Close();
        }
        //=================UPDATE VALUE VOUCHER DI TRANSAKSI LINE YG DITUJU======
        public void update()
        {
            String cmd_update = "UPDATE [transaction] SET VOUCHER='" + value + "', VOUCHER_ID='"+ id_disc +"', VOUCHER_CODE='"+ voucher_code +"' WHERE TRANSACTION_ID='" + id_transaksi3 + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
            uc_coba.Instance.itung_total();
            this.Close();
        }
        //shorcut di menu confirm voucher
        private void W_Vou_Confirm_KeyDown(object sender, KeyEventArgs e)
        {
            //==============BUTTON CHARGE============
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_ok2_Click(null, null);
            }
            //==========BUTTON BACK=================
            if (e.Control && e.KeyCode.ToString() == "B")
            {
                b_cancel2_Click(null, null);
            }
        }
    }
}
