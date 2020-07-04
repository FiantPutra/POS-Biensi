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
    public partial class w_edit_sales_price : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();

        String idArticle, idTransLine;
        int subtotal, newPrice, totDisc, qty, oldPrice;

        public w_edit_sales_price(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        public void set_data(String article_id, String id_trans, String price, String article_name)
        {
            idArticle = article_id;
            idTransLine = id_trans;
            sls_price.Text = price;
            label1.Text = article_name;
            this.Text = "Edit Price #" + article_id;
        }

        private void sls_price_KeyPress(object sender, KeyPressEventArgs e)
        {
            CRUD sql = new CRUD();

            int sales_price = System.Convert.ToInt32(sls_price.Text.ToString());
            try
            {                
                if (e.KeyChar == (char)Keys.Enter)
                {
                    ckon.sqlCon().Open();
                    string cmd = "SELECT * FROM transaction_line WHERE TRANSACTION_ID ='" + idTransLine + "' AND ARTICLE_ID='" + idArticle + "'";
                    ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            qty = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"].ToString());
                            subtotal = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                            totDisc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                            oldPrice = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        }
                    }

                    if (oldPrice != sales_price)
                    {
                        int total = sales_price * qty;
                        String cmd_update = "UPDATE transaction_line SET PRICE='" + sales_price + "',DISCOUNT=0 ,SUBTOTAL='" + total + "' WHERE TRANSACTION_ID='" + idTransLine + "' AND ARTICLE_ID='" + idArticle + "'";                        
                        sql.ExecuteNonQuery(cmd_update);
                    }

                    //ckon.cmd = new MySqlCommand(sql, ckon.con);
                    //ckon.con.Open();
                    //ckon.myReader = ckon.cmd.ExecuteReader();
                    //int count = 0;
                    //while (ckon.myReader.Read())
                    //{
                    //    count = count + 1;
                    //    qty = ckon.myReader.GetInt32("QUANTITY");
                    //    subtotal = ckon.myReader.GetInt32("SUBTOTAL");
                    //    totDisc = ckon.myReader.GetInt32("DISCOUNT");
                    //    oldPrice = ckon.myReader.GetInt32("PRICE");
                    //}
                    //ckon.con.Close();

                    //if (oldPrice != sales_price)
                    //{
                    //    int total = sales_price * qty;
                    //    String sql3 = "UPDATE transaction_line SET PRICE='" + sales_price + "',DISCOUNT=0 ,SUBTOTAL='" + total + "' WHERE TRANSACTION_ID='" + idTransLine + "' AND ARTICLE_ID='" + idArticle + "'";
                    //    CRUD input = new CRUD();
                    //    input.ExecuteNonQuery(sql3);
                    //}

                    //uc_coba.Instance.itung_total();
                    //uc_coba.Instance.retreive();

                    this.Close();
                }
            } 
            catch
            {

            }
        }

        private void w_edit_sales_price_Load(object sender, EventArgs e)
        {

        }
    }
}
