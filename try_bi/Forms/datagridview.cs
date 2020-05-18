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
using Newtonsoft.Json;

namespace try_bi
{
    public partial class datagridview : Form
    {
        public datagridview()
        {
            InitializeComponent();
        }
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        koneksi3 ckon3 = new koneksi3();

        String code_store, customer, S_ID, disc_code, disc_type, disc_desc, t_id, id_spg;

        private void button1_Click(object sender, EventArgs e)
        {
            cek_article();
            //dataGridView1.Rows.Add("IBNU", "FAUZI");
        }
        
        int subtotal, qty = 1, new_price, new_jumlah, new_harga, new_disc;
        public void cek_article()
        {
            String sql = "SELECT * FROM article";
            ckon3.con3.Open();
            ckon3.cmd3 = new MySqlCommand(sql, ckon3.con3);
            ckon3.myReader3 = ckon3.cmd3.ExecuteReader();
            while(ckon3.myReader3.Read())
            {
                S_ID = ckon3.myReader3.GetString("ARTICLE_ID");
                Post_Discount();
            }
            ckon3.con3.Close();
        }
        public async Task Post_Discount()
        {
            int disc = 0;
            Transaction transaction = new Transaction();
            transaction.storeCode = "AAD";
            transaction.customerId = "";
            List<TransactionLine> transLine = new List<TransactionLine>();
            //==================================================================================
            string query = "SELECT * FROM ARTICLE WHERE ARTICLE_ID='" + S_ID + "'";
            ckon2.cmd2 = new MySqlCommand(query, ckon2.con2);
            ckon2.con2.Open();
            ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            Article articleFromDb = new Article();
            while (ckon2.myReader2.Read())
            {
                articleFromDb.articleId = ckon2.myReader2.GetString("ARTICLE_ID");
                articleFromDb.articleName = ckon2.myReader2.GetString("ARTICLE_NAME");
                articleFromDb.brand = ckon2.myReader2.GetString("BRAND");
                articleFromDb.color = ckon2.myReader2.GetString("COLOR");
                articleFromDb.department = ckon2.myReader2.GetString("DEPARTMENT");
                articleFromDb.departmentType = ckon2.myReader2.GetString("DEPARTMENT_TYPE");
                articleFromDb.gender = ckon2.myReader2.GetString("GENDER");
                articleFromDb.id = ckon2.myReader2.GetInt32("_id");
                articleFromDb.price = ckon2.myReader2.GetInt32("PRICE");
                articleFromDb.size = ckon2.myReader2.GetString("SIZE");
                articleFromDb.unit = ckon2.myReader2.GetString("UNIT");
                subtotal = ckon2.myReader2.GetInt32("PRICE");
                articleFromDb.articleIdAlias = ckon2.myReader2.GetString("ARTICLE_ID_ALIAS");
            }
            ckon2.con2.Close();
            //====================================================================
            TransactionLine t = new TransactionLine();
            t.subtotal = subtotal;
            t.quantity = qty;
            t.discount = 0;
            t.price = subtotal;

            t.article = articleFromDb;
            transLine.Add(t);

            transaction.transactionLines = transLine;
            BiensiPosDbContext.BiensiPosDbDataContext contex = new BiensiPosDbContext.BiensiPosDbDataContext();
            DiscountCalculate dc = new DiscountCalculate(contex);
            DiscountMaster resultData = dc.Post(transaction);
            //=================================================
            try
            {
                foreach (var c in resultData.discountItems)
                {
                    disc = (Int32)c.amountDiscount;
                    disc_code = c.discountCode;
                    disc_type = c.discountType;
                    disc_desc = c.discountDesc;
                    //MessageBox.Show("" + disc.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            String input = "insert into log_discount (ID, DISCOUNT) values ('" + S_ID + "','" + disc + "')";
            CRUD input2 = new CRUD();
            input2.ExecuteNonQuery(input);
        }
        //=======================END OF DISCOUNT=============================================================
    }
}
