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
using System.Data.SqlClient;

namespace try_bi
{
    public partial class UC_DeliveryCustomer : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String id_trans, qty2, date, artIdLoc, id_inv;               
        //======================================================
        private static UC_DeliveryCustomer _instance;
        public static UC_DeliveryCustomer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_DeliveryCustomer(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_DeliveryCustomer(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }                     
        
        //=================================GENERATOR NUMBER=================================
        public void new_()
        {            
            dgv_delivCust.Rows.Clear();
            t_delivCustId.Text = "";
            t_qty.Text = "0";
            t_date.Text = "";
            no_Resi.Text = "";
            t_courier.Text = "";
        }
        //==================================================================================

        //======================== KLIK HOLD MASUK KERANJANG BELANJA=======================
        private void dgv_pending_MouseClick(object sender, MouseEventArgs e)
        {            
            if (dgv_Pending.Rows.Count > 0)
            {
                t_delivCustId.Text = id_trans;
                t_date.Text = date;
                holding();
                retreive();
                qty();               
            }            
        }

        private void dgv_delivCust_FilterStringChanged(object sender, EventArgs e)
        {
            var sort = dgv_delivCust.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_delivCust.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        private void dgv_delivCust_Sorting(object sender, EventArgs e)
        {
            var sort = dgv_delivCust.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_delivCust.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }                                                 
        
        //============BUTTON VIEW LIST DC ==================================================
        private void b_list_ro_Click(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM requestorder WHERE  STATUS= '1'";
            UC_RO_list c = new UC_RO_list(f1);
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_RO_list.Instance))
            {
                f1.p_kanan.Controls.Add(UC_RO_list.Instance);
                UC_RO_list.Instance.Dock = DockStyle.Fill;
                UC_RO_list.Instance.holding(sql);
                UC_RO_list.Instance.Show();
            }
            else
            {
                UC_RO_list.Instance.holding(sql);
            }
            UC_RO_list.Instance.Show();
        }
        //==================================================================================

