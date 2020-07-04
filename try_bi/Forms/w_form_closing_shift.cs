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
    public partial class w_form_closing_shift : Form
    {
        public static Form1 f1;
        
        koneksi ckon = new koneksi();
        int change, cash, cash2, bg_ToCasir, qty_article, edc;
        Double valueCash, valueEDC, value_budget, petty_label, cash_label, edc_label, value_modal, deposit_label, deposit_label2;
        String id_modal_store, query, shift, real_trans_balance, real_petty_cash, real_deposit, epy_id2, name_id2;
        public String id_shift2;
        int PANTEK_DEPOSIT = 0;
        public string shift_code, date_closing_shift3, status_sukses;

        //=============FORM LOAD ==============================
        private void w_form_closing_shift_Load(object sender, EventArgs e)
        {
            //set_modal_store();
            l_tgl.Text = date_closing_shift3;
            get_code_Shift();
            itung_cash();
            itung_EDC();
            get_budget();
        }
        //===========================================================================
        public void closing_shift_method(String a)
        {
            status_sukses = a;
        }
        //==================BUTTON OK=====================================
        private void b_ok_Click(object sender, EventArgs e)
        {
            Boolean api_response;
            try
            {
                if (cash_label <= 0)
                {
                    //==========API CLOSING SHIFT=============
                    update();
                    API_Closing_shift close = new API_Closing_shift();
                    close.get_code(id_shift2);
                    api_response = close.Post_Closing_Shift().Result;

                    if (api_response)
                    {
                        UC_Closing_Shift.Instance.reset();
                        //DELETE TRANSAKSI YG DI HOLD DENGAN ID CLOSING SHIFT
                        Del_Trans_Hold DEL = new Del_Trans_Hold();
                        DEL.get_data(id_shift2);
                        DEL.del_trans();
                        DEL.update_runningNumber();
                        //DEL.update_table();
                        //========for logout========
                        f1.Hide();
                        this.Hide();
                        Form_Login login = new Form_Login();
                        login.ShowDialog();
                        f1.Close();
                        this.Close();
                    }
                    else
                    {
                        String cmd_update = "UPDATE closing_shift SET STATUS_CLOSE='0' WHERE ID_SHIFT='" + id_shift2 + "'";
                        CRUD update = new CRUD();
                        update.ExecuteNonQuery(cmd_update);

                        MessageBox.Show("Make Sure You are Connected To Internet");
                    }
                }            
                else
                {
                    MessageBox.Show("Tidak bisa closing shift karena ada perbedaaan antara jumlah fisik dengan sistem");
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }           
        }
        //===============BUTTON CANCEL=======================
        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //=======================GET NAME AND ID EMPLOYEE FOR DB LOKAL===============
        public void set_name2(String id, String name)
        {
            epy_id2 = id;
            name_id2 = name;
        }
        //============GET TOTAL QTY===============
        public void get_qty(int qty)
        {
            qty_article = qty;
        }

        //========================================================================================
        public w_form_closing_shift(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }



        //==============set modal store=================
        //public void set_modal_store()
        //{
        //    //untuk mengeset nilai menjadi 0, karna dari backend blom ada nilaim=nya, nanti akan dihapus
        //    l_deposite.Text = "0,00";
        //    t_deposite.Text = "0,00";
        //}

        //===method untuk memberikan nilai koma saat diberikan angka, untuk textboxt deposite
        private void t_deposite_KeyUp(object sender, KeyEventArgs e)
        {
            //try
            //{
            //    if (t_deposite.Text == "")
            //    {

            //    }
            //    else
            //    {
            //        value_modal = double.Parse(t_deposite.Text);
            //        if(value_modal==0 )
            //        { t_deposite.Text = "0,00"; }
            //        else
            //        {
            //            t_deposite.Text = string.Format("{0:#,###}", value_modal);
            //            t_deposite.Select(t_deposite.Text.Length, 0);
            //        }

            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("Please Input Number");
            //    t_deposite.Text = "";
            //}
        }
        
        //=======SEPARATOR TEXTBOXT UNTUK MEMBERIKAN KOMA DI KOLOM TEXTBOXT=============================================
        private void t_cash_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_cash.Text == "")
                {                   
                    //COBA FUNGSI AGAAR SAAT T_CASH KOSONG, PUNYA KESEMPATAN INPUT NILAI BEBAS, TIDAK LANGSUNG DI SET KE HARGA CASH
                    valueCash = 0; cash_label = System.Convert.ToDouble(cash2);
                    cash_label = valueCash - cash_label;
                    l_cash_dispute.Text = String.Format("{0:#,###}" + ",00", cash_label);
                }
                else
                {
                    valueCash = double.Parse(t_cash.Text);
                    t_cash.Text = string.Format("{0:#,###}", valueCash);
                    t_cash.Select(t_cash.Text.Length, 0);
                    //=======COBA FUNGSI WAKTU DI INPUT LANGSUNG KEHITUNG
                    cash_label = System.Convert.ToDouble(cash2);
                    cash_label = valueCash - cash_label;
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

        private void t_edc_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_edc.Text == "")
                {
                    //COBA FUNGSI AGAAR SAAT T_CASH KOSONG, PUNYA KESEMPATAN INPUT NILAI BEBAS, TIDAK LANGSUNG DI SET KE HARGA CASH
                    valueEDC = 0; edc_label = System.Convert.ToDouble(edc);
                    edc_label = valueEDC - edc_label;
                    l_edc_dispute.Text = String.Format("{0:#,###}" + ",00", edc_label);
                }
                else
                {
                    valueEDC = double.Parse(t_edc.Text);
                    t_edc.Text = string.Format("{0:#,###}", valueEDC);
                    t_edc.Select(t_edc.Text.Length, 0);
                    //=======COBA FUNGSI WAKTU DI INPUT LANGSUNG KEHITUNG
                    edc_label = System.Convert.ToDouble(edc);
                    edc_label = valueEDC - edc_label;
                    if (edc_label == 0)
                    {
                        t_edc.Text = string.Format("{0:#,###}", edc);
                        l_edc_dispute.Text = "0,00";
                    }
                    else
                    {
                        l_edc_dispute.Text = String.Format("{0:#,###}" + ",00", edc_label);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                t_edc.Text = "";
            }
        }

        //================SEPARATOR FOR TEXTBOXT PETTY CASH=========================================================
        private void t_petty_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_petty.Text == "")
                {
                    //COBA FUNGSI AGAAR SAAT T_CASH KOSONG, PUNYA KESEMPATAN INPUT NILAI BEBAS, TIDAK LANGSUNG DI SET KE HARGA CASH
                    value_budget = 0; petty_label = System.Convert.ToDouble(bg_ToCasir);
                    petty_label = value_budget - petty_label;
                    l_dispute_petty.Text = String.Format("{0:#,###}" + ",00", petty_label);
                }
                else
                {
                    value_budget = double.Parse(t_petty.Text);
                    t_petty.Text = string.Format("{0:#,###}", value_budget);
                    t_petty.Select(t_petty.Text.Length, 0);
                    //=======coba ketik langsung kehitung, tanpa di tab
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
        
        //===================ambil dia shift berapa berdasarkan _id dari form sebelum nya===================
        public void get_code_Shift()
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM closing_shift WHERE ID_SHIFT='" + id_shift2 + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        shift = ckon.sqlDataRd["SHIFT"].ToString();
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
        //=================================MENGHITUNG TOTAL CASH================================================================
        public void itung_cash()
        {
            CRUD sql = new CRUD();
            DateTime mydate = DateTime.Now;
            String date = mydate.ToString("yyyy-MM-dd");

            try
            {
                ckon.sqlCon().Open();
                String cmd_transCash = "SELECT SUM([transaction].CASH) as total FROM [transaction] WHERE ID_SHIFT='" + id_shift2 + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transCash, ckon.sqlCon());

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

                String cmd_transChange = "SELECT SUM([transaction].CHANGEE) as total FROM [transaction] WHERE ID_SHIFT='" + id_shift2 + "' AND (STATUS='1' or STATUS='2')";
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
                    valueCash = cash2;
                }
                else
                {
                    l_cash.Text = string.Format("{0:#,###}" + ",00", cash2);
                    t_cash.Text = string.Format("{0:#,###}", cash2);
                    valueCash = cash2;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("No cash transaction", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }            
        }

        public void itung_EDC()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd_transEdc = "SELECT SUM(EDC + EDC2) as total FROM [transaction] WHERE ID_SHIFT='" + id_shift2 + "' AND (STATUS='1' or STATUS='2')";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_transEdc, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        edc = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
                    }
                }
                else
                {
                    edc = 0;
                }

                if (edc <= 0)
                {
                    l_edc.Text = "0,00";
                    t_edc.Text = "0,00";
                    valueEDC = edc;
                }
                else
                {
                    l_edc.Text = string.Format("{0:#,###}" + ",00", edc);
                    t_edc.Text = string.Format("{0:#,###}", edc);
                    valueEDC = edc;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("No EDC transaction", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRd != null)
                    ckon.sqlDataRd.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }
        }
        
        //==================mengambil NILAI OPENING BALANCE CASH DARI TABEL CLOSE SHIFT=============
        public void get_opening()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM closing_shift ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        real_trans_balance = ckon.sqlDataRd["REAL_TRANS_BALANCE"].ToString();
                        real_petty_cash = ckon.sqlDataRd["REAL_PETTY_CASH"].ToString();
                        real_deposit = ckon.sqlDataRd["REAL_DEPOSIT"].ToString();
                    }
                }
                else
                {
                    real_trans_balance = "0";
                    real_petty_cash = "0";
                    real_deposit = "0";
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
        //=========================MENGAMBIL NILAI OPENING DARI TABEL CLOSE STORE=========
        public void get_opening_close()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT TOP 1 * FROM closing_store ORDER BY _id DESC";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        real_trans_balance = ckon.sqlDataRd["REAL_TRANS_BALANCE"].ToString();
                        real_petty_cash = ckon.sqlDataRd["REAL_PETTY_CASH"].ToString();
                        real_deposit = ckon.sqlDataRd["REAL_DEPOSIT"].ToString();
                    }
                }
                else
                {
                    real_trans_balance = "0";
                    real_petty_cash = "0";
                    real_deposit = "0";
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
        //===================method for update into table closing shift=====================
        public void update()
        {
            double closingTrans, realTrans, disputeTrans;

            if(shift=="1")
            {
                get_opening_close();                
            }
            if(shift=="2")
            {
                get_opening();                
            }

            closingTrans = cash2 + edc;
            realTrans = valueCash + valueEDC;
            disputeTrans = cash_label + edc_label;


            DateTime mydate = DateTime.Now;
            String time_now = mydate.ToString("yyyy-MM-dd H:mm:ss");
            String cmd_update = "UPDATE closing_shift SET CLOSING_TIME='" + time_now + "',  CLOSING_TRANS_BALANCE='" + closingTrans + "', REAL_TRANS_BALANCE='" + realTrans + "', DISPUTE_TRANS_BALANCE='" + disputeTrans + "',  CLOSING_PETTY_CASH='" + bg_ToCasir + "',REAL_PETTY_CASH='" + value_budget + "', DISPUTE_PETTY_CASH='" + petty_label + "',CLOSING_DEPOSIT='0',REAL_DEPOSIT='" + value_modal + "',DISPUTE_DEPOSIT='" + deposit_label + "',TOTAL_QTY='" + qty_article + "' ,STATUS_CLOSE='1', EMPLOYEE_ID='" + epy_id2 + "', EMPLOYEE_NAME='" + name_id2 + "' WHERE ID_SHIFT='" + id_shift2 + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }
    }
}
