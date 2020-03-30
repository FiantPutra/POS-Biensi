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

namespace try_bi
{
    public partial class UC_DO_List : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        DateTime mydate = DateTime.Now;
        String id_do, article_name, id_list, total_qty, sj_fisik;
        //======================================================
        private static UC_DO_List _instance;
        public static UC_DO_List Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_DO_List(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_DO_List(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //=============memberi warna orange pada kolom dispute
        public void color_dgv()
        {
            dgv_inv.Columns[6].HeaderCell.Style.ForeColor = Color.Orange;
        }
        //================kembali ke halaman do confirmation
        private void b_back_DO_Click(object sender, EventArgs e)
        {
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_DO.Instance))
            {
                f1.p_kanan.Controls.Add(UC_DO.Instance);
                UC_DO.Instance.Dock = DockStyle.Fill;
                UC_DO.Instance.BringToFront();
            }
            else
                UC_DO.Instance.BringToFront();
        }
        //======================LIST HOLD TRANSACTION============================================
        public void holding(String cmd)
        {
            CRUD sql = new CRUD();            
            //ckon.con.Close();
            dgv_hold.Rows.Clear();

            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_do = ckon.sqlDataRd["DELIVERY_ORDER_ID"].ToString();
                        total_qty = ckon.sqlDataRd["TOTAL_QTY"].ToString();
                        sj_fisik = ckon.sqlDataRd["SJ_FISIK"].ToString();
                        int st_api = Convert.ToInt32(ckon.sqlDataRd["STATUS_API"].ToString());

                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_do;
                        dgv_hold.Rows[dgRows].Cells[1].Value = sj_fisik;
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

            //koneksi2 ckon2 = new koneksi2();
            //ckon.cmd = new MySqlCommand(query, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //List<string> numbersList = new List<string>();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_do = ckon.myReader.GetString("DELIVERY_ORDER_ID");
            //        total_qty = ckon.myReader.GetString("TOTAL_QTY");
            //        sj_fisik = ckon.myReader.GetString("SJ_FISIK");
            //        //String sql2 = "SELECT article.ARTICLE_NAME FROM deliveryorder_line, article  WHERE article._id = deliveryorder_line.ARTICLE_ID AND deliveryorder_line.DELIVERY_ORDER_ID='" + id_do + "'";
            //        //ckon2.cmd2 = new MySqlCommand(sql2, ckon2.con2);
            //        //ckon2.con2.Open();
            //        //ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //        //while (ckon2.myReader2.Read())
            //        //{
            //        //    article_name = ckon2.myReader2.GetString("ARTICLE_NAME");
            //        //    numbersList.Add(Convert.ToString(ckon2.myReader2["ARTICLE_NAME"]));
            //        //}
            //        //string[] numbersArray = numbersList.ToArray();
            //        //numbersList.Clear();
            //        //string result = String.Join(", ", numbersArray);
            //        int st_api = ckon.myReader.GetInt32("STATUS_API");

            //        int n = dgv_hold.Rows.Add();
            //        dgv_hold.Rows[n].Cells[0].Value = id_do;
            //        dgv_hold.Rows[n].Cells[1].Value = sj_fisik;
            //        dgv_hold.Rows[n].Cells[2].Value = st_api.ToString();
            //        ckon2.con2.Close();
            //    }
            //}
            //ckon.con.Close();
        }
        //=============================================================================================