        //=====================ITEM DELIVERY CUSTOMER ==========================================
        public void retreive(string sort = "", string filter = "", string search = "")
        {
            CRUD sql = new CRUD();

            dgv_delivCust.Rows.Clear();                        

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT  deliverycustomer_line.ARTICLE_ID, deliverycustomer_line.QTY, deliverycustomer_line.COURIER, deliverycustomer_line.DELIVERYADDRESS, deliverycustomer_line.DELIVERYTYPE, " +
                                "article.ARTICLE_NAME, article.PRICE FROM deliverycustomer_line, article  WHERE article.ARTICLE_ID = deliverycustomer_line. ARTICLE_ID AND deliverycustomer_line.DELIVERY_CUST_ID ='" + t_delivCustId.Text + "' ";

                if (filter != "")
                {
                    filter = filter.Replace("ARTICLE_ID", "deliverycustomer_line.ARTICLE_ID");
                    cmd += " AND " + filter;
                }
                if (sort != "")
                {
                    cmd += " ORDER BY " + sort;
                }
                else
                {
                    cmd += " ORDER BY deliverycustomer_line._id DESC";
                }
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());
                
                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_delivCust.Rows.Add();
                    dgv_delivCust.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_delivCust.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_delivCust.Rows[dgRows].Cells[2].Value = row["PRICE"].ToString();
                    dgv_delivCust.Rows[dgRows].Cells[3].Value = row["QTY"].ToString();
                    dgv_delivCust.Rows[dgRows].Cells[4].Value = row["DELIVERYADDRESS"];                    
                    dgv_delivCust.Rows[dgRows].Cells[5].Value = row["DELIVERYTYPE"];                    
                }
                dgv_delivCust.Columns[2].DefaultCellStyle.Format = "#,###";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ckon.dt.Clear();
                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
        //==================================================================================

        //======================LIST HOLD TRANSACTION============================================
        public void holding()
        {
            CRUD sql = new CRUD();
            LinkApi link = new LinkApi();            

            dgv_Pending.Rows.Clear();            

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM deliverycustomer a JOIN deliverycustomer_line b " 
                                + "ON a.DELIVERY_CUST_ID = b.DELIVERY_CUST_ID "
                                + "WHERE STATUS = '0' AND STORE_FROM = '"+ link.storeId + "'";                              
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["DELIVERY_CUST_ID"].ToString();          
                        date = ckon.sqlDataRd["DATE"].ToString();

                        int dgRows = dgv_Pending.Rows.Add();
                        dgv_Pending.Rows[dgRows].Cells[0].Value = id_trans;
                        dgv_Pending.Rows[dgRows].Cells[1].Value = date;                       
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

        //====================AMBIL TOTAL QTY FROM REQUEST ORDER========================================
        public void qty()
        {
            CRUD sql = new CRUD();            

            try
            {
                ckon.sqlCon().Open();
                String cmd_reqorQty = "SELECT SUM(deliverycustomer_line.QTY) as total FROM deliverycustomer_line where DELIVERY_CUST_ID = '" + t_delivCustId.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_reqorQty, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        qty2 = ckon.sqlDataRd["total"].ToString();
                        t_qty.Text = qty2;
                    }
                }
                else
                {
                    t_qty.Text = "0";
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

        //=============================BUTTON CONFIRM ==================================================
        private void b_confirm_Click(object sender, EventArgs e)
        {
            string currArtId;
            int currQty;

            if (t_qty.Text == "0")
            {                
                MessageBox.Show("No Item On List");
            }
            else
            {
                foreach (DataGridViewRow row in dgv_delivCust.Rows)
                {
                    currArtId = Convert.ToString(row.Cells["_ARTICLEID"].Value);
                    currQty = Convert.ToInt32(row.Cells["_QTY"].Value);

                    cek_article(currArtId, currQty);
                }

                MessageBox.Show("Confirmed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
        }

        //===reset dari tampilan 
        public void reset()
        {
            dgv_delivCust.Rows.Clear();            
            holding();
            new_();            
        }

        public void cek_article(string _articleId, int _qty)
        {            
            CRUD sql = new CRUD();            
            koneksi ckonCheck = new koneksi();
            String cmd_article;
            String cmd_invent;
            Boolean hasRows = false;

            try
            {
                ckon.sqlCon().Open();
                ckonCheck.sqlCon().Open();

                cmd_article = "SELECT * FROM article WHERE article.ARTICLE_ID_ALIAS = '" + _articleId + "'";
                ckonCheck.sqlDataRd = sql.ExecuteDataReader(cmd_article, ckonCheck.sqlCon());

                if (ckonCheck.sqlDataRd.HasRows)
                {
                    while (ckonCheck.sqlDataRd.Read())
                    {
                        artIdLoc = ckonCheck.sqlDataRd["ARTICLE_ID"].ToString();
                        hasRows = true;
                    }
                }

                if (hasRows)
                {
                    if (ckon.sqlDataRd != null)
                        ckon.sqlDataRd.Close();

                    cmd_article = "SELECT * FROM article WHERE ARTICLE_ID = '" + artIdLoc + "'";
                    ckon.sqlDataRdHeader = sql.ExecuteDataReader(cmd_article, ckon.sqlCon());

                    if (ckon.sqlDataRdHeader.HasRows)
                    {
                        while (ckon.sqlDataRdHeader.Read())
                        {
                            id_inv = ckon.sqlDataRdHeader["_id"].ToString();                            
                        }

                        cmd_invent = "SELECT * FROM inventory WHERE ARTICLE_ID = " + id_inv + " AND GOOD_QTY >= 1";
                        ckon.sqlDataRd = sql.ExecuteDataReader(cmd_invent, ckon.sqlCon());

                        if (ckon.sqlDataRd.HasRows)
                        {
                            //MEMOTONG ARTICLE
                            Inv_Line inv = new Inv_Line();
                            int qty_min_plus = _qty;
                            String type_trans = "10";
                            inv.cek_qty_inv(id_inv);
                            inv.cek_type_trans(type_trans);
                            inv.cek_inv_line(t_delivCustId.Text, qty_min_plus);                            
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("Article Not Found");
                    }
                }
                else
                {
                    if (ckon.sqlDataRd != null)
                        ckon.sqlDataRd.Close();

                    cmd_article = "SELECT * FROM article WHERE ARTICLE_ID = '" + artIdLoc + "'";
                    ckon.sqlDataRdHeader = sql.ExecuteDataReader(cmd_article, ckon.sqlCon());

                    if (ckon.sqlDataRdHeader.HasRows)
                    {
                        while (ckon.sqlDataRdHeader.Read())
                        {
                            id_inv = ckon.sqlDataRdHeader["_id"].ToString();                            
                        }

                        cmd_invent = "SELECT * FROM inventory WHERE ARTICLE_ID = " + id_inv + " AND GOOD_QTY >= 1";
                        ckon.sqlDataRd = sql.ExecuteDataReader(cmd_invent, ckon.sqlCon());

                        if (ckon.sqlDataRd.HasRows)
                        {
                            //MEMOTONG ARTICLE
                            Inv_Line inv = new Inv_Line();
                            int qty_min_plus = _qty;
                            String type_trans = "10";
                            inv.cek_qty_inv(id_inv);
                            inv.cek_type_trans(type_trans);
                            inv.cek_inv_line(t_delivCustId.Text, qty_min_plus);                            
                        }                       
                    }
                    else
                    {
                        MessageBox.Show("Article Not Found");
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
    }
}
