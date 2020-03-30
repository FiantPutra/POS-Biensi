using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.Serialization.Json;



namespace try_bi
{
    public partial class W_Voucher : Form
    {
        public static Form1 f1;
        koneksi ckon = new koneksi();
        String voucher_code;
        public string id_transaksi2, store_code2;
        public W_Voucher()
        {
            InitializeComponent();
        }
        //====GET VOUCHER FROM API, COONECT TO INTERNET======
        public async Task GetVoucher()
        {
            String response = "";
            var credentials = new NetworkCredential("username", "password");
            var handler = new HttpClientHandler { Credentials = credentials };
            using (var client = new HttpClient(handler))
            {
                // Make your request...
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    //HttpResponseMessage message = client.GetAsync("http://retailbiensi.azurewebsites.net/api/Voucher?VoucherCode="+ voucher_code+"&storeCode="+store_code2).Result;
                    HttpResponseMessage message = client.GetAsync("http://mpos.biensicore.co.id/api/Voucher?VoucherCode=" + voucher_code + "&storeCode=" + store_code2).Result;
                    if (message.IsSuccessStatusCode)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(Voucher));
                        var result = message.Content.ReadAsStringAsync().Result;
                        byte[] byteArray = Encoding.UTF8.GetBytes(result);
                        MemoryStream stream = new MemoryStream(byteArray);
                        Voucher resultData = serializer.ReadObject(stream) as Voucher;
                        //==masukan daat ke dalam variable
                        String code = resultData.VoucherCode;
                        String desc = resultData.Description;
                        int value = resultData.Value;
                        int id = resultData.Id;
                        //==VALIDASI JIKA VOUCHER TIDAK ADA
                        if(value==0)
                        {
                            
                            MessageBox.Show("No Voucher Available");
                            this.Close();
                        }
                        else
                        {
                            //===buka form untuk menggunakan voucher
                            W_Vou_Confirm confirm = new W_Vou_Confirm();
                            confirm.id_transaksi3 = id_transaksi2; //digunakan untuk membawa id transaksi
                            confirm.get_voucher_valid(code, desc, value, id);//memberikan data ke form penggunaan voucher
                            confirm.ShowDialog();//buka form nya
                            //====tutup form ini
                            this.Close();
                        }

                    }
                    else
                    {
                        response = "Fail";
                    }

                }
                catch (Exception ex)
                {
                    response = ex.ToString();
                }
            }
        }
        //=====================================================================
        //button ok
        private void b_ok2_Click(object sender, EventArgs e)
        {
            voucher_code = t_voucher.Text;
            //MessageBox.Show(voucher_code + "");
            GetVoucher().Wait();
            //cek_voucher();
        }
        //form ke load
        private void W_Voucher_Load(object sender, EventArgs e)
        {
            this.ActiveControl = t_voucher;
            t_voucher.Focus();

        }
        //button cancel

        private void b_cancel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //shorcut di menu voucher
        private void W_Voucher_KeyDown(object sender, KeyEventArgs e)
        {
            //===============BUTTON CHARGE===================
            if (e.Control && e.KeyCode.ToString() == "C")
            {
                b_ok2_Click(null, null);
            }
            //===============BUTTON BACK=====================
            if (e.Control && e.KeyCode.ToString() == "B")
            {
                b_cancel2_Click(null, null);
            }
        }
    }
}
