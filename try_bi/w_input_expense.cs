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
    public partial class w_input_expense : Form
    {
        public String id_petty, bulan2;
        Double qty, price, total;
        int number_trans;

        //===============================================BUTTON CANCEL ==============================================================
        private void b_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //=========================BUTTON OK ===============================
        private void b_ok_Click(object sender, EventArgs e)
        {
            hitung();
            back_uc();
        }
        //============BACK TO MENU PETTY CASH =================================
        public void back_uc()
        {
            Form1 f1 = new Form1();
            UC_Petty_Cash.Instance.save_petty_header();
            UC_Petty_Cash.Instance.retreive();
            UC_Petty_Cash.Instance.itung_total();
            this.Close();
        }
        //=============================================================================================================================


        public w_input_expense()
        {
            InitializeComponent();
        }
        //==================================================KEY UP T_PRICE FOR SEPARATOR==============================================
        private void t_price_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_price.Text == "")
                {

                }
                else
                {
                    price = Double.Parse(t_price.Text);
                    t_price.Text = string.Format("{0:#,###}", price);
                    t_price.Select(t_price.Text.Length, 0);
                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                t_price.Text = "";
            }
        }

        //============================================================================================================================

        //=====================================================COUNT AND SAVE INTO DATABASE============================================
        public void hitung()
        {
            qty = Double.Parse(t_qty.Text);
            //price = Int32.Parse(t_price.Text);
            total = qty * price;

            String cmd_insert = "INSERT INTO pettycash_line (PETTY_CASH_ID, EXPENSE_NAME, QUANTITY, PRICE, TOTAL) VALUES ('" + id_petty + "', '" + t_exp_name.Text + "', '" + t_qty.Text + "', '" + price + "', '" + total + "')";
            CRUD insert = new CRUD();
            insert.ExecuteNonQuery(cmd_insert);

        }
        //=============================================================================================================================
    }
}
