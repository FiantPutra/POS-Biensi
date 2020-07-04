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
    
    public partial class W_disc_GetArticle : Form
    {
        String disc_code, disc_type, id_trans, art_id, price, spg_id, id_inv, trans_id, kodetoko3, custid3;
        koneksi ckon = new koneksi();
        public W_disc_GetArticle()
        {
            InitializeComponent();
        }
        //=======FORM KE LOAD====
        private void W_disc_GetArticle_Load(object sender, EventArgs e)
        {
            //retreive();
            this.ActiveControl = t_find_article;
            t_find_article.Focus();
        }
        //==========MENDAPATKAN DISCOUNT CODE DARI TOMBOL USE W_PROMOTION POP_UP
        public void get_disc_code(String id, String type, String trans, String spg)
        {
            disc_code = id;
            disc_type = type;
            id_trans = trans;
            spg_id = spg;
            //label1.Text = disc_code;
        }
        //===========================================================================
        public void getget(String aa, String bb, String cc)
        {
            trans_id = aa;
            kodetoko3 = bb;
            custid3 = cc;
        }
        private void t_find_article_OnTextChange(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            dgv_hold.Rows.Clear();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT discount_item.ARTICLE_ID, discount_item.ARTICLE_NAME,inventory._id FROM discount_item INNER JOIN inventory ON discount_item._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >= 1 AND discount_item.ARTICLE_ID LIKE '%" + t_find_article.text + "%'";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_hold.Rows.Add();
                    dgv_hold.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_hold.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_hold.Rows[dgRows].Cells[2].Value = row["_id"].ToString();
                }
            }
            catch (Exception er)
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
            //ckon.con.Open();
            //ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //ckon.adapter.Fill(ckon.dt);
            //foreach (DataRow row in ckon.dt.Rows)
            //{
            //    int n = dgv_hold.Rows.Add();
            //    dgv_hold.Rows[n].Cells[0].Value = row["ARTICLE_ID"].ToString();
            //    dgv_hold.Rows[n].Cells[1].Value = row["ARTICLE_NAME"].ToString();
            //    dgv_hold.Rows[n].Cells[2].Value = row["_id"].ToString();
            //}
            //ckon.dt.Rows.Clear();
            //ckon.con.Close();
        }

        //========tampilkan data article dari tabel discount item yg sesuai dengan discount code===\
        public void retreive()
        {
            CRUD sql = new CRUD();

            dgv_hold.Rows.Clear();
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT discount_item.ARTICLE_ID, discount_item.ARTICLE_NAME,inventory._id FROM discount_item INNER JOIN inventory ON discount_item._id = inventory.ARTICLE_ID WHERE inventory.GOOD_QTY >= 1";
                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_hold.Rows.Add();
                    dgv_hold.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_hold.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_hold.Rows[dgRows].Cells[2].Value = row["_id"].ToString();
                }
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
            //ckon.con.Open();
            //ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //ckon.adapter.Fill(ckon.dt);
            //foreach(DataRow row in ckon.dt.Rows)
            //{
            //    int n = dgv_hold.Rows.Add();
            //    dgv_hold.Rows[n].Cells[0].Value = row["ARTICLE_ID"].ToString();
            //    dgv_hold.Rows[n].Cells[1].Value = row["ARTICLE_NAME"].ToString();
            //    dgv_hold.Rows[n].Cells[2].Value = row["_id"].ToString();
            //}
            //ckon.con.Close();
        }
        //==================klik data di tabel agar masuk ke transaksi line==============================================
        private void dgv_hold_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            id_inv = dgv_hold.SelectedRows[0].Cells[2].Value.ToString();
            //ambil article id, masukan ke variable
            art_id = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
            //MessageBox.Show(art_id + "");
            //======mencari data article sesuai article code==
            search_data_article();
            //===========masukan ke transaksi line
            insert();
            //===========potong inventory article===
            //MEMOTONG ARTICLE
            Inv_Line inv = new Inv_Line();
            int qty_min_plus = -1;
            String type_trans = "1";
            inv.cek_qty_inv(id_inv);
            inv.cek_type_trans(type_trans);
            inv.cek_inv_line(id_trans, qty_min_plus);
            //==panggil method di uc_coba(halaman transaksi)

            DiscountAfterUsePromNew afteruser = new DiscountAfterUsePromNew();
            afteruser.retreive(trans_id, kodetoko3, custid3);

            //uc_coba.Instance.retreive();
            //uc_coba.Instance.itung_total();
            //====tutup dialog===
            this.Close();
        }
        //=======JIKA DISCOUNT ITEM 1, MAKA LIHAT KE TABEL DISCOUNT ITEM DAN CARI DATANYA, LALU JALANKAN METHO SESUAI YG LAIN====
        public void disc_1item()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM discount_item WHERE DISCOUNT_CODE = '" + disc_code + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
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
            //while(ckon.myReader.Read())
            //{
            //    art_id = ckon.myReader.GetString("ARTICLE_ID");
            //}
            //ckon.con.Close();
        }
        //====CARI DATA ARTICLE SESUAI DENGAN ARTICLE ID YG DIPILIH, AMBIL DATANYA
        public void search_data_article()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM article WHERE ARTICLE_ID = '" + art_id + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        price = ckon.sqlDataRd["PRICE"].ToString();
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
            //while(ckon.myReader.Read())
            //{
            //   price = ckon.myReader.GetString("PRICE");
            //}
            //ckon.con.Close();
        }
        //======MASUKAN DATA KE TRANSAKSI LINE=========
        public void insert()
        {
            String cmd_insert = "INSERT INTO transaction_line (TRANSACTION_ID,ARTICLE_ID,QUANTITY,PRICE,DISCOUNT,SUBTOTAL,SPG_ID,DISCOUNT_CODE,DISCOUNT_TYPE,DISCOUNT_DESC) VALUES ('" + id_trans + "','" + art_id + "','1','" + price + "','" + price + "','0','" + spg_id + "','" + disc_code + "','" + disc_type + "','"+ disc_code +"')";
            CRUD insert = new CRUD();
            insert.ExecuteNonQuery(cmd_insert);
        }
        //===coba method tampilkan pesan
        public void message()
        {
            MessageBox.Show("data ditampilkan dari form sebelum nya");
        }
    }
}
