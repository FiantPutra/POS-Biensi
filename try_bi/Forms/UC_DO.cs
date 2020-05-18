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

namespace try_bi
{
    public partial class UC_DO : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        DateTime mydate = DateTime.Now;
        String id_do, article_name, id_list, epy_id, epy_name, date, total_qty, sj_fisik;
        //======================================================
        private static UC_DO _instance;
        public static UC_DO Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_DO(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_DO(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //memberi warna orange pada kolom ke 6
        public void color_dgv()
        {
            
            dgv_inv.Columns[7].HeaderCell.Style.ForeColor = Color.Orange;
        }
        //-----melihat DO yang telah di konfirmasi, do list tampilkan sesuai tanggal hari ini, 
        private void b_do_list_Click(object sender, EventArgs e)
        {
            //date = mydate.ToString("yyyy-MM-dd");
            String sql = "SELECT deliveryorder.DELIVERY_ORDER_ID, deliveryorder.TOTAL_QTY, deliveryorder.SJ_FISIK, deliveryorder.STATUS_API FROM deliveryorder "                            
                            + "WHERE STATUS = 'Confirmed'";

            UC_DO_List c = new UC_DO_List(f1);
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_DO_List.Instance))
            {
                f1.p_kanan.Controls.Add(UC_DO_List.Instance);
                UC_DO_List.Instance.Dock = DockStyle.Fill;
                UC_DO_List.Instance.holding(sql);
                UC_DO_List.Instance.color_dgv();
                UC_DO_List.Instance.Show();
            }
            else
            {
                UC_DO_List.Instance.holding(sql);
                UC_DO_List.Instance.clearRetrive();
                UC_DO_List.Instance.color_dgv();
                UC_DO_List.Instance.Show();
            }                
        }
        //===================GET EMPLOYE ID AND EMPLOYEE NAME dari form 1==============
        public void set_name(String id, String name)
        {
            epy_id = id;
            epy_name = name;
        }


