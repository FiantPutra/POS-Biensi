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
    public partial class w_petty_to_cashier : Form
    {
        double value1;
        public int bg_ToStore2;
        String id_bg;
        DateTime mydate = DateTime.Now;
        koneksi ckon = new koneksi();
        public w_petty_to_cashier()
        {
            InitializeComponent();
        }
        //=================================SEPARATOR TEXTBOXT==============================
        private void t_cash_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_cash.Text == "")
                {

                }
                else
                {
                        value1 = double.Parse(t_cash.Text);
                        t_cash.Text = string.Format("{0:#,###}", value1);
                        t_cash.Select(t_cash.Text.Length, 0);
                    
                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                t_cash.Text = "";
            }
        }
        //==================================================================================================
        private void w_petty_to_cashier_Load(object sender, EventArgs e)
        {
            this.ActiveControl = t_cash;
            t_cash.Focus();

        }
        //==================================================================================================

        //===============================================================
        private void b_ok_Click(object sender, EventArgs e)
        {
            if(value1 > bg_ToStore2)
            {
                MessageBox.Show("jangan melebihi budget dong cuy");
            }
            else
            {
                String cmd_update = "UPDATE store SET BUDGET_TO_CASHIER='" + value1 + "'";
                CRUD UPDATE = new CRUD();
                UPDATE.ExecuteNonQuery(cmd_update);
                UC_Petty_Cash.Instance.get_budget();
                this.Close();
            }

            
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //===============================================================
    }
}
