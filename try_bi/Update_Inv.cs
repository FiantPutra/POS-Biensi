using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace try_bi
{
    class Update_Inv
    {
        koneksi ckon = new koneksi();
        String inv_id, inv_id2;
        int good_qty, min_satu = 1, qty_total;
        //======================================mengembalikan inventory=============
        int qty_trans_line, qty_total_trans;
        //==================================FOR DO========================
        String do_art_id, do_qty_rec;
        int qty_rec_do;
        public void get_inv(String id)
        {
            CRUD sql = new CRUD();

            inv_id = id;
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM inventory WHERE _id = '" + inv_id + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        good_qty = Convert.ToInt32(ckon.sqlDataRd["GOOD_QTY"].ToString());
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
            //    good_qty = ckon.myReader.GetInt32("GOOD_QTY");
            //}
            //ckon.con.Close();
        }

        public void change_inv()
        {
            qty_total = good_qty - min_satu;
            String cmd_update = "UPDATE inventory SET GOOD_QTY='" + qty_total + "' WHERE _id='" + inv_id + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }
        //=========================================================================================================

        //============get _id from article id, dikirim saat delete transaction line==================
        public void Search_id(String id_artilce, String qty)
        {
            CRUD sql = new CRUD();

            qty_trans_line = Int32.Parse(qty);
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM article WHERE ARTICLE_ID='" + id_artilce + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        inv_id2 = ckon.sqlDataRd["_id"].ToString();
                    }
                }
                get_inv(inv_id2);
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
            //    inv_id2 = ckon.myReader.GetString("_id");
            //}
            //ckon.con.Close();            
        }

        public void return_inv()
        {
            qty_total_trans = qty_trans_line + good_qty;
            String cmd_update = "UPDATE inventory SET GOOD_QTY='" + qty_total_trans + "' WHERE _id='" + inv_id + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }
        //=======================================FOR DELIVERY ORDER===========================================
        public void search_DoLine(String id)
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM deliveryorder_line WHERE DELIVERY_ORDER_ID = '" + id + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        do_art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        do_qty_rec = ckon.sqlDataRd["QTY_RECEIVE"].ToString();
                        qty_rec_do = Int32.Parse(do_qty_rec);
                        get_inv(do_art_id);
                        return_inv_DO();
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
            //    do_art_id = ckon.myReader.GetString("ARTICLE_ID");
            //    do_qty_rec = ckon.myReader.GetString("QTY_RECEIVE");
            //    qty_rec_do = Int32.Parse(do_qty_rec);
            //    get_inv(do_art_id);
            //    return_inv_DO();
            //}
            //ckon.con.Close();

        }
        public void return_inv_DO()
        {
            qty_total_trans = qty_rec_do + good_qty;
            String cmd_update = "UPDATE inventory SET GOOD_QTY='" + qty_total_trans + "' WHERE _id='" + inv_id + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }
    }
}
