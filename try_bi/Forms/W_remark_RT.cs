using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using try_bi.Class;

namespace try_bi
{
    public partial class W_remark_RT : Form
    {
        String qty2, return_id2, epy_id2, epy_name2, art_id, inv_id, no_sj2;
        int total_amount, count_ro_line, qty_ro_line, inv_good_qty, count_eror;
        koneksi ckon = new koneksi();
        koneksi2 ckon2 = new koneksi2();
        koneksi3 ckon3 = new koneksi3();
        public W_remark_RT()
        {
            InitializeComponent();
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //FORM KE LOAD
        private void W_remark_RT_Load(object sender, EventArgs e)
        {
            this.ActiveControl = t_remark;
            t_remark.Focus();
            cek_qty_line();
        }

        public void update_header(String qty, String return_id, int total, String no_sj)
        {

            qty2 = qty;
            return_id2 = return_id;
            total_amount = total;
            no_sj2 = no_sj;
        }
        public void set_id(String id, String name)
        {
            epy_id2 = id;
            epy_name2 = name;
        }
        //=====BUTTON OK
        private void b_ok_Click(object sender, EventArgs e)
        {
            API_ReturnOrder returnOrder = new API_ReturnOrder();
            bool api_response;

            try
            {
                if(count_eror != 0)
                {
                    MessageBox.Show("There Is A Total Quantity That Exceeds Inventory. Please Check Again !");
                }
                else
                {
                    api_response = returnOrder.returnOrder().Result;

                    if (api_response)
                    {
                        update_header();
                    }                    
                    else
                    {
                        MessageBox.Show("Make Sure You are Connected To Internet");
                    }
                }
                
            }
            catch
            {
                MessageBox.Show("data failured added");
                this.Close();
            }
        }
        //=========METHOD UNTUK MELAKUKAN UPDATE DI RETURN ORDER=========
        public void update_header()
        {
            String cmd_update = "UPDATE returnorder SET REMARK= '" + t_remark.Text + "' ,TOTAL_QTY='" + qty2 + "', STATUS='1', EMPLOYEE_ID='" + epy_id2 + "', EMPLOYEE_NAME='" + epy_name2 + "', TOTAL_AMOUNT='" + total_amount + "', NO_SJ='"+no_sj2+"' WHERE RETURN_ORDER_ID = '" + return_id2 + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
            //===POTONG INVENTORY SAAT MUTASI OUT
            Inv_Line inv = new Inv_Line();
            String type_trans = "5";
            inv.cek_type_trans(type_trans);
            inv.return_order(return_id2);
            MessageBox.Show("data successfully added");
                        
            UC_Ret_order.Instance.reset();
            UC_Ret_order.Instance.new_invoice();            
            UC_Ret_order.Instance.holding();
            UC_Ret_order.Instance.runRetreive();
            this.Close();
        }

        //METHOD UNTUK MENGECHECK TOTAL QTY DI RET_ORDER_LINE DIBANDINGKAN DENGAN TOTAL INVENTORY
        /* DESC = AKAN DIHITUNG TOTAL BARIS DARI RET_ORDER_LINE, LALU AKAN DIHITUNG BERAPA LINE YANG TIDAK SESUAI, LINE YG TIDAK SESUAI AKAN DIBANDINGKAN JUMLAHNYA DENGAN BERAPA BARIS RET_ORDER_LINE, JIKA TOTAL TIDAK SESUAI, MAKA TIDAK BISA MENJALAN METHOD "UPDATE_HEADER", JIKA JUMLAH SAMA MAKA JALANKAN METHOD UPDATE HEADER*/
        public void cek_qty_line()
        {
            CRUD sql = new CRUD();

            //ckon3.con3.Close();
            count_eror = 0;
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM returnorder_line WHERE RETURN_ORDER_ID = '" + return_id2 + "'";
                ckon.sqlDataRdHeader = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRdHeader.HasRows)
                {
                    while (ckon.sqlDataRdHeader.Read())
                    {
                        art_id = ckon.sqlDataRdHeader["ARTICLE_ID"].ToString();
                        qty_ro_line = Convert.ToInt32(ckon.sqlDataRdHeader["QUANTITY"].ToString());
                        compare(art_id, qty_ro_line);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ckon.sqlDataRdHeader != null)
                    ckon.sqlDataRdHeader.Close();

                if (ckon.sqlCon().State == ConnectionState.Open)
                    ckon.sqlCon().Close();
            }            
        }
        public void compare(String art, int qty_ro_line2)
        {
            CRUD sql = new CRUD();

            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT inventory.GOOD_QTY FROM inventory INNER JOIN article "
                                + "ON article._id = inventory.ARTICLE_ID "
                                + "WHERE article.ARTICLE_ID = '" + art_id + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        inv_good_qty = Convert.ToInt32(ckon.sqlDataRd["GOOD_QTY"].ToString());
                        if (qty_ro_line2 > inv_good_qty)
                        {
                            count_eror = count_eror + 1;
                        }
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
        //=====METHOD MENGHITUNG TOTAL BARIS RET_ORDER_LINE
        public void count_ret_order()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT COUNT(*) as total FROM returnorder_line WHERE RETURN_ORDER_ID = '" + return_id2 + "'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        count_ro_line = Convert.ToInt32(ckon.sqlDataRd["total"].ToString());
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
    }
}
