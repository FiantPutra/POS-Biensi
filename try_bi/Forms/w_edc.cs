using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class w_edc : Form
    {
        public static Form1 f1;
        public String id_trans, a_name, a_date, a_jam, a_total, a_sub, a_id, a_subtotal, type, from_diskon, store_name, store_add, id_bank, cust_Id;
        int total, int_subtotal, real_diskon, field_diskon2;
        DateTime mydate = DateTime.Now;
        DateTime myhour = DateTime.Now;
        koneksi ckon = new koneksi();

        public w_edc(Form1 form1)
        {
            f1 = form1;
            InitializeComponent();
        }
        //mengambil id bank berdasarkan nama bank yang dipilih===
        public void get_id_bank()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM bank WHERE BANK_NAME LIKE '%" + combo_bank.Text + "%'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_bank = ckon.sqlDataRd["BANK_ID"].ToString();
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
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while(ckon.myReader.Read())
            //{
            //    id_bank = ckon.myReader.GetString("BANK_ID");
            //}
            //ckon.con.Close();
        }
        //===========menampilkan data bank ke combobox===================================
        public void isi_combo()
        {
            CRUD sql = new CRUD();

            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM bank";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        String name = ckon.sqlDataRd["BANK_NAME"].ToString();
                        combo_bank.Items.Add(name);
                    }
                }

                combo_bank.SelectedIndex = 0;
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

            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //try
            //{
            //    ckon.con.Open();
            //    ckon.myReader = ckon.cmd.ExecuteReader();
            //    while (ckon.myReader.Read())
            //    {
            //        String name = ckon.myReader.GetString("BANK_NAME");
            //        combo_bank.Items.Add(name);
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ MessageBox.Show("Data gagal ditambilkan untuk combobox"); }
            //combo_bank.SelectedIndex = 0;
        }
        private void b_cancel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void hitung()
        { Double change = 0;
            get_id_bank();
            update_status();
            uc_kembalian uc_k = new uc_kembalian(f1);
            f1.p_kanan.Controls.Clear();
            //bunifuFlatButton1.selected = true;
            if (!f1.p_kanan.Controls.Contains(uc_kembalian.Instance))
            {
                f1.p_kanan.Controls.Add(uc_kembalian.Instance);
                uc_kembalian.Instance.Dock = DockStyle.Fill;
                uc_kembalian.Instance.edc(id_trans, total, combo_bank.Text);
                uc_kembalian.Instance.for_struk(type, t_rev.Text, total, change);
                uc_kembalian.Instance.Show();
                //uc_transaction.Instance.BringToFront();
            }
            else
            {
                uc_kembalian.Instance.edc(id_trans, total, combo_bank.Text);
                uc_kembalian.Instance.for_struk(type, t_rev.Text, total, change);
                uc_kembalian.Instance.Show();
            }            
        }

        private void w_edc_Load(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            isi_combo();
            this.ActiveControl = combo_bank;
            combo_bank.Focus();
            type = "EDC";
            //ckon.con.Close();
            try
            {
                ckon.sqlCon().Open();
                String cmd = "SELECT * FROM store";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        store_name = ckon.sqlDataRd["NAME"].ToString();
                        store_add = ckon.sqlDataRd["ADDRESS"].ToString();
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
        }
        public void aksi_total(int method_total, int diskon, int field_diskon)
        {
            real_diskon = diskon;
            total = method_total;
            field_diskon2 = field_diskon;
            if(real_diskon == 0)
            { from_diskon = "0,00"; }
            else
            { from_diskon = real_diskon.ToString("C2", CultureInfo.GetCultureInfo("id-ID")); }
            l_total.Text = string.Format("{0:#,###}" + ",00", method_total);
        }
        private void b_ok2_Click(object sender, EventArgs e)
        {
            if (t_rev.Text == "")
            {
                MessageBox.Show("Please Input The Value");
            }
            else
            {
                hitung();
                //======================================================
                //NewFunctionPrinter print = new NewFunctionPrinter();
                //print.get_trans_id(id_trans);
                //print.get_nm_store();
                //print.get_currency();
                //print.get_trans_header();
                //print.coba_print();
                //======================================================
                this.Close();
            }
        }

        public void update_status()
        {
            int transType;

            if (cust_Id != "")
                transType = 3;
            else
                transType = 1;

            String cmd_update = "UPDATE [transaction] SET  DISCOUNT ='"+ field_diskon2 +"',TOTAL='"+ total +"', STATUS='1', PAYMENT_TYPE='1', EDC='"+ total +"' ,BANK_NAME='"+ id_bank +"', NO_REF='"+ t_rev.Text + "', TRANSACTION_TYPE = '" + transType + "' WHERE TRANSACTION_ID ='" + id_trans + "'";
            CRUD update = new CRUD();
            update.ExecuteNonQuery(cmd_update);
        }
        //==========================================================================================================================
        private void w_edc_KeyDown(object sender, KeyEventArgs e)
        {
            //==========BUTTON CHARGE=============
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_ok2_Click(null, null);
            }
            //============BUTTON BACK==========
            if (e.Control && e.KeyCode.ToString() == "B")
            {
                b_cancel2_Click(null, null);
            }
        }
        //==========================================================================================================================
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
