using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class UC_Inven_List : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String id, name, status, brand, good, new_status, size, color;
        //======================================================
        private static UC_Inven_List _instance;
        public static UC_Inven_List Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Inven_List(f1);
                return _instance;
            }
        }

        

        //=======================================================
        public UC_Inven_List(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //MEMBUAT FOKUS DI TEXTBOXT SCAN ARTICLE()
        public void scan_fokus()
        {
            this.ActiveControl = t_search_trans;
            t_search_trans.Focus();

        }
        //=====menampilkan data ke datagridview====
        public void retreive(String query)
        {
            CRUD sql = new CRUD();
            dgv_inventory.Rows.Clear();

            //String sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while(ckon.myReader.Read())
            //    {
            //        id = ckon.myReader.GetString("ARTICLE_ID");
            //        name = ckon.myReader.GetString("ARTICLE_NAME");
            //        brand = ckon.myReader.GetString("BRAND");
            //        size = ckon.myReader.GetString("SIZE");
            //        color = ckon.myReader.GetString("COLOR");
            //        status = ckon.myReader.GetString("STATUS");
            //        good = ckon.myReader.GetString("GOOD_QTY");
            //        if(status == "0")
            //        {
            //            new_status = "Available";
            //        }
            //        else
            //        {
            //            new_status = "Not Available";
            //        }
            //        int n = dgv_inventory.Rows.Add();

            //        dgv_inventory.Rows[n].Cells[0].Value = id;
            //        dgv_inventory.Rows[n].Cells[1].Value = name;
            //        dgv_inventory.Rows[n].Cells[2].Value = brand;
            //        dgv_inventory.Rows[n].Cells[3].Value = size;
            //        dgv_inventory.Rows[n].Cells[4].Value = color;
            //        dgv_inventory.Rows[n].Cells[5].Value = new_status;
            //        dgv_inventory.Rows[n].Cells[6].Value = good;
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }

            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        name = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                        brand = ckon.sqlDataRd["BRAND_ID"].ToString();
                        size = ckon.sqlDataRd["SIZE_ID"].ToString();
                        color = ckon.sqlDataRd["COLOR_ID"].ToString();
                        status = ckon.sqlDataRd["STATUS"].ToString();
                        good = ckon.sqlDataRd["GOOD_QTY"].ToString();

                        if (status == "0")
                        {
                            new_status = "Available";
                        }
                        else
                        {
                            new_status = "Not Available";
                        }

                        int dgrows = dgv_inventory.Rows.Add();

                        dgv_inventory.Rows[dgrows].Cells[0].Value = id;
                        dgv_inventory.Rows[dgrows].Cells[1].Value = name;
                        dgv_inventory.Rows[dgrows].Cells[2].Value = brand;
                        dgv_inventory.Rows[dgrows].Cells[3].Value = size;
                        dgv_inventory.Rows[dgrows].Cells[4].Value = color;
                        dgv_inventory.Rows[dgrows].Cells[5].Value = new_status;
                        dgv_inventory.Rows[dgrows].Cells[6].Value = good;
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
        //mencari article id dengan textboxt search
        private void t_search_trans_OnValueChanged(object sender, EventArgs e)
        {
            String count_article = t_search_trans.Text;
            int count_article_int = count_article.Count();
            if (count_article_int < 5)
            {
                if (t_search_trans.Text == "")
                {
                    String sql2a = "SELECT TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.BRAND_ID,article.SIZE_ID, article.COLOR_ID,inventory.STATUS, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID";
                    retreive(sql2a);
                }
                if (t_search_trans.Text == "" && (combo_ktg2.Text == "Brand" || combo_ktg2.Text == "Department" || combo_ktg2.Text == "Department_Type" || combo_ktg2.Text == "Size" || combo_ktg2.Text == "Color" || combo_ktg2.Text == "Gender"))
                {
                    String sql4 = "SELECT TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.BRAND_ID,article.SIZE_ID, article.COLOR_ID,inventory.STATUS, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE article." + combo_ktg2.Text + " = '" + combo_value.Text + "'";
                    retreive(sql4);                    
                }
            }
            if (count_article_int >= 5)
            {
                if (t_search_trans.Text != "")
                {
                    String sql2 = "SELECT TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.BRAND_ID, article.SIZE_ID, article.COLOR_ID,inventory.STATUS, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE  article.ARTICLE_ID LIKE '%" + t_search_trans.Text + "%' OR article.ARTICLE_NAME LIKE '%" + t_search_trans.Text + "%'";
                    retreive(sql2);
                }
                if (t_search_trans.Text != "" && (combo_ktg2.Text == "Brand" || combo_ktg2.Text == "Department" || combo_ktg2.Text == "Department_Type" || combo_ktg2.Text == "Size" || combo_ktg2.Text == "Color" || combo_ktg2.Text == "Gender"))
                {
                    String sql3 = "SELECT TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.BRAND_ID,article.SIZE_ID, article.COLOR_ID,inventory.STATUS, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE article." + combo_ktg2.Text + " = '" + combo_value.Text + "' AND (article.ARTICLE_ID LIKE '%" + t_search_trans.Text + "%' OR article.ARTICLE_NAME LIKE '%" + t_search_trans.Text + "%' )";
                    retreive(sql3);
                }
            }
        }
        //=========================SELECT VALUE COMBOBOX CATEGORY============================
        private void combo_ktg2_SelectedIndexChanged(object sender, EventArgs e)
        {
            scan_fokus();
            if (combo_ktg2.Text == "ALL")
            {
                combo_value.Items.Clear();
                String sql = "SELECT TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.BRAND_ID,article.SIZE_ID, article.COLOR_ID,inventory.STATUS, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID";
                combo_value.Text = "ALL";
                retreive(sql);
                //MENGHITUNG TOTAL, KIRIM KE METHOD HITUNG
                String sql2 = "SELECT SUM(inventory.GOOD_QTY) as total FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID ";
                itung_total(sql2);
            }
            else if(combo_ktg2.Text =="Brand")
            {
                String query = "SELECT * FROM brand";
                isi_combo_value(query);
                combo_value.Text = "MOUTLEY";
            }
            else if(combo_ktg2.Text == "Department")
            {
                String query = "SELECT * FROM departement";
                isi_combo_value(query);
                combo_value.Text = "Shirt";
            }
            else if(combo_ktg2.Text == "Department_Type")
            {
                String query = "SELECT * FROM departementtype";
                isi_combo_value(query);
                combo_value.Text = "Denim";
            }
            else if(combo_ktg2.Text == "Size")
            {
                String query = "SELECT * FROM Size";
                isi_combo_value(query);
                combo_value.Text = "L";
            }
            else if(combo_ktg2.Text == "Color")
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
            else
            { }
        }
        //======================================================================================

        //========================ISI VALUE COMBO ==============================================
        public void isi_combo_value(String query)
        {
            CRUD sql = new CRUD();

            combo_value.Items.Clear();                        

            //String sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while(ckon.myReader.Read())
            //    {
            //        String name = ckon.myReader.GetString("DESCRIPTION");
            //        combo_value.Items.Add(name);
            //    }
            //    ckon.con.Close();

            //}
            //catch
            //{ }

            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        String name = ckon.sqlDataRd["DESCRIPTION"].ToString();
                        combo_value.Items.Add(name);
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

        private void combo_value_SelectedIndexChanged(object sender, EventArgs e)
        {
            scan_fokus();            
            String sql = "SELECT TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.BRAND_ID ,article.SIZE_ID, article.COLOR_ID,inventory.STATUS, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE article." + combo_ktg2.Text +" = '"+ combo_value.Text + "'";
            retreive(sql);
            //MENGHITUNG TOTAL, KIRIM KE METHOD HITUNG
            String sql2 = "SELECT SUM(inventory.GOOD_QTY) as total FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE article." + combo_ktg2.Text + " = '" + combo_value.Text + "' ";
            itung_total(sql2);
        }

        private void UC_Inven_List_Load(object sender, EventArgs e)
        {

        }
        //======================================================================================
        public void itung_total(String query)
        {
            CRUD sql = new CRUD();
            int total;                        

            //ckon.cmd = new MySqlCommand(sql2, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    try
            //    {
            //        total = ckon.myReader.GetInt32("total");
            //        l_total.Text = total.ToString();
            //    }
            //    catch
            //    { l_total.Text = "0"; }
            //}
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        if (ckon.sqlDataRd["total"].ToString() != "")
                        {
                            total = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                            l_total.Text = total.ToString();
                        }
                        else
                        {
                            l_total.Text = "0";
                        }
                    }
                }
                else
                {
                    l_total.Text = "0";
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
        //========MENCOBA MENAMPILKAN ARTICLE DENGAN SCAN BARCODE===============================================
        private void t_search_trans_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                String sql2 = "SELECT article.ARTICLE_ID, article.ARTICLE_NAME, article.BRAND_ID, article.SIZE_ID, article.COLOR_ID,inventory.STATUS, inventory.GOOD_QTY FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID WHERE  article.ARTICLE_ID LIKE '%" + t_search_trans.Text + "%' OR article.ARTICLE_NAME LIKE '%" + t_search_trans.Text + "%' ";
                retreive(sql2);
            }            
        }

    }
}