        //===============================KLIK HOLD DO =================================================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    l_transaksi.Text = id_list;
                    retreive2(id_list);
                    get_data(id_list);
                }
            }
            catch (Exception)
            {

            }
        }
        //============================================================================================

        public void retreive2(String id_do_new)
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            dgv_inv.Rows.Clear();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT deliveryorder_line._id, deliveryorder_line.QTY_DELIVER, deliveryorder_line.QTY_RECEIVE, deliveryorder_line.QTY_DISPUTE,article.ARTICLE_ID,article.ARTICLE_NAME, article.SIZE, article.COLOR FROM deliveryorder_line, article WHERE deliveryorder_line.ARTICLE_ID = article._id AND deliveryorder_line.DELIVERY_ORDER_ID='" + id_do_new + "'";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_inv.Rows.Add();
                    dgv_inv.Rows[dgRows].Cells[0].Value = row["_id"].ToString();
                    dgv_inv.Rows[dgRows].Cells[1].Value = row["ARTICLE_ID"];
                    dgv_inv.Rows[dgRows].Cells[2].Value = row["ARTICLE_NAME"].ToString();
                    dgv_inv.Rows[dgRows].Cells[3].Value = row["SIZE"].ToString();
                    dgv_inv.Rows[dgRows].Cells[4].Value = row["COLOR"].ToString();
                    dgv_inv.Rows[dgRows].Cells[5].Value = row["QTY_DELIVER"];
                    dgv_inv.Rows[dgRows].Cells[6].Value = row["QTY_RECEIVE"];
                    dgv_inv.Rows[dgRows].Cells[7].Value = row["QTY_DISPUTE"];

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
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);
            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_inv.Rows.Add();
            //        dgv_inv.Rows[n].Cells[0].Value = row["_id"].ToString();
            //        dgv_inv.Rows[n].Cells[1].Value = row["ARTICLE_ID"];
            //        dgv_inv.Rows[n].Cells[2].Value = row["ARTICLE_NAME"].ToString();
            //        dgv_inv.Rows[n].Cells[3].Value = row["SIZE"].ToString();
            //        dgv_inv.Rows[n].Cells[4].Value = row["COLOR"].ToString();
            //        dgv_inv.Rows[n].Cells[5].Value = row["QTY_DELIVER"];
            //        dgv_inv.Rows[n].Cells[6].Value = row["QTY_RECEIVE"];
            //        dgv_inv.Rows[n].Cells[7].Value = row["QTY_DISPUTE"];

            //    }
            //    //dgv_inv.Columns[3].DefaultCellStyle.ForeColor = Color.Orange;
            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();

            //}
            //catch
            //{ }
        }

        private void clear_filter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception) { }
        }

        public void reset()
        {
            dgv_inv.Rows.Clear();
            l_transaksi.Text = "-";
            l_dev_date.Text = "-";
            l_qty.Text = "0";
            qty_rcv.Text = "0";
            sj_fisik_text.Text = "-";
            label4.Text = "0";
            tanggal_MO.Value = System.DateTime.Now;
            String sql2 = "SELECT * FROM deliveryorder WHERE STATUS='1'";
            holding(sql2);
        }

        public void clearRetrive()
        {
            dgv_inv.Rows.Clear();
            l_transaksi.Text = "-";
            l_dev_date.Text = "-";
            l_qty.Text = "0";
            qty_rcv.Text = "0";
            sj_fisik_text.Text = "-";
            label4.Text = "0";
            //tanggal_MO.Value = System.DateTime.Now;
            //DateTime aDate = DateTime.Now;
            //tanggal_MO.Value = aDate.ToString("yyyy-MM-dd");
        }
        //==================================TEXTBOXT SEARCH====================================================
        private void t_search_trans_OnTextChange(object sender, EventArgs e)
        {
            clearRetrive();
            if (t_search_trans.text == "")
            {
                String sql = "SELECT * FROM deliveryorder WHERE  STATUS='1' AND DATE='" + tanggal_MO.Text + "'";
                holding(sql);
            }
            else
            {
                String sql = "SELECT * FROM deliveryorder WHERE  STATUS='1' AND DATE='" + tanggal_MO.Text + "' AND (DELIVERY_ORDER_ID LIKE '%" + t_search_trans.text + "%' OR SJ_FISIK LIKE '%" + t_search_trans.text + "%')";
                //String sql = "SELECT * FROM deliveryorder WHERE  STATUS='1' AND DELIVERY_ORDER_ID LIKE '%" + t_search_trans.text + "%' AND DATE='" + tanggal_MO.Text + "'";
                holding(sql);
            }
        }
    

        //============================================================================================
        //============================GET DATA FROM ID================================================
        public void get_data(String id)
        {
            CRUD sql = new CRUD();
            int tot_amount;
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd_do = "SELECT * FROM deliveryorder WHERE DELIVERY_ORDER_ID = '" + id + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_do, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        l_dev_date.Text = ckon.sqlDataRd["DELIVERY_DATE"].ToString();
                        l_qty.Text = ckon.sqlDataRd["TOTAL_QTY"].ToString();
                        sj_fisik_text.Text = ckon.sqlDataRd["SJ_FISIK"].ToString();
                        tot_amount = Convert.ToInt32(ckon.sqlDataRd["TOTAL_AMOUNT"].ToString());
                        label4.Text = string.Format("{0:#,###}" + ",00", tot_amount);
                    }
                }

                String cmd_doReceive = "SELECT sum(QTY_RECEIVE) AS QTY FROM deliveryorder_line WHERE DELIVERY_ORDER_ID = '" + id + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_doReceive, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        qty_rcv.Text = ckon.sqlDataRd["QTY"].ToString();
                    }
                }
                else
                {
                    qty_rcv.Text = "0";
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
            //    l_dev_date.Text = ckon.myReader.GetString("DELIVERY_DATE");
            //    l_qty.Text = ckon.myReader.GetString("TOTAL_QTY");
            //    sj_fisik_text.Text = ckon.myReader.GetString("SJ_FISIK");
            //    tot_amount = ckon.myReader.GetInt32("TOTAL_AMOUNT");
            //    label4.Text = string.Format("{0:#,###}" + ",00", tot_amount);
            //}

            //ckon.con.Close();
            //String sql2 = "SELECT sum(QTY_RECEIVE) AS QTY FROM deliveryorder_line WHERE DELIVERY_ORDER_ID = '" + id + "'";
            //ckon.cmd = new MySqlCommand(sql2, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //try
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        qty_rcv.Text = ckon.myReader.GetString("QTY");
            //    }
            //}
            //catch
            //{
            //    qty_rcv.Text = "0";
            //}
            //ckon.con.Close();
        }
        //=============================================================================================
        private void tanggal_MO_ValueChanged(object sender, EventArgs e)
        {
            clearRetrive();
            String SQL = "SELECT * FROM deliveryorder WHERE  STATUS='1' AND DATE='" + tanggal_MO.Text + "'";
            holding(SQL);
        }
        //=============================================================================================
    }
}
