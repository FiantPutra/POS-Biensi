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
    public partial class w_editQty : Form
    {
        String asal_form;
        String id2,  name2,  size2,  color2,  price2, qty2, order_id2;
        int inv_good_qty, price_int;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();

        private void w_editQty_Load(object sender, EventArgs e)
        {
            this.ActiveControl = t_qty;
            t_qty.Focus();
        }

        private void b_ok_Click(object sender, EventArgs e)
        {
            action();
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public w_editQty()
        {
            InitializeComponent();

        }
        //terima data dari menu sebelumnya
        public void detail(String order_id,String id, String name, String size, String color, String price, String qty)
        {
            order_id2 = order_id; id2 = id; name2 = name; size2 = size; color2 = color; price2 = price; qty2 = qty;
            price_int = Int32.Parse(price);
            l_article_id.Text = id2;
            l_article_name.Text = name2;
            l_size.Text = size2;
            l_color.Text = color2;
            l_price.Text = price2;
            t_qty.Text = qty2;

            l_price.Text = string.Format("{0:#,###}" + ",00", price_int);
        }
        //terima jenis menu(mutasi order, retrurn atau reqquest order)
         public void menu_asal(String asal)
        {
            asal_form = asal;
        }

        public void action()
        {
            if(asal_form == "Req_order")
            {
                try
                {
                    int qty_int = Int32.Parse(t_qty.Text);
                    int total_amount = qty_int * price_int;
                    String cmd_update = "UPDATE requestorder_line SET QUANTITY='" + t_qty.Text + "',SUBTOTAL='"+ total_amount +"' WHERE REQUEST_ORDER_ID = '" + order_id2 + "' AND ARTICLE_ID='"+ id2 +"'";
                    CRUD update = new CRUD();
                    update.ExecuteNonQuery(cmd_update);
                    UC_Req_order.Instance.qty();
                    UC_Req_order.Instance.retreive();
                    this.Close();
                }
                catch(Exception ex)
                { MessageBox.Show(ex.Message); }
                
            }
            if(asal_form == "Mut_order")
            {
                try
                {
                    int qty_int = Int32.Parse(t_qty.Text);
                    int total_amount = qty_int * price_int;
                    int textbox_qty = Int32.Parse(t_qty.Text);
                    if(textbox_qty > inv_good_qty)
                    {
                        MessageBox.Show("Quantity Exceeded");
                    }
                    else
                    {
                        String cmd_update = "UPDATE mutasiorder_line SET QUANTITY='" + t_qty.Text + "',SUBTOTAL='"+ total_amount +"' WHERE MUTASI_ORDER_ID = '" + order_id2 + "' AND ARTICLE_ID='" + id2 + "'";
                        CRUD update = new CRUD();
                        update.ExecuteNonQuery(cmd_update);
                        UC_Mut_order.Instance.qty();
                        UC_Mut_order.Instance.retreive();
                        this.Close();
                    }

                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
            if(asal_form == "Ret_Order")
            {
                try
                {
                    int qty_int = Int32.Parse(t_qty.Text);
                    int total_amount = qty_int * price_int;
                    int textbox_qty = Int32.Parse(t_qty.Text);
                    if (textbox_qty > inv_good_qty)
                    {
                        MessageBox.Show("Quantity Exceeded");
                    }
                    else
                    {
                        String cmd_update = "UPDATE returnorder_line SET QUANTITY='" + t_qty.Text + "',SUBTOTAL='" + total_amount + "' WHERE RETURN_ORDER_ID = '" + order_id2 + "' AND ARTICLE_ID='" + id2 + "'";
                        CRUD update = new CRUD();
                        update.ExecuteNonQuery(cmd_update);
                        UC_Ret_order.Instance.qty();
                        UC_Ret_order.Instance.retreive();
                        this.Close();
                    }

                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }
        //melihat max qty dari article yang dipilih
        public void cek_qty()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT inventory.GOOD_QTY FROM inventory INNER JOIN article "
                                + "ON article._id = inventory.ARTICLE_ID "
                                + "WHERE article.ARTICLE_ID = '" + id2 + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        inv_good_qty = Convert.ToInt32(ckon.sqlDataRd["GOOD_QTY"].ToString());
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
            //    String _id = ckon.myReader.GetString("_id");
            //    //ambil total qty dari tabel inventory
            //    String sql2 = "SELECT * FROM inventory WHERE ARTICLE_ID = '" + _id + "'";
            //    ckon2.cmd2 = new MySqlCommand(sql2, ckon2.con2);
            //    ckon2.con2.Open();
            //    ckon2.myReader2 = ckon2.cmd2.ExecuteReader();
            //    while(ckon2.myReader2.Read())
            //    {
            //        inv_good_qty = ckon2.myReader2.GetInt32("GOOD_QTY");
            //    }
            //    ckon2.con2.Close();
            //}
            //ckon.con.Close();
        }
    }
}
