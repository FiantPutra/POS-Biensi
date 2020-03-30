 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;


namespace try_bi
{
    public partial class w_edc_closing_shift : Form
    {
        koneksi ckon = new koneksi();
        String nm_bank, date, nm_bank2, id_bank;
        int total_amount, total_edc2, fix_total;
        public string shift_code, id_shift2;


        DateTime mydate = DateTime.Now;
        public w_edc_closing_shift()
        {
            InitializeComponent();
        }
        public void total_bank(String tanggal)
        {
            CRUD sql = new CRUD();

            dgv_bank.Rows.Clear();
            String date = tanggal;
            try
            {
                ckon.sqlCon().Open();
                String cmd_bank = "SELECT * FROM bank";
                ckon.sqlDataRdHeader = sql.ExecuteDataReader(cmd_bank, ckon.sqlCon());

                if (ckon.sqlDataRdHeader.HasRows)
                {
                    while (ckon.sqlDataRdHeader.Read())
                    {
                        id_bank = ckon.sqlDataRdHeader["BANK_ID"].ToString();
                        nm_bank = ckon.sqlDataRdHeader["BANK_NAME"].ToString();

                        String cmd_EDC1 = "SELECT SUM([transaction].EDC) as total FROM [transaction] WHERE BANK_NAME='" + id_bank + "'AND ID_SHIFT='" + id_shift2 + "' AND (STATUS='1' or STATUS='2')";
                        ckon.sqlDataRdLine = sql.ExecuteDataReader(cmd_EDC1, ckon.sqlCon());

                        if (ckon.sqlDataRdLine.HasRows)
                        {
                            while (ckon.sqlDataRdLine.Read())
                            {
                                total_amount = Convert.ToInt32(ckon.sqlDataRdLine["total"].ToString());
                            }
                        }
                        else
                        {
                            total_amount = 0;
                        }

                        String cmd_EDC2 = "SELECT SUM([transaction].EDC2) as total FROM [transaction] WHERE BANK_NAME2='" + id_bank + "' AND ID_SHIFT='" + id_shift2 + "' AND (STATUS='1' or STATUS='2')";
                        ckon.sqlDataRdLine = sql.ExecuteDataReader(cmd_EDC1, ckon.sqlCon());

                        if (ckon.sqlDataRdLine.HasRows)
                        {
                            while (ckon.sqlDataRdLine.Read())
                            {
                                total_edc2 = Convert.ToInt32(ckon.sqlDataRdLine["total"].ToString());
                            }
                        }
                        else
                        {
                            total_edc2 = 0;
                        }

                        fix_total = total_amount + total_edc2;
                        int dgRows = dgv_bank.Rows.Add();
                        dgv_bank.Rows[dgRows].Cells[0].Value = nm_bank;
                        dgv_bank.Rows[dgRows].Cells[1].Value = fix_total;
                        dgv_bank.Columns[1].DefaultCellStyle.Format = "#,###";
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("No connection to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRdHeader != null)
                    ckon.sqlDataRdHeader.Close();

                if (ckon.sqlDataRdLine != null)
                    ckon.sqlDataRdLine.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }

            //koneksi2 ckon2 = new koneksi2();
            //ckon.con.Close();
            //String sql = "SELECT * FROM bank";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_bank = ckon.myReader.GetString("BANK_ID");
            //        nm_bank = ckon.myReader.GetString("BANK_NAME");
            //        String sql2 = "SELECT SUM(transaction.EDC) as total FROM transaction WHERE BANK_NAME='" + id_bank + "'AND ID_SHIFT='" + id_shift2 + "' AND (STATUS='1' or STATUS='2') ";
            //        ckon2.cmd2 = new MySqlCommand(sql2, ckon2.con2);
            //        ckon2.con2.Open();
            //        ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //        while (ckon2.myReader2.Read())
            //        {
            //            try
            //            {
            //                total_amount = ckon2.myReader2.GetInt32("total");
            //            }
            //            catch
            //            {
            //                total_amount = 0;
            //            }
            //        }
            //        ckon2.con2.Close();
            //        //=====================================================================================================
            //        String sql3 = "SELECT SUM(transaction.EDC2) as total FROM transaction WHERE BANK_NAME2='" + id_bank + "' AND ID_SHIFT='" + id_shift2 + "' AND (STATUS='1' or STATUS='2')";
            //        ckon2.cmd2 = new MySqlCommand(sql3, ckon2.con2);
            //        ckon2.con2.Open();
            //        ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //        while (ckon2.myReader2.Read())
            //        {
            //            try
            //            {
            //                total_edc2 = ckon2.myReader2.GetInt32("total");
            //            }
            //            catch
            //            {
            //                total_edc2 = 0;
            //            }
            //        }
            //        fix_total = total_amount + total_edc2;
            //        int n = dgv_bank.Rows.Add();
            //        dgv_bank.Rows[n].Cells[0].Value = nm_bank;
            //        dgv_bank.Rows[n].Cells[1].Value = fix_total;
            //        dgv_bank.Columns[1].DefaultCellStyle.Format = "#,###";
            //        ckon2.con2.Close();
            //    }
            //}
            //ckon.con.Close();
        }

        //======================================================================================================================
        private void w_edc_closing_shift_Load(object sender, EventArgs e)
        {
            date = mydate.ToString("yyyy-MM-dd");
            total_bank(date);
        }
        //===============================================================================================================
        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //=====================================================================================================
    }
}
