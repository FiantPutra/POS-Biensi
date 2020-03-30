using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace try_bi
{
    public partial class W_DO_Confirm : Form
    {
        koneksi ckon = new koneksi();
        String id2, id_epy2, name_epy2, cust_id_store;
        public W_DO_Confirm()
        {
            InitializeComponent();
        }
        //MENDAPATKAN DATA DARI FORM SEBELUM NYA
        public void simpan_do_header(String id, String id_epy, String name_epy)
        {
            id2 = id;
            id_epy2 = id_epy;
            name_epy2 = name_epy;
            //l_id_do.Text = id2;
        }
        //melihat cust_id_store dari store
        public void store()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        cust_id_store = ckon.sqlDataRd["CUST_ID_STORE"].ToString();
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

            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while(ckon.myReader.Read())
            //{
            //    cust_id_store = ckon.myReader.GetString("CUST_ID_STORE");
            //}
            //ckon.con.Close();
        }
        //MENGHITUNG TOTAL DISPUTE DARI ID DO YANG DIKIRIM
        public void count_dispute(String id)
        {                        
            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    //l_total_dispute.Text = ckon.myReader.GetString("total");
            //}
            //ckon.con.Close();
        }
        //menampilkan data dari do line yang mengandung dispute
        public void retreive()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            dgv_do.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT deliveryorder_line.QTY_DISPUTE, article.ARTICLE_ID FROM article INNER JOIN deliveryorder_line ON article.ARTICLE_ID = deliveryorder_line.ARTICLE_ID WHERE deliveryorder_line.DELIVERY_ORDER_ID = '" + id2 + "' AND (deliveryorder_line.QTY_DISPUTE < 0 OR deliveryorder_line.QTY_DISPUTE > 0)";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_do.Rows.Add();
                    dgv_do.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"];
                    dgv_do.Rows[dgRows].Cells[1].Value = row["QTY_DISPUTE"];
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
            //ckon.con.Open();
            //ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //ckon.adapter.Fill(ckon.dt);
            //foreach(DataRow row in ckon.dt.Rows)
            //{
            //    int n = dgv_do.Rows.Add();
            //    dgv_do.Rows[n].Cells[0].Value = row["ARTICLE_ID"];
            //    dgv_do.Rows[n].Cells[1].Value = row["QTY_DISPUTE"];
            //}
            //ckon.dt.Rows.Clear();
            //ckon.con.Close();

        }
        //FUNGSI SIMPAN DO HEADER====
        public void simpan_do_header2()
        {
            API_DeliveryOrderConfirm do_confirm = new API_DeliveryOrderConfirm();

            try
            {
                Inv_Line inv = new Inv_Line();
                String type_trans = "2";
                inv.cek_type_trans(type_trans);
                inv.get_do_line(id2);
                do_confirm.deliveryOrderConfirm(id2, id_epy2, name_epy2, cust_id_store);                
                UC_DO.Instance.reset();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        } 
        //BUTTON CLOSE
        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //button ok
        private void b_ok_Click(object sender, EventArgs e)
        {
            simpan_do_header2();
        }
    }
}
