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
    public partial class UC_Promotion : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String id_disc;
        String field_none = "None", field_kosong = "0";
        private static UC_Promotion _instance;
        public static UC_Promotion Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Promotion(f1);
                return _instance;
            }
        }
        public UC_Promotion(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //===========================================================================================================
        public void holding()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            dgv_hold.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                //String cmd = "SELECT * FROM promotion";
                String cmd = "SELECT DiscountCode, DiscountName, StartDate, EndDate FROM DiscountSetup WHERE (DiscountType = 4 or DiscountType = 5)";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_hold.Rows.Add();
                    dgv_hold.Rows[dgRows].Cells[0].Value = row["DiscountCode"];
                    dgv_hold.Rows[dgRows].Cells[1].Value = row["DiscountName"];
                    dgv_hold.Rows[dgRows].Cells[2].Value = row["StartDate"];
                    dgv_hold.Rows[dgRows].Cells[3].Value = row["EndDate"];
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);
            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_hold.Rows.Add();
            //        dgv_hold.Rows[n].Cells[0].Value = row["DISCOUNT_CODE"];
            //        dgv_hold.Rows[n].Cells[1].Value = row["DISCOUNT_NAME"];
            //        dgv_hold.Rows[n].Cells[2].Value = row["START_DATE"];
            //        dgv_hold.Rows[n].Cells[3].Value = row["END_DATE"];
            //    }
            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        //===========================================================================================================
        public void get_data_id()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            dgv_DiscHeader.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                //String cmd = "SELECT * FROM promotion WHERE DISCOUNT_CODE='" + id_disc + "'";
                String cmd = "SELECT * FROM DiscountSetup WHERE DiscountCode = '" + id_disc + "' AND (DiscountType = 4 OR DiscountType = 5)";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        dc_name.Text = ckon.sqlDataRd["DiscountName"].ToString();
                        int dgRows = dgv_DiscHeader.Rows.Add();
                        dgv_DiscHeader.Rows[dgRows].Cells[0].Value = ckon.sqlDataRd["DiscountCode"];

                        int DiscType = Convert.ToInt32(ckon.sqlDataRd["DiscountType"]);
                        if (DiscType == 4)
                            dgv_DiscHeader.Rows[dgRows].Cells[1].Value = "Mix and Match";
                        else
                            dgv_DiscHeader.Rows[dgRows].Cells[1].Value = "Buy and Get";

                        int Status = Convert.ToInt32(ckon.sqlDataRd["Status"]);
                        if (Status == 0)
                            dgv_DiscHeader.Rows[dgRows].Cells[2].Value = "Inactived";
                        else
                            dgv_DiscHeader.Rows[dgRows].Cells[2].Value = "Active";

                        dgv_DiscHeader.Rows[dgRows].Cells[3].Value = ckon.sqlDataRd["DiscountCash"];
                        dgv_DiscHeader.Rows[dgRows].Cells[4].Value = ckon.sqlDataRd["DiscountPercent"];
                        dgv_DiscHeader.Rows[dgRows].Cells[5].Value = ckon.sqlDataRd["QtyMin"];
                        dgv_DiscHeader.Rows[dgRows].Cells[6].Value = ckon.sqlDataRd["QtyMax"];
                        dgv_DiscHeader.Rows[dgRows].Cells[7].Value = ckon.sqlDataRd["AmountMin"];
                        dgv_DiscHeader.Rows[dgRows].Cells[8].Value = ckon.sqlDataRd["AmountMax"];

                        //dc_ctg.Text = ckon.sqlDataRd["DISCOUNT_CATEGORY"].ToString();
                        //dc_cd.Text = ckon.sqlDataRd["DISCOUNT_CODE"].ToString();
                        //dc_desc.Text = ckon.sqlDataRd["DESCRIPTION"].ToString();
                    }

                    foreach (DataGridViewColumn clm in dgv_DiscHeader.Columns)
                    {
                        dgv_DiscHeader.Columns[clm.Index].Visible = false;
                        bool notAvailable = false;

                        foreach (DataGridViewRow row in dgv_DiscHeader.Rows)
                        {
                            if (row.Cells[clm.Index].Value != null)
                            {
                                // If string of value is empty
                                if (row.Cells[clm.Index].Value.ToString() != field_none)
                                {
                                    if (row.Cells[clm.Index].Value.ToString() != field_kosong)
                                    {
                                        if (row.Cells[clm.Index].Value.ToString() != "")
                                        {
                                            notAvailable = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (notAvailable)
                        {
                            dgv_DiscHeader.Columns[clm.Index].Visible = true;
                        }
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    dc_name.Text = ckon.myReader.GetString("DISCOUNT_NAME");
            //    dc_ctg.Text = ckon.myReader.GetString("DISCOUNT_CATEGORY");
            //    dc_cd.Text = ckon.myReader.GetString("DISCOUNT_CODE");
            //    dc_desc.Text = ckon.myReader.GetString("DESCRIPTION");
            //}
            //ckon.con.Close();
        }
        //===========================================================================================================
        public void retreive()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            dgv_purchase.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                //String cmd = "SELECT * FROM promotion_line WHERE DISCOUNT_CODE='" + id_disc + "'";
                String cmd = "SELECT b.Code, c.ARTICLE_NAME, b.DiscountCash, b.DiscountPrecentage, b.QtyMin, b.QtyMax, b.AmountMin, b.AmountMax FROM DiscountSetup a "
                                + "INNER JOIN DiscountSetupLines b ON b.DiscountSetupId = a.Id "
                                + "INNER JOIN article c ON c.ARTICLE_ID = b.Code "
                                + "WHERE(a.DiscountType = 4 OR a.DiscountType = 5) AND a.DiscountCode = '"+ id_disc + "'";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_purchase.Rows.Add();
                    dgv_purchase.Rows[dgRows].Cells[0].Value = row["Code"];
                    dgv_purchase.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"];
                    dgv_purchase.Rows[dgRows].Cells[2].Value = row["DiscountCash"];
                    dgv_purchase.Rows[dgRows].Cells[3].Value = row["DiscountPrecentage"];
                    dgv_purchase.Rows[dgRows].Cells[4].Value = row["QtyMin"];
                    dgv_purchase.Rows[dgRows].Cells[5].Value = row["QtyMax"];
                    dgv_purchase.Rows[dgRows].Cells[6].Value = row["AmountMin"];
                    dgv_purchase.Rows[dgRows].Cells[7].Value = row["AmountMax"];
                    //dgv_purchase.Rows[dgRows].Cells[8].Value = row["CUSTOMER_GROUP"];
                    //dgv_purchase.Rows[dgRows].Cells[9].Value = row["QTA"];
                    //dgv_purchase.Rows[dgRows].Cells[10].Value = row["AMOUNT"];
                    //dgv_purchase.Rows[dgRows].Cells[11].Value = row["BANK"];
                    //dgv_purchase.Rows[dgRows].Cells[12].Value = row["DISCOUNT_PERCENT"];
                    //dgv_purchase.Rows[dgRows].Cells[13].Value = row["DISCOUNT_PRICE"];
                    //dgv_purchase.Rows[dgRows].Cells[14].Value = row["SPESIAL_PRICE"];
                }
                //dgv_purchase.Columns[10].DefaultCellStyle.Format = "#,###";
                //dgv_purchase.Columns[13].DefaultCellStyle.Format = "#,###";
                //dgv_purchase.Columns[14].DefaultCellStyle.Format = "#,###";

                foreach (DataGridViewColumn clm in dgv_purchase.Columns)
                {
                    dgv_purchase.Columns[clm.Index].Visible = false;                    
                    bool notAvailable = false;

                    foreach (DataGridViewRow row in dgv_purchase.Rows)
                    {
                        if (row.Cells[clm.Index].Value != null)
                        {
                            // If string of value is empty
                            if (row.Cells[clm.Index].Value.ToString() != field_none)
                            {
                                if (row.Cells[clm.Index].Value.ToString() != field_kosong)
                                {
                                    if (row.Cells[clm.Index].Value.ToString() != "")
                                    {
                                        notAvailable = true;
                                        break;
                                    }
                                }                                
                            }
                        }
                    }

                    if (notAvailable)
                    {                        
                        dgv_purchase.Columns[clm.Index].Visible = true;                        
                    }
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);
            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_purchase.Rows.Add();
            //        dgv_purchase.Rows[n].Cells[0].Value = row["ARTICLE_ID"];
            //        dgv_purchase.Rows[n].Cells[1].Value = row["ARTICLE_NAME"];
            //        dgv_purchase.Rows[n].Cells[2].Value = row["BRAND"];
            //        dgv_purchase.Rows[n].Cells[3].Value = row["SIZE"];
            //        dgv_purchase.Rows[n].Cells[4].Value = row["COLOR"];
            //        dgv_purchase.Rows[n].Cells[5].Value = row["GENDER"];
            //        dgv_purchase.Rows[n].Cells[6].Value = row["DEPARTMENT"];
            //        dgv_purchase.Rows[n].Cells[7].Value = row["DEPARTMENT_TYPE"];
            //        dgv_purchase.Rows[n].Cells[8].Value = row["CUSTOMER_GROUP"];
            //        dgv_purchase.Rows[n].Cells[9].Value = row["QTA"];
            //        dgv_purchase.Rows[n].Cells[10].Value = row["AMOUNT"];
            //        dgv_purchase.Rows[n].Cells[11].Value = row["BANK"];
            //        dgv_purchase.Rows[n].Cells[12].Value = row["DISCOUNT_PERCENT"];
            //        dgv_purchase.Rows[n].Cells[13].Value = row["DISCOUNT_PRICE"];
            //        dgv_purchase.Rows[n].Cells[14].Value = row["SPESIAL_PRICE"];
            //    }
            //    dgv_purchase.Columns[10].DefaultCellStyle.Format = "#,###";
            //    dgv_purchase.Columns[13].DefaultCellStyle.Format = "#,###";
            //    dgv_purchase.Columns[14].DefaultCellStyle.Format = "#,###";
            //    //===================================================================================================================
            //    foreach (DataGridViewColumn clm in dgv_purchase.Columns)
            //    {

            //        dgv_purchase.Columns[clm.Index].Visible = false;
            //        //bool notAvailable = true;
            //        bool notAvailable = false;

            //        foreach (DataGridViewRow row in dgv_purchase.Rows)
            //        {

            //            if (row.Cells[clm.Index].Value != null)
            //            {
            //                // If string of value is empty
            //                if (row.Cells[clm.Index].Value.ToString() != field_none)
            //                {
            //                    if (row.Cells[clm.Index].Value.ToString() != field_kosong)
            //                    {
            //                        if (row.Cells[clm.Index].Value.ToString() != "")
            //                        {
            //                            notAvailable = true;
            //                            break;
            //                        }
            //                    }
            //                    //notAvailable = false;

            //                }
            //            }
            //        }

            //        if (notAvailable)
            //        {
            //            //dgv_purchase.Columns[clm.Index].Visible = false;
            //            dgv_purchase.Columns[clm.Index].Visible = true;
            //            //dgv_purchase.Columns[clm.Index].Name("h") = true;
            //        }
            //    }
            //    //===================================================================================================================
            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        
        //===========================================================================================================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    id_disc = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    //dgv_purchase.Visible = true;

                    get_data_id();
                    retreive();
                }
            }
            catch (Exception)
            {

            }
        }
        //===========================================================================================================
    }
}
