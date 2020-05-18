using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.Serialization.Json;
using try_bi.Class;
using System.Globalization;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class W_SearchStock : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();        
        public string articleId, articleName, price, transactionId, spgId, store;
        public W_SearchStock(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        //form ke load
        private void W_SearchStock_Load(object sender, EventArgs e)
        {
            t_ArticleId.Text = articleId;
            t_ArticleName.Text = articleName;
            t_Price.Text = price;

            retreive();                 
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv_SearchStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_SearchStock.Columns[e.ColumnIndex].Name == "CbSelected")
            {
                if (dgv_SearchStock.Rows[e.RowIndex].Cells["Qty"].Value.ToString() != "0")
                {
                    if (Convert.ToInt32(dgv_SearchStock.Rows[e.RowIndex].Cells["Qty"].Value) > Convert.ToInt32(dgv_SearchStock.Rows[e.RowIndex].Cells["OnHand"].Value))
                    {
                        dgv_SearchStock.Rows[e.RowIndex].Cells["CbSelected"].Value = false;
                        MessageBox.Show("Quantity must be less than on hand quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                    }   
                    else
                    {
                        if (dgv_SearchStock.Rows[e.RowIndex].Cells["CbSelected"].Value == null)
                            dgv_SearchStock.Rows[e.RowIndex].Cells["CbSelected"].Value = false;
                        switch (dgv_SearchStock.Rows[e.RowIndex].Cells["CbSelected"].Value.ToString())
                        {
                            case "True":
                                dgv_SearchStock.Rows[e.RowIndex].Cells["CbSelected"].Value = false;
                                break;
                            case "False":
                                dgv_SearchStock.Rows[e.RowIndex].Cells["CbSelected"].Value = true;
                                break;
                        }
                    }
                }
                else
                {
                    dgv_SearchStock.Rows[e.RowIndex].Cells["CbSelected"].Value = false;
                    MessageBox.Show("Please input the quantity first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }                       
        }

        private void b_ok_Click(object sender, EventArgs e)
        {
            int addQty = 0;
            string storeCode = "";

            foreach (DataGridViewRow row in dgv_SearchStock.Rows)
            {
                if (Convert.ToBoolean(row.Cells["CbSelected"].Value) == true)
                {
                    addQty = Convert.ToInt32(row.Cells["Qty"].Value);
                    
                    if (storeCode !=  Convert.ToString(row.Cells["Store"].Value).Substring(0,3))
                    {
                        storeCode = Convert.ToString(row.Cells["Store"].Value).Substring(0, 3);

                        discCalcSP(transactionId, articleId, addQty, spgId, "", storeCode);
                    }
                }
            }

            if (addQty == 0)
            {
                MessageBox.Show("Quantity has not been marked, please mark it first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                CRUD sql = new CRUD();
                String cmd_delete = "DELETE FROM [tmp].[" + store + "] WHERE TRANSACTION_ID='" + transactionId + "' AND ARTICLE_ID='" + articleId + "' AND OMNISTORECODE = ''";
                sql.ExecuteNonQuery(cmd_delete);

                this.Close();
                uc_coba.Instance.retreive();
                uc_coba.Instance.itung_total();
            }
        }

        public void get_data(string _artId, string _artName, string _artPrice, string _transId, string _spgId, string storeCode)
        {
            articleId = _artId;
            articleName = _artName;
            price = _artPrice;
            transactionId = _transId;
            spgId = _spgId;
            store = storeCode;
        }

        public void retreive()
        {
            API_OmniStock stock = new API_OmniStock();
            List<OmniStock> stockList = new List<OmniStock>();
            CultureInfo info = CultureInfo.GetCultureInfo("en-ID");


            dgv_SearchStock.Rows.Clear();
            try
            {
                stockList = stock.getOmniStock(articleId);

                for (int i = 0; i < stockList.Count; i++)
                {
                    int dgRows = dgv_SearchStock.Rows.Add();
                    dgv_SearchStock.Rows[dgRows].Cells[0].Value = stockList[i].storeCode + " - " + stockList[i].city;
                    dgv_SearchStock.Rows[dgRows].Cells[1].Value = stockList[i].qty;
                    dgv_SearchStock.Rows[dgRows].Cells[2].Value = "0";
                    dgv_SearchStock.Rows[dgRows].Cells[3].Value = string.Format(new CultureInfo("id-ID"), "{0:c} ", stockList[i].minOngkir) + " - " + string.Format(new CultureInfo("id-ID"), "{0:c} ", stockList[i].maxOngkir);                    
                }                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        public void discCalcSP(String transId, String articleId, int qty, String spgId, String discountCode, String omniStorCode, int is_Service = 0, int is_OmniTrans = 1)
        {
            CRUD sql = new CRUD();

            try
            {
                string command = "SELECT ARTICLE_NAME FROM ARTICLE WHERE ARTICLE_ID='" + articleId + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        articleName = ckon.sqlDataRd["ARTICLE_NAME"].ToString();
                    }
                }

                using (SqlConnection con = new SqlConnection(ckon.sqlCon().ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertTransaction", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@TRANSACTION_ID", SqlDbType.VarChar).Value = transId;
                        cmd.Parameters.Add("@ARTICLE_ID", SqlDbType.VarChar).Value = articleId;
                        cmd.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = qty;
                        cmd.Parameters.Add("@SPG_ID", SqlDbType.VarChar).Value = spgId;
                        cmd.Parameters.Add("@DISCOUNT_CODE", SqlDbType.VarChar).Value = discountCode;
                        cmd.Parameters.Add("@IS_SERVICE", SqlDbType.VarChar).Value = is_Service;
                        cmd.Parameters.Add("@IS_OMNITRANS", SqlDbType.VarChar).Value = is_OmniTrans;
                        cmd.Parameters.Add("@OMNISTORECODE", SqlDbType.VarChar).Value = omniStorCode;

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
    }
}
