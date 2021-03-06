﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace try_bi
{
    public partial class UC_RO_list : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String id_trans, article_id, jam, id_list, qty2, date2;
        //======================================================
        private static UC_RO_list _instance;
        public static UC_RO_list Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_RO_list(f1);
                return _instance;
            }
        }

        //=======================================================
        public UC_RO_list(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }



        //======================LIST HOLD TRANSACTION============================================
        public void holding(String cmd)
        {
            CRUD sql = new CRUD();
            dgv_hold.Rows.Clear();
            //koneksi2 ckon2 = new koneksi2();
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["REQUEST_ORDER_ID"].ToString();
                        jam = ckon.sqlDataRd["TIME"].ToString();

                        int st_api = Convert.ToInt32(ckon.sqlDataRd["STATUS_API"].ToString());
                        String tr_date = ckon.sqlDataRd["DATE"].ToString();

                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_trans;                        
                        dgv_hold.Rows[dgRows].Cells[1].Value = tr_date + " " + jam; ;
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

            //date = mydate.ToString("yyyy-MM-dd");
            //String cmd = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //List<string> numbersList = new List<string>();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_trans = ckon.myReader.GetString("REQUEST_ORDER_ID");
            //        jam = ckon.myReader.GetString("TIME");
            //        //String sql2 = "SELECT article.ARTICLE_NAME FROM requestorder_line, article  WHERE article.ARTICLE_ID = requestorder_line.ARTICLE_ID AND requestorder_line.REQUEST_ORDER_ID='" + id_trans + "'";
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
            //        dgv_hold.Rows[n].Cells[1].Value = tr_date + " " + jam; ;
            //        dgv_hold.Rows[n].Cells[2].Value = st_api.ToString(); 
            //        ckon2.con2.Close();
            //    }

            //}
            //ckon.con.Close();
        }



        //=============================================================================================

        //=====================KLIK HOLD MASUK KE KANAN================================================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    l_transaksi.Text = id_list;
                    get_data();
                    retreive();
                }
            }
            catch (Exception)
            {

            }
        }

        private void dgv_request_FilterStringChanged(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        private void dev_request_Sorting(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        //============================================================================================

        //====================GET DATA FROM ID =======================================================
        public void get_data()
        {
            CRUD sql = new CRUD();
            int amount;
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM requestorder WHERE REQUEST_ORDER_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        date2 = ckon.sqlDataRd["DATE"].ToString();
                        qty2 = ckon.sqlDataRd["TOTAL_QTY"].ToString();
                        t_desc.Text = ckon.sqlDataRd["DESCRIPTION"].ToString();
                        amount = Convert.ToInt32(ckon.sqlDataRd["TOTAL_AMOUNT"].ToString());
                        l_amount.Text = string.Format("{0:#,###}" + ",00", amount);
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
            //        date2 = ckon.myReader.GetString("DATE");
            //        qty2 = ckon.myReader.GetString("TOTAL_QTY");
            //        t_desc.Text = ckon.myReader.GetString("DESCRIPTION");
            //        amount = ckon.myReader.GetInt32("TOTAL_AMOUNT");
            //        l_amount.Text = string.Format("{0:#,###}" + ",00", amount);
            //        no_sj.Text = ckon.myReader.GetString("NO_SJ");
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }
            l_date.Text = date2;
            l_quantity.Text = qty2;
        }
        //============================================================================================
        //=====================ITEM REQUEST ORDER ==========================================
        public void retreive(string sort = "", string filter = "", string search = "")
        {
            CRUD sql = new CRUD();
            dgv_request.Rows.Clear();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT  requestorder_line.ARTICLE_ID ,requestorder_line.QUANTITY, requestorder_line.UNIT, article.ARTICLE_NAME, article.SIZE, article.COLOR, article.PRICE FROM requestorder_line, article  WHERE article.ARTICLE_ID = requestorder_line. ARTICLE_ID AND requestorder_line.REQUEST_ORDER_ID='" + l_transaksi.Text + "' ";
                if (filter != "")
                {
                    filter = filter.Replace("ARTICLE_ID", "requestorder_line.ARTICLE_ID");
                    cmd += " AND " + filter;
                }
                if (sort != "")
                {
                    cmd += " ORDER BY " + sort;
                }
                else
                {
                    cmd += " ORDER BY requestorder_line._id DESC";
                }
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_request.Rows.Add();
                    dgv_request.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_request.Rows[dgRows].Cells[2].Value = row["SIZE"].ToString();
                    dgv_request.Rows[dgRows].Cells[3].Value = row["COLOR"].ToString();
                    dgv_request.Rows[dgRows].Cells[4].Value = row["PRICE"];
                    dgv_request.Rows[dgRows].Cells[5].Value = row["QUANTITY"].ToString();
                    dgv_request.Rows[dgRows].Cells[6].Value = row["UNIT"];
                }
                dgv_request.Columns[4].DefaultCellStyle.Format = "#,###";
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);
            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_request.Rows.Add();
            //        dgv_request.Rows[n].Cells[0].Value = row["ARTICLE_ID"].ToString();
            //        dgv_request.Rows[n].Cells[1].Value = row["ARTICLE_NAME"].ToString();
            //        dgv_request.Rows[n].Cells[2].Value = row["SIZE"].ToString();
            //        dgv_request.Rows[n].Cells[3].Value = row["COLOR"].ToString();
            //        dgv_request.Rows[n].Cells[4].Value = row["PRICE"];
            //        dgv_request.Rows[n].Cells[5].Value = row["QUANTITY"].ToString();
            //        dgv_request.Rows[n].Cells[6].Value = row["UNIT"];
            //    }
            //    dgv_request.Columns[4].DefaultCellStyle.Format = "#,###";

            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        //==================================================================================

        //=============BUTTON SEARCH ===============================
        private void t_search_trans_OnTextChange(object sender, EventArgs e)
        {
            if(t_search_trans.text == "")
            {
                String sql = "SELECT * FROM requestorder WHERE  STATUS='1' AND DATE='" + tanggal_RO.Text + "'";
                holding(sql);
            }
            else
            {
                String sql = "SELECT * FROM requestorder WHERE  STATUS='1' AND REQUEST_ORDER_ID LIKE '%"+ t_search_trans.text + "%' AND DATE='" + tanggal_RO.Text + "'";
                holding(sql);
            }
        }
        //=====================================================
        private void tanggal_RO_ValueChanged(object sender, EventArgs e)
        {
            String SQL = "SELECT * FROM requestorder WHERE  STATUS='1' AND DATE='" + tanggal_RO.Text + "'";
            holding(SQL);
        }
        //================================================

        private void b_back_PC_Click(object sender, EventArgs e)
        {
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_Req_order.Instance))
            {
                f1.p_kanan.Controls.Add(UC_Req_order.Instance);
                UC_Req_order.Instance.Dock = DockStyle.Fill;
                UC_Req_order.Instance.BringToFront();
            }
            else
                UC_Req_order.Instance.BringToFront();
        }
    }
}
