using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace try_bi
{
    public partial class UC_Article_IO_List : UserControl
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        int amount;
        String id_trans, article_id, jam, id_list, qty2, date2, mutasi_from, toCode;

        private static UC_Article_IO_List _instance;

        public static UC_Article_IO_List Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UC_Article_IO_List(f1);
                return _instance;
            }
        }

        //=======================================================
        public UC_Article_IO_List(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        //===================SELECT DATE =======================================
        private void tanggal_MO_ValueChanged(object sender, EventArgs e)
        {
            String SQL = "SELECT * FROM ho_header WHERE  STATUS='1' AND DATE='" + tanggal_MO.Text + "'";
            holding(SQL);
        }

        //======================LIST HOLD TRANSACTION============================================
        public void holding(String query)
        {
            CRUD sql = new CRUD();            

            dgv_hold.Rows.Clear();
            try
            {
                ckon.sqlCon().Open();
                ckon.sqlDataRd = sql.ExecuteDataReader(query, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_trans = ckon.sqlDataRd["MUTASI_ORDER_ID"].ToString();
                        jam = ckon.sqlDataRd["TIME"].ToString();
                        string date = ckon.sqlDataRd["DATE"].ToString();

                        int dgRows = dgv_hold.Rows.Add();
                        dgv_hold.Rows[dgRows].Cells[0].Value = id_trans;
                        dgv_hold.Rows[dgRows].Cells[1].Value = date;
                        dgv_hold.Rows[dgRows].Cells[2].Value = jam;
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

            //koneksi2 ckon2 = new koneksi2();
            //ckon.con.Close();
            //String sql = query;
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //List<string> numbersList = new List<string>();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        id_trans = ckon.myReader.GetString("MUTASI_ORDER_ID");
            //        jam = ckon.myReader.GetString("TIME");
            //        string date = ckon.myReader.GetString("DATE");

            //        int n = dgv_hold.Rows.Add();
            //        dgv_hold.Rows[n].Cells[0].Value = id_trans;
            //        dgv_hold.Rows[n].Cells[1].Value = date;
            //        dgv_hold.Rows[n].Cells[2].Value = jam;
            //        ckon2.con2.Close();
            //    }
            //}
            //ckon.con.Close();
        }

        //================================TEXTBOX SEARCH===================================================
        private void t_search_trans_OnTextChange(object sender, EventArgs e)
        {
            if (t_search_trans.text == "")
            {
                String sql = "SELECT * FROM ho_header WHERE  STATUS='1' AND DATE='" + tanggal_MO.Text + "'";
                holding(sql);
            }
            else
            {
                String sql = "SELECT * FROM ho_header WHERE  STATUS='1' AND MUTASI_ORDER_ID LIKE '%" + t_search_trans.text + "%' AND DATE='" + tanggal_MO.Text + "'";
                holding(sql);
            }
        }

        private void dev_request_Sorting(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        private void print_do_Click(object sender, EventArgs e)
        {
            if (l_transaksi.Text.Length <= 5)
            {
                MessageBox.Show("No data found");
            } else
            {
                HO_Print ho = new HO_Print();
                ho.set_print(l_transaksi.Text);
            }
        }

        private void UC_Article_IO_List_Load(object sender, EventArgs e)
        {
            dgv_request.Rows.Clear();
            l_transaksi.Text = "-";
            l_from.Text = "-";
            l_to.Text = "-";
            l_qty.Text = "0";
            l_date.Text = "-";
            amount_txt.Text = "0";
            print_do.Enabled = false;
        }

        private void dgv_request_FilterStringChanged(object sender, EventArgs e)
        {
            var sort = dgv_request.SortString.Replace("[", "").Replace("]", "");
            var filter = dgv_request.FilterString.Replace("Convert([", "").Replace("],System.String)", "");//.Replace('"', ' ');
            retreive(sort, filter);
        }

        //====================GET DATA FROM ID =======================================================
        public void get_data()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd_hoHeader = "SELECT * FROM ho_header WHERE MUTASI_ORDER_ID ='" + l_transaksi.Text + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_hoHeader, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        mutasi_from = ckon.sqlDataRd["MUTASI_FROM_WAREHOUSE"].ToString();
                        toCode = ckon.sqlDataRd["MUTASI_TO_WAREHOUSE"].ToString();
                        l_from.Text = ckon.sqlDataRd["TRANS_TYPE_CODE"].ToString() + " (From: " + mutasi_from + " To: " + toCode + ")";
                        date2 = ckon.sqlDataRd["REQUEST_DELIVERY_DATE"].ToString();
                        qty2 = ckon.sqlDataRd["TOTAL_QTY"].ToString();
                        amount = Convert.ToInt32(ckon.sqlDataRd["TOTAL_AMOUNT"].ToString());
                        l_ridn.Text = ckon.sqlDataRd["RIDN"].ToString();                        
                    }

                    String cmd_storeRelasi = "SELECT TOP 1 NAME FROM store_relasi WHERE CODE='" + toCode + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd_storeRelasi, ckon.sqlCon());
                    
                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            l_to.Text = toCode + " - " + ckon.sqlDataRd["NAME"].ToString();
                        }
                    }

                    l_date.Text = date2;
                    l_qty.Text = qty2;                    
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

            amount_txt.Text = string.Format("{0:#,###}" + ",00", amount);
            //ckon.con.Close();
            //String sql = "SELECT * FROM ho_header WHERE MUTASI_ORDER_ID ='" + l_transaksi.Text + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        mutasi_from = ckon.myReader.GetString("MUTASI_FROM_WAREHOUSE");
            //        toCode = ckon.myReader.GetString("MUTASI_TO_WAREHOUSE");
            //        l_from.Text = ckon.myReader.GetString("TRANS_TYPE_CODE") + " (From: " + mutasi_from + " To: " + toCode + ")";
            //        date2 = ckon.myReader.GetString("REQUEST_DELIVERY_DATE");
            //        qty2 = ckon.myReader.GetString("TOTAL_QTY");
            //        amount = ckon.myReader.GetInt32("TOTAL_AMOUNT");
            //        l_ridn.Text = ckon.myReader.GetString("RIDN");
            //        l_to.Text = toCode;
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }

            //String sql_to = "SELECT NAME FROM store_relasi WHERE CODE='"+ toCode +"' LIMIT 1";
            //ckon.cmd = new MySqlCommand(sql_to, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        l_to.Text = toCode + " - " + ckon.myReader.GetString("NAME");
            //    }
            //}
            //catch { }            
        }
        //============================================================================================
        //wtu
        private string GetStore(string code)
        {
            CRUD sql = new CRUD();
            String store = "-";
            if (code == "" || code == "-")
            {
                return store;
            }

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT NAME FROM store_relasi WHERE CODE='" + code + "' UNION SELECT NAME FROM store WHERE CODE='" + code + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        store = ckon.sqlDataRd["NAME"].ToString();
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
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        store = ckon.myReader.GetString("NAME");
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }

            return store;
        }

        //=====================ITEM MUTASI  ORDER ==========================================
        public void retreive(string sort = "", string filter = "", string search = "")
        {
            CRUD sql = new CRUD();
            //ckon.con.Close();
            dgv_request.Rows.Clear();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT DISTINCT ho_line.ARTICLE_ID, ho_line.QUANTITY, ho_line.UNIT, ho_line.SUBTOTAL, ho_line.ARTICLE_NAME, article_ho.SIZE, ho_line.COLOR, ho_line.PRICE, ho_line.DEPARTMENT FROM ho_line LEFT JOIN article_ho ON ho_line.`ARTICLE_ID`=article_ho.`ARTICLE_ID` WHERE ho_line.MUTASI_ORDER_ID='" + l_transaksi.Text + "' ";
                if (filter != "")
                {
                    filter = filter.Replace("ARTICLE_ID", "ho_line.ARTICLE_ID");
                    cmd += " AND " + filter;
                }
                if (sort != "")
                {
                    cmd += " ORDER BY " + sort;
                }
                else
                {
                    cmd += " ORDER BY ho_line._id DESC";
                }

                ckon.dt = sql.ExecuteDataTable(cmd, ckon.sqlCon());
                foreach (DataRow row in ckon.dt.Rows)
                {
                    int dgRows = dgv_request.Rows.Add();
                    dgv_request.Rows[dgRows].Cells[0].Value = row["ARTICLE_ID"].ToString();
                    dgv_request.Rows[dgRows].Cells[1].Value = row["ARTICLE_NAME"].ToString();
                    dgv_request.Rows[dgRows].Cells[2].Value = row["COLOR"].ToString();
                    dgv_request.Rows[dgRows].Cells[3].Value = row["QUANTITY"].ToString();
                    dgv_request.Rows[dgRows].Cells[4].Value = row["PRICE"];
                    dgv_request.Rows[dgRows].Cells[5].Value = row["SUBTOTAL"];
                    dgv_request.Rows[dgRows].Cells[6].Value = row["DEPARTMENT"].ToString();
                }
                dgv_request.Columns[4].DefaultCellStyle.Format = "#,###";
                dgv_request.Columns[5].DefaultCellStyle.Format = "#,###";
                dgv_request.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_request.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv_request.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
            //        int n = dgv_request.Rows.Add();
            //        dgv_request.Rows[n].Cells[0].Value = row["ARTICLE_ID"].ToString();
            //        dgv_request.Rows[n].Cells[1].Value = row["ARTICLE_NAME"].ToString();
            //        dgv_request.Rows[n].Cells[2].Value = row["COLOR"].ToString();
            //        dgv_request.Rows[n].Cells[3].Value = row["QUANTITY"].ToString();
            //        dgv_request.Rows[n].Cells[4].Value = row["PRICE"];
            //        dgv_request.Rows[n].Cells[5].Value = row["SUBTOTAL"];
            //        dgv_request.Rows[n].Cells[6].Value = row["DEPARTMENT"].ToString();
            //    }
            //    dgv_request.Columns[4].DefaultCellStyle.Format = "#,###";
            //    dgv_request.Columns[5].DefaultCellStyle.Format = "#,###";
            //    dgv_request.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    dgv_request.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    dgv_request.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //    ckon.dt.Rows.Clear();
            //    ckon.con.Close();
            //}
            //catch
            //{ }
        }
        //==================================================================================


        //===================== KLIK HOLD MASUK KE KANAN===========================
        private void dgv_hold_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dgv_hold.Rows.Count > 0)
                {
                    id_list = dgv_hold.SelectedRows[0].Cells[0].Value.ToString();
                    l_transaksi.Text = id_list;
                    get_data();
                    retreive();
                    print_do.Enabled = true;
                }
            }
            catch (Exception)
            {

            }
        }

        //======================================================================
        private void b_back_PC_Click(object sender, EventArgs e)
        {
            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(UC_Article_In_Out.Instance))
            {
                f1.p_kanan.Controls.Add(UC_Article_In_Out.Instance);
                UC_Article_In_Out.Instance.Dock = DockStyle.Fill;
                UC_Article_In_Out.Instance.BringToFront();
                UC_Article_In_Out.Instance.setFocus();
            }
            else
            {
                UC_Article_In_Out.Instance.BringToFront();
                UC_Article_In_Out.Instance.setFocus();
            }
        }
        //===========================================================================
    }
}
