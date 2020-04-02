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
using System.Globalization;

namespace try_bi
{
    public partial class w_form_closing : Form
    {
        public static Form1 f1;
        
        koneksi ckon = new koneksi();
        int change, cash, cash2, bg_ToCasir;
        Double value1, value_budget, cash_label, petty_label, value_modal, deposit_label;
        String id_modal_store, query, real_trans_balance, real_petty_cash, real_dispute, cek_closing_shift, epy_id2, epy_name2;
        public string id_cStrore2, date_closing_store3, status_sukses;
        int PANTEK_DEPOSIT = 0, qty_article;
        public w_form_closing(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //====================================================================
        private void w_form_closing_Load(object sender, EventArgs e)
        {
            //this.ActiveControl = textBox1;
            //textBox1.Focus();
             query = "SELECT * FROM [transaction] WHERE STATUS='1' AND IS_CLOSE='0'";
            l_tgl.Text =date_closing_store3;
            cek_status_shift();
            get_last_close_store();
            itung_cash();
            get_budget();
        }
        //========================GET NAME EMPLOYEE AND ID===============
        public void set_name2(String id, String name)
        {
            epy_id2 = id;
            epy_name2 = name;
        }
        //============GET TOTAL QTY===============
        public void get_qty(int qty)
        {
            qty_article = qty;
        }
        //==================================================
        private void t_deposite_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_deposite.Text == "")
                {

                }
                else
                {
                    value_modal = double.Parse(t_deposite.Text);
                    if (value_modal == 0)
                    { t_deposite.Text = "0,00"; }
                    else
                    {
                        t_deposite.Text = string.Format("{0:#,###}", value_modal);
                        t_deposite.Select(t_deposite.Text.Length, 0);
                    }

                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                t_deposite.Text = "";
            }
        }
        private void t_deposite_Leave(object sender, EventArgs e)
        {
            deposit_label = System.Convert.ToDouble(PANTEK_DEPOSIT);
            deposit_label = PANTEK_DEPOSIT - value_modal;
            if (deposit_label == 0)
            {
                //l_cash_dispute.Text = "0,00";
                //t_cash.Text = "0,00";
                //l_cash_dispute.Text = string.Format("{0:#,###}" + ",00", cash2);
                t_deposite.Text = string.Format("{0:#,###}", deposit_label);
                l_dispute_deposite.Text = "0,00";
            }
            else
            {
                l_dispute_deposite.Text = String.Format("{0:#,###}" + ",00", deposit_label);
            }
        }
        //==================ambil data closing shift dari tabel shift==================
        public void cek_status_shift()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM closing_shift  ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        cek_closing_shift = ckon.sqlDataRd["STATUS_CLOSE"].ToString();
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
        }

        //=================ambil data terakhir dari table closing store=================
        public void get_last_close_store()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM closing_store WHERE STATUS_CLOSE='1' ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        real_trans_balance = ckon.sqlDataRd["REAL_TRANS_BALANCE"].ToString();
                        real_petty_cash = ckon.sqlDataRd["PETTY_CASH"].ToString();
                        real_dispute = ckon.sqlDataRd["REAL_DEPOSIT"].ToString();
                    }
                }
                else
                {
                    real_trans_balance = "0"; 
                    real_petty_cash = "0"; 
                    real_dispute = "0";
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
        }


        //===================================BUTTON oke===================================
        private void b_ok_Click(object sender, EventArgs e)
        {
            Boolean api_response;

            try
            {
                if (cek_closing_shift == "0")
                {
                    MessageBox.Show("Please Closing Shift First Before Closing Store");
                }
                else
                {
                    update_close();

                    //===========API CLOSING STORE============                    
                    API_Closing_Store closing_Store = new API_Closing_Store();
                    closing_Store.get_code(id_cStrore2);
                    api_response = closing_Store.Post_Closing_Store().Result;                    
                   
                    if (api_response)
                    {
                        //========for logout========
                        UC_Closing_Store.Instance.reset();
                        f1.Hide();
                        this.Hide();
                        Form_Login login = new Form_Login();
                        login.ShowDialog();
                        f1.Close();
                        this.Close();
                    }
                    else
                    {
                        String cmd_update = "UPDATE closing_store SET STATUS_CLOSE='0' WHERE ID_C_STORE='" + id_cStrore2 + "'";
                        CRUD update = new CRUD();
                        update.ExecuteNonQuery(cmd_update);

                        MessageBox.Show("Make Sure You are Connected To Internet");
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }            
        }
        //====================button close=========================
        private void b_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //======================UPDATE TRANS(IS_CLOSING 0)=============================
        public void update_trans()
        {
            ckon.con.Close();
            String cmd_update = "UPDATE [transaction] SET IS_CLOSE='1' WHERE IS_CLOSE='0'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }
       //=============================================================================

        //==================UPDATE TABLE MODAL_STORE==============================
        public void update_Mstore()
        {
            CRUD sql = new CRUD();

            DateTime mydate = DateTime.Now;
            String time_now = mydate.ToString("yyyy-MM-dd H:mm:ss");
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM modal_store ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_modal_store = ckon.sqlDataRd["_id"].ToString();
                    }
                }

                String cmd_update = "UPDATE modal_store SET CLOSING_BALANCE_TRANS='" + value1 + "', OPENING_BALANCE_PETTY_CASH='" + bg_ToCasir + "', CLOSING_BALANCE_PETTY_CASH='" + value_budget + "', CLOSING_TIME='" + time_now + "' WHERE _id='" + id_modal_store + "'";                
                sql.ExecuteNonQuery(cmd_update);
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
        }
        //===============================UPDATE TABLE CLOSING STORE=========================================
        public void update_close()
        {
            DateTime mydate = DateTime.Now;
            String time_now = mydate.ToString("yyyy-MM-dd H:mm:ss");
            String cmd_update = "UPDATE closing_store SET CLOSING_TIME='" + time_now + "', OPENING_TRANS_BALANCE='"+ real_trans_balance +"' ,CLOSING_TRANS_BALANCE='" + cash2 + "', REAL_TRANS_BALANCE='" + value1 + "', DISPUTE_TRANS_BALANCE='" + cash_label + "', OPENING_PETTY_CASH='"+ real_petty_cash +"' ,CLOSING_PETTY_CASH='" + bg_ToCasir + "',REAL_PETTY_CASH='" + value_budget + "', DISPUTE_PETTY_CASH='" + petty_label + "',CLOSING_DEPOSIT='0',REAL_DEPOSIT='" + value_modal + "',DISPUTE_DEPOSIT='" + deposit_label + "',TOTAL_QTY='"+ qty_article +"' ,STATUS_CLOSE='1', EMPLOYEE_ID='"+epy_id2+"', EMPLOYEE_NAME='"+epy_name2+"' WHERE ID_C_STORE='" + id_cStrore2 + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }

        //=================================MENGHITUNG TOTAL CASH================================================================
        public void itung_cash()
        {
            CRUD sql = new CRUD();
            DateTime mydate = DateTime.Now;
            String date =  mydate.ToString("yyyy-MM-dd");

            try
            {
                ckon.sqlCon().Open();
                String cmd_transTotal = "SELECT SUM([transaction].CASH) as total FROM [transaction] WHERE ID_C_STORE = '" + id_cStrore2 + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transTotal, ckon.sqlCon());
                
                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        cash = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                    }
                }
                else
                {
                    cash = 0;
                }

                String cmd_transChange = "SELECT SUM([transaction].CHANGEE) as total FROM [transaction] WHERE ID_C_STORE = '" + id_cStrore2 + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transChange, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        change = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                    }
                }
                else
                {
                    change = 0;
                }

                cash2 = cash - change;
                if (cash2 <= 0)
                {
                    l_cash.Text = "0,00";
                    t_cash.Text = "0,00";
                }
                else
                {
                    l_cash.Text = string.Format("{0:#,###}" + ",00", cash2);
                    t_cash.Text = string.Format("{0:#,###}", cash2);
                    value1 = cash2;
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
        }
        //==================================GET BUDGET STORE==================================================
        public void get_budget()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM store ";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        bg_ToCasir = Convert.ToInt32(ckon.sqlDataRd["BUDGET_TO_STORE"].ToString());                        
                        value_budget = bg_ToCasir;

                        if (bg_ToCasir == 0)
                        {
                            l_petty.Text = "0,00";
                            t_petty.Text = "0";
                        }
                        else
                        {
                            l_petty.Text = String.Format("{0:#,###}" + ",00", bg_ToCasir);
                            t_petty.Text = String.Format("{0:#,###}", bg_ToCasir);
                        }
                    }
                }
                else
                {
                    bg_ToCasir = 0;
                    l_petty.Text = "0,00";
                    t_petty.Text = "0,00";
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
        }
        //====================================================================================================
        //==============================SEPARATOR FOR TEXTBOX CASH=============================================
        private void t_cash_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_cash.Text == "")
                {
                    //t_cash.Text = string.Format("{0:#,###}", cash2);
                    //t_cash.Select(t_cash.Text.Length, 0);
                    //l_cash_dispute.Text = "0,00";
                    //COBA FUNGSI AGAAR SAAT T_CASH KOSONG, PUNYA KESEMPATAN INPUT NILAI BEBAS, TIDAK LANGSUNG DI SET KE HARGA CASH
                    value1 = 0; cash_label = System.Convert.ToDouble(cash2);
                    cash_label = value1 - cash_label;
                    l_cash_dispute.Text = String.Format("{0:#,###}" + ",00", cash_label);
                }
                else
                {
                    value1 = double.Parse(t_cash.Text);
                    t_cash.Text = string.Format("{0:#,###}", value1);
                    t_cash.Select(t_cash.Text.Length, 0);
                    //====kalkulasi otomatis tanpa perlu pindah textboxt===
                    cash_label = System.Convert.ToDouble(cash2);
                    cash_label = value1 - cash_label;
                    if (cash_label == 0)
                    {
                        t_cash.Text = string.Format("{0:#,###}", cash2);
                        l_cash_dispute.Text = "0,00";
                    }
                    else
                    {
                        l_cash_dispute.Text = String.Format("{0:#,###}" + ",00", cash_label);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                t_cash.Text = "";
            }
        }

        //==========================FOCUS MENINGGALKAN TEXTBOX CASH=======================================================
        private void t_cash_Leave(object sender, EventArgs e)
        {
            //cash_label = System.Convert.ToDouble(cash2);
            //cash_label = value1 - cash_label;
            //if(cash_label ==0)
            //{
            //    //l_cash_dispute.Text = "0,00";
            //    //t_cash.Text = "0,00";
            //    //l_cash_dispute.Text = string.Format("{0:#,###}" + ",00", cash2);
            //    t_cash.Text = string.Format("{0:#,###}", cash2);
            //    l_cash_dispute.Text = "0,00";
            //}
            //else
            //{
            //    l_cash_dispute.Text = String.Format("{0:#,###}" + ",00", cash_label);
            //}
        }
        //===========================================================================================================

        //================SEPARATOR FOR TEXTBOXT PETTY CASH=========================================================
        private void t_petty_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_petty.Text == "")
                {
                    //t_petty.Text = string.Format("{0:#,###}", bg_ToCasir);
                    //t_petty.Select(t_petty.Text.Length, 0);
                    //l_dispute_petty.Text = "0,00";
                    value_budget = 0; petty_label = System.Convert.ToDouble(bg_ToCasir);
                    petty_label = value_budget - petty_label;
                    l_dispute_petty.Text = String.Format("{0:#,###}" + ",00", petty_label);
                }
                else
                {
                    value_budget = double.Parse(t_petty.Text);
                    t_petty.Text = string.Format("{0:#,###}", value_budget);
                    t_petty.Select(t_petty.Text.Length, 0);
                    //kalkulasi otomatis tanpa menggunakan pindah textboxt
                    petty_label = System.Convert.ToDouble(bg_ToCasir);
                    petty_label = value_budget - petty_label;
                    if (petty_label == 0)
                    {
                        t_petty.Text = string.Format("{0:#,###}", bg_ToCasir);
                        l_dispute_petty.Text = "0,00";
                    }
                    else
                    {
                        l_dispute_petty.Text = String.Format("{0:#,###}" + ",00", petty_label);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                t_petty.Text = "";
            }
        }
        //==========================================================================================================

        //===============FOCUSS MENINGGALKAN TEXTBOX PETTY CASH========================================
        private void t_petty_Leave(object sender, EventArgs e)
        {
            //petty_label = System.Convert.ToDouble(bg_ToCasir);
            //petty_label = value_budget - petty_label;
            //if (petty_label == 0)
            //{
            //    //l_dispute_petty.Text = string.Format("{0:#,###}" + ",00", bg_ToCasir);
            //    t_petty.Text = string.Format("{0:#,###}", bg_ToCasir);
            //    l_dispute_petty.Text = "0,00";
            //}
            //else
            //{
            //    l_dispute_petty.Text = String.Format("{0:#,###}" + ",00", petty_label);
            //}
        }
        //=============================================================================================
    }
}
