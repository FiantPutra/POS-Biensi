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
    public partial class UC_MO_List : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        int amount;
        String id_trans, article_id, jam, id_list, qty2, date2, mutasi_from;


        //======================================================
        private static UC_MO_List _instance;

        public static UC_MO_List Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_MO_List(f1);
                return _instance;
            }
        }


        //=======================================================
        public UC_MO_List(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        //================================TEXTBOX SEARCH===================================================
        private void t_search_trans_OnTextChange(object sender, EventArgs e)
        {
            if (t_search_trans.text == "")
            {
                String sql = "SELECT * FROM mutasiorder WHERE  STATUS='1' AND DATE='" + tanggal_MO.Text + "'";
                holding(sql);
            }
            else
            {
                String sql = "SELECT * FROM mutasiorder WHERE  STATUS='1' AND MUTASI_ORDER_ID LIKE '%" + t_search_trans.text + "%' AND DATE='" + tanggal_MO.Text + "'";
                holding(sql);
            }
        }
        //=====================================================================================================


        //======================LIST HOLD TRANSACTION============================================
        public void holding(String query)
        {
            CRUD sql = new CRUD();
            dgv_hold.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["MUTASI_ORDER_ID"].ToString();
                        jam = ckon.sqlDataRd["TIME"].ToString();

                        int st_api = Convert.ToInt32(ckon.sqlDataRd["STATUS_API"].ToString());
                        String tr_date = ckon.sqlDataRd["DATE"].ToString();

                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_trans;
                        dgv_hold.Rows[dgRows].Cells[1].Value = tr_date + " " + jam;
                        if (st_api == 1)
                            dgv_hold.Rows[dgRows].Cells[2].Value = "Confirmed";
                        else
                            dgv_hold.Rows[dgRows].Cells[2].Value = "Unconfirmed";
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

        //=======================================================================================

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
                        mutasi_from = ckon.sqlDataRd["MUTASI_FROM_WAREHOUSE"].ToString();
                        date2 = ckon.sqlDataRd["REQUEST_DELIVERY_DATE"].ToString();
                        qty2 = ckon.sqlDataRd["TOTAL_QTY"].ToString();
                        l_to.Text = ckon.sqlDataRd["MUTASI_TO_WAREHOUSE"].ToString();
                        l_no_sj.Text = ckon.sqlDataRd["NO_SJ"].ToString();
                        amount = Convert.ToInt32(ckon.sqlDataRd["TOTAL_AMOUNT"].ToString());
                    }
                }

                l_date.Text = date2;
                l_qty.Text = qty2;
                amount_txt.Text = string.Format("{0:#,###}" + ",00", amount);
                l_mutasi.Text = mutasi_from;
                from_txt.Text = GetStore(mutasi_from);
                to_text.Text = GetStore(l_to.Text);
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
        //============================================================================================

        //wtu
        private string GetStore(string code)
        {
            CRUD sql = new CRUD();

            String store = "-";
            if(code == "" || code == "-")
            {
                return store;
            }

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT NAME FROM store_relasi WHERE CODE='" + code + "' UNION SELECT NAME FROM store WHERE CODE='" + code + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        store = ckon.sqlDataRd["NAME"].ToString();
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

            return store;
        }


        //=====================ITEM MUTASI  ORDER ==========================================
        public void retreive(string sort = "", string filter = "", string search = "")
        {
            CRUD sql = new CRUD();
            dgv_request.Rows.Clear();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT  mutasiorder_line.ARTICLE_ID ,mutasiorder_line.QUANTITY, mutasiorder_line.UNIT, article.ARTICLE_NAME, itemdimensionsize.Description as SIZE_ID, itemdimensioncolor.Description as COLOR_ID, article.PRICE FROM mutasiorder_line, article, itemdimensioncolor, itemdimensionsize WHERE article.ARTICLE_ID = mutasiorder_line. ARTICLE_ID AND itemdimensioncolor.Id = article.COLOR_ID AND itemdimensionsize.Id = article.SIZE_ID AND mutasiorder_line.MUTASI_ORDER_ID='" + l_transaksi.Text + "' ";
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
                    dgv_request.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_request.Rows[dgRows].Cells[2].Value = row["SIZE_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[3].Value = row["COLOR_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[4].Value = row["PRICE"];
                    dgv_request.Rows[dgRows].Cells[5].Value = row["QUANTITY"].ToString();
                    dgv_request.Rows[dgRows].Cells[6].Value = row["UNIT"];
                }
                dgv_request.Columns[4].DefaultCellStyle.Format = "#,###";
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


        //===================== KLIK HOLD MASUK KE KANAN===========================
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
        //========================================================================

        //======================================================================

        //===================SELECT DATE =======================================
        private void tanggal_MO_ValueChanged(object sender, EventArgs e)
        {
            String SQL = "SELECT * FROM mutasiorder WHERE  STATUS='1' AND DATE='" + tanggal_MO.Text + "'";
            holding(SQL);
        }

        //======================================================================
        private void b_back_PC_Click(object sender, EventArgs e)
        {

            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_Mut_order.Instance))
            {
                f1.p_kanan.Controls.Add(UC_Mut_order.Instance);
                UC_Mut_order.Instance.Dock = DockStyle.Fill;
                UC_Mut_order.Instance.BringToFront();
            }
            else
                UC_Mut_order.Instance.BringToFront();
        }
        //===========================================================================
    }
}
