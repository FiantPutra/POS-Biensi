using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace try_bi
{
    public partial class W_Promotion_Popup : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String id_diskon, diskon_kode, diskon_name, diskon_ktg, diskon_desc, jenis, id_transaksi, status, art_id_diskon, art_bonus, disc_type, disc_desc;
        int the_real_totall, net_diskon, net_price, total_kotor, total_bersih, sub_total_TransLine, count=0, count_disc_Tline=0;
        String field_none = "None", field_kosong="0", spg_id, field_none2 = "NONE", kodetoko2, custid2;
        //===VARIABEL UNTUK MENDAPATKAN DISKON DARI HARGA ASLI BARANG DIKALI DENGAN QUANTITY
        int qty_kali, price_kali;
        //=============================BUTTON USE========================================
        private void b_ok_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            //if(status=="0")
            //{
            //    MessageBox.Show("Discount Not Match");
            //}
            //else
            //{
            //===CEK APAKAH DISKON SUDAH PERNAH DIGUNAKAN DI TRANSAKSI INI
            cek_discount_line();
            //jika sudah ada maka tampilkan pesan agar tidak bisa digunakan lagi
            if (count_disc_Tline >= 1)
            {
                MessageBox.Show("Discounts Have Been Used");
            }
            else
            {
                //jika tipe diskon adalah 3, atau buy and get
                //if (disc_type == "3")
                //{
                //    //========HITUNG ADA BRAPA BANYAK DISCOUNT ITEM DARI DISCOUNT CODE
                //    count_artcile();
                //    //JIKA ADA 1, LANGSUNG APPLY, JIKA ADA BANYAK, OPEN DIALOG
                //    if (count == 1)
                //    {
                //        W_disc_GetArticle disc = new W_disc_GetArticle();
                //        //disc.message();
                //        disc.get_disc_code(diskon_kode, disc_type, id_transaksi, spg_id);
                //        disc.disc_1item();
                //        disc.search_data_article();
                //        disc.insert();

                //        DiscountAfterUsePromNew afteruser = new DiscountAfterUsePromNew();
                //        afteruser.retreive(id_transaksi, kodetoko2, custid2);

                //        uc_coba.Instance.retreive();
                //        uc_coba.Instance.itung_total();
                //        this.Close();
                //    }
                //    //jumlah diskon_item lebih dari 1, open dialog
                //    else
                //    {
                //        W_disc_GetArticle disc = new W_disc_GetArticle();
                //        disc.get_disc_code(diskon_kode, disc_type, id_transaksi, spg_id);
                //        disc.getget(id_transaksi, kodetoko2, custid2);
                //        disc.retreive();
                //        disc.ShowDialog();
                //        this.Close();
                //    }

                //}
                ////============akhir tipe diskon 3================

                ////jenis diskon 2
                //else if (disc_type == "2")
                //{
                //    try
                //    {
                //        ckon.sqlCon().Open();
                //        String cmd_discType = "Select * from disctype2 where DiscountRetailId='" + id_diskon + "' and TransId = '" + id_transaksi + "'";
                //        ckon.sqlDataRd = sql.ExecuteDataReader(cmd_discType, ckon.sqlCon());

                //        if (ckon.sqlDataRd.HasRows)
                //        {
                //            while (ckon.sqlDataRd.Read())
                //            {
                //                String discount = ckon.sqlDataRd["Discount"].ToString();
                //                String totharga = ckon.sqlDataRd["TotHarga"].ToString();
                //                String artid = ckon.sqlDataRd["articleid"].ToString();
                //                String persen = ckon.sqlDataRd["DiscPersent"].ToString();

                //                String update = "UPDATE transaction_line set DISCOUNT = '" + discount + "', SUBTOTAL = '" + totharga + "', DISCOUNT_CODE = '" + id_diskon + "', DISCOUNT_TYPE='" + disc_type + "', DISCOUNT_DESC = '" + persen + "' WHERE TRANSACTION_ID = '" + id_transaksi + "' and ARTICLE_ID = '" + artid + "'";                                    
                //                sql.ExecuteNonQuery(update);

                //                DiscountAfterUsePromNew afteruser = new DiscountAfterUsePromNew();
                //                afteruser.retreive(id_transaksi, kodetoko2, custid2);

                //                uc_coba.Instance.retreive();
                //                uc_coba.Instance.itung_total();
                //                this.Close();
                //            }
                //        }
                //    }
                //    catch (Exception er)
                //    {
                //        MessageBox.Show("No connection to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //    finally
                //    {
                //        if (ckon.sqlDataRd != null)
                //            ckon.sqlDataRd.Close();

                //        if (ckon.sqlCon().State == ConnectionState.Open)
                //            ckon.sqlCon().Close();
                //    }

                //ckon.cmd = new MySqlCommand(sql, ckon.con);
                //{
                //    try
                //    {
                //        ckon.con.Open();
                //        ckon.myReader = ckon.cmd.ExecuteReader();
                //        while (ckon.myReader.Read())
                //        {
                //            String discount = ckon.myReader.GetString("Discount");
                //            String totharga = ckon.myReader.GetString("TotHarga");
                //            String artid = ckon.myReader.GetString("articleid");
                //            String persen = ckon.myReader.GetString("DiscPersent");

                //            String update = "UPDATE transaction_line set DISCOUNT = '" + discount + "', SUBTOTAL = '" + totharga + "', DISCOUNT_CODE = '"+ id_diskon +"', DISCOUNT_TYPE='"+ disc_type +"', DISCOUNT_DESC = '"+ persen +"' WHERE TRANSACTION_ID = '" + id_transaksi + "' and ARTICLE_ID = '"+ artid +"'";

                //            CRUD changee = new CRUD();
                //            changee.ExecuteNonQuery(update);

                //            DiscountAfterUseProm afteruser = new DiscountAfterUseProm();
                //            afteruser.retreive(id_transaksi, kodetoko2, custid2);

                //            uc_coba.Instance.retreive();
                //            uc_coba.Instance.itung_total();
                //            this.Close();
                //            //diskon_kode = ckon.myReader.GetString("DISCOUNT_CODE");
                //            //diskon_name = ckon.myReader.GetString("DISCOUNT_NAME");
                //            //diskon_ktg = ckon.myReader.GetString("DISCOUNT_CATEGORY");
                //            //diskon_desc = ckon.myReader.GetString("DESCRIPTION");
                //            //status = ckon.myReader.GetString("STATUS");
                //            //art_id_diskon = ckon.myReader.GetString("ARTICLE_ID");
                //            //disc_type = ckon.myReader.GetString("DISCOUNT_TYPE");
                //            //disc_desc = ckon.myReader.GetString("DISCOUNT_DESC");
                //            ////l_diskon_name.Text = diskon_name;
                //            //t_disc_name.Text = diskon_name;
                //            //l_d_ctg.Text = diskon_ktg;
                //            //l_d_code.Text = diskon_kode;
                //            //l_d_desc.Text = diskon_desc;
                //        }
                //        ckon.con.Close();
                //    }
                //    catch (Exception ex)
                //    { MessageBox.Show(ex.ToString()); }
                //}
                //}

                //jenis diskon 4, atau selain tipe 3
                //else
                //{
                //    //=====JENIS DISKON==========
                //    get_type_diskon();
                //    //=====HARGA SETELAH DISKON===
                //    get_total_price();
                //    //MessageBox.Show(total_kotor.ToString());
                //    //=====MASUKAN HARGA SETELAH DISKON KE TRANS_LINE YG SESUAI DENGAN
                //    update(); 
                //}
                //====akhir diskon tipe selain 3

                discPromoCalcSP(id_transaksi, id_diskon);

                uc_coba.Instance.retreive();
                uc_coba.Instance.itung_total();
                this.Close();
            }
            //====akhir jika diskon belom pernah digunakan
            //}            
        }

        public void discPromoCalcSP(String transId, String discPromoCode)
        {
            CRUD sql = new CRUD();

            try
            {
                using (SqlConnection con = new SqlConnection(ckon.sqlCon().ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("ItemUpdatepromotionLine", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@TRANSACTION_ID", SqlDbType.VarChar).Value = transId;
                        cmd.Parameters.Add("@DiscountpromoCode", SqlDbType.VarChar).Value = discPromoCode;                        

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //============================BUTTON CANCEL=======================================
        private void b_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //===================================================================================
        private void W_Promotion_Popup_Load(object sender, EventArgs e)
        {
            //========DATA PROMOTION HEADER======
            data_from_id();
            //===TAMPILKAN DATA PROMOTION LINE=======
            retreive();
            //=====CEK TIPE DISKON DARI PROMOTION LINE===
            //get_data();
            //===========MENDAPATKAN SUBTOTAL DARI TRANS ID DAN ARTICLE ID YG DI DAPAT DARI POST DISCOUT, UTK DIHITUNG HARGA KENA DISKON
            //get_price_TransLine();
        }
        //====================================================================================
        public W_Promotion_Popup(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //==================================METHOD FOR GET DATA FORM TRANSACTION============================
        public void get_id(String id, int the_real, String id_trans, String spg, String custid, String kodetoko)
        {
            id_diskon = id;
            the_real_totall = the_real;
            id_transaksi = id_trans;
            spg_id = spg;
            kodetoko2 = kodetoko;
            custid2 = custid;
        }
        //===========================METHOD FOR GET DATA FROM DISCOUNT CODE FROM DISCOUNT HEADER============================
        public void data_from_id()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                //String cmd = "Select * from promotion where DISCOUNT_CODE='" + id_diskon + "'";
                String cmd = "SELECT a.DiscountCode, a.DiscountName, b.Code, a.DiscountType FROM DiscountSetup a "
                                + "INNER JOIN DiscountSetupLines b ON b.DiscountSetupId = a.Id "
                                + "INNER JOIN article c ON c.ARTICLE_ID = b.Code "
                                + "WHERE(a.DiscountType = 4 OR a.DiscountType = 5) AND a.DiscountCode = '" + id_diskon + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        diskon_kode = ckon.sqlDataRd["DiscountCode"].ToString();
                        diskon_name = ckon.sqlDataRd["DiscountName"].ToString();
                        //diskon_ktg = ckon.sqlDataRd["DISCOUNT_CATEGORY"].ToString();
                        //diskon_desc = ckon.sqlDataRd["DESCRIPTION"].ToString();
                        //status = ckon.sqlDataRd["STATUS"].ToString();
                        art_id_diskon = ckon.sqlDataRd["Code"].ToString();
                        disc_type = ckon.sqlDataRd["DiscountType"].ToString();
                        //disc_desc = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();
                        //l_diskon_name.Text = diskon_name;
                        t_disc_name.Text = diskon_name;
                        l_d_code.Text = diskon_kode;
                        //l_d_name.Text = diskon_kode;
                        l_d_type.Text = disc_type;
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
            //{
            //    try
            //    {
            //        ckon.con.Open();
            //        ckon.myReader = ckon.cmd.ExecuteReader();
            //        while(ckon.myReader.Read())
            //        {
            //            diskon_kode = ckon.myReader.GetString("DISCOUNT_CODE");
            //            diskon_name = ckon.myReader.GetString("DISCOUNT_NAME");
            //            diskon_ktg = ckon.myReader.GetString("DISCOUNT_CATEGORY");
            //            diskon_desc = ckon.myReader.GetString("DESCRIPTION");
            //            status = ckon.myReader.GetString("STATUS");
            //            art_id_diskon = ckon.myReader.GetString("ARTICLE_ID");
            //            disc_type = ckon.myReader.GetString("DISCOUNT_TYPE");
            //            disc_desc = ckon.myReader.GetString("DISCOUNT_DESC");
            //            //l_diskon_name.Text = diskon_name;
            //            t_disc_name.Text = diskon_name;
            //            l_d_ctg.Text = diskon_ktg;
            //            l_d_code.Text = diskon_kode;
            //            l_d_desc.Text = diskon_desc;
            //        }
            //    }
            //    catch
            //    { }
            //}
        }
        //===============================================================================================
        public void get_data()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM promotion_line WHERE DISCOUNT_CODE='" + id_diskon + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        art_bonus = ckon.sqlDataRd["ARTICLE_ID_DISCOUNT"].ToString();
                        net_diskon = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT_PERCENT"].ToString());
                        net_price = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT_PRICE"].ToString());
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
            //        //try
            //        //{ net_diskon = ckon.myReader.GetInt32("DISCOUNT_PERCENT"); }
            //        //catch
            //        //{ net_diskon = 0; }
            //        ////===============================
            //        //try
            //        //{ net_price = ckon.myReader.GetInt32("DISCOUNT_PRICE"); }
            //        //catch
            //        //{ net_price = 0; }
            //        art_bonus = ckon.myReader.GetString("ARTICLE_ID_DISCOUNT");
            //        net_diskon = ckon.myReader.GetInt32("DISCOUNT_PERCENT");
            //        net_price = ckon.myReader.GetInt32("DISCOUNT_PRICE");
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }

        }
        
        //===============================================================================================

        //===========================METHOD FOR GET DATA FROM DISCOUNT LINE================================
        public void retreive()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            dgv_purchase.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                //String cmd = "SELECT * FROM promotion_line WHERE DISCOUNT_CODE='" + id_diskon + "'";
                String cmd = "SELECT a.ARTICLE_ID, b.ARTICLE_NAME, a.DiscountCode, a.DiscountName, a.DiscountType, a.DiscountPercent, DiscountCash, "
                                + "a.QtyMin, a.QtyMax, a.AmountMin, a.AmountMax, a.HeaderDiscountPercent, a.HeaderDiscountCash, a.HeaderQtyMin, a.HeaderQtyMax, a.HeaderAmountMin, a.HeaderAmountMax "
                                + "FROM TransactionDiscount a INNER JOIN article b ON b.ARTICLE_ID = a.ARTICLE_ID "
                                + "WHERE DiscountCode ='" + id_diskon + "'";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_purchase.Rows.Add();
                    dgv_purchase.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[2].Value = row["DiscountPercent"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[3].Value = row["DiscountCash"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[4].Value = row["QtyMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[5].Value = row["QtyMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[6].Value = row["AmountMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[7].Value = row["AmountMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[8].Value = row["HeaderDiscountPercent"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[9].Value = row["HeaderDiscountCash"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[10].Value = row["HeaderQtyMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[11].Value = row["HeaderQtyMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[12].Value = row["HeaderAmountMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[13].Value = row["HeaderAmountMax"].ToString();
                    //dgv_purchase.Rows[dgRows].Cells[14].Value = row["SPESIAL_PRICE"];

                }                

                //==========================FUNCTION FOR HIDE FIELD WHEN FIELD EMPTY===============================================
                foreach (DataGridViewColumn clm in dgv_purchase.Columns)
                {
                    dgv_purchase.Columns[clm.Index].Visible = false;
                    //bool notAvailable = true;
                    bool notAvailable = false;

                    foreach (DataGridViewRow row in dgv_purchase.Rows)
                    {
                        if (row.Cells[clm.Index].Value != null)
                        {
                            // If string of value is empty
                            if (row.Cells[clm.Index].Value.ToString() != field_none)
                            {
                                if (row.Cells[clm.Index].Value.ToString() != field_none2)
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
                    }

                    if (notAvailable)
                    {
                        //dgv_purchase.Columns[clm.Index].Visible = false;
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
        }
        //===========================================================================================================

        public void promotionByItem()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            dgv_purchase.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();                
                String cmd = "SELECT b.Code, c.ARTICLE_NAME, a.DiscountCode, a.DiscountName, a.DiscountType, a.DiscountPercent as HeaderDiscountPercent, a. DiscountCash as HeaderDiscountCash, "
                                + "a.QtyMin as HeaderQtyMin, a.QtyMax as HeaderQtyMax, a.AmountMin as HeaderAmountMin, a.AmountMax as HeaderAmountMax, b.DiscountPrecentage, b.DiscountCash, "
                                + "b.QtyMin, b.QtyMax, b.AmountMin, b.AmountMax FROM DiscountSetup a "
                                + "INNER JOIN DiscountSetupLines b ON b.DiscountSetupId = a.Id "
                                + "INNER JOIN article c ON c.ARTICLE_ID = b.Code "
                                + "WHERE a.DiscountCode = '" + id_diskon + "'";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_purchase.Rows.Add();
                    dgv_purchase.Rows[dgRows].Cells[0].Value = row["Code"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[2].Value = row["DiscountPercent"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[3].Value = row["DiscountCash"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[4].Value = row["QtyMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[5].Value = row["QtyMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[6].Value = row["AmountMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[7].Value = row["AmountMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[8].Value = row["HeaderDiscountPercent"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[9].Value = row["HeaderDiscountCash"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[10].Value = row["HeaderQtyMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[11].Value = row["HeaderQtyMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[12].Value = row["HeaderAmountMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[13].Value = row["HeaderAmountMax"].ToString();                     
                }

                //==========================FUNCTION FOR HIDE FIELD WHEN FIELD EMPTY===============================================
                foreach (DataGridViewColumn clm in dgv_purchase.Columns)
                {
                    dgv_purchase.Columns[clm.Index].Visible = false;
                    //bool notAvailable = true;
                    bool notAvailable = false;

                    foreach (DataGridViewRow row in dgv_purchase.Rows)
                    {
                        if (row.Cells[clm.Index].Value != null)
                        {
                            // If string of value is empty
                            if (row.Cells[clm.Index].Value.ToString() != field_none)
                            {
                                if (row.Cells[clm.Index].Value.ToString() != field_none2)
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
                    }

                    if (notAvailable)
                    {
                        //dgv_purchase.Columns[clm.Index].Visible = false;
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
        }

        public void promotionByBrand()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            dgv_purchase.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT b.Code, c.Description, a.DiscountCode, a.DiscountName, a.DiscountType, a.DiscountPercent as HeaderDiscountPercent, a. DiscountCash as HeaderDiscountCash, "
                                + "a.QtyMin as HeaderQtyMin, a.QtyMax as HeaderQtyMax, a.AmountMin as HeaderAmountMin, a.AmountMax as HeaderAmountMax, b.DiscountPrecentage, b.DiscountCash, "
                                + "b.QtyMin, b.QtyMax, b.AmountMin, b.AmountMax FROM DiscountSetup a "
                                + "INNER JOIN DiscountSetupLines b ON b.DiscountSetupId = a.Id "
                                + "INNER JOIN itemdimensionbrand c ON c.Code = b.Code "
                                + "WHERE a.DiscountCode = '" + id_diskon + "'";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_purchase.Rows.Add();
                    dgv_purchase.Rows[dgRows].Cells[0].Value = row["Code"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[1].Value = row["Description"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[2].Value = row["DiscountPercent"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[3].Value = row["DiscountCash"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[4].Value = row["QtyMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[5].Value = row["QtyMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[6].Value = row["AmountMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[7].Value = row["AmountMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[8].Value = row["HeaderDiscountPercent"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[9].Value = row["HeaderDiscountCash"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[10].Value = row["HeaderQtyMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[11].Value = row["HeaderQtyMax"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[12].Value = row["HeaderAmountMin"].ToString();
                    dgv_purchase.Rows[dgRows].Cells[13].Value = row["HeaderAmountMax"].ToString();
                }

                //==========================FUNCTION FOR HIDE FIELD WHEN FIELD EMPTY===============================================
                foreach (DataGridViewColumn clm in dgv_purchase.Columns)
                {
                    dgv_purchase.Columns[clm.Index].Visible = false;
                    //bool notAvailable = true;
                    bool notAvailable = false;

                    foreach (DataGridViewRow row in dgv_purchase.Rows)
                    {
                        if (row.Cells[clm.Index].Value != null)
                        {
                            // If string of value is empty
                            if (row.Cells[clm.Index].Value.ToString() != field_none)
                            {
                                if (row.Cells[clm.Index].Value.ToString() != field_none2)
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
                    }

                    if (notAvailable)
                    {
                        //dgv_purchase.Columns[clm.Index].Visible = false;
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
        }

        //=======================GET TYPE DISKON=====================================================================
        public void get_type_diskon()
        {
            //if(net_price == 0)
            //if(net_price == 0 && art_bonus == "")
            if (net_price == 0 )
            {
                jenis = "Percent";
            }
            //if(net_diskon == 0)
            //else if(net_diskon==0 && art_bonus == "")
            else if (net_diskon == 0 )
            {
                jenis = "Price";
            }
            else
            //if(net_diskon == 0 && net_price == 0)
            {
                jenis = "Bonus";
            }

        }
        //===========================================================================================================

        //==================================HITUNG TOTAL BELANJA SETELAH DISKON======================================
        public void get_total_price()
        {
            if(jenis == "Percent")
            {
                //total_kotor = (sub_total_TransLine * net_diskon) / 100;
                //total_bersih = the_real_totall - total_kotor;
                //======BARU, UNTUK MENGHITUNG TOTAL DISKON DARI HARGA ASLI PRICE x QUANTITY
                total_kotor = (sub_total_TransLine * net_diskon) / 100;
                total_bersih = sub_total_TransLine - total_kotor;
            }
            if(jenis == "Price")
            {
                //total_kotor = net_price; 
                //total_bersih = the_real_totall - total_kotor;
                //======BARU, UNTUK MENGHITUNG TOTAL DISKON DARI HARGA ASLI PRICE x QUANTITY
                total_kotor = net_price;
                total_bersih = sub_total_TransLine - total_kotor;

            }
        }
        //===========================================================================================================

        //========================UPDATE VALUES DISKON INTO DATABASE TRANSACTION HEADER==============================
        public void update()
        {
            String cmd_update = "UPDATE transaction_line SET DISCOUNT='" + total_kotor + "', SUBTOTAL = '"+ total_bersih +"', DISCOUNT_CODE = '"+ diskon_kode + "', DISCOUNT_DESC = '"+ disc_desc +"' WHERE TRANSACTION_ID='" + id_transaksi + "' AND ARTICLE_ID='" + art_id_diskon + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);

            DiscountAfterUsePromNew afteruser = new DiscountAfterUsePromNew();
            afteruser.retreive(id_transaksi, kodetoko2, custid2);

            uc_coba.Instance.retreive();
            uc_coba.Instance.itung_total();
            this.Close();
        }
        //===========================================================================================================

        //================ambil total harga dari article id yg didapat dari saat post discount, untuk diganti harga discount nya==
        public void get_price_TransLine()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM transaction_line WHERE TRANSACTION_ID='" + id_transaksi + "' AND ARTICLE_ID = '" + art_id_diskon + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        qty_kali = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"].ToString());
                        price_kali = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());                        
                        sub_total_TransLine = qty_kali * price_kali;
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
            //    qty_kali = ckon.myReader.GetInt32("QUANTITY");
            //    price_kali = ckon.myReader.GetInt32("PRICE");
            //    //sub_total_TransLine = ckon.myReader.GetInt32("SUBTOTAL");
            //    sub_total_TransLine = qty_kali * price_kali;
            //}
            //ckon.con.Close();
        }
        //===============================================================
        //===================hitung ada berapa item disc sesuai discount code======
        public void count_artcile()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM discount_item WHERE DISCOUNT_CODE='" + diskon_kode + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        count = count + 1;
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
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        count = count + 1;
            //    }
            //    ckon.con.Close();
            //}
            //else
            //{

            //}
            //ckon.con.Close();
        }
        //CEK DI TRANS LINE APAKAH DISCOUNT CODE INI SUDAH ADA APA BLOM, JIKA ADA=TOLAK, JIKA TIDAK ADA=LANJUTKAN
        public void cek_discount_line()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                //String cmd = "SELECT * FROM transaction_line WHERE TRANSACTION_ID='" + id_transaksi + "' AND DISCOUNT_CODE = '" + id_diskon + "'";
                String cmd = "SELECT * FROM [tmp]."+ kodetoko2 + " WHERE TRANSACTION_ID='" + id_transaksi + "' AND DISCOUNT_CODE = '" + id_diskon + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        count_disc_Tline = count_disc_Tline + 1;
                    }
                }
                else
                {
                    count_disc_Tline = 0;
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
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        count_disc_Tline = count_disc_Tline + 1;
            //    }
            //    ckon.con.Close();
            //}
            //else
            //{
            //    count_disc_Tline = 0;
            //}
            //ckon.con.Close();
        }

    }
}
