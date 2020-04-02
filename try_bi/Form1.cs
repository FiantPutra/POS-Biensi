 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class Form1 : Form
    {
        DateTime mydate = DateTime.Now;
        String date, st_shift, cust_id_store, nm_cur, VarBackDate;
        public string nama_employee, id_employee, status_buka_menu, store_code, comp_name;
        koneksi ckon = new koneksi();
        public Form1()
        {
            InitializeComponent();
        }
       
        private void btn_maax_Click(object sender, EventArgs e)
        {
            if(p_kiri.Width==50)
            {
                p_kiri.Width = 222;
                LogoMax.HideSync(btn_maax);
                AnimatorKiri.ShowSync(p_kiri);
                LogoClose.ShowSync(btn_close);
            }
        }

        public void setProp()
        {
            bunifuFlatButton2.BackColor = Color.FromArgb(255, 111, 0);
            b_transaction.BackColor = Color.FromArgb(70, 70, 70);
        }

        public void setHo()
        {
            if (store_code == "000")
            {
                menu_inout.Visible = false;
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if(p_kiri.Width==222)
            {
                p_kiri.Width = 50;
                LogoMax.ShowSync(btn_maax);
                AnimatorKiri.ShowSync(p_kiri);
                LogoClose.HideSync(btn_close);
            }
        }

        //==============================BUTTON TRANSACTION HISTORY=====================
        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if (status_buka_menu == "1")
            {
                MessageBox.Show("Please Close Store First");
            }
            else
            {
                label3.Text = "TRANSACTION HISTORY";
                date = mydate.ToString("yyyy-MM-dd");
                String query = "SELECT * FROM [transaction] WHERE DATE = '" + date + "' AND (STATUS='1' or STATUS='2')";
                UC_Trans_hist c = new UC_Trans_hist(this);
                p_kanan.Controls.Clear();
                if (!p_kanan.Controls.Contains(UC_Trans_hist.Instance))
                {
                    p_kanan.Controls.Add(UC_Trans_hist.Instance);
                    UC_Trans_hist.Instance.Dock = DockStyle.Fill;
                    UC_Trans_hist.Instance.get_id_shift();
                    UC_Trans_hist.Instance.employee_id = id_employee;
                    UC_Trans_hist.Instance.cust_store_id = cust_id_store;
                    UC_Trans_hist.Instance.comp_name = comp_name;
                    UC_Trans_hist.Instance.holding(query);
                    UC_Trans_hist.Instance.BringToFront();
                }
                else
                {
                    UC_Trans_hist.Instance.get_id_shift();
                    UC_Trans_hist.Instance.employee_id = id_employee;
                    UC_Trans_hist.Instance.cust_store_id = cust_id_store;
                    UC_Trans_hist.Instance.comp_name = comp_name;
                    UC_Trans_hist.Instance.holding(query);
                    UC_Trans_hist.Instance.BringToFront();
                }
            }
           
           
        }
        //============================BUTTON TRANSACTION==================================================
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            if(status_buka_menu == "1")
            {
                MessageBox.Show("Please Close Store First");
            }
            else
            {
                //======CEK APAKAH USER MEN SET BACK DATE = YES======
                VarBackDate = Properties.Settings.Default.mBackDate;
                if(VarBackDate == "YES")
                {
                    label3.Text = "TRANSACTION";
                    date = mydate.ToString("yyyy-MM-dd");
                    uc_coba c = new uc_coba(this);
                    p_kanan.Controls.Clear();
                    if (!p_kanan.Controls.Contains(uc_coba.Instance))
                    {
                        p_kanan.Controls.Add(uc_coba.Instance);
                        uc_coba.Instance.Dock = DockStyle.Fill;
                        uc_coba.Instance.cust_id_store2 = cust_id_store;
                        uc_coba.Instance.new_invoice();
                        uc_coba.Instance.isi_combo_spg();
                        uc_coba.Instance.holding(date);
                        uc_coba.Instance.id_employe2 = id_employee;
                        uc_coba.Instance.comp_name = comp_name;
                        uc_coba.Instance.runRetreive();
                        uc_coba.Instance.BringToFront();
                    }
                    else
                    {
                        uc_coba.Instance.cust_id_store2 = cust_id_store;
                        uc_coba.Instance.new_invoice();
                        uc_coba.Instance.isi_combo_spg();
                        uc_coba.Instance.holding(date);
                        uc_coba.Instance.id_employe2 = id_employee;
                        uc_coba.Instance.comp_name = comp_name;
                        uc_coba.Instance.runRetreive();
                        uc_coba.Instance.BringToFront();
                    }
                }
                else
                {
                    label3.Text = "TRANSACTION";
                    date = mydate.ToString("yyyy-MM-dd");
                    uc_coba c = new uc_coba(this);
                    p_kanan.Controls.Clear();
                    if (!p_kanan.Controls.Contains(uc_coba.Instance))
                    {
                        p_kanan.Controls.Add(uc_coba.Instance);
                        uc_coba.Instance.Dock = DockStyle.Fill;
                        uc_coba.Instance.cust_id_store2 = cust_id_store;
                        uc_coba.Instance.new_invoice();
                        uc_coba.Instance.isi_combo_spg();
                        uc_coba.Instance.holding(date);
                        uc_coba.Instance.id_employe2 = id_employee;
                        uc_coba.Instance.comp_name = comp_name;
                        uc_coba.Instance.runRetreive();
                        uc_coba.Instance.BringToFront();
                    }
                    else
                    {
                        uc_coba.Instance.cust_id_store2 = cust_id_store;
                        uc_coba.Instance.new_invoice();
                        uc_coba.Instance.isi_combo_spg();
                        uc_coba.Instance.holding(date);
                        uc_coba.Instance.id_employe2 = id_employee;
                        uc_coba.Instance.comp_name = comp_name;
                        uc_coba.Instance.runRetreive();
                        uc_coba.Instance.BringToFront();
                    }
                }
            }
            
        }
        //==================BUTTON CLOSING STORE==========================
        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            //Jika POS tersebut bukan PC Master maka menu ini tidak bisa diakses
            if (Properties.Settings.Default.MstrOrChld == "Child")
            {
                MessageBox.Show("This Menu Can Only Be Accessed By A Master PCss", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                label3.Text = "Closing Store";
                date = mydate.ToString("yyyy-MM-dd");               
                UC_Closing_Store c = new UC_Closing_Store(this);
                p_kanan.Controls.Clear();
                if (!p_kanan.Controls.Contains(UC_Closing_Store.Instance))
                {
                    p_kanan.Controls.Add(UC_Closing_Store.Instance);
                    UC_Closing_Store.Instance.Dock = DockStyle.Fill;
                    UC_Closing_Store.Instance.set_name(id_employee, nama_employee);
                    UC_Closing_Store.Instance.get_id_close();
                    UC_Closing_Store.Instance.from_form1();                    
                    UC_Closing_Store.Instance.total_trans();
                    UC_Closing_Store.Instance.BringToFront();
                }
                else
                {
                    UC_Closing_Store.Instance.set_name(id_employee, nama_employee);
                    UC_Closing_Store.Instance.get_id_close();
                    UC_Closing_Store.Instance.from_form1();
                    UC_Closing_Store.Instance.total_trans();
                    UC_Closing_Store.Instance.BringToFront();
                }
            }
            
            
           
        }
        //==============================================================================
        private void b_inventory_Click(object sender, EventArgs e)
        {
            
            String sql = "SELECT TOP 100 article.ARTICLE_ID, article.ARTICLE_NAME, article.BRAND_ID, article.SIZE_ID, article.COLOR_ID ,inventory.STATUS, inventory.GOOD_QTY FROM article, inventory WHERE article._id = inventory.ARTICLE_ID";
            label3.Text = "Inventory List";
            String sql2 = "SELECT SUM(inventory.GOOD_QTY) as total FROM article INNER JOIN inventory ON article._id = inventory.ARTICLE_ID ";
            UC_Inven_List c = new UC_Inven_List(this);
            p_kanan.Controls.Clear();
            if (!p_kanan.Controls.Contains(UC_Inven_List.Instance))
            {
                p_kanan.Controls.Add(UC_Inven_List.Instance);
                UC_Inven_List.Instance.Dock = DockStyle.Fill;
                UC_Inven_List.Instance.retreive(sql);
                UC_Inven_List.Instance.itung_total(sql2);
                UC_Inven_List.Instance.scan_fokus();
                UC_Inven_List.Instance.BringToFront();
            }
            else
            {
                UC_Inven_List.Instance.retreive(sql);
                UC_Inven_List.Instance.itung_total(sql2);
                UC_Inven_List.Instance.scan_fokus();
                UC_Inven_List.Instance.BringToFront();
            }

        }
        //==============================================================================
        private void b_stockTake_Click(object sender, EventArgs e)
        {
            //Jika POS tersebut bukan PC Master maka menu ini tidak bisa diakses
            if (Properties.Settings.Default.MstrOrChld == "Child")
            {
                MessageBox.Show("This Menu Can Only Be Accessed By A Master PCss", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {                
                label3.Text = "Stock Take";
                UC_Stock_Take c = new UC_Stock_Take(this);
                p_kanan.Controls.Clear();
                if (!p_kanan.Controls.Contains(UC_Stock_Take.Instance))
                {
                    p_kanan.Controls.Add(UC_Stock_Take.Instance);
                    UC_Stock_Take.Instance.Dock = DockStyle.Fill;
                    //UC_Stock_Take.Instance.from_form1();
                    UC_Stock_Take.Instance.id_employee2 = id_employee;
                    UC_Stock_Take.Instance.nm_employee2 = nama_employee;

                    //UC_Stock_Take.Instance.retreive(sql);
                    UC_Stock_Take.Instance.BringToFront();
                }
                else
                {
                    //UC_Stock_Take.Instance.from_form1();
                    UC_Stock_Take.Instance.id_employee2 = id_employee;
                    UC_Stock_Take.Instance.nm_employee2 = nama_employee;
                    //UC_Stock_Take.Instance.retreive(sql);
                    UC_Stock_Take.Instance.BringToFront();
                }
            }
            
        }
        //==============================================================================
        private void b_req_Order_Click(object sender, EventArgs e)
        {
            //Jika POS tersebut bukan PC Master maka menu ini tidak bisa diakses
            if (Properties.Settings.Default.MstrOrChld == "Child")
            {
                MessageBox.Show("This Menu Can Only Be Accessed By A Master PCss", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                label3.Text = "Request Order";
                UC_Req_order c = new UC_Req_order(this);
                p_kanan.Controls.Clear();
                if (!p_kanan.Controls.Contains(UC_Req_order.Instance))
                {
                    p_kanan.Controls.Add(UC_Req_order.Instance);
                    UC_Req_order.Instance.Dock = DockStyle.Fill;
                    UC_Req_order.Instance.new_invoice();
                    UC_Req_order.Instance.set_name(id_employee, nama_employee);
                    UC_Req_order.Instance.holding();
                    //RunningNumber running = new RunningNumber();
                    //running.get_data_before("2", "RO");
                    UC_Req_order.Instance.set_running_number();
                    UC_Req_order.Instance.BringToFront();
                }
                else
                {
                    UC_Req_order.Instance.new_invoice();
                    UC_Req_order.Instance.set_name(id_employee, nama_employee);
                    UC_Req_order.Instance.holding();
                    //RunningNumber running = new RunningNumber();
                    //running.get_data_before("2", "RO");
                    UC_Req_order.Instance.set_running_number();
                    UC_Req_order.Instance.BringToFront();
                }
            }
           
        }
        //==============================================================================
        private void b_mutasi_Click(object sender, EventArgs e)
        {
            //Jika POS tersebut bukan PC Master maka menu ini tidak bisa diakses
            if (Properties.Settings.Default.MstrOrChld == "Child")
            {
                MessageBox.Show("This Menu Can Only Be Accessed By A Master PCss", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                label3.Text = "Mutasi Order";
                UC_Mut_order c = new UC_Mut_order(this);
                p_kanan.Controls.Clear();
                if (!p_kanan.Controls.Contains(UC_Mut_order.Instance))
                {
                    p_kanan.Controls.Add(UC_Mut_order.Instance);
                    UC_Mut_order.Instance.Dock = DockStyle.Fill;
                    UC_Mut_order.Instance.new_invoice();
                    UC_Mut_order.Instance.set_name(id_employee, nama_employee);
                    UC_Mut_order.Instance.holding();
                    //RunningNumber running = new RunningNumber();
                    //running.get_data_before("3", "MT");
                    UC_Mut_order.Instance.set_running_number();
                    UC_Mut_order.Instance.BringToFront();
                }
                else
                {
                    UC_Mut_order.Instance.new_invoice();
                    UC_Mut_order.Instance.set_name(id_employee, nama_employee);
                    UC_Mut_order.Instance.holding();
                    //RunningNumber running = new RunningNumber();
                    //running.get_data_before("3", "MT");
                    UC_Mut_order.Instance.set_running_number();
                    UC_Mut_order.Instance.BringToFront();
                }
            }
            
        }
        //==============================================================================
        private void b_return_Click(object sender, EventArgs e)
        {
            //Jika POS tersebut bukan PC Master maka menu ini tidak bisa diakses
            if (Properties.Settings.Default.MstrOrChld == "Child")
            {
                MessageBox.Show("This Menu Can Only Be Accessed By A Master PCss", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                label3.Text = "Return Order";
                UC_Ret_order c = new UC_Ret_order(this);
                p_kanan.Controls.Clear();
                if (!p_kanan.Controls.Contains(UC_Ret_order.Instance))
                {
                    p_kanan.Controls.Add(UC_Ret_order.Instance);
                    UC_Ret_order.Instance.Dock = DockStyle.Fill;
                    UC_Ret_order.Instance.new_invoice();
                    UC_Ret_order.Instance.set_name(id_employee, nama_employee);
                    //RunningNumber running = new RunningNumber();
                    //running.get_data_before("4", "RT");
                    UC_Ret_order.Instance.set_running_number();
                    UC_Ret_order.Instance.holding();
                    UC_Ret_order.Instance.BringToFront();
                }
                else
                {
                    UC_Ret_order.Instance.new_invoice();
                    UC_Ret_order.Instance.set_name(id_employee, nama_employee);
                    //RunningNumber running = new RunningNumber();
                    //running.get_data_before("4", "RT");
                    UC_Ret_order.Instance.set_running_number();
                    UC_Ret_order.Instance.holding();
                    UC_Ret_order.Instance.BringToFront();
                }
            }
            

        }
        //==============================================================================
        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            label3.Text = "Promotion";
            UC_Promotion c = new UC_Promotion(this);
            p_kanan.Controls.Clear();
            if(!p_kanan.Controls.Contains(UC_Promotion.Instance))
            {
                p_kanan.Controls.Add(UC_Promotion.Instance);
                UC_Promotion.Instance.Dock = DockStyle.Fill;
                UC_Promotion.Instance.holding();
                UC_Promotion.Instance.BringToFront();
            }
            else
                UC_Promotion.Instance.holding();
            UC_Promotion.Instance.BringToFront();
        }
        //==============================================================================
        private void b_petyCash_Click(object sender, EventArgs e)
        {
            //Jika POS tersebut bukan PC Master maka menu ini tidak bisa diakses
            if (Properties.Settings.Default.MstrOrChld == "Child")
            {
                MessageBox.Show("This Menu Can Only Be Accessed By A Master PCss", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (status_buka_menu == "1")
                {
                    MessageBox.Show("Please Close Store First");
                }
                else
                {
                    label3.Text = "Petty Cash";
                    UC_Petty_Cash c = new UC_Petty_Cash(this);
                    p_kanan.Controls.Clear();
                    if (!p_kanan.Controls.Contains(UC_Petty_Cash.Instance))
                    {
                        p_kanan.Controls.Add(UC_Petty_Cash.Instance);
                        UC_Petty_Cash.Instance.Dock = DockStyle.Fill;
                        UC_Petty_Cash.Instance.isi_combo_expanse();
                        UC_Petty_Cash.Instance.holding();
                        UC_Petty_Cash.Instance.new_invoice();
                        UC_Petty_Cash.Instance.get_budget();
                        UC_Petty_Cash.Instance.get_expense();
                        UC_Petty_Cash.Instance.cust_id_store2 = cust_id_store;
                        //RunningNumber running = new RunningNumber();
                        //running.get_data_before("6", "PC");
                        UC_Petty_Cash.Instance.set_running_number();
                        UC_Petty_Cash.Instance.BringToFront();
                    }
                    else
                    {
                        UC_Petty_Cash.Instance.holding();
                        UC_Petty_Cash.Instance.isi_combo_expanse();
                        UC_Petty_Cash.Instance.new_invoice();
                        UC_Petty_Cash.Instance.get_budget();
                        UC_Petty_Cash.Instance.get_expense();
                        UC_Petty_Cash.Instance.cust_id_store2 = cust_id_store;
                        //RunningNumber running = new RunningNumber();
                        //running.get_data_before("6", "PC");
                        UC_Petty_Cash.Instance.set_running_number();
                        UC_Petty_Cash.Instance.BringToFront();
                    }
                }
            }
            
        
        }

        private void b_SyncStore_Click(object sender, EventArgs e)
        {
            UC_SyncStore syncStore = new UC_SyncStore(this);
            
            p_kanan.Controls.Clear();
            if (!p_kanan.Controls.Contains(UC_SyncStore.Instance))
            {
                p_kanan.Controls.Add(UC_SyncStore.Instance);
                UC_SyncStore.Instance.Dock = DockStyle.Fill;
                UC_SyncStore.Instance.BringToFront();
            }
            else
            {
                UC_SyncStore.Instance.BringToFront();
            }
        }

        //==============================================================================
        private void b_do_Click(object sender, EventArgs e)
        {
            //Jika POS tersebut bukan PC Master maka menu ini tidak bisa diakses
            if (Properties.Settings.Default.MstrOrChld == "Child")
            {
                MessageBox.Show("This Menu Can Only Be Accessed By A Master PCss", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                String sql = "SELECT * FROM deliveryorder WHERE STATUS = 'pending' AND DELIVERY_DATE = '" + System.DateTime.Now + "'";
                label3.Text = "DO Confirmation";
                UC_DO c = new UC_DO(this);
                p_kanan.Controls.Clear();
                if (!p_kanan.Controls.Contains(UC_DO.Instance))
                {
                    p_kanan.Controls.Add(UC_DO.Instance);
                    UC_DO.Instance.Dock = DockStyle.Fill;
                    UC_DO.Instance.reset();
                    UC_DO.Instance.set_name(id_employee, nama_employee);
                    UC_DO.Instance.color_dgv();
                    UC_DO.Instance.holding(sql);
                    UC_DO.Instance.BringToFront();
                }
                else
                {
                    UC_DO.Instance.reset();
                    UC_DO.Instance.set_name(id_employee, nama_employee);
                    UC_DO.Instance.color_dgv();
                    UC_DO.Instance.holding(sql);
                    UC_DO.Instance.BringToFront();
                }
            }
            
        }

        private void menu_inout_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            UC_Article_In_Out c = new UC_Article_In_Out(this);
            p_kanan.Controls.Clear();
            if (!p_kanan.Controls.Contains(UC_Article_In_Out.Instance))
            {
                p_kanan.Controls.Add(UC_Article_In_Out.Instance);
                UC_Article_In_Out.Instance.Dock = DockStyle.Fill;
                UC_Article_In_Out.Instance.epy_id = id_employee;
                UC_Article_In_Out.Instance.epy_name = nama_employee;
                UC_Article_In_Out.Instance.runRetrive();
                UC_Article_In_Out.Instance.BringToFront();
            }
            else
            {
                UC_Article_In_Out.Instance.epy_id = id_employee;
                UC_Article_In_Out.Instance.epy_name = nama_employee;
                UC_Article_In_Out.Instance.runRetrive();
                UC_Closing_Shift.Instance.BringToFront();
            }
        }

        //==============================================================================
        private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
        {
            if (status_buka_menu == "1")
            {
                MessageBox.Show("Please Close Store First");
            }
            else
            {
                label3.Text = "Closing Shift";
                UC_Closing_Shift c = new UC_Closing_Shift(this);
                p_kanan.Controls.Clear();
                if (!p_kanan.Controls.Contains(UC_Closing_Shift.Instance))
                {
                    p_kanan.Controls.Add(UC_Closing_Shift.Instance);
                    UC_Closing_Shift.Instance.Dock = DockStyle.Fill;
                    UC_Closing_Shift.Instance.get_id_shift();
                    UC_Closing_Shift.Instance.set_name(id_employee, nama_employee);
                    //UC_Closing_Shift.Instance.get_shift();
                    UC_Closing_Shift.Instance.from_form1();
                    UC_Closing_Shift.Instance.total_trans();
                    
                    UC_Closing_Shift.Instance.BringToFront();
                }
                else
                {
                    UC_Closing_Shift.Instance.get_id_shift();
                    UC_Closing_Shift.Instance.set_name(id_employee, nama_employee);
                    //UC_Closing_Shift.Instance.get_shift();
                    UC_Closing_Shift.Instance.from_form1();
                    UC_Closing_Shift.Instance.total_trans();
                    
                    UC_Closing_Shift.Instance.BringToFront();
                }
            }
        }
        //==============================================================================
        private void pic_profil_Click(object sender, EventArgs e)
        {
            w_logout log = new w_logout(this);
            log.ShowDialog();
        }
        //==============================================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            label_title.Text = "BIENSI " + try_bi.Properties.Settings.Default.mVersion;
            //wahyu 12-11-19
            comp_name = Environment.MachineName.ToString();
            DateTime mydate = DateTime.Now;
            //String sql = "UPDATE transaction SET STATUS='4' WHERE DATE !='"+ mydate.ToString("yyyy-MM-dd") + "'";
            //CRUD input = new CRUD();
            //input.ExecuteNonQuery(sql);
            //end
            St_last_shift();
            get_name();
            get_currency(); 
            //=====================================
            if (store_code == "000")
            {
                t_nama.Text = "Cashier : " + nama_employee;
            } else
            {
                t_nama.Text = nama_employee;
            }
            
            //JIKA STATUS TERAKHIR ADALAH 0, MAKA========================================
            if (store_code == "000")
            {
                if (st_shift == "0")
                {
                    label3.Text = "TRANSACTION";
                    date = mydate.ToString("yyyy-MM-dd");
                    uc_coba c = new uc_coba(this);
                    p_kanan.Controls.Clear();
                    if (!p_kanan.Controls.Contains(uc_coba.Instance))
                    {
                        p_kanan.Controls.Add(uc_coba.Instance);
                        uc_coba.Instance.Dock = DockStyle.Fill;
                        uc_coba.Instance.nm_cur2 = nm_cur;
                        uc_coba.Instance.cust_id_store2 = cust_id_store;
                        uc_coba.Instance.new_invoice();
                        uc_coba.Instance.isi_combo_spg();
                        uc_coba.Instance.holding(date);
                        uc_coba.Instance.id_employe2 = id_employee;
                        uc_coba.Instance.comp_name = comp_name;
                        uc_coba.Instance.runRetreive();
                        uc_coba.Instance.BringToFront();
                    }
                    else
                    {
                        uc_coba.Instance.nm_cur2 = nm_cur;
                        uc_coba.Instance.cust_id_store2 = cust_id_store;
                        uc_coba.Instance.new_invoice();
                        uc_coba.Instance.isi_combo_spg();
                        uc_coba.Instance.holding(date);
                        uc_coba.Instance.id_employe2 = id_employee;
                        uc_coba.Instance.comp_name = comp_name;
                        uc_coba.Instance.runRetreive();
                        uc_coba.Instance.BringToFront();
                    }
                }
                //=============JIKA STATUS TERAKHIR ADALAH 1, MAKA=================================
                else
                {
                    UC_Popup_Info c = new UC_Popup_Info(this);
                    p_kanan.Controls.Clear();
                    if (!p_kanan.Controls.Contains(UC_Popup_Info.Instance))
                    {
                        p_kanan.Controls.Add(UC_Popup_Info.Instance);
                        UC_Popup_Info.Instance.Dock = DockStyle.Fill;
                        UC_Popup_Info.Instance.set_angka();
                        UC_Popup_Info.Instance.get_name();
                        UC_Popup_Info.Instance.get_currency();
                        UC_Popup_Info.Instance.comp_name = comp_name;
                        UC_Popup_Info.Instance.set_name(id_employee, nama_employee);
                        UC_Popup_Info.Instance.set_shift();
                        UC_Popup_Info.Instance.BringToFront();
                    }
                    else
                    {
                        UC_Popup_Info.Instance.set_angka();
                        UC_Popup_Info.Instance.get_name();
                        UC_Popup_Info.Instance.get_currency();
                        //UC_Popup_Info.Instance.cust_store();
                        UC_Popup_Info.Instance.comp_name = comp_name;
                        UC_Popup_Info.Instance.set_name(id_employee, nama_employee);
                        UC_Popup_Info.Instance.set_shift();
                        UC_Popup_Info.Instance.BringToFront();
                    }
                }
            }
            else
            {
                //wahyu-ho
                btn_maax.Visible = true;
                bunifuFlatButton2.Visible = true;
                b_transaction.Visible = true;
                bunifuFlatButton1.Visible = true;
                bunifuFlatButton3.Visible = true;
                bunifuFlatButton4.Visible = true;
                b_petyCash.Visible = true;
                b_inventory.Visible = true;
                b_stockTake.Visible = true;
                b_req_Order.Visible = true;
                b_mutasi.Visible = true;
                b_return.Visible = true;
                b_do.Visible = true;

                menu_inout_Click(null, null);
            }

        }
        //=======MENGECEK STATUS TERAKHIR SHIFT, KALO 0 DIA LANGSUNG KE MENU TRANSAKSI==============
        public void St_last_shift()
        {
            string command;
            string id_shift = "";

            //ckon.con.Close();
            //String sql = "SELECT * FROM closing_shift ORDER BY _id DESC LIMIT 1";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //if (ckon.myReader.HasRows)
            //{
            //    while (ckon.myReader.Read())
            //    {
            //        st_shift = ckon.myReader.GetString("STATUS_CLOSE");
            //    }
            //}
            //else
            //{ st_shift = "1"; }
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT TOP 1 * FROM closing_shift ORDER BY _id DESC";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        st_shift = ckon.sqlDataRd["STATUS_CLOSE"].ToString();
                        id_shift = ckon.sqlDataRd["EMPLOYEE_ID"].ToString();
                    }
                }
                else
                {
                    st_shift = "1";
                }

                if (id_employee != id_shift && st_shift == "0")
                {
                    MessageBox.Show("Please login using ID : " + id_shift + "", "Warning");
                    //========for logout========                    
                    this.Hide();
                    Form_Login login = new Form_Login();
                    login.ShowDialog();
                    this.Close();                    
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
        //=============GET NAME STORE==========================================================
        public void get_name()
        {
            string command;            

            //ckon.con.Close();
            //String sql = "SELECT * FROM store";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    if (ckon.myReader.HasRows)
            //    {
            //        while (ckon.myReader.Read())
            //        {

            //            cust_id_store = ckon.myReader.GetString("CUST_ID_STORE");

            //        }

            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ }

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM store";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        cust_id_store = ckon.sqlDataRd["CUST_ID_STORE"].ToString();
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
        //==========================GET DATA CURRENCY==================================
        public void get_currency()
        {
            string command;            

            //ckon.con.Close();
            //String sql = "SELECT * FROM currency";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    nm_cur = ckon.myReader.GetString("NAME");
            //}
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM currency";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        nm_cur = ckon.sqlDataRd["NAME"].ToString();
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
        //=============================================================================
    }
}
