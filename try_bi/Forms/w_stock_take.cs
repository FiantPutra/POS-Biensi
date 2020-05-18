using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace try_bi
{
    public partial class w_stock_take : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        int dispute, good2;
        String new_good, new_reject, new_id, real_inv_id;
        public w_stock_take(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        public void get_data(String id, String nama, String good, String reject, String w_good, String w_reject, String total, String new_inv_id)
        {
            l_id.Text = id;
            l_nama.Text = nama;
            l_good.Text = good;
            l_reject.Text = reject;
            l_w_good.Text = w_good;
            l_w_reject.Text = w_reject;
            l_total.Text = total;
            new_id = id;
            real_inv_id = new_inv_id;
            good2 = Int32.Parse(good);
        }

        private void b_ok_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();
            //====ambil dari textboxt
            new_good = l_w_good.Text;
            new_reject = l_w_reject.Text;
            //============convert dari textboxt, agar bisa di dapatkan dispute==========
            int new_good2 = Int32.Parse(new_good);
            dispute = new_good2 - good2;
            //===================update ke tabel stock take==============
            String cmd_update = "UPDATE stock_take set WH_GOOD_QTY ='" + new_good + "', WH_REJECT_QTY ='" + new_reject + "',DISPUTE='"+ dispute + "' WHERE ARTICLE_ID='" + real_inv_id + "'";
            sql.ExecuteNonQuery(cmd_update);

            String cmd = "SELECT article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID";            
            UC_Stock_Take.Instance.retreive(cmd);
            this.Close();

            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.cmd.ExecuteNonQuery();
            //    ckon.con.Close();
            //    String sql2 = "SELECT article.ARTICLE_NAME, article.ARTICLE_ID, stock_take.GOOD_QTY, stock_take.REJECT_QTY, stock_take.WH_GOOD_QTY, stock_take.WH_REJECT_QTY, stock_take._id,stock_take.DISPUTE FROM article INNER JOIN stock_take ON article._id = stock_take.ARTICLE_ID";
            //    //MessageBox.Show("update data successfully");
            //    UC_Stock_Take.Instance.retreive(sql2);

            //    this.Close();
            //}
            //catch
            //{
            //    MessageBox.Show("updating failed data");
            //}
        }

        private void w_stock_take_Load(object sender, EventArgs e)
        {

        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void update()
        {
            
        }

    }
}
