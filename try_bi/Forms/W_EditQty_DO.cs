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
    public partial class W_EditQty_DO : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String id_do_line, id_do_header, qty_rec;

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void b_ok_Click(object sender, EventArgs e)
        {
            int dev = int.Parse(l_w_dev.Text);
            int rec = int.Parse(l_w_rec.Text);
            int total = rec - dev;
           try
           {
                String cmd_update = "UPDATE deliveryorder_line set QTY_RECEIVE ='" + l_w_rec.Text + "', QTY_DISPUTE = '"+ total +"' WHERE _id='" + id_do_line + "' AND DELIVERY_ORDER_ID = '"+ id_do_header + "'";
                CRUD update = new CRUD();
                update.ExecuteNonQuery(cmd_update);
            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
           
                //kon.cmd.ExecuteNonQuery();
                //ckon.con.Close();

                UC_DO.Instance.retreive2(id_do_header);
                UC_DO.Instance.get_data(id_do_header);

                this.Close();
            }
            catch
            {
                MessageBox.Show("updating failed data");
            }

        }

        private void W_EditQty_DO_Load(object sender, EventArgs e)
        {
            this.ActiveControl = l_w_rec;
            l_w_rec.Focus();

        }

        public W_EditQty_DO(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }

        //======================================================================
        public void get_data(String id, String item, String qty_dev, String qty_rec, String id_do)
        {
            id_do_header = id_do;
            id_do_line = id;
            l_nama.Text = item;
            l_w_dev.Text = qty_dev;
            l_w_rec.Text = qty_rec;

            //label2.Text = id;
            //label3.Text = id_do;
        }
        //======================================================================
    }
}
