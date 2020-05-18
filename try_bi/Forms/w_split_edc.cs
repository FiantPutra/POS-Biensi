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
    public partial class w_split_edc : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        DateTime mydate = DateTime.Now;
        DateTime myhour = DateTime.Now;
        CRUD edit = new CRUD();
        Double edc1,  edc2;
        public string id_trans, store_name, store_add,a_date,a_jam,a_total,a_subtotal,a_sub,a_name,a_id, from_diskon, id_bank, id_bank2, cust_Id;
        int tot_bel,int_subtotal, real_diskon, field_diskon2;
        public w_split_edc(Form1 form1)
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
                String cmd_bank1 = "SELECT * FROM bank WHERE BANK_NAME LIKE '%" + combo_bank1.Text + "%'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_bank1, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_bank = ckon.sqlDataRd["BANK_ID"].ToString();
                    }
                }

                String cmd_bank2 = "SELECT * FROM bank WHERE BANK_NAME LIKE '%" + combo_bank2.Text + "%'";
                ckon.sqlDataRd = sql.ExecuteDataReader(cmd_bank2, ckon.sqlCon());

                if (ckon.sqlDataRd.HasRows)
                {
                    while (ckon.sqlDataRd.Read())
                    {
                        id_bank2 = ckon.sqlDataRd["BANK_ID"].ToString();
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

            ///memilih id bank dari combo box bank 1======
            //String sql = "SELECT * FROM bank WHERE BANK_NAME LIKE '%" + combo_bank1.Text + "%'";
            //ckon.cmd = new MySqlCommand(sql, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while (ckon.myReader.Read())
            //{
            //    id_bank = ckon.myReader.GetString("BANK_ID");
            //}
            //ckon.con.Close();
            ////======mengambil id bank dari combo bank 2====
            //String sql2 = "SELECT * FROM bank WHERE BANK_NAME LIKE '%"+ combo_bank2.Text +"%'";
            //ckon.cmd = new MySqlCommand(sql2, ckon.con);
            //ckon.con.Open();
            //ckon.myReader = ckon.cmd.ExecuteReader();
            //while(ckon.myReader.Read())
            //{
            //    id_bank2 = ckon.myReader.GetString("BANK_ID");
            //}
            //ckon.con.Close();
        }
        //=================isi combo box dengan data bank==========================================
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
                        combo_bank1.Items.Add(name);
                        combo_bank2.Items.Add(name);
                    }
                }
                combo_bank1.SelectedIndex = 0;
                combo_bank2.SelectedIndex = 0;
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
            //        combo_bank1.Items.Add(name);
            //        combo_bank2.Items.Add(name);
            //    }
            //    ckon.con.Close();
            //}
            //catch
            //{ MessageBox.Show("Data gagal ditambilkan untuk combobox"); }
            //combo_bank1.SelectedIndex = 0;
            //combo_bank2.SelectedIndex = 0;
        }
        //==========================button ok======================================================
        private void b_ok2_Click(object sender, EventArgs e)
        {
            if (t_edc1.Text == "")
            {
                MessageBox.Show("Please Input The Value");
            }
            else
            {

                if (combo_bank1.Text == combo_bank2.Text)
                {
                    MessageBox.Show("Banks Should Not Be The Same");
                }
                else
                {
                    if (t_edc1.Text.Contains("-") == true || t_edc2.Text.Contains("-") == true)
                    {
                        MessageBox.Show("Amount Can't Be Minus");
                    }
                    else
                    {
                        get_id_bank();
                        hitung();
                        //=====================================================================
                        //NewFunctionPrinter print = new NewFunctionPrinter();
                        //print.get_trans_id(id_trans);
                        //print.get_nm_store();
                        //print.get_currency();
                        //print.get_trans_header();
                        //print.coba_print();
                        this.Close();
                    }

                }

            }
        }
        //==============fungsi hitung dan update transaction=====================================
        public void hitung()
        {
            int transType;

            if (cust_Id != "")
                transType = 3;
            else
                transType = 1;

            String cmd_update = "UPDATE [transaction] SET DISCOUNT='"+ field_diskon2 +"',TOTAL='" + tot_bel + "', STATUS='1', PAYMENT_TYPE='3', EDC='" + edc1 + "', EDC2='"+ edc2 +"' ,BANK_NAME='" + id_bank + "',BANK_NAME2='"+ id_bank2 +"' ,NO_REF='" + t_rev.Text + "',NO_REF2='"+ t_rev2.Text + "', TRANSACTION_TYPE = '" + transType + "' WHERE TRANSACTION_ID ='" + id_trans + "'";
            CRUD edit = new CRUD();
            edit.ExecuteNonQuery(cmd_update);
            uc_kembalian uc_k = new uc_kembalian(f1);
            f1.p_kanan.Controls.Clear();
            //bunifuFlatButton1.selected = true;
            if (!f1.p_kanan.Controls.Contains(uc_kembalian.Instance))
            {
                f1.p_kanan.Controls.Add(uc_kembalian.Instance);
                uc_kembalian.Instance.Dock = DockStyle.Fill;
                uc_kembalian.Instance.split_edc(id_trans, edc1, edc2, combo_bank1.Text, combo_bank2.Text);
                uc_kembalian.Instance.Show();
                //uc_transaction.Instance.BringToFront();
            }
            else
            {
                uc_kembalian.Instance.split_edc(id_trans, edc1, edc2, combo_bank1.Text, combo_bank2.Text);
                uc_kembalian.Instance.Show();
            }

        }
        //==========================button cancel==================================================
        private void b_cancel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //=======================FORM LOAD========================================================
        private void w_split_edc_Load(object sender, EventArgs e)
        {
            CRUD sql = new CRUD();

            isi_combo();
            this.ActiveControl = t_edc1;
            t_edc1.Focus();
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
                MessageBox.Show("No connection to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        //=============DAPATKAN NILAI DARI FORM SEBELUM NYA (CHARGE)==================================
        public void get_nilai(int totals, String id, int diskon, int field_diskon)
        {
            real_diskon = diskon;
            id_trans = id;
            tot_bel = totals;
            field_diskon2 = field_diskon;
            if (real_diskon == 0)
            { from_diskon = "0,00"; }
            else
            { from_diskon = real_diskon.ToString("C2", CultureInfo.GetCultureInfo("id-ID")); }
            l_total.Text = string.Format("{0:#,###}" + ",00", tot_bel);
            //l_total.Text = total;
        }
        //=============================================================================================================================
        private void t_edc1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_edc1.Text == "")
                { t_edc2.Text = ""; }
                else
                {
                    edc1 = double.Parse(t_edc1.Text);
                    t_edc1.Text = string.Format("{0:#,###}", edc1);
                    t_edc1.Select(t_edc1.Text.Length, 0);
                    String split = "from_Tedc1";
                    auto_total(edc1, tot_bel, split);
                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                t_edc1.Text = "";
            }
           
        }
        //=============================================================================================================================
        //===========================================METHOD FOR GET NILAI AUTO SPLIT===================================================
        public void auto_total(double awal, double totalk, string split)
        {
            double akhir;
            if (split == "from_Tedc1")
            {
                if (edc1 == totalk)
                {
                    t_edc2.Text = "0";
                }
                else
                {
                    akhir = totalk - awal;
                    edc2 = akhir;
                    t_edc2.Text = string.Format("{0:#,###}", akhir);
                    t_edc2.Select(t_edc2.Text.Length, 0);
                }
            }
            else
            {
                if (edc2 == totalk)
                {
                    t_edc1.Text = "0";
                }
                else
                {
                    akhir = totalk - awal;
                    edc1 = akhir;
                    t_edc1.Text = string.Format("{0:#,###}", akhir);
                    t_edc1.Select(t_edc1.Text.Length, 0);
                }
            }
        }
        //=============================================================================================================================
        //=============================================================================================================================
        private void w_split_edc_KeyDown(object sender, KeyEventArgs e)
        {
            //=================BUTTON CHARGE===========
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_ok2_Click(null, null);
            }
            //===============BUTTON OK===================
            if (e.Control && e.KeyCode.ToString() == "B")
            {
                b_cancel2_Click(null, null);
            }
        }
        //=============================================================================================================================
        //=========================================TEXTBOX EDC DIMASUKAN NILAI========================================================
        private void t_edc2_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (t_edc2.Text == "")
                { t_edc1.Text = ""; }
                else
                {
                    edc2 = double.Parse(t_edc2.Text);
                    t_edc2.Text = string.Format("{0:#,###}", edc2);
                    t_edc2.Select(t_edc2.Text.Length, 0);
                    String split = "from_Tedc2";
                    auto_total(edc2, tot_bel, split);
                }
            }
            catch
            {
                MessageBox.Show("Please Input Number");
                t_edc2.Text = "";
            }
            
        }
        //=============================================================================================================================
        //===========================FOR PRINT STRUCK===========================
        //======================================================================
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