        //======================LIST HOLD TRANSACTION============================================
        public void holding(String cmd)
        {
            CRUD sql = new CRUD();
            List<string> numbersList = new List<string>();

            dgv_hold.Rows.Clear();
            dgv_inv.Rows.Clear();
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
                        
                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_do;
                        dgv_hold.Rows[dgRows].Cells[1].Value = sj_fisik;
                        dgv_hold.Rows[dgRows].Cells[2].Value = total_qty;
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
        //=============================================================================================

        //==================================KLIK HOLD TABEL, GET DATA AND RETREIVE DATA===============
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    retreive2(id_list);
                    l_transaksi.Text = id_list;
                    get_data(id_list);
                }
            }
            catch (Exception)
            {

            }
        }
        //============================================================================================

        //============================GET DATA FROM ID, untuk ditampilkan detail dari do header=======================
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

                        if (ckon.sqlDataRd["TOTAL_AMOUNT"].ToString() != "")
                        {
                            tot_amount = Convert.ToInt32(ckon.sqlDataRd["TOTAL_AMOUNT"]);
                            l_amount.Text = string.Format("{0:#,###}" + ",00", tot_amount);
                        }
                        else
                        {
                            l_amount.Text = "0,00";
                        }

                        if (Convert.ToInt32(ckon.sqlDataRd["IS_PBM_PBK"].ToString()) == 1)
                        {
                            b_confirm.Enabled = false;
                        }
                        else
                        {
                            b_confirm.Enabled = true;
                        }
                    }
                }
                else
                {
                    tot_amount = 0;
                    l_amount.Text = "0,00";
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

        private void T_search_trans_KeyPress(object sender, EventArgs e)
        {
            try
            {
                //if (e.KeyChar == (char)Keys.Enter)
                //{
                //    t_search_trans_OnTextChange(null, null);
                //}
            }
            catch (Exception) { }
        }

        private void Clear_filter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception) { }
        }

        //=========menampilkan do line dengan detail dari article======================================== 
        public void retreive2(String id_do_new)
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            dgv_inv.Rows.Clear();
            
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT deliveryorder_line._id, deliveryorder_line.QTY_DELIVER, deliveryorder_line.QTY_RECEIVE, deliveryorder_line.QTY_DISPUTE, deliveryorder_line.PACKING_NUMBER ,article.ARTICLE_ID,article.ARTICLE_NAME, article.SIZE_ID, article.COLOR_ID FROM deliveryorder_line, article WHERE deliveryorder_line.ARTICLE_ID = article.ARTICLE_ID AND deliveryorder_line.DELIVERY_ORDER_ID='" + id_do_new + "'";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_inv.Rows.Add();
                    dgv_inv.Rows[dgRows].Cells[0].Value = row["_id"].ToString();
                    dgv_inv.Rows[dgRows].Cells[1].Value = row["ARTICLE_ID"];
                    dgv_inv.Rows[dgRows].Cells[2].Value = row["ARTICLE_NAME"].ToString();
                    dgv_inv.Rows[dgRows].Cells[3].Value = row["PACKING_NUMBER"];
                    dgv_inv.Rows[dgRows].Cells[4].Value = row["SIZE_ID"].ToString();
                    dgv_inv.Rows[dgRows].Cells[5].Value = row["COLOR_ID"].ToString();
                    dgv_inv.Rows[dgRows].Cells[6].Value = row["QTY_DELIVER"];
                    dgv_inv.Rows[dgRows].Cells[7].Value = row["QTY_RECEIVE"];
                    dgv_inv.Rows[dgRows].Cells[8].Value = row["QTY_DISPUTE"];
                }
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
        //============================================================================================

        //====================================KLIK AMOUNT IN DO LINE AND OPEN POP UP WINDOWS========
        private void dgv_inv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_inv.Columns[e.ColumnIndex].Name == "Column6")
            {
                String id = dgv_inv.Rows[e.RowIndex].Cells["Column8"].Value.ToString();
                String name = dgv_inv.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
                String good = dgv_inv.Rows[e.RowIndex].Cells["Column3"].Value.ToString();
                String reject = dgv_inv.Rows[e.RowIndex].Cells["Column6"].Value.ToString();
               
                W_EditQty_DO edit_do = new W_EditQty_DO(f1);                
                edit_do.get_data(id, name, good, reject, id_list);
                edit_do.ShowDialog();                
            }
        }

        //====================================================================================
        private void tanggal_req_ValueChanged(object sender, EventArgs e)
        {
            clearRetrive();
            String sql = "SELECT * FROM deliveryorder "
                            + "WHERE STATUS = 'Pending' AND DELIVERY_DATE = '" + tanggal_DO.Text + "'";
            holding(sql);
        }
        //================================================================================================
        private void t_search_trans_OnTextChange(object sender, EventArgs e)
        {
            clearRetrive();
            if (t_search_trans.text == "")
            {
                String sql = "SELECT * FROM deliveryorder "
                            + "WHERE STATUS = 'Pending' AND DELIVERY_DATE = '" + tanggal_DO.Text + "'";
                holding(sql);
            }
            else
            {
                String sql = "SELECT * FROM deliveryorder "
                            + "WHERE STATUS = 'Pending' AND DELIVERY_DATE = '" + tanggal_DO.Text + "' AND (DELIVERY_ORDER_ID LIKE '%" + t_search_trans.text + "%' OR SJ_FISIK LIKE '%" + t_search_trans.text + "%')";
                holding(sql);
            }
        }

        //==========================================================================================

        private void b_confirm_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            if (l_transaksi.Text == "-")
            {                
                MessageBox.Show("Please choose delivery order number first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
               /*DILAKUKAN PENGECEKAN APAKAH LINE DO YANG TAMPIL SUDAH SESUAI DENGAN SURAT JALAN DARI HO*/
                int total_sj=0, total_line=0;
                
                try
                {
                    ckon.sqlCon().Open();
                    String cmd_totalsj = "SELECT COUNT(*) as total_sj FROM deliveryorder_line where delivery_order_id = '" + l_transaksi.Text + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_totalsj, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            total_sj = Convert.ToInt32(ckon.sqlDataRd["total_sj"].ToString());
                        }
                    }

                    String cmd_totalLine = "SELECT COUNT(*) as total_line FROM deliveryorder_line, article WHERE deliveryorder_line.ARTICLE_ID = article.ARTICLE_ID AND deliveryorder_line.DELIVERY_ORDER_ID='" + l_transaksi.Text + "';";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_totalLine, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            total_line = Convert.ToInt32(ckon.sqlDataRd["total_line"].ToString());
                        }
                    }

                    if (total_line == total_sj)
                    {
                        W_DO_Confirm confirm = new W_DO_Confirm();
                        confirm.simpan_do_header(l_transaksi.Text, epy_id, epy_name);
                        confirm.store();                        
                        confirm.retreive();
                        confirm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Please get data Article first form sync stroe before confirm this DO", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        //================================================================================================
        //====================PUBLIC RESET===============================================
        public void reset()
        {
            l_transaksi.Text = "-";
            l_dev_date.Text = "-";
            sj_fisik_text.Text = "-";
            qty_rcv.Text = "0";
            l_qty.Text = "0";
            l_amount.Text = "0";
            tanggal_DO.Value = System.DateTime.Now;
            dgv_inv.Rows.Clear();
            String cmd = "SELECT * FROM deliveryorder "
                            + "WHERE STATUS = 'Pending' AND DATE = '" + tanggal_DO.Text + "'";
            holding(cmd);
        }

        public void clearRetrive()
        {
            l_transaksi.Text = "-";
            l_dev_date.Text = "-";
            sj_fisik_text.Text = "-";
            qty_rcv.Text = "0";
            l_qty.Text = "0";
            l_amount.Text = "0";
            dgv_inv.Rows.Clear();
        }

        //===============================================================================

    }
}
