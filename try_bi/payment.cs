using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PrinterUtility;
using System.Collections;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace try_bi
{
    public partial class payment : Form
    {
        public static Form1 f1;
        public int tot_bl, cash, ubah2, int_subtotal, field_diskon2;
        double value1, total, change, real_diskon;
        public String new_total, id_trans,value1_string, cust_Id;
        String method_payment, nilai_quick_cash, a_name, a_date, a_jam, a_total, a_sub, a_id, a_subtotal, type, from_diskon, store_name, store_add, store_code;
        DateTime mydate = DateTime.Now;
        DateTime myhour = DateTime.Now;
        koneksi ckon = new koneksi();
        public payment(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //================METHOD HITUNG=========================================
        public void hitung()
        {
            String noref = "-";
            if (value1 < total)
            {
                MessageBox.Show("Your total payment is less than total spend");
            }
            else
            {
                if (method_payment == "cash")
                {
                    change = value1 - total;
                    String change2 = change.ToString();
                    update_status();
                    value1_string = value1.ToString();
                    textBox1.Text = "";
                    uc_kembalian uc_k = new uc_kembalian(f1);
                    f1.p_kanan.Controls.Clear();
                    if (!f1.p_kanan.Controls.Contains(uc_kembalian.Instance))
                    {
                        f1.p_kanan.Controls.Add(uc_kembalian.Instance);
                        uc_kembalian.Instance.Dock = DockStyle.Fill;
                        uc_kembalian.Instance.kembali(value1_string, change2, id_trans);
                        uc_kembalian.Instance.for_struk(type, noref, value1, change);
                        uc_kembalian.Instance.Show();

                    }
                    else
                    {
                        uc_kembalian.Instance.Show();
                    }
                    //fungsi_cetak_struk();
                        
                }
                else
                {
                    
                    change = value1 - total;
                    String change2 = change.ToString();
                    update_status();
                    value1_string = value1.ToString();
                    textBox1.Text = "";
                    uc_kembalian uc_k = new uc_kembalian(f1);
                    f1.p_kanan.Controls.Clear();
                    if (!f1.p_kanan.Controls.Contains(uc_kembalian.Instance))
                    {
                        f1.p_kanan.Controls.Add(uc_kembalian.Instance);
                        uc_kembalian.Instance.Dock = DockStyle.Fill;
                        uc_kembalian.Instance.kembali(value1_string, change2, id_trans);
                        uc_kembalian.Instance.for_struk(type, noref, value1, change);
                        uc_kembalian.Instance.Show();

                    }
                    else
                    {
                        uc_kembalian.Instance.Show();
                    }
                    //fungsi_cetak_struk();

                }
               
            }
           
        }
        //======================================================================
        private void payment_Load(object sender, EventArgs e)
        {
            string command;
            LinkApi link = new LinkApi();

            this.ActiveControl = textBox1;
            textBox1.Focus();
            type = "Cash";
            //textBox1.Text = "";
            //ckon.con.Close();
            //String sql2 = "SELECT * FROM store";
            //ckon.cmd = new MySqlCommand(sql2, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    if (ckon.myReader.HasRows)
            //    {
            //        while (ckon.myReader.Read())
            //        {
            //            store_name = ckon.myReader.GetString("NAME");
            //            store_add = ckon.myReader.GetString("ADDRESS");
            //        }
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ MessageBox.Show("Failed when get data from store data"); }

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
                        store_code = ckon.sqlDataRd["CODE"].ToString();
                        store_name = ckon.sqlDataRd["NAME"].ToString();
                        store_add = ckon.sqlDataRd["ADDRESS"].ToString();
                    }
                }
                else
                {
                    store_code = link.storeId;
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
        }
        //======================================================================
        private void payment_KeyDown(object sender, KeyEventArgs e)
        {
            //===========BUTTON CHARGE================
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_ok2_Click(null, null);
            }
            //=======BUTTON BACK===============
            if (e.Control && e.KeyCode.ToString() == "B")
            {
                b_cancel2_Click(null, null);
            }
        }

        //======================================================================
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                {

                }
                else
                {
                    value1 = double.Parse(textBox1.Text);
                    textBox1.Text = string.Format("{0:#,###}", value1);
                    textBox1.Select(textBox1.Text.Length, 0);
                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                textBox1.Text = "";
            }
        }

        //======================================================================
        //======BUTTON OK BARU======
        private void b_ok2_Click(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Input The Value");
            }
            else
            {
                hitung();

                String cmd_deletePromo = "DELETE FROM promotion";
                sql.ExecuteNonQuery(cmd_deletePromo);

                String cmd_deleteTmp = "DROP TABLE [tmp].["+ store_code +"]";
                sql.ExecuteNonQuery(cmd_deleteTmp);

                this.Close();
            }
        }

        private void b_cancel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //======================================================================
        public void fungsi_cetak_struk()
        {
            LinkApi ls = new LinkApi();
            if (ls.print_default == "1")
            {
                PrintThermal print = new PrintThermal();
                print.get_trans_id(id_trans);
                print.get_nm_store();
                print.get_currency();
                print.get_trans_header();
                print.coba_print();
            }
            else
            {
                NewFunctionPrinter print = new NewFunctionPrinter();
                print.get_trans_id(id_trans);
                print.get_nm_store();
                print.get_currency();
                print.get_trans_header();
                print.coba_print();
            }
        }
        //======================================================================
        public void aksi_total(double total2, String method, double diskon2, int field_diskon)
        {
            
            method_payment = method;
            total = total2;
            real_diskon = diskon2;
            field_diskon2 = field_diskon;
            l_total.Text = string.Format("{0:#,###}" + ",00", total);
            if (diskon2 == 0)
            {
                from_diskon = "0,00";
            }
            else { from_diskon = real_diskon.ToString("C2", CultureInfo.GetCultureInfo("id-ID")); }
            //l_total.Text = method_total;
        }
        //======================================================================

        public void quick_cash(String total2)
        {
            String a = total2;
            textBox1.Text = a;
            nilai_quick_cash = total2;
            string replacedString = nilai_quick_cash.Replace("(A)", "");
            string clean = Regex.Replace(nilai_quick_cash, "[^0-9 ]", "");
            value1 = double.Parse(clean);
            //total = method_total;
            //l_total.Text = total.ToString("C2", CultureInfo.GetCultureInfo("id-ID"));
            //l_total.Text = method_total;
            //b_cash.Text = cash;
        }
        //======================================================================

        //======================================================================
        public void update_status()
        {
            int transType;

            if (cust_Id != "")
                transType = 3;
            else
                transType = 1;

            String sql= "UPDATE [transaction] SET DISCOUNT='"+ field_diskon2 +"',STATUS = '1', TOTAL='"+ total +"', PAYMENT_TYPE='0', CASH='"+ value1 +"', CHANGEE='"+ change +"', TRANSACTION_TYPE = '"+ transType +"' WHERE TRANSACTION_ID ='" + id_trans + "'";
            CRUD edit = new CRUD();
            edit.ExecuteNonQuery(sql);
        }
        //======================================================================

        //===========================FOR PRINT STRUCK===========================
        //public byte[] CutPage()
        //{
        //    List<byte> oby = new List<byte>();
        //    oby.Add(Convert.ToByte(Convert.ToChar(0x1D)));
        //    oby.Add(Convert.ToByte('V'));
        //    oby.Add((byte)66);
        //    oby.Add((byte)3);
        //    return oby.ToArray();
        //}

        //public byte[] GetLogo(string LogoPath)
        //{
        //    List<byte> byteList = new List<byte>();
        //    if (!File.Exists(LogoPath))
        //        return null;
        //    BitmapData data = GetBitmapData(LogoPath);
        //    BitArray dots = data.Dots;
        //    byte[] width = BitConverter.GetBytes(data.Width);

        //    int offset = 0;
        //    MemoryStream stream = new MemoryStream();
        //    // BinaryWriter bw = new BinaryWriter(stream);
        //    byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
        //    //bw.Write((char));
        //    byteList.Add(Convert.ToByte('@'));
        //    //bw.Write('@');
        //    byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
        //    // bw.Write((char)0x1B);
        //    byteList.Add(Convert.ToByte('3'));
        //    //bw.Write('3');
        //    //bw.Write((byte)24);
        //    byteList.Add((byte)24);
        //    while (offset < data.Height)
        //    {
        //        byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
        //        byteList.Add(Convert.ToByte('*'));
        //        //bw.Write((char)0x1B);
        //        //bw.Write('*');         // bit-image mode
        //        byteList.Add((byte)33);
        //        //bw.Write((byte)33);    // 24-dot double-density
        //        byteList.Add(width[0]);
        //        byteList.Add(width[1]);
        //        //bw.Write(width[0]);  // width low byte
        //        //bw.Write(width[1]);  // width high byte

        //        for (int x = 0; x < data.Width; ++x)
        //        {
        //            for (int k = 0; k < 3; ++k)
        //            {
        //                byte slice = 0;
        //                for (int b = 0; b < 8; ++b)
        //                {
        //                    int y = (((offset / 8) + k) * 8) + b;
        //                    // Calculate the location of the pixel we want in the bit array.
        //                    // It'll be at (y * width) + x.
        //                    int i = (y * data.Width) + x;

        //                    // If the image is shorter than 24 dots, pad with zero.
        //                    bool v = false;
        //                    if (i < dots.Length)
        //                    {
        //                        v = dots[i];
        //                    }
        //                    slice |= (byte)((v ? 1 : 0) << (7 - b));
        //                }
        //                byteList.Add(slice);
        //                //bw.Write(slice);
        //            }
        //        }
        //        offset += 24;
        //        byteList.Add(Convert.ToByte(0x0A));
        //        //bw.Write((char));
        //    }
        //    // Restore the line spacing to the default of 30 dots.
        //    byteList.Add(Convert.ToByte(0x1B));
        //    byteList.Add(Convert.ToByte('3'));
        //    //bw.Write('3');
        //    byteList.Add((byte)30);
        //    return byteList.ToArray();
        //    //bw.Flush();
        //    //byte[] bytes = stream.ToArray();
        //    //return logo + Encoding.Default.GetString(bytes);
        //}

        //public BitmapData GetBitmapData(string bmpFileName)
        //{
        //    using (var bitmap = (Bitmap)Bitmap.FromFile(bmpFileName))
        //    {
        //        var threshold = 127;
        //        var index = 0;
        //        double multiplier = 570; // this depends on your printer model. for Beiyang you should use 1000
        //        double scale = (double)(multiplier / (double)bitmap.Width);
        //        int xheight = (int)(bitmap.Height * scale);
        //        int xwidth = (int)(bitmap.Width * scale);
        //        var dimensions = xwidth * xheight;
        //        var dots = new BitArray(dimensions);

        //        for (var y = 0; y < xheight; y++)
        //        {
        //            for (var x = 0; x < xwidth; x++)
        //            {
        //                var _x = (int)(x / scale);
        //                var _y = (int)(y / scale);
        //                var color = bitmap.GetPixel(_x, _y);
        //                var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
        //                dots[index] = (luminance < threshold);
        //                index++;
        //            }
        //        }

        //        return new BitmapData()
        //        {
        //            Dots = dots,
        //            Height = (int)(bitmap.Height * scale),
        //            Width = (int)(bitmap.Width * scale)
        //        };
        //    }
        //}

        //public class BitmapData
        //{
        //    public BitArray Dots
        //    {
        //        get;
        //        set;
        //    }

        //    public int Height
        //    {
        //        get;
        //        set;
        //    }

        //    public int Width
        //    {
        //        get;
        //        set;
        //    }
        //}
    }
}
