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
    public partial class W_DeliveryAddress : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();        
        public string articleId, articleName, qty, transactionId, spgId, fromStore, store;
        public bool shippingByTrans;

        public W_DeliveryAddress(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        //form ke load
        private void W_DeliveryAddress_Load(object sender, EventArgs e)
        {          
            if (shippingByTrans)
            {
                l_Item.Visible = false;
                t_ArtName.Visible = false;
                l_Dyn.Text = "Transaction ID";
                t_FromStore.Enabled = true;                
                retreiveByTrans();
            }
            else
            {
                retreiveByTransLine();
            }                          
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }        

        private void b_ok_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();
            int addQty = 1;
            string storeCode = "";
           
            discCalcSP(transactionId, "B001", addQty, spgId, "", storeCode);

            if (shippingByTrans)
            {
                
            }
            else
            {
                string cmd_update = "UPDATE [tmp].[" + store + "] SET DELIVERYCUSTADDRESS = '" + t_DeliveryAddress.Text + "', OMNISHIPPINGCOST = '', OMNICOURIER = '' " +
                                    "WHERE TRANSACTION_ID = '" + transactionId + "' AND ARTICLE_ID = '" + t_ArtId.Text + "' AND OMNISTORECODE = '" + t_FromStore.Text + "'";
                sql.ExecuteNonQuery(cmd_update);
            }

            this.Close();
            uc_coba.Instance.retreive();
            uc_coba.Instance.itung_total();            
        }

        public void get_data(string _artId, string _artName, string _qty, string _transId, string _spgId, string _fromStore, string _storeCode, bool isShippingByTrans)
        {
            articleId = _artId;
            articleName = _artName;
            qty = _qty;
            transactionId = _transId;
            spgId = _spgId;
            fromStore = _fromStore;
            shippingByTrans = isShippingByTrans;
            store = _storeCode;
        }

        public void retreiveByTransLine()
        {
            t_ArtId.Text = articleId;
            t_ArtName.Text = articleName;
            t_Qty.Text = qty;
            t_FromStore.Text = fromStore;
        }

        public void retreiveByTrans()
        {
            t_ArtId.Text = transactionId;
            t_Qty.Text = qty;
            t_FromStore.Text = fromStore;
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
