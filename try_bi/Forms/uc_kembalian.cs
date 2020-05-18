using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PrinterUtility;
using System.Collections;
using System.IO;

namespace try_bi
{
    public partial class uc_kembalian : UserControl
    {
        String date, id_transaksi, method_payment, no_ref, change, a_date, a_id, a_total, a_name, a_jam, a_sub, a_subtotal;
        int kembali2, cash2, int_subtotal;
        double tot_aft_diskon, kembalian2;
        DateTime mydate = DateTime.Now;
        DateTime myhour = DateTime.Now;       

        koneksi ckon = new koneksi();
        public static Form1 f1;
        private static uc_kembalian _instance;

        public static uc_kembalian Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_kembalian(f1);
                return _instance;
            }
        }
        //==================================================================================================
        public uc_kembalian(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //=============================METHOD FOR CASH PAYMENT=============================================
        public void kembali(String cash, String kembalian, String new_id)
        {
            this.ActiveControl = t_shorcut2;
            t_shorcut2.Focus();

            kembali2 = Int32.Parse(kembalian);
            cash2 = Int32.Parse(cash);
            id_transaksi = new_id;
            //===coba fungsi menampilkan kedalam textbot yang transparan, arag lebih rapih
            String label_kembali;
            String label_total;
            label_total = string.Format("{0:#,###}" + ",00", cash2);
            if (kembali2 == 0)
            { label_kembali = "0,00"; }
            else
            { label_kembali = String.Format("{0:#,###}" + ",00", kembali2); }
            t_kembali_center.Text = "Change  " + label_kembali;//taro tulisan change dan kembalian di textboxt pertama
            t_paymentMethod.Text = "Payment Cash";
            t_detail_center.Text = "Rp. " + label_total;

        }
        //==================================================================================================
        private void b_new_trans2_Click(object sender, EventArgs e)
        {
            date = mydate.ToString("yyyy-MM-dd");

            f1.p_kanan.Controls.Clear();
            if (!f1.p_kanan.Controls.Contains(uc_coba.Instance))
            {
                f1.p_kanan.Controls.Add(uc_coba.Instance);
                uc_coba.Instance.Dock = DockStyle.Fill;
                uc_coba.Instance.new_invoice();
                uc_coba.Instance.holding(date);
                uc_coba.Instance.delete_rows();
                uc_coba.Instance.runRetreive();
                uc_coba.Instance.l_total.Text = "0,00";
                uc_coba.Instance.l_diskon.Text = "0,00";
                uc_coba.Instance.l_voucher.Text = "0,00";
                uc_coba.Instance.void_ref.Text = "";
                uc_coba.Instance.void_amount.Text = "";
                //uc_coba.Instance.Show();
                uc_coba.Instance.BringToFront();
            }
            else
            {
                f1.p_kanan.Controls.Add(uc_coba.Instance);
                uc_coba.Instance.Dock = DockStyle.Fill;
                uc_coba.Instance.new_invoice();
                uc_coba.Instance.holding(date);
                uc_coba.Instance.delete_rows();
                uc_coba.Instance.runRetreive();
                uc_coba.Instance.l_total.Text = "0,00";
                uc_coba.Instance.l_diskon.Text = "0,00";
                uc_coba.Instance.l_voucher.Text = "0,00";
                uc_coba.Instance.void_ref.Text = "";
                uc_coba.Instance.void_amount.Text = "";
                //uc_coba.Instance.Show();
                uc_coba.Instance.BringToFront();
            }
        }
        //==========================METHOD FOR EDC PAYMENT =================================================
        public void edc(String new_id, int cash, String nama_bank)
        {
            this.ActiveControl = t_shorcut2;
            t_shorcut2.Focus();
            
            cash2 = cash;
            id_transaksi = new_id;          
            String var_edc = string.Format("{0:#,###}" + ",00", cash);
            t_kembali_center.Text = "Change 0,00";//taro tulisan change dan kembalian di textboxt pertama
            t_paymentMethod.Text = "Payment EDC";
            t_detail_center.Text = nama_bank + " Rp. " + var_edc;            
        }
        //==================================================================================================

        //=====================================METHOD FOR SPLIT PAYMENT=====================================
        public void split(double change2, double cash3, String nm_bank, double edc2, String new_id, int cashh)
        {
            this.ActiveControl = t_shorcut2;
            t_shorcut2.Focus();
           
            id_transaksi = new_id;
            cash2 = cashh;                        
            String cash = string.Format("{0:#,###}" + ",00", cash3);
            String nama_bank = nm_bank;            
            String edc = string.Format("{0:#,###}" + ",00", edc2);            

            t_kembali_center.Text = "Change 0,00";
            t_paymentMethod.Text = "Payment Split";
            t_detail_center.Text = "Cash Rp. " + cash;
            t_detail_center_split.Text = "EDC " + nama_bank + " Rp. " + edc;
        }
        //==============================METHOD FOR SPLIT EDC=================================================
        public void split_edc(String new_id, double edc1, double edc2, String nm_bank1, String nm_bank2)
        {
            this.ActiveControl = t_shorcut2;
            t_shorcut2.Focus();
            
            id_transaksi = new_id;
            String edc_1 = String.Format("{0:#,###}" + ",00", edc1);
            String edc_2 = String.Format("{0:#,###}" + ",00", edc2);                        

            t_kembali_center.Text = "Change 0,00";
            t_paymentMethod.Text = "Payment Split";
            t_detail_center.Text = "EDC " + nm_bank1 + " Rp. " + edc_1;
            t_detail_center_split.Text = "EDC " + nm_bank2 + " Rp. " + edc_2;
        }
        //================================METHOD FOR STRUCK================================================
        public void for_struk(String jenis, String noref, Double totall, Double kembalian)
        {
            method_payment = jenis;
            tot_aft_diskon = totall;
            no_ref = noref;
            kembalian2 = kembalian;
            if(kembalian2 != 0)
            {
                change = kembalian2.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
            }

        }
        //=================================================================================================

        //==============================TOMBOL PRINT=======================================================
        private void b_print_Click(object sender, EventArgs e)
        {
            LinkApi ls = new LinkApi();
            if (ls.print_default == "1")
            {
                PrintThermal print = new PrintThermal();
                print.get_trans_id(id_transaksi);
                print.get_nm_store();
                print.get_currency();
                print.get_trans_header();
                print.coba_print();
            }
            else
            {
                NewFunctionPrinter print = new NewFunctionPrinter();
                print.get_trans_id(id_transaksi);
                print.get_nm_store();
                print.get_currency();
                print.get_trans_header();
                print.coba_print();
            }
        }
        //===========================SHORTCUT TOMBOL=========================================
        private void t_shorcut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "P")
            {
                b_print_Click(null, null);
            }
            if (e.Control && e.KeyCode.ToString() == "N")
            {
                b_new_trans2_Click(null, null);
            }
        }
        //===================================================================================
        private void t_shorcut2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "P")
            {
                b_print_Click(null, null);
            }
            if (e.Control && e.KeyCode.ToString() == "N")
            {
                b_new_trans2_Click(null, null);
            }
        }
    }
}
