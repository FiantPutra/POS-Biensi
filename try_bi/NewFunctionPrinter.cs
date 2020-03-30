using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace try_bi
{
    class NewFunctionPrinter
    {
        koneksi ckon = new koneksi();
        String nm_store, trans_id, art_id, size, pay_type, diskon_format, price_format, SubTotal_format, mata_uang, sign, SubTotal_format2, bank_id, noref, bank_name, bank_name2, bank_id2, noref2, date, time, discount_desc, disc_desc_real;
        int disc, price, sub_total, qty, voucher, total_trans_header, cash, changee, edc, paid_total, edc2;
        int count_nama_store; int jrk_nama_store = 0;
        //MENDAPATKAN CURRENCY DARI TABEL CURRENCY
        public void get_currency()
        {
            string command;            

            //ckon.con.Close();
            //String sql = "SELECT * FROM currency";
            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    sign = ckon.myReader.GetString("NAME");
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
                        sign = ckon.sqlDataRd["NAME"].ToString();
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

            if (sign == "IDR")
            {
                mata_uang = "Rp";
            }
            if (sign == "SGD")
            {
                mata_uang = "$";
            }
            if (sign == "MYR")
            {
                mata_uang = "RM";
            }
        }

        //mendapatkan nama toko dari tabel store
        public void get_nm_store()
        {
            string command;
            
            //ckon.con.Close();
            //String sql = "SELECT * FROM store";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    nm_store = ckon.myReader.GetString("NAME");
            //}
            //ckon.con.Close();

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
                        nm_store = ckon.sqlDataRd["NAME"].ToString();
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

            count_nama_store = nm_store.Count();
            int jrk_smntr_store = 44 - count_nama_store;
            jrk_nama_store = (jrk_smntr_store / 2) * 2;
        }

        //MENDAPATKAN DETAIL DATA TRANSAKSI DARI TRANSAKSI HEADER
        public void get_trans_header()
        {
            string command;            

            //ckon.con.Close();
            //String query = "SELECT * FROM transaction WHERE TRANSACTION_ID = '" + trans_id + "'";
            //ckon.con.Open();
            //ckon.cmd = new MySqlCommand(query, ckon.con);
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    voucher = ckon.myReader.GetInt32("VOUCHER");
            //    total_trans_header = ckon.myReader.GetInt32("TOTAL");
            //    pay_type = ckon.myReader.GetString("PAYMENT_TYPE");
            //    cash = ckon.myReader.GetInt32("CASH");
            //    changee = ckon.myReader.GetInt32("CHANGEE");
            //    bank_id = ckon.myReader.GetString("BANK_NAME");
            //    bank_id2 = ckon.myReader.GetString("BANK_NAME2");
            //    noref = ckon.myReader.GetString("NO_REF");
            //    noref2 = ckon.myReader.GetString("NO_REF2");
            //    edc = ckon.myReader.GetInt32("EDC");
            //    edc2 = ckon.myReader.GetInt32("EDC2");
            //    paid_total = ckon.myReader.GetInt32("TOTAL");
            //    date = ckon.myReader.GetString("DATE");
            //    time = ckon.myReader.GetString("TIME");
            //}
            //ckon.con.Close();

            try
            {
                ckon.sqlCon().Open();
                command = "SELECT * FROM [transaction] WHERE TRANSACTION_ID = '" + trans_id + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        voucher = Convert.ToInt32(ckon.sqlDataRd["VOUCHER"].ToString());
                        total_trans_header = Convert.ToInt32(ckon.sqlDataRd["TOTAL"].ToString());
                        pay_type = ckon.sqlDataRd["PAYMENT_TYPE"].ToString();
                        cash = Convert.ToInt32(ckon.sqlDataRd["CASH"].ToString());
                        changee = Convert.ToInt32(ckon.sqlDataRd["CHANGEE"].ToString());
                        bank_id = ckon.sqlDataRd["BANK_NAME"].ToString();
                        bank_id2 = ckon.sqlDataRd["BANK_NAME2"].ToString();
                        noref = ckon.sqlDataRd["NO_REF"].ToString();
                        noref2 = ckon.sqlDataRd["NO_REF2"].ToString();
                        edc = Convert.ToInt32(ckon.sqlDataRd["EDC"].ToString());
                        edc2 = Convert.ToInt32(ckon.sqlDataRd["EDC2"].ToString());
                        paid_total = Convert.ToInt32(ckon.sqlDataRd["TOTAL"].ToString());
                        date = ckon.sqlDataRd["DATE"].ToString();
                        time = ckon.sqlDataRd["TIME"].ToString();
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

        //MENDAPATKAN ID TRANSAKSI DYG DIKIRIM DARI FORM SEBELUMNYA
        public void get_trans_id(String id_trans)
        {
            trans_id = id_trans;
        }

        //TOMBOL PRINT AKAN MEMANGGIL KE METHOD INI, berguna untuk memanggil method print
        public void coba_print()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();

                PrintDocument printDocument = new PrintDocument();

                printDialog.Document = printDocument; //add the document to the dialog box...        

                printDocument.PrinterSettings.PrinterName = try_bi.Properties.Settings.Default.mPrinter;//seting nama printer
                printDocument.PrintPage += new PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing
                printDocument.Print();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Your Printer Name Is Unavailable", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("Can't connect to the printer: " + try_bi.Properties.Settings.Default.mPrinter);
                MessageBox.Show("Can't connect to the printer #{" + try_bi.Properties.Settings.Default.mPrinter + "}#" + ex.ToString());
                var appLog = new System.Diagnostics.EventLog("Pos Biensi");
                appLog.Source = "Printer";
                appLog.WriteEntry(ex.Message);
                //MessageBox.Show(ex.ToString());
            }

        }

        //METHOD UNTUK MENCETAK STRUK
        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //this prints the reciept
            //int jrk_artc = 0;
            int qty2 = 0; int subtotal2 = 0;
            Graphics graphic = e.Graphics;

            string command;            

            Font font = new Font("Arial", 10); //must use a mono spaced font as the spaces need to line up

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 45;

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            //graphic.DrawString(" STRUK PENJUALAN", new Font("Arial", 10), new SolidBrush(Color.Black), 120, startY + 25);
            graphic.DrawString(" RECEIPT", new Font("Arial", 10), new SolidBrush(Color.Black), 150, startY);
            //e.Graphics.DrawString(" RECEIPT", new Font("Arial", 10), new SolidBrush(Color.Black), 150, sf);

            graphic.DrawString(nm_store.PadLeft(jrk_nama_store), new Font("Arial", 10), new SolidBrush(Color.Black), 70, startY + 20);

            graphic.DrawString("============================================", new Font("Arial", 10), new SolidBrush(Color.Black), startX, startY + 30);

            graphic.DrawString("Trans ID  : " + trans_id, new Font("Arial", 9), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent

            graphic.DrawString("Date        : " + date + "  " + time, new Font("Arial", 9), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0; //make the spacing consistent

            graphic.DrawString("============================================", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0; //make the spacing consistent

            String judul = "Article ID".PadRight(26) + "Size".PadRight(10) + "Qty".PadRight(10) + "Disc".PadRight(12) + "Price".PadRight(12) + "Sub Total";
            graphic.DrawString(judul, new Font("Arial", 9), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0; //make the spacing consistent

            graphic.DrawString("============================================", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0; //make the spacing consistent

            //ckon.con.Close();
            //String sql = "SELECT transaction_line.ARTICLE_ID, transaction_line.QUANTITY, transaction_line.DISCOUNT, transaction_line.PRICE, transaction_line.SUBTOTAL,transaction_line.DISCOUNT_DESC, article.SIZE FROM article INNER JOIN transaction_line ON article.ARTICLE_ID = transaction_line.ARTICLE_ID WHERE transaction_line.TRANSACTION_ID = '" + trans_id + "'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            try
            {
                ckon.sqlCon().Open();
                command = "SELECT transaction_line.ARTICLE_ID, transaction_line.QUANTITY, transaction_line.DISCOUNT, transaction_line.PRICE, transaction_line.SUBTOTAL,transaction_line.DISCOUNT_DESC, article.SIZE " +
                            "FROM article INNER JOIN transaction_line ON article.ARTICLE_ID = transaction_line.ARTICLE_ID WHERE transaction_line.TRANSACTION_ID = '" + trans_id + "'";
                CRUD sql = new CRUD();
                ckon.sqlDataRd = sql.ExecuteDataReader(command, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        int jrk_size = 0; int jrk_qty = 0; int jrk_disc = 0; int jrk_price = 0; int jrk_subTotal = 0;

                        art_id = ckon.sqlDataRd["ARTICLE_ID"].ToString();
                        size = ckon.sqlDataRd["SIZE"].ToString();
                        qty = Convert.ToInt32(ckon.sqlDataRd["QUANTITY"].ToString());
                        disc = Convert.ToInt32(ckon.sqlDataRd["DISCOUNT"].ToString());
                        price = Convert.ToInt32(ckon.sqlDataRd["PRICE"].ToString());
                        sub_total = Convert.ToInt32(ckon.sqlDataRd["SUBTOTAL"].ToString());
                        disc_desc_real = ckon.sqlDataRd["DISCOUNT_DESC"].ToString();

                        discount_desc = disc_desc_real.Length > 4 ? disc_desc_real.Substring(0, 4) : disc_desc_real;
                        //FORMAT ANGKA UNTUK DIBERIKAN KOMA DI JUMLAH UANG
                        diskon_format = string.Format("{0:#,###}", disc);
                        price_format = string.Format("{0:#,###}", price);
                        SubTotal_format = string.Format("{0:#,###}", sub_total);

                        //hitung total qty dan subtotal
                        qty2 = qty2 + qty;
                        subtotal2 = subtotal2 + sub_total;

                        //FORMAT SUBTOTAL2 KE DALAM FORMAT MATA UANG, DENGAN KOMA
                        SubTotal_format2 = string.Format("{0:#,###}", subtotal2);

                        //hitung panjang karakter size dan article id
                        int count_art_id = art_id.Count();
                        int count_size = size.Count();
                        int count_disc = discount_desc.Count();
                        String disc_string;
                        int count_price = Convert.ToString(price).Count();
                        int count_subTotal = Convert.ToString(sub_total).Count();
                        //============HITUNG ARTICLE, JIKA KURANG DARI 15, hitung
                        if (count_art_id < 15)
                        {
                            int smntr = 15 - count_art_id;
                            jrk_size = (smntr * 2) + 13;
                        }
                        else
                        {
                            jrk_size = 11;
                        }
                        //=============HITUNG KARAKTER SIZE, JIKA LEBIH DARI 1, HITUNG==
                        if (count_size > 1)
                        {
                            int smntr2 = count_size - 1;
                            jrk_qty = 15 - smntr2;
                        }
                        else
                        {
                            jrk_qty = 15;
                        }
                        //==========================UNTUK DISKON=============
                        if (count_disc == 0 || count_disc == 1)
                        {
                            disc_string = "0";
                            jrk_disc = 13;

                            //********************UNTUK HARGA*****************************
                            if (count_price == 5)
                            {
                                jrk_price = 25;

                                //+++++++++++++++++++++++++ UNTUK SUB TOTAL+++++++++++++++++++++++++
                                if (count_subTotal == 5)
                                {
                                    jrk_subTotal = 20;
                                }
                                else if (count_subTotal == 6)
                                {
                                    jrk_subTotal = 19;
                                }
                                else
                                {
                                    jrk_subTotal = 18;
                                }
                            }
                            else
                            {
                                jrk_price = 24;

                                //+++++++++++++++++++++++++ UNTUK SUB TOTAL+++++++++++++++++++++++++
                                if (count_subTotal == 5)
                                {
                                    jrk_subTotal = 20;
                                }
                                else if (count_subTotal == 6)
                                {
                                    jrk_subTotal = 19;
                                }
                                else
                                {
                                    jrk_subTotal = 18;
                                }
                            }
                        }
                        else if (count_disc == 3 || count_disc == 4)
                        {
                            disc_string = discount_desc;
                            jrk_disc = 16;

                            //********************UNTUK HARGA*****************************
                            if (count_price == 5)
                            {
                                jrk_price = 19;

                                //+++++++++++++++++++++++++ UNTUK SUB TOTAL+++++++++++++++++++++++++
                                if (count_subTotal == 5)
                                {
                                    jrk_subTotal = 20;
                                }
                                else if (count_subTotal == 6)
                                {
                                    jrk_subTotal = 19;
                                }
                                else
                                {
                                    jrk_subTotal = 18;
                                }
                            }
                            else
                            {
                                jrk_price = 18;

                                //+++++++++++++++++++++++++ UNTUK SUB TOTAL+++++++++++++++++++++++++
                                if (count_subTotal == 5)
                                {
                                    jrk_subTotal = 20;
                                }
                                else if (count_subTotal == 6)
                                {
                                    jrk_subTotal = 19;
                                }
                                else
                                {
                                    jrk_subTotal = 18;
                                }
                            }
                        }
                        else
                        {
                            disc_string = string.Format("{0:#,###}", Convert.ToInt32(discount_desc));
                            jrk_disc = 17;

                            //********************UNTUK HARGA*****************************
                            if (count_price == 5)
                            {
                                jrk_price = 17;

                                //+++++++++++++++++++++++++ UNTUK SUB TOTAL+++++++++++++++++++++++++
                                if (count_subTotal == 5)
                                {
                                    jrk_subTotal = 20;
                                }
                                else if (count_subTotal == 6)
                                {
                                    jrk_subTotal = 19;
                                }
                                else
                                {
                                    jrk_subTotal = 18;
                                }
                            }
                            else
                            {
                                jrk_price = 16;

                                //+++++++++++++++++++++++++ UNTUK SUB TOTAL+++++++++++++++++++++++++
                                if (count_subTotal == 5)
                                {
                                    jrk_subTotal = 20;
                                }
                                else if (count_subTotal == 6)
                                {
                                    jrk_subTotal = 19;
                                }
                                else
                                {
                                    jrk_subTotal = 18;
                                }
                            }
                        }

                        String LINE = art_id.PadRight(0) + size.PadLeft(jrk_size).PadRight(0) + qty.ToString().PadLeft(jrk_qty).PadRight(0) + disc_string.PadLeft(jrk_disc) + price_format.PadLeft(jrk_price) + SubTotal_format.PadLeft(jrk_subTotal);
                        graphic.DrawString(LINE, new Font("Arial", 7), new SolidBrush(Color.Black), startX, startY + offset);
                        offset = offset + (int)fontHeight + 5;
                    }
                }
            }
            catch (Exception er)
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
            //ckon.con.Close();

            graphic.DrawString("============================================", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 0;

            String total_qty = "Sub Total".PadRight(45) + qty2.ToString().PadRight(37) + mata_uang.PadRight(7) + SubTotal_format2;
            graphic.DrawString(total_qty, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 3;

            //PERCABANGAN UNTUK MEMBUAT JARAK DI VOUCHER
            if (voucher == 0)
            {
                graphic.DrawString("Voucher".PadRight(103) + "0", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;
            }
            else
            {
                String format_voucher = string.Format("{0:#,###}", voucher);
                graphic.DrawString("Voucher".PadRight(83) + mata_uang.PadRight(7) + format_voucher, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;
            }

            String format_total_header = string.Format("{0:#,###}", total_trans_header);
            graphic.DrawString("Total".PadRight(86) + mata_uang.PadRight(7) + format_total_header, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 12;

            //PERCABANGAN UNTUK MELIAHT METHOD PEMBAYARAN DAN MENCETAK BARIS SESUAI METHOD PEMBAYARAN
            if (pay_type == "0")
            {
                //JIKA METHOD PEMBAYARAN ADALAH CASH
                graphic.DrawString("Payment Method  : CASH", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                String paid = string.Format("{0:#,###}", cash);
                graphic.DrawString("Paid".PadRight(86) + mata_uang.PadRight(7) + paid, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                //BUAT PERCABANGAN UNTUK MENGATUR MARGIN UNTUK KEMBALIAN, JIKA KEMBALIAN O ATAU TIDAK O
                if (changee == 0)
                {
                    graphic.DrawString("Change".PadRight(103) + "0", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                    offset = offset + (int)fontHeight + 3;
                }
                else
                {
                    String change = string.Format("{0:#,###}", changee);
                    graphic.DrawString("Change".PadRight(83) + mata_uang.PadRight(7) + change, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                    offset = offset + (int)fontHeight + 3;
                }

            }
            else if (pay_type == "1")
            {
                //JIKA METHOD PAYMENTN ADALAH 1 ATAU EDC
                //ckon.con.Close();
                //String query_edc = "SELECT * FROM bank WHERE BANK_ID = '" + bank_id + "'";
                //ckon.cmd = new MySqlCommand(query_edc, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    bank_name = ckon.myReader.GetString("BANK_NAME");
                //}
                //ckon.con.Close();

                try
                {
                    ckon.sqlCon().Open();
                    command = "SELECT * FROM bank WHERE BANK_ID = '" + bank_id + "'";
                    CRUD sql_bank = new CRUD();
                    ckon.sqlDataRd = sql_bank.ExecuteDataReader(command, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            bank_name = ckon.sqlDataRd["BANK_NAME"].ToString();
                        }
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (ckon.sqlDataRd != null)
                        ckon.sqlDataRd.Close();

                    if (ckon.sqlCon().State == ConnectionState.Open)
                        ckon.sqlCon().Close();
                }

                graphic.DrawString("Payment Method  : EDC", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                graphic.DrawString("Bank                     : " + bank_name + " - " + noref, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                String paid_edc = string.Format("{0:#,###}", edc);
                graphic.DrawString("Paid".PadRight(86) + mata_uang.PadRight(7) + paid_edc, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                graphic.DrawString("Change".PadRight(103) + "0", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;
            }
            else if (pay_type == "2")
            {
                //JIKA METHOD ADALAH 2 ATAU SPLIT
                //ckon.con.Close();
                //String query_edc = "SELECT * FROM bank WHERE BANK_ID = '" + bank_id + "'";
                //ckon.cmd = new MySqlCommand(query_edc, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    bank_name = ckon.myReader.GetString("BANK_NAME");
                //}
                //ckon.con.Close();

                try
                {
                    ckon.sqlCon().Open();
                    command = "SELECT * FROM bank WHERE BANK_ID = '" + bank_id + "'";
                    CRUD sql_bank = new CRUD();
                    ckon.sqlDataRd = sql_bank.ExecuteDataReader(command, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            bank_name = ckon.sqlDataRd["BANK_NAME"].ToString();
                        }
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (ckon.sqlDataRd != null)
                        ckon.sqlDataRd.Close();

                    if (ckon.sqlCon().State == ConnectionState.Open)
                        ckon.sqlCon().Close();
                }

                graphic.DrawString("Payment Method  : SPLIT", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                String paid = string.Format("{0:#,###}", cash);
                graphic.DrawString("Cash".PadRight(84) + mata_uang.PadRight(7) + paid, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                String paid_edc = string.Format("{0:#,###}", edc);
                graphic.DrawString("Bank                     : " + bank_name + " - " + noref + " - " + mata_uang.PadRight(5) + paid_edc, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                String total_bayar = string.Format("{0:#,###}", paid_total);
                graphic.DrawString("Paid".PadRight(85) + mata_uang.PadRight(7) + total_bayar, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                graphic.DrawString("Change".PadRight(102) + "0", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;
            }
            else
            {
                //JIKA METHOD PAYMENT ADALAH 3 ATAU SPLIT EDC
                //ckon.con.Close();
                //String query_edc = "SELECT * FROM bank WHERE BANK_ID = '" + bank_id + "'";
                //ckon.cmd = new MySqlCommand(query_edc, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    bank_name = ckon.myReader.GetString("BANK_NAME");
                //}
                //ckon.con.Close();
                //String query_edc2 = "SELECT * FROM bank WHERE BANK_ID = '" + bank_id2 + "'";
                //ckon.cmd = new MySqlCommand(query_edc2, ckon.con);
                //ckon.con.Open();
                //ckon.myReader = ckon.cmd.ExecuteReader();
                //while (ckon.myReader.Read())
                //{
                //    bank_name2 = ckon.myReader.GetString("BANK_NAME");
                //}
                //ckon.con.Close();

                try
                {
                    ckon.sqlCon().Open();
                    command = "SELECT * FROM bank WHERE BANK_ID = '" + bank_id + "'";
                    CRUD sql_bank1 = new CRUD();
                    ckon.sqlDataRd = sql_bank1.ExecuteDataReader(command, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            bank_name = ckon.sqlDataRd["BANK_NAME"].ToString();
                        }
                    }

                    command = "SELECT * FROM bank WHERE BANK_ID = '" + bank_id2 + "'";
                    CRUD sql_bank2 = new CRUD();
                    ckon.sqlDataRd = sql_bank2.ExecuteDataReader(command, ckon.sqlCon());

                    if (ckon.sqlDataRd.HasRows)
                    {
                        while (ckon.sqlDataRd.Read())
                        {
                            bank_name2 = ckon.sqlDataRd["BANK_NAME"].ToString();
                        }
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (ckon.sqlDataRd != null)
                        ckon.sqlDataRd.Close();

                    if (ckon.sqlCon().State == ConnectionState.Open)
                        ckon.sqlCon().Close();
                }

                graphic.DrawString("Payment Method  : SPLIT EDC", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                String paid_edc = string.Format("{0:#,###}", edc);
                graphic.DrawString("Bank1                   : " + bank_name + " - " + noref + " - " + mata_uang.PadRight(5) + paid_edc, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                String paid_edc2 = string.Format("{0:#,###}", edc2);
                //graphic.DrawString("Bank                     : " + bank_name2 + " - " + noref2.PadRight(29) + mata_uang.PadRight(7) + paid_edc2, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                graphic.DrawString("Bank2                   : " + bank_name2 + " - " + noref2 + " - " + mata_uang.PadRight(5) + paid_edc2, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                String total_bayar = string.Format("{0:#,###}", paid_total);
                graphic.DrawString("Paid".PadRight(86) + mata_uang.PadRight(7) + total_bayar, new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;

                graphic.DrawString("Change".PadRight(103) + "0", new Font("Arial", 8), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight + 3;
            }

            graphic.DrawString("Return and Exchanges", new Font("Arial", 8), new SolidBrush(Color.Black), 130, startY + offset);
            offset = offset + (int)fontHeight + 3;

            graphic.DrawString("Within 7 Days With Receipt And Tags Attached", new Font("Arial", 8), new SolidBrush(Color.Black), 70, startY + offset);
            offset = offset + (int)fontHeight + 3;

            graphic.DrawString("Thank You For Coming", new Font("Arial", 8), new SolidBrush(Color.Black), 130, startY + offset);
            offset = offset + (int)fontHeight + 3;

            graphic.DrawString("WWW.3SECOND.CO.ID", new Font("Arial", 8), new SolidBrush(Color.Black), 128, startY + offset);
            offset = offset + (int)fontHeight + 3;


        }
    }
}
