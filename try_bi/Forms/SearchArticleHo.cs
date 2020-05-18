using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace try_bi
{
    public partial class SearchArticleHo : Form
    {
        koneksi ckon = new koneksi();
        public String t_id, S_barcode, S_article, S_nama, S_price, S_ID, id_spg, store_code2, id_inv;
        int new_price;

        public SearchArticleHo()
        {
            InitializeComponent();
        }

        //=================FORM KE LOAD========================================
        private void SearchArticle_Load(object sender, EventArgs e)
        {
            this.ActiveControl = t_find_article;
            t_find_article.Focus();
            if (Properties.Settings.Default.OnnOrOff == "Offline")
            {
                //====HIT INVEN LOCAL==========================
                API_Inventory InvLocal = new API_Inventory();
                InvLocal.SetInvHitLocal();
                //==============================================
                //dataGridView1.Columns[0].HeaderCell.Style.ForeColor = Color.Orange;
                dgv_2.EnableHeadersVisualStyles = false;
                String sql = "SELECT TOP 100 * FROM article_ho";
                get_load_data(sql);
                dgv_2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv_2.MultiSelect = true;
            }
            else
            {
                dgv_2.EnableHeadersVisualStyles = false;
                String sql = "SELECT TOP 100 * FROM article_ho";
                get_load_data(sql);
                dgv_2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv_2.MultiSelect = true;
            }
        }

        //==============CLOSE BY KEY DOWN IN FORM =====================================
        private void SearchArticle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "X")
            {
                this.Close();
            }
        }

        //=============TEXTBOX SEARCH CHANGED=====================================
        private void t_find_article_OnTextChange(object sender, EventArgs e)
        {
            String count_article = t_find_article.text;
            int count_article_int = count_article.Count();

            if (t_find_article.text == "")
            {

                String sql2a = "SELECT TOP 100 * FROM article_ho";
                get_load_data(sql2a);

            } else
            {
                String sql2 = "SELECT TOP 100 * FROM article_ho WHERE ARTICLE_ID LIKE '%" + t_find_article.text + "%' OR ARTICLE_NAME LIKE '%" + t_find_article.text + "%'";
                get_load_data(sql2);
            }
        }

        private void dgv_2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                S_ID = dgv_2.SelectedRows[0].Cells[0].Value.ToString();

                S_price = dgv_2.SelectedRows[0].Cells[4].Value.ToString();
                id_inv = dgv_2.SelectedRows[0].Cells[5].Value.ToString();

                back_uc(S_ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        //=================SELECT VALUE BY PRESS ENTER=======================
        private void dgv_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    S_ID = dgv_2.SelectedRows[0].Cells[0].Value.ToString();

                    S_price = dgv_2.SelectedRows[0].Cells[4].Value.ToString();
                    id_inv = dgv_2.SelectedRows[0].Cells[5].Value.ToString();

                    back_uc(S_ID);
                }
                catch
                {
                    MessageBox.Show("Select a value in the table");
                }
            }
        }
        //======================MASUK KO FORM UTAMA=====================================
        public void back_uc(String idArticle="")
        {
            if (idArticle != "")
            {
                UC_Article_In_Out.Instance.to_line(idArticle);
                this.Close();
            }
            this.Close();
        }

        //======================RETREIVE DATA ====================================================
        public void get_load_data(String query)
        {
            dgv_2.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                CRUD sql = new CRUD();
                ckon.dt = sql.ExecuteDataTable(query, ckon.sqlCon());

                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_2.Rows.Add();
                    dgv_2.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_2.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_2.Rows[dgRows].Cells[2].Value = row["DEPARTMENT"].ToString();
                    dgv_2.Rows[dgRows].Cells[3].Value = row["COLOR"].ToString();
                    dgv_2.Rows[dgRows].Cells[4].Value = row["PRICE"];
                    dgv_2.Rows[dgRows].Cells[5].Value = row["_id"];
                }
                dgv_2.Columns[4].DefaultCellStyle.Format = "#,###";
                                
                if (dgv_2.Rows.Count > 1 || dgv_2.Rows.Count < 6)
                {
                    fokus_dgv();
                }
                if (dgv_2.Rows.Count > 5)
                {
                    this.ActiveControl = t_find_article;
                    t_find_article.Focus();
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
            //string sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.adapter = new MySqlDataAdapter(ckon.cmd);
            //    ckon.adapter.Fill(ckon.dt);

            //    foreach (DataRow row in ckon.dt.Rows)
            //    {
            //        int n = dgv_2.Rows.Add();
            //        dgv_2.Rows[n].Cells[0].Value = row["ARTICLE_ID"].ToString();
            //        dgv_2.Rows[n].Cells[1].Value = row["ARTICLE_NAME"].ToString();
            //        dgv_2.Rows[n].Cells[2].Value = row["DEPARTMENT"].ToString();
            //        dgv_2.Rows[n].Cells[3].Value = row["COLOR"].ToString();
            //        dgv_2.Rows[n].Cells[4].Value = row["PRICE"];
            //        dgv_2.Rows[n].Cells[5].Value = row["_id"];
            //    }
            //    dgv_2.Columns[4].DefaultCellStyle.Format = "#,###";
            //    //con.Close();
            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //    if (dgv_2.Rows.Count > 1 || dgv_2.Rows.Count < 6)
            //    {
            //        fokus_dgv();
            //    }
            //    if (dgv_2.Rows.Count > 5)
            //    {
            //        this.ActiveControl = t_find_article;
            //        t_find_article.Focus();

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    ckon.con.Close();
            //}

        }
        //===================================================================================
        //=================MENCOBA MEMINDAHKAN FOKUS KE DATAGRIDVIEW SAAT SUDAH MEMASUKAN 5 ARTICLE
        public void fokus_dgv()
        {
            this.ActiveControl = dgv_2;
            dgv_2.Focus();

        }
    }
}
