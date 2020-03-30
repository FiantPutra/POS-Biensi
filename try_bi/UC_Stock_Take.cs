using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class UC_Stock_Take : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        int total;
        public string id_employee2, nm_employee2, status_sukses;
        //======================================================
        private static UC_Stock_Take _instance;
        public static UC_Stock_Take Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Stock_Take(f1);
                return _instance;
            }
        }
        //=======================================================
        public UC_Stock_Take(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        public void from_form1()
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                //String cmd_stock = "SELECT * FROM stock_take";
                //ckon.sqlDataRd = sql.ExecuteDataReader(cmd_stock, ckon.sqlCon());

                //if (ckon.sqlDataRd.HasRows)
                //{
                    String cmd_delete = "DELETE FROM stock_take";
                    sql.ExecuteNonQuery(cmd_delete);

                    List<GetStockTake> list_stoktake = new List<GetStockTake>();                                        

                    String cmd_inv = "SELECT * FROM inventory";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_inv, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            String id2 = ckon.sqlDataRd["_id"].ToString();
                            String art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                            String good_qty = ckon.sqlDataRd["GOOD_QTY"].ToString();
                            String reject_qty = ckon.sqlDataRd["REJECT_QTY"].ToString();
                            String wh_reject = ckon.sqlDataRd["WH_REJECT_QTY"].ToString();
                            String status = ckon.sqlDataRd["STATUS"].ToString();

                            list_stoktake.Add(new GetStockTake() { id = id2, articleid = art_id, goodqty = good_qty, rejectqty = reject_qty, whgoodqty = good_qty, whrejectqty = wh_reject, status = status });
                        }
                        String connectionString = ckon.conSQL;

                        using (SqlConnection mConnection = new SqlConnection(connectionString))
                        {
                            mConnection.Open();                            
                            String cmd_insert = "INSERT INTO stock_take (_id, ARTICLE_ID, GOOD_QTY, REJECT_QTY, WH_GOOD_QTY, WH_REJECT_QTY, STATUS) " +
                                                    "VALUES (@PARM1, @PARM2, @PARM3, @PARM4, @PARM5, @PARM6, @PARM7)";
                            SqlCommand cmd = new SqlCommand(cmd_insert);
                            cmd.Connection = mConnection;
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@PARM1", DbType.Int32);
                            cmd.Parameters.Add("@PARM2", DbType.String);
                            cmd.Parameters.Add("@PARM3", DbType.Int32);
                            cmd.Parameters.Add("@PARM4", DbType.Int32);
                            cmd.Parameters.Add("@PARM5", DbType.Int32);
                            cmd.Parameters.Add("@PARM6", DbType.Int32);
                            cmd.Parameters.Add("@PARM7", DbType.Int32);                            

                            for (int i = 0; i < list_stoktake.Count; i++)
                            {
                                cmd.Parameters[0].Value = list_stoktake[i].id;
                                cmd.Parameters[1].Value = list_stoktake[i].articleid;
                                cmd.Parameters[2].Value = list_stoktake[i].goodqty;
                                cmd.Parameters[3].Value = list_stoktake[i].rejectqty;
                                cmd.Parameters[4].Value = list_stoktake[i].whgoodqty;
                                cmd.Parameters[5].Value = list_stoktake[i].whrejectqty;
                                cmd.Parameters[6].Value = list_stoktake[i].status;                                
                                cmd.ExecuteNonQuery();
                            }
                            mConnection.Close();
                        }

                        String cmd_article = "SELECT TOP 100 article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID";
                        retreive(cmd_article);
                    }
                //}
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
            //    String sql2 = "DELETE FROM stock_take";
            //    CRUD delete = new CRUD();
            //    delete.ExecuteNonQuery(sql2);

            //        List<GetStockTake> list_stoktake = new List<GetStockTake>();
            //    string ConnectionString = "Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
            //    StringBuilder sCommand = new StringBuilder("INSERT INTO stock_take (_id, ARTICLE_ID, GOOD_QTY, REJECT_QTY, WH_GOOD_QTY, WH_REJECT_QTY,STATUS) VALUES");

            //    String sql3 = "SELECT * FROM inventory";
            //    ckon2.cmd2 = new MySqlCommand(sql3, ckon2.con2);
            //    ckon2.con2.Open();
            //    ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //    while (ckon2.myReader2.Read())
            //    {
            //        String id2 = ckon2.myReader2.GetString("_id");
            //        String art_id = ckon2.myReader2.GetString("ARTICLE_ID");
            //        String good_qty = ckon2.myReader2.GetString("GOOD_QTY");
            //        String reject_qty = ckon2.myReader2.GetString("REJECT_QTY");
            //        String wh_reject = ckon2.myReader2.GetString("WH_REJECT_QTY");
            //        String status = ckon2.myReader2.GetString("STATUS");

            //        list_stoktake.Add(new GetStockTake() { id = id2, articleid = art_id, goodqty = good_qty, rejectqty = reject_qty, whgoodqty = good_qty, whrejectqty = wh_reject, status = status });

            //        //String query = "INSERT INTO stock_take (_id, ARTICLE_ID, GOOD_QTY, REJECT_QTY, WH_GOOD_QTY, WH_REJECT_QTY,STATUS) VALUES ('" + id2 + "', '" + art_id + "', '" + good_qty + "', '" + reject_qty + "', '" + good_qty + "', '" + wh_reject + "', '" + status + "')";
            //        //CRUD input = new CRUD();
            //        //input.ExecuteNonQuery(query);

            //    }

            //    using (MysqlCon()nection mConnection = new MysqlCon()nection(ConnectionString))
            //    {
            //        List<string> Rows = new List<string>();
            //        for (int i = 0; i < list_stoktake.Count; i++)
            //        {
            //            //for(int j= i; j < 500; j++)
            //            //{
            //            Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", MySqlHelper.EscapeString(list_stoktake[i].id.ToString()), MySqlHelper.EscapeString(list_stoktake[i].articleid), MySqlHelper.EscapeString(list_stoktake[i].goodqty), MySqlHelper.EscapeString(list_stoktake[i].rejectqty), MySqlHelper.EscapeString(list_stoktake[i].goodqty), MySqlHelper.EscapeString(list_stoktake[i].whrejectqty), MySqlHelper.EscapeString(list_stoktake[i].status)));
            //            //}

            //        }
            //        sCommand.Append(string.Join(",", Rows));
            //        sCommand.Append(";");
            //        mConnection.Open();
            //        using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
            //        {
            //            myCmd.CommandType = CommandType.Text;
            //            myCmd.ExecuteNonQuery();
            //        }
            //    }


            //    ckon2.con2.Close();
            //    String sql4 = "SELECT article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID LIMIT 100";
            //    retreive(sql4);
            //    //String sql2 = "SELECT article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID";
            //    //retreive(sql2);
            //}
            //else
            //{
            //    String sql2 = "DELETE FROM stock_take";
            //    CRUD delete = new CRUD();
            //    delete.ExecuteNonQuery(sql2);

            //    List<GetStockTake> list_stoktake = new List<GetStockTake>();
            //    string ConnectionString = "Server='" + try_bi.Properties.Settings.Default.mServer + "';Database='" + try_bi.Properties.Settings.Default.mDBName + "';Uid='" + try_bi.Properties.Settings.Default.mUserDB + "';Pwd='" + try_bi.Properties.Settings.Default.mPassDB + "';";
            //    StringBuilder sCommand = new StringBuilder("INSERT INTO stock_take (_id, ARTICLE_ID, GOOD_QTY, REJECT_QTY, WH_GOOD_QTY, WH_REJECT_QTY,STATUS) VALUES");

            //    String sql3 = "SELECT * FROM inventory";
            //    ckon2.cmd2 = new MySqlCommand(sql3, ckon2.con2);
            //    ckon2.con2.Open();
            //    ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //    while (ckon2.myReader2.Read())
            //    {
            //        String id2 = ckon2.myReader2.GetString("_id");
            //        String art_id = ckon2.myReader2.GetString("ARTICLE_ID");
            //        String good_qty = ckon2.myReader2.GetString("GOOD_QTY");
            //        String reject_qty = ckon2.myReader2.GetString("REJECT_QTY");
            //        String wh_reject = ckon2.myReader2.GetString("WH_REJECT_QTY");
            //        String status = ckon2.myReader2.GetString("STATUS");

            //        list_stoktake.Add(new GetStockTake() { id = id2, articleid = art_id, goodqty = good_qty, rejectqty = reject_qty, whgoodqty = good_qty, whrejectqty = wh_reject, status = status });

            //        //String query = "INSERT INTO stock_take (_id, ARTICLE_ID, GOOD_QTY, REJECT_QTY, WH_GOOD_QTY, WH_REJECT_QTY,STATUS) VALUES ('" + id2 + "', '" + art_id + "', '" + good_qty + "', '" + reject_qty + "', '" + good_qty + "', '" + wh_reject + "', '" + status + "')";
            //        //CRUD input = new CRUD();
            //        //input.ExecuteNonQuery(query);

            //    }

            //    using (MysqlCon()nection mConnection = new MysqlCon()nection(ConnectionString))
            //    {
            //        List<string> Rows = new List<string>();
            //        for (int i = 0; i < list_stoktake.Count; i++)
            //        {
            //            //for(int j= i; j < 500; j++)
            //            //{
            //            Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", MySqlHelper.EscapeString(list_stoktake[i].id.ToString()), MySqlHelper.EscapeString(list_stoktake[i].articleid), MySqlHelper.EscapeString(list_stoktake[i].goodqty), MySqlHelper.EscapeString(list_stoktake[i].rejectqty), MySqlHelper.EscapeString(list_stoktake[i].goodqty), MySqlHelper.EscapeString(list_stoktake[i].whrejectqty), MySqlHelper.EscapeString(list_stoktake[i].status)));
            //            //}

            //        }
            //        sCommand.Append(string.Join(",", Rows));
            //        sCommand.Append(";");
            //        mConnection.Open();
            //        using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
            //        {
            //            myCmd.CommandType = CommandType.Text;
            //            myCmd.ExecuteNonQuery();
            //        }
            //    }


            //    ckon2.con2.Close();
            //    String sql4 = "SELECT article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID LIMIT 100";
            //    retreive(sql4);
            //}
            //ckon.con.Close();

        }
        //===========================================================================
        //===========================================================================
        public void retreive(String cmd)
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            dgv_inv.Rows.Clear();

            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        String name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        String id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        String goodQty = ckon.sqlDataRd["GOOD_QTY"].ToString();
                        String rejectQty = ckon.sqlDataRd["REJECT_QTY"].ToString();
                        String whGoodQty = ckon.sqlDataRd["WH_GOOD_QTY"].ToString();
                        String whRejectQty = ckon.sqlDataRd["WH_REJECT_QTY"].ToString();
                        String inv_article_id = ckon.sqlDataRd["_id"].ToString();
                        String dispute = ckon.sqlDataRd["DISPUTE"].ToString();

                        int good = int.Parse(goodQty);
                        int reject = int.Parse(rejectQty);
                        int wh_good = int.Parse(whGoodQty);
                        int wh_reject = int.Parse(whRejectQty);
                        int total_awal = wh_good + wh_reject;

                        if (total_awal == 0)
                        {
                            total = good + reject;
                        }
                        else
                        { total = total_awal; }
                        
                        int dgRows = dgv_inv.Rows.Add();
                        dgv_inv.Rows[dgRows].Cells[0].Value = id;
                        dgv_inv.Rows[dgRows].Cells[1].Value = name;
                        dgv_inv.Rows[dgRows].Cells[2].Value = good;
                        dgv_inv.Rows[dgRows].Cells[3].Value = rejectQty;
                        dgv_inv.Rows[dgRows].Cells[4].Value = whGoodQty;
                        dgv_inv.Rows[dgRows].Cells[5].Value = whRejectQty;
                        dgv_inv.Rows[dgRows].Cells[6].Value = total;
                        dgv_inv.Rows[dgRows].Cells[7].Value = inv_article_id;
                        dgv_inv.Rows[dgRows].Cells[8].Value = dispute;
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
            //    String name = ckon.myReader.GetString("ARTICLE_NAME");
            //    String id = ckon.myReader.GetString("ARTICLE_ID");
            //    String goodQty = ckon.myReader.GetString("GOOD_QTY");
            //    String rejectQty = ckon.myReader.GetString("REJECT_QTY");
            //    String whGoodQty = ckon.myReader.GetString("WH_GOOD_QTY");
            //    String whRejectQty = ckon.myReader.GetString("WH_REJECT_QTY");
            //    String inv_article_id = ckon.myReader.GetString("_id");
            //    String dispute = ckon.myReader.GetString("DISPUTE");

            //    int good = int.Parse(goodQty);
            //    int reject = int.Parse(rejectQty);
            //    int wh_good = int.Parse(whGoodQty);
            //    int wh_reject = int.Parse(whRejectQty);
            //    int total_awal = wh_good + wh_reject;
            //    if(total_awal==0)
            //    {
            //        total = good + reject;
            //    }
            //    else
            //    { total = total_awal; }
            //    //int total = good + reject;
            //    int n = dgv_inv.Rows.Add();
            //    dgv_inv.Rows[n].Cells[0].Value = id;
            //    dgv_inv.Rows[n].Cells[1].Value = name;
            //    dgv_inv.Rows[n].Cells[2].Value = good;
            //    dgv_inv.Rows[n].Cells[3].Value = rejectQty;
            //    dgv_inv.Rows[n].Cells[4].Value = whGoodQty;
            //    dgv_inv.Rows[n].Cells[5].Value = whRejectQty;
            //    dgv_inv.Rows[n].Cells[6].Value = total;
            //    dgv_inv.Rows[n].Cells[7].Value = inv_article_id;
            //    dgv_inv.Rows[n].Cells[8].Value = dispute;

            //}
            //ckon.con.Close();
        }

        private void dgv_inv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("aa
            if (dgv_inv.Columns[e.ColumnIndex].Name == "Column5" || dgv_inv.Columns[e.ColumnIndex].Name == "Column6")
            {
                String id = dgv_inv.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                String name = dgv_inv.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
                String good = dgv_inv.Rows[e.RowIndex].Cells["Column3"].Value.ToString();
                String reject = dgv_inv.Rows[e.RowIndex].Cells["Column4"].Value.ToString();
                String w_good = dgv_inv.Rows[e.RowIndex].Cells["Column5"].Value.ToString();
                String w_reject = dgv_inv.Rows[e.RowIndex].Cells["Column6"].Value.ToString();
                String total = dgv_inv.Rows[e.RowIndex].Cells["Column7"].Value.ToString();
                String inv_article = dgv_inv.Rows[e.RowIndex].Cells["Column8"].Value.ToString();
                w_stock_take stock = new w_stock_take(f1);
                stock.get_data(id, name, good, reject, w_good, w_reject, total, inv_article);


                stock.ShowDialog();
                
                //MessageBox.Show(" "+id+ " "+name);
            }
        }

        private void combo_ktg2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(combo_ktg2.Text == "ALL")
            {
                combo_value.Items.Clear();
                String sql = "SELECT TOP 100 article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID";
               
                combo_value.Text = "ALL";
                retreive(sql);
            }
            else if (combo_ktg2.Text == "Brand")
            {
                String query = "SELECT * FROM brand";
                isi_combo_value(query);
                combo_value.Text = "3 SECOND";
            }
            else if (combo_ktg2.Text == "Department")
            {
                String query = "SELECT * FROM departement";
                isi_combo_value(query);
                combo_value.Text = "Shirt";
            }
            else if (combo_ktg2.Text == "Department_Type")
            {
                String query = "SELECT * FROM departementtype";
                isi_combo_value(query);
                combo_value.Text = "Denim";
            }
            else if (combo_ktg2.Text == "Size")
            {
                String query = "SELECT * FROM Size";
                isi_combo_value(query);
                combo_value.Text = "L";
            }
            else if (combo_ktg2.Text == "Color")
            {
                String query = "SELECT * FROM Color";
                isi_combo_value(query);
                combo_value.Text = "Blue";
            }
            else if (combo_ktg2.Text == "Gender")
            {
                String query = "SELECT * FROM Gender";
                isi_combo_value(query);
                combo_value.Text = "Men";
            }            
        }
        //======================================================================================================
        //========================ISI VALUE COMBO ==============================================
        public void isi_combo_value(String query)
        {
            combo_value.Items.Clear();
            String sql = query;
            ckon.cmd = new MySqlCommand(sql, ckon.con);
            try
            {
                ckon.con.Open();
                ckon.myReader = ckon.cmd.ExecuteReader();
                while (ckon.myReader.Read())
                {
                    String name = ckon.myReader.GetString("DESCRIPTION");
                    combo_value.Items.Add(name);
                }
                ckon.con.Close();

            }
            catch
            { }
        }
        //=============================================================================
        private void combo_value_SelectedIndexChanged(object sender, EventArgs e)
        {
            String sql = "SELECT TOP 100 article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID WHERE article." + combo_ktg2.Text + " = '" + combo_value.Text + "'";
            retreive(sql);           
        }
        //=============================================================================
        private void t_search_trans_OnTextChange(object sender, EventArgs e)
        {
            String count_article = t_search_trans.Text;
            int count_article_int = count_article.Count();
            
            if (t_search_trans.text == "")
            {
                String sql2a = "SELECT TOP 100 article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID";
                retreive(sql2a);
            }
            if (t_search_trans.text == "" && (combo_ktg2.Text == "Brand" || combo_ktg2.Text == "Department" || combo_ktg2.Text == "Department_Type" || combo_ktg2.Text == "Size" || combo_ktg2.Text == "Color" || combo_ktg2.Text == "Gender"))
            {
                String sql4 = "SELECT TOP 100 article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID WHERE article." + combo_ktg2.Text + " = '" + combo_value.Text + "'";
                retreive(sql4);
                ckon.con.Close();
            }
            
            //============================================
            if (t_search_trans.text != "")
            {
                String sql2 = "SELECT TOP 100 article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID WHERE  article.ARTICLE_ID LIKE '%" + t_search_trans.text + "%' OR article.ARTICLE_NAME LIKE '%" + t_search_trans.text + "%'";
                retreive(sql2);
            }
            if (t_search_trans.text != "" && (combo_ktg2.Text == "Brand" || combo_ktg2.Text == "Department" || combo_ktg2.Text == "Department_Type" || combo_ktg2.Text == "Size" || combo_ktg2.Text == "Color" || combo_ktg2.Text == "Gender"))
            {
                String sql3 = "SELECT TOP 100 article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID WHERE article." + combo_ktg2.Text + " = '" + combo_value.Text + "' AND (article.ARTICLE_ID LIKE '%" + t_search_trans.text + "%' OR article.ARTICLE_NAME LIKE '%" + t_search_trans.text + "%' )";
                retreive(sql3);
            }
            
            
            
        }
        //=============================================================================
        private void b_confirm_Click(object sender, EventArgs e)
        {
            W_StockTake_Confirm confirm = new W_StockTake_Confirm();
            confirm.ShowDialog();
         }

        private void b_reset_Click(object sender, EventArgs e)
        {
            from_form1();
            //String sql4 = "SELECT article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID LIMIT 100";
            //retreive(sql4);
        }

        public void delete()
        {
            String cmd_delete = "DELETE FROM stock_take";
            CRUD delete = new CRUD();
            delete.ExecuteNonQuery(cmd_delete);
            dgv_inv.Rows.Clear();
        }
        //METHOD CONFIRM DARI FORM FORM LAEN
        public void method_confirm()
        {
            try
            {
                API_StockTake stock = new API_StockTake();
                stock.id_employe3 = id_employee2;
                stock.nm_employe3 = nm_employee2;
                stock.select_store();
                stock.Post_stockTake().Wait();
                status_sukses = "1";
            }
            catch
            {
                status_sukses = "0";
            }
            if(status_sukses=="1")
            {
                MessageBox.Show("Data Has Been Sent To Backend");
                delete();
            }
            else
            {
                MessageBox.Show("Make Sure You Are Connected To Internet");
            }

        }

        // MENGURUTKAN DATA BERDASARKAN ASCENDING DAN DESCENDING
        private void combo_sorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(combo_sorting.Text== "Ascending")
            {

                dgv_inv.Sort(this.dgv_inv.Columns["Column3"], ListSortDirection.Ascending);
            }
            else
            {
                dgv_inv.Sort(this.dgv_inv.Columns["Column3"], ListSortDirection.Descending);
            }
        }

    }
}
